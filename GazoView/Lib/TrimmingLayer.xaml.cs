using GazoView.Conf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public TrimmingLayer()
        {
            InitializeComponent();
            this.DataContext = Item.BindingParam;
        }

        //  前バージョンは必要とした処理だけど、今バージョンでは不要かもしれない。
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (Item.BindingParam?.State.TrimmingMode ?? false)
            {
                //  描画時の処理
                //  GreayAreaReload()
            }
        }

        enum DragLine
        {
            None,
            Top,
            Bottom,
            Left,
            Right
        }

        private DragLine _dragLine = DragLine.None;

        private void UserControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(TrimLayer);

            if (point.X > Item.BindingParam.Trimming.BorderLeft &&
                point.X < Item.BindingParam.Trimming.BorderRight &&
                point.Y < Item.BindingParam.Trimming.BorderTop)
            {
                //  GreaArea (Top)
                _dragLine = DragLine.Top;
            }
            else if (point.X > Item.BindingParam.Trimming.BorderLeft &&
                point.X < Item.BindingParam.Trimming.BorderRight &&
                point.Y > Item.BindingParam.Trimming.BorderBottom)
            {
                //  GreaArea (Bottom)
                _dragLine = DragLine.Bottom;
            }
            else if (point.Y > Item.BindingParam.Trimming.BorderTop &&
                point.Y < Item.BindingParam.Trimming.BorderBottom &&
                point.X < Item.BindingParam.Trimming.BorderLeft)
            {
                //  GreaArea (Left)
                _dragLine = DragLine.Left;
            }
            else if (point.Y > Item.BindingParam.Trimming.BorderTop &&
                point.Y < Item.BindingParam.Trimming.BorderBottom &&
                point.X > Item.BindingParam.Trimming.BorderRight)
            {
                //  GreaArea (Right)
                _dragLine = DragLine.Right;
            }
            else
            {
                //  GreaArea (Center)
                _dragLine = DragLine.None;
            }
        }

        private void UserControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragLine = DragLine.None;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var point = e.GetPosition(TrimLayer);
                double newLocation = -1;
                switch (_dragLine)
                {
                    case DragLine.Top:
                        newLocation = point.Y;
                        if (newLocation >= Item.BindingParam.Trimming.ViewBottom)
                        {
                            newLocation = Item.BindingParam.Trimming.ViewBottom;
                        }
                        else if (newLocation < 0)
                        {
                            newLocation = 0;
                        }
                        Item.BindingParam.Trimming.Top = newLocation / Item.BindingParam.Trimming.Scale;
                        break;
                    case DragLine.Bottom:
                        newLocation = point.Y;
                        if (newLocation <= Item.BindingParam.Trimming.ViewTop)
                        {
                            newLocation = Item.BindingParam.Trimming.ViewTop;
                        }
                        else if (newLocation > this.ActualHeight)
                        {
                            newLocation = this.ActualHeight;
                        }
                        Item.BindingParam.Trimming.Bottom = newLocation / Item.BindingParam.Trimming.Scale;
                        break;
                    case DragLine.Left:
                        newLocation = point.X;
                        if (newLocation >= Item.BindingParam.Trimming.ViewRight)
                        {
                            newLocation = Item.BindingParam.Trimming.ViewRight;
                        }
                        else if (newLocation < 0)
                        {
                            newLocation = 0;
                        }
                        Item.BindingParam.Trimming.Left = newLocation / Item.BindingParam.Trimming.Scale;
                        break;
                    case DragLine.Right:
                        newLocation = point.X;
                        if (newLocation <= Item.BindingParam.Trimming.ViewLeft)
                        {
                            newLocation = Item.BindingParam.Trimming.ViewLeft;
                        }
                        else if (newLocation > this.ActualWidth)
                        {
                            newLocation = this.ActualWidth;
                        }
                        Item.BindingParam.Trimming.Right = newLocation / Item.BindingParam.Trimming.Scale;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
