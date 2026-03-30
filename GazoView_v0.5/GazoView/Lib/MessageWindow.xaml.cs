using GazoView.Lib.Conf;
using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GazoView.Lib
{
    /// <summary>
    /// MessageWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MessageWindow : Window
    {
        public enum MessageType
        {
            None,
            Delete,
            Restore,
        }

        public MessageType Type { get; set; }
        public bool IsClosed { get; set; }
        private static readonly Regex _pattern_StarFile = new Regex(@"★\.[^\.]+$");

        public MessageWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Message window for Delete
        /// </summary>
        /// <param name="images"></param>
        public MessageWindow(Images images)
        {
            InitializeComponent();

            MsgWindowImage.Source = images.Current.Source;
            MsgWindowText_FilePath.Content = images.Current.FilePath;
            MsgWindowText_FileName.Content = images.Current.FileName;
            MsgWindowText_FileExtension.Content = images.Current.FileExtension;
            MsgWindowText_ImageSize.Content = $"{images.Current.Width} x {images.Current.Height}";
            MsgWindowText_FileSize.Content = images.Current.Size;
            MsgWindowText_TimeStamp.Content = images.Current.LastWriteTime;
            StarIcon.Visibility = images.Current.IsStar ? Visibility.Visible : Visibility.Hidden;

            Button_OK.Focus();
        }

        /// <summary>
        /// Message window for Restore
        /// </summary>
        /// <param name="restoreSrcPath"></param>
        /// <param name="restoredDstPath"></param>
        public MessageWindow(string restoreSrcPath, string restoredDstPath)
        {
            InitializeComponent();

            ImageItem item = new(restoreSrcPath);
            MsgWindowImage.Source = item.Source;

            MsgWindowText_FilePath.Content = restoredDstPath;
            MsgWindowText_FileName.Content = System.IO.Path.GetFileName(restoredDstPath);
            MsgWindowText_FileExtension.Content = System.IO.Path.GetExtension(restoredDstPath);
            MsgWindowText_ImageSize.Content = $"{item.Width} x {item.Height}";
            MsgWindowText_FileSize.Content = item.Size;
            MsgWindowText_TimeStamp.Content = item.LastWriteTime;
            StarIcon.Visibility = _pattern_StarFile.IsMatch(System.IO.Path.GetFileName(restoredDstPath)) ?
                Visibility.Visible : Visibility.Hidden;

            Button_OK.Focus();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.IsClosed = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Left:
                    Button_Cancel.Focus();
                    break;
                case Key.Right:
                    Button_OK.Focus();
                    break;
            }
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if(this.Type == MessageType.Delete)
            {
                FileFunction.DeleteImageFile(Item.BindingParam.Images);
            }
            else if (this.Type == MessageType.Restore)
            {
                FileFunction.RestoreImageFile(Item.BindingParam.Images);
            }
            this.Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
