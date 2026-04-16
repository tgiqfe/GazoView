using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using MaterialDesignThemes.Wpf;

namespace GazoView.Lib.Panel
{
    /// <summary>
    /// RenameBoxWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class RenameBoxWindow : Window
    {
        private bool _isLoaded = false;
        private bool _renamable = false;
        private string _defaultName = string.Empty;

        public RenameBoxWindow()
        {
            InitializeComponent();
            this.DataContext = Item.BindingParam;
        }

        private void RenameBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxForFileName.Text = Path.GetFileNameWithoutExtension(Item.BindingParam.Images.Current.FileName);
            TextBlockForExtension.Text = Item.BindingParam.Images.Current.FileExtension;

            TextBoxForFileName.Focus();
            TextBoxForFileName.Select(TextBoxForFileName.Text.Length, 0);

            _isLoaded = true;
            _defaultName = TextBoxForFileName.Text;
        }

        private void RenameWindowBase_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!TextBoxForFileName.IsFocused)
            {
                TextBoxForFileName.Focus();
                TextBoxForFileName.SelectAll();
            }
        }

        /// <summary>
        /// Key down event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
                case Key.Enter:
                    RenameImageFile();
                    break;
            }
        }

        private void ButtonRenameStart_Click(object sender, RoutedEventArgs e)
        {
            RenameImageFile();
        }

        private void RenameImageFile()
        {
            if (_renamable)
            {
                Item.BindingParam.Images.RenameImageFile($"{TextBoxForFileName.Text}{TextBlockForExtension.Text}");
                Item.BindingParam.RenameBox.HideWindow();
            }   
        }

        /// <summary>
        /// Text changed event for the file name text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxForFileName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!_isLoaded) return;
            if (_defaultName == TextBoxForFileName.Text)
            {
                PrecheckStatusIcon.Kind = PackIconKind.PencilCircle;
                PrecheckStatusIcon.Foreground = Brushes.Gray;
                _renamable = false;
                return;
            }

            var newPath = Path.Combine(
                Item.BindingParam.Images.Current.Parent,
                TextBoxForFileName.Text + Item.BindingParam.Images.Current.FileExtension);

            if (File.Exists(newPath) && !_defaultName.Equals(TextBoxForFileName.Text, StringComparison.OrdinalIgnoreCase))
            {
                PrecheckStatusIcon.Kind = PackIconKind.CloseCircle;
                PrecheckStatusIcon.Foreground = Brushes.Red;
                _renamable = false;
            }
            else
            {
                PrecheckStatusIcon.Kind = PackIconKind.CheckCircle;
                PrecheckStatusIcon.Foreground = Brushes.Green;
                _renamable = true;
            }
        }
    }
}
