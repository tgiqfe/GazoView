﻿using GazoView.Lib.Conf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView
{
    internal class BindingParam
    {
        public Setting Setting { get; set; }

        public Images Images { get; set; }

        public State State { get; set; }

        public Trimming Trimming { get; set; }
    }
}
