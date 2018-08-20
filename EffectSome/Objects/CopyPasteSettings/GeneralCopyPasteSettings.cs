using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectSome.Objects.CopyPasteSettings
{
    public class GeneralCopyPasteSettings
    {
        #region Copy-Paste Automation Settings
        // General parameters that apply to every object type
        // When the array only contains -1 then the settings are to be applied for all objects except triggers and special objects
        // When the value of the Group/Item/Block ID adjustment is -1,
        // the IDs of the new objects are to be set to the next free
        
        #region Counters
        public int Color1IDValueCounter;
        public int Color2IDValueCounter;
        public int[] GroupIDValueCounters = new int[10];
        #endregion

        #region Adjustment Modes
        public AdjustmentMode Color1IDValueAdjustmentMode;
        public AdjustmentMode Color2IDValueAdjustmentMode;
        public AdjustmentMode[] GroupIDValueAdjustmentModes = new AdjustmentMode[10];
        #endregion
        
        #region General Objects Parameters
        public List<int> ObjectIDs;
        public float X;
        public float Y;
        public float Scaling;
        public float Rotation;
        public float Hue1;
        public float Saturation1;
        public float Brightness1;
        public float Hue2;
        public float Saturation2;
        public float Brightness2;
        public int EL1;
        public int EL2;
        public int ZOrder;
        public int ZLayer;
        public List<int> Color1IDs;
        public List<int> Color2IDs;
        public List<int> GroupIDs;
        #endregion
        #endregion

        public GeneralCopyPasteSettings()
        {
            ObjectIDs = new List<int> { -1 };
            Color1IDs = new List<int> { 0 };
            Color2IDs = new List<int> { 0 };
            GroupIDs = new List<int> { 0 };
            for (int i = 0; i < 10; i++)
            {
                GroupIDValueCounters[i] = 0;
                GroupIDValueAdjustmentModes[i] = AdjustmentMode.FlatAdjustment;
            }
        }
    }

    // FOR ACTUAL FUCK'S SAKE USE A FUCKING ENUM TO DISTINGUISH THE VALUES PROPERLY
    public enum AdjustmentMode
    {
        FlatAdjustment = 0,
        SpecificValues = 1,
        UnusedIDs = 2,
    }
}