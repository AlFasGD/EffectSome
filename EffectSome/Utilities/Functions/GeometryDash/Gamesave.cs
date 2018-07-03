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
using System.IO.Compression;
using static EffectSome.EffectSome;
using static EffectSome.LevelObject;
using static System.Convert;

namespace EffectSome
{
    public static partial class Gamesave
    {
        public static readonly int[] triggerList = { 29, 30, 32, 33, 105, 744, 899, 900, 901, 915, 1006, 1007, 1049, 1268, 1346, 1347, 1520, 1585, 1595, 1611, 1612, 1613, 1616, 1811, 1812, 1814, 1815, 1817, 1818, 1819 };
        public static readonly int[] manipulationPortalList = { 13, 47, 111, 1331 };
        public static readonly int[] speedPortalList = { 200, 201, 202, 203, 1334 };
        public static readonly int[] orbList = { 36, 84, 141, 1022, 1330, 1333, 1704, 1751 };
        public static readonly int[] pickupItemList = { 1275, 1587, 1589, 1598, 1614 };
        public static readonly int[] pulsatingObjectList = { 1839, 1840, 1841, 1842 };
        public static readonly int[] rotatingObjectList = { 85, 86, 87, 97, 137, 138, 139, 154, 155, 156, 180, 181, 182, 183, 184, 185, 186, 187, 188, 222, 223, 224, 375, 376, 377, 378, 394, 395, 396, 678, 679, 680, 740, 741, 742, 997, 998, 999, 1000, 1019, 1020, 1021, 1055, 1056, 1057, 1058, 1059, 1060, 1061, 1521, 1522, 1523, 1524, 1525, 1526, 1527, 1528, 1582, 1619, 1620, 1705, 1706, 1707, 1708, 1709, 1710, 1734, 1735, 1736, 1752, 1831, 1832, 1833, 1834 };
        public static readonly int[] notGeneralObjectList = { 13, 29, 30, 32, 33, 36, 47, 84, 85, 86, 87, 97, 105, 111, 137, 138, 139, 141, 154, 155, 156, 180, 181, 182, 183, 184, 185, 186, 187, 188, 200, 201, 202, 203, 222, 223, 224, 375, 376, 377, 378, 394, 395, 396, 678, 679, 680, 740, 741, 742, 744, 899, 900, 901, 915, 997, 998, 999, 1000, 1006, 1007, 1019, 1020, 1021, 1022, 1049, 1055, 1056, 1057, 1058, 1059, 1060, 1061, 1268, 1275, 1330, 1331, 1333, 1334, 1346, 1347, 1520, 1521, 1522, 1523, 1524, 1525, 1526, 1527, 1528, 1582, 1585, 1587, 1589, 1595, 1598, 1611, 1612, 1613, 1614, 1616, 1619, 1620, 1704, 1705, 1706, 1707, 1708, 1709, 1710, 1734, 1735, 1736, 1751, 1752, 1811, 1812, 1814, 1815, 1817, 1818, 1819, 1831, 1832, 1833, 1834, 1839, 1840, 1841, 1842 };
        public const int totalDifferentObjectCount = 1607;
        public static readonly int[] versionObjectIDLimits = { 41, 46, 47, 75, 105, 141, 199, 285, 505, 744, 1329, 1911 };
        public static readonly int[] versionParameterIDLimits = { 10, 10, 10, 10, 10, 11, 13, 17, 17, 20, 66, 107 };
        public static readonly int[] levelStringkALimits = { 1, 1, 1, 1, 1, 1, 1, 7, 10, 16, 18, 18 };
        public const string defaultLevelString = "kS38,1_40_2_125_3_255_11_255_12_255_13_255_4_-1_6_1000_7_1_15_1_18_0_8_1|1_0_2_102_3_255_11_255_12_255_13_255_4_-1_6_1001_7_1_15_1_18_0_8_1|1_0_2_102_3_255_11_255_12_255_13_255_4_-1_6_1009_7_1_15_1_18_0_8_1|1_255_2_255_3_255_11_255_12_255_13_255_4_-1_6_1002_5_1_7_1_15_1_18_0_8_1|1_255_2_0_3_0_11_255_12_255_13_255_4_-1_6_1005_5_1_7_1_15_1_18_0_8_1|1_255_2_255_3_255_11_255_12_255_13_255_4_-1_6_1006_5_1_7_1_15_1_18_0_8_1|,kA13,0,kA15,0,kA16,0,kA14,,kA6,0,kA7,0,kA17,0,kA18,0,kS39,0,kA2,0,kA3,0,kA8,0,kA4,0,kA9,0,kA10,0,kA11,0;";
        public const string levelDataStart = "<?xml version=\"1.0\"?><plist version=\"1.0\" gjver=\"2.0\"><dict><k>LLM_01</k><d><k>_isArr</k><t />";
        public const string levelDataEnd = "</d><k>LLM_02</k><i>33</i></dict></plist>";
        public const string defaultLevelData = "<?xml version=\"1.0\"?><plist version=\"1.0\" gjver=\"2.0\"><dict><k>LLM_01</k><d><k>_isArr</k><t /></d><k>LLM_02</k><i>33</i></dict></plist>";
        public static readonly string[] levelLengthNames = { "Tiny", "Small", "Medium", "Long", "XL" };
        public static readonly int[] intParameters = { 1, 7, 8, 9, 12, 20, 21, 22, 23, 24, 25, 30, 49, 50, 51, 52, 61, 68, 69, 71, 76, 77, 79, 80, 81, 82, 88, 95, 97, 101, 108 };
        public static readonly int[] doubleParameters = { 2, 3, 6, 10, 28, 29, 32, 35, 45, 46, 47, 54, 60, 63, 72, 73, 75, 84, 85, 90, 91, 92, 105, 107 };
        public static readonly int[] boolParameters = { 4, 5, 11, 13, 15, 16, 17, 34, 36, 41, 42, 56, 58, 59, 62, 64, 65, 66, 67, 70, 78, 86, 87, 89, 93, 94, 96, 98, 99, 100, 102, 103 };
        public static readonly int[] stringParameters = { 31 };
        public static readonly int[] intArrayParameters = { 57 };
        public static readonly int[] HSVParameters = { 43, 44, 49 };
        public const double unitsPerSecond = 360; // The units per second a player moves at a speed with 1.0 speed multiplier - Still under testing
        public static readonly double[] speedMultipliers = { 0.7, 0.9, 1.1, 1.3, 1.6 };
        /// <summary>This enumeration provides values representing the length of a level.</summary>
        public enum LevelLength
        {
            /// <summary>Represents the Tiny length of a level. Levels with Tiny length are less than 10 seconds long.</summary>
            Tiny,
            /// <summary>Represents the Small length of a level. Levels with Small length are 10 to 29 seconds long.</summary>
            Small,
            /// <summary>Represents the Medium length of a level. Levels with Medium length are 30 to 59 seconds long.</summary>
            Medium,
            /// <summary>Represents the Long length of a level. Levels with Long length are 60 to 119 seconds long.</summary>
            Long,
            /// <summary>Represents the XL length of a level. Levels with XL length are at least 120 seconds long.</summary>
            XL
        }

        /// <summary>Returns the decrypted version of the gamesave after checking whether the gamesave is encrypted or not. Returns true if the gamesave is encrypted; otherwise false.</summary>
        /// <param name="decrypted">The string to return the decrypted gamesave.</param>
        /// <returns>Returns true if the gamesave is encrypted; otherwise false.</returns>
        public static bool TryDecryptGamesave(out string decrypted)
        {
            string path = GDLocalData + "\\CCGameManager.dat";
            string gamesave = "";
            bool isEncrypted = CheckIfGamesaveIsEncrypted();
            if (isEncrypted)
                gamesave = DecryptGamesave();
            else
                gamesave = Encoding.UTF8.GetString(File.ReadAllBytes(path));
            decrypted = gamesave;
            doneDecryptingGamesave = true;
            return isEncrypted;
        }
        public static bool CheckIfGamesaveIsEncrypted()
        {
            string path = GDLocalData + "\\CCGameManager.dat";
            string gamesave = Encoding.UTF8.GetString(File.ReadAllBytes(path));
            int checks = 0;
            string[] tests = { "<k>bgVolume</k>", "<k>sfxVolume</k>", "<k>playerUDID</k>", "<k>playerName</k>", "<k>playerUserID</k>" };
            for (int i = 0; i < tests.Length; i++)
                if (gamesave.Contains(tests[i]))
                    checks++;
            return checks != tests.Length;
        }
        public static string DecryptGamesave()
        {
            string path = GDLocalData + "\\CCGameManager.dat"; // Get the path
            string gamesave = GetData(path); // Get the gamesave data
            return GDGamesaveDecrypt(gamesave);
        }

        /// <summary>Returns the decrypted version of the level data after checking whether the level data is encrypted or not. Returns true if the level data is encrypted; otherwise false.</summary>
        /// <param name="decrypted">The string to return the decrypted level data.</param>
        /// <returns>Returns true if the level data is encrypted; otherwise false.</returns>
        public static bool TryDecryptLevelData(out string decrypted)
        {
            string path = GDLocalData + "\\CCLocalLevels.dat";
            string levelData = "";
            bool isEncrypted = CheckIfLevelDataIsEncrypted();
            if (isEncrypted)
                levelData = DecryptLevelData();
            else
                levelData = Encoding.UTF8.GetString(File.ReadAllBytes(path));
            decrypted = levelData;
            return isEncrypted;
        }
        public static bool CheckIfLevelDataIsEncrypted()
        {
            string path = GDLocalData + "\\CCLocalLevels.dat";
            string levelData = Encoding.UTF8.GetString(File.ReadAllBytes(path));
            string test = "<?xml version=\"1.0\"?><plist version=\"1.0\" gjver=\"2.0\">";
            return !levelData.Contains(test);
        }
        public static string DecryptLevelData()
        {
            string path = GDLocalData + "\\CCLocalLevels.dat"; // Get the path
            string levelData = GetData(path); // Get the gamesave data
            return GDGamesaveDecrypt(levelData);
        }

        /// <summary>Returns the decrypted version of the level string after checking whether the level string is encrypted or not. Returns true if the level string is encrypted; otherwise false.</summary>
        /// <param name="decrypted">The string to return the decrypted level string.</param>
        /// <returns>Returns true if the level string is encrypted; otherwise false.</returns>
        public static bool TryDecryptLevelString(int index, out string decrypted)
        {
            bool isEncrypted = CheckIfLevelStringIsEncrypted(index, out decrypted);
            if (isEncrypted)
                decrypted = DecryptLevelString(decrypted);
            return isEncrypted;
        }
        public static bool TryDecryptLevelString(string ls, out string decrypted)
        {
            bool isEncrypted = CheckIfLevelStringIsEncrypted(ls, out decrypted);
            if (isEncrypted)
                decrypted = DecryptLevelString(decrypted);
            return isEncrypted;
        }
        public static bool CheckIfLevelStringIsEncrypted(int index, out string levelString)
        {
            levelString = UserLevels[index].LevelString;
            int checks = 0;
            string[] tests = { "kA13,", "kA15,", "kA16,", "kA14,", "kA6," };
            for (int i = 0; i < tests.Length; i++)
                if (UserLevels[index].LevelString.Contains(tests[i]))
                    checks++;
            return checks != tests.Length;
        }
        public static bool CheckIfLevelStringIsEncrypted(string ls, out string levelString)
        {
            levelString = ls;
            int checks = 0;
            string[] tests = { "kA13,", "kA15,", "kA16,", "kA14,", "kA6," };
            for (int i = 0; i < tests.Length; i++)
                if (levelString.Contains(tests[i]))
                    checks++;
            return checks != tests.Length;
        }
        public static string DecryptLevelString(int index)
        {
            return GDLevelStringDecrypt(UserLevels[index].LevelString);
        }
        public static string DecryptLevelString(string ls)
        {
            return GDLevelStringDecrypt(ls);
        }

        public static List<Guideline> GetGuidelines(string guidelineString)
        {
            List<Guideline> guidelines = new List<Guideline>();
            if (guidelineString != null && guidelineString != "")
            {
                guidelineString = guidelineString.Remove(guidelineString.Length - 1);
                string[] s = guidelineString.Split('~');
                for (int i = 0; i < s.Length; i += 2)
                    guidelines = guidelines.Add(ToDouble(s[i]), ToDouble(s[i + 1]));
            }
            return guidelines;
        }
        public static List<Guideline> RemoveDuplicatedGuidelines(List<Guideline> guidelines)
        {
            List<Guideline> g = new List<Guideline>();
            for (int i = 0; i < guidelines.Count; i++)
                if (!g.Contains(guidelines[i]))
                    g.Add(guidelines[i]);
            return g;
        }
        public static List<LevelObject> GetObjects(string objectString)
        {
            List<LevelObject> objects = new List<LevelObject>();
            while (objectString.Length > 0 && objectString[objectString.Length - 1] == ';')
                objectString = objectString.Remove(objectString.Length - 1);
            if (objectString.Length > 0)
            {
                string[,] objectParameters = objectString.Split(';').Split(',');
                int length0 = objectParameters.GetLength(0);
                int length1 = objectParameters.GetLength(1);
                for (int i = 0; i < length0; i++)
                {
                    objects.Add(new LevelObject(ToInt32(objectParameters[i, 1]), ToDouble(objectParameters[i, 3]), ToDouble(objectParameters[i, 5]))); // Get IDs of the selected objects
                    for (int j = 7; j < length1; j += 2)
                    {
                        if (objectParameters[i, j] != null)
                        {
                            try
                            {
                                int currentParameterID = ToInt32(objectParameters[i, j - 1]);
                                if (intParameters.Contains(currentParameterID))
                                    objects[objects.Count - 1].Parameters[currentParameterID] = ToInt32(objectParameters[i, j]);
                                else if (doubleParameters.Contains(currentParameterID))
                                    objects[objects.Count - 1].Parameters[currentParameterID] = ToDouble(objectParameters[i, j]);
                                else if (boolParameters.Contains(currentParameterID))
                                    objects[objects.Count - 1].Parameters[currentParameterID] = ToBoolean(ToInt32(objectParameters[i, j]));
                                else if (HSVParameters.Contains(currentParameterID))
                                {
                                    string[] values = objectParameters[i, j].ToString().Split('a');
                                    object[] HSVValues = { ToBoolean(ToInt32(values[0])), ToBoolean(ToInt32(values[1])), ToDouble(values[2]), ToDouble(values[3]), ToDouble(values[4]) };
                                    objects[objects.Count - 1].Parameters[currentParameterID] = HSVValues;
                                }
                                else if (currentParameterID == 31)
                                    objects[objects.Count - 1].Parameters[currentParameterID] = Encoding.ASCII.GetString(Base64Decrypt(objectParameters[i, j]));
                                else if (intArrayParameters.Contains(currentParameterID))
                                    objects[objects.Count - 1].Parameters[currentParameterID] = objectParameters[i, j].ToString().Split('.').ToInt32Array();
                            }
                            catch (FormatException) // If the parameter is not just a number; most likely a Start Pos object
                            {
                                // Something to do, I guess
                            }
                        }
                        else break;
                    }
                }
                objectParameters = null;
                //GC.Collect();
            }
            return objects;
        }
        public static LevelObject GetCommonAttributes(List<LevelObject> list, int objectID)
        {
            LevelObject commonAttributes = new LevelObject(objectID);
            for (int i = list.Count; i >= 0; i--)
                if ((int)list[i][ObjectParameter.ID] != objectID)
                    list.RemoveAt(i);
            if (list.Count > 1)
            {
                for (int i = 1; i < ParameterCount; i++)
                {
                    object commonAttribute = list[0].Parameters[i];
                    for (int j = 1; j < list.Count; j++)
                        if (commonAttribute != list[j].Parameters[i])
                        {
                            commonAttribute = null;
                            break;
                        }
                    commonAttributes.Parameters[i] = commonAttribute;
                }
                return commonAttributes;
            }
            else if (list.Count == 1)
                return list[0];
            else return null;
        }
        public static bool GetBoolKeyValue(int index, int key)
        {
            return DecryptedLevelData.Find("<k>k" + key.ToString() + "</k>", LevelKeyStartIndices[index], (index < UserLevelCount - 1) ? LevelKeyStartIndices[index + 1] : DecryptedLevelData.Find("</d></d></d>")) > -1;
        }
        public static bool GetBoolKeyValue(string level, int key)
        {
            return level.Find("<k>k" + key.ToString() + "</k>") > -1;
        }
        public static bool[] GetBoolKeyValues(int key)
        {
            bool[] result = new bool[UserLevelCount];
            for (int i = 0; i < UserLevelCount; i++)
                result[i] = DecryptedLevelData.Find("<k>k" + key.ToString() + "</k>", LevelKeyStartIndices[i], (i < UserLevelCount - 1) ? LevelKeyStartIndices[i + 1] : DecryptedLevelData.Find("</d></d></d>")) > -1;
            return result;
        }
        public static bool[] GetLevelVerifiedStatus()
        {
            return GetBoolKeyValues(14);
        }
        public static bool[] GetLevelUploadedStatus()
        {
            return GetBoolKeyValues(15);
        }
        public static int GetGuidelineStringStartIndex(string ls)
        {
            return ls.Find("kA14,") + 5;
        }
        public static int GetGuidelineStringStartIndex(int index)
        {
            return GetGuidelineStringStartIndex(UserLevels[index].DecryptedLevelString);
        }
        public static int GetLevelCount()
        {
            return DecryptedLevelData.FindAll("<k>k_").Length;
        }
        public static int GetLevelLength(int index)
        {
            try { return ToInt32(GetKeyValue(index, 23, "i")); }
            catch { return 0; }
        }
        public static int GetLevelRevision(int index)
        {
            return ToInt32(GetKeyValue(index, 46, "i"));
        }
        public static int GetNumberOfGuidelines(string guidelineString)
        {
            if (guidelineString != null && guidelineString != "")
            {
                guidelineString = guidelineString.Remove(guidelineString.Length - 1);
                string[] s = guidelineString.Split('~');
                return s.Length / 2;
            }
            else return 0;
        }
        public static int GetNumberOfGuidelines(int index)
        {
            return GetNumberOfGuidelines(GetGuidelineString(index));
        }
        public static int GetTriggerCount(List<LevelObject> l)
        {
            int count = 0;
            for (int i = 0; i < l.Count; i++)
                if (triggerList.Contains((int)(l[i][LevelObject.ObjectParameter.ID])))
                    count++;
            return count;
        }
        public static int GetDifferentObjectCount(List<LevelObject> l)
        {
            List<int> diffObjIDs = new List<int>();
            for (int i = 0; i < l.Count; i++)
                if (!diffObjIDs.Contains((int)(l[i][LevelObject.ObjectParameter.ID])))
                    diffObjIDs.Add((int)(l[i][LevelObject.ObjectParameter.ID]));
            return diffObjIDs.Count;
        }
        public static int GetUsedGroupCounts(List<LevelObject> l)
        {
            List<int> usedGroupIDs = new List<int>();
            for (int i = 0; i < l.Count; i++)
            {
                int[] GroupIDs = (int[])(l[i][LevelObject.ObjectParameter.GroupIDs]);
                for (int j = 0; j < GroupIDs.Length; j++)
                    if (!usedGroupIDs.Contains(GroupIDs[j]))
                        usedGroupIDs.Add(GroupIDs[j]);
            }
            return usedGroupIDs.Count;
        }
        public static int SetLevelStringsForEmptyLevels()
        {
            int lvls = 0;
            for (int i = 0; i < UserLevelCount; i++)
                if (UserLevels[i].LevelString == "") { AddLevelStringParameter(defaultLevelString, i); lvls++; }
            UpdateLevelData();
            return lvls;
        }
        public static int[] GetDifferentLevelObjectCounts()
        {
            int[] objCounts = new int[UserLevelCount];
            for (int i = 0; i < UserLevelCount; i++)
                objCounts[i] = GetDifferentObjectCount(GetObjects(GetObjectString(UserLevels[i].LevelString)));
            return objCounts;
        }
        public static int[] GetDifferentObjectIDs(List<LevelObject> l)
        {
            List<int> diffObjIDs = new List<int>();
            for (int i = 0; i < l.Count; i++)
                if (!diffObjIDs.Contains((int)(l[i][LevelObject.ObjectParameter.ID])))
                    diffObjIDs.Add((int)(l[i][LevelObject.ObjectParameter.ID]));
            return diffObjIDs.ToArray();
        }
        public static int[] GetUsedGroupCounts()
        {
            int[] usedGroupIDs = new int[UserLevelCount];
            for (int i = 0; i < UserLevelCount; i++)
                usedGroupIDs[i] = GetUsedGroupCounts(GetObjects(GetObjectString(UserLevels[i].LevelString)));
            return usedGroupIDs;
        }
        public static int[] GetLevelAttempts()
        {
            return TryGetKeyValues(18, "i", "0").ToInt32Array();
        }
        public static int[] GetLevelFolders()
        {
            return TryGetKeyValues(84, "i", "0").ToInt32Array();
        }
        public static int[] GetLevelIDs()
        {
            return TryGetKeyValues(1, "i", "0").ToInt32Array();
        }
        public static int[] GetLevelLengths()
        {
            return TryGetKeyValues(23, "i", "0").ToInt32Array();
        }
        public static int[] GetLevelObjectCounts()
        {
            int[] objCounts = new int[UserLevelCount];
            for (int i = 0; i < UserLevelCount; i++)
            {
                string[] objs = GetObjectString(UserLevels[i].LevelString).Split(';');
                for (int j = 0; j < objs.Length; j++)
                    if (objs[j].Length > 0 && !objs[j].StartsWith("1,31,2"))
                        objCounts[i]++;
            }
            return objCounts;
        }
        public static int[] GetLevelRevisions()
        {
            return TryGetKeyValues(46, "i", "0").ToInt32Array();
        }
        public static int[] GetLevelVersions()
        {
            return TryGetKeyValues(16, "i", "0").ToInt32Array();
        }
        public static int[] GetUsedGroupIDs(List<LevelObject> l)
        {
            List<int> usedGroupIDs = new List<int>();
            for (int i = 0; i < l.Count; i++)
            {
                int[] GroupIDs = (int[])(l[i][LevelObject.ObjectParameter.GroupIDs]);
                if (GroupIDs != null)
                    for (int j = 0; j < GroupIDs.Length; j++)
                        if (!usedGroupIDs.Contains(GroupIDs[j]))
                            usedGroupIDs.Add(GroupIDs[j]);
            }
            return usedGroupIDs.ToArray();
        }
        public static List<int[]> GetDifferentLevelObjectIDs()
        {
            List<int[]> objCounts = new List<int[]>();
            for (int i = 0; i < UserLevelCount; i++)
                objCounts.Add(GetDifferentObjectIDs(GetObjects(GetObjectString(UserLevels[i].DecryptedLevelString))));
            return objCounts;
        }
        public static List<int[]> GetUsedGroupIDs()
        {
            List<int[]> usedGroupIDs = new List<int[]>();
            for (int i = 0; i < UserLevelCount; i++)
                usedGroupIDs.Add(GetUsedGroupIDs(GetObjects(GetObjectString(UserLevels[i].DecryptedLevelString))));
            return usedGroupIDs;
        }
        public static string CreateGuidelineString(List<double> timeStamps, List<double> colors)
        {
            string guidelineString = "";
            for (int i = 0; i < timeStamps.Count; i++)
                guidelineString += timeStamps[i].ToString() + "~" + colors[i].ToString() + "~";
            return guidelineString;
        }
        public static string CreateGuidelineString(List<Guideline> guidelines)
        {
            string guidelineString = "";
            for (int i = 0; i < guidelines.Count; i++)
                guidelineString += guidelines[i].TimeStamp.ToString() + "~" + guidelines[i].Color.ToString() + "~";
            return guidelineString;
        }
        public static string GetCustomSongLocation(int index)
        {
            return GDLocalData + "\\" + GetCustomSongID(index) + ".mp3";
        }
        public static string GetCustomSongID(int index)
        {
            return GetKeyValue(index, 45, "i");
        }
        public static string GetDecryptedLevelString(int index)
        {
            TryDecryptLevelString(index, out string decrypted);
            return decrypted;
        }
        public static string GetGuidelineString(string ls)
        {
            int guidelinesStartIndex = ls.Find("kA14,") + 5;
            int guidelinesEndIndex = ls.Find(",kA6");
            int guidelinesLength = guidelinesEndIndex - guidelinesStartIndex;
            return ls.Substring(guidelinesStartIndex, guidelinesLength);
        }
        public static string GetGuidelineString(int index)
        {
            if (UserLevels[index].DecryptedLevelString.Length > 0)
                return GetGuidelineString(UserLevels[index].DecryptedLevelString);
            return "";
        }

        public static string GetKeyValue(string level, int key, string valueType)
        {
            string startKeyString = "<k>k" + key.ToString() + "</k><" + valueType + ">";
            string endKeyString = "</" + valueType + ">";
            if (UserLevelCount > 0)
            {
                int parameterStartIndex, parameterEndIndex, parameterLength;
                parameterStartIndex = DecryptedLevelData.Find(startKeyString, 0, level.Length - 1) + startKeyString.Length;
                parameterEndIndex = DecryptedLevelData.Find(endKeyString, parameterStartIndex, level.Length - 1);
                parameterLength = parameterEndIndex - parameterStartIndex;
                if (parameterStartIndex == startKeyString.Length - 1)
                    throw new KeyNotFoundException();
                else
                    return DecryptedLevelData.Substring(parameterStartIndex, parameterLength);
            }
            else
                throw new ArgumentNullException();
        }
        public static string GetKeyValue(int index, int key, string valueType)
        {
            string startKeyString = "<k>k" + key.ToString() + "</k><" + valueType + ">";
            string endKeyString = "</" + valueType + ">";
            if (UserLevelCount > 0)
            {
                if (index <= UserLevelCount - 1)
                {
                    int parameterStartIndex = UserLevels[index].RawLevel.Find(startKeyString, 0, UserLevels[index].RawLevel.Length - 1) + startKeyString.Length;
                    if (parameterStartIndex == startKeyString.Length - 1)
                        throw new KeyNotFoundException();
                    int parameterEndIndex = UserLevels[index].RawLevel.Find(endKeyString, parameterStartIndex, UserLevels[index].RawLevel.Length - 1);
                    int parameterLength = parameterEndIndex - parameterStartIndex;
                    return UserLevels[index].RawLevel.Substring(parameterStartIndex, parameterLength);
                }
                else
                    throw new ArgumentOutOfRangeException();
            }
            else
                throw new ArgumentNullException();
        }
        public static string GetLevel(int index)
        {
            string levelKey = "<k>k_" + index + "</k>"; // The key of the level that will be returned
            string level = ""; // The level that is being returned
            if (index < UserLevelCount - 1) // If the level is not the last level in the list
                level = levelKey + DecryptedLevelData.Substring(LevelKeyStartIndices[index], DecryptedLevelData.Find("<k>k_" + (index + 1) + "</k>") - LevelKeyStartIndices[index]);
            else if (index == UserLevelCount - 1) // If the level is the last level in the list
                level = levelKey + DecryptedLevelData.Substring(LevelKeyStartIndices[index], Math.Max(DecryptedLevelData.Find("</d></d></d>") + 8, DecryptedLevelData.Find("<d /></d></d>") + 9) - LevelKeyStartIndices[index]);
            return level;
        }
        public static string GetLevelDescription(int index)
        {
            try { return Encoding.UTF8.GetString(FromBase64String(GetKeyValue(index, 3, "s"))); }
            catch { return ""; }
        }
        public static string GetLevelKeyEntry(string levelString, string name, string desc)
        {
            List<LevelObject> objects = GetObjects(levelString);
            string level = "<d><k>kCEK</k><i>4</i><k>k2</k><s>" + name + "</s><k>k4</k><s>" + levelString + "</s>" + (desc.Length > 0 ? "<k>k3</k><s>" + ToBase64String(Encoding.ASCII.GetBytes(desc)) + "</s>" : "") + "<k>k46</k><i>0</i><k>k48</k><i>" + objects.Count + "</i><k>k5</k><s>" + UserName + "</s><k>k13</k><t /><k>k21</k><i>2</i><k>k16</k><i>1</i><k>k80</k><i>1</i><k>k50</k><i>33</i><k>k47</k><t /><k>kI1</k><r>0</r><k>kI2</k><r>36</r><k>kI3</k><r>1</r><k>kI6</k><d><k>0</k><s>0</s><k>1</k><s>0</s><k>2</k><s>0</s><k>3</k><s>0</s><k>4</k><s>0</s><k>5</k><s>0</s><k>6</k><s>0</s><k>7</k><s>0</s><k>8</k><s>0</s><k>9</k><s>0</s><k>10</k><s>0</s><k>11</k><s>0</s><k>12</k><s>0</s></d></d>";
            return level;
        }
        public static string GetLevelLengthString(int index)
        {
            string levelLength;
            int length = GetLevelLength(index);
            switch (length)
            {
                case 0: levelLength = "Tiny"; break;
                case 1: levelLength = "Small"; break;
                case 2: levelLength = "Medium"; break;
                case 3: levelLength = "Long"; break;
                case 4: levelLength = "XL"; break;
                default: levelLength = "Tiny"; break;
            }
            return levelLength;
        }
        public static string GetLevelString(int index)
        {
            try { return GetKeyValue(index, 4, "s"); }
            catch (KeyNotFoundException) { return ""; }
        }
        public static string GetLevelString(string level)
        {
            try { return GetKeyValue(level, 4, "s"); }
            catch (KeyNotFoundException) { return ""; }
        }
        public static string GetObjectString(string ls)
        {
            try { return ls.Split(';').RemoveAt(0).Combine(";"); }
            catch { return ""; }
        }
        public static string GetOfficialSongID(int index)
        {
            return GetKeyValue(index, 8, "i");
        }
        public static string RemoveLevelIndexKey(string level)
        {
            string result = level;
            int keyIndex = result.Find("<k>k_");
            if (keyIndex > -1)
            {
                int endIndex = result.Find("</k>", keyIndex, result.Length) + 4;
                result = result.Remove(keyIndex, endIndex);
            }
            return result;
        }
        public static string TryGetKeyValue(int index, int key, string valueType, string defaultValueOnException)
        {
            string startKeyString = "<k>k" + key.ToString() + "</k><" + valueType + ">";
            string endKeyString = "</" + valueType + ">";
            if (UserLevelCount > 0)
            {
                if (index <= UserLevelCount - 1)
                {
                    int parameterStartIndex;
                    int parameterEndIndex;
                    int parameterLength;
                    if (index < UserLevelCount - 1)
                    {
                        parameterStartIndex = DecryptedLevelData.Find(startKeyString, LevelKeyStartIndices[index], LevelKeyStartIndices[index + 1]) + startKeyString.Length;
                        parameterEndIndex = DecryptedLevelData.Find(endKeyString, parameterStartIndex, LevelKeyStartIndices[index + 1]);
                    }
                    else
                    {
                        parameterStartIndex = DecryptedLevelData.Find(startKeyString, LevelKeyStartIndices[index], DecryptedLevelData.Length - 1) + startKeyString.Length;
                        parameterEndIndex = DecryptedLevelData.Find(endKeyString, parameterStartIndex, DecryptedLevelData.Length - 1);
                    }
                    parameterLength = parameterEndIndex - parameterStartIndex;
                    if (parameterStartIndex == startKeyString.Length - 1)
                        return defaultValueOnException;
                    else
                        return DecryptedLevelData.Substring(parameterStartIndex, parameterLength);
                }
                else
                    throw new ArgumentOutOfRangeException();
            }
            else
                throw new ArgumentNullException();
        }
        public static string TryGetKeyValue(string level, int key, string valueType, string defaultValueOnException)
        {
            string startKeyString = "<k>k" + key.ToString() + "</k><" + valueType + ">";
            string endKeyString = "</" + valueType + ">";
            int parameterStartIndex = level.Find(startKeyString) + startKeyString.Length;
            int parameterEndIndex = level.Find(endKeyString, parameterStartIndex, level.Length - 1);
            int parameterLength = parameterEndIndex - parameterStartIndex;
            if (parameterStartIndex == startKeyString.Length - 1)
                return defaultValueOnException;
            else
                return level.Substring(parameterStartIndex, parameterLength);
        }
        public static string[] GetGuidelineStrings()
        {
            string[] gs = new string[UserLevelCount];
            for (int i = 0; i < UserLevelCount; i++)
                if (UserLevels[i].LevelString != "" && UserLevels[i].LevelString != null)
                    gs[i] = GetGuidelineString(i);
            return gs;
        }
        public static string[] GetLevelDescriptions()
        {
            string[] descs = new string[UserLevelCount];
            for (int i = 0; i < UserLevelCount; i++)
                descs[i] = GetLevelDescription(i);
            return descs;
        }
        public static string[] GetLevelLengthStrings()
        {
            int[] lengths = GetLevelLengths();
            string[] levelLengthStrings = new string[lengths.Length];
            for (int i = 0; i < lengths.Length; i++)
                levelLengthStrings[i] = levelLengthNames[lengths[i]];
            return levelLengthStrings;
        }
        public static string[] GetLevelNames()
        {
            return GetKeyValues(2, "s");
        }
        public static string[] GetLevelNamesWithRevisions()
        {
            try
            {
                string[] lvlNames = GetLevelNames();
                int[] lvlRevs = GetLevelRevisions();
                string[] final = new string[lvlNames.Length];
                for (int i = 0; i < lvlNames.Length; i++)
                    final[i] = lvlNames[i] + (lvlRevs[i] > 0 ? " (Rev. " + lvlRevs[i].ToString() + ")" : "");
                return final;
            }
            catch { return null; }
        }
        public static string[] GetLevelStrings()
        {
            string[] ls = new string[UserLevelCount];
            for (int i = 0; i < UserLevelCount; i++)
            {
                try { TryDecryptLevelString(i, out ls[i]); }
                catch (Exception ex) when (ex.GetType() == typeof(ArgumentException) || ex.GetType() == typeof(KeyNotFoundException) || ex.GetType() == typeof(FormatException)) { ls[i] = ""; }
                catch (Exception e) { string type = e.GetType().ToString(); new DataException("An unknown error has occured. Contact us immediately about this occurence and provide us with details. Your level data file may be asked for debugging and examination."); }
            }
            return ls;
        }
        public static string[] GetKeyValues(int key, string valueType)
        {
            string startKeyString = "<k>k" + key.ToString() + "</k><" + valueType + ">";
            string endKeyString = "</" + valueType + ">";
            if (UserLevelCount > 0)
            {
                string[] lvlParameters = new string[UserLevelCount];
                int[] parameterStartIndices = new int[UserLevelCount];
                int[] parameterEndIndices = new int[UserLevelCount];
                int[] parameterLengths = new int[UserLevelCount];
                for (int i = 0; i < UserLevelCount; i++)
                {
                    if (i < UserLevelCount - 1)
                    {
                        parameterStartIndices[i] = DecryptedLevelData.Find(startKeyString, LevelKeyStartIndices[i], LevelKeyStartIndices[i + 1]) + startKeyString.Length;
                        parameterEndIndices[i] = DecryptedLevelData.Find(endKeyString, parameterStartIndices[i], LevelKeyStartIndices[i + 1]);
                    }
                    else
                    {
                        parameterStartIndices[i] = DecryptedLevelData.Find(startKeyString, LevelKeyStartIndices[i], DecryptedLevelData.Length - 1) + startKeyString.Length;
                        parameterEndIndices[i] = DecryptedLevelData.Find(endKeyString, parameterStartIndices[i], DecryptedLevelData.Length - 1);
                    }
                    parameterLengths[i] = parameterEndIndices[i] - parameterStartIndices[i];
                }
                for (int i = 0; i < parameterStartIndices.Length; i++)
                    if (parameterStartIndices[i] == startKeyString.Length - 1)
                        throw new KeyNotFoundException();
                for (int i = 0; i < UserLevelCount; i++)
                    lvlParameters[i] = DecryptedLevelData.Substring(parameterStartIndices[i], parameterLengths[i]);
                return lvlParameters;
            }
            else
                throw new ArgumentNullException();
        }
        public static string[] TryGetKeyValues(int key, string valueType, string defaultValueOnException)
        {
            string startKeyString = "<k>k" + key.ToString() + "</k><" + valueType + ">";
            string endKeyString = "</" + valueType + ">";
            if (UserLevelCount > 0)
            {
                string[] lvlParameters = new string[UserLevelCount];
                int[] parameterStartIndices = new int[UserLevelCount];
                int[] parameterEndIndices = new int[UserLevelCount];
                int[] parameterLengths = new int[UserLevelCount];
                for (int i = 0; i < UserLevelCount; i++)
                {
                    if (i < UserLevelCount - 1)
                    {
                        parameterStartIndices[i] = DecryptedLevelData.Find(startKeyString, LevelKeyStartIndices[i], LevelKeyStartIndices[i + 1]) + startKeyString.Length;
                        parameterEndIndices[i] = DecryptedLevelData.Find(endKeyString, parameterStartIndices[i], LevelKeyStartIndices[i + 1]);
                    }
                    else
                    {
                        parameterStartIndices[i] = DecryptedLevelData.Find(startKeyString, LevelKeyStartIndices[i], DecryptedLevelData.Length - 1) + startKeyString.Length;
                        parameterEndIndices[i] = DecryptedLevelData.Find(endKeyString, parameterStartIndices[i], DecryptedLevelData.Length - 1);
                    }
                    parameterLengths[i] = parameterEndIndices[i] - parameterStartIndices[i];
                }
                for (int i = 0; i < UserLevelCount; i++)
                    if (parameterStartIndices[i] == startKeyString.Length - 1)
                        lvlParameters[i] = defaultValueOnException;
                    else
                        lvlParameters[i] = DecryptedLevelData.Substring(parameterStartIndices[i], parameterLengths[i]);
                return lvlParameters;
            }
            else
                throw new ArgumentNullException();
        }
        public static void AddKey(int index, int key, string value, string valueType)
        {
            string keyToInsert = "<k>k" + key + "</k><" + valueType + ">" + value + "</" + valueType + ">";
            UserLevels[index].RawLevel = UserLevels[index].RawLevel.Insert(22, keyToInsert);
            DecryptedLevelData = DecryptedLevelData.Insert(DecryptedLevelData.Find("<k>kCEK</k><i>4</i>", index + 1) + 19, keyToInsert);
            for (int i = index + 1; i < UserLevelCount; i++)
                LevelKeyStartIndices[i] += keyToInsert.Length;
        }
        public static void AddLevelStringParameter(string newLS, int levelIndex)
        {
            string nameKey = "<k>k2</k><s>" + UserLevels[levelIndex].LevelName + "</s>";
            int nameKeyOccurence = 0;
            for (int i = 0; i < levelIndex; i++)
                if (UserLevels[i].LevelName == UserLevels[levelIndex].LevelName)
                    nameKeyOccurence++;
            int index = DecryptedLevelData.Find(nameKey, nameKeyOccurence + 1) + nameKey.Length;
            DecryptedLevelData = DecryptedLevelData.Insert(index, "<k>k4</k><s>" + newLS + "</s>");
            for (int i = levelIndex + 1; i < UserLevelCount; i++)
                LevelKeyStartIndices[i] += ("<k>k4</k><s>" + newLS + "</s>").Length;
            UserLevels[levelIndex].LevelString = newLS;
        }
        public static void CloneLevel(int index)
        {
            if (index >= 0)
            {
                if (index < UserLevelCount)
                {
                    CloneAllLevelInfo(0, index);
                    UpdateLevelData();
                }
                else throw new ArgumentOutOfRangeException("index", "The argument that is parsed is out of range.");
            }
            else throw new ArgumentOutOfRangeException("index", "The index of the level cannot be a negative number.");
        }
        public static void CloneLevels(int[] indices)
        {
            indices = indices.RemoveDuplicates().Sort();
            for (int i = 0; i < indices.Length; i++)
                if (indices[i] >= 0)
                    if (indices[i] < UserLevelCount)
                        CloneAllLevelInfo(i, indices[i] + i);
            UpdateMemoryLevelData();
            UpdateLevelData(); // Write the new data
        }
        public static void CreateLevel()
        {
            int unnamedNumber = 0;
            List<int> unnamedNumbers = new List<int>();
            for (int i = 0; i < UserLevelCount; i++) // Add the unnamed numbers to the list
                if (UserLevels[i].LevelName.Contains("Unnamed ") && UserLevels[i].LevelName.Split(' ').Length == 2 && int.TryParse(UserLevels[i].LevelName.Split(' ')[1], out int shit)) // Add the number to the list if the 
                    unnamedNumbers.Add(UserLevels[i].LevelName.GetLastNumber());
            unnamedNumbers.Sort();
            for (int i = 0; i < unnamedNumbers.Count; i++)
            {
                if (unnamedNumber != unnamedNumbers[i]) break; // If the unnamed number is not used in the levels, then stop looking
                unnamedNumber++; // Increase the unnamed number
            }
            CreateLevel("Unnamed " + unnamedNumber, "");
        } 
        public static void CreateLevel(string name)
        {
            CreateLevel(name, "");
        }
        public static void CreateLevel(string name, string desc)
        {
            int levelRevision = 0;
            List<int> levelsWithSameName = new List<int>();
            for (int i = 0; i < UserLevels.Count; i++)
                if (name == UserLevels[i].LevelName)
                    levelsWithSameName.Add(i);
            List<int> revisionsOfLevelsWithSameName = new List<int>(); // The revisions of the levels with the same name
            for (int i = 0; i < levelsWithSameName.Count; i++) // Add the revisions of the levels with the same name in the list
                revisionsOfLevelsWithSameName.Add(UserLevels[levelsWithSameName[i]].LevelRevision);
            revisionsOfLevelsWithSameName.Sort();
            for (int i = 0; i < revisionsOfLevelsWithSameName.Count; i++)
            {
                if (levelRevision != revisionsOfLevelsWithSameName[i]) break; // If the level revision is not used in the levels, then stop looking
                levelRevision++; // Increase the level revision
            }
            string level = "<k>k_0</k><d><k>kCEK</k><i>4</i><k>k2</k><s>" + name + "</s><k>k4</k><s>" + defaultLevelString + "</s>" + (desc.Length > 0 ? "<k>k3</k><s>" + ToBase64String(Encoding.ASCII.GetBytes(desc)) + "</s>" : "") + "<k>k46</k><i>" + levelRevision.ToString() + "</i><k>k5</k><s>" + UserName + "</s><k>k13</k><t /><k>k21</k><i>2</i><k>k16</k><i>1</i><k>k80</k><i>1</i><k>k50</k><i>33</i><k>k47</k><t /><k>kI1</k><r>0</r><k>kI2</k><r>36</r><k>kI3</k><r>1</r><k>kI6</k><d><k>0</k><s>0</s><k>1</k><s>0</s><k>2</k><s>0</s><k>3</k><s>0</s><k>4</k><s>0</s><k>5</k><s>0</s><k>6</k><s>0</s><k>7</k><s>0</s><k>8</k><s>0</s><k>9</k><s>0</s><k>10</k><s>0</s><k>11</k><s>0</s><k>12</k><s>0</s></d></d>";
            // Insert the cloned level's parameters
            UserLevels.Insert(0, new Level(level));
            GetLevelInfo(0);
            UpdateMemoryLevelData();
            UpdateLevelData(); // Write the new data
        }
        public static void CreateLevel(string name, string desc, string levelString)
        {
            int levelRevision = 0;
            List<int> levelsWithSameName = new List<int>();
            for (int i = 0; i < UserLevels.Count; i++)
                if (name == UserLevels[i].LevelName)
                    levelsWithSameName.Add(i);
            List<int> revisionsOfLevelsWithSameName = new List<int>(); // The revisions of the levels with the same name
            for (int i = 0; i < levelsWithSameName.Count; i++) // Add the revisions of the levels with the same name in the list
                revisionsOfLevelsWithSameName.Add(UserLevels[levelsWithSameName[i]].LevelRevision);
            revisionsOfLevelsWithSameName.Sort();
            for (int i = 0; i < revisionsOfLevelsWithSameName.Count; i++)
            {
                if (levelRevision != revisionsOfLevelsWithSameName[i]) break; // If the level revision is not used in the levels, then stop looking
                levelRevision++; // Increase the level revision
            }
            string level = "<k>k_0</k><d><k>kCEK</k><i>4</i><k>k2</k><s>" + name + "</s><k>k4</k><s>" + levelString + "</s>" + (desc.Length > 0 ? "<k>k3</k><s>" + ToBase64String(Encoding.ASCII.GetBytes(desc)) + "</s>" : "") + "<k>k46</k><i>" + levelRevision.ToString() + "</i><k>k5</k><s>" + UserName + "</s><k>k13</k><t /><k>k21</k><i>2</i><k>k16</k><i>1</i><k>k80</k><i>1</i><k>k50</k><i>33</i><k>k47</k><t /><k>kI1</k><r>0</r><k>kI2</k><r>36</r><k>kI3</k><r>1</r><k>kI6</k><d><k>0</k><s>0</s><k>1</k><s>0</s><k>2</k><s>0</s><k>3</k><s>0</s><k>4</k><s>0</s><k>5</k><s>0</s><k>6</k><s>0</s><k>7</k><s>0</s><k>8</k><s>0</s><k>9</k><s>0</s><k>10</k><s>0</s><k>11</k><s>0</s><k>12</k><s>0</s></d></d>";
            
            UpdateMemoryLevelData();
            UpdateLevelData(); // Write the new data
        }
        public static void CreateLevels(int numberOfLevels)
        {
            // Useless ATM
        }
        public static void CreateLevels(int numberOfLevels, string[] names)
        {
            // Useless ATM
        }
        public static void CreateLevels(int numberOfLevels, string[] names, string[] descs)
        {
            // Useless ATM
        }
        public static void DeleteAllLevels()
        {
            DecryptedLevelData = defaultLevelData; // Set the level data to the default
            UpdateLevelData(); // Write the new data

            // Delete all the level info from the prorgam's memory3
            UserLevels = new List<Level>();
            LevelKeyStartIndices = new List<int>();
        }
        public static void DeleteLevels(int[] indices)
        {
            indices = indices.RemoveDuplicates();
            indices = indices.Sort();
            for (int i = indices.Length - 1; i >= 0; i--)
                RemoveAllLevelInfoAt(indices[i]);
            UpdateMemoryLevelData();
            UpdateLevelData(); // Write the new data
        }
        public static void ExportLevel(int index, string folderPath)
        {
            File.WriteAllText(folderPath + UserLevels[index].LevelNameWithRevision, GetLevel(index));
        }
        public static void ExportLevels(int[] indices, string folderPath)
        {
            for (int i = 0; i < indices.Length; i++) File.WriteAllText(folderPath + "\\" + UserLevels[indices[i]].LevelNameWithRevision + ".txt", GetLevel(indices[i]));
        }
        public static void GetCustomObjects()
        {
            int startIndex = DecryptedGamesave.Find("<k>customObjectDict</k><d>") + 26;
            int endIndex = DecryptedGamesave.Find("</d>", startIndex, DecryptedGamesave.Length);
            List<CustomLevelObject> customObjs = new List<CustomLevelObject>();
            int currentIndex = startIndex;
            while ((currentIndex = DecryptedGamesave.Find("</k><s>", currentIndex, endIndex) + 7) > 6)
                customObjs.Add(new CustomLevelObject(GetObjects(DecryptedGamesave.Substring(currentIndex, DecryptedGamesave.Find("</s>", currentIndex, DecryptedGamesave.Length) - currentIndex))));
            CustomObjects = customObjs;
        }
        public static void GetKeyIndices()
        {
            LevelKeyStartIndices = new List<int>();
            int count = GetLevelCount();
            for (int i = 0; i < GetLevelCount(); i++)
            {
                if (i > 0)
                    LevelKeyStartIndices.Add(DecryptedLevelData.Find("<k>k_" + i + "</k><d>", LevelKeyStartIndices[i - 1], DecryptedLevelData.Length) + ("<k>k_" + i + "</k>").Length);
                else if (i == 0)
                    LevelKeyStartIndices.Add(DecryptedLevelData.Find("<k>k_" + i + "</k><d>") + ("<k>k_" + i + "</k>").Length);
            }
        }
        public static void GetLevels()
        {
            UserLevels = new List<Level>();
            for (int i = 0; i < LevelKeyStartIndices.Count; i++)
            {
                if (i < LevelKeyStartIndices.Count - 1)
                    UserLevels.Add(new Level(DecryptedLevelData.Substring(LevelKeyStartIndices[i], LevelKeyStartIndices[i + 1] - LevelKeyStartIndices[i] - ("<k>k_" + (i + 1) + "</k>").Length)));
                else if (i == LevelKeyStartIndices.Count - 1)
                    UserLevels.Add(new Level(DecryptedLevelData.Substring(LevelKeyStartIndices[i], Math.Max(DecryptedLevelData.Find("</d></d></d>", LevelKeyStartIndices[i], DecryptedLevelData.Length) + 8, DecryptedLevelData.Find("<d /></d></d>", LevelKeyStartIndices[i], DecryptedLevelData.Length) + 9) - LevelKeyStartIndices[i])));
            }
        }
        /// <summary>Gets the level info for all levels in the gamesave.</summary>
        public static void GetAllLevelInfo()
        {
            for (int i = 0; i < UserLevelCount; i++)
                GetLevelInfo(i);
        }
        /// <summary>Gets the level info for a level in the specified index in the gamesave.</summary>
        public static void GetLevelInfo(int index)
        {
            string startKeyString = "<k>k";
            string endKeyString = "</k><";
            if (UserLevelCount > index)
            {
                int parameterIDStartIndex;
                int parameterIDEndIndex;
                int parameterValueTypeStartIndex;
                int parameterValueTypeEndIndex;
                int parameterValueStartIndex;
                int parameterValueEndIndex;
                int parameterID;
                string parameterValueType;
                string parameterValue;
                int finalIndex = (index < UserLevelCount - 1 ? LevelKeyStartIndices[index + 1] : DecryptedLevelData.Length - 1) - 10 - index / 10;
                for (int i = LevelKeyStartIndices[index]; i < finalIndex; )
                {
                    parameterIDStartIndex = DecryptedLevelData.Find(startKeyString, i, finalIndex) + startKeyString.Length;
                    if (parameterIDStartIndex <= startKeyString.Length)
                        break; 
                    parameterIDEndIndex = DecryptedLevelData.Find(endKeyString, parameterIDStartIndex, finalIndex);
                    parameterValueTypeStartIndex = parameterIDEndIndex + endKeyString.Length;
                    parameterValueTypeEndIndex = DecryptedLevelData.Find(">", parameterValueTypeStartIndex, finalIndex);
                    parameterValueType = DecryptedLevelData.Substring(parameterValueTypeStartIndex, parameterValueTypeEndIndex - parameterValueTypeStartIndex);
                    parameterValueStartIndex = parameterValueTypeEndIndex + 1;
                    parameterValueEndIndex = parameterValueType[parameterValueType.Length - 1] != '/' ? DecryptedLevelData.Find($"</{parameterValueType}>", parameterValueStartIndex, finalIndex) : parameterValueStartIndex;
                    parameterValue = DecryptedLevelData.Substring(parameterValueStartIndex, parameterValueEndIndex - parameterValueStartIndex);
                    string s = DecryptedLevelData.Substring(parameterIDStartIndex, parameterIDEndIndex - parameterIDStartIndex);
                    if (s != "CEK" && !s.StartsWith("I"))
                    {
                        parameterID = ToInt32(s);
                        switch (parameterID)
                        {
                            case 1: // Level ID
                                UserLevels[index].LevelID = ToInt32(parameterValue);
                                break;
                            case 2: // Level Name
                                UserLevels[index].LevelName = parameterValue;
                                break;
                            case 3: // Level Description
                                UserLevels[index].LevelDescription = Encoding.UTF8.GetString(Base64Decrypt(parameterValue));
                                break;
                            case 4: // Level String
                                UserLevels[index].LevelString = parameterValue;
                                break;
                            case 8: // Official Song ID
                                UserLevels[index].LevelOfficialSongID = ToInt32(parameterValue);
                                break;
                            case 14: // Level Verified Status
                                UserLevels[index].LevelVerifiedStatus = parameterValueType == "t /"; // Well that's how it's implemented ¯\_(ツ)_/¯
                                break;
                            case 15: // Level Uploaded Status
                                UserLevels[index].LevelUploadedStatus = parameterValueType == "t /";
                                break;
                            case 16: // Level Version
                                UserLevels[index].LevelVersion = ToInt32(parameterValue);
                                break;
                            case 18: // Level Attempts
                                UserLevels[index].LevelAttempts = ToInt32(parameterValue);
                                break;
                            case 23: // Level Length
                                UserLevels[index].LevelLength = ToInt32(parameterValue);
                                break;
                            case 45: // Custom Song ID
                                UserLevels[index].LevelCustomSongID = ToInt32(parameterValue);
                                break;
                            case 46: // Level Revision
                                UserLevels[index].LevelRevision = ToInt32(parameterValue);
                                break;
                            case 80: // Time Spent
                                UserLevels[index].BuildTime = ToInt32(parameterValue);
                                break;
                            case 84: // Level Folder
                                UserLevels[index].LevelFolder = ToInt32(parameterValue);
                                break;
                            default: // Not something we care about
                                break;
                        }
                    }
                    i = parameterValueEndIndex;
                }
                UserLevels[index].ObjectCounts = new Dictionary<int, int>();
                if (Options.BoolDictionary["decryptLevelStrings"])
                {
                    if (UserLevels[index].LevelString != null) // If there is a level string
                    {
                        try
                        {
                            bool isEncrypted = TryDecryptLevelString(index, out UserLevels[index].DecryptedLevelString);
                            if (isEncrypted)
                            {
                                UserLevels[index].RawLevel.Replace(UserLevels[index].LevelString, UserLevels[index].DecryptedLevelString);
                                // TODO: Probably refactor
                                UserLevels[index].LevelString = UserLevels[index].DecryptedLevelString;
                            }
                        }
                        catch (Exception ex) when (ex.GetType() == typeof(ArgumentException) || ex.GetType() == typeof(KeyNotFoundException) || ex.GetType() == typeof(FormatException))
                        {
                            UserLevels[index].DecryptedLevelString = "";
                        }
                        catch (Exception e)
                        {
                            string type = e.GetType().ToString();
                            throw new DataException($"An unknown error has occured while attempting to decrypt the level string of the level with index {index + 1} (zero-based index: {index}). Contact us immediately about this occurence and provide us the details as stated in this message. Your level data file may be asked for debugging and (manual) examination.");
                        }
                        UserLevels[index].LevelGuidelinesString = GetGuidelineString(index);
                        if (Options.BoolDictionary["analyzeDifferentObjectIDs"] || Options.BoolDictionary["analyzeUsedGroupIDs"] || Options.BoolDictionary["analyzeObjectCount"])
                        {
                            UserLevels[index].LevelObjects = GetObjects(GetObjectString(UserLevels[index].DecryptedLevelString));
                            if (Options.BoolDictionary["analyzeDifferentObjectIDs"])
                                UserLevels[index].LevelDifferentObjectIDs = GetDifferentObjectIDs(UserLevels[index].LevelObjects);
                            if (Options.BoolDictionary["analyzeUsedGroupIDs"])
                                UserLevels[index].LevelUsedGroupIDs = GetUsedGroupIDs(UserLevels[index].LevelObjects);
                            if (Options.BoolDictionary["analyzeObjectCount"])
                                GetObjectCounts(index);
                        }
                    }
                }
            }
            else
                throw new ArgumentOutOfRangeException("The level's index may not be greater or equal to the count of the levels.");
        }

        public static void GetObjectCounts(int index)
        {
            if (UserLevels[index].LevelObjects.Count > 0)
            {
                for (int i = 0; i < UserLevels[index].LevelObjects.Count; i++)
                {
                    int ID = (int)UserLevels[index].LevelObjects[i][ObjectParameter.ID];
                    if (!UserLevels[index].ObjectCounts.ContainsKey(ID))
                        UserLevels[index].ObjectCounts.Add(ID, 1);
                    else
                        UserLevels[index].ObjectCounts[ID]++;
                }
            }
        }
        public static Dictionary<int, int> GetObjectCounts(string level)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            string[,] objects = GetObjectString(GetLevelString(level)).Split(';').Split(',');
            if (objects.GetLength(1) > 1)
            {
                for (int i = 0; i < objects.GetLength(0); i++)
                {
                    int ID = ToInt32(objects[i, 1]);
                    if (!result.ContainsKey(ID))
                        result.Add(ID, 1);
                    else
                        result[ID]++;
                }
            }
            return result;
        }
        public static void ImportLevel(string level)
        {
            for (int i = UserLevelCount - 1; i >= 0; i--) // Increase the level indices of all the other levels to insert the cloned level at the start
                DecryptedLevelData = DecryptedLevelData.Replace("<k>k_" + i.ToString() + "</k>", "<k>k_" + (i + 1).ToString() + "</k>");
            level = RemoveLevelIndexKey(level); // Remove the index key of the level
            DecryptedLevelData = DecryptedLevelData.Insert(LevelKeyStartIndices[0] - 10, "<k>k_0</k>" + level); // Insert the new level
            int clonedLevelLength = level.Length + 10; // The length of the inserted level
            LevelKeyStartIndices = LevelKeyStartIndices.InsertAtStart(LevelKeyStartIndices[0]); // Add the new key start position in the array
            for (int i = 1; i < LevelKeyStartIndices.Count; i++)
                LevelKeyStartIndices[i] += clonedLevelLength; // Increase the other key indices by the length of the cloned level
            // Insert the imported level's parameters
            UserLevels = UserLevels.InsertAtStart(new Level());
            GetLevelInfo(0);
            UpdateLevelData(); // Write the new data
        }
        public static void ImportLevelFromFile(string levelPath)
        {
            ImportLevel(File.ReadAllText(levelPath));
        }
        public static void ImportLevels(string[] lvls)
        {
            for (int i = 0; i < lvls.Length; i++)
            {
                lvls[i] = RemoveLevelIndexKey(lvls[i]); // Remove the index key of the level
                UserLevels.InsertAtStart(new Level(lvls[i]));
                GetLevelInfo(0);
            }
            GetKeyIndices();
            UpdateMemoryLevelData();
            UpdateLevelData(); // Write the new data
        }
        public static void ImportLevelsFromFiles(string[] levelPaths)
        {
            string[] levels = new string[levelPaths.Length];
            for (int i = 0; i < levelPaths.Length; i++)
                levels[i] = File.ReadAllText(levelPaths[i]);
            ImportLevels(levels);
        }
        public static void MoveLevelsDown(int[] indices)
        {
            indices = indices.RemoveDuplicates().Sort().RemoveElementsMatchingIndicesFromEnd(UserLevelCount);
            for (int i = indices.Length - 1; i >= 0; i--)
                if (indices[i] < UserLevelCount - 1) // If the level can be moved further down
                    SwapAllLevelInfo(indices[i], indices[i] + 1);
            UpdateMemoryLevelData(); // Rebuild the level data
            UpdateLevelData(); // Write the new data
        }
        public static void MoveLevelsToBottom(int[] indices)
        {
            indices = indices.RemoveDuplicates().Sort();
            for (int i = indices.Length - 1; i >= 0; i--)
                MoveAllLevelInfo(indices[i], UserLevelCount - indices.Length + i + 1); // +1?
            UpdateMemoryLevelData(); // Rebuild the level data
            UpdateLevelData();
        }
        public static void MoveLevelsToTop(int[] indices)
        {
            indices = indices.RemoveDuplicates().Sort();
            for (int i = 0; i < indices.Length; i++)
                MoveAllLevelInfo(indices[i], i);
            UpdateMemoryLevelData(); // Rebuild the level data
            UpdateLevelData();
        }
        public static void MoveLevelsUp(int[] indices)
        {
            indices = indices.RemoveDuplicates().Sort().RemoveElementsMatchingIndices();
            for (int i = 0; i < indices.Length; i++)
                if (indices[i] >= i) // If the level can be moved further up
                    SwapAllLevelInfo(indices[i], indices[i] - 1);
            UpdateMemoryLevelData(); // Rebuild the level data
            UpdateLevelData(); // Write the new data
        }
        public static void SetGuidelineString(string newGuidelines, int levelIndex)
        {
            string oldGS = UserLevels[levelIndex].LevelGuidelinesString;
            int gsStartIndex = GetGuidelineStringStartIndex(levelIndex);
            string newLS;
            if (oldGS.Length == 0)
                newLS = UserLevels[levelIndex].LevelString.Insert(gsStartIndex, newGuidelines);
            else
                newLS = UserLevels[levelIndex].LevelString.Replace(oldGS, newGuidelines);
            UserLevels[levelIndex].LevelGuidelinesString = newGuidelines;
            SetLevelString(newLS, levelIndex);
        }
        public static void SetGuidelineStrings(string[] newGuidelineStrings, int[] levelIndices)
        {
            string[] oldGS = new string[levelIndices.Length];
            string[] newLS = new string[levelIndices.Length];
            int[] gsStartIndices = new int[levelIndices.Length];
            for (int i = 0; i < levelIndices.Length; i++)
            {
                oldGS[i] = UserLevels[levelIndices[i]].LevelGuidelinesString;
                gsStartIndices[i] = GetGuidelineStringStartIndex(levelIndices[i]);
            }
            for (int i = 0; i < levelIndices.Length; i++)
            {
                if (oldGS[i].Length == 0)
                    newLS[i] = UserLevels[levelIndices[i]].LevelString.Insert(gsStartIndices[i], newGuidelineStrings[i]);
                else
                    newLS[i] = UserLevels[levelIndices[i]].LevelString.Replace(oldGS[i], newGuidelineStrings[i]);
                UserLevels[levelIndices[i]].LevelGuidelinesString = newGuidelineStrings[i];
            }
            SetLevelStrings(newLS, levelIndices);
        }
        public static void SetGuidelineStrings(string newGuidelines, int[] levelIndices)
        {
            string[] oldGS = new string[levelIndices.Length];
            string[] newLS = new string[levelIndices.Length];
            int[] gsStartIndices = new int[levelIndices.Length];
            for (int i = 0; i < levelIndices.Length; i++)
            {
                oldGS[i] = UserLevels[levelIndices[i]].LevelGuidelinesString;
                gsStartIndices[i] = GetGuidelineStringStartIndex(levelIndices[i]);
            }
            for (int i = 0; i < levelIndices.Length; i++)
            {
                if (oldGS[i].Length == 0)
                    newLS[i] = UserLevels[levelIndices[i]].LevelString.Insert(gsStartIndices[i], newGuidelines);
                else
                    newLS[i] = UserLevels[levelIndices[i]].LevelString.Replace(oldGS[i], newGuidelines);
                UserLevels[levelIndices[i]].LevelGuidelinesString = newGuidelines;
            }
            SetLevelStrings(newLS, levelIndices);
        }
        public static void SetKeyValue(int index, int keyValue, string keyType, string newValue)
        {
            TemporarilySetKeyValue(index, keyValue, keyType, newValue);
            UpdateLevelData();
        }
        public static void SetLevel(string newLevel, int levelIndex)
        {
            UserLevels[levelIndex].RawLevel = newLevel;
            UpdateMemoryLevelData();
            UpdateLevelData(); // Write the new data
        }
        public static void SetLevels(string[] newLevels, int[] levelIndices)
        {
            for (int i = 0; i < levelIndices.Length; i++)
            {
                DecryptedLevelData = DecryptedLevelData.Replace(newLevels[i], LevelKeyStartIndices[levelIndices[i]], UserLevels[levelIndices[i]].RawLevel.Length); // Change the level in the game
                for (int j = levelIndices[i]; j < UserLevelCount; j++)
                    LevelKeyStartIndices[j] += newLevels[i].Length - UserLevels[levelIndices[i]].RawLevel.Length; // Change the indices of the other levels
                UserLevels[levelIndices[i]].RawLevel = newLevels[i];
            }
            UpdateLevelData(); // Write the new data
        }
        public static void SetLevelString(string newLS, int levelIndex)
        {
            string oldLS = GetLevelString(levelIndex);
            UserLevels[levelIndex].LevelString = newLS; // Set the new level strings in the memory

            string newLevel = UserLevels[levelIndex].RawLevel.Replace("<k>k4</k><s>" + GetLevelString(levelIndex) + "</s>", "<k>k4</k><s>" + newLS + "</s>");
            SetLevel(newLevel, levelIndex);
        }
        public static void SetLevelStrings(string[] newLS, int[] levelIndices)
        {
            string[] newLevels = new string[levelIndices.Length];
            for (int i = 0; i < levelIndices.Length; i++)
            {
                string oldLS = GetLevelString(levelIndices[i]);
                UserLevels[levelIndices[i]].LevelString = newLS[i]; // Set the new level strings in the memory
                newLevels[i] = UserLevels[levelIndices[i]].RawLevel.Replace("<k>k4</k><s>" + GetLevelString(levelIndices[i]) + "</s>", "<k>k4</k><s>" + newLS[i] + "</s>");
            }
            SetLevels(newLevels, levelIndices);
        }
        public static void SwapLevels(int levelIndexA, int levelIndexB)
        {
            SwapAllLevelInfo(levelIndexA, levelIndexB);
            UpdateMemoryLevelData();
            UpdateLevelData();
        }
        public static void TemporarilySetCustomSongID(int index, string newValue)
        {
            TemporarilySetKeyValue(index, 45, "i", newValue);
            UserLevels[index].RawLevel = UserLevels[index].RawLevel.Replace("<k>k45</k><i>" + UserLevels[index].LevelCustomSongID + "</i>", "<k>k45</k><i>" + newValue + "</i>");
            UserLevels[index].LevelCustomSongID = ToInt32(newValue);
        }
        public static void TemporarilySetKeyValue(int index, int keyValue, string keyType, string newValue)
        {
            string keyStart = "<k>k" + keyValue.ToString() + "</k><" + keyType + ">";
            string keyEnd = "</" + keyType + ">";
            int keyStartIndex = 0;
            if (index >= 0)
            {
                if (index < UserLevelCount - 1)
                    keyStartIndex = DecryptedLevelData.Find(keyStart, LevelKeyStartIndices[index], LevelKeyStartIndices[index + 1]) + keyStart.Length;
                else if (index == UserLevelCount - 1)
                    keyStartIndex = DecryptedLevelData.Find(keyStart, LevelKeyStartIndices[index], DecryptedLevelData.Length) + keyStart.Length;
                if (keyStartIndex >= keyStart.Length)
                {
                    int keyEndIndex = DecryptedLevelData.Find(keyEnd, keyStartIndex, DecryptedLevelData.Length);
                    int length = keyEndIndex - keyStartIndex;
                    DecryptedLevelData = DecryptedLevelData.Replace(newValue, keyStartIndex, length);
                    UserLevels[index].RawLevel = UserLevels[index].RawLevel.Replace(newValue, keyStartIndex - LevelKeyStartIndices[index], length);
                    for (int i = index + 1; i < UserLevelCount; i++) // Adjust the indices of the other levels
                        LevelKeyStartIndices[i] += newValue.Length - length;
                }
                else
                    AddKey(index, keyValue, newValue, keyType);
            }
            else
                throw new ArgumentException("The index may not be a negative number.");
        }
        public static void TemporarilySetLevelDescription(int index, string newValue)
        {
            TemporarilySetKeyValue(index, 3, "s", Base64Encrypt(Encoding.UTF8.GetBytes(newValue)));
            UserLevels[index].RawLevel = UserLevels[index].RawLevel.Replace("<k>k3</k><s>" + Base64Encrypt(Encoding.UTF8.GetBytes(UserLevels[index].LevelDescription)) + "</s>", "<k>k3</k><s>" + Base64Encrypt(Encoding.UTF8.GetBytes(newValue)) + "</s>");
            UserLevels[index].LevelDescription = newValue;
        }
        public static void TemporarilySetLevelFolder(int index, string newValue)
        {
            TemporarilySetKeyValue(index, 84, "i", newValue);
            UserLevels[index].RawLevel = UserLevels[index].RawLevel.Replace("<k>k84</k><i>" + UserLevels[index].LevelFolder + "</i>", "<k>k84</k><i>" + newValue + "</i>");
            UserLevels[index].LevelFolder = ToInt32(newValue);
        }
        public static void TemporarilySetLevelID(int index, string newValue)
        {
            TemporarilySetKeyValue(index, 1, "i", newValue);
            UserLevels[index].RawLevel = UserLevels[index].RawLevel.Replace("<k>k1</k><i>" + UserLevels[index].LevelID + "</i>", "<k>k1</k><i>" + newValue + "</i>");
            UserLevels[index].LevelID = ToInt32(newValue);
        }
        public static void TemporarilySetLevelName(int index, string newValue)
        {
            TemporarilySetKeyValue(index, 2, "s", newValue);
            UserLevels[index].RawLevel = UserLevels[index].RawLevel.Replace("<k>k2</k><s>" + UserLevels[index].LevelName + "</s>", "<k>k2</k><s>" + newValue + "</s>");
            UserLevels[index].LevelName = newValue;
        }
        public static void TemporarilySetLevelRevision(int index, string newValue)
        {
            TemporarilySetKeyValue(index, 46, "i", newValue);
            UserLevels[index].RawLevel = UserLevels[index].RawLevel.Replace("<k>k46</k><i>" + UserLevels[index].LevelRevision + "</i>", "<k>k46</k><i>" + newValue + "</i>");
            UserLevels[index].LevelRevision = ToInt32(newValue);
        }
        public static void TemporarilySetLevelVersion(int index, string newValue)
        {
            TemporarilySetKeyValue(index, 16, "i", newValue);
            UserLevels[index].RawLevel = UserLevels[index].RawLevel.Replace("<k>k16</k><i>" + UserLevels[index].LevelVersion + "</i>", "<k>k16</k><i>" + newValue + "</i>");
            UserLevels[index].LevelVersion = ToInt32(newValue);
        }
        public static void UpdateLevelData()
        {
            File.WriteAllText(GDLocalData + "\\CCLocalLevels.dat", DecryptedLevelData); // Write the level data
        }
        public static void UpdateLevelDataWithEncryption()
        {
            string finalizedLevelData = DecryptedLevelData;
            // Code to encrypt the fucking level data
            File.WriteAllText(GDLocalData + "\\CCLocalLevels.dat", finalizedLevelData); // Write the encrypted level data
        }
        public static void UpdateMemoryLevelData()
        {
            LevelKeyStartIndices = new List<int>();
            StringBuilder lvlDat = new StringBuilder(levelDataStart);
            for (int i = 0; i < UserLevelCount; i++)
            {
                lvlDat = lvlDat.Append("<k>k_" + i + "</k>");
                LevelKeyStartIndices.Add(lvlDat.Length);
                lvlDat = lvlDat.Append(UserLevels[i].RawLevel);
            }
            lvlDat = lvlDat.Append(levelDataEnd);
            DecryptedLevelData = lvlDat.ToString();
        }

        public static void CloneAllLevelInfo(int a, int b)
        {
            UserLevels.Insert(a, UserLevels[b]);
        }
        public static void MoveAllLevelInfo(int a, int b)
        {
            UserLevels.MoveElement(a, b);
        }
        public static void MoveAllLevelInfoToStart(int a)
        {
            UserLevels.MoveElementToStart(a);
        }
        public static void MoveAllLevelInfoToEnd(int a)
        {
            UserLevels.MoveElementToEnd(a);
        }
        public static void RemoveAllLevelInfoAt(int a)
        {
            UserLevels.RemoveAt(a);
        }
        public static void SwapAllLevelInfo(int a, int b)
        {
            UserLevels = UserLevels.Swap(a, b);
        }

        public static string GetData(string path)
        {
            StreamReader sr = new StreamReader(path);
            string readfile = sr.ReadToEnd();
            sr.Close();
            return readfile;
        }
        public static string GetPlayerName()
        {
            int playerNameStartIndex = DecryptedGamesave.FindFromEnd("<k>playerName</k><s>") + 20;
            int playerNameEndIndex = DecryptedGamesave.FindFromEnd("</s><k>playerUserID</k>");
            int playerNameLength = playerNameEndIndex - playerNameStartIndex;
            return DecryptedGamesave.Substring(playerNameStartIndex, playerNameLength);
        }
        public static string GetUserID()
        {
            int userIDStartIndex = DecryptedGamesave.FindFromEnd("<k>playerUserID</k><i>") + 22;
            int userIDEndIndex = DecryptedGamesave.FindFromEnd("</i><k>playerFrame</k>");
            int userIDLength = userIDEndIndex - userIDStartIndex;
            return DecryptedGamesave.Substring(userIDStartIndex, userIDLength);
        }
        public static string GetAccountID()
        {
            int accountIDStartIndex = DecryptedGamesave.FindFromEnd("<k>GJA_003</k><i>") + 17;
            int accountIDEndIndex = DecryptedGamesave.FindFromEnd("</i><k>KBM_001</k>");
            int accountIDLength = accountIDEndIndex - accountIDStartIndex;
            if (accountIDLength > 0)
                return DecryptedGamesave.Substring(accountIDStartIndex, accountIDLength);
            else
                return "0";
        }
        public static List<string> GetFolderNames()
        {
            List<string> names = new List<string>();
            int foldersStartIndex = DecryptedGamesave.FindFromEnd("<k>GLM_19</k><d>") + 16;
            if (foldersStartIndex > 15)
            {
                int foldersEndIndex = DecryptedGamesave.Find("</d>", foldersStartIndex, DecryptedGamesave.Length);
                int currentIndex = foldersStartIndex;
                while ((currentIndex = DecryptedGamesave.Find("<k>", currentIndex, foldersEndIndex) + 3) > 2)
                {
                    int endingIndex = DecryptedGamesave.Find("</k>", currentIndex, foldersEndIndex);
                    int folderIndex = int.Parse(DecryptedGamesave.Substring(currentIndex, endingIndex - currentIndex));
                    int folderNameStartIndex = endingIndex + 7;
                    int folderNameEndIndex = DecryptedGamesave.Find("</s>", folderNameStartIndex, foldersEndIndex);
                    while (folderIndex >= names.Count)
                        names.Add("");
                    names[folderIndex] = DecryptedGamesave.Substring(folderNameStartIndex, folderNameEndIndex - folderNameStartIndex);
                }
            }
            return names;
        }

        public static byte[] Base64Decrypt(string encodedData)
        {
            while (encodedData.Length % 4 != 0)
                encodedData += "=";
            byte[] encodedDataAsBytes = FromBase64String(encodedData);
            return encodedDataAsBytes;
        }
        public static string Base64Encrypt(byte[] decryptedData)
        {
            return ToBase64String(decryptedData);
        }
        public static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                var buffer = new byte[4096];
                int read;

                while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                    resultStream.Write(buffer, 0, read);

                return resultStream.ToArray();
            }
        }
        public static byte[] Compress(byte[] data)
        {
            using (var decompressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(decompressedStream, CompressionMode.Compress))
            using (var resultStream = new MemoryStream())
            {
                var buffer = new byte[4096];
                int read;

                while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                    resultStream.Write(buffer, 0, read);

                return resultStream.ToArray();
            }
        }
        public static string XORDecrypt(string text, int key)
        {
            byte[] result = new byte[text.Length];
            for (int c = 0; c < text.Length; c++)
                result[c] = (byte)((uint)text[c] ^ key);
            string dexored = Encoding.UTF8.GetString(result);
            return dexored;
        }
        public static string XOR11Decrypt(string text)
        {
            byte[] result = new byte[text.Length];
            for (int c = 0; c < text.Length - 1; c++)
                result[c] = (byte)((uint)text[c] ^ 11);
            string dexored = Encoding.UTF8.GetString(result);
            return dexored;
        }
        public static string GDGamesaveDecrypt(string data)
        {
            string xored = XOR11Decrypt(data); // Decrypt XOR ^ 11
            string replaced = xored.Replace('-', '+').Replace('_', '/').Replace("\0", string.Empty); // Replace characters
            byte[] gzipb64 = Decompress(Base64Decrypt(replaced)); // Decompress
            string result = Encoding.ASCII.GetString(gzipb64); // Change to string
            return result;
        }
        public static string GDLevelStringDecrypt(string ls)
        {
            string replaced = ls.Replace('-', '+').Replace('_', '/').Replace("\0", string.Empty); // Replace characters
            //string fixedBase64 = replaced.FixBase64String();
            byte[] gzipb64 = Base64Decrypt(replaced);
            if (replaced.StartsWith("H4sIAAAAAAAA"))
                gzipb64 = Decompress(gzipb64); // Decompress
            else throw new ArgumentException("The level string is not valid.");
            string result = Encoding.ASCII.GetString(gzipb64); // Change to string
            return result;
        }
    }
}