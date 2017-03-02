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
        public SetTimeForm()
        {
            InitializeComponent();
        }

        private void textTime_TextChanged(object sender, EventArgs e)
        {
            if (textTime.Text.Length == 2)
            {
                textTime.Text += ':';
            }
        }

        private void textTime_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void textTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
