using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GazoView.Lib
{
    class DpiAgnosticImage : Image
    {
        protected override Size MeasureOverride(Size constraint)
        {
            var bitmapImage = Source as BitmapImage;
            return bitmapImage == null?
                base.MeasureOverride(constraint) :
                new Size(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            return base.ArrangeOverride(arrangeSize);
        }
    }
}
