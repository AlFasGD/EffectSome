using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.IO.File;
using static System.IO.Directory;
using static System.Convert;
using static EffectSome.GlobalParameterSettingsPreset;

namespace EffectSome
{
    public partial class GlobalParameterSettings : Form
    {
        public static bool IsOpen = false;

        public static List<GlobalParameterSettingsPreset> presets = new List<GlobalParameterSettingsPreset>();
        public static GlobalParameterSettingsPreset tempPreset;
        public static int selectedPresetAutoCopyPaste, selectedPresetAdjustIDs, selectedPresetAutoAddGroupIDs;

        public enum Tab
        {
            AutoCopyPaste,
            AdjustIDs,
            AutoAddGroupIDs
        }

        public GlobalParameterSettings()
        {
            IsOpen = true;
            InitializeComponent();
            LoadPresets();
            if (presets.Count == 0)
            {
                textBox1.Text = "New Preset 1";
                CreateNewPreset(comboBox1, textBox1);
            }
        }

        #region RadioButtons
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            tempPreset.AdjustIDAdjustmentMode = (AdjustmentMode)(ToInt32(radioButton2.Checked) + ToInt32(radioButton9.Checked) * 2);
            CheckPresetUpdateAbility(comboBox2, button12, Tab.AdjustIDs);
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = radioButton2.Checked;
            tempPreset.AdjustIDAdjustmentMode = (AdjustmentMode)(ToInt32(radioButton2.Checked) + ToInt32(radioButton9.Checked) * 2);
            CheckPresetUpdateAbility(comboBox2, button12, Tab.AdjustIDs);
        }
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown4.Enabled = radioButton8.Checked;
            tempPreset.CopyPasteMode = (AutoCopyPasteMode)(ToInt32(radioButton14.Checked) + ToInt32(radioButton15.Checked) * 2);
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            button7.Enabled = (listBox1.Enabled = numericUpDown28.Enabled = button4.Enabled = radioButton9.Checked) && listBox1.SelectedItems.Count != 0;
            tempPreset.AdjustIDAdjustmentMode = (AdjustmentMode)(ToInt32(radioButton2.Checked) + ToInt32(radioButton9.Checked) * 2);
            CheckPresetUpdateAbility(comboBox2, button12, Tab.AdjustIDs);
        }
        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            button8.Enabled = (listBox2.Enabled = numericUpDown29.Enabled = button9.Enabled = radioButton10.Checked) && listBox2.SelectedItems.Count != 0;
            tempPreset.AutoAddGroupIDAdjustmentMode = (AdjustmentMode)(ToInt32(radioButton11.Checked) + ToInt32(radioButton10.Checked) * 2);
            CheckPresetUpdateAbility(comboBox4, button18, Tab.AutoAddGroupIDs);
        }
        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown30.Enabled = radioButton11.Checked;
            tempPreset.AutoAddGroupIDAdjustmentMode = (AdjustmentMode)(ToInt32(radioButton11.Checked) + ToInt32(radioButton10.Checked) * 2);
            CheckPresetUpdateAbility(comboBox4, button18, Tab.AutoAddGroupIDs);
        }
        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            tempPreset.AutoAddGroupIDAdjustmentMode = (AdjustmentMode)(ToInt32(radioButton11.Checked) + ToInt32(radioButton10.Checked) * 2);
            CheckPresetUpdateAbility(comboBox4, button18, Tab.AutoAddGroupIDs);
        }
        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Enabled = checkBox4.Enabled = radioButton14.Checked;
            numericUpDown11.Enabled = checkBox4.Checked && radioButton14.Checked;
            numericUpDown12.Enabled = checkBox3.Checked && radioButton14.Checked;
            tempPreset.CopyPasteMode = (AutoCopyPasteMode)(ToInt32(radioButton14.Checked) + ToInt32(radioButton15.Checked) * 2);
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
            button11.Enabled = true;
        }
        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Enabled = checkBox2.Enabled = radioButton15.Checked;
            numericUpDown9.Enabled = checkBox2.Checked && radioButton15.Checked;
            numericUpDown10.Enabled = checkBox1.Checked && radioButton15.Checked;
            tempPreset.CopyPasteMode = (AutoCopyPasteMode)(ToInt32(radioButton14.Checked) + ToInt32(radioButton15.Checked) * 2);
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
            button11.Enabled = true;
        }
        #endregion
        #region Buttons
        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(numericUpDown28.Value);
            tempPreset.AdjustIDsSpecifiedValues = listBox1.Items.ToInt32List();
            CheckPresetUpdateAbility(comboBox2, button12, Tab.AdjustIDs);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            ChangePresetName(comboBox1, textBox1);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            RemoveItems(listBox1);
            tempPreset.AdjustIDsSpecifiedValues = listBox1.Items.ToInt32List();
            CheckPresetUpdateAbility(comboBox2, button12, Tab.AdjustIDs);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            RemoveItems(listBox2);
            tempPreset.AutoAddGroupIDsSpecifiedValues = listBox2.Items.ToInt32List();
            CheckPresetUpdateAbility(comboBox4, button18, Tab.AutoAddGroupIDs);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add(numericUpDown29.Value);
            tempPreset.AutoAddGroupIDsSpecifiedValues = listBox2.Items.ToInt32List();
            CheckPresetUpdateAbility(comboBox4, button18, Tab.AutoAddGroupIDs);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            DeletePreset(comboBox1, textBox1, button10);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            SavePreset(comboBox1.Text, Tab.AutoCopyPaste);
            UpdatePreset(comboBox1.Text);
            button11.Enabled = false;
        }
        private void button12_Click(object sender, EventArgs e)
        {
            SavePreset(comboBox2.Text, Tab.AdjustIDs);
            button12.Enabled = false;
        }
        private void button13_Click(object sender, EventArgs e)
        {
            DeletePreset(comboBox2, textBox2, button13);
        }
        private void button14_Click(object sender, EventArgs e)
        {
            ChangePresetName(comboBox2, textBox2);
        }
        private void button18_Click(object sender, EventArgs e)
        {
            SavePreset(comboBox4.Text, Tab.AutoAddGroupIDs);
            button18.Enabled = false;
        }
        private void button19_Click(object sender, EventArgs e)
        {
            DeletePreset(comboBox4, textBox4, button19);
        }
        private void button20_Click(object sender, EventArgs e)
        {
            ChangePresetName(comboBox4, textBox4);
        }
        private void button24_Click(object sender, EventArgs e)
        {
            CreateNewPreset(comboBox1, textBox1);
        }
        private void button25_Click(object sender, EventArgs e)
        {
            CreateNewPreset(comboBox2, textBox2);
        }
        private void button27_Click(object sender, EventArgs e)
        {
            CreateNewPreset(comboBox4, textBox4);
        }
        #endregion
        #region CheckBoxes
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown10.Enabled = checkBox1.Checked;
            tempPreset.AutoCopyPasteSpecifiedDistanceYEnabled = checkBox1.Checked;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown9.Enabled = checkBox2.Checked;
            tempPreset.AutoCopyPasteSpecifiedDistanceXEnabled = checkBox2.Checked;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown12.Enabled = checkBox3.Checked;
            tempPreset.AutoCopyPasteSpecifiedLocationYEnabled = checkBox3.Checked;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown11.Enabled = checkBox4.Checked;
            tempPreset.AutoCopyPasteSpecifiedLocationXEnabled = checkBox4.Checked;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            tempPreset.AutoAddGroupIDsChooseGroupIDsToAdjust = checkBox9.Checked;
            CheckPresetUpdateAbility(comboBox4, button18, Tab.AutoAddGroupIDs);
        }
        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            checkBox20.Enabled = checkBox21.Enabled = checkBox17.Checked;
            numericUpDown7.Enabled = checkBox20.Enabled && checkBox20.Checked;
            numericUpDown6.Enabled = checkBox21.Enabled && checkBox21.Checked;
            tempPreset.MoveCopyPastedObjects = checkBox17.Checked;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown7.Enabled = checkBox20.Enabled && checkBox20.Checked;
            tempPreset.AutoCopyPasteMoveXEnabled = checkBox20.Checked;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown6.Enabled = checkBox21.Enabled && checkBox21.Checked;
            tempPreset.AutoCopyPasteMoveYEnabled = checkBox21.Checked;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        #endregion
        #region NumericUpDowns
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            tempPreset.AdjustIDsAdjustment = (int)numericUpDown2.Value;
            CheckPresetUpdateAbility(comboBox2, button12, Tab.AdjustIDs);
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            tempPreset.AutoCopyPasteTimes = (int)numericUpDown4.Value;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            tempPreset.AutoCopyPasteMoveY = (float)numericUpDown6.Value;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            tempPreset.AutoCopyPasteMoveX = (float)numericUpDown7.Value;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            tempPreset.AutoCopyPasteSpecifiedDistanceX = (float)numericUpDown9.Value;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            tempPreset.AutoCopyPasteSpecifiedDistanceY = (float)numericUpDown10.Value;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            tempPreset.AutoCopyPasteSpecifiedLocationX = (float)numericUpDown11.Value;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            tempPreset.AutoCopyPasteSpecifiedLocationY = (float)numericUpDown12.Value;
            CheckPresetUpdateAbility(comboBox1, button11, Tab.AutoCopyPaste);
        }
        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            button11.Enabled = true;
        }
        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            button11.Enabled = true;
        }
        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            button11.Enabled = true;
        }
        private void numericUpDown30_ValueChanged(object sender, EventArgs e)
        {
            tempPreset.AutoAddGroupIDsAdjustment = (int)numericUpDown30.Value;
            CheckPresetUpdateAbility(comboBox4, button18, Tab.AutoAddGroupIDs);
        }
        #endregion
        #region ListBoxes
        private void targetGroupIDList_SelectedIndexChanged(object sender, EventArgs e)
        {
            button7.Enabled = listBox1.SelectedItems.Count != 0;
        }
        private void groupIDList_SelectedIndexChanged(object sender, EventArgs e)
        {
            button8.Enabled = listBox2.SelectedItems.Count != 0;
        }
        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox6.SelectedIndices.Count; i++)
                tempPreset.AutoAddGroupIDsAdjustedGroupIDs[listBox6.SelectedIndices[i]] = true;
            CheckPresetUpdateAbility(comboBox4, button18, Tab.AutoAddGroupIDs);
        }
        #endregion
        #region TextBoxes
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button6.Enabled = textBox1.Text.Length != 0 && comboBox1.Text != "" && comboBox1.Text != textBox1.Text;
            button24.Enabled = textBox1.Text.Length != 0 && comboBox1.Text != textBox1.Text;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button14.Enabled = textBox2.Text.Length != 0 && comboBox2.Text != "" && comboBox2.Text != textBox2.Text;
            button25.Enabled = textBox2.Text.Length != 0 && comboBox2.Text != textBox2.Text;
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            button20.Enabled = textBox4.Text.Length != 0 && comboBox4.Text != "" && comboBox4.Text != textBox4.Text;
            button27.Enabled = textBox4.Text.Length != 0 && comboBox4.Text != textBox4.Text;
        }
        #endregion
        #region ComboBoxes
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangePreset(comboBox1, textBox1, button11, Tab.AutoCopyPaste);
            selectedPresetAutoCopyPaste = comboBox1.SelectedIndex;
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangePreset(comboBox2, textBox2, button12, Tab.AdjustIDs);
            selectedPresetAdjustIDs = comboBox2.SelectedIndex;
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangePreset(comboBox4, textBox4, button18, Tab.AutoAddGroupIDs);
            selectedPresetAutoAddGroupIDs = comboBox4.SelectedIndex;
        }
        #endregion

        private void GlobalParameterSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
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
                            if (ID < (decimal)listBox.Items[i])
                            {
                                listBox.Items.Insert(i, ID);
                                break;
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
        void SavePreset(string presetName, Tab tab)
        {
            if (presetName.Contains('?', '/', '|', '<', '>', '\\', '"', ':'))
                MessageBox.Show("The name you entered contains invalid characters. If the name contains any of the following invalid characters, please remove them.\n\n\", ?, |, <, >, :, \\, /, *", "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string fileData = tempPreset.ToString(tab);
                UpdatePreset(presetName, tab);
                CreateDirectory("EffectSome/Presets/Global Parameter Settings/" + presetName + "/");
                WriteAllText("EffectSome/Presets/Global Parameter Settings/" + presetName + "/" + tab.ToString() + ".esf", fileData);
            }
        }
        void LoadPreset(ComboBox CB, Tab tab)
        {
            int selectedIndex = CB.SelectedIndex;
            switch (tab)
            {
                case Tab.AutoCopyPaste:
                    {
                        checkBox1.Checked = presets[selectedIndex].AutoCopyPasteSpecifiedDistanceYEnabled;
                        checkBox2.Checked = presets[selectedIndex].AutoCopyPasteSpecifiedDistanceXEnabled;
                        checkBox3.Checked = presets[selectedIndex].AutoCopyPasteSpecifiedLocationYEnabled;
                        checkBox4.Checked = presets[selectedIndex].AutoCopyPasteSpecifiedLocationXEnabled;
                        checkBox17.Checked = presets[selectedIndex].MoveCopyPastedObjects;
                        checkBox20.Checked = presets[selectedIndex].AutoCopyPasteMoveXEnabled;
                        checkBox21.Checked = presets[selectedIndex].AutoCopyPasteMoveYEnabled;
                        numericUpDown4.Value = presets[selectedIndex].AutoCopyPasteTimes;
                        numericUpDown6.Value = (decimal)presets[selectedIndex].AutoCopyPasteMoveY;
                        numericUpDown7.Value = (decimal)presets[selectedIndex].AutoCopyPasteMoveX;
                        numericUpDown9.Value = (decimal)presets[selectedIndex].AutoCopyPasteSpecifiedDistanceX;
                        numericUpDown10.Value = (decimal)presets[selectedIndex].AutoCopyPasteSpecifiedDistanceY;
                        numericUpDown11.Value = (decimal)presets[selectedIndex].AutoCopyPasteSpecifiedLocationX;
                        numericUpDown12.Value = (decimal)presets[selectedIndex].AutoCopyPasteSpecifiedLocationY;
                        radioButton8.Checked = presets[selectedIndex].CopyPasteMode == AutoCopyPasteMode.NumberOfTimes;
                        radioButton14.Checked = presets[selectedIndex].CopyPasteMode == AutoCopyPasteMode.SpecifiedLocation;
                        radioButton15.Checked = presets[selectedIndex].CopyPasteMode == AutoCopyPasteMode.SpecifiedDistance;
                        break;
                    }
                case Tab.AdjustIDs:
                    {
                        numericUpDown2.Value = presets[selectedIndex].AdjustIDsAdjustment;
                        radioButton1.Checked = presets[selectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs;
                        radioButton2.Checked = presets[selectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment;
                        radioButton9.Checked = presets[selectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues;
                        listBox1.Items.Clear();
                        for (int i = 0; i < presets[selectedIndex].AdjustIDsSpecifiedValues.Count; i++)
                            listBox1.Items.Add(presets[selectedIndex].AdjustIDsSpecifiedValues[i]);
                        break;
                    }
                case Tab.AutoAddGroupIDs:
                    {
                        numericUpDown30.Value = presets[selectedIndex].AutoAddGroupIDsAdjustment;
                        radioButton12.Checked = presets[selectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs;
                        radioButton11.Checked = presets[selectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment;
                        radioButton10.Checked = presets[selectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues;
                        listBox2.Items.Clear();
                        for (int i = 0; i < presets[selectedIndex].AutoAddGroupIDsSpecifiedValues.Count; i++)
                            listBox2.Items.Add(presets[selectedIndex].AutoAddGroupIDsSpecifiedValues[i]);
                        checkBox9.Checked = presets[selectedIndex].AutoAddGroupIDsChooseGroupIDsToAdjust;
                        listBox6.SelectedIndices.Clear();
                        int[] indices = presets[selectedIndex].AutoAddGroupIDsAdjustedGroupIDs.GetIndicesOfMatchingValues(true);
                        for (int i = 0; i < indices.Length; i++)
                            listBox6.SelectedIndices.Add(indices[i]);
                        break;
                    }
                default:
                    break;
            }
        }
        void DeletePreset(ComboBox CB, TextBox TB, Button B)
        {
            ComboBox[] CBs = { comboBox1, comboBox2, comboBox4 };
            TextBox[] TBs = { textBox1, textBox2, textBox4 };
            Button[] Bs = { button10, button13, button19, button6, button14, button20, button24, button25, button27 };
            List<ComboBox> others = CBs.ToList();
            others.Remove(CB);
            CBs = others.ToArray();
            EffectSome.DeleteEntireDirectory("EffectSome/Presets/Global Parameter Settings/" + CB.Text + "/");
            for (int i = 0; i < CBs.Length; i++)
            {
                CBs[i].Items.Remove(CB.Text);
                if (CBs[i].Items.Count >= 1)
                    if (CBs[i].Text == CB.Text)
                    {
                        CBs[i].SelectedItem = CBs[i].Items[0];
                        TBs[i].Text = CBs[i].Text;
                    }
            }
            CB.Items.Remove(CB.Text);
            presets = presets.DeletePreset(CB.Text);
            if (CB.Items.Count >= 1)
                CB.SelectedItem = CB.Items[0];
            if (CB.Items.Count == 0)
                for (int i = 0; i < CBs.Length * 2; i++)
                    Bs[i].Enabled = false;
            for (int i = 0; i < CBs.Length; i++)
                Bs[i + 6].Enabled = true;
        }
        void ChangePresetName(ComboBox CB, TextBox TB)
        {
            if (TB.Text.Length != 0)
            {
                foreach (string s in CB.Items)
                {
                    if (TB.Text.ToLower() != s.ToLower())
                    {
                        if (Directory.Exists("EffectSome/Presets/Global Parameter Settings/" + TB.Text + "/"))
                            EffectSome.DeleteEntireDirectory("EffectSome/Presets/Global Parameter Settings/" + TB.Text + "/");
                        Directory.Move("EffectSome/Presets/Global Parameter Settings/" + CB.Text, "EffectSome/Presets/Global Parameter Settings/" + TB.Text);
                        ComboBox[] CBs = { comboBox1, comboBox2, comboBox4 };
                        TextBox[] TBs = { textBox1, textBox2, textBox4 };
                        List<ComboBox> others = CBs.ToList();
                        others.Remove(CB);
                        others.Add(CB);
                        CBs = others.ToArray();
                        for (int i = 0; i < CBs.Length; i++)
                        {
                            CBs[i].Items.Remove(CB.Text);
                            CBs[i].Items.Add(TB.Text);
                            if (CB.Items.Count < 2)
                            {
                                CBs[i].Text = TB.Text;
                                TBs[i].Text = TB.Text;
                            }
                        }
                        presets = presets.RenamePreset(CB.Text, TB.Text);
                        for (int i = 0; i < CBs.Length; i++)
                        {
                            CBs[i].Items.Clear();
                            for (int j = 0; j < presets.Count; j++)
                                CBs[i].Items.Add(presets[j].PresetName);
                        }
                        CB.Text = TB.Text;
                        presets = presets.SortPresetList();
                        break;
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("There already is a preset with that name. Do you want to overwrite the already existing one?", "Already Existing Preset", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                            foreach (Tab i in Enum.GetValues(typeof(Tab)))
                                SavePreset(TB.Text, i);
                    }
                }
                TB.Focus();
            }
            else
                MessageBox.Show("Please enter a name for the preset.", "Empty Name", MessageBoxButtons.OK);
        }
        void CreateNewPreset(ComboBox CB, TextBox TB)
        {
            if (TB.Text.Contains('?') || TB.Text.Contains('/') || TB.Text.Contains('|') || TB.Text.Contains('<') || TB.Text.Contains('>') || TB.Text.Contains('\\') || TB.Text.Contains('"') || TB.Text.Contains(':'))
                MessageBox.Show("The name you entered contains an invalid character. If the name contains any of the following invalid characters, please remove them.\n\n\", ?, |, <, >, :, \\, /, *");
            else
            {
                ComboBox[] CBs = { comboBox1, comboBox2, comboBox4 };
                TextBox[] TBs = { textBox1, textBox2, textBox4 };
                Button[] Bs = { button10, button13, button19, button6, button14, button20, button24, button25, button27 };

                if (presets.Count > 0)
                {
                    foreach (Tab i in Enum.GetValues(typeof(Tab)))
                        SavePreset(TB.Text, i);
                    presets.Add(new GlobalParameterSettingsPreset(TB.Text));
                    tempPreset = presets[presets.Count - 1].Clone();
                }
                else
                {
                    presets.Add(new GlobalParameterSettingsPreset(TB.Text));
                    tempPreset = presets[presets.Count - 1].Clone();
                    foreach (Tab i in Enum.GetValues(typeof(Tab)))
                        SavePreset(TB.Text, i);
                }
                for (int i = 0; i < CBs.Length; i++)
                {
                    CBs[i].Items.Add(TB.Text);
                    if (CBs[i].Items.Count == 1)
                    {
                        CBs[i].Text = TB.Text;
                        TBs[i].Text = TB.Text;
                    }
                    Bs[i].Enabled = !(Bs[i + 3].Enabled = Bs[i + 6].Enabled = false);
                }
                presets = presets.SortPresetList();
                for (int i = 0; i < CBs.Length; i++)
                {
                    CBs[i].Items.Clear();
                    for (int j = 0; j < presets.Count; j++)
                        CBs[i].Items.Add(presets[j].PresetName);
                }
                CB.Text = TB.Text;
            }
        }
        void ChangePreset(ComboBox CB, TextBox TB, Button B, Tab tab)
        {
            if (B.Enabled)
            {
                DialogResult result = MessageBox.Show("You haven't saved your changes on the preset. Would you like to save them now?", "Unsaved Settings", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    SavePreset(TB.Text, tab);
                    LoadPreset(CB, tab);
                    TB.Text = CB.Text;
                }
            }
            else
                TB.Text = CB.Text;
        }
        void LoadPresets()
        {
            ComboBox[] CBs = { comboBox1, comboBox2, comboBox4 };
            TextBox[] TBs = { textBox1, textBox2, textBox4 };
            Button[] Bs = { button10, button13, button19 }; // Delete preset buttons
            foreach (string preset in GetDirectories("EffectSome/Presets/Global Parameter Settings"))
            {
                string[] files = { "AdjustIDs.esf", "AutoAddGroupIDs.esf", "AutoCopyPaste.esf" };
                int validFiles = 0;
                for (int i = 0; i < files.Length; i++)
                    files[i] = preset + "\\" + files[i];
                try
                {
                    for (int i = 0; i < CBs.Length; i++)
                        if (GetFiles(preset)[i] == files[i])
                            validFiles++;
                }
                catch (IndexOutOfRangeException) { } // Just because it is needed... MS KYS
                if (validFiles == 3)
                    foreach (ComboBox CB in CBs)
                        CB.Items.Add(preset.Split('\\')[1]);
                else
                {
                    string[] filesToBeDeleted = GetFiles(preset);
                    for (int j = 0; j < filesToBeDeleted.Length; j++)
                        File.Delete(filesToBeDeleted[j]);
                    Directory.Delete(preset);
                }
                if (comboBox1.Items.Count > 0)
                    for (int i = 0; i < CBs.Length; i++)
                    {
                        CBs[i].Text = CBs[i].Items[0].ToString();
                        TBs[i].Text = CBs[i].Text;
                        Bs[i].Enabled = true;
                    }
            }
            LoadPresetObjects();
            for (int i = 0; i < CBs.Length; i++)
                if (CBs[i].Items.Count > 0)
                {
                    tempPreset = presets[0].Clone();
                    LoadPreset(CBs[i], (Tab)i);
                }
        }
        public static void LoadPresetObjects()
        {
            presets.Clear();
            string[] presetNames = GetDirectories("EffectSome\\Presets\\Global Parameter Settings");
            for (int i = 0; i < presetNames.Length; i++)
            {
                presets.Add(new GlobalParameterSettingsPreset(presetNames[i].Split('\\').Last()));
                string[] adjustIDsData = ReadAllText(presetNames[i] + "\\AdjustIDs.esf").Split('|');
                string[] autoAddGroupIDsData = ReadAllText(presetNames[i] + "\\AutoAddGroupIDs.esf").Split('|');
                string[] autoCopyPasteData = ReadAllText(presetNames[i] + "\\AutoCopyPaste.esf").Split('|');
                // Add the values for the presets
                presets[i].CopyPasteMode = (AutoCopyPasteMode)ToInt32(autoCopyPasteData[0]);
                presets[i].AutoCopyPasteTimes = ToInt32(autoCopyPasteData[1]);
                presets[i].AutoCopyPasteSpecifiedLocationX = ToSingle(autoCopyPasteData[2]);
                presets[i].AutoCopyPasteSpecifiedLocationY = ToSingle(autoCopyPasteData[3]);
                presets[i].AutoCopyPasteSpecifiedLocationXEnabled = ToBoolean(autoCopyPasteData[4]);
                presets[i].AutoCopyPasteSpecifiedLocationYEnabled = ToBoolean(autoCopyPasteData[5]);
                presets[i].AutoCopyPasteSpecifiedDistanceX = ToSingle(autoCopyPasteData[6]);
                presets[i].AutoCopyPasteSpecifiedDistanceY = ToSingle(autoCopyPasteData[7]);
                presets[i].AutoCopyPasteSpecifiedDistanceXEnabled = ToBoolean(autoCopyPasteData[8]);
                presets[i].AutoCopyPasteSpecifiedDistanceYEnabled = ToBoolean(autoCopyPasteData[9]);
                presets[i].MoveCopyPastedObjects = ToBoolean(autoCopyPasteData[10]);
                presets[i].AutoCopyPasteMoveX = ToSingle(autoCopyPasteData[11]);
                presets[i].AutoCopyPasteMoveY = ToSingle(autoCopyPasteData[12]);
                presets[i].AutoCopyPasteMoveXEnabled = ToBoolean(autoCopyPasteData[13]);
                presets[i].AutoCopyPasteMoveYEnabled = ToBoolean(autoCopyPasteData[14]);

                presets[i].AdjustIDAdjustmentMode = (AdjustmentMode)ToInt32(adjustIDsData[0]);
                presets[i].AdjustIDsAdjustment = ToInt32(adjustIDsData[1]);
                presets[i].AdjustIDsSpecifiedValues = adjustIDsData[2].Length > 0 ? adjustIDsData[2].Split(':').ToInt32List() : new List<int>();

                presets[i].AutoAddGroupIDAdjustmentMode = (AdjustmentMode)ToInt32(autoAddGroupIDsData[0]);
                presets[i].AutoAddGroupIDsAdjustment = ToInt32(autoAddGroupIDsData[1]);
                presets[i].AutoAddGroupIDsSpecifiedValues = autoAddGroupIDsData[2].Length > 0 ? autoAddGroupIDsData[2].Split(':').ToInt32List() : new List<int>();
                presets[i].AutoAddGroupIDsAdjustedGroupIDs = autoAddGroupIDsData[3].Split(':').ToBooleanArray();
                presets[i].AutoAddGroupIDsChooseGroupIDsToAdjust = ToBoolean(autoAddGroupIDsData[4]);
            }
            presets = presets.SortPresetList();
        }
        public static void UpdatePreset(string presetName)
        {
            int presetIndex = presets.FindPresetIndex(presetName);
            string[] adjustIDsData = ReadAllText("EffectSome\\Presets\\Global Parameter Settings\\" + presets[presetIndex].PresetName + "\\AdjustIDs.esf").Split('|');
            string[] autoAddGroupIDsData = ReadAllText("EffectSome\\Presets\\Global Parameter Settings\\" + presets[presetIndex].PresetName + "\\AutoAddGroupIDs.esf").Split('|');
            string[] autoCopyPasteData = ReadAllText("EffectSome\\Presets\\Global Parameter Settings\\" + presets[presetIndex].PresetName + "\\AutoCopyPaste.esf").Split('|');
            // Add the values for the presets
            presets[presetIndex].CopyPasteMode = (AutoCopyPasteMode)ToInt32(autoCopyPasteData[0]);
            presets[presetIndex].AutoCopyPasteTimes = ToInt32(autoCopyPasteData[1]);
            presets[presetIndex].AutoCopyPasteSpecifiedLocationX = ToSingle(autoCopyPasteData[2]);
            presets[presetIndex].AutoCopyPasteSpecifiedLocationY = ToSingle(autoCopyPasteData[3]);
            presets[presetIndex].AutoCopyPasteSpecifiedLocationXEnabled = ToBoolean(autoCopyPasteData[4]);
            presets[presetIndex].AutoCopyPasteSpecifiedLocationYEnabled = ToBoolean(autoCopyPasteData[5]);
            presets[presetIndex].AutoCopyPasteSpecifiedDistanceX = ToSingle(autoCopyPasteData[6]);
            presets[presetIndex].AutoCopyPasteSpecifiedDistanceY = ToSingle(autoCopyPasteData[7]);
            presets[presetIndex].AutoCopyPasteSpecifiedDistanceXEnabled = ToBoolean(autoCopyPasteData[8]);
            presets[presetIndex].AutoCopyPasteSpecifiedDistanceYEnabled = ToBoolean(autoCopyPasteData[9]);
            presets[presetIndex].MoveCopyPastedObjects = ToBoolean(autoCopyPasteData[10]);
            presets[presetIndex].AutoCopyPasteMoveX = ToSingle(autoCopyPasteData[11]);
            presets[presetIndex].AutoCopyPasteMoveY = ToSingle(autoCopyPasteData[12]);
            presets[presetIndex].AutoCopyPasteMoveXEnabled = ToBoolean(autoCopyPasteData[13]);
            presets[presetIndex].AutoCopyPasteMoveYEnabled = ToBoolean(autoCopyPasteData[14]);

            presets[presetIndex].AdjustIDAdjustmentMode = (AdjustmentMode)ToInt32(adjustIDsData[0]);
            presets[presetIndex].AdjustIDsAdjustment = ToInt32(adjustIDsData[1]);
            presets[presetIndex].AdjustIDsSpecifiedValues = adjustIDsData[2].Length > 0 ? adjustIDsData[2].Split(':').ToInt32List() : new List<int>();

            presets[presetIndex].AutoAddGroupIDAdjustmentMode = (AdjustmentMode)ToInt32(autoAddGroupIDsData[0]);
            presets[presetIndex].AutoAddGroupIDsAdjustment = ToInt32(autoAddGroupIDsData[1]);
            presets[presetIndex].AutoAddGroupIDsSpecifiedValues = autoAddGroupIDsData[2].Length > 0 ? autoAddGroupIDsData[2].Split(':').ToInt32List() : new List<int>();
            presets[presetIndex].AutoAddGroupIDsAdjustedGroupIDs = autoAddGroupIDsData[3].Split(':').ToBooleanArray();
            presets[presetIndex].AutoAddGroupIDsChooseGroupIDsToAdjust = ToBoolean(autoAddGroupIDsData[4]);
        }
        public static void CheckPresetUpdateAbility(ComboBox c, Button b, Tab t)
        {
            b.Enabled = !presets[c.SelectedIndex].Equals(tempPreset, t);
        }
        void UpdatePreset(int presetIndex, Tab tab)
        {
            if (presets.Count > 0)
                presets[presetIndex].From(tempPreset, tab);
        }
        void UpdatePreset(string presetName, Tab tab)
        {
            if (presets.Count > 0)
                presets[presets.FindPresetIndex(presetName)].From(tempPreset, tab);
        }
    }
} 