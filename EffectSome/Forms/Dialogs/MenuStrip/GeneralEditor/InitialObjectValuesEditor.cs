using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Convert;

namespace EffectSome
{
    public partial class InitialObjectValuesEditor : Form
    {
        public InitialObjectValuesEditor()
        {
            InitializeComponent();
        }

        #region CheckBoxes
        private void checkBox1_CheckedChanged(object sender, EventArgs e) => numericUpDown1.Enabled = !checkBox1.Checked;
        private void checkBox2_CheckedChanged(object sender, EventArgs e) => numericUpDown2.Enabled = !checkBox2.Checked;
        private void checkBox4_CheckedChanged(object sender, EventArgs e) => groupBox2.Enabled = checkBox4.Checked;
        private void checkBox19_CheckedChanged(object sender, EventArgs e) => numericUpDown17.Minimum = (numericUpDown17.Maximum = 2 - ToInt32(checkBox19.Checked)) - 2;
        private void checkBox20_CheckedChanged(object sender, EventArgs e) => numericUpDown16.Minimum = (numericUpDown16.Maximum = 2 - ToInt32(checkBox20.Checked)) - 2;
        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox22.Checked && !checkBox23.Checked)
                checkBox23.Checked = true;
        }
        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox22.Checked && !checkBox23.Checked)
                checkBox22.Checked = true;
        }
        private void checkBox74_CheckedChanged(object sender, EventArgs e) => numericUpDown61.Minimum = (numericUpDown61.Maximum = 2 - ToInt32(checkBox74.Checked)) - 2;
        private void checkBox75_CheckedChanged(object sender, EventArgs e) => numericUpDown62.Minimum = (numericUpDown62.Maximum = 2 - ToInt32(checkBox75.Checked)) - 2;
        private void checkBox79_CheckedChanged(object sender, EventArgs e) => groupBox46.Enabled = checkBox79.Checked;
        #endregion
        #region RadioButtons
        private void radioButton3_CheckedChanged(object sender, EventArgs e) => checkBox22.Enabled = checkBox23.Enabled = radioButton3.Checked;
        private void radioButton17_CheckedChanged(object sender, EventArgs e) => groupBox55.Enabled = radioButton17.Checked;
        private void radioButton18_CheckedChanged(object sender, EventArgs e) => groupBox56.Enabled = radioButton18.Checked;
        private void radioButton27_CheckedChanged(object sender, EventArgs e) => numericUpDown26.Enabled = radioButton27.Checked;
        #endregion
        #region Buttons
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = pulseColorPick.ShowDialog();
            if (result == DialogResult.OK)
            {
                numericUpDown15.Value = pulseColorPick.Color.R;
                numericUpDown13.Value = pulseColorPick.Color.G;
                numericUpDown14.Value = pulseColorPick.Color.B;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = colorTriggerColorPick.ShowDialog();
            if (result == DialogResult.OK)
            {
                numericUpDown66.Value = colorTriggerColorPick.Color.R;
                numericUpDown64.Value = colorTriggerColorPick.Color.G;
                numericUpDown65.Value = colorTriggerColorPick.Color.B;
            }
        }
        #endregion
    }
}