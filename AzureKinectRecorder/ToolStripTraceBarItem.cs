using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AzureKinectRecorder
{
    /// <summary>
    /// Adds trackbar to toolstrip stuff
    /// </summary>
    [
    ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)
    ]

    public class ToolStripTraceBarItem : ToolStripControlHost
    {
        public TrackBar bar;
        public ToolStripTraceBarItem() : base(new TrackBar())
        {
            bar = (TrackBar)base.Control;
        }
    }
}
