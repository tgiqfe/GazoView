using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView
{
    /// <summary>
    /// トリミングモードの為の、ドラッグ中のLine
    /// </summary>
    public enum DragLine
    {
        None,
        HorizontalLeft,
        HorizontalRight,
        VerticalTop,
        VerticalBottom
    }
}
