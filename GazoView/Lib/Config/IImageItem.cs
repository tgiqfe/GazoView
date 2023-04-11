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
        string FileName { get; set; }

        string FilePath { get; set; }

        string FileExtension { get; set; }

        string LabelFileName { get; }

        string LabelFilePath { get; }

        string LabelFileExtension { get; }

        ImageSource Source { get; }

        double Width { get; }

        double Height { get; }

        string Size { get; }

        string LastWriteTime { get; }
    }
}
