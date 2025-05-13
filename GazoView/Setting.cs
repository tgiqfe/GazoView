using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView
{
    internal class Setting
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        
        public int MaxTrimmingHistory { get; set; }
        public int MaxSizeHistory { get; set; }

        public List<string> TrimmingHistries { get; set; }
        public List<string> SizeHistories { get; set; }
    }
}
