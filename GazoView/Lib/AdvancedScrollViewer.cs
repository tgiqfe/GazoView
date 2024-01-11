using GazoView.Conf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GazoView.Lib
{
    internal class AdvancedScrollViewer : ScrollViewer
    {
        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
        {
            return null;
        }

        /// <summary>
        /// 画像部分、背景部分の両方でドラッグ移動できるようにする
        /// ※静的オブジェクト経由でメインウィンドウのイベントハンドルを呼び出し
        /// </summary>
        public AdvancedScrollViewer()
        {
            EventManager.RegisterClassHandler(
                typeof(AdvancedScrollViewer),
                FrameworkElement.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler((sender, e) =>
                {
                    if (!Item.BindingParam.State.MouseMoveTrimming)
                    {
                        Item.Mainbase.DragMove();
                    }
                }));
        }
    }
}
