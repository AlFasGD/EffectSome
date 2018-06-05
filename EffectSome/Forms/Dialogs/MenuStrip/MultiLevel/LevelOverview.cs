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
using System.Runtime.InteropServices;

namespace EffectSome
{
    public partial class LevelOverview : Form
    {
        public static bool IsOpen = false;
        public static List<int> selectedLevelIndices = new List<int>();
        public static bool requiresChangeCommission = false;
        public static bool isInEditMode = false;

        public LevelOverview()
        {
            IsOpen = true;
            InitializeComponent();
            LoadLevels();
            dataGridView1.Columns["LevelRevision"].ReadOnly = !Options.BoolDictionary["allowEditLevelRevisions"];
            dataGridView1.Columns["LevelSong"].ReadOnly = !Options.BoolDictionary["allowEditSongIDs"];
            dataGridView1.Columns["LevelVersion"].ReadOnly = !Options.BoolDictionary["allowEditLevelVersions"];
            dataGridView1.Columns["LevelID"].ReadOnly = !Options.BoolDictionary["allowEditLevelIDs"];
            UpdateTitle();
        }
        
        private void LevelOverview_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
        }
        #region Buttons
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                Gamesave.ImportLevelsFromFiles(openFileDialog1.FileNames); // Import the levels in memory and gamesave
                for (int i = openFileDialog1.FileNames.Length - 1; i >= 0; i--) // Show the imported levels
                    ShowLevel(i);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                Gamesave.ExportLevels(selectedLevelIndices.ToArray(), folderBrowserDialog1.SelectedPath); // Export the levels to the specifed folder
            EffectSome.notification.ShowBalloonTip(5000, "Export Successful", "The levels have been successfully exported.", ToolTipIcon.Info);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Gamesave.DeleteLevels(selectedLevelIndices.ToArray());
            for (int i = selectedLevelIndices.Count - 1; i >= 0; i--)
                dataGridView1.Rows.RemoveAt(selectedLevelIndices[i]);
            selectedLevelIndices.Clear();
            CheckForSelectedLevels();
            UpdateTitle();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Gamesave.CloneLevels(selectedLevelIndices.ToArray());
            for (int i = selectedLevelIndices.Count - 1; i >= 0; i--) // Show the cloned levels
                ShowLevel(i);
            for (int i = 0; i < selectedLevelIndices.Count; i++)
                selectedLevelIndices[i] = i;
            UpdateSelection();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Gamesave.CreateLevel();
            for (int i = 0; i < selectedLevelIndices.Count; i++) // Increase the selected level indices
                selectedLevelIndices[i]++;
            ShowLevel(0);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++) // Temporarily commit changes to the level data for all the values in the levels that were changed
            {
                if (Convert.ToInt32(dataGridView1["LevelFolder", i].Value.ToString()) != EffectSome.UserLevels[i].LevelFolder) // Change the name of the level
                    Gamesave.TemporarilySetLevelFolder(i, dataGridView1["LevelFolder", i].Value.ToString());
                if (dataGridView1["LevelName", i].Value.ToString() != EffectSome.UserLevels[i].LevelName) // Change the name of the level
                    Gamesave.TemporarilySetLevelName(i, dataGridView1["LevelName", i].Value.ToString());
                if (Convert.ToInt32(dataGridView1["LevelRevision", i].Value.ToString()) != EffectSome.UserLevels[i].LevelRevision) // Change the revision of the level
                    Gamesave.TemporarilySetLevelRevision(i, dataGridView1["LevelRevision", i].Value.ToString());
                if (((string)dataGridView1["LevelDescription", i].Value ?? "") != EffectSome.UserLevels[i].LevelDescription) // Change the description of the level
                    Gamesave.TemporarilySetLevelDescription(i, ((string)dataGridView1["LevelDescription", i].Value ?? ""));
                if (Convert.ToInt32(dataGridView1["LevelSong", i].Value.ToString()) != EffectSome.UserLevels[i].LevelCustomSongID) // Change the Song ID of the level
                    Gamesave.TemporarilySetCustomSongID(i, dataGridView1["LevelSong", i].Value.ToString());
                if (Convert.ToInt32(dataGridView1["LevelVersion", i].Value.ToString()) != EffectSome.UserLevels[i].LevelVersion) // Change the level version of the level
                    Gamesave.TemporarilySetLevelVersion(i, dataGridView1["LevelVersion", i].Value.ToString());
                if (Convert.ToInt32(dataGridView1["LevelID", i].Value.ToString()) != EffectSome.UserLevels[i].LevelID) // Change the Level ID of the level
                    Gamesave.TemporarilySetLevelID(i, dataGridView1["LevelID", i].Value.ToString());
            }
            Gamesave.UpdateLevelData(); // Write the new data to the file
            button6.Enabled = requiresChangeCommission = false;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            Gamesave.MoveLevelsUp(selectedLevelIndices.ToArray());
            for (int i = 0; i < selectedLevelIndices.Count; i++)
            {
                if (selectedLevelIndices[i] > 0)
                    ShowLevel(selectedLevelIndices[i] - 1, selectedLevelIndices[i] - 1);
                ShowLevel(selectedLevelIndices[i], selectedLevelIndices[i]);
            }
            for (int i = 0; i < selectedLevelIndices.Count; i++)
                if (selectedLevelIndices[i] > i)
                    selectedLevelIndices[i]--;
            UpdateSelection();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Gamesave.MoveLevelsDown(selectedLevelIndices.ToArray());
            for (int i = 0; i < selectedLevelIndices.Count; i++)
            {
                if (selectedLevelIndices[i] < EffectSome.UserLevelCount - 1)
                    ShowLevel(selectedLevelIndices[i] + 1, selectedLevelIndices[i] + 1);
                ShowLevel(selectedLevelIndices[i], selectedLevelIndices[i]);
            }
            for (int i = 0; i < selectedLevelIndices.Count; i++)
                if (selectedLevelIndices[i] < EffectSome.UserLevelCount - selectedLevelIndices.Count + i)
                    selectedLevelIndices[i]++;
            UpdateSelection();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            Gamesave.MoveLevelsToBottom(selectedLevelIndices.ToArray());
            int lvls = selectedLevelIndices.Count; // The level count that was moved to bottom
            selectedLevelIndices.Clear();
            for (int i = 1; i <= lvls; i++) // Add the indices of the levels that were moved to bottom
                selectedLevelIndices.Add(EffectSome.UserLevelCount - i);
            UpdateSelection();
            for (int i = 0; i < EffectSome.UserLevelCount; i++)
                ShowLevel(i, i);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            Gamesave.MoveLevelsToTop(selectedLevelIndices.ToArray());
            int lvls = selectedLevelIndices.Count; // The level count that was moved to top
            selectedLevelIndices.Clear();
            for (int i = 0; i < lvls; i++) // Add the indices of the levels that were moved to top
                selectedLevelIndices.Add(i);
            UpdateSelection();
            for (int i = 0; i < EffectSome.UserLevelCount; i++)
                ShowLevel(i, i);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < EffectSome.UserLevelCount; i++)
                dataGridView1[0, i].Value = true;
            selectedLevelIndices.Clear();
            for (int i = 0; i < EffectSome.UserLevelCount; i++)
                selectedLevelIndices.Add(i);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < EffectSome.UserLevelCount; i++)
                dataGridView1[0, i].Value = false;
            selectedLevelIndices.Clear();
        }
        private void button13_Click(object sender, EventArgs e)
        {
            Gamesave.SwapLevels(selectedLevelIndices[0], selectedLevelIndices[1]);
            ShowLevel(selectedLevelIndices[0], selectedLevelIndices[0]);
            ShowLevel(selectedLevelIndices[1], selectedLevelIndices[1]);
            selectedLevelIndices.Sort();
        }
        private void button14_Click(object sender, EventArgs e)
        {
            int lastLevel = selectedLevelIndices[selectedLevelIndices.Count - 1];
            selectedLevelIndices.Clear();
            for (int i = 0; i <= lastLevel; i++)
                selectedLevelIndices.Add(i);
            UpdateSelection();
        }
        private void button15_Click(object sender, EventArgs e)
        {
            int firstLevel = selectedLevelIndices[0];
            selectedLevelIndices.Clear();
            for (int i = firstLevel; i < EffectSome.UserLevelCount; i++)
                selectedLevelIndices.Add(i);
            UpdateSelection();
        }
        private void button16_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < EffectSome.UserLevelCount; i++)
                if (!selectedLevelIndices.Contains(i))
                    selectedLevelIndices.Add(i);
                else
                    selectedLevelIndices.Remove(i);
            UpdateSelection();
        }
        #endregion
        
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            isInEditMode = true;
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            isInEditMode = false;
        }
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    DataGridViewCheckBoxCell a = dataGridView1["Selected", e.RowIndex] as DataGridViewCheckBoxCell;
                    try
                    {
                        if (Convert.ToBoolean(a.Value)) selectedLevelIndices.Add(e.RowIndex);
                        else if (!Convert.ToBoolean(a.Value)) selectedLevelIndices.Remove(e.RowIndex);
                    }
                    catch { }
                }
                else if (e.ColumnIndex == 1)
                {
                    if (!int.TryParse(dataGridView1["LevelFolder", e.RowIndex].Value.ToString(), out int folder)) // Avoid non-numeral entries
                        dataGridView1["LevelFolder", e.RowIndex].Value = EffectSome.UserLevels[e.RowIndex].LevelFolder;
                }
                else if (e.ColumnIndex == 2)
                {
                    if (dataGridView1["LevelName", e.RowIndex].Value == null)
                        dataGridView1["LevelName", e.RowIndex].Value = EffectSome.UserLevels[e.RowIndex].LevelName;
                }
                else if (e.ColumnIndex == 3)
                {
                    if (!int.TryParse(dataGridView1["LevelRevision", e.RowIndex].Value.ToString(), out int rev)) // Avoid non-numeral entries
                        dataGridView1["LevelRevision", e.RowIndex].Value = EffectSome.UserLevels[e.RowIndex].LevelRevision;
                    else if (rev < 0) // Avoid negative entires
                        dataGridView1["LevelRevision", e.RowIndex].Value = "0";
                }
                else if (e.ColumnIndex == 6)
                {
                    if (!int.TryParse(dataGridView1["LevelSong", e.RowIndex].Value.ToString(), out int songID)) // Avoid non-numeral entries
                        dataGridView1["LevelSong", e.RowIndex].Value = EffectSome.UserLevels[e.RowIndex].LevelCustomSongID;
                    else if (songID < 0) // Avoid negative entires
                        dataGridView1["LevelSong", e.RowIndex].Value = "0";
                }
                else if (e.ColumnIndex == 11)
                {
                    if (!int.TryParse(dataGridView1["LevelVersion", e.RowIndex].Value.ToString(), out int version)) // Avoid non-numeral entries
                        dataGridView1["LevelVersion", e.RowIndex].Value = EffectSome.UserLevels[e.RowIndex].LevelVersion;
                }
                else if (e.ColumnIndex == 12)
                {
                    if (!int.TryParse(dataGridView1["LevelID", e.RowIndex].Value.ToString(), out int levelID)) // Avoid non-numeral entries
                        dataGridView1["LevelID", e.RowIndex].Value = EffectSome.UserLevels[e.RowIndex].LevelID;
                    else if (levelID < 128 && levelID != 0) // Avoid invalid Level IDs (at least 128, otherwise 0)
                        dataGridView1["LevelID", e.RowIndex].Value = "0";
                }
            }
            CheckForCommissionRequirement();
            selectedLevelIndices.Sort();
            selectedLevelIndices = selectedLevelIndices.RemoveDuplicates();
            CheckForSelectedLevels();
        }
        
        string GenerateTriggerAnalyticsToolTipText(int index)
        {
            string result = "";
            result += EffectSome.UserLevels[index].LevelObjectCount > 0 ? ((double)EffectSome.UserLevels[index].LevelTriggerCount / EffectSome.UserLevels[index].LevelObjectCount).ToString("P5") + " of total level objects" : "No level objects";
            result += "\n\nColor:\t\t" + EffectSome.UserLevels[index].ColorTriggersCount;
            result += "\n\nMove:\t\t" + EffectSome.UserLevels[index].MoveTriggersCount;
            result += "\nStop:\t\t" + EffectSome.UserLevels[index].StopTriggersCount;
            result += "\nPulse:\t\t" + EffectSome.UserLevels[index].PulseTriggersCount;
            result += "\nAlpha:\t\t" + EffectSome.UserLevels[index].AlphaTriggersCount;
            result += "\nToggle:\t\t" + EffectSome.UserLevels[index].ToggleTriggersCount;
            result += "\nSpawn:\t\t" + EffectSome.UserLevels[index].SpawnTriggersCount;
            result += "\nRotate:\t\t" + EffectSome.UserLevels[index].RotateTriggersCount;
            result += "\nFollow:\t\t" + EffectSome.UserLevels[index].FollowTriggersCount;
            result += "\nShake:\t\t" + EffectSome.UserLevels[index].ShakeTriggersCount;
            result += "\nAnimate:\t" + EffectSome.UserLevels[index].AnimateTriggersCount;
            result += "\nFollow Player Y:\t" + EffectSome.UserLevels[index].FollowPlayerYTriggersCount;
            result += "\nTouch:\t\t" + EffectSome.UserLevels[index].TouchTriggersCount;
            result += "\nCount:\t\t" + EffectSome.UserLevels[index].CountTriggersCount;
            result += "\nInstant Count:\t" + EffectSome.UserLevels[index].InstantCountTriggersCount;
            result += "\nPickup:\t\t" + EffectSome.UserLevels[index].PickupTriggersCount;
            result += "\nCollision:\t" + EffectSome.UserLevels[index].CollisionTriggersCount;
            result += "\nOn Death:\t" + EffectSome.UserLevels[index].OnDeathTriggersCount;
            result += "\n\nDisable Trail:\t" + EffectSome.UserLevels[index].DisableTrailTriggersCount;
            result += "\nEnable Trail:\t" + EffectSome.UserLevels[index].EnableTrailTriggersCount;
            result += "\n\nShow Player:\t" + EffectSome.UserLevels[index].ShowPlayerTriggersCount;
            result += "\nHide Player:\t" + EffectSome.UserLevels[index].HidePlayerTriggersCount;
            result += "\n\nBG Effect On:\t" + EffectSome.UserLevels[index].BGEffectOnTriggersCount;
            result += "\nBG Effect Off:\t" + EffectSome.UserLevels[index].BGEffectOffTriggersCount;
            return result;
        }
        string GenerateUsedGroupIDsAnalyticsToolTipText(int index) => EffectSome.UserLevels[index].LevelUsedGroupIDs.Length > 0 ? EffectSome.UserLevels[index].LevelUsedGroupIDs.ShowValuesWithRanges() : "No groups used.";
        
        void ShowLevel(int index)
        {
            dataGridView1.Rows.Insert(0);
            dataGridView1["LevelFolder", 0].Value = EffectSome.UserLevels[index].LevelFolder;
            dataGridView1["LevelFolder", 0].ToolTipText = EffectSome.FolderNames[EffectSome.UserLevels[index].LevelFolder];
            dataGridView1["LevelName", 0].Value = EffectSome.UserLevels[index].LevelName;
            dataGridView1["LevelRevision", 0].Value = EffectSome.UserLevels[index].LevelRevision.ToString();
            dataGridView1["LevelDescription", 0].Value = EffectSome.UserLevels[index].LevelDescription;
            dataGridView1["LevelBuildTIme", 0].Value = $"{EffectSome.UserLevels[index].TotalBuildTime:hh\\:mm\\:ss}";
            dataGridView1["LevelLength", 0].Value = Gamesave.levelLengthNames[EffectSome.UserLevels[index].LevelLength];
            dataGridView1["LevelSong", 0].Value = EffectSome.UserLevels[index].LevelCustomSongID.ToString();
            if (EffectSome.UserLevels[index].LevelCustomSongID == 0)
                dataGridView1["LevelSong", 0].ToolTipText = "Official Song: " + (EffectSome.UserLevels[index].LevelOfficialSongID + 1).ToString("D2") + " - " + EffectSome.OfficialSongNames[EffectSome.UserLevels[index].LevelOfficialSongID];
            dataGridView1["LevelObjects", 0].Value = EffectSome.UserLevels[index].LevelObjectCount.ToString();
            if (Options.BoolDictionary["analyzeObjectCount"])
                dataGridView1["LevelObjects", 0].ToolTipText = "Different Object Count: " + EffectSome.UserLevels[index].LevelDifferentObjectIDs.Length + " (" + ((double)EffectSome.UserLevels[index].LevelDifferentObjectIDs.Length / Gamesave.totalDifferentObjectCount).ToString("P5") + ")";
            if (Options.BoolDictionary["analyzeUsedGroupIDs"])
            {
                dataGridView1["LevelGroups", 0].Value = EffectSome.UserLevels[index]?.LevelUsedGroupIDs.Length.ToString();
                dataGridView1["LevelGroups", 0].ToolTipText = GenerateUsedGroupIDsAnalyticsToolTipText(index);
            }
            if (Options.BoolDictionary["analyzeObjectCount"])
            {
                dataGridView1["LevelTriggers", 0].Value = EffectSome.UserLevels[index]?.LevelTriggerCount.ToString();
                dataGridView1["LevelTriggers", 0].ToolTipText = GenerateTriggerAnalyticsToolTipText(index);
            }
            dataGridView1["LevelAttempts", 0].Value = EffectSome.UserLevels[index].LevelAttempts.ToString();
            dataGridView1["LevelVersion", 0].Value = EffectSome.UserLevels[index].LevelVersion.ToString();
            dataGridView1["LevelID", 0].Value = EffectSome.UserLevels[index].LevelID.ToString();
            if (EffectSome.UserLevels[index].LevelVerifiedStatus)
            {
                if (EffectSome.UserLevels[index].LevelUploadedStatus)
                    dataGridView1["LevelStatus", 0].Value = "Uploaded";
                else
                    dataGridView1["LevelStatus", 0].Value = "Verified";
            }
            else
                dataGridView1["LevelStatus", 0].Value = "Unverified";
            UpdateTitle();
        }
        void ShowLevel(int levelIndex, int rowIndex)
        {
            dataGridView1["LevelFolder", rowIndex].Value = EffectSome.UserLevels[levelIndex].LevelFolder;
            dataGridView1["LevelFolder", rowIndex].ToolTipText = EffectSome.FolderNames[EffectSome.UserLevels[levelIndex].LevelFolder];
            dataGridView1["LevelName", rowIndex].Value = EffectSome.UserLevels[levelIndex].LevelName;
            dataGridView1["LevelRevision", rowIndex].Value = EffectSome.UserLevels[levelIndex].LevelRevision.ToString();
            dataGridView1["LevelDescription", rowIndex].Value = EffectSome.UserLevels[levelIndex].LevelDescription;
            dataGridView1["LevelLength", rowIndex].Value = Gamesave.levelLengthNames[EffectSome.UserLevels[levelIndex].LevelLength];
            dataGridView1["LevelSong", rowIndex].Value = EffectSome.UserLevels[levelIndex].LevelCustomSongID.ToString();
            if (EffectSome.UserLevels[levelIndex].LevelCustomSongID == 0)
                dataGridView1["LevelSong", rowIndex].ToolTipText = "Official Song: " + EffectSome.UserLevels[levelIndex].LevelOfficialSongID.ToString("D2") + " - " + EffectSome.OfficialSongNames[EffectSome.UserLevels[levelIndex].LevelOfficialSongID];
            dataGridView1["LevelObjects", rowIndex].Value = EffectSome.UserLevels[levelIndex].LevelObjectCount.ToString();
            if (Options.BoolDictionary["analyzeObjectCount"])
                dataGridView1["LevelObjects", rowIndex].ToolTipText = "Different Object Count: " + EffectSome.UserLevels[levelIndex].LevelDifferentObjectIDs.Length + " (" + ((double)EffectSome.UserLevels[levelIndex].LevelDifferentObjectIDs.Length / Gamesave.totalDifferentObjectCount).ToString("P5") + ")";
            if (Options.BoolDictionary["analyzeUsedGroupIDs"])
            {
                dataGridView1["LevelGroups", rowIndex].Value = EffectSome.UserLevels[levelIndex].LevelUsedGroupIDs.Length.ToString();
                dataGridView1["LevelGroups", rowIndex].ToolTipText = GenerateUsedGroupIDsAnalyticsToolTipText(rowIndex);
            }
            if (Options.BoolDictionary["analyzeObjectCount"])
            {
                dataGridView1["LevelTriggers", rowIndex].Value = EffectSome.UserLevels[levelIndex].LevelTriggerCount.ToString();
                dataGridView1["LevelTriggers", rowIndex].ToolTipText = GenerateTriggerAnalyticsToolTipText(rowIndex);
            }
            dataGridView1["LevelAttempts", rowIndex].Value = EffectSome.UserLevels[levelIndex].LevelAttempts.ToString();
            dataGridView1["LevelVersion", rowIndex].Value = EffectSome.UserLevels[levelIndex].LevelVersion.ToString();
            dataGridView1["LevelID", rowIndex].Value = EffectSome.UserLevels[levelIndex].LevelID.ToString();
            if (EffectSome.UserLevels[levelIndex].LevelVerifiedStatus)
            {
                if (EffectSome.UserLevels[levelIndex].LevelUploadedStatus)
                    dataGridView1["LevelStatus", rowIndex].Value = "Uploaded";
                else
                    dataGridView1["LevelStatus", rowIndex].Value = "Verified";
            }
            else
                dataGridView1["LevelStatus", rowIndex].Value = "Unverified";
            UpdateTitle();
        }
        void LoadLevels()
        {
            // Please do not fuck my computer up thanks
            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
            for (int i = EffectSome.UserLevelCount - 1; i >= 0; i--)
                ShowLevel(i);
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
        }
        void CheckForCommissionRequirement()
        {
            bool requiresCommission = false;
            for (int i = 0; i < dataGridView1.Rows.Count && !requiresCommission; i++)
                try { requiresCommission = Convert.ToInt32(dataGridView1[1, i].Value.ToString()) != EffectSome.UserLevels[i].LevelFolder || dataGridView1[2, i].Value.ToString() != EffectSome.UserLevels[i].LevelName || Convert.ToInt32(dataGridView1[3, i].Value.ToString()) != EffectSome.UserLevels[i].LevelRevision || dataGridView1[4, i].Value.ToString() != EffectSome.UserLevels[i].LevelDescription || Convert.ToInt32(dataGridView1[6, i].Value.ToString()) != EffectSome.UserLevels[i].LevelCustomSongID || Convert.ToInt32(dataGridView1[10, i].Value.ToString()) != EffectSome.UserLevels[i].LevelVersion || Convert.ToInt32(dataGridView1[11, i].Value.ToString()) != EffectSome.UserLevels[i].LevelID; }
                catch { }
            requiresChangeCommission = button6.Enabled = requiresCommission;
        }
        void CheckForSelectedLevels()
        {
            button2.Enabled = button3.Enabled = button4.Enabled = selectedLevelIndices.Count > 0;
            button7.Enabled = button10.Enabled = button14.Enabled = !selectedLevelIndices.MatchIndices() && selectedLevelIndices.Count > 0; // Checks whether the selected indices are all from the start
            button8.Enabled = button9.Enabled = button15.Enabled = !selectedLevelIndices.MatchIndicesFromEnd(EffectSome.UserLevelCount) && selectedLevelIndices.Count > 0; // Checks whether the selected indices are all from the start
            button11.Enabled = selectedLevelIndices.Count < EffectSome.UserLevelCount; // Checks whether all the levels are selected so as to select them all
            button12.Enabled = selectedLevelIndices.Count > 0; // Checks whether there are any selected levels to deselect
            button13.Enabled = selectedLevelIndices.Count == 2; // Checks whether there are exactly two selected levels to swap
        }
        void UpdateSelection()
        {
            for (int i = 0; i < EffectSome.UserLevelCount; i++)
                dataGridView1["Selected", i].Value = selectedLevelIndices.Contains(i);
        }
        void UpdateTitle()
        {
            Text = $"Level Overview - {EffectSome.UserLevelCount} Levels";
        }

        private void KeyDownCheck(object sender, KeyEventArgs e)
        {
            if (sender != dataGridView1 || !isInEditMode)
            {
                if (e.Shift && e.KeyCode == Keys.A)
                    button14_Click(sender, e);
                else if (e.Shift && e.KeyCode == Keys.B)
                    button15_Click(sender, e);
                else if (e.Shift && e.KeyCode == Keys.C)
                    button4_Click(sender, e);
                else if (e.Shift && e.KeyCode == Keys.D)
                    button12_Click(sender, e);
                else if (e.Shift && e.KeyCode == Keys.S)
                    button11_Click(sender, e);
                else if (e.Shift && e.KeyCode == Keys.Home)
                    button10_Click(sender, e);
                else if (e.Shift && e.KeyCode == Keys.End)
                    button9_Click(sender, e);
                else if (e.Shift && e.KeyCode == Keys.Up)
                    button7_Click(sender, e);
                else if (e.Shift && e.KeyCode == Keys.Down)
                    button8_Click(sender, e);
                else if (e.Shift && e.KeyCode == Keys.Delete)
                    button3_Click(sender, e);
                else if (e.Control && e.KeyCode == Keys.S)
                    button13_Click(sender, e);
            }
        }
    }
}