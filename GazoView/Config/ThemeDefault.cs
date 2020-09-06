using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Config
{
    public class ThemeDefault : ThemeBase
    {
        public override string WindowBackground { get; set; } = "#FF404040";
        public override string DragOverBackground { get; set; } = "DodgerBlue";
        public override string TitleForeground { get; set; } = "Wheat";
        public override string ImageInfoForeground { get; set; } = "White";
    }
}
