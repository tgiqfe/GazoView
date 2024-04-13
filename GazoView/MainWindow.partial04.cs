using GazoView.Lib.Conf;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }
    }
}
