using System;
using System.Drawing;
using System.Windows.Forms;
using NLog;

namespace KHTimerApp
{
    public partial class TimerForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private int _timer;
        private TimerLib timerObj = null;

        public TimerForm()
        {
            InitializeComponent();
        }


        #region timers
        private void timer1s_Tick(object sender, EventArgs e)
        {
            try
            {
                _timer += 1;
                RefreshTime();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "timer1s_Tick()");                
            }
        }

        private void timer5s_Tick(object sender, EventArgs e)
        {
            try
            {
                Sync();
                RefreshTime();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "timer5s_Tick()");
            }
        }
        #endregion

        #region private
        private void RefreshTime()
        {
            lblTime.ForeColor = (timerObj == null) ? Color.Red : Color.Lime;
            lblTime.Text = string.Format("{0:d2}:{1:d2}", _timer / 60, _timer % 60);
        }

        private void Sync()
        {
            try
            {
                if (timerObj != null)
                {
                    ReSync();
                }
                if (timerObj == null)
                {
                    timerObj = TimerLib.Seek();
                    if (null != timerObj)
                    {
                        ReSync();
                    }
                }
                btnStart.Text = timer1s.Enabled ? "STOP" : "START";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Cannot sync with the timer!");
                throw;
            }
        }

        private void ReSync()
        {
            try
            {
                if (!timerObj.IsOpen)
                {
                    timerObj = null;
                    this.Text = "KHTimer: ?";
                }
                else
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
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Cannot re-sync with the timer!");
                throw;
            }
        }

        #endregion

        #region UI Handlers
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Info("btnStart_Click()");
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
            catch (Exception ex)
            {
                logger.Error(ex, "btnStart_Click()");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Info("btnReset_Click()");
                _timer = 0;
                RefreshTime();
                timer1s.Start();
                if (null != timerObj) timerObj.Reset();
                btnStart.Text = timer1s.Enabled ? "STOP" : "START";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "btnStart_Click()");
            }
        }

        private void TimerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                logger.Info("TimerForm_FormClosing()");
                var res = MessageBox.Show("Do you really want to CLOSE this application?", "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "TimerForm_FormClosing()");
            }
        }

        private void TimerForm_Load(object sender, EventArgs e)
        {
            try
            {
                timerObj = TimerLib.Seek();
                Sync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "TimerForm_Load()");
            }
        }
        #endregion

        private void lblTime_DoubleClick(object sender, EventArgs e)
        {
            SetTimeForm setTime = new SetTimeForm();
            if (setTime.ShowDialog(this) == DialogResult.OK)
            {
                // Set time
            }
        }
    }
}