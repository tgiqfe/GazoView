using GazoView.Conf;
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
            }

        }

        private void UserControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            Item.BindingParam.State.MouseMoveTrimming = true;

            if (e.LeftButton == MouseButtonState.Pressed)
            {

                var point = e.GetPosition(TrimLayer);
                switch (_dragLine)
                {
                    case DragLine.Top:
                        var newTop = point.Y * Item.BindingParam.Trimming.Scale;
                        if (newTop > Item.BindingParam.Trimming.BorderBottom)
                        {
                            newTop = Item.BindingParam.Trimming.BorderBottom;
                        }
                        else if (newTop < 0)
                        {
                            newTop = 0;
                        }
                        Item.BindingParam.Trimming.Top = newTop * Item.BindingParam.Trimming.Scale;


                        Item.BindingParam.Trimming.Top = point.Y / Item.BindingParam.Trimming.Scale;
                        break;
                    case DragLine.Bottom:
                        Item.BindingParam.Trimming.Bottom = point.Y / Item.BindingParam.Trimming.Scale;
                        break;
                    case DragLine.Left:
                        Item.BindingParam.Trimming.Left = point.X / Item.BindingParam.Trimming.Scale;
                        break;
                    case DragLine.Right:
                        Item.BindingParam.Trimming.Right = point.X / Item.BindingParam.Trimming.Scale;
                        break;
                    default:
                        break;
                }

                
            }

            Item.BindingParam.State.MouseMoveTrimming = false;
            _dragLine = DragLine.None;
        }
    }
}
