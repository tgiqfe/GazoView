using System.Windows.Controls;
using System.Windows.Input;

namespace GazoView.Lib.Panel
{
    /// <summary>
    /// MoveTriangleLayer.xaml の相互作用ロジック
    /// </summary>
    public partial class MoveTriangleLayer : UserControl
    {
        public MoveTriangleLayer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Navigate to previous image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftTriangle_Click(object sender, MouseButtonEventArgs e)
        {
            Item.BindingParam.Images.Index--;
            Item.BindingParam.Images.ViewImage();
        }

        /// <summary>
        /// Navigate to next image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightTriangle_Click(object sender, MouseButtonEventArgs e)
        {
            Item.BindingParam.Images.Index++;
            Item.BindingParam.Images.ViewImage();
        }
    }
}
