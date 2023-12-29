using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Functions
{
    internal class ScalingRate
    {
        private static readonly double[] _ticks = new double[]
        {
            0.2, 0.25, 0.3, 0.35, 0.4, 0.45, 0.5, 0.6, 0.7, 0.8, 0.9, 1, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.8, 2, 2.2, 2.4, 2.6, 2.8, 3, 3.2, 3.6, 4, 4.8, 5.6, 6.4, 7.2, 8
        };

        public static int TickIndex = 11;

        public static (double width, double height, double rate) GetScalingRate(double mainWidth, double mainHeight, int direction)
        {
            double retWidth = mainWidth;
            double retheight = mainHeight;
            double rate = 1;

            if (direction > 0 && TickIndex < _ticks.Length - 1)
            {
                TickIndex++;
            }
            else if (direction < 0 && TickIndex > 0)
            {
                TickIndex--;
                retWidth = mainWidth * _ticks[TickIndex];
                retheight = mainHeight * _ticks[TickIndex];
                if (retWidth < 100 || retheight < 100)
                {
                    TickIndex++;
                    retWidth = mainWidth * _ticks[TickIndex];
                    retheight = mainHeight * _ticks[TickIndex];
                }
            }
            rate = retWidth / mainWidth;

            return (retWidth, retheight, rate);

        }


    }
}
