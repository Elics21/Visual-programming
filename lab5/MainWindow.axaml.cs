using Avalonia.Controls;
using Avalonia.Input;

namespace lab5_vis
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
        private void MyListBoxOnTapped(object? sender, TappedEventArgs e)
        {
            if (e.Source is Control { DataContext: IFileSystemExplorer fileExplorer })
            {
                if (this.DataContext is Explorer explorer)
                {
                    explorer.LoadImage(fileExplorer.Path);
                }
            }
        }
    }
}