using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GazoView.Lib.Panel
{
    /// <summary>
    /// DeleteMessageWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DeleteMessageWindow : Window
    {
        public DeleteMessageWindow()
        {
            InitializeComponent();
        }

        public DeleteMessageWindow(
            string action,
            string filePath,
            string fileName,
            string fileExtension,
            string imageSize,
            string fileSize,
            string timeStamp,
            ImageSource imageSource)
        {
            InitializeComponent();
            TextBlockAction.Text = action;
            TextBlockFilePath.Text = filePath;
            TextBlockFileName.Text = fileName;
            TextBlockFileExtension.Text = fileExtension;
            TextBlockImageSize.Text = imageSize;
            TextBlockFileSize.Text = fileSize;
            TextBlockTimeStamp.Text = timeStamp;
            TargetImage.Source = imageSource;
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button eventObject = e.Source as Button;
            if (eventObject == null)
            {
                this.DragMove();
            }
        }
    }
}
