using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GazoView.Functions
{
    class AlternateApplication
    {
        public static void Execute(params string[] args)
        {
            if (args.Length > 0)
            {
                string arguments = string.Join(" ",
                    args.Select(x => x.Contains(" ") ? "\"" + x + "\"" : x));
                if (!string.IsNullOrEmpty(arguments))
                {
                    using (Process proc = new Process())
                    {
                        proc.StartInfo.FileName = Item.Data.Setting.AlternateApp;
                        proc.StartInfo.Arguments = arguments;
                        proc.Start();
                    }
                }
            }
        }
    }
}
