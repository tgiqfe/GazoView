namespace GazoView.Lib.Functions
{
    public class ImageFunction
    {
        public static void SwitchTrimmingMode(bool? toEnable = null)
        {
            Item.BindingParam.Trimming.IsTrimmingMode =
                toEnable ?? !Item.BindingParam.Trimming.IsTrimmingMode;
        }


    }
}
