using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3SplitterCommon;

namespace AudioBooker.classes
{
    public enum IRecorderState {
        Idle = 1,
        Recording = 2,
        Paused = 3,
    }
    public interface IRecorder {
        IRecorderState State { get; }
    }
}
