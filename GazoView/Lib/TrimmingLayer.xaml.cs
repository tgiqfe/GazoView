using System.Windows.Controls;
using System.Windows.Input;

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
        }

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

        private DragLine _dragLine = DragLine.None;

        private void TrimLayer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(TrimLayer);
            var trimming = Item.BindingParam.Trimming;

            if (point.X > trimming.ViewLeft && point.X < trimming.ViewRight && point.Y < trimming.ViewTop)
            {
                _dragLine = DragLine.Top;
                this.Cursor = Cursors.SizeNS;
            }
            else if (point.X > trimming.ViewLeft && point.X < trimming.ViewRight && point.Y > trimming.ViewBottom)
            {
                _dragLine = DragLine.Bottom;
                this.Cursor = Cursors.SizeNS;
            }
            else if (point.Y > trimming.ViewTop && point.Y < trimming.ViewBottom && point.X < trimming.ViewLeft)
            {
                _dragLine = DragLine.Left;
                this.Cursor = Cursors.SizeWE;
            }
            else if (point.Y > trimming.ViewTop && point.Y < trimming.ViewBottom && point.X > trimming.ViewRight)
            {
                _dragLine = DragLine.Right;
                this.Cursor = Cursors.SizeWE;
            }
            else if (point.X < trimming.ViewLeft && point.Y < trimming.ViewTop)
            {
                _dragLine = DragLine.TopLeft;
                this.Cursor = Cursors.SizeNWSE;
            }
            else if (point.X > trimming.ViewRight && point.Y < trimming.ViewTop)
            {
                _dragLine = DragLine.TopRight;
                this.Cursor = Cursors.SizeNESW;
            }
            else if (point.X < trimming.ViewLeft && point.Y > trimming.ViewBottom)
            {
                _dragLine = DragLine.BottomLeft;
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
        }

        private void TrimLayer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragLine = DragLine.None;
            this.Cursor = Cursors.Arrow;
        }

        private void TrimLayer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var point = e.GetPosition(TrimLayer);
                double newLocationX = -1;
                double newLocationY = -1;
                switch (_dragLine)
                {
                    case DragLine.Top:
                        newLocationY = point.Y;
                        if (newLocationY >= Item.BindingParam.Trimming.ViewBottom)
                        {
                            newLocationY = Item.BindingParam.Trimming.ViewBottom;
                        }
                        else if (newLocationY < 1)
                        {
                            newLocationY = 0;
                        }
                        Item.BindingParam.Trimming.Top = (int)Math.Round(newLocationY / Item.BindingParam.Trimming.Scale);
                        break;
                    case DragLine.Bottom:
                        newLocationY = point.Y;
                        if (newLocationY <= Item.BindingParam.Trimming.ViewTop)
                        {
                            newLocationY = Item.BindingParam.Trimming.ViewTop;
                        }
                        else if (newLocationY > Item.MainBase.MainImage.ActualHeight)
                        {
                            newLocationY = Item.MainBase.MainImage.ActualHeight;
                        }
                        Item.BindingParam.Trimming.Bottom = (int)Math.Round(newLocationY / Item.BindingParam.Trimming.Scale);
                        break;
                    case DragLine.Left:
                        newLocationX = point.X;
                        if (newLocationX >= Item.BindingParam.Trimming.ViewRight)
                        {
                            newLocationX = Item.BindingParam.Trimming.ViewRight;
                        }
                        else if (newLocationX < 1)
                        {
                            newLocationX = 0;
                        }
                        Item.BindingParam.Trimming.Left = (int)Math.Round(newLocationX / Item.BindingParam.Trimming.Scale);
                        break;
                    case DragLine.Right:
                        newLocationX = point.X;
                        if (newLocationX <= Item.BindingParam.Trimming.ViewLeft)
                        {
                            newLocationX = Item.BindingParam.Trimming.ViewLeft;
                        }
                        else if (newLocationX > Item.MainBase.MainImage.ActualWidth)
                        {
                            newLocationX = Item.MainBase.MainImage.ActualWidth;
                        }
                        Item.BindingParam.Trimming.Right = (int)Math.Round(newLocationX / Item.BindingParam.Trimming.Scale);
                        break;
                    case DragLine.TopLeft:
                        newLocationX = point.X;
                        newLocationY = point.Y;
                        if (newLocationX >= Item.BindingParam.Trimming.ViewRight)
                        {
                            newLocationX = Item.BindingParam.Trimming.ViewRight;
                        }
                        else if (newLocationX < 1)
                        {
                            newLocationX = 0;
                        }
                        if (newLocationY >= Item.BindingParam.Trimming.ViewBottom)
                        {
                            newLocationY = Item.BindingParam.Trimming.ViewBottom;
                        }
                        else if (newLocationY < 1)
                        {
                            newLocationY = 0;
                        }
                        Item.BindingParam.Trimming.Left = (int)Math.Round(newLocationX / Item.BindingParam.Trimming.Scale);
                        Item.BindingParam.Trimming.Top = (int)Math.Round(newLocationY / Item.BindingParam.Trimming.Scale);
                        break;
                    case DragLine.TopRight:
                        newLocationX = point.X;
                        newLocationY = point.Y;
                        if (newLocationX <= Item.BindingParam.Trimming.ViewLeft)
                        {
                            newLocationX = Item.BindingParam.Trimming.ViewLeft;
                        }
                        else if (newLocationX > Item.MainBase.MainImage.ActualWidth)
                        {
                            newLocationX = Item.MainBase.MainImage.ActualWidth;
                        }
                        if (newLocationY >= Item.BindingParam.Trimming.ViewBottom)
                        {
                            newLocationY = Item.BindingParam.Trimming.ViewBottom;
                        }
                        else if (newLocationY < 1)
                        {
                            newLocationY = 0;
                        }
                        Item.BindingParam.Trimming.Right = (int)Math.Round(newLocationX / Item.BindingParam.Trimming.Scale);
                        Item.BindingParam.Trimming.Top = (int)Math.Round(newLocationY / Item.BindingParam.Trimming.Scale);
                        break;
                    case DragLine.BottomLeft:
                        newLocationX = point.X;
                        newLocationY = point.Y;
                        if (newLocationX >= Item.BindingParam.Trimming.ViewRight)
                        {
                            newLocationX = Item.BindingParam.Trimming.ViewRight;
                        }
                        else if (newLocationX < 1)
                        {
                            newLocationX = 0;
                        }
                        if (newLocationY <= Item.BindingParam.Trimming.ViewTop)
                        {
                            newLocationY = Item.BindingParam.Trimming.ViewTop;
                        }
                        else if (newLocationY > Item.MainBase.MainImage.ActualHeight)
                        {
                            newLocationY = Item.MainBase.MainImage.ActualHeight;
                        }
                        Item.BindingParam.Trimming.Left = (int)Math.Round(newLocationX / Item.BindingParam.Trimming.Scale);
                        Item.BindingParam.Trimming.Bottom = (int)Math.Round(newLocationY / Item.BindingParam.Trimming.Scale);
                        break;
                    case DragLine.BottomRight:
                        newLocationX = point.X;
                        newLocationY = point.Y;
                        if (newLocationX <= Item.BindingParam.Trimming.ViewLeft)
                        {
                            newLocationX = Item.BindingParam.Trimming.ViewLeft;
                        }
                        else if (newLocationX > Item.MainBase.MainImage.ActualWidth)
                        {
                            newLocationX = Item.MainBase.MainImage.ActualWidth;
                        }
                        if (newLocationY <= Item.BindingParam.Trimming.ViewTop)
                        {
                            newLocationY = Item.BindingParam.Trimming.ViewTop;
                        }
                        else if (newLocationY > Item.MainBase.MainImage.ActualHeight)
                        {
                            newLocationY = Item.MainBase.MainImage.ActualHeight;
                        }
                        Item.BindingParam.Trimming.Right = (int)Math.Round(newLocationX / Item.BindingParam.Trimming.Scale);
                        Item.BindingParam.Trimming.Bottom = (int)Math.Round(newLocationY / Item.BindingParam.Trimming.Scale);
                        break;
                }
            }
        }
    }
}
