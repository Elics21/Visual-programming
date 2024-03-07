using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace lab4
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
            IconPath = "avares://lab4/icons/parent.png";
        }
        public ParentFolder(string path, string name)
        {
            Name = name;
            Path = path;
            IconPath = "avares://lab4/icons/parent.png";
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
            IconPath = "avares://lab4/icons/folder.png";
        }
        public Folder(string path, string name)
        {
            Name = name;
            Path = path;
            IconPath = "avares://lab4/icons/folder.png";
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
            IconPath = "avares://lab4/icons/file.png";
        }
        public File(string path, string name)
        {
            Name = name;
            Path = path;
            IconPath = "avares://lab4/icons/file.png";
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
            IconPath = "avares://lab4/icons/driver.png";
        }
        public Disk(string path, string name)
        {
            Name = name;
            Path = path;
            IconPath = "avares://lab4/icons/driver.png";
        }
    }
    public class Explorer : INotifyPropertyChanged
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

        public Explorer() {
            _currentDirectory = "C:\\";
            LoadItems();
        }

        private void LoadItems()
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
                FileSystemInfo[] fileSystemInfos = new DirectoryInfo(_currentDirectory).GetFileSystemInfos();
                string parentDirectory = Path.GetDirectoryName(_currentDirectory);
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
                foreach (var info in fileSystemInfos)
                {
                    if (info is FileInfo fileInfo)
                    {
                        File file = new File(fileInfo.DirectoryName, fileInfo.Name);
                        _items.Add(file);
                    }
                    else if (info is DirectoryInfo directoryInfo)
                    {
                        Folder folder = new Folder(directoryInfo.FullName, directoryInfo.Name);
                        _items.Add(folder);
                    }
                }
            }
            //Text = new DirectoryInfo(_currentDirectory);

        }
        public void ClickDirectory(string path)
        {
            _currentDirectory = path;
            LoadItems();
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