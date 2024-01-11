﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Image
{
    internal class ScaleRate
    {
        private static readonly double[] _ticks = new double[]
        {
            0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.8, 2, 2.4, 2.8, 3.2, 3.6, 4, 4.8, 5.6, 6.4, 7.2, 8, 9, 10
        };

        const int DEF_INDEX = 8;

        private int _index = DEF_INDEX;
        private int _preview = DEF_INDEX;

        public double Scale { get { return _ticks[_index]; } }
        public double PreviewScale { get { return _ticks[_preview]; } }
        public bool IsMax { get { return _index == _ticks.Length - 1; } }
        public bool IsMin { get { return _index == 0; } }

        public int Index
        {
            get { return _index; }
            set
            {
                _preview = _index;
                _index = value;
            }
        }

        public ScaleRate()
        {
            Init();
        }

        public void Init()
        {
            _index = DEF_INDEX;
            _preview = DEF_INDEX;
        }
    }
}
