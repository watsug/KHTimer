using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KHTimerApp
{
    public partial class SetTimeForm : Form
    {
        private string _previous;

        public SetTimeForm()
        {
            InitializeComponent();
            RefreshOkButton();
        }

        private void textTime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textTime.Text.Length == 2)
                {
                    textTime.Text = Direction > 0
                        ? textTime.Text + ":" 
                        : textTime.Text.Substring(0, textTime.Text.Length - 1);
                }

                if (!VerifyPartialTime(textTime.Text))
                {
                    textTime.Text = textTime.Text.Length == 0 ? "" : textTime.Text.Substring(0, textTime.Text.Length - 1);
                }

                textTime.SelectionStart = textTime.Text.Length;
                textTime.SelectionLength = 0;
            }
            finally
            {
                _previous = textTime.Text;
                RefreshOkButton();
            }
        }

        private void RefreshOkButton()
        {
            btnOk.Enabled = VerifyPartialTime(textTime.Text) && textTime.Text.Length == 5;
        }

        private void textTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textTime.Text.Length >= 5 && char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private int Direction
        {
            get
            {
                return _previous == null ? 1 : textTime.Text.Length - _previous.Length;
            }
        }

        private bool VerifyPartialTime(string time)
        {
            try
            {
                if (time.Length == 0) return true;
                if (time.Length >= 1)
                {
                    if (!char.IsDigit(time[0])) return false;
                }
                if (time.Length >= 2)
                {
                    if (!char.IsDigit(time[1])) return false;
                    int _val = GetDigit(time[1]);
                    if (_val < 0 || _val > 9) return false;
                }
                if (time.Length >= 3)
                {
                    if (time[2] != ':') return false;
                }

                if (time.Length >= 4)
                {
                    if (!char.IsDigit(time[3])) return false;
                    int _val = GetDigit(time[3]);
                    if (_val < 0 || _val > 5) return false;
                }
                if (time.Length >= 5)
                {
                    if (!char.IsDigit(time[4])) return false;
                    int _val = GetDigit(time[4]);
                    if (_val < 0 || _val > 9) return false;
                }

                return time.Length > 5 ? false : true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private int GetDigit(char ch)
        {
            return ch - '0';
        }
    }
}
