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
    public partial class TriggersSelectionMenu : Form
    {
        public static bool IsOpen = false;
        public TriggersSelectionMenu()
        {
            IsOpen = true;
            InitializeComponent();
            comboBox2.Text = "None";
        }

        private void TriggersSelectionMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
        }
        private void TriggersSelectionMenu_Load(object sender, EventArgs e)
        {

        }

        #region CheckBoxes
        private void checkBox1_CheckedChanged(object sender, EventArgs e) => radioButton3.Enabled = radioButton4.Enabled = checkBox1.Checked;
        private void checkBox2_CheckedChanged(object sender, EventArgs e) => radioButton5.Enabled = radioButton7.Enabled = checkBox2.Checked;
        private void checkBox3_CheckedChanged(object sender, EventArgs e) => numericUpDown11.Enabled = checkBox3.Checked;
        private void checkBox4_CheckedChanged(object sender, EventArgs e) => numericUpDown13.Enabled = checkBox4.Checked;
        private void checkBox5_CheckedChanged(object sender, EventArgs e) => numericUpDown12.Enabled = checkBox5.Checked;
        private void checkBox8_CheckedChanged(object sender, EventArgs e) => numericUpDown2.Enabled = checkBox8.Checked;
        private void checkBox9_CheckedChanged(object sender, EventArgs e) => checkBox31.Enabled = checkBox32.Enabled = checkBox9.Checked;
        private void checkBox10_CheckedChanged(object sender, EventArgs e) => numericUpDown1.Enabled = checkBox10.Checked;
        private void checkBox11_CheckedChanged(object sender, EventArgs e) => numericUpDown5.Enabled = checkBox11.Checked;
        private void checkBox12_CheckedChanged(object sender, EventArgs e) => checkBox10.Enabled = checkBox11.Enabled = checkBox12.Checked;
        private void checkBox13_CheckedChanged(object sender, EventArgs e) => groupBox15.Enabled = groupBox16.Enabled = button2.Enabled = checkBox13.Checked;
        private void checkBox14_CheckedChanged(object sender, EventArgs e) => checkBox112.Enabled = checkBox113.Enabled = checkBox14.Checked;
        private void checkBox18_CheckedChanged(object sender, EventArgs e) => checkBox19.Enabled = checkBox21.Enabled = checkBox18.Checked;
        private void checkBox20_CheckedChanged(object sender, EventArgs e) => groupBox7.Enabled = groupBox10.Enabled = button3.Enabled = checkBox20.Checked;
        private void checkBox22_CheckedChanged(object sender, EventArgs e) => checkBox24.Enabled = checkBox25.Enabled = checkBox22.Checked;
        private void checkBox23_CheckedChanged(object sender, EventArgs e) => groupBox8.Enabled = groupBox11.Enabled = button4.Enabled = checkBox23.Checked;
        private void checkBox26_CheckedChanged(object sender, EventArgs e) => numericUpDown6.Enabled = checkBox26.Checked;
        private void checkBox28_CheckedChanged(object sender, EventArgs e) => numericUpDown8.Enabled = checkBox28.Checked;
        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            checkBox26.Enabled = checkBox28.Enabled = checkBox29.Enabled = checkBox34.Enabled = checkBox37.Enabled = checkBox41.Enabled = checkBox30.Checked;
            numericUpDown6.Enabled = checkBox26.Checked && checkBox30.Checked;
            numericUpDown8.Enabled = checkBox28.Checked && checkBox30.Checked;
            numericUpDown14.Enabled = checkBox41.Checked && checkBox30.Checked;
        }
        private void checkBox33_CheckedChanged(object sender, EventArgs e) => numericUpDown9.Enabled = checkBox33.Checked;
        private void checkBox36_CheckedChanged(object sender, EventArgs e) => groupBox5.Enabled = groupBox6.Enabled = button1.Enabled = checkBox36.Checked;
        private void checkBox38_CheckedChanged(object sender, EventArgs e)
        {
            checkBox33.Enabled = checkBox35.Enabled = checkBox39.Enabled = checkBox40.Enabled = checkBox42.Enabled = checkBox38.Checked;
            numericUpDown9.Enabled = checkBox33.Checked && checkBox38.Checked;
            numericUpDown10.Enabled = checkBox42.Checked && checkBox38.Checked;
        }
        private void checkBox41_CheckedChanged(object sender, EventArgs e) => numericUpDown14.Enabled = checkBox41.Checked;
        private void checkBox42_CheckedChanged(object sender, EventArgs e) => numericUpDown10.Enabled = checkBox42.Checked;
        private void checkBox43_CheckedChanged(object sender, EventArgs e) => numericUpDown15.Enabled = checkBox43.Checked;
        private void checkBox48_CheckedChanged(object sender, EventArgs e) => groupBox13.Enabled = groupBox14.Enabled = button6.Enabled = checkBox48.Checked;
        private void checkBox45_CheckedChanged(object sender, EventArgs e) => numericUpDown15.Enabled = (checkBox27.Enabled = checkBox43.Enabled = checkBox44.Enabled = checkBox46.Enabled = checkBox47.Enabled = checkBox45.Checked) && checkBox43.Checked;
        private void checkBox49_CheckedChanged(object sender, EventArgs e) => numericUpDown7.Enabled = checkBox49.Checked;
        private void checkBox50_CheckedChanged(object sender, EventArgs e) => numericUpDown17.Enabled = checkBox50.Checked;
        private void checkBox51_CheckedChanged(object sender, EventArgs e) => numericUpDown18.Enabled = checkBox51.Checked;
        private void checkBox53_CheckedChanged(object sender, EventArgs e) => checkBox54.Enabled = checkBox55.Enabled = checkBox53.Checked;
        private void checkBox55_CheckedChanged(object sender, EventArgs e) => numericUpDown16.Enabled = checkBox55.Checked;
        private void checkBox56_CheckedChanged(object sender, EventArgs e)
        {
            checkBox49.Enabled = checkBox50.Enabled = checkBox51.Enabled = checkBox52.Enabled = checkBox57.Enabled = checkBox58.Enabled = checkBox59.Enabled = checkBox60.Enabled = checkBox61.Enabled = checkBox75.Enabled = checkBox76.Enabled = checkBox77.Enabled = checkBox78.Enabled = checkBox79.Enabled = checkBox80.Enabled = checkBox81.Enabled = checkBox82.Enabled = checkBox83.Enabled = checkBox84.Enabled = checkBox56.Checked;
            numericUpDown7.Enabled = checkBox49.Checked && checkBox56.Checked;
            numericUpDown17.Enabled = checkBox50.Checked && checkBox56.Checked;
            numericUpDown18.Enabled = checkBox51.Checked && checkBox56.Checked;
            numericUpDown19.Enabled = checkBox61.Checked && checkBox56.Checked;
            numericUpDown20.Enabled = checkBox78.Checked && checkBox56.Checked;
            numericUpDown21.Enabled = checkBox76.Checked && checkBox56.Checked;
            numericUpDown22.Enabled = checkBox77.Checked && checkBox56.Checked;
            numericUpDown23.Enabled = checkBox59.Checked && checkBox56.Checked;
            numericUpDown24.Enabled = checkBox57.Checked && checkBox56.Checked;
            numericUpDown25.Enabled = checkBox79.Checked && checkBox56.Checked;
        }
        private void checkBox57_CheckedChanged(object sender, EventArgs e) => numericUpDown24.Enabled = checkBox57.Checked;
        private void checkBox59_CheckedChanged(object sender, EventArgs e) => numericUpDown23.Enabled = checkBox59.Checked;
        private void checkBox61_CheckedChanged(object sender, EventArgs e) => numericUpDown19.Enabled = checkBox61.Checked;
        private void checkBox62_CheckedChanged(object sender, EventArgs e) => groupBox5.Enabled = groupBox6.Enabled = button1.Enabled = checkBox62.Checked;
        private void checkBox63_CheckedChanged(object sender, EventArgs e) => checkBox64.Enabled = checkBox65.Enabled = checkBox63.Checked;
        private void checkBox66_CheckedChanged(object sender, EventArgs e)
        {
            checkBox6.Enabled = checkBox7.Enabled = checkBox8.Enabled = checkBox10.Enabled = checkBox11.Enabled = checkBox14.Enabled = checkBox15.Enabled = checkBox67.Enabled = checkBox70.Enabled = checkBox73.Enabled = checkBox74.Enabled = checkBox66.Checked;
            checkBox68.Enabled = checkBox70.Checked && checkBox66.Checked;
            checkBox69.Enabled = checkBox67.Checked && checkBox66.Checked;
            checkBox71.Enabled = checkBox72.Enabled = (comboBox2.Enabled = checkBox74.Checked && checkBox66.Checked) && comboBox2.SelectedIndex > 0;
            checkBox112.Enabled = checkBox113.Enabled = checkBox14.Checked && checkBox66.Checked;
            numericUpDown1.Enabled = checkBox10.Checked && checkBox66.Checked;
            numericUpDown2.Enabled = checkBox8.Checked && checkBox66.Checked;
            numericUpDown5.Enabled = checkBox11.Checked && checkBox66.Checked;
            numericUpDown31.Enabled = checkBox70.Checked && !checkBox68.Checked && checkBox66.Checked;
            numericUpDown32.Enabled = checkBox67.Checked && !checkBox69.Checked && checkBox66.Checked;
            numericUpDown33.Enabled = checkBox73.Checked && checkBox66.Checked;
        }
        private void checkBox67_CheckedChanged(object sender, EventArgs e) => numericUpDown32.Enabled = (checkBox69.Enabled = checkBox67.Checked) && !checkBox69.Checked;
        private void checkBox68_CheckedChanged(object sender, EventArgs e) => numericUpDown31.Enabled = !checkBox68.Checked;
        private void checkBox69_CheckedChanged(object sender, EventArgs e) => numericUpDown32.Enabled = !checkBox69.Checked;
        private void checkBox70_CheckedChanged(object sender, EventArgs e) => numericUpDown31.Enabled = (checkBox68.Enabled = checkBox70.Checked) && !checkBox68.Checked;
        private void checkBox71_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox71.Checked && !checkBox72.Checked)
                checkBox72.Checked = true;
        }
        private void checkBox72_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox71.Checked && !checkBox72.Checked)
                checkBox71.Checked = true;
        }
        private void checkBox73_CheckedChanged(object sender, EventArgs e) => numericUpDown33.Enabled = checkBox73.Checked;
        private void checkBox74_CheckedChanged(object sender, EventArgs e) => checkBox71.Enabled = checkBox72.Enabled = (comboBox2.Enabled = checkBox74.Checked) && comboBox2.SelectedIndex > 0;
        private void checkBox76_CheckedChanged(object sender, EventArgs e) => numericUpDown21.Enabled = checkBox76.Checked;
        private void checkBox77_CheckedChanged(object sender, EventArgs e) => numericUpDown22.Enabled = checkBox77.Checked;
        private void checkBox78_CheckedChanged(object sender, EventArgs e) => numericUpDown20.Enabled = checkBox78.Checked;
        private void checkBox79_CheckedChanged(object sender, EventArgs e) => numericUpDown25.Enabled = checkBox79.Checked;
        private void checkBox85_CheckedChanged(object sender, EventArgs e) => checkBox86.Enabled = checkBox87.Enabled = checkBox85.Checked;
        private void checkBox88_CheckedChanged(object sender, EventArgs e) => numericUpDown3.Enabled = checkBox88.Checked;
        private void checkBox90_CheckedChanged(object sender, EventArgs e) => numericUpDown3.Enabled = (checkBox88.Enabled = checkBox89.Enabled = checkBox94.Enabled = checkBox98.Enabled = checkBox90.Checked) && checkBox88.Checked;
        private void checkBox95_CheckedChanged(object sender, EventArgs e) => numericUpDown29.Enabled = checkBox95.Checked;
        private void checkBox96_CheckedChanged(object sender, EventArgs e) => numericUpDown30.Enabled = checkBox96.Checked;
        private void checkBox97_CheckedChanged(object sender, EventArgs e) => numericUpDown26.Enabled = checkBox97.Checked;
        private void checkBox99_CheckedChanged(object sender, EventArgs e) => numericUpDown27.Enabled = checkBox99.Checked;
        private void checkBox100_CheckedChanged(object sender, EventArgs e) => numericUpDown28.Enabled = checkBox100.Checked;
        private void checkBox101_CheckedChanged(object sender, EventArgs e) => numericUpDown36.Enabled = checkBox101.Checked;
        private void checkBox102_CheckedChanged(object sender, EventArgs e) => numericUpDown34.Enabled = checkBox102.Checked;
        private void checkBox103_CheckedChanged(object sender, EventArgs e) => numericUpDown35.Enabled = checkBox103.Checked;
        private void checkBox105_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Enabled = checkBox2.Enabled = checkBox3.Enabled = checkBox4.Enabled = checkBox5.Enabled = checkBox9.Enabled = checkBox91.Enabled = checkBox92.Enabled = checkBox93.Enabled = checkBox95.Enabled = checkBox96.Enabled = checkBox97.Enabled = checkBox98.Enabled = checkBox99.Enabled = checkBox100.Enabled = checkBox101.Enabled = checkBox102.Enabled = checkBox103.Enabled = checkBox104.Enabled = checkBox106.Enabled = checkBox107.Enabled = checkBox105.Checked;
            checkBox31.Enabled = checkBox32.Enabled = checkBox9.Checked && checkBox105.Checked;
            numericUpDown11.Enabled = checkBox3.Checked && checkBox105.Checked;
            numericUpDown12.Enabled = checkBox5.Checked && checkBox105.Checked;
            numericUpDown13.Enabled = checkBox4.Checked && checkBox105.Checked;
            numericUpDown26.Enabled = checkBox97.Checked && checkBox105.Checked;
            numericUpDown27.Enabled = checkBox99.Checked && checkBox105.Checked;
            numericUpDown28.Enabled = checkBox100.Checked && checkBox105.Checked;
            numericUpDown29.Enabled = checkBox95.Checked && checkBox105.Checked;
            numericUpDown30.Enabled = checkBox96.Checked && checkBox105.Checked;
            numericUpDown34.Enabled = checkBox102.Checked && checkBox105.Checked;
            numericUpDown35.Enabled = checkBox103.Checked && checkBox105.Checked;
            numericUpDown36.Enabled = checkBox101.Checked && checkBox105.Checked;
            radioButton3.Enabled = radioButton4.Enabled = checkBox1.Checked && checkBox105.Checked;
            radioButton5.Enabled = radioButton7.Enabled = checkBox2.Checked && checkBox105.Checked;
        }
        private void checkBox108_CheckedChanged(object sender, EventArgs e) => checkBox108.Enabled = checkBox109.Enabled = checkBox108.Checked;
        private void checkBox111_CheckedChanged(object sender, EventArgs e) => groupBox19.Enabled = groupBox18.Enabled = button8.Enabled = checkBox111.Checked;
        private void checkBox114_CheckedChanged(object sender, EventArgs e) => checkBox115.Enabled = checkBox116.Enabled = checkBox114.Checked;
        private void checkBox117_CheckedChanged(object sender, EventArgs e) => checkBox118.Enabled = checkBox119.Enabled = checkBox117.Checked;
        private void checkBox120_CheckedChanged(object sender, EventArgs e) => numericUpDown34.Enabled = checkBox120.Checked;
        private void checkBox121_CheckedChanged(object sender, EventArgs e) => numericUpDown16.Enabled = checkBox121.Checked;
        private void checkBox122_CheckedChanged(object sender, EventArgs e) => numericUpDown37.Enabled = checkBox122.Checked;
        private void checkBox124_CheckedChanged(object sender, EventArgs e)
        {
            checkBox120.Enabled = checkBox121.Enabled = checkBox122.Enabled = checkBox123.Enabled = checkBox125.Enabled = checkBox126.Enabled = checkBox127.Enabled = checkBox128.Enabled = checkBox130.Enabled = checkBox131.Enabled = checkBox132.Enabled = checkBox124.Checked;
            checkBox133.Enabled = checkBox134.Enabled = (comboBox1.Enabled = checkBox130.Checked && checkBox124.Checked) && comboBox1.SelectedIndex > 0;
            numericUpDown4.Enabled = checkBox120.Checked && checkBox124.Checked;
            numericUpDown16.Enabled = checkBox121.Checked && checkBox124.Checked;
            numericUpDown37.Enabled = checkBox122.Checked && checkBox124.Checked;
            numericUpDown38.Enabled = checkBox125.Checked && checkBox124.Checked;
            numericUpDown39.Enabled = checkBox131.Checked && checkBox124.Checked;
            numericUpDown40.Enabled = checkBox132.Checked && checkBox124.Checked;
        }
        private void checkBox125_CheckedChanged(object sender, EventArgs e) => numericUpDown38.Enabled = checkBox125.Checked;
        private void checkBox129_CheckedChanged(object sender, EventArgs e) => numericUpDown41.Enabled = checkBox129.Checked;
        private void checkBox130_CheckedChanged(object sender, EventArgs e) => comboBox1.Enabled = checkBox133.Enabled = checkBox134.Enabled = checkBox130.Checked;
        private void checkBox131_CheckedChanged(object sender, EventArgs e) => numericUpDown39.Enabled = checkBox131.Checked;
        private void checkBox132_CheckedChanged(object sender, EventArgs e) => numericUpDown40.Enabled = checkBox132.Checked;
        private void checkBox133_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox133.Checked && !checkBox134.Checked)
                checkBox134.Checked = true;
        }
        private void checkBox134_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox133.Checked && !checkBox134.Checked)
                checkBox133.Checked = true;
        }
        private void checkBox135_CheckedChanged(object sender, EventArgs e) => groupBox21.Enabled = groupBox23.Enabled = button9.Enabled = checkBox135.Checked;
        private void checkBox136_CheckedChanged(object sender, EventArgs e) => numericUpDown42.Enabled = checkBox136.Checked;
        private void checkBox137_CheckedChanged(object sender, EventArgs e) => numericUpDown43.Enabled = checkBox137.Checked;
        private void checkBox139_CheckedChanged(object sender, EventArgs e)
        {
            checkBox129.Enabled = checkBox137.Enabled = checkBox138.Enabled = checkBox140.Enabled = checkBox141.Enabled = checkBox143.Enabled = checkBox145.Enabled = checkBox146.Enabled = checkBox139.Checked;
            numericUpDown41.Enabled = checkBox129.Checked && checkBox139.Checked;
            numericUpDown43.Enabled = checkBox137.Checked && checkBox139.Checked;
            numericUpDown44.Enabled = checkBox140.Checked && checkBox139.Checked;
            numericUpDown45.Enabled = checkBox145.Checked && checkBox139.Checked;
            numericUpDown46.Enabled = checkBox146.Checked && checkBox139.Checked;
        }
        private void checkBox140_CheckedChanged(object sender, EventArgs e) => numericUpDown44.Enabled = checkBox140.Checked;
        private void checkBox144_CheckedChanged(object sender, EventArgs e)
        {
            checkBox136.Enabled = checkBox142.Enabled = checkBox147.Enabled = checkBox148.Enabled = checkBox150.Enabled = checkBox144.Checked;
            numericUpDown42.Enabled = checkBox136.Checked && checkBox144.Checked;
            numericUpDown47.Enabled = checkBox150.Checked && checkBox144.Checked;
        }
        private void checkBox145_CheckedChanged(object sender, EventArgs e) => numericUpDown45.Enabled = checkBox145.Checked;
        private void checkBox146_CheckedChanged(object sender, EventArgs e) => numericUpDown46.Enabled = checkBox146.Checked;
        private void checkBox149_CheckedChanged(object sender, EventArgs e) => groupBox25.Enabled = groupBox26.Enabled = button10.Enabled = checkBox149.Checked;
        private void checkBox150_CheckedChanged(object sender, EventArgs e) => numericUpDown47.Enabled = checkBox150.Checked;
        private void checkBox151_CheckedChanged(object sender, EventArgs e) => checkBox152.Enabled = checkBox153.Enabled = checkBox151.Checked;
        private void checkBox154_CheckedChanged(object sender, EventArgs e) => groupBox29.Enabled = groupBox30.Enabled = button11.Enabled = checkBox154.Checked;
        private void checkBox155_CheckedChanged(object sender, EventArgs e) => checkBox156.Enabled = checkBox157.Enabled = checkBox155.Checked;
        private void checkBox158_CheckedChanged(object sender, EventArgs e) => numericUpDown48.Enabled = checkBox158.Checked;
        private void checkBox159_CheckedChanged(object sender, EventArgs e) => numericUpDown49.Enabled = checkBox159.Checked;
        private void checkBox161_CheckedChanged(object sender, EventArgs e)
        {
            checkBox158.Enabled = checkBox159.Enabled = checkBox160.Enabled = checkBox162.Enabled = checkBox163.Enabled = checkBox164.Enabled = checkBox165.Enabled = checkBox166.Enabled = checkBox168.Enabled = checkBox161.Checked;
            numericUpDown48.Enabled = checkBox158.Checked && checkBox161.Checked;
            numericUpDown49.Enabled = checkBox159.Checked && checkBox161.Checked;
            numericUpDown50.Enabled = checkBox162.Checked && checkBox161.Checked;
            numericUpDown51.Enabled = checkBox165.Checked && checkBox161.Checked;
            numericUpDown52.Enabled = checkBox166.Checked && checkBox161.Checked;
            numericUpDown53.Enabled = checkBox168.Checked && checkBox161.Checked;
        }
        private void checkBox162_CheckedChanged(object sender, EventArgs e) => numericUpDown50.Enabled = checkBox162.Checked;
        private void checkBox165_CheckedChanged(object sender, EventArgs e) => numericUpDown51.Enabled = checkBox165.Checked;
        private void checkBox166_CheckedChanged(object sender, EventArgs e) => numericUpDown52.Enabled = checkBox166.Checked;
        private void checkBox167_CheckedChanged(object sender, EventArgs e) => groupBox32.Enabled = groupBox33.Enabled = button12.Enabled = checkBox167.Checked;
        private void checkBox168_CheckedChanged(object sender, EventArgs e) => numericUpDown53.Enabled = checkBox168.Checked;
        private void checkBox169_CheckedChanged(object sender, EventArgs e) => numericUpDown54.Enabled = checkBox169.Checked;
        private void checkBox170_CheckedChanged(object sender, EventArgs e) => numericUpDown55.Enabled = checkBox170.Checked;
        private void checkBox172_CheckedChanged(object sender, EventArgs e)
        {
            checkBox169.Enabled = checkBox170.Enabled = checkBox171.Enabled = checkBox173.Enabled = checkBox174.Enabled = checkBox175.Enabled = checkBox172.Checked;
            numericUpDown54.Enabled = checkBox169.Checked && checkBox172.Checked;
            numericUpDown55.Enabled = checkBox170.Checked && checkBox172.Checked;
            numericUpDown56.Enabled = checkBox175.Checked && checkBox172.Checked;
        }
        private void checkBox175_CheckedChanged(object sender, EventArgs e) => numericUpDown56.Enabled = checkBox175.Checked;
        private void checkBox176_CheckedChanged(object sender, EventArgs e) => checkBox177.Enabled = checkBox178.Enabled = checkBox176.Checked;
        private void checkBox179_CheckedChanged(object sender, EventArgs e) => groupBox35.Enabled = groupBox36.Enabled = button13.Enabled = checkBox179.Checked;
        private void checkBox181_CheckedChanged(object sender, EventArgs e) => numericUpDown57.Enabled = checkBox181.Checked;
        private void checkBox183_CheckedChanged(object sender, EventArgs e)
        {
            checkBox180.Enabled = checkBox181.Enabled = checkBox182.Enabled = checkBox184.Enabled = checkBox185.Enabled = checkBox190.Enabled = checkBox191.Enabled = checkBox183.Checked;
            numericUpDown57.Enabled = checkBox181.Checked && checkBox183.Checked;
            radioButton1.Enabled = radioButton2.Enabled = radioButton6.Enabled = checkBox191.Checked && checkBox183.Checked;
        }
        private void checkBox186_CheckedChanged(object sender, EventArgs e) => checkBox187.Enabled = checkBox188.Enabled = checkBox186.Checked;
        private void checkBox189_CheckedChanged(object sender, EventArgs e) => groupBox38.Enabled = groupBox39.Enabled = button14.Enabled = checkBox189.Checked;
        private void checkBox191_CheckedChanged(object sender, EventArgs e) => radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = checkBox191.Checked;
        private void checkBox192_CheckedChanged(object sender, EventArgs e) => numericUpDown58.Enabled = checkBox192.Checked;
        private void checkBox193_CheckedChanged(object sender, EventArgs e) => numericUpDown59.Enabled = checkBox193.Checked;
        private void checkBox195_CheckedChanged(object sender, EventArgs e)
        {
            checkBox192.Enabled = checkBox193.Enabled = checkBox194.Enabled = checkBox196.Enabled = checkBox197.Enabled = checkBox198.Enabled = checkBox203.Enabled = checkBox204.Enabled = checkBox195.Checked;
            numericUpDown58.Enabled = checkBox192.Checked && checkBox195.Checked;
            numericUpDown59.Enabled = checkBox193.Checked && checkBox195.Checked;
            numericUpDown60.Enabled = checkBox198.Checked && checkBox195.Checked;
        }
        private void checkBox198_CheckedChanged(object sender, EventArgs e) => numericUpDown60.Enabled = checkBox198.Checked;
        private void checkBox199_CheckedChanged(object sender, EventArgs e) => checkBox200.Enabled = checkBox201.Enabled = checkBox199.Checked;
        private void checkBox202_CheckedChanged(object sender, EventArgs e) => groupBox41.Enabled = groupBox42.Enabled = button15.Enabled = checkBox202.Checked;
        private void checkBox205_CheckedChanged(object sender, EventArgs e) => radioButton8.Enabled = radioButton9.Enabled = radioButton10.Enabled = checkBox205.Checked;
        private void checkBox207_CheckedChanged(object sender, EventArgs e) => numericUpDown61.Enabled = checkBox207.Checked;
        private void checkBox208_CheckedChanged(object sender, EventArgs e) => numericUpDown62.Enabled = checkBox208.Checked;
        private void checkBox210_CheckedChanged(object sender, EventArgs e)
        {
            checkBox205.Enabled = checkBox206.Enabled = checkBox207.Enabled = checkBox208.Enabled = checkBox209.Enabled = checkBox211.Enabled = checkBox212.Enabled = checkBox213.Enabled = checkBox210.Checked;
            numericUpDown61.Enabled = checkBox207.Checked && checkBox210.Checked;
            numericUpDown62.Enabled = checkBox208.Checked && checkBox210.Checked;
            numericUpDown63.Enabled = checkBox213.Checked && checkBox210.Checked;
            radioButton8.Enabled = radioButton9.Enabled = radioButton10.Enabled = checkBox205.Checked && checkBox210.Checked;
        }
        private void checkBox213_CheckedChanged(object sender, EventArgs e) => numericUpDown63.Enabled = checkBox213.Checked;
        private void checkBox214_CheckedChanged(object sender, EventArgs e) => checkBox215.Enabled = checkBox216.Enabled = checkBox214.Checked;
        private void checkBox217_CheckedChanged(object sender, EventArgs e) => groupBox44.Enabled = groupBox45.Enabled = button16.Enabled = checkBox217.Checked;
        private void checkBox218_CheckedChanged(object sender, EventArgs e) => numericUpDown64.Enabled = checkBox218.Checked;
        private void checkBox220_CheckedChanged(object sender, EventArgs e)
        {
            checkBox218.Enabled = checkBox219.Enabled = checkBox221.Enabled = checkBox222.Enabled = checkBox223.Enabled = checkBox220.Checked;
            numericUpDown64.Enabled = checkBox218.Checked && checkBox220.Checked;
            numericUpDown65.Enabled = checkBox223.Checked && checkBox220.Checked;
        }
        private void checkBox223_CheckedChanged(object sender, EventArgs e) => numericUpDown65.Enabled = checkBox223.Checked;
        private void checkBox224_CheckedChanged(object sender, EventArgs e) => checkBox225.Enabled = checkBox226.Enabled = checkBox224.Checked;
        private void checkBox227_CheckedChanged(object sender, EventArgs e) => groupBox47.Enabled = groupBox48.Enabled = button17.Enabled = checkBox227.Checked;
        private void checkBox229_CheckedChanged(object sender, EventArgs e) => numericUpDown66.Enabled = checkBox229.Checked;
        private void checkBox231_CheckedChanged(object sender, EventArgs e) => numericUpDown66.Enabled = (checkBox228.Enabled = checkBox229.Enabled = checkBox230.Enabled = checkBox232.Enabled = checkBox233.Enabled = checkBox231.Checked) && checkBox229.Checked;
        private void checkBox234_CheckedChanged(object sender, EventArgs e) => checkBox235.Enabled = checkBox236.Enabled = checkBox234.Checked;
        private void checkBox237_CheckedChanged(object sender, EventArgs e) => groupBox50.Enabled = groupBox51.Enabled = button18.Enabled = checkBox237.Checked;
        private void checkBox240_CheckedChanged(object sender, EventArgs e) => numericUpDown67.Enabled = checkBox240.Checked;
        private void checkBox241_CheckedChanged(object sender, EventArgs e) => numericUpDown69.Enabled = checkBox241.Checked;
        private void checkBox242_CheckedChanged(object sender, EventArgs e) => numericUpDown68.Enabled = checkBox242.Checked;
        private void checkBox244_CheckedChanged(object sender, EventArgs e)
        {
            checkBox238.Enabled = checkBox239.Enabled = checkBox240.Enabled = checkBox241.Enabled = checkBox242.Enabled = checkBox243.Enabled = checkBox245.Enabled = checkBox246.Enabled = checkBox244.Checked;
            numericUpDown67.Enabled = checkBox240.Checked && checkBox244.Checked;
            numericUpDown68.Enabled = checkBox241.Checked && checkBox244.Checked;
            numericUpDown69.Enabled = checkBox242.Checked && checkBox244.Checked;
        }
        private void checkBox247_CheckedChanged(object sender, EventArgs e) => checkBox248.Enabled = checkBox249.Enabled = checkBox247.Checked;
        private void checkBox250_CheckedChanged(object sender, EventArgs e) => groupBox53.Enabled = groupBox54.Enabled = button19.Enabled = checkBox250.Checked;
        #endregion
        #region ComboBoxes
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) => checkBox71.Enabled = checkBox72.Enabled = comboBox2.SelectedIndex > 0;
        #endregion
        #region RadioButtons
        private void radioButton1_CheckedChanged(object sender, EventArgs e) => numericUpDown4.Enabled = radioButton1.Checked;
        private void radioButton2_CheckedChanged(object sender, EventArgs e) => numericUpDown3.Enabled = radioButton2.Checked;
        private void radioButton3_CheckedChanged(object sender, EventArgs e) => checkBox32.Enabled = checkBox31.Enabled = radioButton3.Checked;
        #endregion
        #region Buttons
        private void button1_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Move);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Pulse);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Alpha);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Spawn);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Toggle);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Color);
        }
        private void button7_Click(object sender, EventArgs e)
        {

        } // The "Apply!" button
        private void button8_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Stop);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Rotate);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Follow);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Animate);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.FollowPlayerY);
        }
        private void button13_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Shake);
        }
        private void button14_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Touch);
        }
        private void button15_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Count);
        }
        private void button16_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.InstantCount);
        }
        private void button17_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Pickup);
        }
        private void button18_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.OnDeath);
        }
        private void button19_Click(object sender, EventArgs e)
        {
            List<LevelObject> dummy = new List<LevelObject>();
            Gamesave.GetCommonAttributes(dummy, (int)LevelObject.Trigger.Collision);
        }
        #endregion
    }
}