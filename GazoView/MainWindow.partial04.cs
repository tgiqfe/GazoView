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
        private int _infoPanelIndex = 0;

        private void ChangeInfoPanel()
        {
            //Item.BindingParam.State.ShowInfoPanel = !Item.BindingParam.State.ShowInfoPanel;

            _infoPanelIndex++;
            if (_infoPanelIndex > 2)
            {
                _infoPanelIndex = 0;
            }

            Item.InfoPanel1 ??= new InfoPanel1();

            switch (_infoPanelIndex)
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
