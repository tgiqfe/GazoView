using System.Windows;
using System.Windows.Controls;

namespace GazoView.Lib.Panel
{
    public partial class ParamSliderPanel : UserControl
    {
        public ParamSliderPanel()
        {
            InitializeComponent();
            ScaleRateSlider.Maximum = Item.BindingParam.ScaleRate.TicksLength;
            InitScaleRateSlider();
        }

        private void InitScaleRateSlider()
        {
            ScaleRateSlider.Value = Item.BindingParam.ScaleRate.DefaultIndex;
            UpdateScaleRateValue();
        }

        private void UpdateScaleRateValue()
        {
            int index = (int)ScaleRateSlider.Value;
            if (index >= 0 && index < Item.BindingParam.ScaleRate.TicksLength)
            {
                ScaleRateValue.Text = Item.BindingParam.ScaleRate.Scale.ToString();
            }
        }

        private void ScaleRateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateScaleRateValue();
        }
    }
}
