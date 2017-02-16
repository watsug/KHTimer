using System;
using System.Windows.Forms;

namespace KHTimerApp
{
    public partial class TimerForm : Form
    {
        private int _timer;
        private TimerLib timerObj = null;

        public TimerForm()
        {
            InitializeComponent();
        }

        private int _each5s = 0;
        private void timer1s_Tick(object sender, EventArgs e)
        {
            _timer += 1;
            RefreshTime();

            _each5s += 1;
            if (_each5s%5 == 0)
            {
                Sync();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (timer1s.Enabled)
            {
                timer1s.Stop();
                if (null != timerObj) timerObj.Stop();
            }
            else
            {
                timer1s.Start();
                if (null != timerObj) timerObj.Start();
            }
            btnStart.Text = timer1s.Enabled ? "STOP" : "START";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _timer = 0;
            RefreshTime();
            timer1s.Start();
            if (null != timerObj) timerObj.Reset();
            btnStart.Text = timer1s.Enabled ? "STOP" : "START";
        }

        private void RefreshTime()
        {
            lblTime.Text = string.Format("{0:d2}:{1:d2}", _timer/60, _timer%60);
        }

        private void TimerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show("Do you really want to CLOSE this application?", "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (res != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void TimerForm_Load(object sender, EventArgs e)
        {
            timerObj = TimerLib.Seek();
            Sync();
        }

        private void Sync()
        {
            if (timerObj != null)
            {
                this.Text = "KHTimer: " + timerObj.Name;
                var _time = timerObj.Time();
                _timer = _time.Item2;
                RefreshTime();
                switch (_time.Item1)
                {
                    case "run":
                        timer1s.Start();
                        break;
                    case "stop":
                        timer1s.Stop();
                        break;
                }
            }
            btnStart.Text = timer1s.Enabled ? "STOP" : "START";
        }
    }
}