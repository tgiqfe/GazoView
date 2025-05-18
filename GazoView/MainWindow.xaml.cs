using GazoView.Lib;
using GazoView.Lib.Conf;
using GazoView.Lib.Functions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GazoView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = Item.BindingParam;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
            }

        }

        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (SpecialKeyStatus.IsCtrPressed())
            {
                ZoomImage(e);
            }
            else
            {

            }
        }



        private void ZoomImage(MouseWheelEventArgs e = null)
        {
            if (e == null)
            {
                Item.ScaleRate.Index = ScaleRate.DEF_INDEX;
            }
            else
            {
                int direction = e.Delta > 0 ? 1 : -1;
                Item.ScaleRate.Index += direction;
            }

            var scale = Item.ScaleRate.Scale;

            if (scale == 1)
            {
                MainImage.SetBinding(Image.WidthProperty, new Binding("ActualWidth") { Source = ScrollViewer });
                MainImage.SetBinding(Image.HeightProperty, new Binding("ActualHeight") { Source = ScrollViewer });
            }
            else if (scale > 1)
            {
                BindingOperations.ClearBinding(MainImage, Image.WidthProperty);
                BindingOperations.ClearBinding(MainImage, Image.HeightProperty);

                //  Before scaling, get pointer location.
                Point mousePoint = e.GetPosition(ScrollViewer);
                double viewX = ScrollViewer.HorizontalOffset;
                double viewY = ScrollViewer.VerticalOffset;

                //  Set the new size of the ScrollViewer. and move mouse pointer.
                MainImage.Width = this.ActualWidth * scale;
                MainImage.Height = (this.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;

                var relateScale = scale / Item.ScaleRate.PreviewScale;
                ScrollViewer.ScrollToHorizontalOffset((mousePoint.X + viewX) * relateScale - mousePoint.X);
                ScrollViewer.ScrollToVerticalOffset((mousePoint.Y + viewY) * relateScale - mousePoint.Y);
            }
            else if (scale < 1)
            {
                MainImage.Width = this.ActualWidth * scale;
                MainImage.Height = (this.ActualHeight - SystemParameters.WindowCaptionHeight) * scale;
            }
        }


    }
}