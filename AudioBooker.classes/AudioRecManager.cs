using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AudioBooker.classes
{
    public class AudioRecManager
    {
        private WaveIn waveIn;
        private WaveFileWriter writer;

        //TODO: this needs an output folder
        public AudioRecManager() {
            TmpFilename = null;
            DeviceNumber = 0;
        }

        public string TmpFilename { get; private set; }

        // TODO: pass sampleRate and channels as parameters
        public void StartRecording() {
            if (waveIn != null) // already recording!!!
                return;
            waveIn = new WaveIn();
            int sampleRate = 22000;
            int channels = 2;
            waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
            waveIn.DeviceNumber = DeviceNumber;
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
            TmpFilename = String.Format(".{0}.wav", Guid.NewGuid().ToString().Replace("-", ""));
            writer = new WaveFileWriter(TmpFilename, waveIn.WaveFormat);
            waveIn.StartRecording();
        }
        private void waveIn_DataAvailable(object sender, WaveInEventArgs e) {
            //writer.WriteData(e.Buffer, 0, e.BytesRecorded);
            writer.Write(e.Buffer, 0, e.BytesRecorded);
        }
        public void StopRecording() {
            if (waveIn == null)
                return;
            waveIn.StopRecording();
            waveIn.Dispose();
            writer.Close();
            waveIn = null;
            writer = null;
        }
        public void DisposeOfLastRecording() {
            if (TmpFilename == null)
                return;
            var fnameSrc = TmpFilename;
            TmpFilename = null;
            new Thread(() => {
                File.Delete(fnameSrc);
            }).Start();
        }
        public void SaveLastRecording(string filename) {
            if (TmpFilename == null)
                return;
            var fnameSrc = TmpFilename;
            TmpFilename = null;
            new Thread(() => {
                if (File.Exists(filename))
                    File.Delete(filename);
                var dir = Directory.GetParent(filename).FullName;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.Move(fnameSrc, filename);
            }).Start();
        }

        public TimeSpan LastRecordingLength {
            get {
                return TimeSpan.Zero;
            }
        }

        private Dictionary<string, WaveInCapabilities> _devices = null;
        public Dictionary<string, WaveInCapabilities> Devices {
            get {
                if (_devices != null)
                    return _devices;
                _devices = new Dictionary<string, WaveInCapabilities>();
                int waveInDevices = WaveIn.DeviceCount;
                for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++) {
                    WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                    _devices.Add(deviceInfo.ProductName, deviceInfo);
                    //txtArea.AppendText(String.Format("Device {0}: {1}, {2} channels\n", waveInDevice, deviceInfo.ProductName, deviceInfo.Channels));
                }
                return _devices;
            }
        }

        public int DeviceNumber { get; set; }
    }
}
