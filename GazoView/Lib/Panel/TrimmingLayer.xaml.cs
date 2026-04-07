using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GazoView.Lib.Panel
{
    public partial class TrimmingLayer : UserControl
    {
        private DragLine _dragLine = DragLine.None;

        public TrimmingLayer()
        {
            InitializeComponent();
        }

        private void TrimmingLayer_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!Item.IsInitialized) return;
            if (this.IsVisible)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (Item.BindingParam.Trimming.Top < 0)
                    {
                        int top = (int)(this.DesiredSize.Height / 3);
                        Item.BindingParam.Trimming.Top = top;
                        Debug.WriteLine($"Trimming.Top is set to {top}.");
                    }
                    if (Item.BindingParam.Trimming.Bottom > this.DesiredSize.Height)
                    {
                        int bottom = (int)(this.DesiredSize.Height / 3 * 2);
                        Item.BindingParam.Trimming.Bottom = bottom;
                        Debug.WriteLine($"Trimming.Bottom is set to {bottom}.");
                    }
                    if (Item.BindingParam.Trimming.Left < 0)
                    {
                        int left = (int)(this.DesiredSize.Width / 3);
                        Item.BindingParam.Trimming.Left = left;
                        Debug.WriteLine($"Trimming.Left is set to {left}.");
                    }
                    if (Item.BindingParam.Trimming.Right > this.DesiredSize.Width)
                    {
                        int right = (int)(this.DesiredSize.Width / 3 * 2);
                        Item.BindingParam.Trimming.Right = right;
                        Debug.WriteLine($"Trimming.Right is set to {right}.");
                    }
                }), System.Windows.Threading.DispatcherPriority.Loaded);
            }
            else
            {
                //  TrimmingMode to disable event.
                Debug.WriteLine("TrimmingLayer is not visible.");
            }
        }

        #region Mouse event for trimming area moving.

        enum DragLine
        {
            None,
            Top,
            Bottom,
            Left,
            Right,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        private void TrimmingLayer_view_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = e.GetPosition(TrimmingLayer_view);
            var trimming = Item.BindingParam.Trimming;

            //  20px margin for dragging line start position.
            var margin = 20;
            var letherAreaLength = Math.Min(trimming.ViewRight - trimming.ViewLeft, trimming.ViewBottom - trimming.ViewTop);
            if (margin * 2 > letherAreaLength)
            {
                margin = 0;
            }

            if (point.X >= trimming.ViewLeft && point.X <= trimming.ViewRight && point.Y < (trimming.ViewTop + margin))
            {
                _dragLine = DragLine.Top;
                this.Cursor = Cursors.SizeNS;
            }
            else if (point.X >= trimming.ViewLeft && point.X <= trimming.ViewRight && 
                (point.Y > (trimming.ViewBottom - margin) || point.Y > (this.DesiredSize.Height - margin)))
            {
                _dragLine = DragLine.Bottom;
                this.Cursor = Cursors.SizeNS;
            }
            else if (point.Y >= trimming.ViewTop && point.Y <= trimming.ViewBottom && point.X < (trimming.ViewLeft + margin))
            {
                _dragLine = DragLine.Left;
                this.Cursor = Cursors.SizeWE;
            }
            else if (point.Y >= trimming.ViewTop && point.Y <= trimming.ViewBottom && 
                (point.X > (trimming.ViewRight - margin) || point.X > (this.DesiredSize.Width - margin)))
            {
                _dragLine = DragLine.Right;
                this.Cursor = Cursors.SizeWE;
            }
            else if (point.X < trimming.ViewLeft && point.Y < trimming.ViewTop)
            {
                _dragLine = DragLine.TopLeft;
                this.Cursor = Cursors.SizeNWSE;
            }
            else if (point.X < trimming.ViewLeft && point.Y > trimming.ViewBottom)
            {
                _dragLine = DragLine.BottomLeft;
                this.Cursor = Cursors.SizeNESW;
            }
            else if (point.X > trimming.ViewRight && point.Y < trimming.ViewTop)
            {
                _dragLine = DragLine.TopRight;
                this.Cursor = Cursors.SizeNESW;
            }
            else if (point.X > trimming.ViewRight && point.Y > trimming.ViewBottom)
            {
                _dragLine = DragLine.BottomRight;
                this.Cursor = Cursors.SizeNWSE;
            }
            else
            {
                _dragLine = DragLine.None;
            }

            if (_dragLine != DragLine.None)
            {
                TrimmingLayer_view.CaptureMouse();
            }
        }

        private void TrimmingLayer_view_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragLine = DragLine.None;
            this.Cursor = Cursors.Arrow;
            TrimmingLayer_view.ReleaseMouseCapture();
        }

        private void TrimmingLayer_view_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _dragLine != DragLine.None)
            {
                var point = e.GetPosition(TrimmingLayer_view);
                var trimming = Item.BindingParam.Trimming;

                //double maxRight = _viewLeft + _viewWidth;
                //double maxBottom = _viewTop + _viewHeight;

                switch (_dragLine)
                {
                    case DragLine.Top:
                        {
                            double y = point.Y;
                            if (y < 0) y = 0;
                            trimming.Top = (int)y;
                        }
                        break;
                    case DragLine.Bottom:
                        {
                            double y = point.Y;
                            if (y > this.DesiredSize.Height) y = this.DesiredSize.Height;
                            trimming.Bottom = (int)y;
                        }
                        break;
                    case DragLine.Left:
                        {
                            double x = point.X;
                            if (x < 0) x = 0;
                            trimming.Left = (int)x;
                        }
                        break;
                    case DragLine.Right:
                        {
                            double x = point.X;
                            if(x > this.DesiredSize.Width) x = this.DesiredSize.Width;
                            trimming.Right = (int)x;
                        }
                        break;
                    case DragLine.TopLeft:
                        {
                            double x = Math.Max(0, Math.Min(point.X, TrimmingLayer_view.ActualWidth));
                            double y = Math.Max(0, Math.Min(point.Y, TrimmingLayer_view.ActualHeight));
                            trimming.Top = (int)y;
                            trimming.Left = (int)x;
                        }
                        break;
                    case DragLine.TopRight:
                        {
                            double x = point.X;
                            if (x > this.DesiredSize.Width) x = this.DesiredSize.Width;
                            double y = point.Y;
                            if (y < 0) y = 0;
                            trimming.Top = (int)y;
                            trimming.Right = (int)x;
                        }
                        break;
                    case DragLine.BottomLeft:
                        {
                            double x = point.X;
                            if (x < 0) x = 0;
                            double y = point.Y;
                            if (y > this.DesiredSize.Height) y = this.DesiredSize.Height;
                            trimming.Bottom = (int)y;
                            trimming.Left = (int)x;
                        }
                        break;
                    case DragLine.BottomRight:
                        {
                            double x = point.X;
                            if (x > this.DesiredSize.Width) x = this.DesiredSize.Width;
                            double y = point.Y;
                            if (y > this.DesiredSize.Height) y = this.DesiredSize.Height;
                            trimming.Bottom = (int)y;
                            trimming.Right = (int)x;
                        }
                        break;
                }
            }
        }

        #endregion
    }
}
