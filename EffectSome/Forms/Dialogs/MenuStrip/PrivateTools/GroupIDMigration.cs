using EffectSome.Objects.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EffectSome.Forms.Dialogs.MenuStrip.PrivateTools
{
    public partial class GroupIDMigration : Form
    {
        public static bool IsOpen = false;

        public bool IsRunning = false;
        private BackgroundWorker analyzer;

        public bool IsLoadingRanges = false;
        public List<SourceTargetRange> Ranges = new List<SourceTargetRange>();

        private int currentLevelIndex = -1;
        private int currentRangeIndex = -1;
        private int sourceFrom = 0;
        private int sourceTo = 0;
        private int targetFrom = 0;
        private int targetTo = 0;

        private double stepProgressValue = 0;
        private int stepsCompleted = -1;

        public int SourceFrom
        {
            get => sourceFrom;
            set
            {
                sourceFrom = value;
                sourceFromNUD.Value = value;
                if (!IsLoadingRanges)
                {
                    if (sourceFrom > sourceTo)
                        SourceTo = value;
                    Ranges[currentRangeIndex].SourceFrom = sourceFrom;
                    Ranges[currentRangeIndex].SourceTo = sourceTo;
                    CheckRange();
                }
            }
        }
        public int SourceTo
        {
            get => sourceTo;
            set
            {
                sourceTo = value;
                sourceToNUD.Value = value;
                if (!IsLoadingRanges)
                {
                    if (sourceFrom > sourceTo)
                        SourceFrom = value;
                    Ranges[currentRangeIndex].SourceFrom = sourceFrom;
                    Ranges[currentRangeIndex].SourceTo = sourceTo;
                    CheckRange();
                }
            }
        }
        public int TargetFrom
        {
            get => targetFrom;
            set
            {
                targetFrom = value;
                targetFromNUD.Value = value;
                if (!IsLoadingRanges)
                {
                    Ranges[currentRangeIndex].TargetFrom = targetFrom;
                    CheckRange();
                }
            }
        }
        public int TargetTo
        {
            get => targetTo;
            set
            {
                targetTo = value;
                targetToNUD.Value = value;
                if (!IsLoadingRanges)
                    Ranges[currentRangeIndex].TargetTo = targetTo;
            }
        }
        public int Range => sourceTo - sourceFrom;
        
        public GroupIDMigration()
        {
            InitializeComponent();
            progressBar1.Maximum = progressBar1.Width;
            progressBar2.Maximum = progressBar2.Width;

            for (int i = 0; i < EffectSome.UserLevelCount; i++)
                comboBox1.Items.Add(EffectSome.UserLevels[i].LevelNameWithRevision);
            comboBox1.SelectedIndex = 0;

            analyzer = new BackgroundWorker();
            analyzer.WorkerSupportsCancellation = true;
            analyzer.WorkerReportsProgress = true;
            analyzer.RunWorkerCompleted += ChangeStatusToComplete;
            analyzer.ProgressChanged += ReportProgress;
            analyzer.DoWork += PerformSteps;
        }

        private void GroupIDMigration_Load(object sender, EventArgs e)
        {
            IsOpen = true;
        }
        private void GroupIDMigration_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsOpen = false;
        }
        private void sourceFromNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoadingRanges)
                SourceFrom = (int)sourceFromNUD.Value;
        }
        private void sourceToNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoadingRanges)
                SourceTo = (int)sourceToNUD.Value;
        }
        private void targetFromNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoadingRanges)
                TargetFrom = (int)targetFromNUD.Value;
        }
        private void targetToNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoadingRanges)
                TargetTo = (int)targetToNUD.Value;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentRangeIndex = listBox1.SelectedIndex;
            groupBox1.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = currentRangeIndex >= 0;
            if (currentRangeIndex >= 0)
                LoadRange(listBox1.SelectedIndex);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Ranges.Add(new SourceTargetRange(0, 0, 0, 0));
            listBox1.Items.Add(Ranges.Last());
            listBox1.SelectedIndex = currentRangeIndex = Ranges.Count - 1;
            LoadRange(currentRangeIndex);
            button5.Enabled = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Ranges.RemoveAt(currentRangeIndex);
            listBox1.Items.RemoveAt(currentRangeIndex);
            button5.Enabled = Ranges.Count > 0;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (!IsRunning)
            {
                ChangeStatusToActive();
                analyzer.RunWorkerAsync();
                stepProgressValue = EffectSome.UserLevels[currentLevelIndex].LevelObjectCount;
            }
            else
                analyzer.CancelAsync();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Ranges = SourceTargetRange.LoadRangesFromStringArray(File.ReadAllLines(openFileDialog1.FileName));
                listBox1.Items.Clear();
                foreach (var item in Ranges)
                    listBox1.Items.Add(item);
                button5.Enabled = Ranges.Count > 0;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentLevelIndex = comboBox1.SelectedIndex;
            objectsLabel.Text = $"Level Objects: {EffectSome.UserLevels[currentLevelIndex].LevelObjectCount}";
        }

        private void PerformSteps(object sender, DoWorkEventArgs e)
        {
            // Just fucking get the objects nonetheless
            string objString = EffectSome.UserLevels[currentLevelIndex].GetObjectString();
            EffectSome.UserLevels[currentLevelIndex].LevelObjects = Gamesave.GetObjects(objString);
            int objCount = EffectSome.UserLevels[currentLevelIndex].LevelObjectCount;
            int previousProgress = -1;
            for (int s = 0; s < Ranges.Count; s++)
            {
                analyzer.ReportProgress(previousProgress = 0);
                int d = Ranges[s].Difference;
                for (int i = 0; i < objCount;)
                {
                    AdjustGroups(EffectSome.UserLevels[currentLevelIndex].LevelObjects[i], Ranges[s]);
                    int p = (int)(++i / (double)objCount * progressBar1.Maximum);
                    if (p > previousProgress)
                        analyzer.ReportProgress(previousProgress = p);
                }
            }
            // WARNING: THIS MIGHT BE THE DANGER ZONE
            string ls = EffectSome.UserLevels[currentLevelIndex].DecryptedLevelString;
            Gamesave.SetLevelString(ls.Replace(Gamesave.GetObjectString(ls), EffectSome.UserLevels[currentLevelIndex].GetObjectString()), currentLevelIndex);
        }

        private void AdjustGroups(LevelObject o, SourceTargetRange r)
        {
            int d = r.Difference;

            var groups = (int[])o[LevelObject.ObjectParameter.GroupIDs];
            if (groups != null)
                for (int g = 0; g < groups.Length; g++)
                    if (r.IsWithinSourceRange(groups[g]))
                        groups[g] += d;
            int? p;
            if ((p = (int?)o[LevelObject.ObjectParameter.CenterGroupID]).HasValue)
                if (r.IsWithinSourceRange(p.Value))
                    o[LevelObject.ObjectParameter.CenterGroupID] = p.Value + d;
            if ((p = (int?)o[LevelObject.ObjectParameter.FollowGroupID]).HasValue)
                if (r.IsWithinSourceRange(p.Value))
                    o[LevelObject.ObjectParameter.FollowGroupID] = p.Value + d;
            if ((p = (int?)o[LevelObject.ObjectParameter.TargetGroupID]).HasValue)
                if (r.IsWithinSourceRange(p.Value))
                    o[LevelObject.ObjectParameter.TargetGroupID] = p.Value + d;
            if ((p = (int?)o[LevelObject.ObjectParameter.TargetPosGroupID]).HasValue)
                if (r.IsWithinSourceRange(p.Value))
                    o[LevelObject.ObjectParameter.TargetPosGroupID] = p.Value + d;
        }
        
        private void ReportProgress(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
                statusLabel.Text = $"Steps: {++stepsCompleted + 1}/{Ranges.Count}";
            progressBar1.Value = (int)((stepsCompleted * progressBar1.Maximum + e.ProgressPercentage) / (double)(Ranges.Count * progressBar1.Maximum) * progressBar1.Maximum);
            progressBar2.Value = e.ProgressPercentage;
        }
        private void ChangeStatusToActive()
        {
            button5.Text = "Cancel Operation";
        }
        private void ChangeStatusToComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            statusLabel.Text = e.Cancelled ? "Cancelled" : "Complete";
            button5.Text = "Begin Operation";
            stepsCompleted = -1;
        }

        private void LoadRange(int index)
        {
            IsLoadingRanges = true;
            var r = Ranges[index];
            SourceFrom = r.SourceFrom;
            SourceTo = r.SourceTo;
            TargetFrom = r.TargetFrom;
            TargetTo = r.TargetTo;
            IsLoadingRanges = false;
        }
        private void CheckRange()
        {
            int range = sourceTo - sourceFrom;
            TargetTo = targetFrom + range;
            targetFromNUD.Maximum = 999 - range;
            IsLoadingRanges = true;
            listBox1.Items[currentRangeIndex] = Ranges[currentRangeIndex];
            IsLoadingRanges = false;
        }
    }
}
