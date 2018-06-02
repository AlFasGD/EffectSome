using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EffectSome
{
    public partial class LimitBypasses : Form
    {
        public static bool IsOpen = false;
        public LimitBypasses()
        {
            IsOpen = true;
            Process process = Process.GetProcessesByName("GeometryDash").FirstOrDefault();
            if (process != null)
            {
                int objLim1 = BitConverter.ToInt32(MemoryEdit.ReadMemory(0x4EF1AF, 4, (int)EffectSome.processHandle), 0);
                int objLim2 = BitConverter.ToInt32(MemoryEdit.ReadMemory(0x469903, 4, (int)EffectSome.processHandle), 0);
                int objLim3 = BitConverter.ToInt32(MemoryEdit.ReadMemory(0x46B896, 4, (int)EffectSome.processHandle), 0);
                int objLim4 = BitConverter.ToInt32(MemoryEdit.ReadMemory(0x46BBFA, 4, (int)EffectSome.processHandle), 0);
            }
            InitializeComponent();
        }

        private void LimitBypasses_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        } // Character limit
        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            numericUpDown4.Enabled = checkBox1.Checked;
        } // Bypass text objects' character limit
        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {

        } // Bypass buttons per row limit
        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {

        } // Bypass button rows limit
        private void checkBox4_CheckStateChanged(object sender, EventArgs e)
        {

        } // Bypass zoom in/out limit
        private void checkBox6_CheckStateChanged(object sender, EventArgs e)
        {

        } // Guidelines limit
        private void checkBox8_CheckStateChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = checkBox8.Checked;
            if (checkBox8.CheckState == CheckState.Indeterminate)
                DLLs.LimitBypasses.BypassCustomObjectLimit((int)numericUpDown2.Value);
            else if (checkBox8.CheckState == CheckState.Checked)
                DLLs.LimitBypasses.BypassCustomObjectLimit((int)numericUpDown2.Value);
            else
                DLLs.LimitBypasses.BypassCustomObjectLimit((int)numericUpDown2.Value);
        } // Bypass custom object limit
        private void checkBox9_CheckStateChanged(object sender, EventArgs e)
        {
            numericUpDown3.Visible = checkBox9.Checked;
            if (checkBox9.CheckState == CheckState.Indeterminate)
                DLLs.LimitBypasses.BypassCustomObjectsObjectLimit((int)numericUpDown3.Value);
            else if (checkBox9.CheckState == CheckState.Checked)
                DLLs.LimitBypasses.BypassCustomObjectsObjectLimit((int)numericUpDown3.Value);
            else
                DLLs.LimitBypasses.BypassCustomObjectsObjectLimit((int)numericUpDown3.Value);
        } // Bypass custom objects' object limit
        private void checkBox10_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox10.CheckState == CheckState.Indeterminate)
            {
                DialogResult result = MessageBox.Show("WARNING: Bypassing the object limit means that you are able to add as many objects as you want above the object limit possibly resulting in lag on low-end devices!\n\nAre you sure you want to bypass the object limit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    checkBox10.Checked = false;
                else if (result == DialogResult.Yes)
                {
                    numericUpDown1.Visible = true;
                    DLLs.LimitBypasses.BypassObjectLimit((int)numericUpDown1.Value);
                }
            }
            else if (checkBox10.CheckState == CheckState.Checked)
            {
                DialogResult result = MessageBox.Show("WARNING: Bypassing the object limit means that you are able to add as many objects as you want above the object limit possibly resulting in lag on low-end devices!\n\nAre you sure you want to bypass the object limit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    checkBox10.Checked = false;
                else if (result == DialogResult.Yes)
                {
                    numericUpDown1.Visible = true;
                    DLLs.LimitBypasses.BypassObjectLimit();
                }
            }
            else
                DLLs.LimitBypasses.RestoreObjectLimit();
        } // Bypass object limit
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            MemoryEdit.WriteMemory(0x4EF1AF, BitConverter.GetBytes((int)numericUpDown1.Value), (int)EffectSome.processHandle);
            MemoryEdit.WriteMemory(0x469903, BitConverter.GetBytes((int)numericUpDown1.Value), (int)EffectSome.processHandle);
            MemoryEdit.WriteMemory(0x46B896, BitConverter.GetBytes((int)numericUpDown1.Value), (int)EffectSome.processHandle);
            MemoryEdit.WriteMemory(0x46BBFA, BitConverter.GetBytes((int)numericUpDown1.Value), (int)EffectSome.processHandle);
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown5.Value > numericUpDown6.Value)
                numericUpDown6.Value = numericUpDown5.Value;
        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown5.Value > numericUpDown6.Value)
                numericUpDown5.Value = numericUpDown6.Value;
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown8.Value > numericUpDown7.Value)
                numericUpDown8.Value = numericUpDown7.Value;
        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown8.Value > numericUpDown7.Value)
                numericUpDown7.Value = numericUpDown8.Value;
        }

        public void CloseForm()
        {
            Close();
        }
    }
}
