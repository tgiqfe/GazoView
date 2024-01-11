using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GazoView.Conf;

namespace GazoView.Lib
{
    class AdvancedScrollViewer : ScrollViewer
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
                new MouseButtonEventHandler((sender, e) => Item.Mainbase.DragMove()));
        }
    }
}
