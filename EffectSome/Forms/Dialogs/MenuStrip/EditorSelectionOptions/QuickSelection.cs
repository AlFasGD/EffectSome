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
    public partial class QuickSelection : Form
    {
        public static bool IsOpen = false;
        public QuickSelection()
        {
            IsOpen = true;
            InitializeComponent();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            groupIDs1.Enabled = checkBox2.Checked;
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            groupIDs2.Enabled = checkBox3.Checked;
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }
        public void CloseForm()
        {
            Close();
        }
        private void QuickSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
        }
    }
}
