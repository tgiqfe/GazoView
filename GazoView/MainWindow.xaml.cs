using GazoView.Lib;
using GazoView.Lib.Functions;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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

            Item.MainBase = this;
            this.DataContext = Item.BindingParam;

            //  Add event (for drag move).
            EventManager.RegisterClassHandler(
                typeof(AdvancedScrollViewer),
                FrameworkElement.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler((sender, e) =>
                {
                    this.DragMove();
                }));
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    System.Windows.Application.Current.Shutdown();
                    break;
                case Key.Left:
                case Key.BrowserBack:
                    ImageFunction.ChangeImage(-1);
                    break;
                case Key.Right:
                case Key.BrowserForward:
                    ImageFunction.ChangeImage(1);
                    break;
                case Key.Home:
                    ImageFunction.ChangeImage(Item.BindingParam.Images.Length);
                    break;
                case Key.End:
                    ImageFunction.ChangeImage(-1 * Item.BindingParam.Images.Length);
                    break;
                case Key.R:
                    ImageFunction.ZoomImage(this, MainImage, ScrollViewer);
                    break;
                case Key.C:
                    if (SpecialKeyStatus.IsCtrPressed())
                    {
                        FileFunction.CopyImageFile(Item.BindingParam.Images, SpecialKeyStatus.IsShiftPressed());
                    }
                    break;
                case Key.O:
                    if (SpecialKeyStatus.IsCtrPressed())
                    {
                        FolderWindow.Close(Item.BindingParam.Images.Current.Parent);
                    }
                    else
                    {
                        FolderWindow.Open(Item.BindingParam.Images.Current.Parent, Item.BindingParam.Images.Current.FileName);
                    }
                    break;
                case Key.Delete:
                    FileFunction.DeleteImageFile(Item.BindingParam.Images);
                    break;
                case Key.Z:
                    if (SpecialKeyStatus.IsCtrPressed())
                    {
                        FileFunction.RestoreImageFile(Item.BindingParam.Images);
                    }
                    break;
                case Key.OemBackslash:
                    FileFunction.ToggleStarFile(Item.BindingParam.Images);
                    break;
                case Key.OemOpenBrackets:
                    //  Flip vertical
                    ImageFunction.ImageFlip(Item.BindingParam.Images, false);
                    break;
                case Key.OemCloseBrackets:
                    //  Flip horizontal
                    ImageFunction.ImageFlip(Item.BindingParam.Images, true);
                    break;
                case Key.Oem1:
                    //  Rotate right ([colon]key for Japanese keyboard)
                    ImageFunction.ImageRotate(Item.BindingParam.Images);
                    break;
            }
        }

        /// <summary>
        /// Wheel event
        ///     ctrl + wheel -> zoom in/out.
        ///     wheel -> next/prev image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (SpecialKeyStatus.IsCtrPressed())
            {
                ImageFunction.ZoomImage(this, MainImage, ScrollViewer, e);
            }
            else
            {
                ImageFunction.ChangeImage(e.Delta > 0 ? -1 : 1);
            }
        }
    }
}