using System.Windows.Controls;
using System.Windows.Media;

namespace GazoView.Lib.Panel
{
    public class AdvancedScrollViewer : ScrollViewer
    {
        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
        {
            return null;
        }
    }
}
