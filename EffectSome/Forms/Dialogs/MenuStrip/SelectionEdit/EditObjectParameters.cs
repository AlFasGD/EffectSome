using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EffectSome
{
    public partial class EditObjectParameters : Form
    {
        public EditObjectParameters()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown4.Enabled = radioButton4.Checked;
        }
        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void radioButton20_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void radioButton21_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void radioButton22_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void radioButton23_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
