using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace GazoView.Lib.Config
{
    interface IImageItem
    {
        string FileName { get; }

        string FilePath { get; }

        string FileExtension { get; }

        ImageSource ImageSource { get; }

        double Width { get; }

        double Height { get; }
    }
}
