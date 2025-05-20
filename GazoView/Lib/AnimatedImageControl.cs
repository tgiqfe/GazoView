using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace GazoView.Lib
{
    internal class AnimatedImageControl : System.Windows.Controls.UserControl
    {
        delegate void OnFrameChangedDelegate();
        System.Drawing.Image m_animatedImage;
        HwndSource m_hwnd;
        Rectangle m_rectangle;
        System.Windows.Point m_point;
        System.Drawing.Brush m_brush;

        public AnimatedImageControl() { }

        public AnimatedImageControl(Window p_window, Bitmap p_bitmap, System.Drawing.Brush p_brush)
        {
            m_animatedImage = p_bitmap;
            m_hwnd = PresentationSource.FromVisual((Visual)p_window) as HwndSource;
            m_brush = p_brush;
            Width = p_bitmap.Width;
            Height = p_bitmap.Height;
            m_point = p_window.PointToScreen(new System.Windows.Point(0, 0));

            ImageAnimator.Animate(m_animatedImage, new EventHandler(OnFrameChanged));

            Loaded += new RoutedEventHandler(AnimatedImageControl_Loaded);
        }

        void AnimatedImageControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Point point = this.PointToScreen(new System.Windows.Point(0, 0));
            m_rectangle = new System.Drawing.Rectangle((int)(point.X - m_point.X), (int)(point.Y - m_point.Y), (int)Width, (int)Height);
        }

        void OnFrameChangedTS()
        {
            InvalidateVisual();
        }

        void OnFrameChanged(object o, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new OnFrameChangedDelegate(OnFrameChangedTS));
        }

        protected override void OnRender(DrawingContext p_drawingContext)
        {
            //Get the next frame ready for rendering.
            ImageAnimator.UpdateFrames(m_animatedImage);

            //Draw the next frame in the animation.
            Graphics gr = Graphics.FromHwnd(m_hwnd.Handle);
            gr.FillRectangle(m_brush, m_rectangle);
            gr.DrawImage(m_animatedImage, m_rectangle);
        }
    }
}
