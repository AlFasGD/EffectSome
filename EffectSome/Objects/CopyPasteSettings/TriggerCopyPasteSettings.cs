using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectSome.Objects.CopyPasteSettings
{
    public class TriggerCopyPasteSettings : GeneralCopyPasteSettings
    {
        // TODO: Create specific classes for each individual object type

        #region Counters
        public int BlockAIDValueCounter;
        public int BlockBIDValueCounter;
        #endregion

        #region Adjustment Modes
        public AdjustmentMode BlockAIDValueAdjustmentMode;
        public AdjustmentMode BlockBIDValueAdjustmentMode;
        #endregion

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

        public TriggerCopyPasteSettings()
        {
            BlockAIDs = new List<int> { 0 };
            BlockBIDs = new List<int> { 0 };
        }
    }
}
