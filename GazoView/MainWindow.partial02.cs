using GazoView.Conf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 画像を変更
        /// direction ⇒ -1:前の画像, 1:次の画像
        /// </summary>
        /// <param name="direction"></param>
        private void ChangeImage(int direction)
        {
            Item.BindingParam.Images.Index += direction;
        }
    }
}
