using System;
using System.Collections.Generic;
using System.Text;

namespace GazoView.Lib.Functions
{
    public class ImageFunction
    {
        public static void SwitchTrimmingMode(bool? toEnable = null)
        {
            /*
            if (Item.BindingParam.Trimming.Top < 0)
            {
                Item.BindingParam.Trimming.Top = 100;
            }
            if (Item.BindingParam.Trimming.Bottom < 0)
            {
                Item.BindingParam.Trimming.Bottom = 300;
            }
            if (Item.BindingParam.Trimming.Left < 0)
            {
                Item.BindingParam.Trimming.Left = 100;
            }
            if (Item.BindingParam.Trimming.Right < 0)
            {
                Item.BindingParam.Trimming.Right = 300;
            }
            */
            Item.BindingParam.Trimming.IsTrimmingMode =
                toEnable ?? !Item.BindingParam.Trimming.IsTrimmingMode;
        }
    }
}
