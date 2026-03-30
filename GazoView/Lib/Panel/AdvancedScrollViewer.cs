using System.Windows.Controls;
using System.Windows.Media;

namespace GazoView.Lib.Panel
{
    internal class AdvancedScrollViewer : ScrollViewer
    {
        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
        {
            return null;
        }
    }
}
