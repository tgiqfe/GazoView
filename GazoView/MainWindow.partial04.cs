using GazoView.Lib.Conf;
using GazoView.Lib.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GazoView
{
    public partial class MainWindow : Window
    {
        private void ChangeInfoPanel()
        {
            Item.InfoPanel ??= new();
            Item.BindingParam.State.InfoPanel ^= true;

            if (Item.BindingParam.State.InfoPanel)
            {
                if (SpecialKeyStatus.IsShiftPressed())
                {
                    //  Shiftを押しながら
                    Grid.SetColumn(Item.InfoPanel, 0);
                    GlobalGrid.Children.Add(Item.InfoPanel);
                    Column0.Width = new GridLength(300);
                }
                else
                {
                    //  Shiftを押さないで
                    Grid.SetColumn(Item.InfoPanel, 1);
                    GlobalGrid.Children.Add(Item.InfoPanel);
                    Item.InfoPanel.HorizontalAlignment = HorizontalAlignment.Left;
                }
            }
            else
            {
                GlobalGrid.Children.Remove(Item.InfoPanel);
                Column0.Width = new GridLength(0);
            }
        }

        private void SwitchTrimmingMode(bool? toEnable = null)
        {
            Item.BindingParam.State.TrimmingMode =
                toEnable ?? !Item.BindingParam.State.TrimmingMode;
        }
    }
}
