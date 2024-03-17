using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using Avalonia.Threading;
using System.Threading;

namespace lab5_vis
{
    public interface IFileSystemExplorer
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public Bitmap? IconBit { get => new Bitmap(AssetLoader.Open(new Uri(IconPath))); }
    }
    public class ParentFolder : IFileSystemExplorer
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public Bitmap? IconBit { get => new Bitmap(AssetLoader.Open(new Uri(IconPath))); }

        public ParentFolder()
        {
            Name = string.Empty;
            Path = string.Empty;
            IconPath = "avares://lab5-vis/icons/parent.png";
        }
        public ParentFolder(string path, string name)
        {
            Name = name;
            Path = path;
            IconPath = "avares://lab5-vis/icons/parent.png";
        }
    }
    public class Folder : IFileSystemExplorer
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public Bitmap? IconBit { get => new Bitmap(AssetLoader.Open(new Uri(IconPath))); }

        public Folder()
        {
            Name = string.Empty;
            Path = string.Empty;
            IconPath = "avares://lab5-vis/icons/folder.png";
        }
        public Folder(string path, string name)
        {
            Name = name;
            Path = path;
            IconPath = "avares://lab5-vis/icons/folder.png";
        }
    }
    public class File : IFileSystemExplorer
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public string IconPath { get; set; } 
        public Bitmap? IconBit { get => new Bitmap(AssetLoader.Open(new Uri(IconPath))); }

        public File()
        {
            Name = string.Empty;
            Path = string.Empty;
            IconPath = "avares://lab5-vis/icons/file.png";
        }
        public File(string path, string name)
        {
            Name = name;
            Path = path;
            IconPath = "avares://lab5-vis/icons/file.png";
        }
    }
    public class Disk : IFileSystemExplorer
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public Bitmap? IconBit { get => new Bitmap(AssetLoader.Open(new Uri(IconPath))); }

        public Disk()
        {
            Name = string.Empty;
            Path = string.Empty;
            IconPath = "avares://lab5-vis/icons/driver.png";
        }
        public Disk(string path, string name)
        {
            Name = name;
            Path = path;
            IconPath = "avares://lab5-vis/icons/driver.png";
        }
    }
    public class ImageExplorer : IFileSystemExplorer
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public Bitmap? IconBit { get => new Bitmap(AssetLoader.Open(new Uri(IconPath))); }
        public Bitmap ImageBit { get => new Bitmap(AssetLoader.Open(new Uri(Path))); }

        public ImageExplorer()
        {
            Name = string.Empty;
            Path = string.Empty;
            IconPath = "avares://lab5-vis/icons/file.png";
        }
        public ImageExplorer(string path, string name)
        {
            Name = name;
            Path = path;
            IconPath = "avares://lab5-vis/icons/file.png";
        }
    }


    public class  Explorer : INotifyPropertyChanged
    {
        public ObservableCollection<IFileSystemExplorer> Items { get { return _items; } set { OnPropertyChanged(); } }
        ObservableCollection<IFileSystemExplorer> _items = new ObservableCollection<IFileSystemExplorer>();
        private string _currentDirectory;
        public string CurrentDirectory
        {
            get { return _currentDirectory; }
            set
            {
                if (_currentDirectory != value)
                {
                    _currentDirectory = value;
                    OnPropertyChanged();
                    LoadItems();
                }
            }
        }

        private Bitmap? _selectedImageBit;
        public Bitmap? ImageBit
        {
            get { return _selectedImageBit; }
            set
            {
                if (_selectedImageBit != value)
                {
                    _selectedImageBit = value;
                    OnPropertyChanged();
                }
            }
        }

        private FileSystemWatcher _fileSystemWatcher;
        public Explorer()
        {
            _currentDirectory = "C:\\test";
            LoadItems();

            _fileSystemWatcher = new FileSystemWatcher(_currentDirectory);
            _fileSystemWatcher.IncludeSubdirectories = true; //����������� ��������� � ������������
            _fileSystemWatcher.Created += FileSystemWatcher_Changed; //����������� �������
            _fileSystemWatcher.Deleted += FileSystemWatcher_Changed; //����������� ��������
            _fileSystemWatcher.Renamed += FileSystemWatcher_Changed; //����������� ������������
            _fileSystemWatcher.EnableRaisingEvents = true; //��������� �������
        }

        private async void LoadItems()
        {
            Items.Clear();
            if(_currentDirectory == "\\")
            {
                foreach (var driveInfo in DriveInfo.GetDrives())
                {
                    Disk drive = new Disk(driveInfo.Name, driveInfo.Name);
                    _items.Add(drive);
                }
            }
            else
            {
                await LoadDirectoryContentsAsync(_currentDirectory);
            }

        }
        private async Task LoadDirectoryContentsAsync(string directory)
        {
            string parentDirectory = Path.GetDirectoryName(directory);
            if (parentDirectory != null)
            {
                ParentFolder parentFolder = new ParentFolder(parentDirectory, "...");
                _items.Add(parentFolder);
            }
            else
            {
                ParentFolder parentFolder = new ParentFolder("\\", "...");
                _items.Add(parentFolder);
            }

            await Task.Delay(1000); // �������� � 1 �������
            var fileSystemInfos = await Task.Run(() => new DirectoryInfo(directory).GetFileSystemInfos());

            foreach (var info in fileSystemInfos)
            {
                if (info is FileInfo fileInfo)
                {
                    if (IsImage(fileInfo.FullName))
                    {
                        ImageExplorer imageExplorer = new ImageExplorer(fileInfo.FullName, fileInfo.Name);
                        _items.Add(imageExplorer);
                    }
                    else
                    {
                        File file = new File(fileInfo.FullName, fileInfo.Name);
                        _items.Add(file);
                    }
                }
                else if (info is DirectoryInfo directoryInfo)
                {
                    Folder folder = new Folder(directoryInfo.FullName, directoryInfo.Name);
                    _items.Add(folder);
                }
            }
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //��������� ��������� �����
            Task.Run(() =>
            {
                //�������� � 0.3�
                Thread.Sleep(300);

                //������������� ��������
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    LoadItems();
                });
            });
        }

        public void LoadImage(string path)
        {
            if (IsImage(path))
            {
                ImageBit = new Bitmap(path);
            }

        }
        public void ClickDirectory(string path)
        {
            if (Path.GetExtension(path) == string.Empty)
            {
                _currentDirectory = path;
                LoadItems();
            }
            else if (IsImage(path))
            {
            }
        }

        public bool IsImage(string path)
        {
            string extension = Path.GetExtension(path);
            if (extension == ".png" || extension == ".jpg")
            {
                return true;
            }
            else { return false; }
        }

        //������������� ��������
        public void Dispose()
        {
            _fileSystemWatcher.Dispose();
        }





        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}