using System;
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
using XamlAnimatedGif;

namespace GazoView.Lib
{
    public partial class GifAnimeLayer : UserControl
    {
        public GifAnimeLayer()
        {
            InitializeComponent();

            //  Add event (for drag move).
            EventManager.RegisterClassHandler(
                typeof(GifAnimeLayer),
                FrameworkElement.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler((sender, e) =>
                {
                    if (!Item.BindingParam.State.TrimmingMode) Item.MainBase.DragMove();
                }));
        }

        private void GifAnimation_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Item.BindingParam.Images.Current?.FilePath == null) return;
            if ((bool)e.NewValue)
            {   
                AnimationBehavior.SetSourceUri(ImageAnime, new Uri(Item.BindingParam.Images.Current.FilePath));
            }
            else
            {
                AnimationBehavior.SetSourceUri(ImageAnime, null);
            }
        }
    }
}
