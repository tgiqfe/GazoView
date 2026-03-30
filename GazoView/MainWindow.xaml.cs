using GazoView.Lib;
using GazoView.Lib.Panel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GazoView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            Item.MainWindow = this;
            this.DataContext = Item.BindingParam;

            //  Add event (for drag move).
            EventManager.RegisterClassHandler(
                typeof(AdvancedScrollViewer),
                FrameworkElement.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler((sender, e) => this.DragMove()));
        }

        /// <summary>
        /// Key down event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
            }
        }
    }
}
