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
    public partial class SegmentProgressView : UserControl {
        private XmlWavEvent _curSegment;
        public SegmentProgressView() {
            InitializeComponent();
            updateUI();
        }

        public XmlWavEvent CurSegment {
            get {
                return _curSegment;
            }
            set {
                _curSegment = value;
                updateUI();
            }
        }

        private void updateUI() {
            if (CurSegment == null) {
                lblIndicator.Text = "---";
                lblIndicator.BackColor = Color.White;
                lblIndicator.ForeColor = Color.Gray;
                progressBar.Value = 0;
            }
            else {
                lblIndicator.Text = "Listening!";
                lblIndicator.BackColor = Color.Red;
                lblIndicator.ForeColor = Color.White;
                progressBar.Value = 100;
            }
        }

    }
}
