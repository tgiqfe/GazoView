﻿using System.Runtime.InteropServices;

namespace GazoView.Lib.Functions
{
    internal class NaturalStringComparer :
        System.Collections.IComparer,
        System.Collections.Generic.IComparer<string>
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int StrCmpLogicalW(string x, string y);

        public int Compare(string x, string y)
        {
            return StrCmpLogicalW(x, y);
        }

        public int Compare(object x, object y)
        {
            return this.Compare(x.ToString(), y.ToString());
        }
    }
}
