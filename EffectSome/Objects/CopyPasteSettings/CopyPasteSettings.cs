using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectSome.Objects.CopyPasteSettings
{
    public class CopyPasteSettings
    {
        #region Copy-Paste Automation Settings
        // General parameters that apply to every object type
        // When the array only contains -1 then the settings are to be applied for all objects except triggers and special objects
        // When the value of the Group/Item/Block ID adjustment is -1,
        // the Color/Group/Item/Block IDs of the new objects are to be set to the next free

        // TODO: Migrate non-general settings to other specific classes
        // This is important to make the project more organized and not bamboozle it with millions of fucking parameters in one single class

        // The counters for the arrays
        #region Counters
        public int Color1IDValueCounter;
        public int Color2IDValueCounter;
        public int BlockAIDValueCounter;
        public int BlockBIDValueCounter;
        public int ItemIDValueCounter;
        public int[] GroupIDValueCounters = new int[10];
        #endregion Counters
        
        // The adjustment modes of the values
        #region Adjustment Modes
        public AdjustmentMode Color1IDValueAdjustmentMode;
        public AdjustmentMode Color2IDValueAdjustmentMode;
        public AdjustmentMode BlockAIDValueAdjustmentMode;
        public AdjustmentMode BlockBIDValueAdjustmentMode;
        public AdjustmentMode ItemIDValueAdjustmentMode;
        public AdjustmentMode[] GroupIDValueAdjustmentModes = new AdjustmentMode[10];
        #endregion Adjustment Modes

        // The values for the general object parameters
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
        #endregion General Objects Parameters

        // Incomplete
        #region Trigger-Specific Parameters
        public bool TouchTriggered;
        public bool SpawnTriggered;
        public bool ActivateGroup;
        public bool AdjustActivateGroup;
        public bool MultiTrigger;
        public bool RandomizeDuration;
        public float Duration; // The duration of the effect
        public float EasingRate; // The easing rate of the movement/rotation
        public int ItemID; // The Item ID of the trigger
        public int PrimaryTargetID; // The primary Target ID of the trigger
        public int EasingType; // The easing type of the movement/rotation

        #region Move Trigger Exclusive Parameters
        public bool RandomizeMoveX, RandomizeMoveY;
        public float MoveX, MoveY;
        public int TargetPosGroupID, TargetPosCoordinates; // The coordinates of the Target Pos
        #endregion Move Trigger Exclusive Parameters

        #region Pulse Trigger Exclusive Parameters
        public bool Main, Detail, AdjustMainDetail, RandomizeR, RandomizeG, RandomizeB, RandomizeH, RandomizeS, RandomizeV;
        public float FadeIn, Hold, FadeOut;
        public int R, G, B;
        public float H, S, V;
        public int PulseMode, TargetType;
        #endregion Pulse Trigger Exclusive Parameters

        #region Collision Trigger Exclusive Parameters
        public bool TriggerOnExit, AdjustTriggerOnExit;
        public List<int> BlockAIDs;
        public List<int> BlockBIDs;
        #endregion Collision Trigger Exclusive Parameters

        #region Alpha Trigger Exclusive Parameters
        public float Opacity;
        #endregion Alpha Trigger Exclusive Parameters

        #region Spawn Trigger Exclusive Parameters
        public float SpawnDelay;
        #endregion Spawn Trigger Exclusive Parameters

        #region Follow Trigger Exclusive Parameters
        public float XMod, YMod;
        public int FollowGroupID;
        #endregion Follow Trigger Exclusive Parameters

        #region Follow Player Y Trigger Exclusive Parameters
        public float FollowSpeed, FollowDelay, Offset, MaxSpeed;
        #endregion Follow Player Y Trigger Exclusive Parameters

        #region Touch Trigger Exclusive Parameters
        public float HoldMode, DualMode;
        public int ToggleStatus;
        #endregion Touch Trigger Exclusive Parameters

        #region Count Trigger Exclusive Parameters
        public int TargetCount;
        #endregion Count Trigger Exclusive Parameters
        #endregion Trigger-Specific Parameters

        // The values for the special object parameters
        #region Special Object Exclusive Parameters
        public bool RandomizeStart;
        public float AnimationSpeed; // The duration of each loop of the animation in seconds
        public bool DynamicBlock;
        public List<int> BlockIDs;
        #endregion Special Object Exclusive Parameters
        #endregion Copy-Paste Automation Settings

        public CopyPasteSettings()
        {
            ObjectIDs = new List<int> { -1 };
            Color1IDs = new List<int> { 0 };
            Color2IDs = new List<int> { 0 };
            GroupIDs = new List<int> { 0 };
            BlockIDs = new List<int> { 0 };
            BlockAIDs = new List<int> { 0 };
            BlockBIDs = new List<int> { 0 };
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