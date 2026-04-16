using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;

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
                case Key.Up:
                    if (TextBoxForFileName.IsFocused)
                    {
                        UpDownNumber(1);
                        e.Handled = true;
                    }
                    break;
                case Key.Down:
                    if (TextBoxForFileName.IsFocused)
                    {
                        UpDownNumber(-1);
                        e.Handled = true;
                    }
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

        #region up,down key for number selection in file name

        /// <summary>
        /// Updown number in textbox.
        /// </summary>
        /// <param name="offset"></param>
        private void UpDownNumber(int offset)
        {
            if (TextBoxForFileName.SelectionLength == 0)
            {
                var caretIndex = TextBoxForFileName.CaretIndex;
                var text = TextBoxForFileName.Text;
                var numberString = GetNumberStringAtPosition(text, caretIndex);

                if (!string.IsNullOrEmpty(numberString))
                {
                    var startIndex = text.IndexOf(numberString, Math.Max(0, caretIndex - numberString.Length));
                    if (startIndex >= 0)
                    {
                        TextBoxForFileName.Select(startIndex, numberString.Length);
                    }
                }
            }
            string selectedText = TextBoxForFileName.SelectedText;
            int newNumber = int.Parse(selectedText) + offset;
            string newSelectedText = newNumber.ToString().PadLeft(selectedText.Length, '0');
            string newText = TextBoxForFileName.Text.
                Remove(TextBoxForFileName.SelectionStart, TextBoxForFileName.SelectionLength).
                Insert(TextBoxForFileName.SelectionStart, newSelectedText);
            int newSelectionStart = TextBoxForFileName.SelectionStart;
            TextBoxForFileName.Text = newText;
            TextBoxForFileName.Select(newSelectionStart, selectedText.Length);
        }

        /// <summary>
        /// Get number string at the specified position in the text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private string GetNumberStringAtPosition(string text, int position)
        {
            if (string.IsNullOrEmpty(text) || position < 0 || position > text.Length) return string.Empty;

            int currentPos = position == text.Length ? position - 1 : position;
            if (currentPos < 0 || currentPos >= text.Length || !char.IsDigit(text[currentPos])) return string.Empty;

            int startPos = currentPos;
            while (startPos > 0 && char.IsDigit(text[startPos - 1]))
            {
                startPos--;
            }

            int endPos = currentPos;
            while (endPos < text.Length - 1 && char.IsDigit(text[endPos + 1]))
            {
                endPos++;
            }

            return text.Substring(startPos, endPos - startPos + 1);
        }

        #endregion
    }
}
