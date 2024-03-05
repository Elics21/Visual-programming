using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Windows.Input;
using Avalonia.Interactivity;
using Avalonia.Controls;
using System;
using Avalonia.Input;
using Avalonia;

namespace lab4
{
    public class Explorer : INotifyPropertyChanged
    {
        private string _text;
        private string _currentDirectory;
        private string[] _files;
        private string[] _directorys;

        private string _selectedDirectory;
        public string SelectedDirectory
        {
            get { return _selectedDirectory; }
            set
            {
               if (_selectedDirectory != value) 
               { 
                    _selectedDirectory = value;
                    ClickDirectory(value);
                    OnPropertyChanged();
               }
            }
        }


        public string Text {
            get {
                return _text; 
            } 
            set {
                _ = SetField(ref _text, value); 
            } 
        }
        public string[] Files
        {
            get { return _files; }
            set { _ = SetField(ref _files, value); }
        }
        public string[] Directorys
        {
            get { return _directorys; }
            set { _ = SetField(ref _directorys, value); }
        }

        public Explorer() {
            _currentDirectory = System.IO.Directory.GetCurrentDirectory();
            Text = _currentDirectory;
            string[] directoriesPaths = Directory.GetDirectories(_currentDirectory);
            string[] filePaths = Directory.GetFiles(_currentDirectory);
            Directorys = directoriesPaths.Select(filePath => Path.GetFileName(filePath)).ToArray();
            Files = filePaths.Select(filePath => Path.GetFileName(filePath)).ToArray();

        }

        public void ClickBack()
        {
            DirectoryInfo parentDirectoryInfo = Directory.GetParent(_currentDirectory);
            if (parentDirectoryInfo != null)
            {
                string parentDirectory = parentDirectoryInfo.FullName;
                _currentDirectory = parentDirectory;
                Text = _currentDirectory;
                Directorys = Directory.GetDirectories(_currentDirectory).Select(filePath => Path.GetFileName(filePath)).ToArray();
                Files = Directory.GetFiles(_currentDirectory).Select(filePath => Path.GetFileName(filePath)).ToArray();
            }
        }


        public void ClickDirectory(string path)
        {
            _currentDirectory = _currentDirectory + "\\" + path;
            Text = _currentDirectory;
            Directorys = Directory.GetDirectories(_currentDirectory).Select(filePath => Path.GetFileName(filePath)).ToArray();
            Files = Directory.GetFiles(_currentDirectory).Select(filePath => Path.GetFileName(filePath)).ToArray();
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