using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace lab2_visul
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetColor1(object sender, RoutedEventArgs args)
        {
            rectangleBackground.Background = Brushes.Khaki;
        }
        public void SetColor2(object sender, RoutedEventArgs args)
        {
            rectangleBackground.Background = Brushes.Red;
        }
        public void SetColor3(object sender, RoutedEventArgs args)
        {
            rectangleBackground.Background = Brushes.MediumVioletRed;
        }
        public void SetColor4(object sender, RoutedEventArgs args)
        {
            rectangleBackground.Background = Brushes.Bisque;
        }
        public void SetColor5(object sender, RoutedEventArgs args)
        {
            rectangleBackground.Background = Brushes.LemonChiffon;
        }
        public void SetColor6(object sender, RoutedEventArgs args)
        {
            rectangleBackground.Background = Brushes.PowderBlue;
        }
        public void SetColor7(object sender, RoutedEventArgs args)
        {
            rectangleBackground.Background = Brushes.MintCream;
        }
        public void SetColor8(object sender, RoutedEventArgs args)
        {
            rectangleBackground.Background = Brushes.Maroon;
        }
        public void SetColor9(object sender, RoutedEventArgs args)
        {
            rectangleBackground.Background = Brushes.RosyBrown;
        }
        public void SetColor10(object sender, RoutedEventArgs args)
        {
            rectangleBackground.Background = Brushes.LightPink;
        }
    }
}