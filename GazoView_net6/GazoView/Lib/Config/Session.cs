using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Config
{
    internal class Session
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public Session()
        {
            this.Id = Process.GetCurrentProcess().Id;
            this.Date = DateTime.Now;
        }
    }
}
