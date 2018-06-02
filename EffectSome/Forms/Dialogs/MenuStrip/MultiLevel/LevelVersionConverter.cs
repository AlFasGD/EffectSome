using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EffectSome.EffectSome;
using static EffectSome.Gamesave;
using static EffectSome.LevelObject;

namespace EffectSome
{
    public partial class LevelVersionConverter : Form
    {
        public static bool IsOpen = false;

        public static List<int> selectedLevelIndices = new List<int>();

        public LevelVersionConverter()
        {
            IsOpen = true;
            InitializeComponent();
            LoadLevels();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = selectedLevelIndices.Count - 1; i >= 0; i--)
            {
                string objectString = GetObjectString(UserLevels[selectedLevelIndices[i]].LevelString);
                List<LevelObject> levelObjects = GetObjects(objectString);
                for (int j = 0; j < levelObjects.Count; j++)
                {
                    if ((int)levelObjects[j][ObjectParameter.ID] > versionObjectIDLimits[(int)(numericUpDown1.Value * 10) - 10])
                        levelObjects.RemoveAt(j);
                    else
                        for (int k = versionParameterIDLimits[(int)(numericUpDown1.Value * 10) - 10] + 1; k <= ParameterCount; k++)
                            levelObjects[j].Parameters[k] = null;
                }
                string newLevelString = UserLevels[selectedLevelIndices[i]].LevelString.Replace(objectString, GetObjectString(levelObjects));
                if (radioButton1.Checked)
                {
                    CreateLevel(UserLevels[selectedLevelIndices[i]].LevelName + " " + numericUpDown1.Value, UserLevels[selectedLevelIndices[i]].LevelDescription + " (GD Version: " + numericUpDown1.Value + ")", newLevelString);
                    for (int j = 0; j < selectedLevelIndices.Count; j++)
                        selectedLevelIndices[j]++;
                }
                else
                    SetLevel(GetLevelKeyEntry(newLevelString, UserLevels[selectedLevelIndices[i]].LevelName + " " + numericUpDown1.Value, UserLevels[selectedLevelIndices[i]].LevelDescription + " (GD Version: " + numericUpDown1.Value + ")"), selectedLevelIndices[i]);
            }
            if (radioButton1.Checked)
                LoadLevels();
            else
                ReloadLevels();
            UpdateSelection();
            CheckForSelectedLevels();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < UserLevelCount; i++)
                dataGridView1[0, i].Value = true;
            selectedLevelIndices.Clear();
            for (int i = 0; i < UserLevelCount; i++)
                selectedLevelIndices.Add(i);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < UserLevelCount; i++)
                dataGridView1[0, i].Value = false;
            selectedLevelIndices.Clear();
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
            for (int i = firstLevel; i < UserLevelCount; i++)
                selectedLevelIndices.Add(i);
            UpdateSelection();
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell a = dataGridView1[0, e.RowIndex] as DataGridViewCheckBoxCell;
                try
                {
                    if (Convert.ToBoolean(a.Value)) selectedLevelIndices.Add(e.RowIndex);
                    else if (!Convert.ToBoolean(a.Value)) selectedLevelIndices.Remove(e.RowIndex);
                }
                catch { }
            }
            selectedLevelIndices.Sort();
            selectedLevelIndices = selectedLevelIndices.RemoveDuplicates();
            CheckForSelectedLevels();
        }

        void UpdateSelection()
        {
            for (int i = 0; i < EffectSome.UserLevelCount; i++)
                dataGridView1[0, i].Value = selectedLevelIndices.Contains(i);
        }
        void LoadLevels()
        {
            dataGridView1.Rows.Clear();
            for (int i = UserLevelCount - 1; i >= 0; i--)
            {
                dataGridView1.Rows.Insert(0);
                dataGridView1[1, 0].Value = UserLevels[i].LevelName;
            }
        }
        void ReloadLevels()
        {
            for (int i = 0; i < UserLevelCount; i++)
                dataGridView1[1, i].Value = UserLevels[i].LevelName;
        }

        void CheckForSelectedLevels()
        {
            button14.Enabled = !selectedLevelIndices.MatchIndices() && selectedLevelIndices.Count > 0; // Checks whether the selected indices are all from the start
            button15.Enabled = !selectedLevelIndices.MatchIndicesFromEnd(UserLevelCount) && selectedLevelIndices.Count > 0; // Checks whether the selected indices are all from the start
            button11.Enabled = selectedLevelIndices.Count < UserLevelCount; // Checks whether all the levels are selected so as to select them all
            button1.Enabled = button12.Enabled = selectedLevelIndices.Count > 0; // Checks whether there are any selected levels to deselect
        }

        private void LevelVersionConverter_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
        }
    }
}