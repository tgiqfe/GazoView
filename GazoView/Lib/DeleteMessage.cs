using GazoView.Lib.Panel;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazoView.Lib
{
    public class DeleteMessage
    {
        private DeleteMessageWindow _deleteMessageWindow;
        public bool IsVisible { get; set; }

        public void ShowWindow()
        {
            _deleteMessageWindow ??= new DeleteMessageWindow();
            _deleteMessageWindow.Owner = Item.MainWindow;
            _deleteMessageWindow.Show();
            
        }

        public void HideWindow()
        {

        }

        public void SwitchMode(bool? toEnable = null)
        {
            if (toEnable == null) toEnable = !this.IsVisible;
            if (toEnable == true)
            {
                ShowWindow();
            }
            else
            {
                HideWindow();
            }
        }
    }
}
