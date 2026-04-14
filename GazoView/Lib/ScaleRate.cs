
using GazoView.Lib.Panel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GazoView.Lib
{
    public class ScaleRate
    {
        private static double[] _ticks = new double[]
        {
            0.18,  0.26, 0.34, 0.42,
            0.5, 0.56, 0.62, 0.68, 0.74, 0.8, 0.84, 0.88, 0.92, 0.96,
            1, 1.04, 1.08, 1.12, 1.16, 1.2, 1.26, 1.32, 1.38, 1.44, 1.5,
            1.58, 1.64, 1.72, 1.8, 1.9, 2, 2,1, 2.2, 2.3, 2.4,
            2.6, 2.8, 3, 3.2, 3.4, 3.6, 3.8, 4,
            4.4, 4.8, 5.2, 5.6, 6, 6.4, 6.8, 7.2, 7.6,
            8, 8.8, 9.6, 10.4, 11.2, 12, 12.8, 13.6, 14.4, 15.2, 16
        };

        private static readonly int DEF_INDEX = Array.IndexOf(_ticks, 1.0);
        private int _index = DEF_INDEX;
        private int _preview = DEF_INDEX;

        public int TicksLength { get => _ticks.Length; }
        public int DefaultIndex { get => DEF_INDEX; }
        public double Scale { get => _ticks[_index]; }
        public double PreviewScale { get => _ticks[_preview]; }
        public bool IsMax { get => _index == _ticks.Length - 1; }
        public bool IsMin { get => _index == 0; }

        public bool IsScalingMode = true;

        public int Index
        {
            get => _index;
            set
            {
                _preview = _index;
                _index = value;
            }
        }

        public void Reset()
        {
            _index = DEF_INDEX;
            _preview = DEF_INDEX;
        }

        public void ZoomImage(Image mainImage, AdvancedScrollViewer scrollViewer, MouseWheelEventArgs e)
        {
            if (e == null)
            {
                this.Reset();
            }
            else
            {
                if (e.Delta > 0 && !this.IsMax)
                {
                    //  Zoom in.
                    this.Index++;
                }
                else if (e.Delta < 0 && !this.IsMin)
                {
                    //  Zoom out.
                    this.Index--;
                }
                else
                {
                    //  No change. current index is max or min.
                    return;
                }
            }

            if (this.Scale == 1.0)
            {
                mainImage.SetBinding(Image.WidthProperty, new Binding("ActualWidth") { Source = scrollViewer });
                mainImage.SetBinding(Image.HeightProperty, new Binding("ActualHeight") { Source = scrollViewer });
                this.IsScalingMode = false;
            }
            else
            {
                BindingOperations.ClearBinding(mainImage, Image.WidthProperty);
                BindingOperations.ClearBinding(mainImage, Image.HeightProperty);
                var newWidth = Item.MainWindow.ActualWidth * this.Scale;
                var newHeight = (Item.MainWindow.ActualHeight - SystemParameters.WindowCaptionHeight) * this.Scale;
                if (this.Scale > 1)
                {
                    //  Before scaling, get poinger location.
                    var mousePoint = e.GetPosition(scrollViewer);
                    var viewX = scrollViewer.HorizontalOffset;
                    var viewY = scrollViewer.VerticalOffset;
                    mainImage.Width = newWidth;
                    mainImage.Height = newHeight;
                    var relateScale = this.Scale / this.PreviewScale;
                    scrollViewer.ScrollToHorizontalOffset((mousePoint.X + viewX) * relateScale - mousePoint.X);
                    scrollViewer.ScrollToVerticalOffset((mousePoint.Y + viewY) * relateScale - mousePoint.Y);
                }
                else if (this.Scale < 1)
                {
                    mainImage.Width = newWidth;
                    mainImage.Height = newHeight;
                }
            }
        }
    }
}
