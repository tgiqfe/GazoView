namespace GazoView.Lib
{
    internal class AdvancedScrollViewer : System.Windows.Controls.ScrollViewer
    {
        protected override System.Windows.Media.GeometryHitTestResult HitTestCore(System.Windows.Media.PointHitTestParameters parameters)
        {
            return null;
        }
    }
}
