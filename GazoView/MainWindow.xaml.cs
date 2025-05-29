using GazoView.Lib;
using GazoView.Lib.Functions;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

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
                    if (!Item.BindingParam.State.TrimmingMode) Item.MainBase.DragMove();
                }));
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    //  Application shutdown
                    Application.Current.Shutdown();
                    break;
                case Key.Left:
                case Key.BrowserBack:
                    //  Move prev image.
                    if (Item.BindingParam.State.IsGifAnimationView) return;
                    ImageFunction.ChangeImage(-1);
                    break;
                case Key.Right:
                case Key.BrowserForward:
                    //  Move next image.
                    if (Item.BindingParam.State.IsGifAnimationView) return;
                    ImageFunction.ChangeImage(1);
                    break;
                case Key.Home:
                    //  Move first image from same directory.
                    if (Item.BindingParam.State.IsGifAnimationView) return;
                    ImageFunction.ChangeImage(Item.BindingParam.Images.Length);
                    break;
                case Key.End:
                    //  Move last image from same directory.
                    if (Item.BindingParam.State.IsGifAnimationView) return;
                    ImageFunction.ChangeImage(-1 * Item.BindingParam.Images.Length);
                    break;
                case Key.R:
                    //  Rest size fit to window.
                    ImageFunction.ZoomImage(this, MainImage, ScrollViewer);
                    break;
                case Key.C:
                    //  Copy image file.
                    //  Ctrl+C -> image data copy (image)
                    //  Ctrl+Shift+C -> image path copy (string)
                    if (SpecialKeyStatus.IsCtrPressed())
                    {
                        FileFunction.CopyImageFile(Item.BindingParam.Images, SpecialKeyStatus.IsShiftPressed());
                    }
                    break;
                case Key.T:
                    //  Switch trimming mode.
                    ImageFunction.SwitchTrimmingMode();
                    break;
                case Key.G:
                    //  Start image trimming.
                    ImageFunction.StartTrimming();
                    break;
                case Key.O:
                    //  Open or close folder path
                    //  O -> Open folder
                    //  Ctrl+O -> Close folder
                    if (SpecialKeyStatus.IsCtrPressed())
                    {
                        FolderWindow.Close(Item.BindingParam.Images.Current.Parent);
                    }
                    else
                    {
                        FolderWindow.Open(Item.BindingParam.Images.Current.Parent, Item.BindingParam.Images.Current.FileName);
                    }
                    break;
                case Key.F5:
                    //  Reload file from same directory.
                    Item.BindingParam.Images.ReloadFiles();
                    break;
                case Key.Delete:
                    //  Delete image file.
                    FileFunction.DeleteImageFile(Item.BindingParam.Images);
                    break;
                case Key.Z:
                    //  Restore image file (deleted file only).
                    if (SpecialKeyStatus.IsCtrPressed())
                    {
                        FileFunction.RestoreImageFile(Item.BindingParam.Images);
                    }
                    break;
                case Key.U:
                    if (Item.BindingParam.Images.Current.FileExtension == ".gif")
                    {
                        if (Item.BindingParam.State.IsGifAnimationView)
                        {
                            Item.BindingParam.State.IsGifAnimationView = false;
                        }
                        else
                        {
                            Item.BindingParam.State.IsGifAnimationView = true;
                        }
                    }
                    break;
                case Key.OemBackslash:
                    //  Start on/off
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

            if (Item.BindingParam.State.IsGifAnimationView) { return; }
            if (SpecialKeyStatus.IsCtrPressed())
            {
                ImageFunction.ZoomImage(this, MainImage, ScrollViewer, e);
            }
            else
            {
                ImageFunction.ChangeImage(e.Delta > 0 ? -1 : 1);
            }
        }

        private void MainImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var scale = e.NewSize.Width / Item.BindingParam.Images.Current.Source.Width;
            /*
            Item.BindingParam.Trimming.Scale = e.NewSize.Width / Item.BindingParam.Images.Current.Source.Width;
            */
            ImageFunction.SwitchNearestNeighbor(scale >= 3);
        }

        #region Vile drag and drop

        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
            if (e.Data.GetData(DataFormats.FileDrop) is string[] targets)
            {
                if (File.Exists(targets[0]) || Directory.Exists(targets[0]))
                {
                    e.Effects = DragDropEffects.Copy;
                    MainImage.Opacity = 0.6;
                    this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3395FF"));
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Window_PreviewDragLeave(object sender, DragEventArgs e)
        {
            MainImage.Opacity = 1;
            this.Background = Brushes.DimGray;
        }

        private void Window_PreviewDrop(object sender, DragEventArgs e)
        {
            MainImage.Opacity = 1;
            this.Background = Brushes.DimGray;
            if (e.Data.GetData(DataFormats.FileDrop) is string[] targets)
            {
                Item.BindingParam.Images.LoadFiles(targets);
            }
        }

        #endregion
    }
}