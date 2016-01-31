using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioBooker.classes;

namespace Audiobooker.controls {

    public partial class RecordingControls : UserControl, IRecorder
    {

        private IRecorderState state;
        private AudioRecManager recMan;
        private Form parentForm;
        private IAudioBookerLogicForRecControls logicShit;

        public RecordingControls() {
            InitializeComponent();

            state = IRecorderState.Idle;
            recMan = new AudioRecManager();

            focusDiverter.GotFocus += new EventHandler(focusDiverterChanged);
            focusDiverter.LostFocus += new EventHandler(focusDiverterChanged);

            recMan.DeviceNumber = Devices.SelectedIndex;

            // call from frame level
            //PopulateDevices();
        }
        public void setForm(Form parentForm) {
            this.parentForm = parentForm;
        }
        public void ForceFocusDiversion() {
            focusDiverter.Focus();
            parentForm.BackColor = Color.Gray;
        }
        public void setLogic(IAudioBookerLogicForRecControls logicShit)
        {
            this.logicShit = logicShit;
            logicShit.SegmentTimeUpdated += (TimeSpan ts) =>
            {
                lblTime.Text = ts.ToString(@"m\:ss");
            };
        }

        #region controls and shit

        private void focusDiverterChanged(object sender, EventArgs e) {
            if (focusDiverter.Focused)
                parentForm.BackColor = Color.Gray;
            else
                parentForm.BackColor = SystemColors.Control;
        }
        private void btnRec_Click_1(object sender, EventArgs e) {
            if (state == IRecorderState.Idle) {
                if (Devices.SelectedItem == null) {
                    MessageBox.Show("Please select a device", "Device not selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                state = IRecorderState.Recording;
                ForceFocusDiversion();
                btnRec.Text = "Stop";
                btnRec.BackColor = Color.LightBlue;
                if (logicShit != null)
                    logicShit.startRecording();
            }
            else if (state == IRecorderState.Recording) {

                if (logicShit != null)
                    logicShit.stopRecording();
                btnRec.Text = "Rec";
                btnRec.BackColor = Color.Red;
                state = IRecorderState.Idle;
            }
        }

        public void PopulateDevices() {
            foreach (var device in recMan.Devices) {
                Devices.Items.Add(device.Key);
            }
            var someDef = getSomeDefaultMic();
            if (someDef != null)
                Devices.SelectedItem = someDef;
        }

        private object getSomeDefaultMic() {
            return recMan.Devices.Keys.FirstOrDefault(x => x.ToLower().Contains("plantronics"))
                ?? recMan.Devices.Keys.FirstOrDefault(x => x.ToLower().Contains("internal"))
                ?? recMan.Devices.Keys.FirstOrDefault(x => x.ToLower().Contains("microphone"))
                ?? recMan.Devices.Keys.FirstOrDefault(x => x.ToLower().Contains("mic"))
                ?? null;
        }

        private void Devices_SelectedIndexChanged_1(object sender, EventArgs e) {
            SelectedDevice = recMan.Devices[Devices.SelectedItem.ToString()];
            recMan.DeviceNumber = Devices.SelectedIndex;
        }


        #endregion

        #region properties

        public NAudio.Wave.WaveInCapabilities SelectedDevice { get; set; }
        public AudioRecManager RecMan { get { return recMan; } }
        public IRecorderState State {
            get {
                return state;
            }
        }

        #endregion
    }
}
