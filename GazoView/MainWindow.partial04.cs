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
            /*
            Item.BindingParam.State.InfoPanel++;
            Item.InfoPanel ??= new();

            switch (Item.BindingParam.State.InfoPanel)
            {
                case State.InfoPanelStatus.None:
                    GlobalGrid.Children.Remove(Item.InfoPanel);
                    Column0.Width = new GridLength(0);
                    break;
                case State.InfoPanelStatus.OverlapLeft:
                    Grid.SetColumn(Item.InfoPanel, 1);
                    GlobalGrid.Children.Add(Item.InfoPanel);
                    Item.InfoPanel.HorizontalAlignment = HorizontalAlignment.Left;
                    break;
                case State.InfoPanelStatus.ViewLeft:
                    GlobalGrid.Children.Remove(Item.InfoPanel);
                    Grid.SetColumn(Item.InfoPanel, 0);
                    GlobalGrid.Children.Add(Item.InfoPanel);
                    Column0.Width = new GridLength(300);
                    break;
            }
            */

            Item.InfoPanel ??= new();
            Item.BindingParam.State.InfoPanel2 ^= true;

            if (Item.BindingParam.State.InfoPanel2)
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
