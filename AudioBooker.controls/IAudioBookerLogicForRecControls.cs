using Miktemk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audiobooker.controls
{
    public interface IAudioBookerLogicForRecControls
    {
        void startRecording();
        void stopRecording();
        event TimeSpanHandler SegmentTimeUpdated;
    }
}
