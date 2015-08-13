//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AudioBooker.classes {
//    /// <summary>
//    /// Responsible for running Mp3 recording (mp3_stream.exe) as well as lame encoding (lame.exe)
//    /// </summary>
//    public class ExternalProcessManager : IDisposable
//    {
//        private string workingDir;
//        private Process RecordingProc;
//        private bool recordingFinished;
//        private DateTime RecordingProc_StartTime_cache;
//        //private const int TimerFudgeFactor = 3;

//        public ExternalProcessManager(string workingDir) {
//            this.workingDir = workingDir;
//            recordingFinished = false;
//            RecordingProc_StartTime_cache = DateTime.MinValue;
//        }

//        private void InitiateMP3StreamProcess(Process proc, string arguments) {
//            proc.StartInfo.CreateNoWindow = true;
//            proc.StartInfo.WorkingDirectory = workingDir;
//            proc.StartInfo.FileName = "External/mp3_stream.exe";
//            proc.StartInfo.Arguments = arguments;
//            proc.StartInfo.UseShellExecute = false;
//            proc.StartInfo.RedirectStandardOutput = true;
//            proc.StartInfo.RedirectStandardError = true;
//            proc.StartInfo.RedirectStandardInput = true;
//            proc.EnableRaisingEvents = true;
//            proc.Start();
//        }

//        public void Dispose() {
//            StopRecording(null);
//        }

//        public List<string> ExecuteMp3Stream(string command) {
//            Process devicesProc = new Process();
//            InitiateMP3StreamProcess(devicesProc, command);
//            devicesProc.WaitForExit();

//            List<string> response = new List<string>();
//            string line;
//            while ((line = devicesProc.StandardOutput.ReadLine()) != null) {
//                response.Add(line);
//            }

//            return response;
//        }

//        public void StartRecording(string selectedDevice, string selectedLine, string volume, string bitRate) {
//            string args = string.Format("-device=\"{0}\" -line=\"{1}\" -v={2} -br={3} -sr=32000",
//                                            selectedDevice,
//                                            //Devices.SelectedItem.ToString(),
//                                            selectedLine,
//                                            //Lines.SelectedItem.ToString(),
//                                            volume,
//                                            bitRate);
//            RecordingProc = new Process();
//            InitiateMP3StreamProcess(RecordingProc, args);
//        }

//        public void StopRecording(Action whenDoneSaving = null) {
//            if (RecordingProc == null)
//                return;
            
//            //RecordingProc.Exited += (sender, e) => {
//            //};
//            recordingFinished = true;
//            RecordingProc_StartTime_cache = RecordingProc.StartTime;
//            System.Threading.Thread.Sleep(2000);
//            RecordingProc.Kill();
//            RecordingProc.Close();
//            RecordingProc = null;
//            if (whenDoneSaving != null)
//                whenDoneSaving();
//        }

//        public void ConvertWavToMp3(string wavFile, string mp3File) {
//            string args = string.Format("-b 128 \"{0}\" \"{1}\" ", wavFile, mp3File);
//            var proc = new Process();
//            proc.StartInfo.CreateNoWindow = true;
//            proc.StartInfo.WorkingDirectory = workingDir;
//            proc.StartInfo.FileName = "External/lame.exe";
//            proc.StartInfo.Arguments = args;
//            proc.StartInfo.UseShellExecute = false;
//            proc.StartInfo.RedirectStandardOutput = true;
//            proc.StartInfo.RedirectStandardError = true;
//            proc.StartInfo.RedirectStandardInput = true;
//            proc.Start();
//        }

//        #region propertaes

//        public DateTime Mp3ProcessStartTime {
//            get {
//                if (RecordingProc == null)
//                    return DateTime.MinValue;
//                if (recordingFinished)
//                    return RecordingProc_StartTime_cache;
//                return RecordingProc.StartTime;
//            }
//        }
//        private Dictionary<string, List<string>> _devicesAndLines = null;
//        public Dictionary<string, List<string>> DevicesAndLines {
//            get {
//                if (_devicesAndLines != null)
//                    return _devicesAndLines;
//                // if not, need to initialize
//                _devicesAndLines = new Dictionary<string, List<string>>();
//                List<string> devices = ExecuteMp3Stream("-devices");
//                foreach (string device in devices) {
//                    _devicesAndLines.Add(device, new List<string>());
//                    List<string> lines = ExecuteMp3Stream(string.Format("-device=\"{0}\"", device));
//                    foreach (string line in lines) {
//                        _devicesAndLines[device].Add(line);
//                    }
//                }
//                return _devicesAndLines;
//            }
//        }

//        #endregion
//    }
//}
