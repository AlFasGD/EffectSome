using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EffectSome.Objects.CopyPasteSettings;

namespace EffectSome
{
    public class GlobalParameterSettingsPreset
    {
        public string PresetName;
        public AutoCopyPasteMode CopyPasteMode;
        public int AutoCopyPasteTimes;
        public float AutoCopyPasteSpecifiedLocationX;
        public float AutoCopyPasteSpecifiedLocationY;
        public bool AutoCopyPasteSpecifiedLocationXEnabled;
        public bool AutoCopyPasteSpecifiedLocationYEnabled;
        public float AutoCopyPasteSpecifiedDistanceX;
        public float AutoCopyPasteSpecifiedDistanceY;
        public bool AutoCopyPasteSpecifiedDistanceXEnabled;
        public bool AutoCopyPasteSpecifiedDistanceYEnabled;
        public bool MoveCopyPastedObjects;
        public float AutoCopyPasteMoveX;
        public float AutoCopyPasteMoveY;
        public bool AutoCopyPasteMoveXEnabled;
        public bool AutoCopyPasteMoveYEnabled;
        public AdjustmentMode AdjustIDAdjustmentMode;
        public int AdjustIDsAdjustment;
        public List<int> AdjustIDsSpecifiedValues;
        public AdjustmentMode AutoAddGroupIDAdjustmentMode;
        public int AutoAddGroupIDsAdjustment;
        public List<int> AutoAddGroupIDsSpecifiedValues;
        public bool[] AutoAddGroupIDsAdjustedGroupIDs = new bool[10];
        public bool AutoAddGroupIDsChooseGroupIDsToAdjust;

        public enum AutoCopyPasteMode
        {
            NumberOfTimes = 0,
            SpecifiedLocation = 1,
            SpecifiedDistance = 2
        }

        /// <summary>Creates a new instance of the <see cref="GlobalParameterSettingsPreset"/> class with a specified preset name.</summary>
        /// <param name="presetName">The name of the preset to create.</param>
        public GlobalParameterSettingsPreset(string presetName)
        {
            PresetName = presetName;
            AdjustIDsSpecifiedValues = new List<int>();
            AutoAddGroupIDsSpecifiedValues = new List<int>();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append((int)CopyPasteMode + "|");
            result.Append(AutoCopyPasteTimes + "|");
            result.Append(AutoCopyPasteSpecifiedLocationX + "|");
            result.Append(AutoCopyPasteSpecifiedLocationY + "|");
            result.Append(AutoCopyPasteSpecifiedLocationXEnabled + "|");
            result.Append(AutoCopyPasteSpecifiedLocationYEnabled + "|");
            result.Append(AutoCopyPasteSpecifiedDistanceX + "|");
            result.Append(AutoCopyPasteSpecifiedDistanceY + "|");
            result.Append(AutoCopyPasteSpecifiedDistanceXEnabled + "|");
            result.Append(AutoCopyPasteSpecifiedDistanceYEnabled + "|");
            result.Append(MoveCopyPastedObjects + "|");
            result.Append(AutoCopyPasteMoveX + "|");
            result.Append(AutoCopyPasteMoveY + "|");
            result.Append(AutoCopyPasteMoveXEnabled + "|");
            result.Append(AutoCopyPasteMoveYEnabled + "|");
            result.Append((int)AdjustIDAdjustmentMode + "|");
            result.Append(AdjustIDsAdjustment + "|");
            for (int i = 0; i < AdjustIDsSpecifiedValues.Count; i++)
                result.Append(AdjustIDsSpecifiedValues[i] + ":");
            if (AdjustIDsSpecifiedValues.Count > 0)
                result = result.Remove(result.Length - 1, 1);
            result.Append("|");
            result.Append((int)AutoAddGroupIDAdjustmentMode + "|");
            result.Append(AutoAddGroupIDsAdjustment + "|");
            for (int i = 0; i < AutoAddGroupIDsSpecifiedValues.Count; i++)
                result.Append(AutoAddGroupIDsSpecifiedValues[i] + ":");
            if (AutoAddGroupIDsSpecifiedValues.Count > 0)
                result = result.Remove(result.Length - 1, 1);
            result.Append("|");
            for (int i = 0; i < AutoAddGroupIDsAdjustedGroupIDs.Length; i++)
                result.Append(AutoAddGroupIDsAdjustedGroupIDs[i] + ":");
            if (AutoAddGroupIDsAdjustedGroupIDs.Length > 0)
                result = result.Remove(result.Length - 1, 1);
            result.Append("|");
            result.Append(AutoAddGroupIDsChooseGroupIDsToAdjust); // Misery ends here
            return result.ToString();
        }
        public string ToString(GlobalParameterSettings.Tab t)
        {
            StringBuilder result = new StringBuilder();
            if (t == GlobalParameterSettings.Tab.AutoCopyPaste)
            {
                result.Append((int)CopyPasteMode + "|");
                result.Append(AutoCopyPasteTimes + "|");
                result.Append(AutoCopyPasteSpecifiedLocationX + "|");
                result.Append(AutoCopyPasteSpecifiedLocationY + "|");
                result.Append(AutoCopyPasteSpecifiedLocationXEnabled + "|");
                result.Append(AutoCopyPasteSpecifiedLocationYEnabled + "|");
                result.Append(AutoCopyPasteSpecifiedDistanceX + "|");
                result.Append(AutoCopyPasteSpecifiedDistanceY + "|");
                result.Append(AutoCopyPasteSpecifiedDistanceXEnabled + "|");
                result.Append(AutoCopyPasteSpecifiedDistanceYEnabled + "|");
                result.Append(MoveCopyPastedObjects + "|");
                result.Append(AutoCopyPasteMoveX + "|");
                result.Append(AutoCopyPasteMoveY + "|");
                result.Append(AutoCopyPasteMoveXEnabled + "|");
                result.Append(AutoCopyPasteMoveYEnabled);
            }
            else if (t == GlobalParameterSettings.Tab.AdjustIDs)
            {
                result.Append((int)AdjustIDAdjustmentMode + "|");
                result.Append(AdjustIDsAdjustment + "|");
                for (int i = 0; i < AdjustIDsSpecifiedValues.Count; i++)
                    result.Append(AdjustIDsSpecifiedValues[i] + ":");
                if (AdjustIDsSpecifiedValues.Count > 0)
                    result = result.Remove(result.Length - 1, 1);
            }
            else
            {
                result.Append((int)AutoAddGroupIDAdjustmentMode + "|");
                result.Append(AutoAddGroupIDsAdjustment + "|");
                for (int i = 0; i < AutoAddGroupIDsSpecifiedValues.Count; i++)
                    result.Append(AutoAddGroupIDsSpecifiedValues[i] + ":");
                if (AutoAddGroupIDsSpecifiedValues.Count > 0)
                    result = result.Remove(result.Length - 1, 1);
                result.Append("|");
                for (int i = 0; i < AutoAddGroupIDsAdjustedGroupIDs.Length; i++)
                    result.Append(AutoAddGroupIDsAdjustedGroupIDs[i] + ":");
                if (AutoAddGroupIDsAdjustedGroupIDs.Length > 0)
                    result = result.Remove(result.Length - 1, 1);
                result.Append("|");
                result.Append(AutoAddGroupIDsChooseGroupIDsToAdjust); // Misery ends here
            }
            return result.ToString();
        }
        // Stupid autism in my ass - If there is a better way to do this I swear I'm gonna kill myself
        public bool Equals(GlobalParameterSettingsPreset right, GlobalParameterSettings.Tab t)
        {
            bool result = true;
            if (t == GlobalParameterSettings.Tab.AutoCopyPaste)
            {
                result &= CopyPasteMode == right.CopyPasteMode;
                if (!result) return false;
                result &= AutoCopyPasteTimes == right.AutoCopyPasteTimes;
                if (!result) return false;
                result &= AutoCopyPasteSpecifiedLocationX == right.AutoCopyPasteSpecifiedLocationX;
                if (!result) return false;
                result &= AutoCopyPasteSpecifiedLocationY == right.AutoCopyPasteSpecifiedLocationY;
                if (!result) return false;
                result &= AutoCopyPasteSpecifiedLocationXEnabled == right.AutoCopyPasteSpecifiedLocationXEnabled;
                if (!result) return false;
                result &= AutoCopyPasteSpecifiedLocationYEnabled == right.AutoCopyPasteSpecifiedLocationYEnabled;
                if (!result) return false;
                result &= AutoCopyPasteSpecifiedDistanceX == right.AutoCopyPasteSpecifiedDistanceX;
                if (!result) return false;
                result &= AutoCopyPasteSpecifiedDistanceY == right.AutoCopyPasteSpecifiedDistanceY;
                if (!result) return false;
                result &= AutoCopyPasteSpecifiedDistanceXEnabled == right.AutoCopyPasteSpecifiedDistanceXEnabled;
                if (!result) return false;
                result &= AutoCopyPasteSpecifiedDistanceYEnabled == right.AutoCopyPasteSpecifiedDistanceYEnabled;
                if (!result) return false;
                result &= MoveCopyPastedObjects == right.MoveCopyPastedObjects;
                if (!result) return false;
                result &= AutoCopyPasteMoveX == right.AutoCopyPasteMoveX;
                if (!result) return false;
                result &= AutoCopyPasteMoveY == right.AutoCopyPasteMoveY;
                if (!result) return false;
                result &= AutoCopyPasteMoveXEnabled == right.AutoCopyPasteMoveXEnabled;
                if (!result) return false;
                result &= AutoCopyPasteMoveYEnabled == right.AutoCopyPasteMoveYEnabled;
            }
            else if (t == GlobalParameterSettings.Tab.AdjustIDs)
            {
                result &= AdjustIDAdjustmentMode == right.AdjustIDAdjustmentMode;
                if (!result) return false;
                result &= AdjustIDsAdjustment == right.AdjustIDsAdjustment;
                if (!result) return false;
                result &= AdjustIDsSpecifiedValues == right.AdjustIDsSpecifiedValues;
            }
            else if (t == GlobalParameterSettings.Tab.AutoAddGroupIDs)
            {
                result &= AutoAddGroupIDAdjustmentMode == right.AutoAddGroupIDAdjustmentMode;
                if (!result) return false;
                result &= AutoAddGroupIDsAdjustment == right.AutoAddGroupIDsAdjustment;
                if (!result) return false;
                result &= AutoAddGroupIDsSpecifiedValues == right.AutoAddGroupIDsSpecifiedValues;
                if (!result) return false;
                result &= AutoAddGroupIDsAdjustedGroupIDs == right.AutoAddGroupIDsAdjustedGroupIDs;
                if (!result) return false;
                result &= AutoAddGroupIDsChooseGroupIDsToAdjust == right.AutoAddGroupIDsChooseGroupIDsToAdjust;
            }
            return result;
        }
        public void From(GlobalParameterSettingsPreset right, GlobalParameterSettings.Tab t)
        {
            if (t == GlobalParameterSettings.Tab.AutoCopyPaste)
            {
                CopyPasteMode = right.CopyPasteMode;
                AutoCopyPasteTimes = right.AutoCopyPasteTimes;
                AutoCopyPasteSpecifiedLocationX = right.AutoCopyPasteSpecifiedLocationX;
                AutoCopyPasteSpecifiedLocationY = right.AutoCopyPasteSpecifiedLocationY;
                AutoCopyPasteSpecifiedLocationXEnabled = right.AutoCopyPasteSpecifiedLocationXEnabled;
                AutoCopyPasteSpecifiedLocationYEnabled = right.AutoCopyPasteSpecifiedLocationYEnabled;
                AutoCopyPasteSpecifiedDistanceX = right.AutoCopyPasteSpecifiedDistanceX;
                AutoCopyPasteSpecifiedDistanceY = right.AutoCopyPasteSpecifiedDistanceY;
                AutoCopyPasteSpecifiedDistanceXEnabled = right.AutoCopyPasteSpecifiedDistanceXEnabled;
                AutoCopyPasteSpecifiedDistanceYEnabled = right.AutoCopyPasteSpecifiedDistanceYEnabled;
                MoveCopyPastedObjects = right.MoveCopyPastedObjects;
                AutoCopyPasteMoveX = right.AutoCopyPasteMoveX;
                AutoCopyPasteMoveY = right.AutoCopyPasteMoveY;
                AutoCopyPasteMoveXEnabled = right.AutoCopyPasteMoveXEnabled;
                AutoCopyPasteMoveYEnabled = right.AutoCopyPasteMoveYEnabled;
            }
            else if (t == GlobalParameterSettings.Tab.AdjustIDs)
            {
                AdjustIDAdjustmentMode = right.AdjustIDAdjustmentMode;
                AdjustIDsAdjustment = right.AdjustIDsAdjustment;
                AdjustIDsSpecifiedValues = right.AdjustIDsSpecifiedValues.Clone();
            }
            else
            {
                AutoAddGroupIDAdjustmentMode = right.AutoAddGroupIDAdjustmentMode;
                AutoAddGroupIDsAdjustment = right.AutoAddGroupIDsAdjustment;
                AutoAddGroupIDsSpecifiedValues = right.AutoAddGroupIDsSpecifiedValues.Clone();
                AutoAddGroupIDsAdjustedGroupIDs = right.AutoAddGroupIDsAdjustedGroupIDs.CopyArray();
                AutoAddGroupIDsChooseGroupIDsToAdjust = right.AutoAddGroupIDsChooseGroupIDsToAdjust;
            }
        }
        public GlobalParameterSettingsPreset Clone()
        {
            GlobalParameterSettingsPreset result = new GlobalParameterSettingsPreset(PresetName)
            {
                CopyPasteMode = CopyPasteMode,
                AutoCopyPasteTimes = AutoCopyPasteTimes,
                AutoCopyPasteSpecifiedLocationX = AutoCopyPasteSpecifiedLocationX,
                AutoCopyPasteSpecifiedLocationY = AutoCopyPasteSpecifiedLocationY,
                AutoCopyPasteSpecifiedLocationXEnabled = AutoCopyPasteSpecifiedLocationXEnabled,
                AutoCopyPasteSpecifiedLocationYEnabled = AutoCopyPasteSpecifiedLocationYEnabled,
                AutoCopyPasteSpecifiedDistanceX = AutoCopyPasteSpecifiedDistanceX,
                AutoCopyPasteSpecifiedDistanceY = AutoCopyPasteSpecifiedDistanceY,
                AutoCopyPasteSpecifiedDistanceXEnabled = AutoCopyPasteSpecifiedDistanceXEnabled,
                AutoCopyPasteSpecifiedDistanceYEnabled = AutoCopyPasteSpecifiedDistanceYEnabled,
                MoveCopyPastedObjects = MoveCopyPastedObjects,
                AutoCopyPasteMoveX = AutoCopyPasteMoveX,
                AutoCopyPasteMoveY = AutoCopyPasteMoveY,
                AutoCopyPasteMoveXEnabled = AutoCopyPasteMoveXEnabled,
                AutoCopyPasteMoveYEnabled = AutoCopyPasteMoveYEnabled,
                AdjustIDAdjustmentMode = AdjustIDAdjustmentMode,
                AdjustIDsAdjustment = AdjustIDsAdjustment,
                AdjustIDsSpecifiedValues = AdjustIDsSpecifiedValues.Clone(),
                AutoAddGroupIDAdjustmentMode = AutoAddGroupIDAdjustmentMode,
                AutoAddGroupIDsAdjustment = AutoAddGroupIDsAdjustment,
                AutoAddGroupIDsSpecifiedValues = AutoAddGroupIDsSpecifiedValues.Clone(),
                AutoAddGroupIDsAdjustedGroupIDs = AutoAddGroupIDsAdjustedGroupIDs.CopyArray(),
                AutoAddGroupIDsChooseGroupIDsToAdjust = AutoAddGroupIDsChooseGroupIDsToAdjust
            };
            return result;
        }
        public static bool operator ==(GlobalParameterSettingsPreset left, GlobalParameterSettingsPreset right)
        {
            bool result = true;
            result &= left.CopyPasteMode == right.CopyPasteMode;
            if (!result) return false;
            result &= left.AutoCopyPasteTimes == right.AutoCopyPasteTimes;
            if (!result) return false;
            result &= left.AutoCopyPasteSpecifiedLocationX == right.AutoCopyPasteSpecifiedLocationX;
            if (!result) return false;
            result &= left.AutoCopyPasteSpecifiedLocationY == right.AutoCopyPasteSpecifiedLocationY;
            if (!result) return false;
            result &= left.AutoCopyPasteSpecifiedLocationXEnabled == right.AutoCopyPasteSpecifiedLocationXEnabled;
            if (!result) return false;
            result &= left.AutoCopyPasteSpecifiedLocationYEnabled == right.AutoCopyPasteSpecifiedLocationYEnabled;
            if (!result) return false;
            result &= left.AutoCopyPasteSpecifiedDistanceX == right.AutoCopyPasteSpecifiedDistanceX;
            if (!result) return false;
            result &= left.AutoCopyPasteSpecifiedDistanceY == right.AutoCopyPasteSpecifiedDistanceY;
            if (!result) return false;
            result &= left.AutoCopyPasteSpecifiedDistanceXEnabled == right.AutoCopyPasteSpecifiedDistanceXEnabled;
            if (!result) return false;
            result &= left.AutoCopyPasteSpecifiedDistanceYEnabled == right.AutoCopyPasteSpecifiedDistanceYEnabled;
            if (!result) return false;
            result &= left.MoveCopyPastedObjects == right.MoveCopyPastedObjects;
            if (!result) return false;
            result &= left.AutoCopyPasteMoveX == right.AutoCopyPasteMoveX;
            if (!result) return false;
            result &= left.AutoCopyPasteMoveY == right.AutoCopyPasteMoveY;
            if (!result) return false;
            result &= left.AutoCopyPasteMoveXEnabled == right.AutoCopyPasteMoveXEnabled;
            if (!result) return false;
            result &= left.AutoCopyPasteMoveYEnabled == right.AutoCopyPasteMoveYEnabled;
            if (!result) return false;
            result &= left.AdjustIDAdjustmentMode == right.AdjustIDAdjustmentMode;
            if (!result) return false;
            result &= left.AdjustIDsAdjustment == right.AdjustIDsAdjustment;
            if (!result) return false;
            result &= left.AdjustIDsSpecifiedValues == right.AdjustIDsSpecifiedValues;
            if (!result) return false;
            result &= left.AutoAddGroupIDAdjustmentMode == right.AutoAddGroupIDAdjustmentMode;
            if (!result) return false;
            result &= left.AutoAddGroupIDsAdjustment == right.AutoAddGroupIDsAdjustment;
            if (!result) return false;
            result &= left.AutoAddGroupIDsSpecifiedValues == right.AutoAddGroupIDsSpecifiedValues;
            if (!result) return false;
            result &= left.AutoAddGroupIDsAdjustedGroupIDs == right.AutoAddGroupIDsAdjustedGroupIDs;
            if (!result) return false;
            result &= left.AutoAddGroupIDsChooseGroupIDsToAdjust == right.AutoAddGroupIDsChooseGroupIDsToAdjust;
            return result;
        }
        public static bool operator !=(GlobalParameterSettingsPreset left, GlobalParameterSettingsPreset right)
        {
            bool result = false;
            result |= left.CopyPasteMode == right.CopyPasteMode;
            if (result) return false;
            result |= left.AutoCopyPasteTimes == right.AutoCopyPasteTimes;
            if (result) return false;
            result |= left.AutoCopyPasteSpecifiedLocationX == right.AutoCopyPasteSpecifiedLocationX;
            if (result) return false;
            result |= left.AutoCopyPasteSpecifiedLocationY == right.AutoCopyPasteSpecifiedLocationY;
            if (result) return false;
            result |= left.AutoCopyPasteSpecifiedLocationXEnabled == right.AutoCopyPasteSpecifiedLocationXEnabled;
            if (result) return false;
            result |= left.AutoCopyPasteSpecifiedLocationYEnabled == right.AutoCopyPasteSpecifiedLocationYEnabled;
            if (result) return false;
            result |= left.AutoCopyPasteSpecifiedDistanceX == right.AutoCopyPasteSpecifiedDistanceX;
            if (result) return false;
            result |= left.AutoCopyPasteSpecifiedDistanceY == right.AutoCopyPasteSpecifiedDistanceY;
            if (result) return false;
            result |= left.AutoCopyPasteSpecifiedDistanceXEnabled == right.AutoCopyPasteSpecifiedDistanceXEnabled;
            if (result) return false;
            result |= left.AutoCopyPasteSpecifiedDistanceYEnabled == right.AutoCopyPasteSpecifiedDistanceYEnabled;
            if (result) return false;
            result |= left.MoveCopyPastedObjects == right.MoveCopyPastedObjects;
            if (result) return false;
            result |= left.AutoCopyPasteMoveX == right.AutoCopyPasteMoveX;
            if (result) return false;
            result |= left.AutoCopyPasteMoveY == right.AutoCopyPasteMoveY;
            if (result) return false;
            result |= left.AutoCopyPasteMoveXEnabled == right.AutoCopyPasteMoveXEnabled;
            if (result) return false;
            result |= left.AutoCopyPasteMoveYEnabled == right.AutoCopyPasteMoveYEnabled;
            if (result) return false;
            result |= left.AdjustIDAdjustmentMode == right.AdjustIDAdjustmentMode;
            if (result) return false;
            result |= left.AdjustIDsAdjustment == right.AdjustIDsAdjustment;
            if (result) return false;
            result |= left.AdjustIDsSpecifiedValues == right.AdjustIDsSpecifiedValues;
            if (result) return false;
            result |= left.AutoAddGroupIDAdjustmentMode == right.AutoAddGroupIDAdjustmentMode;
            if (result) return false;
            result |= left.AutoAddGroupIDsAdjustment == right.AutoAddGroupIDsAdjustment;
            if (result) return false;
            result |= left.AutoAddGroupIDsSpecifiedValues == right.AutoAddGroupIDsSpecifiedValues;
            if (result) return false;
            result |= left.AutoAddGroupIDsAdjustedGroupIDs == right.AutoAddGroupIDsAdjustedGroupIDs;
            if (result) return false;
            result |= left.AutoAddGroupIDsChooseGroupIDsToAdjust == right.AutoAddGroupIDsChooseGroupIDsToAdjust;
            return !result;
        }
    }
}