using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GazoView.Lib.ImageInfo
{
    interface IImageItem
    {
        string FilePath { get;  }
        string FileName { get; }
        string FileExtension { get; }
        string LabelFileName { get; }
        string LabelFilePath { get; }
        string LabelFileExtension { get; }

        ImageSource Source { get; }

        double Width { get; }
        double Height { get; }
        string Size { get; }
        string LastWriteTime { get; }
        string Hash { get; }
    }
}
