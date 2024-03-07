using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace lab4
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MyListBoxOnDoubleTapped(object? sender, TappedEventArgs e)
        {
            if (e.Source is Control { DataContext: IFileSystemExplorer fileExplorer })
            {
                if (this.DataContext is Explorer explorer)
                {
                    explorer.ClickDirectory(fileExplorer.Path);
                }
            }
        }
    }
}