using GazoView.Conf;
using GazoView.Lib;
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
            //Item.BindingParam.State.ShowInfoPanel = !Item.BindingParam.State.ShowInfoPanel;

            Item.BindingParam.State.InfoPanelIndex++;
            if (Item.BindingParam.State.InfoPanelIndex > 2)
            {
                Item.BindingParam.State.InfoPanelIndex = 0;
            }

            Item.InfoPanel1 ??= new InfoPanel1();

            switch (Item.BindingParam.State.InfoPanelIndex)
            {
                case 0:
                    GlobalGrid.Children.Remove(Item.InfoPanel1);
                    Column0.Width = new GridLength(0);
                    break;
                case 1:
                    Grid.SetColumn(Item.InfoPanel1, 1);
                    GlobalGrid.Children.Add(Item.InfoPanel1);
                    break;
                case 2:
                    GlobalGrid.Children.Remove(Item.InfoPanel1);
                    Grid.SetColumn(Item.InfoPanel1, 0);
                    GlobalGrid.Children.Add(Item.InfoPanel1);
                    Column0.Width = new GridLength(300);
                    break;
            }
        }
    }
}
