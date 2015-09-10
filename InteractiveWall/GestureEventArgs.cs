using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveWall
{
    public class GestureEventArgs : EventArgs
    {
        public readonly string GestureName;

        public readonly float Confidence;

        public GestureEventArgs(string gestureName, float confidence)
        {
            this.GestureName = gestureName;
            this.Confidence = confidence;
        }
    }
}
