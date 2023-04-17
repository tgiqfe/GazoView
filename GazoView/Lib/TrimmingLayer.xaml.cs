using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GazoView.Lib
{
    /// <summary>
    /// TrimmingLayer.xaml の相互作用ロジック
    /// </summary>
    public partial class TrimmingLayer : UserControl
    {
        enum DragLine
        {
            None,
            Left,
            Top,
            Right,
            Bottom,
        }

        private DragLine _dragLine = DragLine.None;

        public TrimmingLayer()
        {
            InitializeComponent();

            Item.Layer = this;
            this.DataContext = Item.BindingParam;
        }

        /// <summary>
        /// トリミングのエリアの境界線をマウスDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            _dragLine = (sender as Line).Name switch
            {
                "LineTop" => DragLine.Top,
                "LineBottom" => DragLine.Bottom,
                "LineLeft" => DragLine.Left,
                "LineRight" => DragLine.Right,
                _ => DragLine.None
            };
        }

        /// <summary>
        /// コントロール上でマウスDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Layer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(Layer);
            if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down)
            {
                if (point.Y < GrayAreaTop.Height)
                {
                    _dragLine = DragLine.Top;
                }
                else if (point.Y > (Layer.ActualHeight - GrayAreaBottom.Height))
                {
                    _dragLine = DragLine.Bottom;
                }
                else
                {
                    var centerTop = point.Y - GrayAreaTop.Height;
                    var centerBottom = Layer.ActualHeight - GrayAreaBottom.Height - point.Y;
                    _dragLine = centerTop < centerBottom ?
                        DragLine.Top : DragLine.Bottom;
                }
            }
            else
            {
                if (point.X < GrayAreaLeft.Width)
                {
                    _dragLine = DragLine.Left;
                }
                else if (point.X > (Layer.ActualWidth - GrayAreaRight.Width))
                {
                    _dragLine = DragLine.Right;
                }
                else
                {
                    var centerLeft = point.X - GrayAreaLeft.Width;
                    var centerRight = Layer.ActualWidth - GrayAreaRight.Width - point.X;
                    _dragLine = centerLeft < centerRight ?
                        DragLine.Left : DragLine.Right;
                }
            }
        }

        private void Layer_MouseMove(object sender, MouseEventArgs e)
        {
            Item.BindingParam.MouseMovingInTrimming = true;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var point = e.GetPosition(Layer);
                switch (_dragLine)
                {
                    case DragLine.Top:
                        var newTop = point.Y;
                        Item.BindingParam.Setting.Trimming.Top = (int)newTop;
                        break;
                    case DragLine.Bottom:
                        var newBottom = point.Y;
                        Item.BindingParam.Setting.Trimming.Bottom = (int)newBottom;
                        break;
                    case DragLine.Left:
                        var newLeft = point.X;
                        Item.BindingParam.Setting.Trimming.Left = (int)newLeft;
                        break;
                    case DragLine.Right:
                        var newRight = point.X;
                        Item.BindingParam.Setting.Trimming.Right = (int)newRight;
                        break;
                }
            }
            else
            {
                _dragLine = DragLine.None;
            }

            Item.BindingParam.MouseMovingInTrimming = false;
        }
    }
}
