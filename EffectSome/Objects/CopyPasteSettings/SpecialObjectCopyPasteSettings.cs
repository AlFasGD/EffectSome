using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectSome.Objects.CopyPasteSettings
{
    public class SpecialObjectCopyPasteSettings : GeneralCopyPasteSettings
    {
        // TODO: Create specific classes for each individual object type
        public int ItemIDValueCounter;

        public AdjustmentMode ItemIDValueAdjustmentMode;

        // The values for the special object parameters
        #region Special Object Exclusive Parameters
        public bool RandomizeStart;
        public float AnimationSpeed; // The duration of each loop of the animation in seconds
        public bool DynamicBlock;
        public List<int> BlockIDs;
        #endregion Special Object Exclusive Parameters

        public SpecialObjectCopyPasteSettings()
        {
            BlockIDs = new List<int> { 0 };
        }
    }
}
