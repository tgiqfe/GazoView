using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// キーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    KeyEvent_PressLeft();
                    break;
                case Key.Right:
                    KeyEvent_PressRight();
                    break;
            }
        }

        /// <summary>
        /// キー押下時イベント: Left
        /// 1つ前の画像を表示
        /// </summary>
        private void KeyEvent_PressLeft()
        {
            Item.BindingParam.Images.Index--;
        }

        /// <summary>
        /// キー押下時イベント: Right
        /// 次の画像を表示
        /// </summary>
        public void KeyEvent_PressRight()
        {
            Item.BindingParam.Images.Index++;
        }   
    }
}
