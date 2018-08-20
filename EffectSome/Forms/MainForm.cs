using EffectSome.Objects.CopyPasteSettings;
using EffectSome.Utilities.Functions.GeometryDash.CopyPaste;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static EffectSome.CopyPasteSettingsWriting;
using static EffectSome.Editor;
using static EffectSome.Gamesave;
using static EffectSome.GlobalParameterSettings;
using static EffectSome.MemoryEdit;
using static EffectSome.Utilities.Functions.GeometryDash.Easing;
using static System.Convert;
using static System.IO.Directory;
using static System.IO.File;

namespace EffectSome
{
    public partial class EffectSome : Form
    {
        public static readonly string[] OfficialSongNames = { "Stereo Madness", "Back On Track", "Polargeist", "Dry Out", "Base After Base", "Can't Let Go", "Jumper", "Time Machine", "Cycles", "xStep", "Clutterfunk", "Theory Of Everything", "Electroman Adventures", "Clubstep", "Electrodynamix", "Hexagon Force", "Blast Processing", "Theory Of Everything 2", "Geometrical Dominator", "Deadlocked", "Fingerbang" };

        #region Copy-Paste Automation Settigns
        #region Objects
        public static bool applyForAllObjects = true, applyForSpecifiedObjectIDs;
        public static List<int> objectIDs = new List<int>();
        public static float rotationAdj, scalingAdj, hue1Adj, sat1Adj, val1Adj, hue2Adj, sat2Adj, val2Adj;
        public static int EL1Adj, EL2Adj, ZOrderAdj, ZLayerAdj;
        public static int mainColorIDPresetIndex = -1, detailColorIDPresetIndex = -1, groupIDPresetIndex = -1, autoCopyPastePresetIndex = -1;
        #endregion
        #endregion
        #region Dialog instances
        //ObjectsSelectionMenu objs;
        //QuickSelection quickSelection;
        //LimitBypasses limits;
        //StatusBox status;
        //TriggersSelectionMenu triggaz;
        //ColorChannelEditor color;
        //ExtraLevelEditorFunctions extraLevelEditorFunctions;
        //InitialObjectValuesEditor initialObjectValuesEditor;
        About AboutWindow;
        Options OptionsWindow;
        GlobalParameterSettings GlobalParameterSettingsWindow;
        GuidelineEditor GuidelineEditorWindow;
        LevelOverview LevelOverviewWindow;
        LevelVersionConverter LevelVersionConverterWindow;
        CustomObjectEditor CustomObjectEditorWindow;
        #endregion
        #region HTTP stuff
        public static WebClient songIDUploader;
        public static WebClient userInfoUploader;
        public static WebClient updateDownloader;
        public static WebClient resourceDownloader;
        public static HttpWebRequest checkForUpdates = WebRequest.CreateHttp("http://someeffect.altervista.org/checkForUpdates.php");
        #endregion
        #region BGWs
        public static BackgroundWorker writeCP = new BackgroundWorker();
        public static BackgroundWorker decryptGamesaveAndUploadInfo = new BackgroundWorker();
        public static BackgroundWorker getLevelData = new BackgroundWorker();
        public static BackgroundWorker updateResources = new BackgroundWorker();
        public static bool doneDecryptingGamesave, successfullyRetrievedLevelData, isLevelDataValid, canOpenGuidelineEditor, canOpenLevelOverview, canOpenLevelVersionConverter;
        #endregion
        #region Gamesave data
        // To migrate
        public static List<Level> UserLevels;
        public static List<string> FolderNames;
        public static List<int> LevelKeyStartIndices;
        public static string UserName = "";
        public static string DecryptedGamesave = "";
        public static string DecryptedLevelData = "";
        public static int UserLevelCount => UserLevels.Count;
        public static List<CustomLevelObject> CustomObjects;
        #endregion
        #region Update stuff
        public static string updateFilePath;
        public static bool updatedVersion = true;
        #endregion
        #region Process stuff
        public static Process process = Process.GetProcessesByName("GeometryDash").FirstOrDefault();
        public static IntPtr processHandle;
        public static int baseAddress = 0x3222D0;
        #endregion
        #region Level Editor stuff
        public static int[] objIDs, selectedObjIDs, buildObjIDs, selectedObjMainColorIDs, selectedObjDetailColorIDs;
        public static int[,] selectedObjGroupIDs;
        public static List<int> usedGroupIDs, usedColorIDs;
        public static List<LevelObject> selectedObjects, editorLevelObjects;
        #endregion
        #region Custom Variables stuff
        public static List<string> customVariableNames = new List<string>();
        public static List<double> customVariablesInitialValues = new List<double>();
        public static List<double> customVariablesAdjustments = new List<double>();
        public static List<int> selectedVariableIndices = new List<int>();
        #endregion
        #region Undo/Redo stuff
        public static string initialValues = "";
        public static List<string> undo = new List<string>();
        public static List<string> redo = new List<string>();
        public static List<string> undoActionDescs = new List<string>();
        public static List<string> redoActionDescs = new List<string>();
        public static bool canRegisterActions = false;
        #endregion

        public static List<CopyPasteSettings> CopyPasteSettings;
        public static Dictionary<int, double> CustomSongBPMs;

        public static NotifyIcon notification;
        public static string appLocation;

        public static bool canWriteCP = true;
        public static string ESFileName = Application.ExecutablePath.Split('\\').ExcludeLast(1).Combine("\\");
        public static string GDLocalData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\GeometryDash";
        public static string version = "0.6.5a";
        public static bool isGDOpen = false;
        
        public static int timesAboutDialogWasShown, timesLennyDialogWasShown;

        public EffectSome()
        {
            CreateFolders();
            CopyPasteSettingsWritingFunctions.InitializeCopyPasteAutomationSettings();
            SetupWebObjects();
            appLocation = GetCurrentDirectory();
            InitializeNotificationIcon();
            Options.InitializePreferences();
            if (File.Exists("EffectSome/preferences.esf"))
                Options.LoadPreferences();
            else
                Options.SavePreferences();
            GetBPMValuesFromFile();
            DeclareBGWs();
            RunBGWs();
            notification.Text = "Initializing Window...";
            InitializeComponent();
            isGDOpen = CheckGDStatus();
            if (isGDOpen)
            {
                InjectCopyPasteDLL();
            }
            else
            {
                try
                {
                    FileStream a = OpenRead("EffectSome\\lib\\Auto_CP.dll");
                    FileStream b = OpenRead(GDLocalData + "\\Auto_CP.dll");
                    if (!File.Exists(GDLocalData + "\\Auto_CP.dll") || a.Length != b.Length)
                    {
                        a.Close();
                        b.Close();
                        File.Delete(GDLocalData + "\\Auto_CP.dll");
                        Copy("EffectSome\\lib\\Auto_CP.dll", GDLocalData + "\\Auto_CP.dll");
                    }
                }
                catch (FileNotFoundException)
                {
                    Copy("EffectSome\\lib\\Auto_CP.dll", GDLocalData + "\\Auto_CP.dll");
                }
                tabControl2.Enabled = false;
            }
            comboBox1.Text = comboBox44.Text = "None";
            //CheckTriggersStatus(); // ??? Nigga
            SaveSettings("EffectSome/default.esf");
            if (Options.BoolDictionary["autoLoadSettings"])
                if (File.Exists(Options.StringDictionary["defaultSettingsPath"]))
                    try { LoadSettings(Options.StringDictionary["defaultSettingsPath"]); }
                    catch (FormatException)
                    {
                        notification.ShowBalloonTip(5000, "Corrupted File", "The default settings file is corrupted. The program will have its default settings.", ToolTipIcon.Warning);
                        SaveSettings(Options.StringDictionary["defaultSettingsPath"]);
                    }
            RegisterStartingValues();
            ReadPresets();
            LoadPresetObjects();
            notification.Text = "Ready";
            //DetectDLLStatus();
            canRegisterActions = true;
        }

        private void EffectSome_Activated(object sender, EventArgs e)
        {
            ReadPresets();
            CheckDialogStatus();
            CheckGDStatus();
        }
        private void EffectSome_FormClosing(object sender, FormClosingEventArgs e)
        {
            Options.SavePreferences();
            if (GuidelineEditor.IsOpen)
                GuidelineEditorWindow.Close();
            if (Options.BoolDictionary["autoSaveSettings"])
                SaveSettings(Options.StringDictionary["defaultSettingsPath"]);
            else
            {
                DialogResult result = MessageBox.Show("You haven't saved your current settings list yet! Do you wish to save them now before quitting?", "Save settings before quitting", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                    SaveSettings(Options.StringDictionary["defaultSettingsPath"]);
                else if (result == DialogResult.No)
                    Close();
                else if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }
        private void EffectSome_FormClosed(object sender, FormClosedEventArgs e)
        {
            decryptGamesaveAndUploadInfo.CancelAsync();
        }

        #region Buttons
        private void button1_Click(object sender, EventArgs e)
        {
            writeCP.RunWorkerAsync(2);
            if (canRegisterActions)
                RegisterAction("Write Special Objects Copy-Paste Automation");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            writeCP.RunWorkerAsync(0);
            if (canRegisterActions)
                RegisterAction("Write Objects Copy-Paste Automation");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            int[] selected = new int[listBox1.SelectedIndices.Count];
            for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                selected[i] = (int)listBox1.SelectedItems[i];
            objectIDs = RemoveItems(objectIDs, selected);
            RemoveItems(listBox1);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            writeCP.RunWorkerAsync(1);
            if (canRegisterActions)
                RegisterAction("Write Triggers Copy-Paste Automation");
        }
        private void button6_Click(object sender, EventArgs e)
        {
            AddItem(listBox1, (int)numericUpDown17.Value);
            objectIDs = AddItem(objectIDs, (int)numericUpDown17.Value);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            List<int> objIDs = GetCurrentlySelectedObjectIDs();
            for (int i = 0; i < objIDs.Count; i++)
            {
                AddItem(listBox1, objIDs[i]);
                objectIDs = AddItem(objectIDs, objIDs[i]);
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            if (canRegisterActions)
                RegisterAction("Change Property");
        }
        private void button10_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
            customVariableNames.Add("");
            customVariablesAdjustments.Add(0);
            customVariablesInitialValues.Add(0);
            dataGridView1[2, dataGridView1.Rows.Count - 1].Value = "0";
            dataGridView1[3, dataGridView1.Rows.Count - 1].Value = "0";
            if (canRegisterActions)
                RegisterAction("Add Custom Variable");
        }
        private void button11_Click(object sender, EventArgs e)
        {
            for (int i = selectedVariableIndices.Count - 1; i >= 0; i--)
            {
                dataGridView1.Rows.RemoveAt(selectedVariableIndices[i]);
                customVariableNames.RemoveAt(selectedVariableIndices[i]);
                customVariablesAdjustments.RemoveAt(selectedVariableIndices[i]);
                customVariablesInitialValues.RemoveAt(selectedVariableIndices[i]);
            }
            if (canRegisterActions)
                RegisterAction("Remove " + selectedVariableIndices.Count + " Custom Variables");
            selectedVariableIndices.Clear();
            button11.Enabled = false;
        }
        private void button13_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region RadioButtons
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox46.Enabled = radioButton3.Checked;
            checkBox31.Enabled = checkBox32.Enabled = checkBox239.Enabled = !radioButton4.Checked;
            comboBox23.Enabled = checkBox46.Checked && radioButton3.Checked;
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e) => comboBox24.Enabled = (checkBox3.Enabled = radioButton4.Checked) && checkBox3.Checked;
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            checkBox59.Enabled = checkBox60.Enabled = checkBox235.Enabled = !radioButton7.Checked;
            numericUpDown52.Enabled = !radioButton7.Checked && checkBox60.Checked;
            numericUpDown53.Enabled = !radioButton7.Checked && checkBox235.Checked;
            numericUpDown54.Enabled = !radioButton7.Checked && checkBox59.Checked;
            checkBox174.Enabled = !radioButton5.Checked;
            comboBox31.Enabled = !radioButton5.Checked && checkBox174.Checked;
        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            checkBox31.Enabled = checkBox32.Enabled = checkBox239.Enabled = !radioButton4.Checked;
            checkBox46.Enabled = checkBox3.Enabled = radioButton6.Checked;
            comboBox24.Enabled = checkBox3.Checked && radioButton6.Checked;
            comboBox23.Enabled = checkBox46.Checked && radioButton6.Checked;
        }
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            checkBox236.Enabled = checkBox237.Enabled = checkBox238.Enabled = !radioButton5.Checked;
            numericUpDown55.Enabled = !radioButton5.Checked && checkBox237.Checked;
            numericUpDown56.Enabled = !radioButton5.Checked && checkBox238.Checked;
            numericUpDown57.Enabled = !radioButton5.Checked && checkBox236.Checked;
            checkBox174.Enabled = !radioButton5.Checked;
            comboBox31.Enabled = !radioButton5.Checked && checkBox174.Checked;
        }
        private void radioButton8_CheckedChanged(object sender, EventArgs e) => numericUpDown42.Enabled = (checkBox129.Enabled = radioButton8.Checked) && checkBox129.Checked;
        private void radioButton10_CheckedChanged(object sender, EventArgs e) => checkBox152.Enabled = radioButton10.Checked;
        private void radioButton11_CheckedChanged(object sender, EventArgs e) => checkBox153.Enabled = radioButton11.Checked;
        private void radioButton13_CheckedChanged(object sender, EventArgs e) => applyForAllObjects = radioButton13.Checked;
        private void radioButton14_CheckedChanged(object sender, EventArgs e) => button3.Enabled = (numericUpDown17.Enabled = button6.Enabled = button8.Enabled = listBox1.Enabled = applyForSpecifiedObjectIDs = radioButton14.Checked) && listBox1.SelectedItems.Count > 0;
        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
            checkBox59.Enabled = checkBox60.Enabled = checkBox235.Enabled = !radioButton7.Checked;
            checkBox236.Enabled = checkBox237.Enabled = checkBox238.Enabled = !radioButton5.Checked;
            numericUpDown52.Enabled = !radioButton7.Checked && checkBox60.Checked;
            numericUpDown53.Enabled = !radioButton7.Checked && checkBox235.Checked;
            numericUpDown54.Enabled = !radioButton7.Checked && checkBox59.Checked;
            numericUpDown55.Enabled = !radioButton5.Checked && checkBox237.Checked;
            numericUpDown56.Enabled = !radioButton5.Checked && checkBox238.Checked;
            numericUpDown57.Enabled = !radioButton5.Checked && checkBox236.Checked;
            checkBox174.Enabled = !radioButton5.Checked;
            comboBox31.Enabled = !radioButton5.Checked && checkBox174.Checked;
        }
        private void radioButton18_CheckedChanged(object sender, EventArgs e) => checkBox148.Enabled = radioButton18.Checked;
        #endregion
        #region ComboBoxes
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) => checkBox24.Enabled = checkBox25.Enabled = !(comboBox1.Text == "None");
        private void comboBox28_SelectedIndexChanged(object sender, EventArgs e) => mainColorIDPresetIndex = checkBox78.Checked ? comboBox28.SelectedIndex : -1;
        private void comboBox29_SelectedIndexChanged(object sender, EventArgs e) => groupIDPresetIndex = checkBox13.Checked ? comboBox29.SelectedIndex : -1;
        private void comboBox30_SelectedIndexChanged(object sender, EventArgs e) => autoCopyPastePresetIndex = checkBox8.Checked ? comboBox30.SelectedIndex : -1;
        private void comboBox44_SelectedIndexChanged(object sender, EventArgs e) => checkBox95.Enabled = checkBox61.Enabled = !(comboBox44.Text == "None");
        private void comboBox77_SelectedIndexChanged(object sender, EventArgs e) => detailColorIDPresetIndex = checkBox19.Checked ? comboBox77.SelectedIndex : -1;
        #endregion
        #region CheckBoxes
        private void checkBox3_CheckedChanged(object sender, EventArgs e) => comboBox24.Enabled = checkBox3.Checked;
        private void checkBox8_CheckedChanged(object sender, EventArgs e) => comboBox30.Enabled = checkBox8.Checked;
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            ZOrderAdj = checkBox9.Checked ? (int)numericUpDown39.Value : 0;
            numericUpDown39.Enabled = checkBox9.Checked;
        }
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            hue1Adj = checkBox10.Checked ? (float)numericUpDown36.Value : 0;
            hue2Adj = checkBox10.Checked ? (float)numericUpDown59.Value : 0;
            numericUpDown36.Enabled = numericUpDown59.Enabled = checkBox10.Checked;
        }
        private void checkBox13_CheckedChanged(object sender, EventArgs e) => comboBox29.Enabled = checkBox13.Checked;
        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = checkBox14.Checked && !checkBox33.Checked;
            checkBox33.Enabled = checkBox14.Checked;
        }
        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            ZLayerAdj = checkBox18.Checked ? (int)numericUpDown40.Value : 0;
            numericUpDown40.Enabled = checkBox18.Checked;
        }
        private void checkBox19_CheckedChanged(object sender, EventArgs e) => comboBox77.Enabled = checkBox19.Checked;
        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = checkBox22.Checked && !checkBox34.Checked;
            checkBox34.Enabled = checkBox22.Checked;
        }
        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox24.Checked && !checkBox25.Checked)
                checkBox25.Checked = true;
        }
        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBox24.Checked == false) && (checkBox25.Checked == false))
                checkBox24.Checked = true;
        }
        private void checkBox26_CheckedChanged(object sender, EventArgs e) => numericUpDown3.Enabled = checkBox26.Checked;
        private void checkBox27_CheckedChanged(object sender, EventArgs e) => numericUpDown4.Enabled = checkBox27.Checked;
        private void checkBox28_CheckedChanged(object sender, EventArgs e) => numericUpDown5.Enabled = checkBox28.Checked;
        private void checkBox33_CheckedChanged(object sender, EventArgs e) => numericUpDown1.Enabled = !checkBox33.Checked;
        private void checkBox34_CheckedChanged(object sender, EventArgs e) => numericUpDown2.Enabled = !checkBox34.Checked;
        private void checkBox37_CheckedChanged(object sender, EventArgs e) => numericUpDown6.Enabled = checkBox37.Checked;
        private void checkBox40_CheckedChanged(object sender, EventArgs e) => numericUpDown7.Enabled = checkBox40.Checked;
        private void checkBox42_CheckedChanged(object sender, EventArgs e) => numericUpDown8.Enabled = checkBox42.Checked;
        private void checkBox44_CheckedChanged(object sender, EventArgs e) => numericUpDown9.Enabled = checkBox44.Checked;
        private void checkBox45_CheckedChanged(object sender, EventArgs e) => numericUpDown10.Enabled = checkBox45.Checked;
        private void checkBox46_CheckedChanged(object sender, EventArgs e) => comboBox23.Enabled = checkBox46.Checked;
        private void checkBox47_CheckedChanged(object sender, EventArgs e) => numericUpDown11.Enabled = checkBox47.Checked;
        private void checkBox48_CheckedChanged(object sender, EventArgs e) => numericUpDown12.Enabled = checkBox48.Checked;
        private void checkBox49_CheckedChanged(object sender, EventArgs e) => comboBox40.Enabled = checkBox49.Checked;
        private void checkBox50_CheckedChanged(object sender, EventArgs e) => comboBox39.Enabled = checkBox50.Checked;
        private void checkBox53_CheckedChanged(object sender, EventArgs e) => comboBox41.Enabled = checkBox53.Checked;
        private void checkBox56_CheckedChanged(object sender, EventArgs e) => numericUpDown13.Enabled = checkBox56.Checked;
        private void checkBox59_CheckedChanged(object sender, EventArgs e) => numericUpDown54.Enabled = checkBox59.Checked;
        private void checkBox60_CheckedChanged(object sender, EventArgs e) => numericUpDown52.Enabled = checkBox60.Checked;
        private void checkBox61_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBox61.Checked == false) && (checkBox95.Checked == false))
                checkBox95.Checked = true;
        }
        private void checkBox66_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox67_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox72_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox73_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox74_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox75_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox77_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox78_CheckedChanged(object sender, EventArgs e) => comboBox28.Enabled = checkBox78.Checked;
        private void checkBox81_CheckedChanged(object sender, EventArgs e) => comboBox9.Enabled = checkBox81.Checked;
        private void checkBox84_CheckedChanged(object sender, EventArgs e) => comboBox8.Enabled = checkBox84.Checked;
        private void checkBox91_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox95_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBox61.Checked == false) && (checkBox95.Checked == false))
                checkBox61.Checked = true;
        }
        private void checkBox97_CheckedChanged(object sender, EventArgs e) => numericUpDown14.Enabled = checkBox97.Checked;
        private void checkBox98_CheckedChanged(object sender, EventArgs e) => numericUpDown15.Enabled = checkBox98.Checked;
        private void checkBox99_CheckedChanged(object sender, EventArgs e) => comboBox43.Enabled = checkBox99.Checked;
        private void checkBox100_CheckedChanged(object sender, EventArgs e) => comboBox19.Enabled = checkBox100.Checked;
        private void checkBox101_CheckedChanged(object sender, EventArgs e) => numericUpDown16.Enabled = checkBox101.Checked;
        private void checkBox102_CheckedChanged(object sender, EventArgs e) => comboBox42.Enabled = checkBox102.Checked;
        private void checkBox103_CheckedChanged(object sender, EventArgs e) => comboBox45.Enabled = checkBox103.Checked;
        private void checkBox104_CheckedChanged(object sender, EventArgs e) => numericUpDown18.Enabled = checkBox104.Checked;
        private void checkBox107_CheckedChanged(object sender, EventArgs e) => comboBox47.Enabled = checkBox107.Checked;
        private void checkBox108_CheckedChanged(object sender, EventArgs e) => comboBox46.Enabled = checkBox108.Checked;
        private void checkBox109_CheckedChanged(object sender, EventArgs e) => comboBox27.Enabled = checkBox109.Checked;
        private void checkBox110_CheckedChanged(object sender, EventArgs e) => numericUpDown22.Enabled = checkBox110.Checked;
        private void checkBox115_CheckedChanged(object sender, EventArgs e) => comboBox26.Enabled = checkBox115.Checked;
        private void checkBox116_CheckedChanged(object sender, EventArgs e) => numericUpDown26.Enabled = checkBox116.Checked;
        private void checkBox117_CheckedChanged(object sender, EventArgs e) => numericUpDown24.Enabled = checkBox117.Checked;
        private void checkBox118_CheckedChanged(object sender, EventArgs e) => comboBox7.Enabled = checkBox118.Checked;
        private void checkBox119_CheckedChanged(object sender, EventArgs e) => numericUpDown23.Enabled = checkBox119.Checked;
        private void checkBox120_CheckedChanged(object sender, EventArgs e) => numericUpDown25.Enabled = checkBox120.Checked;
        private void checkBox122_CheckedChanged(object sender, EventArgs e) => comboBox2.Enabled = checkBox122.Checked;
        private void checkBox123_CheckedChanged(object sender, EventArgs e) => comboBox20.Enabled = checkBox123.Checked;
        private void checkBox127_CheckedChanged(object sender, EventArgs e) => comboBox5.Enabled = checkBox127.Checked;
        private void checkBox128_CheckedChanged(object sender, EventArgs e) => comboBox25.Enabled = checkBox128.Checked;
        private void checkBox129_CheckedChanged(object sender, EventArgs e) => numericUpDown42.Enabled = checkBox129.Checked;
        private void checkBox130_CheckedChanged(object sender, EventArgs e) => comboBox14.Enabled = checkBox130.Checked;
        private void checkBox131_CheckedChanged(object sender, EventArgs e) => comboBox11.Enabled = checkBox131.Checked;
        private void checkBox132_CheckedChanged(object sender, EventArgs e) => comboBox12.Enabled = checkBox132.Checked;
        private void checkBox133_CheckedChanged(object sender, EventArgs e) => comboBox15.Enabled = checkBox133.Checked;
        private void checkBox134_CheckedChanged(object sender, EventArgs e) => comboBox13.Enabled = checkBox134.Checked;
        private void checkBox136_CheckedChanged(object sender, EventArgs e) => comboBox3.Enabled = checkBox136.Checked;
        private void checkBox138_CheckedChanged(object sender, EventArgs e) => comboBox10.Enabled = checkBox138.Checked;
        private void checkBox139_CheckedChanged(object sender, EventArgs e) => comboBox18.Enabled = checkBox139.Checked;
        private void checkBox142_CheckedChanged(object sender, EventArgs e) => comboBox6.Enabled = checkBox142.Checked;
        private void checkBox143_CheckedChanged(object sender, EventArgs e) => comboBox22.Enabled = checkBox143.Checked;
        private void checkBox144_CheckedChanged(object sender, EventArgs e) => comboBox16.Enabled = checkBox144.Checked;
        private void checkBox146_CheckedChanged(object sender, EventArgs e) => comboBox4.Enabled = checkBox146.Checked;
        private void checkBox147_CheckedChanged(object sender, EventArgs e) => comboBox21.Enabled = checkBox147.Checked;
        private void checkBox149_CheckedChanged(object sender, EventArgs e) => comboBox32.Enabled = checkBox149.Checked;
        private void checkBox151_CheckedChanged(object sender, EventArgs e) => comboBox17.Enabled = checkBox151.Checked;
        private void checkBox154_CheckedChanged(object sender, EventArgs e) => comboBox33.Enabled = checkBox154.Checked;
        private void checkBox155_CheckedChanged(object sender, EventArgs e) => numericUpDown43.Enabled = checkBox155.Checked;
        private void checkBox156_CheckedChanged(object sender, EventArgs e) => numericUpDown44.Enabled = checkBox158.Enabled = checkBox157.Enabled = checkBox156.Checked;
        private void checkBox157_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox157.Checked && !checkBox158.Checked)
                checkBox158.Checked = true;
        }
        private void checkBox158_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox157.Checked && !checkBox158.Checked)
                checkBox157.Checked = true;
        }
        private void checkBox159_CheckedChanged(object sender, EventArgs e) => comboBox50.Enabled = checkBox159.Checked;
        private void checkBox161_CheckedChanged(object sender, EventArgs e) => numericUpDown20.Enabled = checkBox161.Checked;
        private void checkBox162_CheckedChanged(object sender, EventArgs e) => comboBox49.Enabled = checkBox162.Checked;
        private void checkBox163_CheckedChanged(object sender, EventArgs e) => comboBox48.Enabled = checkBox163.Checked;
        private void checkBox164_CheckedChanged(object sender, EventArgs e) => comboBox35.Enabled = checkBox164.Checked;
        private void checkBox165_CheckedChanged(object sender, EventArgs e) => comboBox34.Enabled = checkBox165.Checked;
        private void checkBox166_CheckedChanged(object sender, EventArgs e) => comboBox70.Enabled = checkBox166.Checked;
        private void checkBox168_CheckedChanged(object sender, EventArgs e)
        {
            rotationAdj = checkBox168.Checked ? (float)numericUpDown32.Value : 0;
            numericUpDown32.Enabled = checkBox168.Checked;
        }
        private void checkBox169_CheckedChanged(object sender, EventArgs e)
        {
            scalingAdj = checkBox169.Checked ? (float)numericUpDown33.Value : 0;
            numericUpDown33.Enabled = checkBox169.Checked;
        }
        private void checkBox170_CheckedChanged(object sender, EventArgs e)
        {
            val1Adj = checkBox170.Checked ? (float)numericUpDown34.Value : 0;
            val2Adj = checkBox170.Checked ? (float)numericUpDown41.Value : 0;
            numericUpDown34.Enabled = numericUpDown41.Enabled = checkBox170.Checked;
        }
        private void checkBox171_CheckedChanged(object sender, EventArgs e)
        {
            sat1Adj = checkBox171.Checked ? (float)numericUpDown35.Value : 0;
            sat2Adj = checkBox171.Checked ? (float)numericUpDown58.Value : 0;
            numericUpDown35.Enabled = numericUpDown58.Enabled = checkBox171.Checked;
        }
        private void checkBox172_CheckedChanged(object sender, EventArgs e)
        {
            EL1Adj = checkBox172.Checked ? (int)numericUpDown38.Value : 0;
            EL2Adj = checkBox172.Checked ? (int)numericUpDown37.Value : 0;
            numericUpDown37.Enabled = numericUpDown38.Enabled = checkBox172.Checked;
        }
        private void checkBox174_CheckedChanged(object sender, EventArgs e) => comboBox31.Enabled = checkBox174.Checked;
        private void checkBox175_CheckedChanged(object sender, EventArgs e) => comboBox36.Enabled = checkBox175.Checked;
        private void checkBox176_CheckedChanged(object sender, EventArgs e) => numericUpDown46.Enabled = checkBox176.Checked;
        private void checkBox177_CheckedChanged(object sender, EventArgs e) => numericUpDown47.Enabled = checkBox177.Checked;
        private void checkBox178_CheckedChanged(object sender, EventArgs e) => numericUpDown48.Enabled = checkBox178.Checked;
        private void checkBox181_CheckedChanged(object sender, EventArgs e) => comboBox38.Enabled = checkBox181.Checked;
        private void checkBox182_CheckedChanged(object sender, EventArgs e) => comboBox37.Enabled = checkBox182.Checked;
        private void checkBox186_CheckedChanged(object sender, EventArgs e) => comboBox51.Enabled = checkBox186.Checked;
        private void checkBox189_CheckedChanged(object sender, EventArgs e) => numericUpDown19.Enabled = checkBox189.Checked;
        private void checkBox190_CheckedChanged(object sender, EventArgs e) => comboBox53.Enabled = checkBox190.Checked;
        private void checkBox191_CheckedChanged(object sender, EventArgs e) => comboBox52.Enabled = checkBox191.Checked;
        private void checkBox193_CheckedChanged(object sender, EventArgs e) => comboBox54.Enabled = checkBox193.Checked;
        private void checkBox196_CheckedChanged(object sender, EventArgs e) => comboBox57.Enabled = checkBox196.Checked;
        private void checkBox197_CheckedChanged(object sender, EventArgs e) => comboBox56.Enabled = checkBox197.Checked;
        private void checkBox198_CheckedChanged(object sender, EventArgs e) => comboBox55.Enabled = checkBox198.Checked;
        private void checkBox199_CheckedChanged(object sender, EventArgs e) => numericUpDown29.Enabled = checkBox199.Checked;
        private void checkBox200_CheckedChanged(object sender, EventArgs e) => numericUpDown21.Enabled = checkBox200.Checked;
        private void checkBox201_CheckedChanged(object sender, EventArgs e) => numericUpDown27.Enabled = checkBox201.Checked;
        private void checkBox202_CheckedChanged(object sender, EventArgs e) => numericUpDown28.Enabled = checkBox202.Checked;
        private void checkBox203_CheckedChanged(object sender, EventArgs e) => comboBox60.Enabled = checkBox203.Checked;
        private void checkBox204_CheckedChanged(object sender, EventArgs e) => comboBox59.Enabled = checkBox204.Checked;
        private void checkBox205_CheckedChanged(object sender, EventArgs e) => numericUpDown30.Enabled = checkBox205.Checked;
        private void checkBox208_CheckedChanged(object sender, EventArgs e) => comboBox58.Enabled = checkBox208.Checked;
        private void checkBox209_CheckedChanged(object sender, EventArgs e) => comboBox63.Enabled = checkBox209.Checked;
        private void checkBox210_CheckedChanged(object sender, EventArgs e) => comboBox61.Enabled = checkBox210.Checked;
        private void checkBox211_CheckedChanged(object sender, EventArgs e) => numericUpDown31.Enabled = checkBox211.Checked;
        private void checkBox212_CheckedChanged(object sender, EventArgs e) => comboBox62.Enabled = checkBox212.Checked;
        private void checkBox215_CheckedChanged(object sender, EventArgs e) => numericUpDown45.Enabled = checkBox215.Checked;
        private void checkBox220_CheckedChanged(object sender, EventArgs e) => comboBox69.Enabled = checkBox220.Checked;
        private void checkBox221_CheckedChanged(object sender, EventArgs e) => comboBox65.Enabled = checkBox221.Checked;
        private void checkBox222_CheckedChanged(object sender, EventArgs e) => comboBox64.Enabled = checkBox222.Checked;
        private void checkBox224_CheckedChanged(object sender, EventArgs e) => comboBox68.Enabled = checkBox224.Checked;
        private void checkBox225_CheckedChanged(object sender, EventArgs e) => comboBox71.Enabled = checkBox225.Checked;
        private void checkBox226_CheckedChanged(object sender, EventArgs e) => comboBox67.Enabled = checkBox226.Checked;
        private void checkBox227_CheckedChanged(object sender, EventArgs e) => comboBox66.Enabled = checkBox227.Checked;
        private void checkBox230_CheckedChanged(object sender, EventArgs e) => comboBox73.Enabled = checkBox230.Checked;
        private void checkBox231_CheckedChanged(object sender, EventArgs e) => comboBox72.Enabled = checkBox231.Checked;
        private void checkBox232_CheckedChanged(object sender, EventArgs e) => numericUpDown51.Enabled = checkBox232.Checked;
        private void checkBox233_CheckedChanged(object sender, EventArgs e) => numericUpDown49.Enabled = checkBox233.Checked;
        private void checkBox234_CheckedChanged(object sender, EventArgs e) => numericUpDown50.Enabled = checkBox234.Checked;
        private void checkBox235_CheckedChanged(object sender, EventArgs e) => numericUpDown53.Enabled = checkBox235.Checked;
        private void checkBox236_CheckedChanged(object sender, EventArgs e) => numericUpDown57.Enabled = checkBox236.Checked;
        private void checkBox237_CheckedChanged(object sender, EventArgs e) => numericUpDown55.Enabled = checkBox237.Checked;
        private void checkBox238_CheckedChanged(object sender, EventArgs e) => numericUpDown56.Enabled = checkBox238.Checked;
        private void checkBox240_CheckedChanged(object sender, EventArgs e) => comboBox74.Enabled = checkBox240.Checked;
        private void checkBox241_CheckedChanged(object sender, EventArgs e) => comboBox76.Enabled = checkBox241.Checked;
        private void checkBox242_CheckedChanged(object sender, EventArgs e) => comboBox75.Enabled = checkBox242.Checked;
        private void checkBox23_CheckStateChanged(object sender, EventArgs e)
        {
            checkBox24.Enabled = checkBox25.Enabled = (checkBox23.CheckState == CheckState.Checked) && (comboBox1.Text != "None");
            comboBox1.Enabled = checkBox23.CheckState == CheckState.Checked;
        }
        private void checkBox96_CheckStateChanged(object sender, EventArgs e)
        {
            checkBox95.Enabled = checkBox61.Enabled = (checkBox96.CheckState == CheckState.Checked) && (comboBox44.Text != "None");
            comboBox44.Enabled = checkBox96.CheckState == CheckState.Checked;
        }
        #endregion
        #region TextBoxes
        private void textBox3_TextChanged(object sender, EventArgs e) => button9.Enabled = textBox3.Text != "";
        #endregion
        #region ToolStripMenuItems
        private void guidelineEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isGDOpen = CheckGDStatus();
            while (isGDOpen)
            {
                isGDOpen = CheckGDStatus();
                DialogResult result = MessageBox.Show("You need to close Geometry Dash first in order to save the changes to the levels.", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Cancel)
                    return;
            }
            if (!isGDOpen)
            {
                if (GuidelineEditor.IsOpen)
                {
                    GuidelineEditorWindow.Close();
                    //GuidelineEditorWindow.IsOpen = false;
                }
                else
                {
                    notification.Text = "Loading Guideline Editor Presets...";
                    GuidelineEditorWindow = new GuidelineEditor();
                    GuidelineEditorWindow.Show();
                    //GuidelineEditorWindow.IsOpen = true;
                    notification.Text = "Ready";
                }
                CheckDialogStatus();
            }
        }
        private void globalParameterSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalParameterSettings.IsOpen)
            {
                GlobalParameterSettingsWindow.Close();
                //isGlobalOpen = false;
            }
            else
            {
                GlobalParameterSettingsWindow = new GlobalParameterSettings();
                //isGlobalOpen = true;
                GlobalParameterSettingsWindow.Show();
            }
            CheckDialogStatus();
        }
        private void customObjectEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CustomObjectEditor.IsOpen)
            {
                CustomObjectEditorWindow.Close();
                //isCustomObjectsOpen = false;
            }
            else
            {
                CustomObjectEditorWindow = new CustomObjectEditor();
                //isCustomObjectsOpen = true;
                CustomObjectEditorWindow.Show();
            }
            CheckDialogStatus();
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Options.IsOpen)
            {
                OptionsWindow.Close();
                //isOptionsOpen = false;
            }
            else
            {
                OptionsWindow = new Options();
                OptionsWindow.Show();
                //isOptionsOpen = true;
            }
        }
        private void objectSelectionMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (isObjsOpen == true)
            //{
            //    objectSelectionMenuToolStripMenuItem.Checked = false;
            //    objs.Close();
            //}
            //else
            //{
            //    objs = new ObjectsSelectionMenu();
            //    objectSelectionMenuToolStripMenuItem.Checked = true;
            //    objs.Show();
            //}
            //isObjsOpen = !isObjsOpen;
        }
        private void quickSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (isQuickSelectionOpen == true)
            //{
            //    quickSelectionToolStripMenuItem.Checked = false;
            //    quickSelection.Close();
            //}
            //else
            //{
            //    quickSelection = new QuickSelection();
            //    quickSelectionToolStripMenuItem.Checked = true;
            //    quickSelection.Show();
            //}
            //isQuickSelectionOpen = !isQuickSelectionOpen;
        }
        private void limitBypassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (isLimitOpen == true)
            //{
            //    limitBypassesToolStripMenuItem.Checked = false;
            //    limits.Close();
            //}
            //else
            //{
            //    limits = new LimitBypasses();
            //    limitBypassesToolStripMenuItem.Checked = true;
            //    limits.Show();
            //}
            //isLimitOpen = !isLimitOpen;
        }
        private void statusBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (isStatusBoxOpen == true)
            //{
            //    statusBoxToolStripMenuItem.Checked = false;
            //    status.Close();
            //}
            //else
            //{
            //    status = new StatusBox();
            //    statusBoxToolStripMenuItem.Checked = true;
            //    status.Show();
            //}
            //isStatusBoxOpen = !isStatusBoxOpen;
        }
        private void triggerSelectionMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (isTriggazOpen == true)
            //{
            //    triggerSelectionMenuToolStripMenuItem.Checked = false;
            //    triggaz.Close();
            //}
            //else
            //{
            //    triggaz = new TriggersSelectionMenu();
            //    triggerSelectionMenuToolStripMenuItem.Checked = true;
            //    triggaz.Show();
            //}
            //isTriggazOpen = !isTriggazOpen;
        }
        private void colorChannelEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (isColorOpen == true)
            //{
            //    colorChannelEditorToolStripMenuItem.Checked = false;
            //    color.Close();
            //}
            //else
            //{
            //    color = new ColorChannelEditor();
            //    colorChannelEditorToolStripMenuItem.Checked = true;
            //    color.Show();
            //}
            //isColorOpen = !isColorOpen;
        }
        private void extraLevelEditorFunctionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (isExtraLevelEditorFunctionsOpen == true)
            //    extraLevelEditorFunctions.Close();
            //else
            //{
            //    extraLevelEditorFunctions = new ExtraLevelEditorFunctions();
            //    extraLevelEditorFunctions.Show();
            //}
            //isExtraLevelEditorFunctionsOpen = !isExtraLevelEditorFunctionsOpen;
            //CheckDialogStatus();
        }
        private void initialObjectValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (isInitialObjectValuesEditor == true)
            //    initialObjectValuesEditor.Close();
            //else
            //{
            //    initialObjectValuesEditor = new InitialObjectValuesEditor();
            //    initialObjectValuesEditor.Show();
            //}
            //isInitialObjectValuesEditor = !isInitialObjectValuesEditor;
            //CheckDialogStatus();
        }
        private void dialogsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            guidelineEditorToolStripMenuItem.Enabled = canOpenGuidelineEditor;
            levelOverviewToolStripMenuItem.Enabled = canOpenLevelOverview;
            levelVersionConverterToolStripMenuItem.Enabled = canOpenLevelVersionConverter;
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openSettingsFile.ShowDialog();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettings(Options.StringDictionary["defaultSettingsPath"]);
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSettingsFile.ShowDialog();
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoActions(1);
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoActions(1);
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timesAboutDialogWasShown++;
            lennyToolStripMenuItem.Visible = timesAboutDialogWasShown >= 5;
            toolStripSeparator1.Visible = timesAboutDialogWasShown >= 5;
            if (About.IsOpen)
                AboutWindow.Focus();
            else
            {
                AboutWindow = new About();
                AboutWindow.Show();
                //isAboutOpen = true;
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void lennyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timesLennyDialogWasShown++;
            MessageBox.Show("( ͡° ͜ʖ ͡°) ( ͡° ͜ʖ ͡°) ( ͡° ͜ʖ ͡°) ( ͡° ͜ʖ ͡°) ( ͡° ͜ʖ ͡°) ( ͡° ͜ʖ ͡°) ( ͡° ͜ʖ ͡°) ( ͡° ͜ʖ ͡°) ( ͡° ͜ʖ ͡°) ( ͡° ͜ʖ ͡°)", "( ͡° ͜ʖ ͡°)", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadSettings("EffectSome/default.esf");
        }
        private void tutorialVideosPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/playlist?list=PLvXDgdu0WmA5DrSUWFLmcFEZbMHkB9m66");
        }
        private void webTutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://someeffect.altervista.org/howToUse.html");
        }
        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://someeffect.altervista.org/changelog.html");
        }
        private void absoluteGamerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("mailto:ben.stafford.03@icloud.com");
        }
        private void alFasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("mailto:kalfas.alex@gmail.com");
        }
        private void sendFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://someeffect.altervista.org/feedback.php");
        }
        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.patreon.com/EffectSome");
        }
        private void officialWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://someeffect.altervista.org");
        }
        private void levelVersionConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isGDOpen = CheckGDStatus();
            while (isGDOpen)
            {
                isGDOpen = CheckGDStatus();
                DialogResult result = MessageBox.Show("You need to close Geometry Dash first in order to save the changes to the levels.", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Cancel)
                    return;
            }
            if (!isGDOpen)
            {
                if (LevelVersionConverter.IsOpen)
                {
                    LevelVersionConverterWindow.Close();
                }
                else
                {
                    notification.Text = "Initializing Level Version Converter...";
                    LevelVersionConverterWindow = new LevelVersionConverter();
                    LevelVersionConverterWindow.Show();
                    notification.Text = "Ready";
                }
                CheckDialogStatus();
            }
        }
        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!updatedVersion)
                CheckForUpdates();
            else
                Process.Start(updateFilePath);
            // Very code
        }
        private void updateResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateResources.RunWorkerAsync();
        }
        private void globalParameterSettingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openGlobalParameterSettingsPresetFile.ShowDialog();
        }
        private void guidelineEditorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openGuidelineEditorPresetFile.ShowDialog();
        }
        private void consolePromptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConsolePrompt.Main(null);
        }
        private void designAssistantToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region Reset all parameters
        private void moveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox1);
            checkBox24.Checked = checkBox25.Checked = checkBox157.Checked = checkBox158.Checked = true;
            comboBox1.Text = "None";
        }
        private void toggleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox6);
        }
        private void spawnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox7);
        }
        private void pulseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox3);
            checkBox31.Checked = checkBox32.Checked = true;
            radioButton4.Checked = radioButton5.Checked = true;
        }
        private void alphaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox2);
        }
        private void textObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void objectsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox19);
        }
        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox23);
            checkBox61.Checked = checkBox95.Checked = true;
            comboBox44.Text = "None";
        }
        private void followToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox22);
        }
        private void shakeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox20);
        }
        private void animateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox21);
        }
        private void followPlayerYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox28);
        }
        private void touchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox24);
        }
        private void countToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox25);
        }
        private void instantCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox26);
            radioButton1.Checked = true;
        }
        private void pickupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox29);
        }
        private void collisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox27);
        }
        private void onDeathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox30);
        }
        private void colorTriggersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetParametersOfGroupBox(groupBox9);
        }
        private void rotatingObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion // work on that
        #region NumericUpDowns
        private void numericUpDown32_ValueChanged(object sender, EventArgs e) => rotationAdj = checkBox168.Checked ? (float)numericUpDown32.Value : 0;
        private void numericUpDown33_ValueChanged(object sender, EventArgs e) => scalingAdj = checkBox169.Checked ? (float)numericUpDown33.Value : 0;
        private void numericUpDown34_ValueChanged(object sender, EventArgs e) => val1Adj = checkBox170.Checked ? (float)numericUpDown34.Value : 0;
        private void numericUpDown35_ValueChanged(object sender, EventArgs e) => sat1Adj = checkBox171.Checked ? (float)numericUpDown35.Value : 0;
        private void numericUpDown36_ValueChanged(object sender, EventArgs e) => hue1Adj = checkBox10.Checked ? (float)numericUpDown36.Value : 0;
        private void numericUpDown37_ValueChanged(object sender, EventArgs e) => EL2Adj = checkBox172.Checked ? (int)numericUpDown37.Value : 0;
        private void numericUpDown38_ValueChanged(object sender, EventArgs e) => EL1Adj = checkBox172.Checked ? (int)numericUpDown38.Value : 0;
        private void numericUpDown39_ValueChanged(object sender, EventArgs e) => ZOrderAdj = checkBox9.Checked ? (int)numericUpDown39.Value : 0;
        private void numericUpDown40_ValueChanged(object sender, EventArgs e) => ZLayerAdj = checkBox18.Checked ? (int)numericUpDown40.Value : 0;
        private void numericUpDown41_ValueChanged(object sender, EventArgs e) => val2Adj = checkBox170.Checked ? (float)numericUpDown41.Value : 0;
        private void numericUpDown50_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown51_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown52_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown53_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown55_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown56_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown57_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown58_ValueChanged(object sender, EventArgs e) => sat2Adj = checkBox171.Checked ? (float)numericUpDown58.Value : 0;
        private void numericUpDown59_ValueChanged(object sender, EventArgs e) => hue2Adj = checkBox10.Checked ? (float)numericUpDown59.Value : 0;
        private void numericUpDown63_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown136_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown137_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown138_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown233_ValueChanged(object sender, EventArgs e)
        {

        }
        #endregion
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Enabled = listBox1.SelectedItems.Count > 0;
        }
        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (canRegisterActions)
                RegisterAction("Change Property");
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1)
            {
                if (e.ColumnIndex == 0)
                {
                    DataGridViewCheckBoxCell a = dataGridView1[0, e.RowIndex] as DataGridViewCheckBoxCell;
                    try
                    {
                        if (ToBoolean(a.Value)) selectedVariableIndices.Add(e.RowIndex);
                        else if (!ToBoolean(a.Value)) selectedVariableIndices.Remove(e.RowIndex);
                        button11.Enabled = selectedVariableIndices.Count > 0;
                    }
                    catch { }
                }
                if (e.ColumnIndex == 1)
                    customVariableNames[e.RowIndex] = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                if (e.ColumnIndex == 2)
                    try { customVariablesInitialValues[e.RowIndex] = ToDouble(dataGridView1[e.ColumnIndex, e.RowIndex].Value); dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Black; }
                    catch { dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red; }
                if (e.ColumnIndex == 3)
                    try { customVariablesAdjustments[e.RowIndex] = ToDouble(dataGridView1[e.ColumnIndex, e.RowIndex].Value); dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Black; }
                    catch { dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red; }
            }
        }
        private void openGlobalParameterSettingsPresetFile_FileOk(object sender, CancelEventArgs e)
        {
            foreach (string file in openGlobalParameterSettingsPresetFile.FileNames)
            {
                string fileName = file.Split('\\').Last().Split('.').ExcludeLast(1).Combine().RemoveLastNumber();
                string fileExtension = file.Split('\\').Last().Split('.').Last();
                if (fileExtension != "esf")
                    continue;
                string dest = appLocation + @"\EffectSome\Presets\Global Parameter Settings\" + fileName + "." + fileExtension;
                for (int i = 0; ; i++)
                {
                    if (i > 0)
                        dest = appLocation + @"\EffectSome\Presets\Global Parameter Settings\" + fileName + " " + i.ToString() + "." + fileExtension;
                    if (!File.Exists(dest))
                    {
                        File.Copy(file, dest);
                        break;
                    }
                }
            }
            notification.ShowBalloonTip(5000, "Success", "The Global Parameter Settings preset files were successfully added!", ToolTipIcon.Info);
        }
        private void openGuidelineEditorPresetFile_FileOk(object sender, CancelEventArgs e)
        {
            foreach (string file in openGuidelineEditorPresetFile.FileNames)
            {
                string fileName = file.Split('\\').Last().Split('.').ExcludeLast(1).Combine().RemoveLastNumber();
                bool containsHash = fileName.Last() == '#';
                if (containsHash)
                    fileName = fileName.Remove(fileName.Length - 1);
                while (fileName.Last() == ' ')
                    fileName = fileName.Remove(fileName.Length - 1);
                string fileExtension = file.Split('\\').Last().Split('.').Last();
                if (fileExtension != "esf")
                    continue;
                string dest = appLocation + @"\EffectSome\Presets\Guideline Editor\" + fileName + "." + fileExtension;
                for (int i = (containsHash ? 1 : 0); ; i++)
                {
                    if (i > 0)
                        dest = appLocation + @"\EffectSome\Presets\Guideline Editor\" + fileName + " " + (containsHash ? "#" : "") + i.ToString() + "." + fileExtension;
                    if (!File.Exists(dest))
                    {
                        File.Copy(file, dest);
                        break;
                    }
                }
            }
            notification.ShowBalloonTip(5000, "Success", "The Guideline Editor preset files were successfully added!", ToolTipIcon.Info);
        }
        private void openSettingsFile_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                LoadSettings(openSettingsFile.FileName);
            }
            catch (FormatException)
            {
                DialogResult result = MessageBox.Show("The default settings file is corrupted. Try again?", "Corrupted file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Retry)
                    openToolStripMenuItem_Click(sender, e);
            }
        }
        private void saveSettingsFile_FileOk(object sender, CancelEventArgs e)
        {
            if (saveSettingsFile.FileName == "default.esf")
            {
                MessageBox.Show("You can't save a settings file with that name. Please change the name and try again.", "Invalid name", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                saveSettingsFile.ShowDialog();
            }
            else
                SaveSettings(saveSettingsFile.FileName);
        }
        private void PropertyControlLoseFocus(object sender, EventArgs e)
        {
            if (canRegisterActions)
                RegisterAction("Change Property");
        }
        private void ChangePropertyEnabledStatusCheckedState(object sender, EventArgs e)
        {
            if (canRegisterActions)
                RegisterAction("Change " + (sender as CheckBox).Text.Replace(" by", "").Replace(" to", "") + " Property Enabled Status");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            button2.Enabled = radioButton14.Checked ? (button1.Enabled = button5.Enabled = canWriteCP) && listBox1.Items.Count > 0 : button1.Enabled = button5.Enabled = canWriteCP;
        }

        private void ToggleLevelOverview(object sender, EventArgs e)
        {
            isGDOpen = CheckGDStatus();
            while (isGDOpen)
            {
                isGDOpen = CheckGDStatus();
                DialogResult result = MessageBox.Show("You need to close Geometry Dash first in order to save the changes to the levels.", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Cancel)
                    return;
            }
            if (!isGDOpen)
            {
                if (LevelOverview.IsOpen)
                    LevelOverviewWindow.Close();
                else
                {
                    notification.Text = "Initializing Level Overiew...";
                    LevelOverviewWindow = new LevelOverview();
                    LevelOverviewWindow.Show();
                    notification.Text = "Ready";
                }
                CheckDialogStatus();
            }
        }
        private void ToggleGuidelineEditor(object sender, EventArgs e)
        {
            isGDOpen = CheckGDStatus();
            while (isGDOpen)
            {
                isGDOpen = CheckGDStatus();
                DialogResult result = MessageBox.Show("You need to close Geometry Dash first in order to save the changes to the levels.", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Cancel)
                    return;
            }
            if (!isGDOpen)
            {
                if (GuidelineEditor.IsOpen)
                    GuidelineEditorWindow.Close();
                else
                {
                    notification.Text = "Loading Guideline Editor Presets...";
                    GuidelineEditorWindow = new GuidelineEditor();
                    GuidelineEditorWindow.Show();
                    notification.Text = "Ready";
                }
                CheckDialogStatus();
            }
        }
        private void Notification_Click(object sender, EventArgs e)
        {
            notification.ContextMenu.MenuItems[0].Enabled = canOpenGuidelineEditor; // Guideline Editor
            notification.ContextMenu.MenuItems[1].Enabled = canOpenLevelOverview; // Level Overview
        }
        
        #region Undo/Redo stuff
        // Probably recode this and also implement a custom class or two I guess
        void RegisterAction(string actionDesc)
        {
            CheckBox[] demChecks = GetCheckBoxes();
            NumericUpDown[] demNUDs = GetNUDs();
            RadioButton[] demRadios = GetRadioButtons();
            ComboBox[] demCombos = GetComboBoxes();
            TextBox[] demTexts = GetTextBoxes();

            StringBuilder temp = new StringBuilder();

            for (int i = 0; i < demChecks.Length; i++)
                temp = temp.Append(demChecks[i].Checked.ToString() + "|");
            for (int i = 0; i < demNUDs.Length; i++)
                temp = temp.Append(demNUDs[i].Value.ToString() + "|");
            for (int i = 0; i < demRadios.Length; i++)
                temp = temp.Append(demRadios[i].Checked.ToString() + "|");
            for (int i = 0; i < demCombos.Length; i++)
                temp = temp.Append(demCombos[i].Text + "|");
            for (int i = 0; i < demTexts.Length; i++)
                temp = temp.Append(demTexts[i].Text + "|");
            for (int i = 0; i < customVariableNames.Count; i++)
                temp = temp.Append(customVariableNames + ";" + customVariablesAdjustments + ";" + customVariablesInitialValues + "|");
            temp = temp.Append(listBox1.Items.ToInt32Array().ToStringArray().Combine(":"));
            if (undo.Count > Options.IntDictionary["maxUndoRedoActions"])
                undo.RemoveAt(0);
            undo.Add(temp.ToString());
            undoActionDescs.Add(actionDesc);
            redo.Clear();
            redoActionDescs.Clear();
            AddUndoAction(actionDesc);
            redoToolStripMenuItem.DropDownItems.Clear();
            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = false;
        }
        void RegisterStartingValues()
        {
            CheckBox[] demChecks = GetCheckBoxes();
            NumericUpDown[] demNUDs = GetNUDs();
            RadioButton[] demRadios = GetRadioButtons();
            ComboBox[] demCombos = GetComboBoxes();
            TextBox[] demTexts = GetTextBoxes();

            StringBuilder temp = new StringBuilder();

            for (int i = 0; i < demChecks.Length; i++)
                temp = temp.Append(demChecks[i].Checked.ToString() + "|");
            for (int i = 0; i < demNUDs.Length; i++)
                temp = temp.Append(demNUDs[i].Value.ToString() + "|");
            for (int i = 0; i < demRadios.Length; i++)
                temp = temp.Append(demRadios[i].Checked.ToString() + "|");
            for (int i = 0; i < demCombos.Length; i++)
                temp = temp.Append(demCombos[i].Text + "|");
            for (int i = 0; i < demTexts.Length; i++)
                temp = temp.Append(demTexts[i].Text + "|");
            for (int i = 0; i < customVariableNames.Count; i++)
                temp = temp.Append(customVariableNames + ";" + customVariablesAdjustments + ";" + customVariablesInitialValues + ":");
            temp = temp.Append("|" + listBox1.Items.ToInt32Array().ToStringArray().Combine(":"));
            initialValues = temp.ToString();
        }
        void UndoActions(int actions)
        {
            canRegisterActions = false;
            bool foundObjects = false, foundTriggers = false, foundSpecialObjects = false;
            for (int i = undo.Count - actions; i < undo.Count; i++)
            {
                if (!foundObjects)
                    if (undoActionDescs[i] == "Inject Objects Copy-Paste Automation")
                    {
                        SetParameters(undo[i]);
                        WriteObjectsCopyPasteAutomation();
                        foundObjects = true;
                        continue;
                    }
                if (!foundTriggers)
                    if (undoActionDescs[i] == "Inject Triggers Copy-Paste Automation")
                    {
                        SetParameters(undo[i]);
                        WriteTriggersCopyPasteAutomation();
                        foundTriggers = true;
                        continue;
                    }
                if (!foundSpecialObjects)
                    if (undoActionDescs[i] == "Inject Special Objects Copy-Paste Automation")
                    {
                        SetParameters(undo[i]);
                        WriteSpecialObjectsCopyPasteAutomation();
                        foundSpecialObjects = true;
                        continue;
                    }
            }
            if (actions < undo.Count)
                SetParameters(undo[undo.Count - actions - 1]);
            else
                SetParameters(initialValues);
            // Transfer the undone actions to the redo list
            for (int i = 0; i < actions; i++)
            {
                redo.Add(undo[undo.Count - 1]);
                redoActionDescs.Add(undoActionDescs[undoActionDescs.Count - 1]);
                undo.RemoveAt(undo.Count - 1);
                undoActionDescs.RemoveAt(undoActionDescs.Count - 1);
            }
            MoveActionsFromUndoToRedo(actions);
            canRegisterActions = true;
        }
        void RedoActions(int actions)
        {
            canRegisterActions = false;
            if (actions > redo.Count)
                actions = redo.Count;
            bool foundObjects = false, foundTriggers = false, foundSpecialObjects = false;
            for (int i = actions - 1; i >= 0; i--)
            {
                if (!foundObjects)
                    if (redoActionDescs[i] == "Inject Objects Copy-Paste Automation")
                    {
                        SetParameters(redo[i]);
                        WriteObjectsCopyPasteAutomation();
                        foundObjects = true;
                        continue;
                    }
                if (!foundTriggers)
                    if (redoActionDescs[i] == "Inject Triggers Copy-Paste Automation")
                    {
                        SetParameters(redo[i]);
                        WriteTriggersCopyPasteAutomation();
                        foundTriggers = true;
                        continue;
                    }
                if (!foundSpecialObjects)
                    if (redoActionDescs[i] == "Inject Special Objects Copy-Paste Automation")
                    {
                        SetParameters(redo[i]);
                        WriteSpecialObjectsCopyPasteAutomation();
                        foundSpecialObjects = true;
                        continue;
                    }
            }
            SetParameters(redo[redo.Count - actions]);
            // Transfer the redone actions to the undo list
            for (int i = 0; i < actions; i++)
            {
                undo.Add(redo[redo.Count - 1]);
                undoActionDescs.Add(redoActionDescs[redoActionDescs.Count - 1]);
                redo.RemoveAt(redo.Count - 1);
                redoActionDescs.RemoveAt(redoActionDescs.Count - 1);
            }
            MoveActionsFromRedoToUndo(actions);
            canRegisterActions = true;
        }
        void SetParameters(string parameters)
        {
            canRegisterActions = false;
            CheckBox[] demChecks = GetCheckBoxes();
            NumericUpDown[] demNUDs = GetNUDs();
            RadioButton[] demRadios = GetRadioButtons();
            ComboBox[] demCombos = GetComboBoxes();
            TextBox[] demTexts = GetTextBoxes();

            string[] splittedUndoData = parameters.Split('|');
            string[,] splittedVariables = splittedUndoData[demChecks.Length + demNUDs.Length + demRadios.Length + demCombos.Length + demTexts.Length].Split(':').Split(';');
            int[] splittedObjectIDs = splittedUndoData[splittedUndoData.Length - 1].Split(':').RemoveEmptyElements().ToInt32Array();

            for (int i = 0; i < demChecks.Length; i++)
                demChecks[i].Checked = bool.Parse(splittedUndoData[i]);
            for (int i = 0; i < demNUDs.Length; i++)
                demNUDs[i].Value = decimal.Parse(splittedUndoData[i + demChecks.Length]);
            for (int i = 0; i < demRadios.Length; i++)
                demRadios[i].Checked = bool.Parse(splittedUndoData[i + demChecks.Length + demNUDs.Length]);
            for (int i = 0; i < demCombos.Length; i++)
                demCombos[i].Text = splittedUndoData[i + demChecks.Length + demNUDs.Length + demRadios.Length];
            for (int i = 0; i < demTexts.Length; i++)
                demTexts[i].Text = splittedUndoData[i + demChecks.Length + demNUDs.Length + demRadios.Length + demCombos.Length];
            customVariableNames.Clear();
            customVariablesAdjustments.Clear();
            customVariablesInitialValues.Clear();
            if (splittedVariables.GetLength(1) > 1)
                for (int i = 0; i < splittedVariables.GetLength(0); i++)
                {
                    customVariableNames.Add(splittedVariables[i, 0]);
                    customVariablesAdjustments.Add(ToInt32(splittedVariables[i, 1]));
                    customVariablesInitialValues.Add(ToInt32(splittedVariables[i, 2]));
                }
            ShowCustomVariables();
            for (int i = 0; i < splittedObjectIDs.Length; i++)
                listBox1.Items.Add(splittedObjectIDs[i]);
            canRegisterActions = true;
        }
        void AddUndoAction(string actionDesc)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(actionDesc);
            undoToolStripMenuItem.DropDownItems.Insert(0, item);
            undoToolStripMenuItem.DropDownItems[0].Click += ClickOnUndoHistoryAction;
            undoToolStripMenuItem.Enabled = true;
        }
        void MoveActionsFromUndoToRedo(int actions)
        {
            if (actions > undoToolStripMenuItem.DropDownItems.Count)
                actions = undoToolStripMenuItem.DropDownItems.Count;
            for (int i = 0; i < actions; i++)
            {
                ToolStripMenuItem item = (ToolStripMenuItem)undoToolStripMenuItem.DropDownItems[0];
                redoToolStripMenuItem.DropDownItems.Insert(0, item);
                redoToolStripMenuItem.DropDownItems[0].Click -= ClickOnUndoHistoryAction;
                redoToolStripMenuItem.DropDownItems[0].Click += ClickOnRedoHistoryAction;
            }
            undoToolStripMenuItem.Enabled = undoToolStripMenuItem.DropDownItems.Count > 0;
            redoToolStripMenuItem.Enabled = true;
        }
        void MoveActionsFromRedoToUndo(int actions)
        {
            if (actions > redoToolStripMenuItem.DropDownItems.Count)
                actions = redoToolStripMenuItem.DropDownItems.Count;
            for (int i = 0; i < actions; i++)
            {
                ToolStripMenuItem item = (ToolStripMenuItem)redoToolStripMenuItem.DropDownItems[0];
                undoToolStripMenuItem.DropDownItems.Insert(0, item);
                undoToolStripMenuItem.DropDownItems[0].Click -= ClickOnRedoHistoryAction;
                undoToolStripMenuItem.DropDownItems[0].Click += ClickOnUndoHistoryAction;
            }
            redoToolStripMenuItem.Enabled = redoToolStripMenuItem.DropDownItems.Count > 0;
            undoToolStripMenuItem.Enabled = true;
        }
        void ClickOnUndoHistoryAction(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            UndoActions((item.OwnerItem as ToolStripMenuItem).DropDownItems.IndexOf(item) + 1);
        }
        void ClickOnRedoHistoryAction(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            RedoActions((item.OwnerItem as ToolStripMenuItem).DropDownItems.IndexOf(item) + 1);
        }
        #endregion
        /// <summary>Deletes the entire <see cref="Directory"/> and all of its contents.</summary><param name="dir">The name of the <see cref="Directory"/>.</param>
        public static void DeleteEntireDirectory(string dir)
        {
            string[] directories = GetDirectories(dir);
            string[] files = GetFiles(dir);
            for (int i = 0; i < files.Length; i++)
                File.Delete(files[i]);
            foreach (string directory in directories)
                DeleteEntireDirectory(directory);
            Directory.Delete(dir);
        }
        public static void SetupWebObject(ref WebClient w)
        {
            w = new WebClient();
            w.Credentials = CredentialCache.DefaultNetworkCredentials;
            if (w.Headers[HttpRequestHeader.UserAgent] != null)
                w.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.47 Safari/537.36";
            if (w.Headers[HttpRequestHeader.ContentType] != null)
                w.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        }
        void AddItem(ListBox listBox, int item)
        {
            if (item != 0)
            {
                if (!listBox.Items.Contains(item))
                {
                    if (listBox.Items.Count != 0)
                    {
                        if (item < (int)listBox.Items[listBox.Items.Count - 1])
                        {
                            for (int i = 0; i < listBox.Items.Count; i++)
                                if (item < (int)listBox.Items[i])
                                {
                                    listBox.Items.Insert(i, item);
                                    break;
                                }
                        }
                        else
                            listBox.Items.Add(item);
                    }
                    else
                        listBox.Items.Add(item);
                }
            }
        }
        List<int> AddItem(List<int> list, int item)
        {
            if (item != 0)
            {
                if (!list.Contains(item))
                {
                    if (list.Count != 0)
                    {
                        if (item < list[list.Count - 1])
                        {
                            for (int i = 0; i < list.Count; i++)
                                if (item < list[i])
                                {
                                    list.Insert(i, item);
                                    break;
                                }
                        }
                        else
                            list.Add(item);
                    }
                    else
                        list.Add(item);
                }
            }
            return list;
        }
        void RemoveItems(ListBox listBox)
        {
            while (listBox.SelectedItems.Count != 0)
                listBox.Items.Remove(listBox.SelectedItems[listBox.SelectedItems.Count - 1]);
        }
        List<int> RemoveItems(List<int> list, int[] toRemove)
        {
            for (int i = 0; i < toRemove.Length; i++)
                list.Remove(toRemove[i]);
            return list;
        }
        void SaveSettings(string filePath)
        {
            CheckBox[] demChecks = GetCheckBoxes();
            NumericUpDown[] demNUDs = GetNUDs();
            RadioButton[] demRadios = GetRadioButtons();
            ComboBox[] demCombos = GetComboBoxes();
            TextBox[] demTexts = GetTextBoxes();

            string[] fileData = new string[5];

            for (int i = 0; i < demChecks.Length; i++)
                fileData[0] += ((int)(demChecks[i].CheckState)).ToString() + "|";
            for (int i = 0; i < demNUDs.Length; i++)
                fileData[1] += demNUDs[i].Value.ToString() + "|";
            for (int i = 0; i < demRadios.Length; i++)
                fileData[2] += demRadios[i].Checked.ToString() + "|";
            for (int i = 0; i < demCombos.Length; i++)
                fileData[3] += demCombos[i].Text + "|";
            for (int i = 0; i < demTexts.Length; i++)
                fileData[4] += demTexts[i].Text + "|";
            //for (int i = 0; i < demChecks.Length; i++)
            //    fileData[5] += demChecks[i].Enabled.ToString() + "|";
            //for (int i = 0; i < demNUDs.Length; i++)
            //    fileData[6] += demNUDs[i].Enabled.ToString() + "|";
            //for (int i = 0; i < demRadios.Length; i++)
            //    fileData[7] += demRadios[i].Enabled.ToString() + "|";
            //for (int i = 0; i < demCombos.Length; i++)
            //    fileData[8] += demCombos[i].Enabled.ToString() + "|";
            for (int i = 0; i < fileData.Length; i++)
                fileData[i] = fileData[i].Remove(fileData[i].Length - 1);

            WriteAllLines(filePath, fileData);
        }
        void LoadSettings(string filePath)
        {
            CheckBox[] demChecks = GetCheckBoxes();
            NumericUpDown[] demNUDs = GetNUDs();
            RadioButton[] demRadios = GetRadioButtons();
            ComboBox[] demCombos = GetComboBoxes();
            TextBox[] demTexts = GetTextBoxes();

            string[] rawOptions = ReadAllLines(filePath);
            string[] checks = rawOptions[0].Split('|');
            string[] NUDs = rawOptions[1].Split('|');
            string[] radios = rawOptions[2].Split('|');
            string[] combos = rawOptions[3].Split('|');
            string[] texts = rawOptions[4].Split('|');
            //string[] checksEnabled = rawOptions[5].Split('|');
            //string[] NUDsEnabled = rawOptions[6].Split('|');
            //string[] radiosEnabled = rawOptions[7].Split('|');
            //string[] combosEnabled = rawOptions[8].Split('|');

            for (int i = 0; i < checks.Length; i++)
                demChecks[i].CheckState = (CheckState)ToInt32(checks[i]);
            for (int i = 0; i < NUDs.Length; i++)
                demNUDs[i].Value = ToDecimal(NUDs[i]);
            for (int i = 0; i < radios.Length; i++)
                demRadios[i].Checked = ToBoolean(radios[i]);
            for (int i = 0; i < combos.Length; i++)
                demCombos[i].Text = combos[i];
            for (int i = 0; i < texts.Length; i++)
                demTexts[i].Text = texts[i];
            //for (int i = 0; i < checksEnabled.Length; i++)
            //    demChecks[i].Enabled = ToBoolean(checksEnabled[i]);
            //for (int i = 0; i < NUDsEnabled.Length; i++)
            //    demNUDs[i].Enabled = ToBoolean(NUDsEnabled[i]);
            //for (int i = 0; i < radiosEnabled.Length; i++)
            //    demRadios[i].Enabled = ToBoolean(radiosEnabled[i]);
            //for (int i = 0; i < combosEnabled.Length; i++)
            //    demCombos[i].Enabled = ToBoolean(combosEnabled[i]);
        }
        void FixStrings()
        {
            ChangeString("Guidelines can be used to make syncronization of music and gameplay easier. Create your own custom song guidelines using this interface. Press on record to start recording, then tap on the screen to create lines. Press on the stop button to stop recording and save your created guidelines.", 0x5CB850);
        }
        void ChangeString(string newString, int address)
        {
            for (int i = 0; i < newString.Length; i++)
                WriteMemory(address + i, BitConverter.GetBytes(newString[i]), (int)processHandle);
        }
        void InjectCopyPasteDLL()
        {
            string dllName = GDLocalData + @"\Auto_CP.dll"; // Apparently this is the correct path that needs to be referred to within the game
            IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            IntPtr allocMemAddress = VirtualAllocEx(processHandle, IntPtr.Zero, 0x1000, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
            VirtualProtectEx(processHandle, IntPtr.Zero, (UIntPtr)0x1000, PAGE_READWRITE, out uint shit);
            WriteProcessMemory(processHandle, allocMemAddress, Encoding.Default.GetBytes(dllName), 260, out UIntPtr bytesWritten);
            CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
        }
        
        void DetectDLLStatus()
        {
            string[] DLLNames = { /*, "Bypasses.dll"*/ };
            int[] statuses = new int[DLLNames.Length];
            string msg = "Something is wrong with the program's libraries. Details are listed below:\n\n";
            int corruptedDLLs = 0, inexistentDLLs = 0;

            //try { CallAllMethodsInClass(typeof(DLLs.LimitBypasses)); }
            //catch (TargetInvocationException ex) when (ex.InnerException.GetType() == typeof(BadImageFormatException)) { statuses[1] = 1; limitBypassesToolStripMenuItem.Enabled = false; }
            //catch (TargetInvocationException ex) when (ex.InnerException.GetType() == typeof(DllNotFoundException)) { statuses[1] = 2; limitBypassesToolStripMenuItem.Enabled = false; }

            for (int i = 0; i < statuses.Length; i++)
            {
                if (statuses[i] == 1)
                    corruptedDLLs++;
                else if (statuses[i] == 2)
                    inexistentDLLs++;
            }

            if (corruptedDLLs > 0)
            {
                msg += "Corrupted DLLs:\n";
                for (int i = 0; i < statuses.Length; i++)
                    if (statuses[i] == 1)
                        msg += "- " + DLLNames[i] + "\n";
                msg += "\n";
            }
            if (inexistentDLLs > 0)
            {
                msg += "Inexistent DLLs:\n";
                for (int i = 0; i < statuses.Length; i++)
                    if (statuses[i] == 2)
                        msg += "- " + DLLNames[i] + "\n";
            }
            while (msg[msg.Length - 1] == '\n')
                msg = msg.Remove(msg.Length - 1);
            if (Options.BoolDictionary["DLLErrorWarnings"])
                if (corruptedDLLs > 0 && inexistentDLLs > 0)
                    MessageBox.Show(msg, "DLL Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }
        
        void CallAllMethodsInClass(Type T)
        {
            var methods = T.GetMethods();
            foreach (MethodInfo m in methods)
            {
                var pars = m.GetParameters();
                Type[] parameterTypes = new Type[pars.Length];
                List<object> parameters = new List<object>();
                //object[,] parameters = new object[pars.Length, 100];
                for (int i = 0; i < pars.Length; i++)
                    parameterTypes[i] = pars[i].ParameterType;
                for (int i = 0; i < pars.Length; i++)
                {
                    if (parameterTypes[i] == typeof(double))
                        parameters.Add(0d);
                    if (parameterTypes[i] == typeof(float))
                        parameters.Add(0f);
                    else if (parameterTypes[i] == typeof(int))
                        parameters.Add(0);
                    else if (parameterTypes[i] == typeof(bool))
                        parameters.Add(false);
                    else if (parameterTypes[i] == typeof(string))
                        parameters.Add("");
                    else if (parameterTypes[i] == typeof(double[]))
                        parameters.Add(new double[100]);
                    else if (parameterTypes[i] == typeof(float[]))
                        parameters.Add(new float[100]);
                    else if (parameterTypes[i] == typeof(int[]))
                        parameters.Add(new int[100]);
                    else if (parameterTypes[i] == typeof(bool[]))
                        parameters.Add(new bool[100]);
                    else
                        continue;
                }
                if (parameters.Count != 0)
                    m.Invoke(new object(), parameters.ToArray());
            }
        }
        
        void CheckDialogStatus()
        {
            notification.ContextMenu.MenuItems[0].Checked = GuidelineEditor.IsOpen; // Guideline Editor
            notification.ContextMenu.MenuItems[1].Checked = LevelOverview.IsOpen; // Level Overview
            guidelineEditorToolStripMenuItem.Checked = GuidelineEditor.IsOpen;
            globalParameterSettingsToolStripMenuItem.Checked = GlobalParameterSettings.IsOpen; // DO NOT YOU DARE REMOVE THE CLASS NAME BEFORE THE PROPERTY OR ELSE
            levelOverviewToolStripMenuItem.Checked = LevelOverview.IsOpen;
            customObjectEditorToolStripMenuItem.Checked = CustomObjectEditor.IsOpen;
            levelVersionConverterToolStripMenuItem.Checked = LevelVersionConverter.IsOpen;
        }
        void DeclareBGWs()
        {
            decryptGamesaveAndUploadInfo.DoWork += new DoWorkEventHandler(DecryptGamesaveAndUploadInfo);
            getLevelData.DoWork += new DoWorkEventHandler(GetLevelData);
            writeCP.DoWork += new DoWorkEventHandler(WriteCPAutomation);
            updateResources.DoWork += new DoWorkEventHandler(UpdateResources);
            decryptGamesaveAndUploadInfo.WorkerSupportsCancellation = getLevelData.WorkerSupportsCancellation = updateResources.WorkerSupportsCancellation = true;
        }
        void RunBGWs()
        {
            decryptGamesaveAndUploadInfo.RunWorkerAsync();
            getLevelData.RunWorkerAsync();
            updateResources.RunWorkerAsync();
        }
        void GetBPMValuesFromFile()
        {
            CustomSongBPMs = new Dictionary<int, double>();
            string[] BPMList = ReadAllLines("EffectSome/Resources/BPMList.esf");
            for (int i = 0; i < BPMList.Length; i++)
            {
                string[] split = BPMList[i].Split(": ");
                CustomSongBPMs.Add(ToInt32(split[0]), ToDouble(split[1]));
            }
        }
        void InitializeNotificationIcon()
        {
            notification = new NotifyIcon();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(EffectSome));
            notification.BalloonTipIcon = ToolTipIcon.Info;
            notification.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            notification.Text = "Ready";
            notification.Visible = true;
            notification.ContextMenu = new ContextMenu();
            notification.Click += Notification_Click;
            notification.ContextMenu.MenuItems.Add(new MenuItem("Guideline Editor", ToggleGuidelineEditor));
            notification.ContextMenu.MenuItems[0].Enabled = false;
            notification.ContextMenu.MenuItems.Add(new MenuItem("Level Overview", ToggleLevelOverview));
            notification.ContextMenu.MenuItems[1].Enabled = false;
        }

        void CreateFolders()
        {
            CreateDirectory("EffectSome");
            CreateDirectory("EffectSome/Guidelines");
            CreateDirectory("EffectSome/Presets");
            CreateDirectory("EffectSome/Presets/Guideline Editor");
            CreateDirectory("EffectSome/Presets/Global Parameter Settings");
            CreateDirectory(GDLocalData + "/tmp");
            CreateDirectory(GDLocalData + "/tmp/cpa");
        }
        void SetupWebObjects()
        {
            SetupWebObject(ref songIDUploader);
            SetupWebObject(ref userInfoUploader);
            SetupWebObject(ref updateDownloader);
            SetupWebObject(ref resourceDownloader);
        }
        void ResetParametersOfGroupBox(GroupBox gb)
        {
            foreach (CheckBox CB in gb.Controls.OfType<CheckBox>())
                CB.Checked = false;
            foreach (ComboBox CB in gb.Controls.OfType<ComboBox>())
                CB.Text = "";
            foreach (NumericUpDown NUD in gb.Controls.OfType<NumericUpDown>())
                NUD.Value = 0;
        }
        void ReadPresets()
        {
            ComboBox[] presetCBs = GetPresetComboBoxes();
            ToolStripItemCollection items = applyTheSamePresetToolStripMenuItem.DropDownItems;
            for (int i = 0; i < presetCBs.Length; i++)
                presetCBs[i].Items.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                ToolStripMenuItem item = items[i] as ToolStripMenuItem;
                item.DropDownItems.Clear();
            }
            foreach (string presetPath in GetDirectories("EffectSome/Presets/Global Parameter Settings/").Replace('/', '\\'))
            {
                string preset = presetPath.Split('\\').Last();
                foreach (ComboBox CB in presetCBs)
                {
                    CB.Items.Add(preset);
                    CB.SelectedIndex = 0;
                }
                for (int i = 0; i < items.Count; i++)
                {
                    ToolStripMenuItem item = items[i] as ToolStripMenuItem;
                    item.DropDownItems.Add(preset);
                    (item.DropDownItems[item.DropDownItems.Count - 1] as ToolStripMenuItem).Click += ApplySamePresets;
                }
            }
        }
        void ApplySamePresets(object sender, EventArgs e)
        {

        }
        void ShowCustomVariables()
        {
            dataGridView1.Rows.Clear();
            if (customVariableNames.Count > 0)
            {
                dataGridView1.Rows.Add(customVariableNames.Count);
                for (int i = 0; i < customVariableNames.Count; i++)
                {
                    dataGridView1[1, i].Value = customVariableNames[i];
                    dataGridView1[2, i].Value = customVariablesInitialValues[i];
                    dataGridView1[3, i].Value = customVariablesAdjustments[i];
                }
            }
        }
        unsafe bool CheckGDStatus()
        {
            process = Process.GetProcessesByName("GeometryDash").FirstOrDefault();
            if (process != null)
            {
                processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);
                IntPtr processID = new IntPtr(process.Id);
                IntPtr[] processes = new IntPtr[0x1000];
                GCHandle handle = GCHandle.Alloc(processes, GCHandleType.Pinned);
                IntPtr ptr = handle.AddrOfPinnedObject();
                int moduleHandle = 0;
                int val = EnumProcessModules(processHandle, ptr, 0x1000, out uint needed);
                handle.Free();
                if (val > 0)
                {
                    fixed (char* n = new char[260])
                    {
                        // WHO THE FUCK DESIGNED THAT LANGUAGE?
                        GetModuleBaseName(processHandle, new IntPtr(0), n, 260);
                        string name = new string(n);
                        for (int i = 0; i < needed / 4; i++)
                        {
                            fixed (char* m = new char[260])
                            {
                                GetModuleBaseName(processHandle, processes[i], m, 260);
                                string modName = new string(m);
                                if (modName == name)
                                {
                                    moduleHandle = (int)processes[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                baseAddress = 0x3222D0 + moduleHandle;
            }
            return tabControl2.Enabled = process != null;
        }

        void DecryptGamesaveAndUploadInfo(object sender, DoWorkEventArgs e)
        {
            DateTime start;
            TimeSpan span;
            start = DateTime.Now;
            TryDecryptGamesave(out DecryptedGamesave);
            span = DateTime.Now - start;
            if (Options.BoolDictionary["gamesaveDecryptedNotifications"])
                notification.ShowBalloonTip(5000, "Gamesave Successfully Decrypted", $"The Gamesave file was successfully decrypted{(Options.BoolDictionary["showOperationTimes"] ? $" in {span.TotalSeconds:N3}s" : "")}.", ToolTipIcon.Info);
            GetCustomObjects();
            try
            {
                UserName = GetPlayerName();
                string userID = GetUserID();
                string accountID = GetAccountID();
                string uploadInfo = "&userName=" + UserName + "&userID=" + userID + "&accountID=" + accountID;
                userInfoUploader.UploadString("http://someeffect.altervista.org/uploadUserInfo.php", uploadInfo);
            }
            catch { }
            try
            {
                var songIDs = CustomSongBPMs.Keys;
                StringBuilder uploadData = new StringBuilder("sID=");
                for (int i = 0; i < songIDs.Count; i++)
                    uploadData.Append(songIDs.ElementAt(i) + "|");
                uploadData = uploadData.Remove(uploadData.Length - 1, 1);
                string sID = uploadData.ToString();
                string response = songIDUploader.UploadString("http://someeffect.altervista.org/uploadSongIDs.php", sID);
                if (response == "1")
                    notification.ShowBalloonTip(5000, "Successful Song ID Registration", "The song IDs that are downloaded have been successfully registered.", ToolTipIcon.Info);
            }
            catch { notification.ShowBalloonTip(5000, "Connection Failed", "An error has occured in an attempt to connect to the server.", ToolTipIcon.Error); }
            FolderNames = GetFolderNames();
        }
        void GetLevelData(object sender, DoWorkEventArgs e)
        {
            DateTime start;
            TimeSpan span;
            if (Options.BoolDictionary["decryptLevelData"])
            {
                notification.Text = "Gathering Level Data...";
                start = DateTime.Now;
                TryDecryptLevelData(out DecryptedLevelData);
                span = DateTime.Now - start;
                if (Options.BoolDictionary["levelDataDecryptedNotifications"])
                    notification.ShowBalloonTip(5000, "Level Data Successfully Decrypted", $"The level data file was successfully decrypted{(Options.BoolDictionary["showOperationTimes"] ? $" in {span.TotalSeconds:N3}s" : "")}.", ToolTipIcon.Info);
                GetKeyIndices();
                GetLevels();
                try
                {
                    start = DateTime.Now;
                    GetAllLevelInfo();
                    span = DateTime.Now - start;
                    if (Options.BoolDictionary["levelInfoRetrieved"])
                        notification.ShowBalloonTip(5000, "Level Info Retrieved", $"The level info has been successfully retrieved{(Options.BoolDictionary["showOperationTimes"] ? $" in {span.TotalSeconds:N3}s" : "")}.", ToolTipIcon.Info);
                    canOpenGuidelineEditor = UserLevelCount > 0;
                    if (Options.BoolDictionary["decryptLevelStrings"])
                    {
                        start = DateTime.Now;
                        int emptyLevels = SetLevelStringsForEmptyLevels();
                        span = DateTime.Now - start;
                        if (Options.BoolDictionary["emptyLevelsLevelStringsSetNotifications"])
                            if (emptyLevels > 0)
                                notification.ShowBalloonTip(5000, "Empty Level Strings Successfully Applied", $"{emptyLevels} empty levels' Level Strings were set to the default Level String{(Options.BoolDictionary["showOperationTimes"] ? $" in {span.TotalSeconds:N3}s" : "")}.", ToolTipIcon.Info);
                    }
                    canOpenLevelOverview = true;
                    canOpenLevelVersionConverter = true;
                    isLevelDataValid = true;
                }
                catch (Exception ex)
                {
                    isLevelDataValid = UserLevelCount == 0;
                    canOpenLevelOverview = isLevelDataValid;
                    if (isLevelDataValid)
                        notification.ShowBalloonTip(5000, "No Levels Detected", "The program did not detect any levels in your game.", ToolTipIcon.Warning);
                    else
                    {
                        notification.Text = "Corrupted Level Data File";
                        WriteAllText("exception.txt", ex.ToString());
                        MessageBox.Show("The program detected that your level data file is corrupted. Do NOT open Geometry Dash if you have important levels saved. It is always suggested to backup your save and levels, whether this is a copy of the files in your computer, or in the cloud on your account.\n\nTo fix this you may ask an expert by giving them your levels file named as \"CCLocalLevels.dat\" located under your songs folder. Otherwise, if you have some knowledge of the gamesave file, you may take a look at it and edit it accordingly.\n\nIf you think this is a false positive, ensure that you copy your CCLocalLevels.dat file to another safe place before opening Geometry Dash. If it is indeed corrupted, the original gamesave file should be kept in its safe place.\n\nThe program will now close.", "Corrupted Level Data", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        notification.Text = "Terminating Process...";
                        ThreadSafeImplementations.CloseForm(this);
                    }
                }
                successfullyRetrievedLevelData = true;
            }
        }
        void WriteCPAutomation(object sender, DoWorkEventArgs e)
        {
            canWriteCP = false;
            notification.Text = "Writing Settings...";
            if ((int)e.Argument == 0) // Write Objects copy-paste automation settings
                WriteObjectsCopyPasteAutomation();
            else if ((int)e.Argument == 1) // Write Triggers copy-paste automation settings
                WriteTriggersCopyPasteAutomation();
            else if ((int)e.Argument == 2) // Write Special Objects copy-paste automation settings
                WriteSpecialObjectsCopyPasteAutomation();
            InjectCopyPasteDLL();
            notification.Text = "Ready";
            notification.ShowBalloonTip(5000, "Settings Written Successfully", "The settings are written successfully!", ToolTipIcon.Info);
            canWriteCP = true;
        }
        void UpdateResources(object sender, DoWorkEventArgs e)
        {
            if (Options.BoolDictionary["checkForResourceUpdatesOnStartup"])
                try
                {
                    HttpWebRequest checkForBPMFile = WebRequest.CreateHttp("http://someeffect.altervista.org/getNewBPMFileSize.php");
                    int newBPMFileSize;
                    int BPMFileSize = ReadAllBytes("EffectSome/Resources/BPMList.esf").Length;
                    using (WebResponse resp = checkForBPMFile.GetResponse())
                        newBPMFileSize = ToInt32(new StreamReader(resp.GetResponseStream()).ReadToEnd());
                    if (newBPMFileSize > BPMFileSize)
                        DownloadNewBPMFile();
                    else
                        notification.ShowBalloonTip(5000, "Up To Date", "The local resource files are up to date!", ToolTipIcon.Info);
                }
                catch { notification.ShowBalloonTip(5000, "Connection Error", "Unable to connect to the server. Please try again later.", ToolTipIcon.Error); }
        }
        
        CheckBox[] GetCheckBoxes() =>
            new CheckBox[]
            {
                checkBox1, checkBox2, checkBox3, checkBox4, checkBox5, checkBox6, checkBox7, checkBox8, checkBox9,
                checkBox10, checkBox11, checkBox12, checkBox13, checkBox14, checkBox15, checkBox16, checkBox17, checkBox18, checkBox19,
                checkBox20, checkBox21, checkBox22, checkBox23, checkBox24, checkBox25, checkBox26, checkBox27, checkBox28, checkBox29,
                checkBox30, checkBox31, checkBox32, checkBox33, checkBox34, checkBox35, checkBox36, checkBox37, checkBox38, checkBox39,
                checkBox40, checkBox41, checkBox42, checkBox43, checkBox44, checkBox45, checkBox46, checkBox47, checkBox48, checkBox49,
                checkBox50, checkBox51, checkBox52, checkBox53, checkBox54, checkBox55, checkBox56, checkBox57, checkBox58, checkBox59,
                checkBox60, checkBox61, checkBox62, checkBox63, checkBox64, checkBox65, checkBox66, checkBox67, checkBox68, checkBox69,
                checkBox70, checkBox71, checkBox72, checkBox73, checkBox74, checkBox75, checkBox76, checkBox77, checkBox78, checkBox79,
                checkBox80, checkBox81, checkBox82, checkBox83, checkBox84, checkBox85, checkBox86, checkBox87, checkBox88, checkBox89,
                checkBox90, checkBox91, checkBox92, checkBox93, checkBox94, checkBox95, checkBox96, checkBox97, checkBox98, checkBox99,
                checkBox100, checkBox101, checkBox102, checkBox103, checkBox104, checkBox105, checkBox106, checkBox107, checkBox108, checkBox109,
                checkBox110, checkBox111, checkBox112, /*checkBox113, checkBox114,*/ checkBox115, checkBox116, checkBox117, checkBox118, checkBox119,
                checkBox120, checkBox121, checkBox122, checkBox123, checkBox124, checkBox125, checkBox126, checkBox127, checkBox128, checkBox129,
                checkBox130, checkBox131, checkBox132, checkBox133, checkBox134, checkBox135, checkBox136, checkBox137, checkBox138, checkBox139,
                checkBox140, checkBox141, checkBox142, checkBox143, checkBox144, checkBox145, checkBox146, checkBox147, checkBox148, checkBox149,
                checkBox150, checkBox151, checkBox152, checkBox153, checkBox154, checkBox155, checkBox156, checkBox157, checkBox158, checkBox159,
                checkBox160, checkBox161, checkBox162, checkBox163, checkBox164, checkBox165, checkBox166, checkBox167, checkBox168, checkBox169,
                checkBox170, checkBox171, checkBox172, /*checkBox173,*/ checkBox174, checkBox175, checkBox176, checkBox177, checkBox178, checkBox179,
                checkBox180, checkBox181, checkBox182, checkBox183, checkBox184, checkBox185, checkBox186, checkBox187, checkBox188, checkBox189,
                checkBox190, checkBox191, checkBox192, checkBox193, checkBox194, checkBox195, checkBox196, checkBox197, checkBox198, checkBox199,
                checkBox200, checkBox201, checkBox202, checkBox203, checkBox204, checkBox205, checkBox206, checkBox207, checkBox208, checkBox209,
                checkBox210, checkBox211, checkBox212, checkBox213, checkBox214, checkBox215, checkBox216, checkBox217, checkBox218, checkBox219,
                checkBox220, checkBox221, checkBox222, checkBox223, checkBox224, checkBox225, checkBox226, checkBox227, checkBox228, checkBox229,
                checkBox230, checkBox231, checkBox232, checkBox233, checkBox234, checkBox235, checkBox236, checkBox237, checkBox238, checkBox239
            };
        NumericUpDown[] GetNUDs() =>
            new NumericUpDown[]
            {
                numericUpDown1, numericUpDown2, numericUpDown3, numericUpDown4, numericUpDown5, numericUpDown6, numericUpDown7, numericUpDown8, numericUpDown9,
                numericUpDown10, numericUpDown11, numericUpDown12, numericUpDown13, numericUpDown14, numericUpDown15, numericUpDown16, numericUpDown17, numericUpDown18, numericUpDown19,
                numericUpDown20, numericUpDown21, numericUpDown22, numericUpDown23, numericUpDown24, numericUpDown25, numericUpDown26, numericUpDown27, numericUpDown28, numericUpDown29,
                numericUpDown30, numericUpDown31, numericUpDown32, numericUpDown33, numericUpDown34, numericUpDown35, numericUpDown36, numericUpDown37, numericUpDown38, numericUpDown39,
                numericUpDown40, /*numericUpDown41,*/ numericUpDown42, numericUpDown43, numericUpDown44, numericUpDown45, numericUpDown46, numericUpDown47, numericUpDown48, numericUpDown49,
                numericUpDown50, numericUpDown51, numericUpDown52, numericUpDown53, numericUpDown54, numericUpDown55, numericUpDown56, numericUpDown57
            };
        RadioButton[] GetRadioButtons() =>
            new RadioButton[]
            {
                radioButton1, radioButton2, radioButton3, radioButton4, radioButton5, radioButton6, radioButton7, radioButton8, radioButton9,
                radioButton10, radioButton11, radioButton12, radioButton13, radioButton14, radioButton15, radioButton16, radioButton17
            };
        ComboBox[] GetComboBoxes() =>
            new ComboBox[]
            {
                comboBox1, comboBox2, comboBox3, comboBox4, comboBox5, comboBox6, comboBox7, comboBox8, comboBox9,
                comboBox10, comboBox11, comboBox12, comboBox13, comboBox14, comboBox15, comboBox16, comboBox17, comboBox18, comboBox19,
                comboBox20, comboBox21, comboBox22, comboBox23, comboBox24, comboBox25, comboBox26, comboBox27, comboBox28, comboBox29,
                comboBox30, comboBox31, comboBox32, comboBox33, comboBox34, comboBox35, comboBox36, comboBox37, comboBox38, comboBox39,
                comboBox40, comboBox41, comboBox42, comboBox43, comboBox44, comboBox45, comboBox46, comboBox47, comboBox48, comboBox49,
                comboBox50, comboBox51, comboBox52, comboBox53, comboBox54, comboBox55, comboBox56, comboBox57, comboBox58, comboBox59,
                comboBox60, comboBox61, comboBox62, comboBox63, comboBox64, comboBox65, comboBox66, comboBox67, comboBox68, comboBox69,
                comboBox70, comboBox71, comboBox72, comboBox73, comboBox74, comboBox75, comboBox76, comboBox77
            };
        TextBox[] GetTextBoxes() => new TextBox[] { textBox3 };
        ComboBox[] GetPresetComboBoxes() =>
            new ComboBox[]
            {
                comboBox2, comboBox3, comboBox4, comboBox5, comboBox6, comboBox7, comboBox8, comboBox9,
                comboBox10, comboBox11, comboBox12, comboBox13, comboBox14, comboBox15, comboBox16, comboBox17, comboBox18, comboBox19,
                comboBox20, comboBox21, comboBox22, comboBox23, comboBox24, comboBox25, comboBox26, comboBox27, comboBox28, comboBox29,
                comboBox30, comboBox31, comboBox32, comboBox33, comboBox34, comboBox35, comboBox36, comboBox37, comboBox38, comboBox39,
                comboBox40, comboBox41, comboBox42, comboBox43, comboBox45, comboBox46, comboBox47, comboBox48, comboBox49,
                comboBox50, comboBox51, comboBox52, comboBox53, comboBox54, comboBox55, comboBox56, comboBox57, comboBox58, comboBox59,
                comboBox60, comboBox61, comboBox62, comboBox63, comboBox64, comboBox65, comboBox66, comboBox67, comboBox68, comboBox69,
                comboBox70, comboBox71, comboBox72, comboBox73, comboBox74, comboBox75, comboBox76, comboBox77
            };
        ComboBox[] GetAutoCopyPastePresetComboBoxes() =>
            new ComboBox[]
            {
                comboBox2, comboBox3, comboBox4, comboBox5, comboBox6, comboBox7,
                comboBox12, comboBox13, comboBox16, comboBox17, comboBox19,
                comboBox30, comboBox35, comboBox36, comboBox38,
                comboBox40, comboBox43, comboBox47, comboBox49,
                comboBox53, comboBox56,
                comboBox60, comboBox62, comboBox63, comboBox64, comboBox66, comboBox68,
                comboBox72, comboBox76
            };
        ComboBox[] GetCopiedColorIDPresetComboBoxes() => new ComboBox[] { comboBox26, comboBox31 };
        ComboBox[] GetSecondaryGroupIDPresetComboBoxes() => new ComboBox[] { comboBox41, comboBox45 };
        ComboBox[] GetTargetItemIDPresetComboBoxes() => new ComboBox[] { comboBox14, comboBox50, comboBox51, comboBox58, comboBox71 };
        ComboBox[] GetTargetColorIDPresetComboBoxes() => new ComboBox[] { comboBox24, comboBox27 };
        ComboBox[] GetTargetGroupIDPresetComboBoxes() => new ComboBox[] { comboBox10, comboBox20, comboBox21, comboBox22, comboBox23, comboBox25, comboBox33, comboBox34, comboBox37, comboBox39, comboBox42, comboBox46, comboBox48, comboBox52, comboBox55, comboBox59, comboBox61};
        ComboBox[] GetUsedColorIDPresetComboBoxes() => new ComboBox[] { comboBox9, comboBox28, comboBox74, comboBox77 };
        ComboBox[] GetAutoAddGroupIDPresetComboBoxes() => new ComboBox[] { comboBox8, comboBox11, comboBox15, comboBox18, comboBox29, comboBox32, comboBox65, comboBox67, comboBox69, comboBox73, comboBox75 };

        void CheckForUpdates()
        {
            try
            {
                string updatePath;
                using (WebResponse resp = checkForUpdates.GetResponse())
                    updatePath = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                if (updatePath != "0")
                    DownloadLatestVersion(updatePath);
            }
            catch { notification.ShowBalloonTip(5000, "Connection Error", "Unable to connect to the server. Please try again later.", ToolTipIcon.Error); }
        }
        void DownloadLatestVersion(string path)
        {
            SetupWebObject(ref updateDownloader);
            checkForUpdatesToolStripMenuItem.Enabled = false;
            checkForUpdatesToolStripMenuItem.Text = "Downloading Update...";
            if (Options.BoolDictionary["softwareUpdateStatusNotifications"])
                notification.ShowBalloonTip(5000, "Download Started", "Software update has just began.", ToolTipIcon.Info);
            updateDownloader.DownloadFile(path, updateFilePath);
            if (Options.BoolDictionary["softwareUpdateStatusNotifications"])
                notification.ShowBalloonTip(5000, "Download Finished", "Software update has been downloaded successfully. Click on the update to open the location of the update.", ToolTipIcon.Info);
            checkForUpdatesToolStripMenuItem.Text = "Open Update Location";
            checkForUpdatesToolStripMenuItem.Enabled = true;
            updatedVersion = true;
        }
        void DownloadNewBPMFile()
        {
            SetupWebObject(ref resourceDownloader);
            updateResourcesToolStripMenuItem.Enabled = false;
            updateResourcesToolStripMenuItem.Text = "Updating Resources...";
            if (Options.BoolDictionary["resourceUpdateStatusNotifications"])
                notification.ShowBalloonTip(5000, "Download Started", "The program has started downloading the new BPM list file.", ToolTipIcon.Info);
            resourceDownloader.DownloadFile("http://someeffect.altervista.org/esfiles/resources/BPMList.esf", appLocation + @"\EffectSome\Resources\BPMList.esf");
            if (Options.BoolDictionary["resourceUpdateStatusNotifications"])
                notification.ShowBalloonTip(5000, "Download Finished", "The new BPM list file was successfully downloaded.", ToolTipIcon.Info);
            updateResourcesToolStripMenuItem.Text = "Update Resources";
            updateResourcesToolStripMenuItem.Enabled = true;
        }
        
        // Section of code that needs to be rewritten to only take up 100 lines instead of 2500
        void WriteTriggersCopyPasteAutomation()
        {
            #region Move
            if (checkBox122.Checked && presets[comboBox20.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs)
            {
                if (checkBox23.CheckState == CheckState.Checked)
                    InjectMoveTriggerCopyPasteAutomation3
                    (
                        checkBox26.Checked ? (float)numericUpDown3.Value : 0, checkBox26.CheckState == CheckState.Indeterminate,
                        checkBox14.Checked ? (int)numericUpDown1.Value : 0, checkBox14.CheckState == CheckState.Indeterminate, checkBox22.Checked ? (int)numericUpDown2.Value : 0, checkBox22.CheckState == CheckState.Indeterminate,
                        new bool[] { checkBox14.Checked && checkBox33.Checked, checkBox2.Checked && checkBox34.Checked }, GetEasingValue(comboBox1.SelectedIndex, checkBox25.Checked, checkBox24.Checked),
                        checkBox56.Checked ? (float)numericUpDown23.Value : 0, checkBox56.CheckState == CheckState.Indeterminate, checkBox156.Checked, checkBox156.Checked ? (int)numericUpDown44.Value : 0,
                        new bool[] { checkBox158.Checked, checkBox157.Checked },
                        checkBox122.Checked ? (presets[comboBox2.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox2.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox122.Checked ? (presets[comboBox2.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox2.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                        checkBox7.Checked, checkBox11.Checked, checkBox6.Checked
                    );
                else
                    InjectMoveTriggerCopyPasteAutomation6
                    (
                        checkBox26.Checked ? (float)numericUpDown3.Value : 0, checkBox26.CheckState == CheckState.Indeterminate,
                        checkBox14.Checked ? (int)numericUpDown1.Value : 0, checkBox14.CheckState == CheckState.Indeterminate, checkBox22.Checked ? (int)numericUpDown2.Value : 0, checkBox22.CheckState == CheckState.Indeterminate,
                        new bool[] { checkBox14.Checked && checkBox33.Checked, checkBox2.Checked && checkBox34.Checked }, checkBox23.CheckState == CheckState.Indeterminate,
                        checkBox56.Checked ? (float)numericUpDown23.Value : 0, checkBox56.CheckState == CheckState.Indeterminate, checkBox156.Checked, checkBox156.Checked ? (int)numericUpDown44.Value : 0,
                        new bool[] { checkBox158.Checked, checkBox157.Checked },
                        checkBox122.Checked ? (presets[comboBox2.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox2.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox122.Checked ? (presets[comboBox2.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox2.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                        checkBox7.Checked, checkBox11.Checked, checkBox6.Checked
                    );
            }
            else if (!checkBox122.Checked || presets[comboBox20.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment)
            {
                if (checkBox23.CheckState == CheckState.Checked)
                    InjectMoveTriggerCopyPasteAutomation1
                    (
                        checkBox26.Checked ? (float)numericUpDown3.Value : 0, checkBox26.CheckState == CheckState.Indeterminate,
                        checkBox14.Checked ? (int)numericUpDown1.Value : 0, checkBox14.CheckState == CheckState.Indeterminate, checkBox22.Checked ? (int)numericUpDown2.Value : 0, checkBox22.CheckState == CheckState.Indeterminate,
                        new bool[] { checkBox14.Checked && checkBox33.Checked, checkBox2.Checked && checkBox34.Checked }, GetEasingValue(comboBox1.SelectedIndex, checkBox25.Checked, checkBox24.Checked),
                        checkBox56.Checked ? (float)numericUpDown23.Value : 0, checkBox56.CheckState == CheckState.Indeterminate, checkBox156.Checked, checkBox156.Checked ? (int)numericUpDown44.Value : 0,
                        new bool[] { checkBox158.Checked, checkBox157.Checked }, presets[comboBox20.SelectedIndex].AdjustIDsAdjustment,
                        checkBox122.Checked ? (presets[comboBox2.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox2.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox122.Checked ? (presets[comboBox2.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox2.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                        checkBox7.Checked, checkBox11.Checked, checkBox6.Checked
                    );
                else
                    InjectMoveTriggerCopyPasteAutomation4
                    (
                        checkBox26.Checked ? (float)numericUpDown3.Value : 0, checkBox26.CheckState == CheckState.Indeterminate,
                        checkBox14.Checked ? (int)numericUpDown1.Value : 0, checkBox14.CheckState == CheckState.Indeterminate, checkBox22.Checked ? (int)numericUpDown2.Value : 0, checkBox22.CheckState == CheckState.Indeterminate,
                        new bool[] { checkBox14.Checked && checkBox33.Checked, checkBox2.Checked && checkBox34.Checked }, checkBox23.CheckState == CheckState.Indeterminate,
                        checkBox56.Checked ? (float)numericUpDown23.Value : 0, checkBox56.CheckState == CheckState.Indeterminate, checkBox156.Checked, checkBox156.Checked ? (int)numericUpDown44.Value : 0,
                        new bool[] { checkBox158.Checked, checkBox157.Checked }, presets[comboBox20.SelectedIndex].AdjustIDsAdjustment,
                        checkBox122.Checked ? (presets[comboBox2.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox2.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox122.Checked ? (presets[comboBox2.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox2.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                        checkBox7.Checked, checkBox11.Checked, checkBox6.Checked
                    );
            }
            else if (presets[comboBox20.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues)
            {
                if (checkBox23.CheckState == CheckState.Checked)
                    InjectMoveTriggerCopyPasteAutomation2
                    (
                        checkBox26.Checked ? (float)numericUpDown3.Value : 0, checkBox26.CheckState == CheckState.Indeterminate,
                        checkBox14.Checked ? (int)numericUpDown1.Value : 0, checkBox14.CheckState == CheckState.Indeterminate, checkBox22.Checked ? (int)numericUpDown2.Value : 0, checkBox22.CheckState == CheckState.Indeterminate,
                        new bool[] { checkBox14.Checked && checkBox33.Checked, checkBox2.Checked && checkBox34.Checked }, GetEasingValue(comboBox1.SelectedIndex, checkBox25.Checked, checkBox24.Checked),
                        checkBox56.Checked ? (float)numericUpDown23.Value : 0, checkBox56.CheckState == CheckState.Indeterminate, checkBox156.Checked, checkBox156.Checked ? (int)numericUpDown44.Value : 0,
                        new bool[] { checkBox158.Checked, checkBox157.Checked }, presets[comboBox20.SelectedIndex].AdjustIDsSpecifiedValues,
                        checkBox122.Checked ? (presets[comboBox2.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox2.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox122.Checked ? (presets[comboBox2.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox2.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                        checkBox7.Checked, checkBox11.Checked, checkBox6.Checked
                    );
                else
                    InjectMoveTriggerCopyPasteAutomation5
                    (
                        checkBox26.Checked ? (float)numericUpDown3.Value : 0, checkBox26.CheckState == CheckState.Indeterminate,
                        checkBox14.Checked ? (int)numericUpDown1.Value : 0, checkBox14.CheckState == CheckState.Indeterminate, checkBox22.Checked ? (int)numericUpDown2.Value : 0, checkBox22.CheckState == CheckState.Indeterminate,
                        new bool[] { checkBox14.Checked && checkBox33.Checked, checkBox2.Checked && checkBox34.Checked }, checkBox23.CheckState == CheckState.Indeterminate,
                        checkBox56.Checked ? (float)numericUpDown23.Value : 0, checkBox56.CheckState == CheckState.Indeterminate, checkBox156.Checked, checkBox156.Checked ? (int)numericUpDown44.Value : 0,
                        new bool[] { checkBox158.Checked, checkBox157.Checked }, presets[comboBox20.SelectedIndex].AdjustIDsSpecifiedValues,
                        checkBox122.Checked ? (presets[comboBox2.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox2.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox122.Checked ? (presets[comboBox2.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox2.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                        checkBox7.Checked, checkBox11.Checked, checkBox6.Checked
                    );
            }
            #endregion
            #region Stop
            if (checkBox165.Checked && presets[comboBox35.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs)
                InjectStopTriggerCopyPasteAutomation3
                (
                    checkBox164.Checked ? (presets[comboBox34.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox34.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox164.Checked ? (presets[comboBox34.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox34.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                    checkBox20.Checked, checkBox21.Checked, checkBox17.Checked
                );
            else if (!checkBox165.Checked || presets[comboBox35.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment)
                InjectStopTriggerCopyPasteAutomation1
                (
                    checkBox165.Checked ? presets[comboBox35.SelectedIndex].AdjustIDsAdjustment : 0,
                    checkBox164.Checked ? (presets[comboBox34.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox34.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox164.Checked ? (presets[comboBox34.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox34.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox20.Checked, checkBox21.Checked, checkBox17.Checked
                );
            else if (presets[comboBox35.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues)
                InjectStopTriggerCopyPasteAutomation2
                (
                    checkBox165.Checked ? presets[comboBox35.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                    checkBox164.Checked ? (presets[comboBox34.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox34.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox164.Checked ? (presets[comboBox34.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox34.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox20.Checked, checkBox21.Checked, checkBox17.Checked
                );
            #endregion
            #region Alpha
            if (checkBox147.Checked && presets[comboBox21.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs)
                InjectAlphaTriggerCopyPasteAutomation3
                (
                    checkBox27.Checked ? (float)numericUpDown4.Value : 0, checkBox27.CheckState == CheckState.Indeterminate,
                    checkBox28.Checked ? (float)numericUpDown5.Value : 0, checkBox28.CheckState == CheckState.Indeterminate,
                    checkBox146.Checked ? (presets[comboBox4.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox4.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox146.Checked ? (presets[comboBox4.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox4.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox43.Checked, checkBox51.Checked, checkBox41.Checked
                );
            else if (!checkBox147.Checked || presets[comboBox21.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment)
                InjectAlphaTriggerCopyPasteAutomation1
                (
                    checkBox27.Checked ? (float)numericUpDown4.Value : 0, checkBox27.CheckState == CheckState.Indeterminate,
                    checkBox28.Checked ? (float)numericUpDown5.Value : 0, checkBox28.CheckState == CheckState.Indeterminate,
                    checkBox147.Checked ? presets[comboBox4.SelectedIndex].AdjustIDsAdjustment : 0,
                    checkBox146.Checked ? (presets[comboBox4.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox4.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox146.Checked ? (presets[comboBox4.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox4.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox43.Checked, checkBox51.Checked, checkBox41.Checked
                );
            else if (presets[comboBox21.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues)
                InjectAlphaTriggerCopyPasteAutomation2
                (
                    checkBox27.Checked ? (float)numericUpDown4.Value : 0, checkBox27.CheckState == CheckState.Indeterminate,
                    checkBox28.Checked ? (float)numericUpDown5.Value : 0, checkBox28.CheckState == CheckState.Indeterminate,
                    checkBox147.Checked ? presets[comboBox4.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                    checkBox146.Checked ? (presets[comboBox4.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox4.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox146.Checked ? (presets[comboBox4.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox4.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox43.Checked, checkBox51.Checked, checkBox41.Checked
                );
            #endregion
            #region On Death
            if (checkBox210.Checked && presets[comboBox61.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs)
                InjectOnDeathTriggerCopyPasteAutomation3
                (
                    checkBox207.Checked, checkBox206.Checked,
                    checkBox209.Checked ? (presets[comboBox63.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox63.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox209.Checked ? (presets[comboBox63.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox63.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox90.Checked, checkBox91.Checked, checkBox89.Checked
                );
            else if (!checkBox210.Checked || presets[comboBox61.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment)
                InjectOnDeathTriggerCopyPasteAutomation1
                (
                    checkBox207.Checked, checkBox206.Checked,
                    checkBox210.Checked ? presets[comboBox61.SelectedIndex].AdjustIDsAdjustment : 0,
                    checkBox209.Checked ? (presets[comboBox63.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox63.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox209.Checked ? (presets[comboBox63.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox63.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox90.Checked, checkBox91.Checked, checkBox89.Checked
                );
            else if (presets[comboBox61.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues)
                InjectOnDeathTriggerCopyPasteAutomation2
                (
                    checkBox207.Checked, checkBox206.Checked,
                    checkBox210.Checked ? presets[comboBox61.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                    checkBox209.Checked ? (presets[comboBox63.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox63.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox209.Checked ? (presets[comboBox63.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox63.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox90.Checked, checkBox91.Checked, checkBox89.Checked
                );
            #endregion
            #region Animate
            if (checkBox182.Checked && presets[comboBox37.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs)
                InjectAnimateTriggerCopyPasteAutomation3
                (
                    checkBox44.Checked ? (int)numericUpDown9.Value : 0, checkBox44.CheckState == CheckState.Indeterminate,
                    checkBox181.Checked ? (presets[comboBox38.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox38.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox181.Checked ? (presets[comboBox38.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox38.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox79.Checked, checkBox80.Checked, checkBox77.Checked
                );
            else if (!checkBox182.Checked || presets[comboBox37.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment)
                InjectAnimateTriggerCopyPasteAutomation1
                (
                    checkBox44.Checked ? (int)numericUpDown9.Value : 0, checkBox44.CheckState == CheckState.Indeterminate,
                    checkBox182.Checked ? presets[comboBox37.SelectedIndex].AdjustIDsAdjustment : 0,
                    checkBox181.Checked ? (presets[comboBox38.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox38.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox181.Checked ? (presets[comboBox38.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox38.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox79.Checked, checkBox80.Checked, checkBox77.Checked
                );
            else if (presets[comboBox37.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues)
                InjectAnimateTriggerCopyPasteAutomation2
                (
                    checkBox44.Checked ? (int)numericUpDown9.Value : 0, checkBox44.CheckState == CheckState.Indeterminate,
                    checkBox182.Checked ? presets[comboBox37.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                    checkBox181.Checked ? (presets[comboBox38.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox38.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox181.Checked ? (presets[comboBox38.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox38.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox79.Checked, checkBox80.Checked, checkBox77.Checked
                );
            #endregion
            #region Spawn
            if (checkBox128.Checked && presets[comboBox25.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs)
                InjectSpawnTriggerCopyPasteAutomation3
                (
                    checkBox155.Checked ? (float)numericUpDown43.Value : 0, checkBox155.CheckState == CheckState.Indeterminate,
                    checkBox127.Checked ? (presets[comboBox5.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox5.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox127.Checked ? (presets[comboBox5.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox5.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox69.Checked, checkBox70.Checked, checkBox68.Checked
                );
            else if (!checkBox128.Checked || presets[comboBox25.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment)
                InjectSpawnTriggerCopyPasteAutomation1
                (
                    checkBox155.Checked ? (float)numericUpDown43.Value : 0, checkBox155.CheckState == CheckState.Indeterminate,
                    checkBox128.Checked ? presets[comboBox25.SelectedIndex].AdjustIDsAdjustment : 0,
                    checkBox127.Checked ? (presets[comboBox5.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox5.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox127.Checked ? (presets[comboBox5.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox5.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox69.Checked, checkBox70.Checked, checkBox68.Checked
                );
            else if (presets[comboBox25.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues)
                InjectSpawnTriggerCopyPasteAutomation2
                (
                    checkBox155.Checked ? (float)numericUpDown43.Value : 0, checkBox155.CheckState == CheckState.Indeterminate,
                    checkBox128.Checked ? presets[comboBox25.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                    checkBox127.Checked ? (presets[comboBox5.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox5.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox127.Checked ? (presets[comboBox5.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox5.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox69.Checked, checkBox70.Checked, checkBox68.Checked
                );
            #endregion  
            #region Touch
            if (checkBox108.Checked && presets[comboBox46.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs)
                InjectTouchTriggerCopyPasteAutomation3
                (
                    checkBox106.Checked, checkBox105.Checked, checkBox124.Checked, checkBox121.Checked, checkBox126.Checked, checkBox125.Checked,
                    checkBox107.Checked ? (presets[comboBox47.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox47.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox107.Checked ? (presets[comboBox47.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox47.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox75.Checked, checkBox76.Checked, checkBox74.Checked
                );
            else if (!checkBox108.Checked || presets[comboBox46.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment)
                InjectTouchTriggerCopyPasteAutomation1
                (
                    checkBox106.Checked, checkBox105.Checked, checkBox124.Checked, checkBox121.Checked, checkBox126.Checked, checkBox125.Checked,
                    checkBox108.Checked ? presets[comboBox46.SelectedIndex].AdjustIDsAdjustment : 0,
                    checkBox107.Checked ? (presets[comboBox47.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox47.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox107.Checked ? (presets[comboBox47.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox47.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox75.Checked, checkBox76.Checked, checkBox74.Checked
                );
            else if (presets[comboBox46.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues)
                InjectTouchTriggerCopyPasteAutomation2
                (
                    checkBox106.Checked, checkBox105.Checked, checkBox124.Checked, checkBox121.Checked, checkBox126.Checked, checkBox125.Checked,
                    checkBox108.Checked ? presets[comboBox46.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                    checkBox107.Checked ? (presets[comboBox47.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox47.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox107.Checked ? (presets[comboBox47.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox47.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox75.Checked, checkBox76.Checked, checkBox74.Checked
                );
            #endregion
            #region Toggle
            if (checkBox143.Checked && presets[comboBox22.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs)
                InjectOnDeathTriggerCopyPasteAutomation3
                (
                    checkBox141.Checked, checkBox140.Checked,
                    checkBox142.Checked ? (presets[comboBox6.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox6.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox142.Checked ? (presets[comboBox6.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox6.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox83.Checked, checkBox85.Checked, checkBox82.Checked
                );
            else if (!checkBox143.Checked || presets[comboBox22.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment)
                InjectOnDeathTriggerCopyPasteAutomation1
                (
                    checkBox141.Checked, checkBox140.Checked,
                    checkBox143.Checked ? presets[comboBox22.SelectedIndex].AdjustIDsAdjustment : 0,
                    checkBox142.Checked ? (presets[comboBox6.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox6.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox142.Checked ? (presets[comboBox6.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox6.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox83.Checked, checkBox85.Checked, checkBox82.Checked
                );
            else if (presets[comboBox22.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues)
                InjectOnDeathTriggerCopyPasteAutomation2
                (
                    checkBox141.Checked, checkBox140.Checked,
                    checkBox143.Checked ? presets[comboBox22.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                    checkBox142.Checked ? (presets[comboBox6.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox6.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox142.Checked ? (presets[comboBox6.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox6.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox83.Checked, checkBox85.Checked, checkBox82.Checked
                );
            #endregion
            #region Rotate
            if (checkBox99.Checked && presets[comboBox42.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
            {
                if (checkBox102.Checked && presets[comboBox45.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Center Group ID
                {
                    if (checkBox96.CheckState == CheckState.Checked)
                        InjectRotateTriggerCopyPasteAutomation3
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate,
                            GetEasingValue((int)numericUpDown44.Value, checkBox95.Checked, checkBox61.Checked),
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                    else
                        InjectRotateTriggerCopyPasteAutomation6
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate, checkBox96.CheckState == CheckState.Indeterminate,
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                }
                else if (!checkBox102.Checked || presets[comboBox45.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Center Group ID
                {
                    if (checkBox96.CheckState == CheckState.Checked)
                        InjectRotateTriggerCopyPasteAutomation9
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate,
                            GetEasingValue((int)numericUpDown44.Value, checkBox95.Checked, checkBox61.Checked),
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox103.Checked ? presets[comboBox45.SelectedIndex].AdjustIDsAdjustment : (int)0,
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                    else
                        InjectRotateTriggerCopyPasteAutomation12
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate, checkBox96.CheckState == CheckState.Indeterminate,
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox103.Checked ? presets[comboBox45.SelectedIndex].AdjustIDsAdjustment : (int)0,
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                }
                else if (presets[comboBox45.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Center Group ID
                {
                    if (checkBox96.CheckState == CheckState.Checked)
                        InjectRotateTriggerCopyPasteAutomation15
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate,
                            GetEasingValue((int)numericUpDown44.Value, checkBox95.Checked, checkBox61.Checked),
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox103.Checked ? presets[comboBox45.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                    else
                        InjectRotateTriggerCopyPasteAutomation18
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate, checkBox96.CheckState == CheckState.Indeterminate,
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox103.Checked ? presets[comboBox45.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                }
            }
            else if (!checkBox99.Checked || presets[comboBox42.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
            {
                if (checkBox102.Checked && presets[comboBox45.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Center Group ID
                {
                    if (checkBox96.CheckState == CheckState.Checked)
                        InjectRotateTriggerCopyPasteAutomation1
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate,
                            GetEasingValue((int)numericUpDown44.Value, checkBox95.Checked, checkBox61.Checked),
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox102.Checked ? presets[comboBox42.SelectedIndex].AdjustIDsAdjustment : (int)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                    else
                        InjectRotateTriggerCopyPasteAutomation4
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate, checkBox96.CheckState == CheckState.Indeterminate,
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox102.Checked ? presets[comboBox42.SelectedIndex].AdjustIDsAdjustment : (int)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                }
                else if (!checkBox102.Checked || presets[comboBox45.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Center Group ID
                {
                    if (checkBox96.CheckState == CheckState.Checked)
                        InjectRotateTriggerCopyPasteAutomation7
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate,
                            GetEasingValue((int)numericUpDown44.Value, checkBox95.Checked, checkBox61.Checked),
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox102.Checked ? presets[comboBox42.SelectedIndex].AdjustIDsAdjustment : (int)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox103.Checked ? presets[comboBox45.SelectedIndex].AdjustIDsAdjustment : (int)0,
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                    else
                        InjectRotateTriggerCopyPasteAutomation10
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate, checkBox96.CheckState == CheckState.Indeterminate,
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox102.Checked ? presets[comboBox42.SelectedIndex].AdjustIDsAdjustment : (int)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox103.Checked ? presets[comboBox45.SelectedIndex].AdjustIDsAdjustment : (int)0,
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                }
                else if (presets[comboBox45.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Center Group ID
                {
                    if (checkBox96.CheckState == CheckState.Checked)
                        InjectRotateTriggerCopyPasteAutomation13
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate,
                            GetEasingValue((int)numericUpDown44.Value, checkBox95.Checked, checkBox61.Checked),
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox102.Checked ? presets[comboBox42.SelectedIndex].AdjustIDsAdjustment : (int)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox103.Checked ? presets[comboBox45.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                    else
                        InjectRotateTriggerCopyPasteAutomation16
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate, checkBox96.CheckState == CheckState.Indeterminate,
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox102.Checked ? presets[comboBox42.SelectedIndex].AdjustIDsAdjustment : (int)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox103.Checked ? presets[comboBox45.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                }
            }
            else if (presets[comboBox42.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
            {
                if (checkBox102.Checked && presets[comboBox45.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Center Group ID
                {
                    if (checkBox96.CheckState == CheckState.Checked)
                        InjectRotateTriggerCopyPasteAutomation2
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate,
                            GetEasingValue((int)numericUpDown44.Value, checkBox95.Checked, checkBox61.Checked),
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox102.Checked ? presets[comboBox42.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                    else
                        InjectRotateTriggerCopyPasteAutomation5
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate, checkBox96.CheckState == CheckState.Indeterminate,
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox102.Checked ? presets[comboBox42.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                }
                else if (!checkBox102.Checked || presets[comboBox45.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Center Group ID
                {
                    if (checkBox96.CheckState == CheckState.Checked)
                        InjectRotateTriggerCopyPasteAutomation8
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate,
                            GetEasingValue((int)numericUpDown44.Value, checkBox95.Checked, checkBox61.Checked),
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox102.Checked ? presets[comboBox42.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox103.Checked ? presets[comboBox45.SelectedIndex].AdjustIDsAdjustment : (int)0,
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                    else
                        InjectRotateTriggerCopyPasteAutomation11
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate, checkBox96.CheckState == CheckState.Indeterminate,
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox102.Checked ? presets[comboBox42.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox103.Checked ? presets[comboBox45.SelectedIndex].AdjustIDsAdjustment : (int)0,
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                }
                else if (presets[comboBox45.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Center Group ID
                {
                    if (checkBox96.CheckState == CheckState.Checked)
                        InjectRotateTriggerCopyPasteAutomation14
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate,
                            GetEasingValue((int)numericUpDown44.Value, checkBox95.Checked, checkBox61.Checked),
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox102.Checked ? presets[comboBox42.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox103.Checked ? presets[comboBox45.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                    else
                        InjectRotateTriggerCopyPasteAutomation17
                        (
                            checkBox101.Checked ? (float)numericUpDown16.Value : 0, checkBox101.CheckState == CheckState.Indeterminate,
                            checkBox98.Checked ? (int)numericUpDown15.Value : 0, checkBox98.CheckState == CheckState.Indeterminate,
                            checkBox97.Checked ? (int)numericUpDown14.Value : 0, checkBox97.CheckState == CheckState.Indeterminate, checkBox96.CheckState == CheckState.Indeterminate,
                            checkBox104.Checked ? (float)numericUpDown18.Value : 0, checkBox104.CheckState == CheckState.Indeterminate, checkBox55.Checked, checkBox54.Checked,
                            checkBox102.Checked ? presets[comboBox42.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox99.Checked ? (presets[comboBox43.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox43.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox103.Checked ? presets[comboBox45.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox15.Checked, checkBox16.Checked, checkBox12.Checked
                        );
                }
            }
            #endregion
            #region Collision
            if (checkBox198.Checked && presets[comboBox55.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
            {
                if (checkBox193.Checked && presets[comboBox54.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block A ID
                {
                    if (checkBox196.Checked && presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation1
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked
                        );
                    else if (!checkBox196.Checked || presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation4
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked
                        );
                    else if (presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation7
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked
                        );
                }
                else if (!checkBox193.Checked || presets[comboBox54.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block A ID
                {
                    if (checkBox196.Checked && presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation2
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked
                        );
                    else if (!checkBox196.Checked || presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation5
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked
                        );
                    else if (presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation8
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked
                        );
                }
                else if (presets[comboBox54.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Block A ID
                {
                    if (checkBox196.Checked && presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation3
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked
                        );
                    else if (!checkBox196.Checked || presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation6
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked
                        );
                    else if (presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation9
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked
                        );
                }
            }
            else if (!checkBox198.Checked || presets[comboBox55.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
            {
                if (checkBox193.Checked && presets[comboBox54.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block A ID
                {
                    if (checkBox196.Checked && presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation10
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (!checkBox196.Checked || presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation13
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation16
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                }
                else if (!checkBox193.Checked || presets[comboBox54.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block A ID
                {
                    if (checkBox196.Checked && presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation11
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (!checkBox196.Checked || presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation14
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation17
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                }
                else if (presets[comboBox54.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Block A ID
                {
                    if (checkBox196.Checked && presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation12
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (!checkBox196.Checked || presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation15
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation18
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                }
            }
            else if (presets[comboBox55.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
            {
                if (checkBox193.Checked && presets[comboBox54.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block A ID
                {
                    if (checkBox196.Checked && presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation19
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                    else if (!checkBox196.Checked || presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation22
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                    else if (presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation25
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                }
                else if (!checkBox193.Checked || presets[comboBox54.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block A ID
                {
                    if (checkBox196.Checked && presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation20
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                    else if (!checkBox196.Checked || presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation23
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                    else if (presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation26
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                }
                else if (presets[comboBox54.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Block A ID
                {
                    if (checkBox196.Checked && presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation21
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                    else if (!checkBox196.Checked || presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation24
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                    else if (presets[comboBox57.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Block B ID
                        InjectCollisionTriggerCopyPasteAutomation27
                        (
                            checkBox195.Checked, checkBox194.Checked, checkBox185.Checked, checkBox184.Checked,
                            checkBox193.Checked ? presets[comboBox54.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox197.Checked ? (presets[comboBox56.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox56.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox196.Checked ? presets[comboBox57.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox93.Checked, checkBox94.Checked, checkBox92.Checked, checkBox198.Checked ? presets[comboBox55.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                }
            }
            #endregion
            #region Follow Player Y
            if (checkBox204.Checked && presets[comboBox59.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs)
                InjectFollowPlayerYTriggerCopyPasteAutomation3
                (
                    checkBox202.Checked ? (float)numericUpDown28.Value : 0, checkBox201.Checked ? (float)numericUpDown27.Value : 0, checkBox199.Checked ? (float)numericUpDown29.Value : 0,
                    checkBox205.Checked ? (int)numericUpDown30.Value : 0, checkBox200.Checked ? (float)numericUpDown21.Value : 0,
                    checkBox203.Checked ? (presets[comboBox60.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox60.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox203.Checked ? (presets[comboBox60.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox60.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox66.Checked, checkBox67.Checked, checkBox65.Checked
                );
            else if (!checkBox204.Checked || presets[comboBox59.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment)
                InjectFollowPlayerYTriggerCopyPasteAutomation1
                (
                    checkBox202.Checked ? (float)numericUpDown28.Value : 0, checkBox201.Checked ? (float)numericUpDown27.Value : 0, checkBox199.Checked ? (float)numericUpDown29.Value : 0,
                    checkBox205.Checked ? (int)numericUpDown30.Value : 0, checkBox200.Checked ? (float)numericUpDown21.Value : 0,
                    checkBox204.Checked ? presets[comboBox59.SelectedIndex].AdjustIDsAdjustment : 0,
                    checkBox203.Checked ? (presets[comboBox60.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox60.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox203.Checked ? (presets[comboBox60.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox60.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox66.Checked, checkBox67.Checked, checkBox65.Checked
                );
            else if (presets[comboBox59.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues)
                InjectFollowPlayerYTriggerCopyPasteAutomation2
                (
                    checkBox202.Checked ? (float)numericUpDown28.Value : 0, checkBox201.Checked ? (float)numericUpDown27.Value : 0, checkBox199.Checked ? (float)numericUpDown29.Value : 0,
                    checkBox205.Checked ? (int)numericUpDown30.Value : 0, checkBox200.Checked ? (float)numericUpDown21.Value : 0,
                    checkBox204.Checked ? presets[comboBox59.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                    checkBox203.Checked ? (presets[comboBox60.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox60.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox203.Checked ? (presets[comboBox60.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox60.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox66.Checked, checkBox67.Checked, checkBox65.Checked
                );
            #endregion
            #region Shake
            InjectShakeTriggerCopyPasteAutomation
            (
                checkBox176.Checked ? (float)numericUpDown46.Value : 0, checkBox177.Checked ? (float)numericUpDown47.Value : 0, checkBox178.Checked ? (float)numericUpDown48.Value : 0,
                presets[comboBox36.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox36.SelectedIndex].AutoCopyPasteMoveX : 0, presets[comboBox36.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox36.SelectedIndex].AutoCopyPasteMoveY : 0,
                checkBox87.Checked, checkBox88.Checked, checkBox86.Checked
            );
            #endregion
            #region Pulse
            if (checkBox174.Checked && presets[comboBox31.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Copied Color ID
            {
                if (checkBox3.Checked && presets[comboBox24.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Color ID
                {
                    if (checkBox46.Checked && presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation1
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked
                        );
                    else if (!checkBox46.Checked || presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation4
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked
                        );
                    else if (presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation7
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked
                        );
                }
                else if (!checkBox3.Checked || presets[comboBox24.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Color ID
                {
                    if (checkBox46.Checked && presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation2
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked
                        );
                    else if (!checkBox46.Checked || presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation5
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked
                        );
                    else if (presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation8
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked
                        );
                }
                else if (presets[comboBox24.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Color ID
                {
                    if (checkBox46.Checked && presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation3
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked
                        );
                    else if (!checkBox46.Checked || presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation6
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked
                        );
                    else if (presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation9
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked
                        );
                }
            }
            else if (!checkBox174.Checked || presets[comboBox31.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Copied Color ID
            {
                if (checkBox3.Checked && presets[comboBox24.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Color ID
                {
                    if (checkBox46.Checked && presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation10
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (!checkBox46.Checked || presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation13
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation16
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                }
                else if (!checkBox3.Checked || presets[comboBox24.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Color ID
                {
                    if (checkBox46.Checked && presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation11
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (!checkBox46.Checked || presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation14
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation17
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                }
                else if (presets[comboBox24.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Color ID
                {
                    if (checkBox46.Checked && presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation12
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (!checkBox46.Checked || presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation15
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation18
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                }
            }
            else if (presets[comboBox31.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Copied Color ID
            {
                if (checkBox3.Checked && presets[comboBox24.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Color ID
                {
                    if (checkBox46.Checked && presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation19
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                    else if (!checkBox46.Checked || presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation22
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                    else if (presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation25
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                }
                else if (!checkBox3.Checked || presets[comboBox24.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Color ID
                {
                    if (checkBox46.Checked && presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation20
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                    else if (!checkBox46.Checked || presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation23
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                    else if (presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation26
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                }
                else if (presets[comboBox24.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Color ID
                {
                    if (checkBox46.Checked && presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation21
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                    else if (!checkBox46.Checked || presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation24
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                    else if (presets[comboBox23.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
                        InjectPulseTriggerCopyPasteAutomation27
                        (
                            ToInt32(radioButton3.Checked) + 2 * ToInt32(radioButton6.Checked), new bool[] { checkBox31.Checked, checkBox32.Checked }, checkBox239.Checked,
                            checkBox37.Checked ? (float)numericUpDown6.Value : 0, checkBox40.Checked ? (float)numericUpDown7.Value : 0, checkBox42.Checked ? (float)numericUpDown8.Value : 0,
                            ToInt32(radioButton7.Checked) + 2 * ToInt32(radioButton17.Checked),
                            new int[] { checkBox59.Checked ? (int)numericUpDown54.Value : 0, checkBox235.Checked ? (int)numericUpDown53.Value : 0, checkBox60.Checked ? (int)numericUpDown52.Value : 0 },
                            new bool[] { checkBox59.CheckState == CheckState.Indeterminate, checkBox235.CheckState == CheckState.Indeterminate, checkBox60.CheckState == CheckState.Indeterminate },
                            new int[] { checkBox236.Checked ? (int)numericUpDown57.Value : 0, checkBox238.Checked ? (int)numericUpDown56.Value : 0, checkBox237.Checked ? (int)numericUpDown55.Value : 0 },
                            new bool[] { checkBox236.CheckState == CheckState.Indeterminate, checkBox238.CheckState == CheckState.Indeterminate, checkBox237.CheckState == CheckState.Indeterminate },
                            checkBox3.Checked ? presets[comboBox24.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox136.Checked ? (presets[comboBox3.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox3.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox46.Checked ? presets[comboBox23.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                            checkBox30.Checked, checkBox25.Checked, checkBox29.Checked,
                            checkBox174.Checked ? presets[comboBox31.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>()
                        );
                }
            }
            #endregion
            #region Pickup
            if (checkBox208.Checked && presets[comboBox58.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs)
                InjectPickupTriggerCopyPasteAutomation3
                (
                    checkBox211.Checked ? (int)numericUpDown31.Value : 0, checkBox211.CheckState == CheckState.Indeterminate,
                    checkBox212.Checked ? (presets[comboBox62.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox62.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox212.Checked ? (presets[comboBox62.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox62.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox4.Checked, checkBox5.Checked, checkBox2.Checked
                );
            else if (!checkBox208.Checked || presets[comboBox58.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment)
                InjectPickupTriggerCopyPasteAutomation1
                (
                    checkBox211.Checked ? (int)numericUpDown31.Value : 0, checkBox211.CheckState == CheckState.Indeterminate,
                    checkBox208.Checked ? presets[comboBox62.SelectedIndex].AdjustIDsAdjustment : 0,
                    checkBox212.Checked ? (presets[comboBox62.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox62.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox212.Checked ? (presets[comboBox62.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox62.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox4.Checked, checkBox5.Checked, checkBox2.Checked
                );
            else if (presets[comboBox58.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues)
                InjectPickupTriggerCopyPasteAutomation2
                (
                    checkBox211.Checked ? (int)numericUpDown31.Value : 0, checkBox211.CheckState == CheckState.Indeterminate,
                    checkBox208.Checked ? presets[comboBox62.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                    checkBox212.Checked ? (presets[comboBox62.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox62.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                    checkBox212.Checked ? (presets[comboBox62.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox62.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                    checkBox4.Checked, checkBox5.Checked, checkBox2.Checked
                );
            #endregion
            #region Count
            if (checkBox163.Checked && presets[comboBox48.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
            {
                if (checkBox159.Checked && presets[comboBox50.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Item ID
                    InjectCountTriggerCopyPasteAutomation3
                    (
                        checkBox180.Checked, checkBox179.Checked, checkBox183.Checked, checkBox160.Checked, checkBox161.Checked ? (int)numericUpDown20.Value : 0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                        checkBox38.Checked, checkBox39.Checked, checkBox36.Checked
                    );
                else if (!checkBox159.Checked || presets[comboBox50.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Item ID
                    InjectCountTriggerCopyPasteAutomation6
                    (
                        checkBox180.Checked, checkBox179.Checked, checkBox183.Checked, checkBox160.Checked, checkBox161.Checked ? (int)numericUpDown20.Value : 0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox159.Checked ? presets[comboBox50.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox38.Checked, checkBox39.Checked, checkBox36.Checked
                    );
                else if (presets[comboBox50.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Item ID
                    InjectCountTriggerCopyPasteAutomation9
                    (
                        checkBox180.Checked, checkBox179.Checked, checkBox183.Checked, checkBox160.Checked, checkBox161.Checked ? (int)numericUpDown20.Value : 0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox159.Checked ? presets[comboBox50.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox38.Checked, checkBox39.Checked, checkBox36.Checked
                    );
            }
            else if (!checkBox163.Checked || presets[comboBox48.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
            {
                if (checkBox159.Checked && presets[comboBox50.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Item ID
                    InjectCountTriggerCopyPasteAutomation1
                    (
                        checkBox180.Checked, checkBox179.Checked, checkBox183.Checked, checkBox160.Checked, checkBox161.Checked ? (int)numericUpDown20.Value : 0,
                        checkBox163.Checked ? presets[comboBox48.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox38.Checked, checkBox39.Checked, checkBox36.Checked
                    );
                else if (!checkBox159.Checked || presets[comboBox50.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Item ID
                    InjectCountTriggerCopyPasteAutomation4
                    (
                        checkBox180.Checked, checkBox179.Checked, checkBox183.Checked, checkBox160.Checked, checkBox161.Checked ? (int)numericUpDown20.Value : 0,
                        checkBox163.Checked ? presets[comboBox48.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox159.Checked ? presets[comboBox50.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox38.Checked, checkBox39.Checked, checkBox36.Checked
                    );
                else if (presets[comboBox50.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Item ID
                    InjectCountTriggerCopyPasteAutomation7
                    (
                        checkBox180.Checked, checkBox179.Checked, checkBox183.Checked, checkBox160.Checked, checkBox161.Checked ? (int)numericUpDown20.Value : 0,
                        checkBox163.Checked ? presets[comboBox48.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox159.Checked ? presets[comboBox50.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox38.Checked, checkBox39.Checked, checkBox36.Checked
                    );
            }
            else if (presets[comboBox48.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
            {
                if (checkBox159.Checked && presets[comboBox50.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Item ID
                    InjectCountTriggerCopyPasteAutomation2
                    (
                        checkBox180.Checked, checkBox179.Checked, checkBox183.Checked, checkBox160.Checked, checkBox161.Checked ? (int)numericUpDown20.Value : 0,
                        checkBox163.Checked ? presets[comboBox48.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox38.Checked, checkBox39.Checked, checkBox36.Checked
                    );
                else if (!checkBox159.Checked || presets[comboBox50.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Item ID
                    InjectCountTriggerCopyPasteAutomation5
                    (
                        checkBox180.Checked, checkBox179.Checked, checkBox183.Checked, checkBox160.Checked, checkBox161.Checked ? (int)numericUpDown20.Value : 0,
                        checkBox163.Checked ? presets[comboBox48.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox159.Checked ? presets[comboBox50.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox38.Checked, checkBox39.Checked, checkBox36.Checked
                    );
                else if (presets[comboBox50.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Item ID
                    InjectCountTriggerCopyPasteAutomation8
                    (
                        checkBox180.Checked, checkBox179.Checked, checkBox183.Checked, checkBox160.Checked, checkBox161.Checked ? (int)numericUpDown20.Value : 0,
                        checkBox163.Checked ? presets[comboBox48.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox162.Checked ? (presets[comboBox49.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox49.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox159.Checked ? presets[comboBox50.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox38.Checked, checkBox39.Checked, checkBox36.Checked
                    );
            }
            #endregion
            #region Instant Count
            if (checkBox191.Checked && presets[comboBox52.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
            {
                if (checkBox186.Checked && presets[comboBox51.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Item ID
                    InjectInstantCountTriggerCopyPasteAutomation3
                    (
                        checkBox188.Checked, checkBox187.Checked, radioButton1.Checked, radioButton12.Checked, radioButton16.Checked, checkBox192.Checked, checkBox189.Checked ? (int)numericUpDown19.Value : 0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                        checkBox63.Checked, checkBox64.Checked, checkBox62.Checked
                    );
                else if (!checkBox186.Checked || presets[comboBox51.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Item ID
                    InjectInstantCountTriggerCopyPasteAutomation6
                    (
                        checkBox188.Checked, checkBox187.Checked, radioButton1.Checked, radioButton12.Checked, radioButton16.Checked, checkBox192.Checked, checkBox189.Checked ? (int)numericUpDown19.Value : 0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox186.Checked ? presets[comboBox51.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox63.Checked, checkBox64.Checked, checkBox62.Checked
                    );
                else if (presets[comboBox51.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Item ID
                    InjectInstantCountTriggerCopyPasteAutomation9
                    (
                        checkBox188.Checked, checkBox187.Checked, radioButton1.Checked, radioButton12.Checked, radioButton16.Checked, checkBox192.Checked, checkBox189.Checked ? (int)numericUpDown19.Value : 0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox186.Checked ? presets[comboBox51.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox63.Checked, checkBox64.Checked, checkBox62.Checked
                    );
            }
            else if (!checkBox191.Checked || presets[comboBox52.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
            {
                if (checkBox186.Checked && presets[comboBox51.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Item ID
                    InjectInstantCountTriggerCopyPasteAutomation1
                    (
                        checkBox188.Checked, checkBox187.Checked, radioButton1.Checked, radioButton12.Checked, radioButton16.Checked, checkBox192.Checked, checkBox189.Checked ? (int)numericUpDown19.Value : 0,
                        checkBox191.Checked ? presets[comboBox52.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox63.Checked, checkBox64.Checked, checkBox62.Checked
                    );
                else if (!checkBox186.Checked || presets[comboBox51.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Item ID
                    InjectInstantCountTriggerCopyPasteAutomation4
                    (
                        checkBox188.Checked, checkBox187.Checked, radioButton1.Checked, radioButton12.Checked, radioButton16.Checked, checkBox192.Checked, checkBox189.Checked ? (int)numericUpDown19.Value : 0,
                        checkBox191.Checked ? presets[comboBox52.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox186.Checked ? presets[comboBox51.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox63.Checked, checkBox64.Checked, checkBox62.Checked
                    );
                else if (presets[comboBox51.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Item ID
                    InjectInstantCountTriggerCopyPasteAutomation7
                    (
                        checkBox188.Checked, checkBox187.Checked, radioButton1.Checked, radioButton12.Checked, radioButton16.Checked, checkBox192.Checked, checkBox189.Checked ? (int)numericUpDown19.Value : 0,
                        checkBox191.Checked ? presets[comboBox52.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox186.Checked ? presets[comboBox51.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox63.Checked, checkBox64.Checked, checkBox62.Checked
                    );
            }
            else if (presets[comboBox52.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
            {
                if (checkBox186.Checked && presets[comboBox51.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Item ID
                    InjectInstantCountTriggerCopyPasteAutomation2
                    (
                        checkBox188.Checked, checkBox187.Checked, radioButton1.Checked, radioButton12.Checked, radioButton16.Checked, checkBox192.Checked, checkBox189.Checked ? (int)numericUpDown19.Value : 0,
                        checkBox191.Checked ? presets[comboBox52.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox63.Checked, checkBox64.Checked, checkBox62.Checked
                    );
                else if (!checkBox186.Checked || presets[comboBox51.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Item ID
                    InjectInstantCountTriggerCopyPasteAutomation5
                    (
                        checkBox188.Checked, checkBox187.Checked, radioButton1.Checked, radioButton12.Checked, radioButton16.Checked, checkBox192.Checked, checkBox189.Checked ? (int)numericUpDown19.Value : 0,
                        checkBox191.Checked ? presets[comboBox52.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox186.Checked ? presets[comboBox51.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox63.Checked, checkBox64.Checked, checkBox62.Checked
                    );
                else if (presets[comboBox51.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Item ID
                    InjectInstantCountTriggerCopyPasteAutomation8
                    (
                        checkBox188.Checked, checkBox187.Checked, radioButton1.Checked, radioButton12.Checked, radioButton16.Checked, checkBox192.Checked, checkBox189.Checked ? (int)numericUpDown19.Value : 0,
                        checkBox191.Checked ? presets[comboBox52.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox190.Checked ? (presets[comboBox53.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox53.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox186.Checked ? presets[comboBox51.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox63.Checked, checkBox64.Checked, checkBox62.Checked
                    );
            }
            #endregion
            #region Follow
            if (checkBox50.Checked && presets[comboBox39.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group ID
            {
                if (checkBox53.Checked && presets[comboBox41.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Follow Group ID
                    InjectFollowTriggerCopyPasteAutomation3
                    (
                        checkBox48.Checked ? (float)numericUpDown12.Value : 0, checkBox47.Checked ? (float)numericUpDown11.Value : 0, checkBox45.Checked ? (float)numericUpDown10.Value : 0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                        checkBox57.Checked, checkBox58.Checked, checkBox52.Checked
                    );
                else if (!checkBox53.Checked || presets[comboBox41.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Follow Group ID
                    InjectFollowTriggerCopyPasteAutomation6
                    (
                        checkBox48.Checked ? (float)numericUpDown12.Value : 0, checkBox47.Checked ? (float)numericUpDown11.Value : 0, checkBox45.Checked ? (float)numericUpDown10.Value : 0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox53.Checked ? presets[comboBox41.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox57.Checked, checkBox58.Checked, checkBox52.Checked
                    );
                else if (presets[comboBox41.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Follow Group ID
                    InjectFollowTriggerCopyPasteAutomation9
                    (
                        checkBox48.Checked ? (float)numericUpDown12.Value : 0, checkBox47.Checked ? (float)numericUpDown11.Value : 0, checkBox45.Checked ? (float)numericUpDown10.Value : 0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox53.Checked ? presets[comboBox41.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox57.Checked, checkBox58.Checked, checkBox52.Checked
                    );
            }
            else if (!checkBox50.Checked || presets[comboBox39.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group ID
            {
                if (checkBox53.Checked && presets[comboBox41.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Follow Group ID
                    InjectFollowTriggerCopyPasteAutomation1
                    (
                        checkBox48.Checked ? (float)numericUpDown12.Value : 0, checkBox47.Checked ? (float)numericUpDown11.Value : 0, checkBox45.Checked ? (float)numericUpDown10.Value : 0,
                        checkBox50.Checked ? presets[comboBox39.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox57.Checked, checkBox58.Checked, checkBox52.Checked
                    );
                else if (!checkBox53.Checked || presets[comboBox41.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Follow Group ID
                    InjectFollowTriggerCopyPasteAutomation4
                    (
                        checkBox48.Checked ? (float)numericUpDown12.Value : 0, checkBox47.Checked ? (float)numericUpDown11.Value : 0, checkBox45.Checked ? (float)numericUpDown10.Value : 0,
                        checkBox50.Checked ? presets[comboBox39.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox53.Checked ? presets[comboBox41.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox57.Checked, checkBox58.Checked, checkBox52.Checked
                    );
                else if (presets[comboBox41.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Follow Group ID
                    InjectFollowTriggerCopyPasteAutomation7
                    (
                        checkBox48.Checked ? (float)numericUpDown12.Value : 0, checkBox47.Checked ? (float)numericUpDown11.Value : 0, checkBox45.Checked ? (float)numericUpDown10.Value : 0,
                        checkBox50.Checked ? presets[comboBox39.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox53.Checked ? presets[comboBox41.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox57.Checked, checkBox58.Checked, checkBox52.Checked
                    );
            }
            else if (presets[comboBox39.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group ID
            {
                if (checkBox53.Checked && presets[comboBox41.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Follow Group ID
                    InjectFollowTriggerCopyPasteAutomation2
                    (
                        checkBox48.Checked ? (float)numericUpDown12.Value : 0, checkBox47.Checked ? (float)numericUpDown11.Value : 0, checkBox45.Checked ? (float)numericUpDown10.Value : 0,
                        checkBox50.Checked ? presets[comboBox39.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox57.Checked, checkBox58.Checked, checkBox52.Checked
                    );
                else if (!checkBox53.Checked || presets[comboBox41.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Follow Group ID
                    InjectFollowTriggerCopyPasteAutomation5
                    (
                        checkBox48.Checked ? (float)numericUpDown12.Value : 0, checkBox47.Checked ? (float)numericUpDown11.Value : 0, checkBox45.Checked ? (float)numericUpDown10.Value : 0,
                        checkBox50.Checked ? presets[comboBox39.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox53.Checked ? presets[comboBox41.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox57.Checked, checkBox58.Checked, checkBox52.Checked
                    );
                else if (presets[comboBox41.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Follow Group ID
                    InjectFollowTriggerCopyPasteAutomation8
                    (
                        checkBox48.Checked ? (float)numericUpDown12.Value : 0, checkBox47.Checked ? (float)numericUpDown11.Value : 0, checkBox45.Checked ? (float)numericUpDown10.Value : 0,
                        checkBox50.Checked ? presets[comboBox39.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox53.Checked ? presets[comboBox41.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox57.Checked, checkBox58.Checked, checkBox52.Checked
                    );
            }
            #endregion
            #region Color
            if (checkBox115.Checked && presets[comboBox26.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Copied Color ID
            {
                if (checkBox109.Checked && presets[comboBox27.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Color ID
                    InjectColorTriggerCopyPasteAutomation1
                    (
                        checkBox112.Checked, checkBox111.Checked, checkBox117.Checked ? (float)numericUpDown24.Value : 0, checkBox117.CheckState == CheckState.Indeterminate,
                        checkBox110.Checked ? (float)numericUpDown22.Value : 0,
                        new int[] { checkBox232.Checked ? (int)numericUpDown51.Value : 0, checkBox234.Checked ? (int)numericUpDown50.Value : 0, checkBox233.Checked ? (int)numericUpDown49.Value : 0 },
                        new bool[] { checkBox232.CheckState == CheckState.Indeterminate, checkBox234.CheckState == CheckState.Indeterminate, checkBox233.CheckState == CheckState.Indeterminate },
                        new int[] { checkBox116.Checked ? (int)numericUpDown26.Value : 0, checkBox120.Checked ? (int)numericUpDown25.Value : 0, checkBox119.Checked ? (int)numericUpDown23.Value : 0 },
                        new bool[] { checkBox116.CheckState == CheckState.Indeterminate, checkBox120.CheckState == CheckState.Indeterminate, checkBox119.CheckState == CheckState.Indeterminate },
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : 0) : 0,
                        checkBox72.Checked, checkBox73.Checked, checkBox71.Checked
                    );
                else if (!checkBox109.Checked || presets[comboBox27.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Color ID
                    InjectColorTriggerCopyPasteAutomation4
                    (
                        checkBox112.Checked, checkBox111.Checked, checkBox117.Checked ? (float)numericUpDown24.Value : 0, checkBox117.CheckState == CheckState.Indeterminate,
                        checkBox110.Checked ? (float)numericUpDown22.Value : 0,
                        new int[] { checkBox232.Checked ? (int)numericUpDown51.Value : 0, checkBox234.Checked ? (int)numericUpDown50.Value : 0, checkBox233.Checked ? (int)numericUpDown49.Value : 0 },
                        new bool[] { checkBox232.CheckState == CheckState.Indeterminate, checkBox234.CheckState == CheckState.Indeterminate, checkBox233.CheckState == CheckState.Indeterminate },
                        new int[] { checkBox116.Checked ? (int)numericUpDown26.Value : 0, checkBox120.Checked ? (int)numericUpDown25.Value : 0, checkBox119.Checked ? (int)numericUpDown23.Value : 0 },
                        new bool[] { checkBox116.CheckState == CheckState.Indeterminate, checkBox120.CheckState == CheckState.Indeterminate, checkBox119.CheckState == CheckState.Indeterminate },
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox109.Checked ? presets[comboBox27.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox72.Checked, checkBox73.Checked, checkBox71.Checked
                    );
                else if (presets[comboBox27.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Color ID
                    InjectColorTriggerCopyPasteAutomation7
                    (
                        checkBox112.Checked, checkBox111.Checked, checkBox117.Checked ? (float)numericUpDown24.Value : 0, checkBox117.CheckState == CheckState.Indeterminate,
                        checkBox110.Checked ? (float)numericUpDown22.Value : 0,
                        new int[] { checkBox232.Checked ? (int)numericUpDown51.Value : 0, checkBox234.Checked ? (int)numericUpDown50.Value : 0, checkBox233.Checked ? (int)numericUpDown49.Value : 0 },
                        new bool[] { checkBox232.CheckState == CheckState.Indeterminate, checkBox234.CheckState == CheckState.Indeterminate, checkBox233.CheckState == CheckState.Indeterminate },
                        new int[] { checkBox116.Checked ? (int)numericUpDown26.Value : 0, checkBox120.Checked ? (int)numericUpDown25.Value : 0, checkBox119.Checked ? (int)numericUpDown23.Value : 0 },
                        new bool[] { checkBox116.CheckState == CheckState.Indeterminate, checkBox120.CheckState == CheckState.Indeterminate, checkBox119.CheckState == CheckState.Indeterminate },
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox109.Checked ? presets[comboBox27.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox72.Checked, checkBox73.Checked, checkBox71.Checked
                    );
            }
            else if (!checkBox115.Checked || presets[comboBox26.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Copied Color ID
            {
                if (checkBox109.Checked && presets[comboBox27.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Color ID
                    InjectColorTriggerCopyPasteAutomation3
                    (
                        checkBox112.Checked, checkBox111.Checked, checkBox117.Checked ? (float)numericUpDown24.Value : 0, checkBox117.CheckState == CheckState.Indeterminate,
                        checkBox110.Checked ? (float)numericUpDown22.Value : 0,
                        new int[] { checkBox232.Checked ? (int)numericUpDown51.Value : 0, checkBox234.Checked ? (int)numericUpDown50.Value : 0, checkBox233.Checked ? (int)numericUpDown49.Value : 0 },
                        new bool[] { checkBox232.CheckState == CheckState.Indeterminate, checkBox234.CheckState == CheckState.Indeterminate, checkBox233.CheckState == CheckState.Indeterminate },
                        new int[] { checkBox116.Checked ? (int)numericUpDown26.Value : 0, checkBox120.Checked ? (int)numericUpDown25.Value : 0, checkBox119.Checked ? (int)numericUpDown23.Value : 0 },
                        new bool[] { checkBox116.CheckState == CheckState.Indeterminate, checkBox120.CheckState == CheckState.Indeterminate, checkBox119.CheckState == CheckState.Indeterminate },
                        checkBox115.Checked ? presets[comboBox26.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox72.Checked, checkBox73.Checked, checkBox71.Checked
                    );
                else if (!checkBox109.Checked || presets[comboBox27.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Color ID
                    InjectColorTriggerCopyPasteAutomation6
                    (
                        checkBox112.Checked, checkBox111.Checked, checkBox117.Checked ? (float)numericUpDown24.Value : 0, checkBox117.CheckState == CheckState.Indeterminate,
                        checkBox110.Checked ? (float)numericUpDown22.Value : 0,
                        new int[] { checkBox232.Checked ? (int)numericUpDown51.Value : 0, checkBox234.Checked ? (int)numericUpDown50.Value : 0, checkBox233.Checked ? (int)numericUpDown49.Value : 0 },
                        new bool[] { checkBox232.CheckState == CheckState.Indeterminate, checkBox234.CheckState == CheckState.Indeterminate, checkBox233.CheckState == CheckState.Indeterminate },
                        new int[] { checkBox116.Checked ? (int)numericUpDown26.Value : 0, checkBox120.Checked ? (int)numericUpDown25.Value : 0, checkBox119.Checked ? (int)numericUpDown23.Value : 0 },
                        new bool[] { checkBox116.CheckState == CheckState.Indeterminate, checkBox120.CheckState == CheckState.Indeterminate, checkBox119.CheckState == CheckState.Indeterminate },
                        checkBox115.Checked ? presets[comboBox26.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox109.Checked ? presets[comboBox27.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox72.Checked, checkBox73.Checked, checkBox71.Checked
                    );
                else if (presets[comboBox27.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Color ID
                    InjectColorTriggerCopyPasteAutomation9
                    (
                        checkBox112.Checked, checkBox111.Checked, checkBox117.Checked ? (float)numericUpDown24.Value : 0, checkBox117.CheckState == CheckState.Indeterminate,
                        checkBox110.Checked ? (float)numericUpDown22.Value : 0,
                        new int[] { checkBox232.Checked ? (int)numericUpDown51.Value : 0, checkBox234.Checked ? (int)numericUpDown50.Value : 0, checkBox233.Checked ? (int)numericUpDown49.Value : 0 },
                        new bool[] { checkBox232.CheckState == CheckState.Indeterminate, checkBox234.CheckState == CheckState.Indeterminate, checkBox233.CheckState == CheckState.Indeterminate },
                        new int[] { checkBox116.Checked ? (int)numericUpDown26.Value : 0, checkBox120.Checked ? (int)numericUpDown25.Value : 0, checkBox119.Checked ? (int)numericUpDown23.Value : 0 },
                        new bool[] { checkBox116.CheckState == CheckState.Indeterminate, checkBox120.CheckState == CheckState.Indeterminate, checkBox119.CheckState == CheckState.Indeterminate },
                        checkBox115.Checked ? presets[comboBox26.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox109.Checked ? presets[comboBox27.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox72.Checked, checkBox73.Checked, checkBox71.Checked
                    );
            }
            else if (presets[comboBox26.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Copied Color ID
            {
                if (checkBox109.Checked && presets[comboBox27.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Color ID
                    InjectColorTriggerCopyPasteAutomation2
                    (
                        checkBox112.Checked, checkBox111.Checked, checkBox117.Checked ? (float)numericUpDown24.Value : 0, checkBox117.CheckState == CheckState.Indeterminate,
                        checkBox110.Checked ? (float)numericUpDown22.Value : 0,
                        new int[] { checkBox232.Checked ? (int)numericUpDown51.Value : 0, checkBox234.Checked ? (int)numericUpDown50.Value : 0, checkBox233.Checked ? (int)numericUpDown49.Value : 0 },
                        new bool[] { checkBox232.CheckState == CheckState.Indeterminate, checkBox234.CheckState == CheckState.Indeterminate, checkBox233.CheckState == CheckState.Indeterminate },
                        new int[] { checkBox116.Checked ? (int)numericUpDown26.Value : 0, checkBox120.Checked ? (int)numericUpDown25.Value : 0, checkBox119.Checked ? (int)numericUpDown23.Value : 0 },
                        new bool[] { checkBox116.CheckState == CheckState.Indeterminate, checkBox120.CheckState == CheckState.Indeterminate, checkBox119.CheckState == CheckState.Indeterminate },
                        checkBox115.Checked ? presets[comboBox26.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox72.Checked, checkBox73.Checked, checkBox71.Checked
                    );
                else if (!checkBox109.Checked || presets[comboBox27.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Color ID
                    InjectColorTriggerCopyPasteAutomation5
                    (
                        checkBox112.Checked, checkBox111.Checked, checkBox117.Checked ? (float)numericUpDown24.Value : 0, checkBox117.CheckState == CheckState.Indeterminate,
                        checkBox110.Checked ? (float)numericUpDown22.Value : 0,
                        new int[] { checkBox232.Checked ? (int)numericUpDown51.Value : 0, checkBox234.Checked ? (int)numericUpDown50.Value : 0, checkBox233.Checked ? (int)numericUpDown49.Value : 0 },
                        new bool[] { checkBox232.CheckState == CheckState.Indeterminate, checkBox234.CheckState == CheckState.Indeterminate, checkBox233.CheckState == CheckState.Indeterminate },
                        new int[] { checkBox116.Checked ? (int)numericUpDown26.Value : 0, checkBox120.Checked ? (int)numericUpDown25.Value : 0, checkBox119.Checked ? (int)numericUpDown23.Value : 0 },
                        new bool[] { checkBox116.CheckState == CheckState.Indeterminate, checkBox120.CheckState == CheckState.Indeterminate, checkBox119.CheckState == CheckState.Indeterminate },
                        checkBox115.Checked ? presets[comboBox26.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox109.Checked ? presets[comboBox27.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox72.Checked, checkBox73.Checked, checkBox71.Checked
                    );
                else if (presets[comboBox27.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Color ID
                    InjectColorTriggerCopyPasteAutomation8
                    (
                        checkBox112.Checked, checkBox111.Checked, checkBox117.Checked ? (float)numericUpDown24.Value : 0, checkBox117.CheckState == CheckState.Indeterminate,
                        checkBox110.Checked ? (float)numericUpDown22.Value : 0,
                        new int[] { checkBox232.Checked ? (int)numericUpDown51.Value : 0, checkBox234.Checked ? (int)numericUpDown50.Value : 0, checkBox233.Checked ? (int)numericUpDown49.Value : 0 },
                        new bool[] { checkBox232.CheckState == CheckState.Indeterminate, checkBox234.CheckState == CheckState.Indeterminate, checkBox233.CheckState == CheckState.Indeterminate },
                        new int[] { checkBox116.Checked ? (int)numericUpDown26.Value : 0, checkBox120.Checked ? (int)numericUpDown25.Value : 0, checkBox119.Checked ? (int)numericUpDown23.Value : 0 },
                        new bool[] { checkBox116.CheckState == CheckState.Indeterminate, checkBox120.CheckState == CheckState.Indeterminate, checkBox119.CheckState == CheckState.Indeterminate },
                        checkBox115.Checked ? presets[comboBox26.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                        checkBox49.Checked ? (presets[comboBox40.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox40.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                        checkBox109.Checked ? presets[comboBox27.SelectedIndex].AdjustIDsSpecifiedValues : new List<int>(),
                        checkBox72.Checked, checkBox73.Checked, checkBox71.Checked
                    );
            }
            #endregion
        }
        void WriteSpecialObjectsCopyPasteAutomation()
        {
            #region Text Objects
            if (checkBox81.Checked && presets[comboBox9.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Used Color IDs
            {
                if (checkBox84.Checked && presets[comboBox8.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                    InjectTextObjectsCopyPasteAutomation1
                    (
                        textBox3.Text, customVariableNames.ToArray(), customVariablesInitialValues.ToArray(), customVariablesAdjustments.ToArray(),
                        presets[comboBox8.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (!checkBox84.Checked || presets[comboBox8.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                    InjectTextObjectsCopyPasteAutomation4
                    (
                        textBox3.Text, customVariableNames.ToArray(), customVariablesInitialValues.ToArray(), customVariablesAdjustments.ToArray(),
                        checkBox84.Checked ? presets[comboBox8.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox8.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (presets[comboBox8.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                    InjectTextObjectsCopyPasteAutomation7
                    (
                        textBox3.Text, customVariableNames.ToArray(), customVariablesInitialValues.ToArray(), customVariablesAdjustments.ToArray(),
                        presets[comboBox8.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox8.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
            }
            else if (!checkBox81.Checked || presets[comboBox9.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Used Color IDs
            {
                if (checkBox84.Checked && presets[comboBox8.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                    InjectTextObjectsCopyPasteAutomation2
                    (
                        textBox3.Text, customVariableNames.ToArray(), customVariablesInitialValues.ToArray(), customVariablesAdjustments.ToArray(),
                        presets[comboBox8.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox81.Checked ? presets[comboBox9.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (!checkBox84.Checked || presets[comboBox8.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                    InjectTextObjectsCopyPasteAutomation5
                    (
                        textBox3.Text, customVariableNames.ToArray(), customVariablesInitialValues.ToArray(), customVariablesAdjustments.ToArray(),
                        checkBox84.Checked ? presets[comboBox8.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox8.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox81.Checked ? presets[comboBox9.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (presets[comboBox8.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                    InjectTextObjectsCopyPasteAutomation8
                    (
                        textBox3.Text, customVariableNames.ToArray(), customVariablesInitialValues.ToArray(), customVariablesAdjustments.ToArray(),
                        presets[comboBox8.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox8.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox81.Checked ? presets[comboBox9.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
            }
            else if (presets[comboBox9.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Main Color IDs
            {
                if (checkBox84.Checked && presets[comboBox8.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                    InjectTextObjectsCopyPasteAutomation3
                    (
                        textBox3.Text, customVariableNames.ToArray(), customVariablesInitialValues.ToArray(), customVariablesAdjustments.ToArray(),
                        presets[comboBox8.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        presets[comboBox9.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (!checkBox84.Checked || presets[comboBox8.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                    InjectTextObjectsCopyPasteAutomation6
                    (
                        textBox3.Text, customVariableNames.ToArray(), customVariablesInitialValues.ToArray(), customVariablesAdjustments.ToArray(),
                        checkBox84.Checked ? presets[comboBox8.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox8.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        presets[comboBox9.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (presets[comboBox8.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                    InjectTextObjectsCopyPasteAutomation9
                    (
                        textBox3.Text, customVariableNames.ToArray(), customVariablesInitialValues.ToArray(), customVariablesAdjustments.ToArray(),
                        presets[comboBox8.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox8.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        presets[comboBox9.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox100.Checked ? (presets[comboBox19.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox19.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
            }
            #endregion
            #region Pickup Objects
            if (checkBox130.Checked && presets[comboBox14.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Item IDs
            {
                if (checkBox154.Checked && presets[comboBox33.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group IDs
                {
                    if (checkBox133.Checked && presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                        InjectPickupItemsCopyPasteAutomation1
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0
                            
                        );
                    else if (!checkBox133.Checked || presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                        InjectPickupItemsCopyPasteAutomation2
                        (
                            checkBox133.Checked ? presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0
                            
                        );
                    else if (presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                        InjectPickupItemsCopyPasteAutomation3
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0
                            
                        );
                }
                else if (!checkBox154.Checked || presets[comboBox33.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group IDs
                {
                    if (checkBox133.Checked && presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                        InjectPickupItemsCopyPasteAutomation4
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox154.Checked ? presets[comboBox33.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0
                            
                        );
                    else if (!checkBox133.Checked || presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                        InjectPickupItemsCopyPasteAutomation5
                        (
                            checkBox133.Checked ? presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox154.Checked ? presets[comboBox33.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0
                            
                        );
                    else if (presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                        InjectPickupItemsCopyPasteAutomation6
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox154.Checked ? presets[comboBox33.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0
                            
                        );
                }
                else if (presets[comboBox33.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group IDs
                {
                    if (checkBox133.Checked && presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                        InjectPickupItemsCopyPasteAutomation7
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            presets[comboBox33.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0
                            
                        );
                    else if (!checkBox133.Checked || presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                        InjectPickupItemsCopyPasteAutomation8
                        (
                            checkBox133.Checked ? presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            presets[comboBox33.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0
                            
                        );
                    else if (presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                        InjectPickupItemsCopyPasteAutomation9
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            presets[comboBox33.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0
                            
                        );
                }
            }
            else if (!checkBox130.Checked || presets[comboBox14.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Item IDs
            {
                if (checkBox154.Checked && presets[comboBox33.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group IDs
                {
                    if (checkBox133.Checked && presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                        InjectPickupItemsCopyPasteAutomation10
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox130.Checked ? presets[comboBox14.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (!checkBox133.Checked || presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                        InjectPickupItemsCopyPasteAutomation11
                        (
                            checkBox133.Checked ? presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox130.Checked ? presets[comboBox14.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                        InjectPickupItemsCopyPasteAutomation12
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox130.Checked ? presets[comboBox14.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                }
                else if (!checkBox154.Checked || presets[comboBox33.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group IDs
                {
                    if (checkBox133.Checked && presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                        InjectPickupItemsCopyPasteAutomation13
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox154.Checked ? presets[comboBox33.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox130.Checked ? presets[comboBox14.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (!checkBox133.Checked || presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                        InjectPickupItemsCopyPasteAutomation14
                        (
                            checkBox133.Checked ? presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox154.Checked ? presets[comboBox33.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox130.Checked ? presets[comboBox14.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                        InjectPickupItemsCopyPasteAutomation15
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox154.Checked ? presets[comboBox33.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox130.Checked ? presets[comboBox14.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                }
                else if (presets[comboBox33.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group IDs
                {
                    if (checkBox133.Checked && presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                        InjectPickupItemsCopyPasteAutomation16
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            presets[comboBox33.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox130.Checked ? presets[comboBox14.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (!checkBox133.Checked || presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                        InjectPickupItemsCopyPasteAutomation17
                        (
                            checkBox133.Checked ? presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            presets[comboBox33.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox130.Checked ? presets[comboBox14.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                    else if (presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                        InjectPickupItemsCopyPasteAutomation18
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            presets[comboBox33.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            checkBox130.Checked ? presets[comboBox14.SelectedIndex].AdjustIDsAdjustment : 0
                        );
                }
            }
            else if (presets[comboBox14.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Item IDs
            {
                if (checkBox154.Checked && presets[comboBox33.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group IDs
                {
                    if (checkBox133.Checked && presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                        InjectPickupItemsCopyPasteAutomation19
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            presets[comboBox14.SelectedIndex].AutoAddGroupIDsSpecifiedValues
                        );
                    else if (!checkBox133.Checked || presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                        InjectPickupItemsCopyPasteAutomation20
                        (
                            checkBox133.Checked ? presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            presets[comboBox14.SelectedIndex].AutoAddGroupIDsSpecifiedValues
                        );
                    else if (presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                        InjectPickupItemsCopyPasteAutomation21
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            presets[comboBox14.SelectedIndex].AutoAddGroupIDsSpecifiedValues
                        );
                }
                else if (!checkBox154.Checked || presets[comboBox33.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group IDs
                {
                    if (checkBox133.Checked && presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                        InjectPickupItemsCopyPasteAutomation22
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox154.Checked ? presets[comboBox33.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            presets[comboBox14.SelectedIndex].AutoAddGroupIDsSpecifiedValues
                        );
                    else if (!checkBox133.Checked || presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                        InjectPickupItemsCopyPasteAutomation23
                        (
                            checkBox133.Checked ? presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox154.Checked ? presets[comboBox33.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            presets[comboBox14.SelectedIndex].AutoAddGroupIDsSpecifiedValues
                        );
                    else if (presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                        InjectPickupItemsCopyPasteAutomation24
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            checkBox154.Checked ? presets[comboBox33.SelectedIndex].AdjustIDsAdjustment : 0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            presets[comboBox14.SelectedIndex].AutoAddGroupIDsSpecifiedValues
                        );
                }
                else if (presets[comboBox33.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Target Group IDs
                {
                    if (checkBox133.Checked && presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                        InjectPickupItemsCopyPasteAutomation25
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            presets[comboBox33.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            presets[comboBox14.SelectedIndex].AutoAddGroupIDsSpecifiedValues
                        );
                    else if (!checkBox133.Checked || presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                        InjectPickupItemsCopyPasteAutomation26
                        (
                            checkBox133.Checked ? presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            presets[comboBox33.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            presets[comboBox14.SelectedIndex].AutoAddGroupIDsSpecifiedValues
                        );
                    else if (presets[comboBox15.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                        InjectPickupItemsCopyPasteAutomation27
                        (
                            presets[comboBox15.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox15.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                            ToInt32(radioButton11.Checked), checkBox152.Checked, checkBox153.Checked, checkBox148.Checked, checkBox228.Checked,
                            presets[comboBox33.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveX : (float)0) : (float)0,
                            checkBox134.Checked ? (presets[comboBox13.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox13.SelectedIndex].AutoCopyPasteMoveY : (float)0) : (float)0,
                            presets[comboBox14.SelectedIndex].AutoAddGroupIDsSpecifiedValues
                        );
                }
            }
            #endregion
            #region Collision Blocks
            if (checkBox166.Checked && presets[comboBox70.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Block IDs
            {
                if (checkBox220.Checked && presets[comboBox69.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                    InjectCollisionBlocksCopyPasteAutomation3
                    (
                        presets[comboBox69.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox219.Checked, checkBox167.Checked,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (!checkBox220.Checked || presets[comboBox69.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                    InjectCollisionBlocksCopyPasteAutomation1
                    (
                        checkBox220.Checked ? presets[comboBox69.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox69.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox219.Checked, checkBox167.Checked,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (presets[comboBox69.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                    InjectCollisionBlocksCopyPasteAutomation2
                    (
                        presets[comboBox69.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox69.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox219.Checked, checkBox167.Checked,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
            }
            else if (!checkBox166.Checked || presets[comboBox70.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Block IDs
            {
                if (checkBox220.Checked && presets[comboBox69.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                    InjectCollisionBlocksCopyPasteAutomation6
                    (
                        presets[comboBox69.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox166.Checked ? presets[comboBox70.SelectedIndex].AdjustIDsAdjustment : 0, checkBox219.Checked, checkBox167.Checked,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (!checkBox220.Checked || presets[comboBox69.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                    InjectCollisionBlocksCopyPasteAutomation4
                    (
                        checkBox220.Checked ? presets[comboBox69.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox69.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox166.Checked ? presets[comboBox70.SelectedIndex].AdjustIDsAdjustment : 0, checkBox219.Checked, checkBox167.Checked,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (presets[comboBox69.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                    InjectCollisionBlocksCopyPasteAutomation5
                    (
                        presets[comboBox69.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox69.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox166.Checked ? presets[comboBox70.SelectedIndex].AdjustIDsAdjustment : 0, checkBox219.Checked, checkBox167.Checked,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
            }
            else if (presets[comboBox70.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Main Block IDs
            {
                if (checkBox220.Checked && presets[comboBox69.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                    InjectCollisionBlocksCopyPasteAutomation9
                    (
                        presets[comboBox69.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        presets[comboBox70.SelectedIndex].AdjustIDsSpecifiedValues, checkBox219.Checked, checkBox167.Checked,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (!checkBox220.Checked || presets[comboBox69.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                    InjectCollisionBlocksCopyPasteAutomation7
                    (
                        checkBox220.Checked ? presets[comboBox69.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox69.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        presets[comboBox70.SelectedIndex].AdjustIDsSpecifiedValues, checkBox219.Checked, checkBox167.Checked,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (presets[comboBox69.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                    InjectCollisionBlocksCopyPasteAutomation8
                    (
                        presets[comboBox69.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox69.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        presets[comboBox70.SelectedIndex].AdjustIDsSpecifiedValues, checkBox219.Checked, checkBox167.Checked,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox224.Checked ? (presets[comboBox68.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox68.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
            }
            #endregion
            #region Trigger Orbs
            if (checkBox138.Checked && presets[comboBox10.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Target Group IDs
            {
                if (checkBox131.Checked && presets[comboBox11.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                    InjectTriggerOrbsCopyPasteAutomation3
                    (
                        presets[comboBox11.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox137.Checked, checkBox135.Checked,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (!checkBox131.Checked || presets[comboBox11.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                    InjectTriggerOrbsCopyPasteAutomation1
                    (
                        checkBox131.Checked ? presets[comboBox11.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox11.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox137.Checked, checkBox135.Checked,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (presets[comboBox11.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                    InjectTriggerOrbsCopyPasteAutomation2
                    (
                        presets[comboBox11.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox11.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox137.Checked, checkBox135.Checked,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
            }
            else if (!checkBox138.Checked || presets[comboBox10.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Target Group IDs
            {
                if (checkBox131.Checked && presets[comboBox11.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                    InjectTriggerOrbsCopyPasteAutomation6
                    (
                        presets[comboBox11.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox138.Checked ? presets[comboBox10.SelectedIndex].AdjustIDsAdjustment : 0, checkBox137.Checked, checkBox135.Checked,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (!checkBox131.Checked || presets[comboBox11.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                    InjectTriggerOrbsCopyPasteAutomation4
                    (
                        checkBox131.Checked ? presets[comboBox11.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox11.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox138.Checked ? presets[comboBox10.SelectedIndex].AdjustIDsAdjustment : 0, checkBox137.Checked, checkBox135.Checked,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (presets[comboBox11.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                    InjectTriggerOrbsCopyPasteAutomation5
                    (
                        presets[comboBox11.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox11.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox138.Checked ? presets[comboBox10.SelectedIndex].AdjustIDsAdjustment : 0, checkBox137.Checked, checkBox135.Checked,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
            }
            else if (presets[comboBox10.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Main Target Group IDs
            {
                if (checkBox131.Checked && presets[comboBox11.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                    InjectTriggerOrbsCopyPasteAutomation9
                    (
                        presets[comboBox11.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        presets[comboBox10.SelectedIndex].AdjustIDsSpecifiedValues, checkBox137.Checked, checkBox135.Checked,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (!checkBox131.Checked || presets[comboBox11.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                    InjectTriggerOrbsCopyPasteAutomation7
                    (
                        checkBox131.Checked ? presets[comboBox11.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox11.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        presets[comboBox10.SelectedIndex].AdjustIDsSpecifiedValues, checkBox137.Checked, checkBox135.Checked,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (presets[comboBox11.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                    InjectTriggerOrbsCopyPasteAutomation8
                    (
                        presets[comboBox11.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox11.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        presets[comboBox10.SelectedIndex].AdjustIDsSpecifiedValues, checkBox137.Checked, checkBox135.Checked,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox132.Checked ? (presets[comboBox12.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox12.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
            }
            #endregion
            #region Pulsating Animation Objects
            if (checkBox139.Checked && presets[comboBox18.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                InjectPulsatingAnimationObjectsCopyPasteAutomation3
                (
                    presets[comboBox18.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                    checkBox214.Checked, checkBox213.Checked, checkBox215.Checked ? (float)numericUpDown45.Value : 0,
                    checkBox144.Checked ? (presets[comboBox16.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox16.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox144.Checked ? (presets[comboBox16.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox16.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                    
                );
            else if (!checkBox139.Checked || presets[comboBox18.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                InjectPulsatingAnimationObjectsCopyPasteAutomation1
                (
                    checkBox139.Checked ? presets[comboBox18.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox18.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                    checkBox214.Checked, checkBox213.Checked, checkBox215.Checked ? (float)numericUpDown45.Value : 0,
                    checkBox144.Checked ? (presets[comboBox16.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox16.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox144.Checked ? (presets[comboBox16.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox16.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                    
                );
            else if (presets[comboBox18.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                InjectPulsatingAnimationObjectsCopyPasteAutomation2
                (
                    presets[comboBox18.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox18.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                    checkBox214.Checked, checkBox213.Checked, checkBox215.Checked ? (float)numericUpDown45.Value : 0,
                    checkBox144.Checked ? (presets[comboBox16.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox16.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox144.Checked ? (presets[comboBox16.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox16.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                    
                );
            #endregion
            #region Manipulation Portals
            if (checkBox221.Checked && presets[comboBox65.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                InjectManipulationPortalsCopyPasteAutomation3
                (
                    presets[comboBox65.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox218.Checked, checkBox217.Checked, 
                    checkBox222.Checked ? (presets[comboBox64.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox64.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox222.Checked ? (presets[comboBox64.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox64.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                    
                );
            else if (!checkBox221.Checked || presets[comboBox65.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                InjectManipulationPortalsCopyPasteAutomation1
                (
                    checkBox221.Checked ? presets[comboBox65.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox65.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox218.Checked, checkBox217.Checked, 
                    checkBox222.Checked ? (presets[comboBox64.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox64.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox222.Checked ? (presets[comboBox64.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox64.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                    
                );
            else if (presets[comboBox65.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                InjectManipulationPortalsCopyPasteAutomation2
                (
                    presets[comboBox65.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox65.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox218.Checked, checkBox217.Checked, 
                    checkBox222.Checked ? (presets[comboBox64.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox64.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox222.Checked ? (presets[comboBox64.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox64.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                    
                );
            #endregion
            #region Speed Portals
            if (checkBox226.Checked && presets[comboBox67.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                InjectSpeedPortalsCopyPasteAutomation3
                (
                    presets[comboBox67.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox223.Checked, checkBox216.Checked,
                    checkBox227.Checked ? (presets[comboBox66.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox66.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox227.Checked ? (presets[comboBox66.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox66.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                    
                );
            else if (!checkBox226.Checked || presets[comboBox67.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                InjectSpeedPortalsCopyPasteAutomation1
                (
                    checkBox226.Checked ? presets[comboBox67.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox67.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox223.Checked, checkBox216.Checked,
                    checkBox227.Checked ? (presets[comboBox66.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox66.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox227.Checked ? (presets[comboBox66.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox66.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                    
                );
            else if (presets[comboBox67.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                InjectSpeedPortalsCopyPasteAutomation2
                (
                    presets[comboBox67.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox67.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox223.Checked, checkBox216.Checked,
                    checkBox227.Checked ? (presets[comboBox66.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox66.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox227.Checked ? (presets[comboBox66.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox66.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                    
                );
            #endregion
            #region Orbs
            if (checkBox149.Checked && presets[comboBox67.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                InjectOrbsCopyPasteAutomation3
                (
                    presets[comboBox67.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox150.Checked, checkBox145.Checked,
                    checkBox151.Checked ? (presets[comboBox66.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox66.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox151.Checked ? (presets[comboBox66.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox66.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                    
                );
            else if (!checkBox149.Checked || presets[comboBox67.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                InjectOrbsCopyPasteAutomation1
                (
                    checkBox149.Checked ? presets[comboBox67.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox67.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox150.Checked, checkBox145.Checked,
                    checkBox151.Checked ? (presets[comboBox66.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox66.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox151.Checked ? (presets[comboBox66.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox66.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                    
                );
            else if (presets[comboBox67.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                InjectOrbsCopyPasteAutomation2
                (
                    presets[comboBox67.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox67.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs, checkBox150.Checked, checkBox145.Checked,
                    checkBox151.Checked ? (presets[comboBox66.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox66.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                    checkBox151.Checked ? (presets[comboBox66.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox66.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                    
                );
            #endregion
            #region Count Objects
            if (checkBox225.Checked && presets[comboBox71.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Item IDs
            {
                if (checkBox230.Checked && presets[comboBox73.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                    InjectCountObjectsCopyPasteAutomation3
                    (
                        presets[comboBox73.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (!checkBox230.Checked || presets[comboBox73.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                    InjectCountObjectsCopyPasteAutomation1
                    (
                        checkBox230.Checked ? presets[comboBox73.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox73.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (presets[comboBox73.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                    InjectCountObjectsCopyPasteAutomation2
                    (
                        presets[comboBox73.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox73.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
            }
            else if (!checkBox225.Checked || presets[comboBox71.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Item IDs
            {
                if (checkBox230.Checked && presets[comboBox73.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                    InjectCountObjectsCopyPasteAutomation6
                    (
                        presets[comboBox73.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox225.Checked ? presets[comboBox71.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (!checkBox230.Checked || presets[comboBox73.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                    InjectCountObjectsCopyPasteAutomation4
                    (
                        checkBox230.Checked ? presets[comboBox73.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox73.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox225.Checked ? presets[comboBox71.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (presets[comboBox73.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                    InjectCountObjectsCopyPasteAutomation5
                    (
                        presets[comboBox73.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox73.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        checkBox225.Checked ? presets[comboBox71.SelectedIndex].AdjustIDsAdjustment : 0,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
            }
            else if (presets[comboBox71.SelectedIndex].AdjustIDAdjustmentMode == AdjustmentMode.SpecificValues) // Item IDs
            {
                if (checkBox230.Checked && presets[comboBox73.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.UnusedIDs) // Group IDs
                    InjectCountObjectsCopyPasteAutomation9
                    (
                        presets[comboBox73.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        presets[comboBox71.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (!checkBox230.Checked || presets[comboBox73.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.FlatAdjustment) // Group IDs
                    InjectCountObjectsCopyPasteAutomation7
                    (
                        checkBox230.Checked ? presets[comboBox73.SelectedIndex].AutoAddGroupIDsAdjustment : 0, presets[comboBox73.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        presets[comboBox71.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
                else if (presets[comboBox73.SelectedIndex].AutoAddGroupIDAdjustmentMode == AdjustmentMode.SpecificValues) // Group IDs
                    InjectCountObjectsCopyPasteAutomation8
                    (
                        presets[comboBox73.SelectedIndex].AutoAddGroupIDsSpecifiedValues, presets[comboBox73.SelectedIndex].AutoAddGroupIDsAdjustedGroupIDs,
                        presets[comboBox71.SelectedIndex].AutoAddGroupIDsSpecifiedValues,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveXEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveX : 0) : 0,
                        checkBox231.Checked ? (presets[comboBox72.SelectedIndex].AutoCopyPasteMoveYEnabled ? presets[comboBox72.SelectedIndex].AutoCopyPasteMoveY : 0) : 0
                        
                    );
            }
            #endregion
            #region Rotating Objects
            if (radioButton8.Checked)
                InjectRotatingObjectsCopyPasteAutomation1((int)numericUpDown42.Value, checkBox129.Checked);
            else
                InjectRotatingObjectsCopyPasteAutomation2(radioButton2.Checked, checkBox229.Checked);
            #endregion
        }
        void WriteObjectsCopyPasteAutomation()
        {
            WriteGeneralObjectCopyPasteAutomation
            (
                GetObjectIDList(),
                hue1Adj, sat1Adj, val1Adj,
                hue2Adj, sat2Adj, val2Adj,
                rotationAdj, scalingAdj,
                groupIDPresetIndex > -1 ? presets[groupIDPresetIndex].AutoAddGroupIDsAdjustedGroupIDs : new bool[10],
                GetGroupIDList(), GetColor1IDList(), GetColor2IDList(),
                autoCopyPastePresetIndex > -1 ? (presets[autoCopyPastePresetIndex].AutoCopyPasteMoveXEnabled ? presets[autoCopyPastePresetIndex].AutoCopyPasteMoveX : 0) : 0,
                autoCopyPastePresetIndex > -1 ? (presets[autoCopyPastePresetIndex].AutoCopyPasteMoveYEnabled ? presets[autoCopyPastePresetIndex].AutoCopyPasteMoveY : 0) : 0,
                ZOrderAdj, ZLayerAdj,
                EL1Adj, EL2Adj,
                mainColorIDPresetIndex > -1 ? presets[mainColorIDPresetIndex].AdjustIDAdjustmentMode : AdjustmentMode.FlatAdjustment,
                detailColorIDPresetIndex > -1 ? presets[detailColorIDPresetIndex].AdjustIDAdjustmentMode : AdjustmentMode.FlatAdjustment,
                groupIDPresetIndex > -1 ? presets[groupIDPresetIndex].AutoAddGroupIDAdjustmentMode : AdjustmentMode.FlatAdjustment
            );
        }
        List<int> GetObjectIDList()
        {
            if (applyForAllObjects)
                return new List<int> { -1 };
            else if (applyForSpecifiedObjectIDs)
                return listBox1.Items.ToInt32List();
            else
                return GetCurrentlySelectedObjectIDs();
        }
        List<int> GetColor1IDList()
        {
            if (mainColorIDPresetIndex > -1)
                switch (presets[mainColorIDPresetIndex].AdjustIDAdjustmentMode)
                {
                    case AdjustmentMode.UnusedIDs:
                        return new List<int> { -1 };
                    case AdjustmentMode.FlatAdjustment:
                        return new List<int> { presets[mainColorIDPresetIndex].AdjustIDsAdjustment };
                    case AdjustmentMode.SpecificValues:
                        return presets[mainColorIDPresetIndex].AdjustIDsSpecifiedValues;
                }
            return new List<int> { 0 };
        }
        List<int> GetColor2IDList()
        {
            if (detailColorIDPresetIndex > -1)
                switch (presets[detailColorIDPresetIndex].AdjustIDAdjustmentMode)
                {
                    case AdjustmentMode.UnusedIDs:
                        return new List<int> { -1 };
                    case AdjustmentMode.FlatAdjustment:
                        return new List<int> { presets[detailColorIDPresetIndex].AdjustIDsAdjustment };
                    case AdjustmentMode.SpecificValues:
                        return presets[detailColorIDPresetIndex].AdjustIDsSpecifiedValues;
                }
            return new List<int> { 0 };
        }
        List<int> GetGroupIDList()
        {
            if (groupIDPresetIndex > -1)
                switch (presets[groupIDPresetIndex].AutoAddGroupIDAdjustmentMode)
                {
                    case AdjustmentMode.UnusedIDs:
                        return new List<int> { -1 };
                    case AdjustmentMode.FlatAdjustment:
                        return new List<int> { presets[groupIDPresetIndex].AutoAddGroupIDsAdjustment };
                    case AdjustmentMode.SpecificValues:
                        return presets[groupIDPresetIndex].AutoAddGroupIDsSpecifiedValues;
                }
            return new List<int> { 0 };
        }
    }
}