using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using static System.Convert;

namespace EffectSome
{
    public partial class Options : Form
    {
        public static bool IsOpen = false;

        public static KeysConverter kc = new KeysConverter();
        public static Dictionary<string, bool> BoolDictionary = new Dictionary<string, bool>();
        public static Dictionary<string, int> IntDictionary = new Dictionary<string, int>();
        public static Dictionary<string, string> StringDictionary = new Dictionary<string, string>();
        
        public Options()
        {
            IsOpen = true;
            InitializeComponent();
            try { LoadPreferences(); }
            catch (FileNotFoundException) { SavePreferences(); LoadPreferences(); }
            ShowPreferences();
        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            SavePreferences();
            IsOpen = false;
        }
        #region TextBoxes
        private void textBox1_TextChanged(object sender, EventArgs e) => StringDictionary["defaultSettingsPath"] = textBox1.Text;
        private void textBox2_KeyDown(object sender, KeyEventArgs e) => textBox2.Text = StringDictionary["orangeGuidelineShortcutKey"] = kc.ConvertToString(e.KeyCode);
        private void textBox2_TextChanged(object sender, EventArgs e) => textBox2.Text = StringDictionary["orangeGuidelineShortcutKey"];
        private void textBox3_KeyDown(object sender, KeyEventArgs e) => textBox3.Text = StringDictionary["yellowGuidelineShortcutKey"] = kc.ConvertToString(e.KeyCode);
        private void textBox3_TextChanged(object sender, EventArgs e) => textBox3.Text = StringDictionary["yellowGuidelineShortcutKey"];
        private void textBox4_KeyDown(object sender, KeyEventArgs e) => textBox4.Text = StringDictionary["greenGuidelineShortcutKey"] = kc.ConvertToString(e.KeyCode);
        private void textBox4_TextChanged(object sender, EventArgs e) => textBox4.Text = StringDictionary["greenGuidelineShortcutKey"];
        private void textBox5_KeyDown(object sender, KeyEventArgs e) => textBox5.Text = StringDictionary["pauseGuidelineRecordingShortcutKey"] = kc.ConvertToString(e.KeyCode);
        private void textBox5_TextChanged(object sender, EventArgs e) => textBox5.Text = StringDictionary["pauseGuidelineRecordingShortcutKey"];
        #endregion
        #region CheckBoxes
        private void checkBox1_CheckedChanged(object sender, EventArgs e) => BoolDictionary["autoLoadSettings"] = checkBox1.Checked;
        private void checkBox2_CheckedChanged(object sender, EventArgs e) => BoolDictionary["autoSaveSettings"] = checkBox2.Checked;
        private void checkBox3_CheckedChanged(object sender, EventArgs e) => BoolDictionary["showOperationTimes"] = checkBox3.Checked;
        private void checkBox4_CheckedChanged(object sender, EventArgs e) => BoolDictionary["invertLevelSelectionShortcut"] = checkBox4.Checked;
        private void checkBox5_CheckedChanged(object sender, EventArgs e) => BoolDictionary["applyChangesShortcut"] = checkBox5.Checked;
        private void checkBox6_CheckedChanged(object sender, EventArgs e) => BoolDictionary["objLimWarnings"] = checkBox6.Checked;
        private void checkBox7_CheckedChanged(object sender, EventArgs e) => BoolDictionary["customObjLimWarnings"] = checkBox7.Checked;
        private void checkBox8_CheckedChanged(object sender, EventArgs e) => BoolDictionary["customObjObjsLimWarnings"] = checkBox8.Checked;
        private void checkBox10_CheckedChanged(object sender, EventArgs e) => BoolDictionary["DLLErrorWarnings"] = checkBox10.Checked;
        private void checkBox11_CheckedChanged(object sender, EventArgs e) => BoolDictionary["showDeleteLevelsPrompt"] = checkBox11.Checked;
        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            BoolDictionary["objLimWarnings"] = checkBox6.Checked || checkBox12.Checked;
            BoolDictionary["customObjLimWarnings"] = checkBox7.Checked || checkBox12.Checked;
            BoolDictionary["customObjObjsLimWarnings"] = checkBox8.Checked || checkBox12.Checked;
            BoolDictionary["exitProgramUnsavedChangesWarnings"] = checkBox10.Checked || checkBox12.Checked;
            BoolDictionary["DLLErrorWarnings"] = checkBox17.Checked || checkBox12.Checked;
            checkBox6.Enabled = checkBox7.Enabled = checkBox8.Enabled = checkBox10.Enabled = checkBox17.Enabled = !checkBox12.Checked;
            BoolDictionary["allWarnings"] = checkBox12.Checked;
        }
        private void checkBox14_CheckedChanged(object sender, EventArgs e) => textBox2.Enabled = BoolDictionary["orangeGuidelineShortcut"] = checkBox14.Checked;
        private void checkBox15_CheckedChanged(object sender, EventArgs e) => textBox3.Enabled = BoolDictionary["yellowGuidelineShortcut"] = checkBox15.Checked;
        private void checkBox16_CheckedChanged(object sender, EventArgs e) => textBox4.Enabled = BoolDictionary["greenGuidelineShortcut"] = checkBox16.Checked;
        private void checkBox17_CheckedChanged(object sender, EventArgs e) => BoolDictionary["DLLErrorWarnings"] = checkBox17.Checked;
        private void checkBox18_CheckedChanged(object sender, EventArgs e) => textBox5.Enabled = BoolDictionary["pauseGuidelineRecordingShortcut"] = checkBox18.Checked;
        private void checkBox19_CheckedChanged(object sender, EventArgs e) => BoolDictionary["checkForResourceUpdatesOnStartup"] = checkBox19.Checked;
        private void checkBox20_CheckedChanged(object sender, EventArgs e) => BoolDictionary["checkForUpdatesOnStartup"] = checkBox20.Checked;
        private void checkBox22_CheckedChanged(object sender, EventArgs e) => BoolDictionary["gamesaveDecryptedNotifications"] = checkBox22.Checked;
        private void checkBox23_CheckedChanged(object sender, EventArgs e) => BoolDictionary["levelInfoRetrieved"] = checkBox23.Checked;
        private void checkBox24_CheckedChanged(object sender, EventArgs e) => BoolDictionary["levelDataDecryptedNotifications"] = checkBox24.Checked;
        private void checkBox25_CheckedChanged(object sender, EventArgs e) => BoolDictionary["softwareUpdateStatusNotifications"] = checkBox25.Checked;
        private void checkBox26_CheckedChanged(object sender, EventArgs e) => BoolDictionary["resourceUpdateStatusNotifications"] = checkBox26.Checked;
        private void checkBox28_CheckedChanged(object sender, EventArgs e) => BoolDictionary["emptyLevelsLevelStringsSetNotifications"] = checkBox28.Checked;
        private void checkBox31_CheckedChanged(object sender, EventArgs e) => BoolDictionary["cloneLevelsShortcut"] = checkBox31.Checked;
        private void checkBox32_CheckedChanged(object sender, EventArgs e) => BoolDictionary["deleteLevelsShortcut"] = checkBox32.Checked;
        private void checkBox33_CheckedChanged(object sender, EventArgs e) => BoolDictionary["moveLevelsUpShortcut"] = checkBox33.Checked;
        private void checkBox34_CheckedChanged(object sender, EventArgs e) => BoolDictionary["moveLevelsDownShortcut"] = checkBox34.Checked;
        private void checkBox36_CheckedChanged(object sender, EventArgs e) => BoolDictionary["moveLevelsToBottomShortcut"] = checkBox36.Checked;
        private void checkBox37_CheckedChanged(object sender, EventArgs e) => BoolDictionary["moveLevelsToTopShortcut"] = checkBox37.Checked;
        private void checkBox38_CheckedChanged(object sender, EventArgs e) => BoolDictionary["swapLevelsShortcut"] = checkBox38.Checked;
        private void checkBox39_CheckedChanged(object sender, EventArgs e) => BoolDictionary["selectAllLevelsShortcut"] = checkBox39.Checked;
        private void checkBox40_CheckedChanged(object sender, EventArgs e) => BoolDictionary["deselectAllLevelsShortcut"] = checkBox40.Checked;
        private void checkBox41_CheckedChanged(object sender, EventArgs e) => BoolDictionary["selectAllLevelsBelowShortcut"] = checkBox41.Checked;
        private void checkBox42_CheckedChanged(object sender, EventArgs e) => BoolDictionary["selectAllLevelsAboveShortcut"] = checkBox42.Checked;
        private void checkBox43_CheckedChanged(object sender, EventArgs e) => BoolDictionary["allowEditLevelIDs"] = checkBox43.Checked;
        private void checkBox44_CheckedChanged(object sender, EventArgs e) => BoolDictionary["allowEditLevelVersions"] = checkBox44.Checked;
        private void checkBox45_CheckedChanged(object sender, EventArgs e) => BoolDictionary["allowEditLevelRevisions"] = checkBox45.Checked;
        private void checkBox46_CheckedChanged(object sender, EventArgs e) => BoolDictionary["allowEditSongIDs"] = checkBox46.Checked;
        private void checkBox48_CheckedChanged(object sender, EventArgs e) => BoolDictionary["analyzeObjectCount"] = checkBox48.Checked;
        private void checkBox49_CheckedChanged(object sender, EventArgs e) => BoolDictionary["decryptLevelData"] = checkBox49.Checked;
        private void checkBox50_CheckedChanged(object sender, EventArgs e) => BoolDictionary["decryptLevelStrings"] = checkBox50.Checked;
        private void checkBox51_CheckedChanged(object sender, EventArgs e) => BoolDictionary["analyzeUsedGroupIDs"] = checkBox51.Checked;
        #endregion
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            IntDictionary["locationMeasurementUnit"] = ToInt32(radioButton2.Checked);
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            IntDictionary["maxUndoRedoActions"] = (int)numericUpDown1.Value;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox1.Text = saveFileDialog1.FileName;
            StringDictionary["defaultSettingsPath"] = textBox1.Text;
        }
        
        public static void SavePreferences()
        {
            Directory.CreateDirectory("EffectSome");
            string[] fileData = new string[BoolDictionary.Count + IntDictionary.Count + StringDictionary.Count];
            for (int i = 0; i < BoolDictionary.Count; i++)
                fileData[i] += BoolDictionary.Keys.ElementAt(i) + "=" + BoolDictionary[BoolDictionary.Keys.ElementAt(i)];
            for (int i = 0; i < IntDictionary.Count; i++)
                fileData[BoolDictionary.Count + i] += IntDictionary.Keys.ElementAt(i) + "=" + IntDictionary[IntDictionary.Keys.ElementAt(i)];
            for (int i = 0; i < StringDictionary.Count; i++)
                fileData[BoolDictionary.Count + IntDictionary.Count + i] += StringDictionary.Keys.ElementAt(i) + "=\"" + StringDictionary[StringDictionary.Keys.ElementAt(i)] + "\"";
            File.WriteAllLines("EffectSome/preferences.esf", fileData);
        }
        public static void LoadPreferences()
        {
            string[] fileData = File.ReadAllLines("EffectSome/preferences.esf");
            for (int i = 0; i < fileData.Length; i++)
            {
                string[] option = fileData[i].Split('=');
                option = new string[] { option[0], option.RemoveAt(0).Combine("=") };
                if (bool.TryParse(option[1], out bool b))
                    BoolDictionary[option[0]] = b;
                else if (int.TryParse(option[1], out int d))
                    IntDictionary[option[0]] = d;
                else
                    StringDictionary[option[0]] = option[1].Substring(1, option[1].Length - 2);
            }
        }
        public static void InitializePreferences()
        {
            BoolDictionary.Add("autoLoadSettings", true);
            BoolDictionary.Add("autoSaveSettings", true);
            BoolDictionary.Add("allWarnings", true);
            BoolDictionary.Add("exitProgramUnsavedChangesWarnings", true);
            BoolDictionary.Add("customObjObjsLimWarnings", true);
            BoolDictionary.Add("objLimWarnings", true);
            BoolDictionary.Add("customObjLimWarnings", true);
            BoolDictionary.Add("DLLErrorWarnings", true);
            BoolDictionary.Add("showDeleteLevelsPrompt", true);
            BoolDictionary.Add("allowEditLevelIDs", false);
            BoolDictionary.Add("allowEditSongIDs", true);
            BoolDictionary.Add("allowEditLevelVersions", true);
            BoolDictionary.Add("allowEditLevelRevisions", true);
            BoolDictionary.Add("analyzeObjectCount", true);
            BoolDictionary.Add("analyzeUsedGroupIDs", true);
            BoolDictionary.Add("decryptLevelData", true);
            BoolDictionary.Add("decryptLevelStrings", true);
            BoolDictionary.Add("orangeGuidelineShortcut", true);
            BoolDictionary.Add("yellowGuidelineShortcut", true);
            BoolDictionary.Add("greenGuidelineShortcut", true);
            BoolDictionary.Add("pauseGuidelineRecordingShortcut", true);
            BoolDictionary.Add("cloneLevelsShortcut", true);
            BoolDictionary.Add("deleteLevelsShortcut", true);
            BoolDictionary.Add("moveLevelsUpShortcut", true);
            BoolDictionary.Add("moveLevelsDownShortcut", true);
            BoolDictionary.Add("moveLevelsToTopShortcut", true);
            BoolDictionary.Add("moveLevelsToBottomShortcut", true);
            BoolDictionary.Add("swapLevelsShortcut", true);
            BoolDictionary.Add("selectAllLevelsShortcut", true);
            BoolDictionary.Add("deselectAllLevelsShortcut", true);
            BoolDictionary.Add("selectAllLevelsAboveShortcut", true);
            BoolDictionary.Add("selectAllLevelsBelowShortcut", true);
            BoolDictionary.Add("invertLevelSelectionShortcut", true);
            BoolDictionary.Add("applyChangesShortcut", true);
            BoolDictionary.Add("checkForUpdatesOnStartup", true);
            BoolDictionary.Add("checkForResourceUpdatesOnStartup", true);
            BoolDictionary.Add("gamesaveDecryptedNotifications", true);
            BoolDictionary.Add("levelDataDecryptedNotifications", true);
            BoolDictionary.Add("levelInfoRetrieved", true);
            BoolDictionary.Add("emptyLevelsLevelStringsSetNotifications", true);
            BoolDictionary.Add("resourceUpdateStatusNotifications", true);
            BoolDictionary.Add("softwareUpdateStatusNotifications", true);
            BoolDictionary.Add("showOperationTimes", true);
            StringDictionary.Add("defaultSettingsPath", "EffectSome/settings.esf");
            StringDictionary.Add("orangeGuidelineShortcutKey", "1");
            StringDictionary.Add("yellowGuidelineShortcutKey", "2");
            StringDictionary.Add("greenGuidelineShortcutKey", "3");
            StringDictionary.Add("pauseGuidelineRecordingShortcutKey", "P");
            StringDictionary.Add("cloneLevelsShortcutKey", "Shift+C");
            StringDictionary.Add("deleteLevelsShortcutKey", "Shift+Delete");
            StringDictionary.Add("moveLevelsUpShortcutKey", "Shift+Up Arrow");
            StringDictionary.Add("moveLevelsDownShortcutKey", "Shift+Down Arrow");
            StringDictionary.Add("moveLevelsToTopShortcutKey", "Shift+Home");
            StringDictionary.Add("moveLevelsToBottomShortcutKey", "Shift+End");
            StringDictionary.Add("swapLevelsShortcutKey", "Alt+S");
            StringDictionary.Add("selectAllLevelsShortcutKey", "Shift+S");
            StringDictionary.Add("deselectAllLevelsShortcutKey", "Shift+D");
            StringDictionary.Add("selectAllLevelsAboveShortcutKey", "Shift+A");
            StringDictionary.Add("selectAllLevelsBelowShortcutKey", "Shift+B");
            StringDictionary.Add("invertLevelSelectionShortcutKey", "Shift+I");
            StringDictionary.Add("applyChangesShortcutKey", "Ctrl+S");
            IntDictionary.Add("maxUndoRedoActions", 256);
            IntDictionary.Add("locationMeasurementUnit", 0);
            IntDictionary.Add("filterOptions", 0);
        }
        void ShowPreferences()
        {
            textBox1.Text = StringDictionary["defaultSettingsPath"];
            textBox2.Text = kc.ConvertToString(StringDictionary["orangeGuidelineShortcutKey"]);
            textBox3.Text = kc.ConvertToString(StringDictionary["yellowGuidelineShortcutKey"]);
            textBox4.Text = kc.ConvertToString(StringDictionary["greenGuidelineShortcutKey"]);
            textBox5.Text = kc.ConvertToString(StringDictionary["pauseGuidelineRecordingShortcutKey"]);
            checkBox1.Checked = BoolDictionary["autoLoadSettings"];
            checkBox2.Checked = BoolDictionary["autoSaveSettings"];
            checkBox3.Checked = BoolDictionary["showOperationTimes"];
            checkBox4.Checked = BoolDictionary["invertLevelSelectionShortcut"];
            checkBox5.Checked = BoolDictionary["applyChangesShortcut"];
            checkBox6.Checked = BoolDictionary["objLimWarnings"];
            checkBox7.Checked = BoolDictionary["customObjLimWarnings"];
            checkBox8.Checked = BoolDictionary["customObjObjsLimWarnings"];
            checkBox10.Checked = BoolDictionary["exitProgramUnsavedChangesWarnings"];
            checkBox11.Checked = BoolDictionary["showDeleteLevelsPrompt"];
            checkBox12.Checked = BoolDictionary["allWarnings"];
            checkBox14.Checked = BoolDictionary["orangeGuidelineShortcut"];
            checkBox15.Checked = BoolDictionary["yellowGuidelineShortcut"];
            checkBox16.Checked = BoolDictionary["greenGuidelineShortcut"];
            checkBox17.Checked = BoolDictionary["DLLErrorWarnings"];
            checkBox18.Checked = BoolDictionary["pauseGuidelineRecordingShortcut"];
            checkBox19.Checked = BoolDictionary["checkForResourceUpdatesOnStartup"];
            checkBox20.Checked = BoolDictionary["checkForUpdatesOnStartup"];
            checkBox22.Checked = BoolDictionary["gamesaveDecryptedNotifications"]; // Useless notifications
            checkBox23.Checked = BoolDictionary["levelInfoRetrieved"]; // Useless notifications
            checkBox24.Checked = BoolDictionary["levelDataDecryptedNotifications"]; // Useless notifications
            checkBox25.Checked = BoolDictionary["softwareUpdateStatusNotifications"];
            checkBox26.Checked = BoolDictionary["resourceUpdateStatusNotifications"];
            checkBox28.Checked = BoolDictionary["emptyLevelsLevelStringsSetNotifications"];
            checkBox31.Checked = BoolDictionary["cloneLevelsShortcut"];
            checkBox32.Checked = BoolDictionary["deleteLevelsShortcut"];
            checkBox33.Checked = BoolDictionary["moveLevelsUpShortcut"];
            checkBox34.Checked = BoolDictionary["moveLevelsDownShortcut"];
            checkBox36.Checked = BoolDictionary["moveLevelsToBottomShortcut"];
            checkBox37.Checked = BoolDictionary["moveLevelsToTopShortcut"];
            checkBox38.Checked = BoolDictionary["swapLevelsShortcut"];
            checkBox39.Checked = BoolDictionary["selectAllLevelsShortcut"];
            checkBox40.Checked = BoolDictionary["deselectAllLevelsShortcut"];
            checkBox41.Checked = BoolDictionary["selectAllLevelsBelowShortcut"];
            checkBox42.Checked = BoolDictionary["selectAllLevelsAboveShortcut"];
            checkBox43.Checked = BoolDictionary["allowEditLevelIDs"];
            checkBox44.Checked = BoolDictionary["allowEditLevelVersions"];
            checkBox45.Checked = BoolDictionary["allowEditLevelRevisions"];
            checkBox46.Checked = BoolDictionary["allowEditSongIDs"];
            checkBox48.Checked = BoolDictionary["analyzeObjectCount"];
            checkBox49.Checked = BoolDictionary["decryptLevelData"];
            checkBox50.Checked = BoolDictionary["decryptLevelStrings"];
            checkBox51.Checked = BoolDictionary["analyzeUsedGroupIDs"];
            radioButton1.Checked = IntDictionary["locationMeasurementUnit"] == (int)LocationMeasurementUnit.Units;
            radioButton2.Checked = IntDictionary["locationMeasurementUnit"] == (int)LocationMeasurementUnit.Blocks;
            radioButton3.Checked = IntDictionary["filterOptions"] == (int)FilterOptions.Stack;
            radioButton4.Checked = IntDictionary["filterOptions"] == (int)FilterOptions.Exclusive;
            numericUpDown1.Value = IntDictionary["maxUndoRedoActions"];
        }
    }

    public enum LocationMeasurementUnit
    {
        Units = 0,
        Blocks = 1
    }

    public enum FilterOptions
    {
        Stack = 0,
        Exclusive = 1
    }
}