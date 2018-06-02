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
    public partial class ColorChannelEditor : Form
    {
        public static bool IsOpen = false;
        public ColorChannelEditor()
        {
            IsOpen = true;
            InitializeComponent();
            toolTip1.SetToolTip(pictureBox51, string.Format("#{0:X}{1:X}{2:X}", pictureBox51.BackColor.R, pictureBox51.BackColor.G, pictureBox51.BackColor.B));
            toolTip1.SetToolTip(label101, string.Format("#{0:X}{1:X}{2:X}", pictureBox51.BackColor.R, pictureBox51.BackColor.G, pictureBox51.BackColor.B));
            toolTip1.SetToolTip(pictureBox52, string.Format("#{0:X}{1:X}{2:X}", pictureBox52.BackColor.R, pictureBox52.BackColor.G, pictureBox52.BackColor.B));
            toolTip1.SetToolTip(label120, string.Format("#{0:X}{1:X}{2:X}", pictureBox52.BackColor.R, pictureBox52.BackColor.G, pictureBox52.BackColor.B));
            ChangeColorValues();
        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown5.Enabled = checkBox1.Checked;
            numericUpDown6.Enabled = checkBox1.Checked;
            numericUpDown7.Enabled = checkBox1.Checked;
            numericUpDown8.Enabled = checkBox1.Checked;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox52.Enabled = radioButton1.Checked;
            label120.Enabled = radioButton1.Checked;
            trackBar2.Enabled = radioButton1.Checked;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton6.Enabled = radioButton2.Checked;
            radioButton7.Enabled = radioButton2.Checked;
            numericUpDown20.Enabled = radioButton2.Checked && radioButton6.Checked;
            numericUpDown21.Enabled = radioButton2.Checked && radioButton6.Checked;
            numericUpDown22.Enabled = radioButton2.Checked && radioButton6.Checked;
            numericUpDown23.Enabled = radioButton2.Checked && radioButton6.Checked;
            numericUpDown24.Enabled = radioButton2.Checked && radioButton7.Checked;
            numericUpDown25.Enabled = radioButton2.Checked && radioButton7.Checked;
            numericUpDown26.Enabled = radioButton2.Checked && radioButton7.Checked;
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown19.Enabled = radioButton3.Checked;
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown17.Enabled = radioButton4.Checked;
            numericUpDown18.Enabled = radioButton4.Checked;
            label119.Enabled = radioButton4.Checked;
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown47.Enabled = radioButton5.Checked;
            button14.Enabled = radioButton5.Checked;
            button13.Enabled = radioButton5.Checked && colorIDs.SelectedItems.Count != 0;
            colorIDs.Enabled = radioButton5.Checked;
        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown20.Enabled = radioButton6.Checked;
            numericUpDown21.Enabled = radioButton6.Checked;
            numericUpDown22.Enabled = radioButton6.Checked;
            numericUpDown23.Enabled = radioButton6.Checked;
        }
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown24.Enabled = radioButton7.Checked;
            numericUpDown25.Enabled = radioButton7.Checked;
            numericUpDown26.Enabled = radioButton7.Checked;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Label[] colorChannels = { label1, label2, label3, label4, label5, label6, label7, label8, label9, label10, label11, label12, label13, label14, label15, label16, label17, label18, label19, label20, label21, label22, label23, label24, label25, label26, label27, label28, label29, label30, label31, label32, label33, label34, label35, label36, label37, label38, label39, label40, label41, label42, label43, label44, label45, label46, label47, label48, label49, label50 };
            Label[] colorsOpacity = { label51, label52, label53, label54, label55, label56, label57, label58, label59, label60, label61, label62, label63, label64, label65, label66, label67, label68, label69, label70, label71, label72, label73, label74, label75, label76, label77, label78, label79, label80, label81, label82, label83, label84, label85, label86, label87, label88, label89, label90, label91, label92, label93, label94, label95, label96, label97, label98, label99, label100 };
            PictureBox[] colors = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20, pictureBox21, pictureBox22, pictureBox23, pictureBox24, pictureBox25, pictureBox26, pictureBox27, pictureBox28, pictureBox29, pictureBox30, pictureBox31, pictureBox32, pictureBox33, pictureBox34, pictureBox35, pictureBox36, pictureBox37, pictureBox38, pictureBox39, pictureBox40, pictureBox41, pictureBox42, pictureBox43, pictureBox44, pictureBox45, pictureBox46, pictureBox47, pictureBox48, pictureBox49, pictureBox50 };
            if (ToInt32(colorChannels[colorChannels.Length - 1].Text) < 1000 || ToInt32(colorChannels[0].Text) >= 1000)
            {
                for (int i = 0; i < colorChannels.Length; i++)
                    colorChannels[i].Text = (ToInt32(colorChannels[i].Text) + 50).ToString();
                button2.Enabled = true;
                ChangeColorValues();
                colorChannels[colorChannels.Length - 1].Visible = colors[colors.Length - 1].Visible = colorsOpacity[colorsOpacity.Length - 1].Visible = !(colorChannels[colorChannels.Length - 1].Text == "1000");
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure you want to proceed into editing the special color channels with values 1000 and above?", "Caution", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                    for (int i = 0; i < colorChannels.Length; i++)
                        colorChannels[i].Text = (1000 + i).ToString();
                colorChannels[colorChannels.Length - 1].Visible = colors[colors.Length - 1].Visible = colorsOpacity[colorsOpacity.Length - 1].Visible = result == DialogResult.Yes;
            }
        }
        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
            {
                if (label50.Text != "1000")
                    button1_Click(sender, e);
            }
            else if (e.KeyCode == Keys.A)
                button2_Click(sender, e);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Label[] colorChannels = { label1, label2, label3, label4, label5, label6, label7, label8, label9, label10, label11, label12, label13, label14, label15, label16, label17, label18, label19, label20, label21, label22, label23, label24, label25, label26, label27, label28, label29, label30, label31, label32, label33, label34, label35, label36, label37, label38, label39, label40, label41, label42, label43, label44, label45, label46, label47, label48, label49, label50 };
            Label[] colorsOpacity = { label51, label52, label53, label54, label55, label56, label57, label58, label59, label60, label61, label62, label63, label64, label65, label66, label67, label68, label69, label70, label71, label72, label73, label74, label75, label76, label77, label78, label79, label80, label81, label82, label83, label84, label85, label86, label87, label88, label89, label90, label91, label92, label93, label94, label95, label96, label97, label98, label99, label100 };
            PictureBox[] colors = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20, pictureBox21, pictureBox22, pictureBox23, pictureBox24, pictureBox25, pictureBox26, pictureBox27, pictureBox28, pictureBox29, pictureBox30, pictureBox31, pictureBox32, pictureBox33, pictureBox34, pictureBox35, pictureBox36, pictureBox37, pictureBox38, pictureBox39, pictureBox40, pictureBox41, pictureBox42, pictureBox43, pictureBox44, pictureBox45, pictureBox46, pictureBox47, pictureBox48, pictureBox49, pictureBox50 };
            colorChannels[colorChannels.Length - 1].Visible = colors[colors.Length - 1].Visible = colorsOpacity[colorsOpacity.Length - 1].Visible = (colorChannels[colorChannels.Length - 1].Text == "1000");
            if (colorChannels[0].Text != "1000")
                for (int i = 0; i < colorChannels.Length; i++)
                    colorChannels[i].Text = (ToInt32(colorChannels[i].Text) - 50).ToString();
            else
            {
                for (int i = 0; i < colorChannels.Length; i++)
                    colorChannels[i].Text = (950 + i).ToString();
                colorChannels[colorChannels.Length - 1].Visible = false;
            }
            button2.Enabled = !(colorChannels[0].Text == "1");
            ChangeColorValues();
        }
        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
            {
                if (label50.Text != "1000")
                    button1_Click(sender, e);
            }
            else if (e.KeyCode == Keys.A)
                button2_Click(sender, e);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            while (listBox1.SelectedItems.Count != 0)
                listBox1.Items.Remove(listBox1.SelectedItems[listBox1.SelectedItems.Count - 1]);
        }
        private void button12_Click(object sender, EventArgs e)
        {

        }
        private void button13_Click(object sender, EventArgs e)
        {
            RemoveItems(colorIDs);
        }
        private void button14_Click(object sender, EventArgs e)
        {
            AddItem(numericUpDown47.Value, colorIDs);
        }
        private void button15_Click(object sender, EventArgs e)
        {
            int[] colorChannelsToEdit = new int[(int)Math.Max(numericUpDown18.Value - numericUpDown17.Value, colorIDs.Items.Count)];
            if (radioButton4.Checked == true)
                for (int i = 0; i <= numericUpDown18.Value - numericUpDown17.Value; i++)
                    colorChannelsToEdit[i] = (int)numericUpDown17.Value + i;
            else if (radioButton5.Checked == true)
                for (int i = 0; i < colorIDs.Items.Count; i++)
                    colorChannelsToEdit[i] = Convert.ToInt32(colorIDs.Items[i]);
            MassEditColorChannels(colorChannelsToEdit);
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

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

        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown18.Value < numericUpDown17.Value)
                numericUpDown18.Value = numericUpDown17.Value;
        }
        private void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown17.Value > numericUpDown18.Value)
                numericUpDown17.Value = numericUpDown18.Value;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Enabled = listBox1.SelectedItems.Count != 0;
        }
        private void colorIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            button13.Enabled = colorIDs.SelectedItems.Count != 0;
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label120.Text = ((double)trackBar2.Value / 100).ToString("F2");
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {
            if (label50.Text == "1000")
                button2.Focus();
            else
                button1.Focus();
        }
        private void groupBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        #region Add color channels events
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label1.Text));
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label2.Text));
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label3.Text));
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label4.Text));
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label5.Text));
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label6.Text));
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label7.Text));
        }
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label8.Text));
        }
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label9.Text));
        }
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label10.Text));
        }
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label11.Text));
        }
        private void pictureBox12_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label12.Text));
        }
        private void pictureBox13_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label13.Text));
        }
        private void pictureBox14_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label14.Text));
        }
        private void pictureBox15_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label15.Text));
        }
        private void pictureBox16_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label16.Text));
        }
        private void pictureBox17_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label17.Text));
        }
        private void pictureBox18_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label18.Text));
        }
        private void pictureBox19_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label19.Text));
        }
        private void pictureBox20_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label20.Text));
        }
        private void pictureBox21_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label21.Text));
        }
        private void pictureBox22_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label22.Text));
        }
        private void pictureBox23_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label23.Text));
        }
        private void pictureBox24_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label24.Text));
        }
        private void pictureBox25_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label25.Text));
        }
        private void pictureBox26_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label26.Text));
        }
        private void pictureBox27_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label27.Text));
        }
        private void pictureBox28_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label28.Text));
        }
        private void pictureBox29_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label29.Text));
        }
        private void pictureBox30_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label30.Text));
        }
        private void pictureBox31_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label31.Text));
        }
        private void pictureBox32_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label32.Text));
        }
        private void pictureBox33_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label33.Text));
        }
        private void pictureBox34_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label34.Text));
        }
        private void pictureBox35_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label35.Text));
        }
        private void pictureBox36_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label36.Text));
        }
        private void pictureBox37_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label37.Text));
        }
        private void pictureBox38_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label38.Text));
        }
        private void pictureBox39_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label39.Text));
        }
        private void pictureBox40_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label40.Text));
        }
        private void pictureBox41_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label41.Text));
        }
        private void pictureBox42_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label42.Text));
        }
        private void pictureBox43_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label43.Text));
        }
        private void pictureBox44_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label44.Text));
        }
        private void pictureBox45_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label45.Text));
        }
        private void pictureBox46_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label46.Text));
        }
        private void pictureBox47_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label47.Text));
        }
        private void pictureBox48_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label48.Text));
        }
        private void pictureBox49_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label49.Text));
        }
        private void pictureBox50_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label50.Text));
        }
        private void label1_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label1.Text));
        }
        private void label2_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label2.Text));
        }
        private void label3_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label3.Text));
        }
        private void label4_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label4.Text));
        }
        private void label5_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label5.Text));
        }
        private void label6_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label6.Text));
        }
        private void label7_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label7.Text));
        }
        private void label8_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label8.Text));
        }
        private void label9_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label9.Text));
        }
        private void label10_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label10.Text));
        }
        private void label11_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label11.Text));
        }
        private void label12_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label12.Text));
        }
        private void label13_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label13.Text));
        }
        private void label14_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label14.Text));
        }
        private void label15_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label15.Text));
        }
        private void label16_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label16.Text));
        }
        private void label17_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label17.Text));
        }
        private void label18_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label18.Text));
        }
        private void label19_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label19.Text));
        }
        private void label20_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label20.Text));
        }
        private void label21_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label21.Text));
        }
        private void label22_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label22.Text));
        }
        private void label23_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label23.Text));
        }
        private void label24_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label24.Text));
        }
        private void label25_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label25.Text));
        }
        private void label26_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label26.Text));
        }
        private void label27_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label27.Text));
        }
        private void label28_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label28.Text));
        }
        private void label29_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label29.Text));
        }
        private void label30_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label30.Text));
        }
        private void label31_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label31.Text));
        }
        private void label32_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label32.Text));
        }
        private void label33_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label33.Text));
        }
        private void label34_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label34.Text));
        }
        private void label35_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label35.Text));
        }
        private void label36_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label36.Text));
        }
        private void label37_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label37.Text));
        }
        private void label38_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label38.Text));
        }
        private void label39_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label39.Text));
        }
        private void label40_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label40.Text));
        }
        private void label41_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label41.Text));
        }
        private void label42_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label42.Text));
        }
        private void label43_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label43.Text));
        }
        private void label44_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label44.Text));
        }
        private void label45_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label45.Text));
        }
        private void label46_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label46.Text));
        }
        private void label47_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label47.Text));
        }
        private void label48_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label48.Text));
        }
        private void label49_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label49.Text));
        }
        private void label50_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label50.Text));
        }
        private void label51_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label1.Text));
        }
        private void label52_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label2.Text));
        }
        private void label53_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label3.Text));
        }
        private void label54_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label4.Text));
        }
        private void label55_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label5.Text));
        }
        private void label56_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label6.Text));
        }
        private void label57_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label7.Text));
        }
        private void label58_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label8.Text));
        }
        private void label59_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label9.Text));
        }
        private void label60_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label10.Text));
        }
        private void label61_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label11.Text));
        }
        private void label62_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label12.Text));
        }
        private void label63_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label13.Text));
        }
        private void label64_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label14.Text));
        }
        private void label65_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label15.Text));
        }
        private void label66_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label16.Text));
        }
        private void label67_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label17.Text));
        }
        private void label68_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label18.Text));
        }
        private void label69_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label19.Text));
        }
        private void label70_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label20.Text));
        }
        private void label71_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label21.Text));
        }
        private void label72_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label22.Text));
        }
        private void label73_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label23.Text));
        }
        private void label74_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label24.Text));
        }
        private void label75_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label25.Text));
        }
        private void label76_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label26.Text));
        }
        private void label77_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label27.Text));
        }
        private void label78_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label28.Text));
        }
        private void label79_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label29.Text));
        }
        private void label80_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label30.Text));
        }
        private void label81_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label31.Text));
        }
        private void label82_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label32.Text));
        }
        private void label83_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label33.Text));
        }
        private void label84_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label34.Text));
        }
        private void label85_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label35.Text));
        }
        private void label86_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label36.Text));
        }
        private void label87_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label37.Text));
        }
        private void label88_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label38.Text));
        }
        private void label89_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label39.Text));
        }
        private void label90_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label40.Text));
        }
        private void label91_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label41.Text));
        }
        private void label92_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label42.Text));
        }
        private void label93_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label43.Text));
        }
        private void label94_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label44.Text));
        }
        private void label95_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label45.Text));
        }
        private void label96_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label46.Text));
        }
        private void label97_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label47.Text));
        }
        private void label98_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label48.Text));
        }
        private void label99_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label49.Text));
        }
        private void label100_Click(object sender, EventArgs e)
        {
            AddColorChannel(Convert.ToInt32(label50.Text));
        }
        #endregion
        private void pictureBox52_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
                pictureBox52.BackColor = colorDialog1.Color;
            toolTip1.SetToolTip(pictureBox52, string.Format("#{0:X}{1:X}{2:X}", pictureBox52.BackColor.R, pictureBox52.BackColor.G, pictureBox52.BackColor.B));
            toolTip1.SetToolTip(label120, string.Format("#{0:X}{1:X}{2:X}", pictureBox52.BackColor.R, pictureBox52.BackColor.G, pictureBox52.BackColor.B));
        }
        private void label120_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
                pictureBox52.BackColor = colorDialog1.Color;
            toolTip1.SetToolTip(pictureBox52, string.Format("#{0:X}{1:X}{2:X}", pictureBox52.BackColor.R, pictureBox52.BackColor.G, pictureBox52.BackColor.B));
            toolTip1.SetToolTip(label120, string.Format("#{0:X}{1:X}{2:X}", pictureBox52.BackColor.R, pictureBox52.BackColor.G, pictureBox52.BackColor.B));
        }

        void AddColorChannel(int colorChannel)
        {
            if (!listBox1.Items.Contains(colorChannel))
            {
                if (listBox1.Items.Count != 0)
                {
                    if (colorChannel < (int)listBox1.Items[listBox1.Items.Count - 1])
                    {
                        for (int i = 0; i < listBox1.Items.Count; i++)
                        {
                            if (colorChannel < (int)listBox1.Items[i])
                            {
                                listBox1.Items.Insert(i, colorChannel);
                                break;
                            }
                        }
                    }
                    else
                        listBox1.Items.Add(colorChannel);
                }
                else
                    listBox1.Items.Add(colorChannel);
            }
            toolTip1.SetToolTip(pictureBox51, string.Format("#{0:X}{1:X}{2:X}", pictureBox51.BackColor.R, pictureBox51.BackColor.G, pictureBox51.BackColor.B));
            toolTip1.SetToolTip(label101, string.Format("#{0:X}{1:X}{2:X}", pictureBox51.BackColor.R, pictureBox51.BackColor.G, pictureBox51.BackColor.B));
        }
        void AddItem(decimal ID, ListBox listBox)
        {
            if (listBox.Items.Contains(ID) == false)
            {
                if (listBox.Items.Count != 0)
                {
                    if (ID < (decimal)listBox.Items[listBox.Items.Count - 1])
                    {
                        for (int i = 0; i < listBox.Items.Count; i++)
                        {
                            if (ID < (decimal)listBox.Items[i])
                            {
                                listBox.Items.Insert(i, ID);
                                break;
                            }
                        }
                    }
                    else
                        listBox.Items.Add(ID);
                }
                else
                    listBox.Items.Add(ID);
            }
        }
        void RemoveItems(ListBox listBox)
        {
            while (listBox.SelectedItems.Count != 0)
                listBox.Items.Remove(listBox.SelectedItems[listBox.SelectedItems.Count - 1]);
        }
        void ChangeColorValues()
        {
            Label[] colorChannels = { label1, label2, label3, label4, label5, label6, label7, label8, label9, label10, label11, label12, label13, label14, label15, label16, label17, label18, label19, label20, label21, label22, label23, label24, label25, label26, label27, label28, label29, label30, label31, label32, label33, label34, label35, label36, label37, label38, label39, label40, label41, label42, label43, label44, label45, label46, label47, label48, label49, label50 };
            Label[] colorsOpacity = { label51, label52, label53, label54, label55, label56, label57, label58, label59, label60, label61, label62, label63, label64, label65, label66, label67, label68, label69, label70, label71, label72, label73, label74, label75, label76, label77, label78, label79, label80, label81, label82, label83, label84, label85, label86, label87, label88, label89, label90, label91, label92, label93, label94, label95, label96, label97, label98, label99, label100 };
            PictureBox[] colors = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20, pictureBox21, pictureBox22, pictureBox23, pictureBox24, pictureBox25, pictureBox26, pictureBox27, pictureBox28, pictureBox29, pictureBox30, pictureBox31, pictureBox32, pictureBox33, pictureBox34, pictureBox35, pictureBox36, pictureBox37, pictureBox38, pictureBox39, pictureBox40, pictureBox41, pictureBox42, pictureBox43, pictureBox44, pictureBox45, pictureBox46, pictureBox47, pictureBox48, pictureBox49, pictureBox50 };
            int red = 255;
            int green = 255;
            int blue = 255;
            double opacity = 1.00;
            for (int i = 0; i < colorChannels.Length; i++)
            {
                // Add code to read the color values of the color channels in-game
                colors[i].BackColor = Color.FromArgb(red, green, blue);
                colorsOpacity[i].Text = opacity.ToString("F2");
                toolTip1.SetToolTip(colorChannels[i], "Color " + ToInt32(colorChannels[i].Text) + string.Format(" - #{0:X}{1:X}{2:X}", colors[i].BackColor.R, colors[i].BackColor.G, colors[i].BackColor.B) + " (" + colors[i].BackColor.ToKnownColor().ToString() + ")");
                toolTip1.SetToolTip(colors[i], "Color " + ToInt32(colorChannels[i].Text) + string.Format(" - #{0:X}{1:X}{2:X}", colors[i].BackColor.R, colors[i].BackColor.G, colors[i].BackColor.B) + " (" + colors[i].BackColor.ToKnownColor().ToString() + ")");
                toolTip1.SetToolTip(colorsOpacity[i], "Color " + ToInt32(colorChannels[i].Text) + string.Format(" - #{0:X}{1:X}{2:X}", colors[i].BackColor.R, colors[i].BackColor.G, colors[i].BackColor.B) + " (" + colors[i].BackColor.ToKnownColor().ToString() + ")");
            }
        }
        void MassEditColorChannels(int[] IDs)
        {
            if (radioButton1.Checked)
            {
                Color ARGB = Color.FromArgb((int)(Convert.ToDouble(label120.Text) * 256), pictureBox52.BackColor.R, pictureBox52.BackColor.G, pictureBox52.BackColor.B);
                for (int i = 0; i < IDs.Length; i++)
                {
                    // Do stuff
                }
            }
            else if (radioButton2.Checked)
            {
                Color ARGB = Color.FromArgb((int)numericUpDown20.Value, (int)numericUpDown21.Value, (int)numericUpDown22.Value, (int)numericUpDown23.Value);
                Color HSV = ColorFromHSV((double)numericUpDown24.Value, (double)numericUpDown25.Value, (double)numericUpDown26.Value);
                for (int i = 0; i < IDs.Length; i++)
                {
                    // Do stuff
                }
            }
            else if (radioButton3.Checked)
            {
                int copiedColorChannel = (int)numericUpDown19.Value;
                for (int i = 0; i < IDs.Length; i++)
                {
                    // Do stuff
                }
            }
        }

        #region ARGB and HSV stuff
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(v, t, p);
            else if (hi == 1)
                return Color.FromArgb(q, v, p);
            else if (hi == 2)
                return Color.FromArgb(p, v, t);
            else if (hi == 3)
                return Color.FromArgb(p, q, v);
            else if (hi == 4)
                return Color.FromArgb(t, p, v);
            else
                return Color.FromArgb(v, p, q);
        }
        void HSVToRGB(double h, double S, double V, out int r, out int g, out int b)
        {
            double H = h;
            while (H < 0)
                H += 360;
            while (H >= 360)
                H -= 360;
            double R, G, B;
            if (V <= 0)
                R = G = B = 0;
            else if (S <= 0)
                R = G = B = V;
            else
            {
                double hf = H / 60;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {
                    case 0:
                        {
                            R = V;
                            G = tv;
                            B = pv;
                            break;
                        }
                    case 1:
                        {
                            R = qv;
                            G = V;
                            B = pv;
                            break;
                        }
                    case 2:
                        {
                            R = pv;
                            G = V;
                            B = tv;
                            break;
                        }
                    case 3:
                        {
                            R = pv;
                            G = qv;
                            B = V;
                            break;
                        }
                    case 4:
                        {
                            R = tv;
                            G = pv;
                            B = V;
                            break;
                        }
                    case 5:
                        {
                            R = V;
                            G = pv;
                            B = qv;
                            break;
                        }
                    case 6:
                        {
                            R = V;
                            G = tv;
                            B = pv;
                            break;
                        }
                    case -1:
                        {
                            R = V;
                            G = pv;
                            B = qv;
                            break;
                        }
                    default:
                        {
                            R = G = B = V; // Just pretend it's black/white
                            break;
                        }
                }
            }
            r = Clamp((int)(R * 255));
            g = Clamp((int)(G * 255));
            b = Clamp((int)(B * 255));
        }
        /// <summary>Clamp a value to [0, 255].</summary>
        int Clamp(int i)
        {
            if (i < 0)
                i = 0;
            else if (i > 255)
                i = 255;
            return i;
        }
        #endregion

        private void ColorChannelEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
        }
    }
}