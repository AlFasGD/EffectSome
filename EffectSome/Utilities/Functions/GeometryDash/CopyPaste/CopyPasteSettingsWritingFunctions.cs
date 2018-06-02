﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static EffectSome.EffectSome;
using static EffectSome.Gamesave;
using static EffectSome.UsefulFunctions;
using static System.IO.File;

namespace EffectSome
{
    public static class CopyPasteSettingsWritingFunctions
    {
        public static string CPSettingsPath = GDLocalData + @"\tmp\cpa\";
        
        public static string GenerateStringFromArray(bool[] param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Length; i++)
                builder = builder.Append($"{Convert.ToInt32(param[i])}_");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public static string GenerateStringFromArray(int[] param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Length; i++)
                builder = builder.Append($"{param[i]}_");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public static string GenerateStringFromArray(AdjustmentMode[] param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Length; i++)
                builder = builder.Append($"{(int)param[i]}_");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public static string GenerateStringFromList(List<bool> param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Count; i++)
                builder = builder.Append($"{Convert.ToInt32(param[i])}_");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public static string GenerateStringFromList(List<int> param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Count; i++)
                builder = builder.Append($"{param[i]}_");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public static string GenerateStringFromList(List<float> param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Count; i++)
                builder = builder.Append($"{param[i]}_");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public static string GenerateStringFromList(List<AdjustmentMode> param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Count; i++)
                builder = builder.Append($"{(int)param[i]}_");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        #region Experimental
        // Probably try to make something good like that?
        public static string GenerateLineForObjectCopyPasteSettings(Predicate<bool> b, string name)
        {
            StringBuilder s = new StringBuilder($"{name};");
            for (int i = 0; i < EffectSome.CopyPasteSettings.Count; i++)
            {
                // I don't know how to use predicates
            }
            return s.ToString();
        }
        public static string GenerateLineForX()
        {
            StringBuilder s = new StringBuilder("x;");
            for (int i = 0; i < EffectSome.CopyPasteSettings.Count; i++)
            {
                // I don't know how to use predicates
            }
            return s.ToString();
        }
        #endregion
        
        public static void RemoveEmptySettings()
        {
            for (int i = EffectSome.CopyPasteSettings.Count - 1; i >= 0; i--)
                if (EffectSome.CopyPasteSettings[i].ObjectIDs.Count == 0)
                    EffectSome.CopyPasteSettings.RemoveAt(i);
        }
        
        public static int ChangeCopyPasteAutomationSettings(List<int> newObjectIDs)
        {
            // TODO: Work on this
            if (newObjectIDs[0] == -1)
            {
                return -1;
            }
            else // Else if the new settings are just for the selected Object IDs
            {
                for (int i = 0; i < EffectSome.CopyPasteSettings.Count; i++)
                {
                    if (newObjectIDs.ContainsAll(EffectSome.CopyPasteSettings[i].ObjectIDs))
                        return i;
                }
                for (int a = 0; a < newObjectIDs.Count; a++)
                {
                    bool found = false;
                    for (int i = 0; i < EffectSome.CopyPasteSettings.Count && !found; i++)
                    {
                        int index = EffectSome.CopyPasteSettings[i].ObjectIDs.FindIndexInList(newObjectIDs[a]);
                        found = index > -1;
                        if (found)
                            EffectSome.CopyPasteSettings[i].ObjectIDs.RemoveAt(index);
                    }
                }
                RemoveEmptySettings();
                EffectSome.CopyPasteSettings.Insert(0, new CopyPasteSettings { ObjectIDs = newObjectIDs.Clone() }); // Add the list before the Object ID list containing -1
                return 0;
            }
        }

        public static void InitializeCopyPasteAutomationSettings()
        {
            EffectSome.CopyPasteSettings = new List<CopyPasteSettings> { new CopyPasteSettings() };
        }
        
        public static void WriteCopyPasteAutomationSettings()
        {
            // Unsure if this has to be recoded, functions are prepared on the top of this file just in case
            List<StringBuilder> toWrite = new List<StringBuilder>
            {
                new StringBuilder("x;"),
                new StringBuilder("y;"),
                new StringBuilder("scaling;"),
                new StringBuilder("rotation;"),
                new StringBuilder("el1;"),
                new StringBuilder("el2;"),
                new StringBuilder("zorder;"),
                new StringBuilder("zlayer;"),
                new StringBuilder("col1;"),
                new StringBuilder("col2;"),
                new StringBuilder("hue1;"),
                new StringBuilder("sat1;"),
                new StringBuilder("val1;"),
                new StringBuilder("hue2;"),
                new StringBuilder("sat2;"),
                new StringBuilder("val2;"),
                new StringBuilder("col1valc;"),
                new StringBuilder("col2valc;"),
                new StringBuilder("col1adj;"),
                new StringBuilder("col2adj;"),
            };
            for (int j = 0; j < 10; j++)
            {
                toWrite.Add(new StringBuilder("groupid" + j + ";"));
                toWrite.Add(new StringBuilder("groupid" + j + "valc;"));
                toWrite.Add(new StringBuilder("groupid" + j + "adj;"));
            }
            for (int i = 0; i < EffectSome.CopyPasteSettings.Count; i++)
            {
                toWrite[0].Append($"{EffectSome.CopyPasteSettings[i].X}:");
                toWrite[1].Append($"{EffectSome.CopyPasteSettings[i].Y}:");
                toWrite[2].Append($"{EffectSome.CopyPasteSettings[i].Scaling}:");
                toWrite[3].Append($"{EffectSome.CopyPasteSettings[i].Rotation}:");
                toWrite[4].Append($"{EffectSome.CopyPasteSettings[i].EL1}:");
                toWrite[5].Append($"{EffectSome.CopyPasteSettings[i].EL2}:");
                toWrite[6].Append($"{EffectSome.CopyPasteSettings[i].ZOrder}:");
                toWrite[7].Append($"{EffectSome.CopyPasteSettings[i].ZLayer}:");
                toWrite[8].Append($"{GenerateStringFromList(EffectSome.CopyPasteSettings[i].Color1IDs)}:");
                toWrite[9].Append($"{GenerateStringFromList(EffectSome.CopyPasteSettings[i].Color2IDs)}:");
                toWrite[10].Append($"{EffectSome.CopyPasteSettings[i].Hue1}:");
                toWrite[11].Append($"{EffectSome.CopyPasteSettings[i].Saturation1}:");
                toWrite[12].Append($"{EffectSome.CopyPasteSettings[i].Brightness1}:");
                toWrite[13].Append($"{EffectSome.CopyPasteSettings[i].Hue2}:");
                toWrite[14].Append($"{EffectSome.CopyPasteSettings[i].Saturation2}:");
                toWrite[15].Append($"{EffectSome.CopyPasteSettings[i].Brightness2}:");
                toWrite[16].Append($"{EffectSome.CopyPasteSettings[i].Color1IDValueCounter}:");
                toWrite[17].Append($"{EffectSome.CopyPasteSettings[i].Color2IDValueCounter}:");
                toWrite[18].Append($"{(int)EffectSome.CopyPasteSettings[i].Color1IDValueAdjustmentMode}:");
                toWrite[19].Append($"{(int)EffectSome.CopyPasteSettings[i].Color2IDValueAdjustmentMode}:");
                for (int j = 0; j < 10; j++)
                {
                    toWrite[20 + 3 * j].Append($"{GenerateStringFromList(EffectSome.CopyPasteSettings[i].GroupIDs)}:");
                    toWrite[21 + 3 * j].Append($"{EffectSome.CopyPasteSettings[i].GroupIDValueCounters[j]}:");
                    toWrite[22 + 3 * j].Append($"{(int)EffectSome.CopyPasteSettings[i].GroupIDValueAdjustmentModes[j]}:");
                }
            }
            for (int i = 0; i < toWrite.Count; i++)
                toWrite[i] = toWrite[i].Remove(toWrite[i].Length - 1, 1);
            WriteAllLinesWithoutUselessNewLine(CPSettingsPath + "settings.tmp", toWrite);
        }
        public static void WriteCopyPasteAutomationTargetObjectIDs()
        {
            List<string> toWrite = new List<string>();
            for (int i = 0; i < EffectSome.CopyPasteSettings.Count; i++)
                toWrite.Add(GenerateStringFromList(EffectSome.CopyPasteSettings[i].ObjectIDs));
            WriteAllLinesWithoutUselessNewLine(CPSettingsPath + "objectIDs.tmp", toWrite);
        }
        public static void WriteAllObjectCopyPasteAutomationSettings()
        {
            WriteCopyPasteAutomationSettings();
            WriteCopyPasteAutomationTargetObjectIDs();
        }
    }
}