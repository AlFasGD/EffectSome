using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.Runtime.InteropServices;
using static EffectSome.GuidelineEditor;

namespace EffectSome
{
    public partial class DetectBPM : Form
    {
        public volatile static int Clicks = -1;
        public static TimeSpan RecordTime = new TimeSpan(0, 0, 0, 0, 0);
        public static ATimer.ElapsedTimerDelegate callback = Timer_Elapsed;
        ATimer timer = new ATimer(3, 1, callback);

        public DetectBPM()
        {
            InitializeComponent();
            textBox1.Text = EffectSome.GDLocalData + "\\" + EffectSome.UserLevels[CurrentLevelIndex].LevelCustomSongID + ".mp3";
            openFileDialog1.InitialDirectory = EffectSome.GDLocalData;
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e) => groupBox1.Enabled = radioButton1.Checked;
        private void radioButton2_CheckedChanged(object sender, EventArgs e) => groupBox3.Enabled = radioButton2.Checked;
        private void button1_Click(object sender, EventArgs e)
        {
            DetectedNewBPM = true;
            timer.Stop();
            button2.Text = "Start recording";
            button1.Enabled = false;
            Clicks = -1;
            RecordTime = new TimeSpan(0, 0, 0, 0, 0);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (Clicks < 0)
            {
                timer.Start();
                button1.Enabled = true;
            }
            Clicks++;
            if (Clicks < 7)
                button2.Text = "Click " + (7 - Clicks).ToString() + " more times to have an accurate result.";
            else if (Clicks >= 7)
            {
                DetectedBPM = Math.Round((Clicks / RecordTime.TotalMinutes) / (double)numericUpDown1.Value) * (double)numericUpDown1.Value;
                button2.Text = $"{DetectedBPM} BPM";
            }
            button1.Focus();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
                textBox1.Text = openFileDialog1.FileName;
        }
        private void button4_Click(object sender, EventArgs e)
        {

        }
        
        public static void Timer_Elapsed()
        {
            RecordTime = RecordTime.Add(new TimeSpan(0, 0, 0, 0, 1));
        }
    }
}