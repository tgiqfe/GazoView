using GazoView.Conf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GazoView.Lib;
using System.Windows.Controls;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
                case Key.T:
                    Item.BindingParam.State.TrimmingMode = !Item.BindingParam.State.TrimmingMode;
                    break;
                case Key.D:
                case Key.Tab:
                    ChangeInfoPanel();
                    break;
                case Key.Left:
                    //  1つ前の画像を表示
                    ChangeImage(-1);
                    break;
                case Key.Right:
                    //  1つ後の画像を表示
                    ChangeImage(1);
                    break;
            }
        }
    }
}
