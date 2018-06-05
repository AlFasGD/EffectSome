using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EffectSome.EffectSome;

namespace EffectSome
{
    public partial class GuidelineEditor : Form
    {
        public static bool IsOpen = false;

        bool IsMouseClickedInDataGridView = false;
        bool MouseLeftClick = false;
        bool MouseRightClick = false;

        public static bool DetectedNewBPM = false;
        public static double DetectedBPM = 0;

        public static WebClient StoreMainLevelGuidelineStringDownloader;
        public static WebClient StoreSelectedLevelsGuidelineStringsDownloader;
        public static BackgroundWorker DownloadMainLevelGuidelinesOnStore = new BackgroundWorker();
        public static BackgroundWorker DownloadSelectedLevelsGuidelinesOnStore = new BackgroundWorker();

        public static double BPM = 0;
        public static int GuidelinesCount = 0;
        public static int CurrentLevelIndex = 0;
        public static int CurrentPresetIndex = -1;
        public static int SelectedEntriesInStore = 0;
        public static int RecordedGuidelinesCount = 0;
        public static int[] TimeSignatures = { 4, 4 };
        public static bool RecordingPaused = true;
        public static string GuidelinesString = "";
        public static string RecordedGuidelineString = "";
        public static List<int> SelectedLevelIndices = new List<int>();
        public static List<int> DeselectLevelIndices = new List<int>();
        public static List<int> SelectedGuidelineIndices = new List<int>();
        public static List<int> DeselectGuidelineIndices = new List<int>();
        public static List<int> RequestedSongsGuidelines = new List<int>();
        public static List<int[]> CurrentCreationPresets = new List<int[]>();
        public static List<Guideline> LevelGuidelines = new List<Guideline>();
        public static List<Guideline> RecordedGuidelines = new List<Guideline>();
        
        public static List<GuidelineEditorPreset> Presets = new List<GuidelineEditorPreset>();

        public static TimeSpan RecordTime = new TimeSpan(0, 0, 0, 0, 0);

        public static ATimer.ElapsedTimerDelegate Callback = Timer_Elapsed;
        ATimer timer = new ATimer(3, 1, Callback);

        public GuidelineEditor()
        {
            IsOpen = true;
            InitializeComponent();
            SetInitialBPM();
            LoadPresets();
            if (Presets.Count == 0)
                CreatePreset("New Preset 1");
            CurrentPresetIndex = 0;
            LoadPreset(0);
            CheckSongType(0);
            PrepareInitialization();
        }

        private void GuidelineEditor_Activated(object sender, EventArgs e)
        {
            if (DetectedNewBPM)
                numericUpDown7.Value = (decimal)DetectedBPM;
            DetectedNewBPM = false;
        }
        private void GuidelineEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            SavePreset(CurrentPresetIndex);
            comboBox5_Leave(sender, e);
            PauseGuidelineRecording(false);
            IsOpen = false;
        }
        private void GuidelineEditor_KeyDown(object sender, KeyEventArgs e)
        {
            button9_KeyDown(sender, e);
        }

        #region NumericUpDowns
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ShowGuidelineInfo();
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (groupBox2.Enabled)
            {
                LevelGuidelines[(int)numericUpDown1.Value - 1].TimeStamp = (double)numericUpDown5.Value;
                UpdateGuidelines();
            }
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown7.ForeColor != Color.Red)
            {
                decimal BPMRelation = (Math.Max(numericUpDown7.Value, (decimal)BPM) / Math.Min(numericUpDown7.Value, (decimal)BPM));
                if (BPMRelation != (int)BPMRelation)
                    numericUpDown7.ForeColor = Color.Blue;
                else
                    numericUpDown7.ForeColor = Color.Black;
            }
            if (CurrentPresetIndex > -1)
                Presets[CurrentPresetIndex].BPM = (double)numericUpDown7.Value;
            button32.Enabled = numericUpDown7.Maximum >= numericUpDown7.Value * 2;
            button33.Enabled = numericUpDown7.Minimum <= numericUpDown7.Value / 2;
        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (CurrentPresetIndex > -1)
                Presets[CurrentPresetIndex].TimeSignature.Beats = (int)numericUpDown8.Value;
            numericUpDown6.Maximum = numericUpDown8.Value;
            int beatInterval = ((double)(16 / numericUpDown9.Value)).OneOrGreater(); // The number of beats per single beat specified in the time signature
            for (int i = 1; i <= 32; i++) // Set the visible columns
                dataGridView1.Columns[i].Visible = numericUpDown8.Value * beatInterval >= i;
        }
        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (CurrentPresetIndex > -1)
                Presets[CurrentPresetIndex].TimeSignature.Denominator = (int)numericUpDown9.Value;
            numericUpDown9.Value = AdjustValueToClosestPowerOf2(numericUpDown9.Value); // Adjust the denominator to the closest power of two
            numericUpDown9.Increment = numericUpDown9.Value / 2; // Change the increment of the control to make it easier when changing value from the buttons
            numericUpDown8.Maximum = Math.Min(numericUpDown9.Value * 2, 32); // Change the maximum of the beat number to up to two times the denominator

            // Expand the range of the value while changing time signature
            while (numericUpDown7.Value < 15 * numericUpDown9.Value)
            {
                if (numericUpDown7.Value * 2 > numericUpDown7.Maximum)
                    numericUpDown7.Maximum *= 2;
                numericUpDown7.Value *= 2;
            }
            while (numericUpDown7.Value > 60 * numericUpDown9.Value)
            {
                if (numericUpDown7.Value / 2 < numericUpDown7.Minimum)
                    numericUpDown7.Minimum /= 2;
                numericUpDown7.Value /= 2;
            }
            numericUpDown7.Minimum = 15 * numericUpDown9.Value;
            numericUpDown7.Maximum = 60 * numericUpDown9.Value;
            button32.Enabled = numericUpDown7.Maximum >= numericUpDown7.Value * 2;
            button33.Enabled = numericUpDown7.Minimum <= numericUpDown7.Value / 2;

            int beatInterval = ((double)(16 / numericUpDown9.Value)).OneOrGreater(); // The number of beats per single beat specified in the time signature
            for (int i = 1; i <= 32; i++) // Change the header texts
                dataGridView1.Columns[i].HeaderText = ((i - 1) % beatInterval == 0) ? ((i - 1) / beatInterval + 1).ToString() : "";
            for (int i = 1; i <= 32; i++) // Set the visible columns
                dataGridView1.Columns[i].Visible = numericUpDown8.Value * beatInterval >= i;
        }
        private void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            if (RecordingPaused)
                RecordTime = new TimeSpan(0, 0, 0, 0, (int)(numericUpDown18.Value * 1000));
        }
        #endregion
        #region ComboBoxes
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (groupBox2.Enabled)
            {
                //colors[(int)numericUpDown1.Value - 1] = 0.80 + comboBox1.SelectedIndex * 0.1;
                LevelGuidelines[(int)numericUpDown1.Value - 1].Color = 0.80 + comboBox1.SelectedIndex * 0.1;
                UpdateGuidelines();
            }
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            button34.Enabled = false;
            DownloadMainLevelGuidelinesOnStore = new BackgroundWorker();
            DownloadMainLevelGuidelinesOnStore.DoWork += DownloadMainLevelGuidelineStringOnStore;
            DownloadMainLevelGuidelinesOnStore.RunWorkerAsync();
            CurrentLevelIndex = comboBox3.SelectedIndex;
            SelectedGuidelineIndices.Clear();
            GuidelinesString = UserLevels[CurrentLevelIndex].LevelGuidelinesString;
            LevelGuidelines = Gamesave.GetGuidelines(GuidelinesString);
            SetInitialBPM();
            CheckSongType(CurrentLevelIndex);
            CheckForGuidelineExistence();
            CheckForSelectedGuidelines();
        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex >= 0)
            {
                if (CurrentPresetIndex > -1)
                    SavePreset(CurrentPresetIndex);
                CurrentPresetIndex = comboBox5.SelectedIndex;
                LoadPreset(CurrentPresetIndex);
                button27.Enabled = Presets[CurrentPresetIndex].Colors.Count < 999;
                button28.Enabled = Presets[CurrentPresetIndex].Colors.Count > 0;
            }
            button21.Enabled = comboBox5.SelectedIndex < comboBox5.Items.Count - 1;
            button20.Enabled = comboBox5.SelectedIndex > 0;
        }
        private void comboBox5_Leave(object sender, EventArgs e)
        {
            // This will be triggered when exiting the Guideline Editor and the name has been still not edited to something not illegal - Needs to be fixed sometime
            // An even better solution would be a better approach and splitting all this work into several functions or compacting the code altogether
            string name = comboBox5.Text;
            if (name.Contains('?', '/', '|', '<', '>', '\\', '"', ':'))
                MessageBox.Show("The name you entered contains invalid characters. If the name contains any of the following invalid characters, please remove them.\n\n\", ?, |, <, >, :, \\, /, *", "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try { File.Move(appLocation + @"\EffectSome\Presets\Guideline Editor\" + comboBox5.Items[CurrentPresetIndex] + ".esf", appLocation + @"\EffectSome\Presets\Guideline Editor\" + comboBox5.Text + ".esf"); }
                catch
                {
                    bool containsNumberAtEnd = false;
                    for (int i = 0; i < name.Length; i++)
                    {
                        string sub = name.Substring(i);
                        if (int.TryParse(sub, out int number))
                        {
                            name = name.Substring(0, i + 1);
                            for (int j = 1; ; j++)
                            {
                                string newName = name + (name[name.Length - 1] != ' ' ? " " : "") + j.ToString();
                                if (!File.Exists(appLocation + @"\EffectSome\Presets\Guideline Editor\" + newName + ".esf"))
                                {
                                    File.Move(appLocation + @"\EffectSome\Presets\Guideline Editor\" + comboBox5.Items[CurrentPresetIndex] + ".esf", appLocation + @"\EffectSome\Presets\Guideline Editor\" + newName + ".esf");
                                    comboBox5.Text = newName;
                                    break;
                                }
                            }
                            containsNumberAtEnd = true;
                        }
                    }
                    if (!containsNumberAtEnd)
                        for (int i = 1; ; i++)
                        {
                            string newName = name + " " + i.ToString();
                            if (!File.Exists(appLocation + @"\EffectSome\Presets\Guideline Editor\" + newName + ".esf"))
                            {
                                File.Move(appLocation + @"\EffectSome\Presets\Guideline Editor\" + comboBox5.Items[CurrentPresetIndex] + ".esf", appLocation + @"\EffectSome\Presets\Guideline Editor\" + newName + ".esf");
                                comboBox5.Text = newName;
                                break;
                            }
                        }
                }
                comboBox5.Items[CurrentPresetIndex] = comboBox5.Text;
                Presets[CurrentPresetIndex].Name = comboBox5.Text;
                comboBox5.SelectedIndex = CurrentPresetIndex;
                comboBox5.SelectionStart = comboBox5.Text.Length;
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    int selectedPreset = (dataGridView2[0, i] as DataGridViewComboBoxCell).GetSelectedIndex();
                    (dataGridView2[0, i] as DataGridViewComboBoxCell).Items[CurrentPresetIndex] = comboBox5.Items[CurrentPresetIndex];
                    (dataGridView2[0, i] as DataGridViewComboBoxCell).Value = (dataGridView2[0, i] as DataGridViewComboBoxCell).Items[selectedPreset];
                }
            }
        }
        #endregion
        #region Buttons
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                for (int i = 0; i < SelectedGuidelineIndices.Count/* && checkBox1.Checked*/; i++)
                    LevelGuidelines[SelectedGuidelineIndices[i]].TimeStamp += (double)numericUpDown4.Value;
            if (checkBox2.Checked)
                for (int i = 0; i < SelectedGuidelineIndices.Count/* && checkBox2.Checked*/; i++)
                    LevelGuidelines[SelectedGuidelineIndices[i]].Color = (0.80 + comboBox2.SelectedIndex * 0.10);
            UpdateGuidelines();
            ShowGuidelineInfo();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DeleteSelectedGuidelines();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            LevelGuidelines.Clear();
            CreateGuidelines();
            CheckForGuidelineExistence();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            DetectBPM detBPM = new DetectBPM();
            detBPM.Show();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                GuidelinesString += RecordedGuidelineString;
            else if (radioButton2.Checked)
                GuidelinesString = RecordedGuidelineString;
            Gamesave.SetGuidelineString(GuidelinesString, CurrentLevelIndex);
            GuidelinesCount = Gamesave.GetNumberOfGuidelines(CurrentLevelIndex);
            UpdateArrays();
            ResetRecordedGuidelines();
            CheckForGuidelineExistence();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            LevelGuidelines.RemoveAt((int)numericUpDown1.Value - 1);
            UpdateGuidelines();
            CheckForSelectedGuidelines();
            CheckForGuidelineExistence();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            SelectedGuidelineIndices.Clear();
            ChangeSelectedGuidelines(ref SelectedGuidelineIndices);
            CheckForSelectedGuidelines();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            PauseGuidelineRecording(true);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            RecordGuideline(RecordTime.TotalSeconds, 0.80);
        }
        private void button9_KeyDown(object sender, KeyEventArgs e)
        {
            if (Options.kc.ConvertToString(e.KeyCode) == Options.StringDictionary["orangeGuidelineShortcutKey"])
                RecordGuideline(RecordTime.TotalSeconds, 0.80);
            else if (Options.kc.ConvertToString(e.KeyCode) == Options.StringDictionary["yellowGuidelineShortcutKey"])
                RecordGuideline(RecordTime.TotalSeconds, 0.90);
            else if (Options.kc.ConvertToString(e.KeyCode) == Options.StringDictionary["greenGuidelineShortcutKey"])
                RecordGuideline(RecordTime.TotalSeconds, 1.00);
            else if (Options.kc.ConvertToString(e.KeyCode) == Options.StringDictionary["pauseGuidelineRecordingShortcutKey"])
                PauseGuidelineRecording(true);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            ResetRecordedGuidelines();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            if (!SelectedGuidelineIndices.Contains((int)numericUpDown1.Value - 1))
                SelectedGuidelineIndices.Add((int)numericUpDown1.Value - 1);
            CheckForSelectedGuidelines();
        }
        private void button12_Click(object sender, EventArgs e)
        {
            SelectedGuidelineIndices.Clear();
            CheckForSelectedGuidelines();
        }
        private void button13_Click(object sender, EventArgs e)
        {
            ChangeSelectedGuidelines(ref SelectedGuidelineIndices);
            CheckForSelectedGuidelines();
        }
        private void button14_Click(object sender, EventArgs e)
        {
            ChangeSelectedGuidelines(ref DeselectGuidelineIndices);
            for (int i = 0; i < DeselectGuidelineIndices.Count; i++)
                SelectedGuidelineIndices.Remove(DeselectGuidelineIndices[i]);
            DeselectGuidelineIndices.Clear();
            CheckForSelectedGuidelines();
        }
        private void button15_Click(object sender, EventArgs e)
        {
            CreateGuidelines();
        }
        private void button16_Click(object sender, EventArgs e)
        {
            ChangeSelectedLevels(ref DeselectLevelIndices);
            for (int i = 0; i < DeselectLevelIndices.Count; i++)
                SelectedLevelIndices.Remove(DeselectLevelIndices[i]);
            CheckForSelectedLevels();
            DeselectLevelIndices.Clear();
        }
        private void button17_Click(object sender, EventArgs e)
        {
            ChangeSelectedLevels(ref SelectedLevelIndices);
            CheckForSelectedLevels();
        }
        private void button18_Click(object sender, EventArgs e)
        {
            SelectedLevelIndices.Clear();
            CheckForSelectedLevels();
        }
        private void button19_Click(object sender, EventArgs e)
        {
            SelectedLevelIndices.Clear();
            ChangeSelectedLevels(ref SelectedLevelIndices);
            CheckForSelectedLevels();
        }
        private void button20_Click(object sender, EventArgs e)
        {
            comboBox5_Leave(sender, e);
            comboBox5.SelectedIndex--;
            button20.Enabled = comboBox5.SelectedIndex > 0;
        }
        private void button21_Click(object sender, EventArgs e)
        {
            comboBox5_Leave(sender, e);
            comboBox5.SelectedIndex++;
            button21.Enabled = comboBox5.SelectedIndex < comboBox5.Items.Count - 1;
        }
        private void button22_Click(object sender, EventArgs e)
        {
            SavePreset(CurrentPresetIndex);
            for (int i = 1; ; i++)
                if (!File.Exists("EffectSome/Presets/Guideline Editor/New Preset " + i.ToString() + ".esf"))
                {
                    CreatePreset("New Preset " + i.ToString());
                    for (int j = 0; j < dataGridView2.Rows.Count; j++)
                        (dataGridView2[0, j] as DataGridViewComboBoxCell).Items.Add("New Preset " + i.ToString());
                    break;
                }
            comboBox5.Focus();
        }
        private void button23_Click(object sender, EventArgs e)
        {
            File.Delete(appLocation + @"\EffectSome\Presets\Guideline Editor\" + comboBox5.Items[CurrentPresetIndex].ToString() + ".esf");
            Presets.RemoveAt(CurrentPresetIndex);
            comboBox5.Items.RemoveAt(CurrentPresetIndex);
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                (dataGridView2[0, i] as DataGridViewComboBoxCell).Items.RemoveAt(CurrentPresetIndex);
                if ((dataGridView2[0, i] as DataGridViewComboBoxCell).Items.Count > 0)
                    (dataGridView2[0, i] as DataGridViewComboBoxCell).Value = (dataGridView2[0, i] as DataGridViewComboBoxCell).Items[0];
            }
            if (CurrentPresetIndex >= Presets.Count)
                CurrentPresetIndex--;
            if (CurrentPresetIndex == -1)
            {
                CreatePreset("New Preset 1");
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    (dataGridView2[0, i] as DataGridViewComboBoxCell).Items.Add("New Preset 1");
                    (dataGridView2[0, i] as DataGridViewComboBoxCell).Value = (dataGridView2[0, i] as DataGridViewComboBoxCell).Items[0];
                }
                CurrentPresetIndex = 0;
            }
            comboBox5.SelectedIndex = CurrentPresetIndex;
        }
        private void button24_Click(object sender, EventArgs e)
        {
            Gamesave.SetGuidelineStrings(GuidelinesString, SelectedLevelIndices.ToArray());
        }
        private void button25_Click(object sender, EventArgs e)
        {
            Gamesave.SetGuidelineStrings("", SelectedLevelIndices.ToArray());
            GuidelinesCount = Gamesave.GetNumberOfGuidelines(CurrentLevelIndex);
            GuidelinesString = UserLevels[CurrentLevelIndex].LevelGuidelinesString;
            LevelGuidelines = Gamesave.GetGuidelines(GuidelinesString);
            CheckForGuidelineExistence();
        }
        private void button26_Click(object sender, EventArgs e)
        {
            decimal BPMToSet = (decimal)BPM;
            while (BPMToSet < numericUpDown7.Minimum)
                BPMToSet *= 2;
            while (BPMToSet > numericUpDown7.Maximum)
                BPMToSet /= 2;
            numericUpDown7.Value = BPMToSet;
        }
        private void button27_Click(object sender, EventArgs e)
        {
            AddMeasure();
            button27.Enabled = Presets[CurrentPresetIndex].Colors.Count < 999;
            button28.Enabled = Presets[CurrentPresetIndex].Colors.Count > 0;
        }
        private void button28_Click(object sender, EventArgs e)
        {
            RemoveMeasure();
            button27.Enabled = Presets[CurrentPresetIndex].Colors.Count < 999;
            button28.Enabled = Presets[CurrentPresetIndex].Colors.Count > 0;
        }
        private void button29_Click(object sender, EventArgs e)
        {
            LevelGuidelines = Gamesave.RemoveDuplicatedGuidelines(LevelGuidelines);
            Gamesave.SetGuidelineString(Gamesave.CreateGuidelineString(LevelGuidelines), CurrentLevelIndex);
            CheckForGuidelineExistence();
        }
        private void button30_Click(object sender, EventArgs e)
        {
            AddPreset();
            button31.Enabled = true;
        }
        private void button31_Click(object sender, EventArgs e)
        {
            RemovePreset();
            button31.Enabled = dataGridView2.Rows.Count > 0;
        }
        private void button32_Click(object sender, EventArgs e)
        {
            if (numericUpDown7.Maximum >= numericUpDown7.Value * 2)
                numericUpDown7.Value *= 2;
        }
        private void button33_Click(object sender, EventArgs e)
        {
            if (numericUpDown7.Minimum <= numericUpDown7.Value / 2)
                numericUpDown7.Value /= 2;
        }
        private void button34_Click(object sender, EventArgs e)
        {
            Gamesave.SetGuidelineString(File.ReadAllText("EffectSome/Guidelines/" + UserLevels[CurrentLevelIndex].LevelCustomSongID.ToString() + ".dat"), CurrentLevelIndex);
            CheckForGuidelineExistence();
        }
        private void button35_Click(object sender, EventArgs e)
        {
            string[] newGS = new string[SelectedLevelIndices.Count];
            for (int i = 0; i < SelectedLevelIndices.Count; i++)
                newGS[i] = File.ReadAllText("EffectSome/Guidelines/" + UserLevels[SelectedLevelIndices[i]].LevelCustomSongID.ToString() + ".dat");
            Gamesave.SetGuidelineStrings(newGS, SelectedLevelIndices.ToArray());
        }
        private void button36_Click(object sender, EventArgs e)
        {
            double interval = 60 / CustomSongBPMs[UserLevels[CurrentLevelIndex].LevelCustomSongID]; // Get the interval
            for (int i = 0; i < LevelGuidelines.Count; i++)
            {
                int mul = (int)Math.Round(LevelGuidelines[i].TimeStamp / interval); // Get the rounded integral multiplier of the interval
                LevelGuidelines[i].TimeStamp = interval * mul; // Apply the quantized timestamp in the guideline
            }
            Gamesave.SetGuidelineString(Gamesave.CreateGuidelineString(LevelGuidelines), CurrentLevelIndex);
            CheckForGuidelineExistence();
        }
        private void button37_Click(object sender, EventArgs e)
        {
            double interval = RecordedGuidelinesCount / RecordTime.TotalSeconds;
            for (int i = 0; i < RecordedGuidelines.Count; i++)
                RecordedGuidelines[i].TimeStamp = interval * i;
        }
        private void button38_Click(object sender, EventArgs e)
        {
            List<Guideline> selectedGuidelines = new List<Guideline>();
            List<Guideline> selectedGuidelinesWithoutDuplicates = new List<Guideline>();
            List<Guideline> duplicatesToRemove = new List<Guideline>();
            for (int i = 0; i < SelectedGuidelineIndices.Count; i++) // Get the selected guidelines
                selectedGuidelines.Add(LevelGuidelines[SelectedGuidelineIndices[i]]);
            selectedGuidelinesWithoutDuplicates = Gamesave.RemoveDuplicatedGuidelines(selectedGuidelines); // Remove the duplicates of the selected guidelines
            for (int i = 0; i < selectedGuidelines.Count; i++) // Create list of the duplicates
                if (!selectedGuidelinesWithoutDuplicates.Contains(selectedGuidelines[i]))
                    duplicatesToRemove.Add(selectedGuidelines[i]);
            for (int i = 0; i < duplicatesToRemove.Count; i++) // Remove the duplicates from the level's guidelines
                LevelGuidelines.Remove(duplicatesToRemove[i]);
            Gamesave.SetGuidelineString(Gamesave.CreateGuidelineString(LevelGuidelines), CurrentLevelIndex);
            CheckForGuidelineExistence();
        }
        private void button39_Click(object sender, EventArgs e)
        {
            DeleteSelectedGuidelines();
        }
        private void button40_Click(object sender, EventArgs e)
        {
            SavePreset(CurrentPresetIndex);
            CopyPreset(CurrentPresetIndex);
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
                (dataGridView2[0, i] as DataGridViewComboBoxCell).Items.Add(comboBox5.Items[comboBox5.Items.Count - 1]);
            LoadPreset(CurrentPresetIndex);
            button28.Enabled = Presets[CurrentPresetIndex].Colors.Count > 0;
        }
        private void button41_Click(object sender, EventArgs e)
        {
            ResetMeasures();
            button27.Enabled = Presets[CurrentPresetIndex].Colors.Count < 999;
            button28.Enabled = Presets[CurrentPresetIndex].Colors.Count > 0;
        }
        #endregion
        #region CheckBoxes
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown4.Enabled = checkBox1.Checked;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = checkBox2.Checked;
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox9.Enabled = checkBox10.Enabled = checkBox15.Enabled = checkBox16.Enabled = !checkBox4.Checked;
            checkBox11.Enabled = checkBox12.Enabled = textBox2.Enabled = checkBox10.Checked && !checkBox4.Checked;
            checkBox13.Enabled = checkBox9.Checked && !checkBox4.Checked;
            checkBox17.Enabled = checkBox16.Checked && !checkBox4.Checked;
            textBox1.Enabled = checkBox9.Checked && !checkBox13.Checked && !checkBox4.Checked;
            numericUpDown15.Enabled = checkBox16.Checked && !checkBox17.Checked && !checkBox4.Checked;
            label6.Enabled = checkBox14.Enabled = numericUpDown13.Enabled = numericUpDown14.Enabled = radioButton3.Enabled = radioButton4.Enabled = checkBox15.Checked && !checkBox4.Checked;
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown10.Enabled = radioButton6.Enabled = radioButton7.Enabled = checkBox5.Checked;
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            comboBox6.Enabled = checkBox6.Checked;
        }
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            checkBox5.Enabled = checkBox6.Enabled = checkBox8.Enabled = !checkBox7.Checked;
            label11.Enabled = !checkBox7.Checked && checkBox8.Checked;
            comboBox6.Enabled = !checkBox7.Checked && checkBox6.Checked;
            checkBox3.Enabled = !checkBox7.Checked && checkBox8.Checked;
            numericUpDown10.Enabled = !checkBox7.Checked && checkBox5.Checked;
            numericUpDown2.Enabled = !checkBox7.Checked && checkBox8.Checked;
            numericUpDown11.Enabled = !checkBox7.Checked && checkBox8.Checked;
            radioButton6.Enabled = !checkBox7.Checked && checkBox5.Checked;
            radioButton7.Enabled = !checkBox7.Checked && checkBox5.Checked;
            radioButton8.Enabled = !checkBox7.Checked && checkBox8.Checked;
            radioButton9.Enabled = !checkBox7.Checked && checkBox8.Checked;
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            label11.Enabled = checkBox3.Enabled = numericUpDown2.Enabled = numericUpDown11.Enabled = radioButton8.Enabled = radioButton9.Enabled = checkBox8.Checked;
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = !checkBox13.Checked && checkBox9.Checked;
            checkBox13.Enabled = checkBox9.Checked;
        }
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            checkBox12.Enabled = checkBox11.Enabled = textBox2.Enabled = checkBox10.Checked;
        }
        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = !checkBox13.Checked;
        }
        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            label6.Enabled = checkBox14.Enabled = numericUpDown13.Enabled = numericUpDown14.Enabled = radioButton3.Enabled = radioButton4.Enabled = checkBox15.Checked;
        }
        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown15.Enabled = !checkBox17.Checked && checkBox16.Checked;
            checkBox17.Enabled = checkBox16.Checked;
        }
        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown15.Enabled = !checkBox17.Checked;
        }
        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown16.Enabled = numericUpDown17.Enabled = label10.Enabled = !checkBox18.Checked;
        }
        #endregion
        #region RadioButton
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                toolTip1.SetToolTip(button5, "Apply the changes to the level. This will append the recorded guidelines to the original ones.");
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                toolTip1.SetToolTip(button5, "Apply the changes to the level. This will replace the original guidelines with the recorded ones.");
        }
        #endregion
        #region DataGridViews
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ChangeBeatColorValue(e.ColumnIndex, e.RowIndex);
            else if (e.Button == MouseButtons.Right)
                RemoveBeatGuideline(e.ColumnIndex, e.RowIndex);
        }
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (IsMouseClickedInDataGridView)
            {
                if (MouseLeftClick)
                    ChangeBeatColorValue(e.ColumnIndex, e.RowIndex);
                else if (MouseRightClick)
                    RemoveBeatGuideline(e.ColumnIndex, e.RowIndex);
            }
        }
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseClickedInDataGridView = true;
            MouseLeftClick = e.Button == MouseButtons.Left;
            MouseRightClick = e.Button == MouseButtons.Right;
        }
        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                MouseLeftClick = false;
            if (e.Button == MouseButtons.Right)
                MouseRightClick = false;
            IsMouseClickedInDataGridView = (MouseLeftClick || MouseRightClick);
        }
        #endregion

        public static decimal AdjustValueToClosestPowerOf2(decimal input)
        {
            double value = 1;

            for (int i = 0; i < 7; i++)
                if ((Math.Pow(2, i) <= (double)input) && ((double)input <= Math.Pow(2, i + 1)))
                {
                    value = Math.Pow(2, i);
                    if (Math.Pow(2, i + 1) - (double)input <= (double)input - Math.Pow(2, i))
                        value *= 2;
                }
            return (decimal)value;
        }
        public static string GetSongLocation(int levelIndex)
        {
            return Gamesave.GetCustomSongLocation(levelIndex);
        }

        void PrepareInitialization()
        {
            DownloadMainLevelGuidelinesOnStore.WorkerSupportsCancellation = true;
            DownloadMainLevelGuidelinesOnStore.DoWork += DownloadMainLevelGuidelineStringOnStore;
            DownloadSelectedLevelsGuidelinesOnStore.WorkerSupportsCancellation = true;
            DownloadSelectedLevelsGuidelinesOnStore.DoWork += DownloadSelectedLevelsGuidelineStringsOnStore;
            comboBox5.SelectedIndex = 0;
            ComboBox[] CBs = { comboBox1, comboBox2, comboBox6 };
            for (int i = 0; i < CBs.Length; i++)
                CBs[i].Text = "Orange";
            for (int i = 0; i < UserLevelCount; i++)
                comboBox3.Items.Add(UserLevels[i].LevelName);
            comboBox3.SelectedIndexChanged += new EventHandler(comboBox3_SelectedIndexChanged);
            comboBox3.SelectedIndex = 0;
            numericUpDown14.Maximum = UserLevelCount;
            GuidelinesString = UserLevels[CurrentLevelIndex].LevelGuidelinesString;
            LevelGuidelines = Gamesave.GetGuidelines(GuidelinesString);
            GuidelinesCount = LevelGuidelines.Count;
            if (GuidelinesCount > 0)
            {
                numericUpDown1.Maximum = GuidelinesCount;
                ShowGuidelineInfo();
            }
            numericUpDown11.Maximum = numericUpDown1.Maximum;
            CheckForGuidelineExistence();
        }

        private void DownloadSelectedLevelsGuidelineStringsOnStore(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < RequestedSongsGuidelines.Count; i++)
            {
                SetupWebObject(ref StoreSelectedLevelsGuidelineStringsDownloader);
                string response = StoreSelectedLevelsGuidelineStringsDownloader.UploadString("http://someeffect.altervista.org/getStoreGuidelineString.php", "sID=" + RequestedSongsGuidelines[i].ToString());
                if (response != "-1")
                {
                    File.WriteAllText("EffectSome/Guidelines/" + RequestedSongsGuidelines[i].ToString() + ".dat", response);
                    button35.Enabled = true;
                }
            }
        }
        private void DownloadMainLevelGuidelineStringOnStore(object sender, DoWorkEventArgs e)
        {
            SetupWebObject(ref StoreMainLevelGuidelineStringDownloader);
            try
            {
                string response = StoreMainLevelGuidelineStringDownloader.UploadString("http://someeffect.altervista.org/getStoreGuidelineString.php", "sID=" + UserLevels[CurrentLevelIndex].LevelCustomSongID.ToString());
                if (response != "-1")
                    File.WriteAllText("EffectSome/Guidelines/" + UserLevels[CurrentLevelIndex].LevelCustomSongID.ToString() + ".dat", response);
                button34.Enabled = response != "-1";
            }
            catch (NotSupportedException)
            {
                button34.Enabled = false;
            }
        }

        void ChangeColors(int columnIndex, int rowIndex, Color c)
        {
            dataGridView1[columnIndex, rowIndex].Style.BackColor = c;
            dataGridView1[columnIndex, rowIndex].Style.SelectionBackColor = c;
        }
        void PauseGuidelineRecording(bool showPauseMessage)
        {
            RecordingPaused = true;
            label15.Enabled = numericUpDown18.Enabled = true;
            RecordedGuidelineString = Gamesave.CreateGuidelineString(RecordedGuidelines);
            timer.Stop();
            button8.Enabled = false;
            CheckForRecordedGuidelines();
            if (showPauseMessage)
                notification.ShowBalloonTip(5000, "Recording Paused", "The recording has been paused. The currently recorded guidelines are temporarily saved.", ToolTipIcon.Info);
        }
        void RecordGuideline(double timeStamp, double color)
        {
            if (RecordingPaused)
            {
                RecordingPaused = false;
                timer.Start();
                button8.Enabled = true;
                label15.Enabled = numericUpDown18.Enabled = false;
            }
            RecordedGuidelinesCount++;
            RecordedGuidelines = RecordedGuidelines.Add(timeStamp, color);
            button9.Text = ((RecordedGuidelinesCount != 1) ? RecordedGuidelinesCount + " guidelines are recorded." : "1 guideline is recorded.");
            if (RecordedGuidelinesCount == 69)
                button9.ForeColor = Color.FromArgb(192, 0, 0);
            else if (RecordedGuidelinesCount == 420)
                button9.ForeColor = Color.FromArgb(0, 128, 0);
            else if (RecordedGuidelinesCount == 1337)
                button9.ForeColor = Color.FromArgb(0, 0, 192);
            else
                button9.ForeColor = Color.FromArgb(0, 192, 0);
            button9.Focus();
        }
        void UpdateGuidelines()
        {
            GuidelinesString = Gamesave.CreateGuidelineString(LevelGuidelines);
            Gamesave.SetGuidelineString(GuidelinesString, CurrentLevelIndex);
            GuidelinesCount = Gamesave.GetNumberOfGuidelines(CurrentLevelIndex);
            notification.ShowBalloonTip(5000, "Successful Edit!", "The guidelines were successfully edited.", ToolTipIcon.Info);
        }
        void UpdateArrays()
        {
            LevelGuidelines = Gamesave.GetGuidelines(GuidelinesString);
        }
        void ChangeSelectedGuidelines(ref List<int> selection)
        {
            if (checkBox7.Checked) // Select all
                for (int i = 0; i < LevelGuidelines.Count; i++)
                    selection.Add(i);
            else // Don't select all; filters apply
            {
                for (int i = 0; i < LevelGuidelines.Count && checkBox6.Checked; i++) // Filter guidelines by their color
                    if (LevelGuidelines[i].Color == (0.80 + comboBox6.SelectedIndex * 0.10))
                        selection.Add(i);
                if (radioButton6.Checked) // Select guidelines before the specifided timestamp
                {
                    for (int i = 0; i < LevelGuidelines.Count && checkBox5.Checked; i++)
                        if (LevelGuidelines[i].TimeStamp < (double)numericUpDown10.Value)
                            selection.Add(i);
                }
                else // Select guidelines after the specifided timestamp
                    for (int i = 0; i < LevelGuidelines.Count && checkBox5.Checked; i++)
                        if (LevelGuidelines[i].TimeStamp > (double)numericUpDown10.Value)
                            selection.Add(i);
                if (radioButton8.Checked) // Select guidelines before the specified index
                    for (int i = 0; i < (int)numericUpDown11.Value - 1 && checkBox8.Checked; i += (int)numericUpDown2.Value)
                        selection.Add(i);
                else // Select guidelines after the specified index
                    for (int i = (int)numericUpDown11.Value - 1; i < LevelGuidelines.Count && checkBox8.Checked; i += (int)numericUpDown2.Value)
                        selection.Add(i);
                if (checkBox8.Checked && !checkBox3.Checked) // Include the guideline at the specified index
                    selection.Remove((int)numericUpDown11.Value - 1);
            }
            CheckForSelectedGuidelines();
            selection.RemoveDuplicates();
        }
        void ChangeSelectedLevels(ref List<int> selection)
        {
            button35.Enabled = false;
            if (checkBox4.Checked) // Select all
                for (int i = 0; i < UserLevelCount; i++)
                    selection.Add(i);
            else // Don't select all; filters apply
            {
                for (int i = 0; i < UserLevelCount && checkBox10.Checked; i++) // Filter levels by their names
                {
                    if (checkBox11.Checked) // Contains
                    {
                        if (checkBox12.Checked) // Match case
                        {
                            if (UserLevels[i].LevelName.Find(textBox2.Text) > -1)
                                selection.Add(i);
                        }
                        else // Case-free search
                        {
                            if (UserLevels[i].LevelName.ToLower().Find(textBox2.Text.ToLower()) > -1)
                                selection.Add(i);
                        }
                    }
                    else // Is exact name
                    {
                        if (checkBox12.Checked) // Match case
                        {
                            if (UserLevels[i].LevelName == textBox2.Text)
                                selection.Add(i);
                        }
                        else // Case-free search
                            if (UserLevels[i].LevelName.MatchesStringCaseFree(textBox2.Text))
                                selection.Add(i);
                    }
                }
                for (int i = 0; i < UserLevelCount && checkBox9.Checked; i++) // Filter levels by Song IDs
                {
                    if (checkBox13.Checked) // Filter all levels with the same Custom Song as the main level
                    {
                        if (UserLevels[CurrentLevelIndex].LevelCustomSongID == UserLevels[i].LevelCustomSongID)
                            selection.Add(i);
                    }
                    else
                    {
                        if (textBox1.Text.Length == 0) // Filter all levels with Custom Song if the Song ID is not specified
                        {
                            if (UserLevels[i].LevelCustomSongID > 0)
                                selection.Add(i);
                        }
                        else // Filter all levels with Custom Song with the specified Song ID
                        {
                            try
                            {
                                if (UserLevels[i].LevelCustomSongID == Convert.ToInt32(textBox1.Text))
                                    selection.Add(i);
                            }
                            catch { }
                        }
                    }
                }
                if (checkBox17.Checked) // Select levels with the same revision as the main level
                {
                    for (int i = 0; i < UserLevelCount && checkBox16.Checked; i++)
                        if (UserLevels[i].LevelRevision == UserLevels[CurrentLevelIndex].LevelRevision)
                            selection.Add(i);
                }
                else // Select levels with the same revision as the main level
                    for (int i = 0; i < UserLevelCount && checkBox16.Checked; i++)
                        if (UserLevels[i].LevelRevision == numericUpDown15.Value)
                            selection.Add(i);
                if (radioButton4.Checked) // Select levels before the specified index
                    for (int i = 0; i < (int)numericUpDown14.Value - 1 && checkBox15.Checked; i += (int)numericUpDown13.Value)
                        selection.Add(i);
                else // Select levels after the specified index
                    for (int i = (int)numericUpDown14.Value - 1; i < UserLevelCount && checkBox15.Checked; i += (int)numericUpDown13.Value)
                        selection.Add(i);
                if (checkBox15.Checked && !checkBox14.Checked) // Include the level at the specified index
                    selection.Remove((int)numericUpDown14.Value - 1);
            }
            CheckForSelectedLevels();
            selection = selection.RemoveDuplicates();
            GetSelectionRequestedSongs();
            DownloadSelectedLevelsGuidelinesOnStore.RunWorkerAsync();
        }
        void CreateGuidelines()
        {
            ParseCurrentCreationPresetData();
            int measure = 1; // The current measure
            int presetMeasure = measure;
            int beat = 1; // The current beat
            int preset = 0; // The current preset index
            double length = FileMetadata.GetSongLength(GetSongLocation(CurrentLevelIndex)); // The length of the song
            double currentTimeStamp = (double)numericUpDown3.Value; // The current timestamp
            List<double> intervals = new List<double>(); // The intervals of each beat, per preset

            for (int i = 0; i < CurrentCreationPresets.Count; i++) // Set the intervals of the presets
                intervals.Add(60 / Presets[CurrentCreationPresets[i][0]].BPM / ((double)(16 / Presets[CurrentCreationPresets[i][0]].TimeSignature.Denominator)).OneOrGreater());
            
            while (checkBox18.Checked ? currentTimeStamp <= length : (measure <= numericUpDown17.Value && (measure < numericUpDown17.Value || beat < numericUpDown16.Value * ((double)(16 / Presets[CurrentCreationPresets[preset][0]].TimeSignature.Denominator)).OneOrGreater()))) // Create the guidelines
            {
                if (measure > (int)numericUpDown6.Value || (measure == (int)numericUpDown6.Value && beat >= (int)numericUpDown12.Value)) // If the currently processed measure is at least at or beyond the starting point
                {
                    int index = LevelGuidelines.FindIndexToInsertGuideline(currentTimeStamp);
                    double color = (double)Presets[CurrentCreationPresets[preset][0]].Colors[presetMeasure - 1][beat - 1];
                    if (color == 0.80 || color == 0.90 || color == 1.00)
                        LevelGuidelines = LevelGuidelines.Insert(index, currentTimeStamp, color);
                }
                beat++;
                if (beat > Presets[CurrentCreationPresets[preset][0]].TimeSignature.Beats * ((double)(16 / Presets[CurrentCreationPresets[preset][0]].TimeSignature.Denominator)).OneOrGreater()) // If the beat of the measure has exceeded the measure's beat count, reset to 1 and increase the measure number
                {
                    measure++;
                    presetMeasure++;
                    beat = 1;
                }
                if (presetMeasure > Presets[CurrentCreationPresets[preset][0]].Colors.Count) // If the measure of the preset has exceeded the count of the measures of the preset, reset to 1
                    presetMeasure = 1;
                if (preset < CurrentCreationPresets.Count - 1) // If the preset is not the last one
                    if (measure >= CurrentCreationPresets[preset + 1][1]) // If the currently processed measure is greater or equal to the next preset's starting measure proceed to the next preset
                        preset++;
                currentTimeStamp += intervals[preset];
            }

            UpdateGuidelines();
            CheckForGuidelineExistence();
            notification.ShowBalloonTip(5000, "Guidelines Created!", "The guidelines were successfully created.", ToolTipIcon.Info);
            CurrentCreationPresets.Clear();
        }
        void ParseCurrentCreationPresetData()
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                CurrentCreationPresets.Add(new int[2]);
                CurrentCreationPresets[i][0] = (dataGridView2[0, i] as DataGridViewComboBoxCell).GetSelectedIndex();
                CurrentCreationPresets[i][1] = Convert.ToInt32(dataGridView2[1, i].Value.ToString());
            }
        }
        void CheckForSelectedGuidelines()
        {
            groupBox1.Enabled = button12.Enabled = button14.Enabled = SelectedGuidelineIndices.Count > 0;
            ShowSelectedGuidelines();
        }
        void CheckForSelectedLevels()
        {
            button16.Enabled = button18.Enabled = groupBox16.Enabled = SelectedLevelIndices.Count > 0;
            ShowSelectedLevels();
        }
        void CheckForGuidelineExistence()
        {
            GuidelinesCount = LevelGuidelines.Count;
            groupBox2.Enabled = groupBox6.Enabled = button36.Enabled = button29.Enabled = LevelGuidelines.Count != 0;
            if (GuidelinesCount > 0)
            {
                if (numericUpDown1.Value > GuidelinesCount)
                    numericUpDown1.Value = GuidelinesCount;
                numericUpDown1.Maximum = GuidelinesCount;
                ShowGuidelineInfo();
            }
        }
        void ShowGuidelineInfo()
        {
            if (GuidelinesCount > 0)
            {
                DisableEvents();
                numericUpDown5.Value = (decimal)LevelGuidelines[(int)numericUpDown1.Value - 1].TimeStamp;
                if (LevelGuidelines[(int)numericUpDown1.Value - 1].Color == 1.00)
                    comboBox1.Text = "Green";
                else if (LevelGuidelines[(int)numericUpDown1.Value - 1].Color == 0.90)
                    comboBox1.Text = "Yellow";
                else
                    comboBox1.Text = "Orange";
                EnableEvents();
            }
        }
        void CheckForRecordedGuidelines()
        {
            button5.Enabled = button10.Enabled = button37.Enabled = RecordedGuidelinesCount != 0;
            button9.Text = (RecordedGuidelinesCount != 0) ? "Continue Recording" : "Start Recording";
        }
        void ResetRecordedGuidelines()
        {
            RecordedGuidelinesCount = 0;
            RecordedGuidelineString = "";
            RecordedGuidelines.Clear();
            RecordTime = new TimeSpan(0, 0, 0, 0, (int)(numericUpDown18.Value * 1000));
            CheckForRecordedGuidelines();
        }
        void DisableEvents()
        {
            numericUpDown5.ValueChanged -= new EventHandler(numericUpDown5_ValueChanged);
            comboBox1.SelectedIndexChanged -= new EventHandler(comboBox1_SelectedIndexChanged);
        }
        void EnableEvents()
        {
            numericUpDown5.ValueChanged += new EventHandler(numericUpDown5_ValueChanged);
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
        }
        void DeleteSelectedGuidelines()
        {
            SelectedGuidelineIndices.Clear();
            ChangeSelectedGuidelines(ref SelectedGuidelineIndices);
            for (int i = SelectedGuidelineIndices.Count - 1; i >= 0; i--)
                LevelGuidelines.RemoveAt(SelectedGuidelineIndices[i]);
            UpdateGuidelines();
            SelectedGuidelineIndices.Clear();
            CheckForSelectedGuidelines();
            CheckForGuidelineExistence();
        }
        void GetSelectionRequestedSongs()
        {
            RequestedSongsGuidelines.Clear();
            for (int i = 0; i < SelectedLevelIndices.Count; i++)
                RequestedSongsGuidelines.Add(UserLevels[SelectedLevelIndices[i]].LevelCustomSongID);
        }
        void ShowSelectedLevels()
        {
            string selectedLevels = "";
            if (SelectedLevelIndices.Count > 0)
            {
                selectedLevels = "Selected Levels:\n\n";
                for (int i = 0; i < SelectedLevelIndices.Count; i++)
                    selectedLevels += UserLevels[SelectedLevelIndices[i]].LevelNameWithRevision + "\n";
                selectedLevels.Remove(selectedLevels.Length - 1);
            }
            else
                selectedLevels = "No levels selected.";
            toolTip1.SetToolTip(groupBox12, selectedLevels);
        }
        void ShowSelectedGuidelines()
        {
            string selectedGuidelines = "";
            if (SelectedGuidelineIndices.Count > 0) // Determines whether there are any selected guidelines
            {
                if (SelectedGuidelineIndices.Count < LevelGuidelines.Count) // Determines whether the count of the selected guidelines is less than the count of the guidelines of the selected level
                    selectedGuidelines = "Selected Guidelines:\n\n" + SelectedGuidelineIndices.ToArray().IncrementByOne().ShowValuesWithRanges();
                else if (SelectedGuidelineIndices.Count == LevelGuidelines.Count) // Determines whether the count of the selected guidelines is equal to the count of the guidelines of the selected level
                    selectedGuidelines = "Selected Guidelines: All";
            }
            else // Determines whether there are no selected guidelines
                selectedGuidelines = "No guidelines selected.";
            toolTip1.SetToolTip(groupBox4, selectedGuidelines);
        }

        void AddMeasure()
        {
            dataGridView1.Rows.Add();
            Presets[CurrentPresetIndex].Colors.Add(new List<decimal>());
            while (dataGridView1.Columns.Count - 1 > Presets[CurrentPresetIndex].Colors[Presets[CurrentPresetIndex].Colors.Count - 1].Count)
                Presets[CurrentPresetIndex].Colors[Presets[CurrentPresetIndex].Colors.Count - 1].Add(0.70m);
            ShowMeasureNumbers();
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        void AddMeasure(int presetIndex)
        {
            dataGridView1.Rows.Add();
            Presets[presetIndex].Colors.Add(new List<decimal>());
            while (dataGridView1.Columns.Count - 1 > Presets[presetIndex].Colors[Presets[presetIndex].Colors.Count - 1].Count)
                Presets[presetIndex].Colors[Presets[presetIndex].Colors.Count - 1].Add(0.70m);
            ShowMeasureNumbers();
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        void AddMeasures(int no)
        {
            for (int i = 0; i < no; i++)
            {
                dataGridView1.Rows.Add();
                Presets[CurrentPresetIndex].Colors.Add(new List<decimal>());
                while (dataGridView1.Columns.Count - 1 > Presets[CurrentPresetIndex].Colors[Presets[CurrentPresetIndex].Colors.Count - 1].Count)
                    Presets[CurrentPresetIndex].Colors[Presets[CurrentPresetIndex].Colors.Count - 1].Add(0.70m);
            }
            ShowMeasureNumbers();
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        void RemoveMeasure()
        {
            dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            Presets[CurrentPresetIndex].Colors.RemoveAt(Presets[CurrentPresetIndex].Colors.Count - 1);
        }
        void ResetMeasures()
        {
            dataGridView1.Rows.Clear();
            Presets[CurrentPresetIndex].Colors.Clear();
            AddMeasure();
        }

        void AddPreset()
        {
            button3.Enabled = button15.Enabled = true;
            dataGridView2.EndEdit();
            dataGridView2.Rows.Add();
            for (int i = 0; i < comboBox5.Items.Count; i++)
                (dataGridView2[0, dataGridView2.Rows.Count - 1] as DataGridViewComboBoxCell).Items.Add(comboBox5.Items[i]);
            (dataGridView2[0, dataGridView2.Rows.Count - 1] as DataGridViewComboBoxCell).Value = (dataGridView2[0, dataGridView2.Rows.Count - 1] as DataGridViewComboBoxCell).Items[0];
            if (dataGridView2.Rows.Count < 2)
                dataGridView2[1, dataGridView2.Rows.Count - 1].Value = "1";
            else
                dataGridView2[1, dataGridView2.Rows.Count - 1].Value = (Convert.ToInt32(dataGridView2[1, dataGridView2.Rows.Count - 2].Value) + Presets[(dataGridView2[0, dataGridView2.Rows.Count - 2] as DataGridViewComboBoxCell).GetSelectedIndex()].Colors.Count).ToString();
        }
        void RemovePreset()
        {
            dataGridView2.Rows.RemoveAt(dataGridView2.Rows.Count - 1);
            if (dataGridView2.Rows.Count < 1)
                button3.Enabled = button15.Enabled = false;
        }

        void SetInitialBPM()
        {
            if (CustomSongBPMs.Count > UserLevels[CurrentLevelIndex].LevelCustomSongID && UserLevels[CurrentLevelIndex].LevelCustomSongID > 0 && CustomSongBPMs[UserLevels[CurrentLevelIndex].LevelCustomSongID] > 0)
            {
                BPM = CustomSongBPMs[UserLevels[CurrentLevelIndex].LevelCustomSongID];
                if (numericUpDown7.Value == (decimal)BPM)
                    numericUpDown7.ForeColor = Color.Black;
                else
                    numericUpDown7.ForeColor = Color.Blue;
            }
            else
            {
                BPM = 130;
                numericUpDown7.ForeColor = Color.Red;
            }
        }
        void ChangeBeatColorValue(int column, int row)
        {
            if (row > -1 && column > 0)
            {
                Presets[CurrentPresetIndex].Colors[row][column - 1] = ((int)Math.Round(Presets[CurrentPresetIndex].Colors[row][column - 1] * 10) - 7 + 1) % 4 / 10m + 0.70m;
                switch (Presets[CurrentPresetIndex].Colors[row][column - 1])
                {
                    case 0.80m:
                        ChangeColors(column, row, Color.Orange);
                        break;
                    case 0.90m:
                        ChangeColors(column, row, Color.Yellow);
                        break;
                    case 1.00m:
                        ChangeColors(column, row, Color.Lime);
                        break;
                    default:
                        ChangeColors(column, row, Color.Black);
                        break;
                }
            }
        }
        void RemoveBeatGuideline(int column, int row)
        {
            if (row > -1 && column > 0)
            {
                Presets[CurrentPresetIndex].Colors[row][column - 1]  = 0.70m;
                ChangeColors(column, row, Color.Black);
            }
        }
        void ShowMeasuresInDataGridView(int presetIndex)
        {
            for (int i = 0; i < Presets[presetIndex].Colors.Count; i++)
            {
                dataGridView1.Rows.Add();

                for (int j = 0; j < Presets[presetIndex].Colors[i].Count; j++)
                {
                    switch (Presets[presetIndex].Colors[i][j])
                    {
                        case 0.80m:
                            ChangeColors(j + 1, i, Color.Orange);
                            break;
                        case 0.90m:
                            ChangeColors(j + 1, i, Color.Yellow);
                            break;
                        case 1.00m:
                            ChangeColors(j + 1, i, Color.Lime);
                            break;
                        default:
                            ChangeColors(j + 1, i, Color.Black);
                            break;
                    }
                }
            }
            ShowMeasureNumbers();
        }
        void ShowMeasureNumbers()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1[0, i].Value = (i + 1).ToString();
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        void LoadPreset(int presetIndex)
        {
            dataGridView1.Rows.Clear();
            ShowMeasuresInDataGridView(presetIndex);
            // Do this to prevent controls from messing with the preset
            decimal presetBPM = (decimal)Presets[presetIndex].BPM;
            int timeSignature0 = Presets[presetIndex].TimeSignature.Beats;
            int timeSignature1 = Presets[presetIndex].TimeSignature.Denominator;
            numericUpDown9.Value = timeSignature1;
            numericUpDown8.Value = timeSignature0;
            numericUpDown7.Value = presetBPM;
        }
        void SavePreset(int presetIndex)
        {
            string fileData = numericUpDown7.Value.ToString() + "|" + numericUpDown8.Value.ToString() + "/" + numericUpDown9.Value.ToString() + "|";
            string presetData = "";
            for (int i = 0; i < Presets[presetIndex].Colors.Count; i++)
            {
                for (int j = 0; j < Presets[presetIndex].Colors[i].Count; j++)
                    presetData += Presets[presetIndex].Colors[i][j].ToString() + ":";
                if (presetData.Length > 0)
                {
                    presetData = presetData.Remove(presetData.Length - 1);
                    presetData += ";";
                }
            }
            fileData += presetData;
            File.WriteAllText(appLocation + @"\EffectSome\Presets\Guideline Editor\" + comboBox5.Items[presetIndex].ToString() + ".esf", fileData);
        }
        void CreatePreset(string presetName)
        {
            Presets.Add(new GuidelineEditorPreset(presetName, BPM, (TimeSignatures[0], TimeSignatures[1]), new List<List<decimal>>()));
            string fileData = BPM.ToString() + "|" + TimeSignatures[0].ToString() + "/" + TimeSignatures[1].ToString() + "|";
            File.WriteAllText(appLocation + @"\EffectSome\Presets\Guideline Editor\" + presetName + ".esf", fileData);
            double newPresetBPM = BPM;
            while ((decimal)newPresetBPM < numericUpDown7.Minimum)
                newPresetBPM *= 2;
            while ((decimal)newPresetBPM > numericUpDown7.Maximum)
                newPresetBPM /= 2;
            numericUpDown7.Value = (decimal)newPresetBPM;
            numericUpDown8.Value = TimeSignatures[0];
            numericUpDown9.Value = TimeSignatures[1];
            comboBox5.Items.Add(presetName);
            comboBox5.SelectedItem = presetName;
            CurrentPresetIndex = comboBox5.SelectedIndex;
            LoadPreset(CurrentPresetIndex);
        }
        void CopyPreset(int presetIndex)
        {
            string currentPresetName = Presets[presetIndex].Name.RemoveLastNumber();
            string presetName;
            if (currentPresetName[currentPresetName.Length - 1] != ' ')
                currentPresetName += " ";
            int tempCurrentPreset = CurrentPresetIndex;
            for (int i = 1; ; i++)
                if (!File.Exists("EffectSome/Presets/Guideline Editor/" + currentPresetName + i.ToString() + ".esf"))
                {
                    presetName = currentPresetName + i.ToString();
                    break;
                }
            Presets.Add(Presets[tempCurrentPreset].Clone());
            string fileData = $"{Presets[tempCurrentPreset].BPM}|{Presets[tempCurrentPreset].TimeSignature.Beats}/{Presets[tempCurrentPreset].TimeSignature.Denominator}|";
            File.WriteAllText(appLocation + @"\EffectSome\Presets\Guideline Editor\" + presetName + ".esf", fileData);
            double newPresetBPM = Presets[tempCurrentPreset].BPM;
            while ((decimal)newPresetBPM < numericUpDown7.Minimum)
                newPresetBPM *= 2;
            while ((decimal)newPresetBPM > numericUpDown7.Maximum)
                newPresetBPM /= 2;
            numericUpDown7.Value = (decimal)newPresetBPM;
            numericUpDown8.Value = Presets[tempCurrentPreset].TimeSignature.Beats;
            numericUpDown9.Value = Presets[tempCurrentPreset].TimeSignature.Denominator;
            comboBox5.Items.Add(presetName);
            comboBox5.SelectedItem = presetName;
            CurrentPresetIndex = comboBox5.SelectedIndex;
            LoadPreset(CurrentPresetIndex);
        }
        void LoadPresets()
        {
            Presets.Clear();
            foreach (string preset in Directory.GetFiles(appLocation + @"\EffectSome\Presets\Guideline Editor"))
            {
                string presetName = preset.Split('\\').Last();
                presetName = presetName.Split('.').ExcludeLast(1).Combine();
                string[] rawPresetData = File.ReadAllText(preset).Split('|');
                string[] timeSignature = rawPresetData[1].Split('/');
                Presets.Add(new GuidelineEditorPreset(presetName, Convert.ToDouble(rawPresetData[0]), (Convert.ToInt32(timeSignature[0]), Convert.ToInt32(timeSignature[1])), new List<List<decimal>>()));
                if (rawPresetData[2].Length > 0)
                    try
                    {
                        string[] measures = rawPresetData[2].Split(';').ExcludeLast(1);
                        decimal[,] beats = measures.Split(':').ToDecimalArray();
                        Presets[Presets.Count - 1].Colors = beats.ToList();
                    }
                    catch { }
                else
                    AddMeasure(Presets.Count - 1);
                comboBox5.Items.Add(presetName);
            }
        }
        public static void ReloadPresets()
        {
            Presets.Clear();
            foreach (string preset in Directory.GetFiles(appLocation + @"\EffectSome\Presets\Guideline Editor"))
            {
                string presetName = preset.Split('\\').Last();
                presetName = presetName.Split('.').ExcludeLast(1).Combine();
                string[] rawPresetData = File.ReadAllText(preset).Split('|');
                string[] timeSignature = rawPresetData[1].Split('/');
                Presets.Add(new GuidelineEditorPreset(presetName, Convert.ToDouble(rawPresetData[0]), (Convert.ToInt32(timeSignature[0]), Convert.ToInt32(timeSignature[1])), new List<List<decimal>>()));
                if (rawPresetData[2].Length > 0)
                    try
                    {
                        string[] measures = rawPresetData[2].Split(';').ExcludeLast(1);
                        decimal[,] beats = measures.Split(':').ToDecimalArray();
                        Presets[Presets.Count - 1].Colors = beats.ToList();
                    }
                    catch { }
                else
                {
                    Presets[CurrentPresetIndex].Colors.Add(new List<decimal>());
                    while (Presets[CurrentPresetIndex].Colors[Presets[CurrentPresetIndex].Colors.Count - 1].Count < 16) // Change this on the Time Signature update
                        Presets[CurrentPresetIndex].Colors[Presets[CurrentPresetIndex].Colors.Count - 1].Add(0.70m);
                }
            }
        }

        void CheckSongType(int levelIndex)
        {
            try { groupBox4.Enabled = groupBox11.Enabled = button24.Enabled = UserLevels[levelIndex].LevelCustomSongID > 0; }
            catch { groupBox4.Enabled = groupBox11.Enabled = button24.Enabled = false; }
        }

        public static void Timer_Elapsed()
        {
            RecordTime = RecordTime.Add(new TimeSpan(0, 0, 0, 0, 1));
        }

    }
}