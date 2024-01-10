﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GazoView.Conf;

namespace GazoView.Lib
{
    /// <summary>
    /// InfoPanel1.xaml の相互作用ロジック
    /// </summary>
    public partial class InfoPanel1 : UserControl
    {
        public InfoPanel1()
        {
            InitializeComponent();
            this.DataContext = Item.BindingParam;
        }
    }
}
