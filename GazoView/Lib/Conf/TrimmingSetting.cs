using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Conf
{
    internal class TrimmingSetting
    {
        public int MaxHistory { get; set; }
        public List<TrimmingHistory> Histories { get; set; }

        public void AddHistory(int left, int top, int width, int height)
        {
            this.Histories.Insert(0, new TrimmingHistory()
            {
                Top = top,
                Bottom = top + height,
                Left = left,
                Right = left + width,
            });
            if (this.Histories.Count > this.MaxHistory)
            {
                this.Histories.RemoveAt(this.Histories.Count - 1);
            }
        }
    }

    internal class TrimmingHistory
    {
        public int Top { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
    }
}
