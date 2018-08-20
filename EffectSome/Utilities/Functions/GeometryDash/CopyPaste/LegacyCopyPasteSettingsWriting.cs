using EffectSome.Objects.CopyPasteSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EffectSome.EffectSome;
using static EffectSome.Utilities.Functions.General.UsefulFunctions;

namespace EffectSome.Utilities.Functions.GeometryDash.CopyPaste
{
    public static class LegacyCopyPasteSettingsWriting
    {
        public static string CPSettingsPath = GDLocalData + @"\tmp\cpa\";

        public static string GenerateStringFromArray(bool[] param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Length; i++)
                builder = builder.Append($"{Convert.ToInt32(param[i])}:");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public static string GenerateStringFromArray(int[] param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Length; i++)
                builder = builder.Append($"{param[i]}:");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public static string GenerateStringFromArray(AdjustmentMode[] param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Length; i++)
                builder = builder.Append($"{(int)param[i]}:");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public static string GenerateStringFromList(List<bool> param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Count; i++)
                builder = builder.Append($"{Convert.ToInt32(param[i])}:");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public static string GenerateStringFromList(List<int> param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Count; i++)
                builder = builder.Append($"{param[i]}:");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public static string GenerateStringFromList(List<float> param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Count; i++)
                builder = builder.Append($"{param[i]}:");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public static string GenerateStringFromList(List<AdjustmentMode> param)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < param.Count; i++)
                builder = builder.Append($"{(int)param[i]}:");
            builder = builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public static void WriteCopyPasteAutomationSettings()
        {
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
                toWrite[0].Append($"{EffectSome.CopyPasteSettings[i].X}_");
                toWrite[1].Append($"{EffectSome.CopyPasteSettings[i].Y}_");
                toWrite[2].Append($"{EffectSome.CopyPasteSettings[i].Scaling}_");
                toWrite[3].Append($"{EffectSome.CopyPasteSettings[i].Rotation}_");
                toWrite[4].Append($"{EffectSome.CopyPasteSettings[i].EL1}_");
                toWrite[5].Append($"{EffectSome.CopyPasteSettings[i].EL2}_");
                toWrite[6].Append($"{EffectSome.CopyPasteSettings[i].ZOrder}_");
                toWrite[7].Append($"{EffectSome.CopyPasteSettings[i].ZLayer}_");
                toWrite[8].Append($"{GenerateStringFromList(EffectSome.CopyPasteSettings[i].Color1IDs)}_");
                toWrite[9].Append($"{GenerateStringFromList(EffectSome.CopyPasteSettings[i].Color2IDs)}_");
                toWrite[10].Append($"{EffectSome.CopyPasteSettings[i].Hue1}_");
                toWrite[11].Append($"{EffectSome.CopyPasteSettings[i].Saturation1}_");
                toWrite[12].Append($"{EffectSome.CopyPasteSettings[i].Brightness1}_");
                toWrite[13].Append($"{EffectSome.CopyPasteSettings[i].Hue2}_");
                toWrite[14].Append($"{EffectSome.CopyPasteSettings[i].Saturation2}_");
                toWrite[15].Append($"{EffectSome.CopyPasteSettings[i].Brightness2}_");
                toWrite[16].Append($"{EffectSome.CopyPasteSettings[i].Color1IDValueCounter}_");
                toWrite[17].Append($"{EffectSome.CopyPasteSettings[i].Color2IDValueCounter}_");
                toWrite[18].Append($"{(int)EffectSome.CopyPasteSettings[i].Color1IDValueAdjustmentMode}_");
                toWrite[19].Append($"{(int)EffectSome.CopyPasteSettings[i].Color2IDValueAdjustmentMode}_");
                for (int j = 0; j < 10; j++)
                {
                    toWrite[20 + 3 * j].Append($"{GenerateStringFromList(EffectSome.CopyPasteSettings[i].GroupIDs)}_");
                    toWrite[21 + 3 * j].Append($"{EffectSome.CopyPasteSettings[i].GroupIDValueCounters[j]}_");
                    toWrite[22 + 3 * j].Append($"{(int)EffectSome.CopyPasteSettings[i].GroupIDValueAdjustmentModes[j]}_");
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
