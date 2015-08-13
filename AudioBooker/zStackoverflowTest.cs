using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace AudioBooker {
    public partial class zStackoverflowTest : Form {
        private WaveIn waveIn;
        private WaveFileWriter writer;
        String outputFilename = @"c:\test.wav";

        public zStackoverflowTest()
        {
            InitializeComponent();

            int waveInDevices = WaveIn.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++) {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                txtArea.AppendText(String.Format("Device {0}: {1}, {2} channels\n", waveInDevice, deviceInfo.ProductName, deviceInfo.Channels));
            }

            waveIn = null;
        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            //writer.WriteData(e.Buffer, 0, e.BytesRecorded);
            writer.Write(e.Buffer, 0, e.BytesRecorded);
        }

        int iii = 0;

        private void button1_Click_1(object sender, EventArgs e) {
            if (waveIn == null) {
                waveIn = new WaveIn();
                int sampleRate = 22000;
                int channels = 2;
                waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
                waveIn.DeviceNumber = 2;
                waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
                iii++;
                var fname = "test" + iii + ".wav";
                writer = new WaveFileWriter(fname, waveIn.WaveFormat);
                waveIn.StartRecording();
                button1.Text = "stop " + fname;
            }
            else {
                waveIn.StopRecording();
                waveIn.Dispose();
                writer.Close();
                waveIn = null;
                writer = null;
                button1.Text = "start";
            }
        }
    }
}
