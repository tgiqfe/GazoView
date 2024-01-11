﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GazoView.Conf;

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
                case Key.Escape:
                    KeyEvent_PressEscape(); break;
                case Key.Delete:
                    KeyEvent_PressDelete(); break;
                case Key.R:
                    KeyEvent_PressR(); break;
                case Key.T:
                    KeyEvent_PressT(); break;
                case Key.Tab:
                case Key.D:
                    KeyEvent_PressTab(); break;
                case Key.N:
                    KeyEvent_PressN();break;
                case Key.Left:
                    KeyEvent_PressLeft(); break;
                case Key.Right:
                    KeyEvent_PressRight(); break;
            }
        }

        /// <summary>
        /// キー押下時イベント: Esc
        /// アプリケーション終了
        /// </summary>
        private void KeyEvent_PressEscape()
        {
            Application.Current.Shutdown();
        }

        private void KeyEvent_PressDelete()
        {
        }


        /// <summary>
        /// キー押下時イベント: R
        /// 拡縮モード切り替え
        /// </summary>
        private void KeyEvent_PressR()
        {
            SwitchScalingMode();
        }

        /// <summary>
        /// キー押下時イベント: T
        /// トリミングモード切り替え
        /// </summary>
        private void KeyEvent_PressT()
        {
            SwitchTrimmingMode();
        }

        /// <summary>
        /// キー押下時イベント: Tab or D
        /// Infoパネル表示切り替え
        /// </summary>
        private void KeyEvent_PressTab()
        {
            SwitchShowInfoPanel();
        }

        /// <summary>
        /// Bitmapスケーリングモードを NearestNeighbor⇔Fnat で切り替える
        /// </summary>
        private void KeyEvent_PressN()
        {
            BitmapScalingMode();
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