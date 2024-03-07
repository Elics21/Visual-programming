using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;


namespace lab4
{
    public interface IFileSystemExplorer
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
    }
    public class ParentFolder : IFileSystemExplorer
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public ParentFolder()
        {
            Name = string.Empty;
            Path = string.Empty;
        }
        public ParentFolder(string path, string name)
        {
            Name = name;
            Path = path;
        }
    }
    public class Folder : IFileSystemExplorer
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public Folder()
        {
            Name = string.Empty;
            Path = string.Empty;
        }
        public Folder(string path, string name)
        {
            Name = name;
            Path = path;
        }
    }
    public class File : IFileSystemExplorer
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public File()
        {
            Name = string.Empty;
            Path = string.Empty;
        }
        public File(string path, string name)
        {
            Name = name;
            Path = path;
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
            _currentDirectory = "C:\\test";
            LoadItems();
        }

        private void LoadItems()
        {
            Items.Clear();
            FileSystemInfo[] fileSystemInfos = new DirectoryInfo(_currentDirectory).GetFileSystemInfos();
            string parentDirectory = Path.GetDirectoryName(_currentDirectory);
            if(parentDirectory != null)
            {
                ParentFolder parentFolder = new ParentFolder(parentDirectory, "...");
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
            /*foreach (var driveInfo in DriveInfo.GetDrives())
            {
                if ((driveInfo.RootDirectory.FullName == _currentDirectory) && (parentDirectory == null))
                {
                    items.Add(new DirectoryInfo(driveInfo.Name));
                }
            }*/
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