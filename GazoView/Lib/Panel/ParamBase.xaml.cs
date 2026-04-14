using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GazoView.Lib.Panel
{
    /// <summary>
    /// ParamBase.xaml の相互作用ロジック
    /// </summary>
    public partial class ParamBase : UserControl
    {
        private bool _isDragging = false;
        private Point _dragStartPoint;

        public ParamBase()
        {
            InitializeComponent();
            this.Margin = new Thickness(
                Item.BindingParam.Setting.ParamPanelLeft,
                Item.BindingParam.Setting.ParamPanelTop,
                0, 0);
        }

        #region Drag move for ParamBase

        /// <summary>
        /// ドラッグ開始
        /// </summary>
        private void ParamBase_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Parent is Grid parentGrid)
            {
                _isDragging = true;
                _dragStartPoint = e.GetPosition(parentGrid);
                this.CaptureMouse();
                e.Handled = true;
            }
        }

        /// <summary>
        /// ドラッグ中の移動
        /// </summary>
        private void ParamBase_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.Parent is Grid parentGrid)
                {
                    Point currentPoint = e.GetPosition(parentGrid);
                    double offsetX = currentPoint.X - _dragStartPoint.X;
                    double offsetY = currentPoint.Y - _dragStartPoint.Y;

                    Thickness currentMargin = this.Margin;
                    this.Margin = new Thickness(
                        currentMargin.Left + offsetX,
                        currentMargin.Top + offsetY,
                        currentMargin.Right,
                        currentMargin.Bottom);

                    _dragStartPoint = currentPoint;
                }
            }
        }

        /// <summary>
        /// ドラッグ終了
        /// </summary>
        private void ParamBase_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                this.ReleaseMouseCapture();
                e.Handled = true;

                var mainWindowWidth = Item.MainWindow.ActualWidth;
                var mainWindowHeight = Item.MainWindow.ActualHeight - SystemParameters.WindowCaptionHeight;
                var paramPanelWidth = this.ActualWidth;
                var paramPanelHeight = this.ActualHeight;
                var margin = this.Margin;
                if (margin.Left < 0) margin.Left = 0;
                if (margin.Top < 0) margin.Top = 0;
                if (margin.Left + paramPanelWidth > mainWindowWidth) margin.Left = mainWindowWidth - paramPanelWidth;
                if (margin.Top + paramPanelHeight > mainWindowHeight) margin.Top = mainWindowHeight - paramPanelHeight;
                this.Margin = new Thickness(margin.Left, margin.Top, 0, 0);

                Item.BindingParam.Setting.ParamPanelLeft = (int)margin.Left;
                Item.BindingParam.Setting.ParamPanelTop = (int)margin.Top;
            }
        }

        #endregion




    }
}
