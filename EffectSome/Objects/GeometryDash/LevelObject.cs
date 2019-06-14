using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;

namespace EffectSome
{
    public class LevelObject
    {
        public const int ParameterCount = 109;
        public object[] Parameters = new object[ParameterCount];

        #region Enumerations
        /// <summary>This enumeration provides values for the parameters of a <see cref="LevelObject"/>.</summary>
        public enum ObjectParameter
        {
            /// <summary>Represents the ID of the <see cref="LevelObject"/>.</summary>
            ID = 1,
            /// <summary>Represents the X location of the <see cref="LevelObject"/> in units.</summary>
            X = 2,
            /// <summary>Represents the Y location of the <see cref="LevelObject"/> in units.</summary>
            Y = 3,
            /// <summary>Represents a value determining whether the <see cref="LevelObject"/> is flipped horizontally.</summary>
            FlippedHorizontally = 4,
            /// <summary>Represents a value determining whether the <see cref="LevelObject"/> is flipped vertically.</summary>
            FlippedVertically = 5,
            /// <summary>Represents the rotation of the <see cref="LevelObject"/> in degrees (positive is clockwise).</summary>
            Rotation = 6,
            /// <summary>Represents the Red value of the color in the trigger.</summary>
            Red = 7,
            /// <summary>Represents the Green value of the color in the trigger.</summary>
            Green = 8,
            /// <summary>Represents the Blue value of the color in the trigger.</summary>
            Blue = 9,
            /// <summary>Represents the duration of the effect of the trigger.</summary>
            Duration = 10,
            /// <summary>Represents the Touch Triggered value of the trigger.</summary>
            TouchTriggered = 11,
            /// <summary>Represents the ID value of the Secret Coin.</summary>
            SecretCoinID = 12,
            /// <summary>Represents the checked property of the property.</summary>
            PortalChecked = 13,
            /// <summary>Unknown feature with ID 14</summary>
            UnknownFeature14 = 14,
            /// <summary>Represents the Player Color 1 property of the Color trigger.</summary>
            SetColorToPlayerColor1 = 15,
            /// <summary>Represents the Player Color 2 property of the Color trigger.</summary>
            SetColorToPlayerColor2 = 16,
            /// <summary>Represents the Blending property of the Color trigger.</summary>
            Blending = 17,
            /// <summary>Unknown feature with ID 18</summary>
            UnknownFeature18 = 18,
            /// <summary>Unknown feature with ID 19</summary>
            UnknownFeature19 = 19,
            /// <summary>Represents the Editor Layer 1 value of the <see cref="LevelObject"/>.</summary>
            EL1 = 20,
            /// <summary>Represents the Main Color Channel value of the <see cref="LevelObject"/>.</summary>
            Color1 = 21,
            /// <summary>Represents the Detail Color Channel value of the <see cref="LevelObject"/>.</summary>
            Color2 = 22,
            /// <summary>Represents the Target Color ID property of the Color trigger.</summary>
            TargetColorID = 23,
            /// <summary>Represents the Z Layer value of the <see cref="LevelObject"/>.</summary>
            ZLayer = 24,
            /// <summary>Represents the Z Order value of the <see cref="LevelObject"/>.</summary>
            ZOrder = 25,
            /// <summary>Unknown feature with ID 26</summary>
            UnknownFeature26 = 26,
            /// <summary>Unknown feature with ID 27</summary>
            UnknownFeature27 = 27,
            /// <summary>Represents the Move X value of the Move trigger in units.</summary>
            MoveX = 28,
            /// <summary>Represents the Move Y value of the Move trigger in units.</summary>
            MoveY = 29,
            /// <summary>Represents the Easing mode value of the trigger.</summary>
            Easing = 30,
            /// <summary>Represents the text of the text object encrypted in Base 64.</summary>
            TextObjectText = 31,
            /// <summary>Represents the scaling of the <see cref="LevelObject"/>.</summary>
            Scaling = 32,
            /// <summary>Unknown feature with ID 33</summary>
            UnknownFeature33 = 33,
            /// <summary>Represents the Group Parent property of the <see cref="LevelObject"/>.</summary>
            GroupParent = 34,
            /// <summary>Represents the opacity value of the trigger.</summary>
            Opacity = 35,
            /// <summary>Represents the value for whether the <see cref="LevelObject"/> is a trigger or not. This is not confirmed.</summary>
            IsTrigger = 36, // Maybe?
            /// <summary>Unknown feature with ID 37</summary>
            UnknownFeature37 = 37,
            /// <summary>Unknown feature with ID 38</summary>
            UnknownFeature38 = 38,
            /// <summary>Unknown feature with ID 39</summary>
            UnknownFeature39 = 39,
            /// <summary>Unknown feature with ID 40</summary>
            UnknownFeature40 = 40,
            /// <summary>Represents the Color 1 HSV enabled value of the trigger.</summary>
            Color1HSVEnabled = 41,
            /// <summary>Represents the Color 2 HSV enabled value of the trigger.</summary>
            Color2HSVEnabled = 42,
            /// <summary>Represents the Color 1 HSV values of the trigger.</summary>
            Color1HSVValues = 43,
            /// <summary>Represents the Color 2 HSV values of the trigger.</summary>
            Color2HSVValues = 44,
            /// <summary>Represents the Fade In value of the Pulse trigger.</summary>
            FadeIn = 45,
            /// <summary>Represents the Hold value of the Pulse trigger.</summary>
            Hold = 46,
            /// <summary>Represents the Fade Out value of the Pulse trigger.</summary>
            FadeOut = 47,
            /// <summary>Represents the Pulse Mode value of the Pulse trigger.</summary>
            PulseMode = 48,
            /// <summary>Represents the Copied Color HSV values of the trigger.</summary>
            CopiedColorHSVValues = 49,
            /// <summary>Represents the Copied Color ID value of the trigger.</summary>
            CopiedColorID = 50,
            /// <summary>Represents the Target Group ID value of the trigger.</summary>
            TargetGroupID = 51,
            /// <summary>Represents the Target Type value of the Pulse trigger.</summary>
            TargetType = 52,
            /// <summary>Unknown feature with ID 53</summary>
            UnknownFeature53 = 53,
            /// <summary>Represents the value for the distance of the yellow teleportation portal from the blue teleportation portal.</summary>
            YellowTeleportationPortalDistance = 54,
            /// <summary>Unknown feature with ID 55</summary>
            UnknownFeature55 = 55,
            /// <summary>Represents the Activate Group property of the trigger.</summary>
            ActivateGroup = 56,
            /// <summary>Represents the assigned Group IDs of the <see cref="LevelObject"/>.</summary>
            GroupIDs = 57,
            /// <summary>Represents the Lock To Player X property of the Move trigger.</summary>
            LockToPlayerX = 58,
            /// <summary>Represents the Lock To Player Y property of the Move trigger.</summary>
            LockToPlayerY = 59,
            /// <summary>Represents the Copy Opacity property of the trigger.</summary>
            CopyOpacity = 60,
            /// <summary>Represents the Editor Layer 2 value of the <see cref="LevelObject"/>.</summary>
            EL2 = 61,
            /// <summary>Represents the Spawn Triggered value of the trigger.</summary>
            SpawnTriggered = 62,
            /// <summary>Represents the Delay in the Spawn Trigger.</summary>
            SpawnDelay = 63,
            /// <summary>Represents the Don't Fade property of the <see cref="LevelObject"/>.</summary>
            DontFade = 64,
            /// <summary>Represents the Main Only property of the Pulse trigger.</summary>
            MainOnly = 65,
            /// <summary>Represents the Detail Only property of the Pulse trigger.</summary>
            DetailOnly = 66,
            /// <summary>Represents the Don't Enter property of the <see cref="LevelObject"/>.</summary>
            DontEnter = 67,
            /// <summary>Represents the Degrees value of the Rotate trigger.</summary>
            Degrees = 68,
            /// <summary>Represents the Times 360 value of the Rotate trigger.</summary>
            Times360 = 69,
            /// <summary>Represents the Lock Object Rotation property of the Rotate trigger.</summary>
            LockObjectRotation = 70,
            /// <summary>Represents the Follow Group ID value of the Follow Trigger.</summary>
            FollowGroupID = 71,
            /// <summary>Represents the Target Pos Group ID value of the Follow Trigger.</summary>
            TargetPosGroupID = 71,
            /// <summary>Represents the Center Group ID value of the Follow Trigger.</summary>
            CenterGroupID = 71,
            /// <summary>Represents the X Mod of value the Follow Trigger.</summary>
            XMod = 72,
            /// <summary>Represents the Y Mod of value the Follow Trigger.</summary>
            YMod = 73,
            /// <summary>Represents a value in the Follow trigger whose use is unknown.</summary>
            UnknownFollowTriggerFeature = 74,
            /// <summary>Represents the Strength value of the Shake trigger.</summary>
            Strength = 75,
            /// <summary>Represents the Animation ID value of the Animate trigger.</summary>
            AnimationID = 76,
            /// <summary>Represents the Count value of the Pickup trigger or Pickup Item.</summary>
            Count = 77,
            /// <summary>Represents the Subtract Count property of the Pickup trigger or Pickup Item.</summary>
            SubtractCount = 78,
            /// <summary>Represents the Pickup Mode value of the Pickup Item.</summary>
            PickupMode = 79,
            /// <summary>Represents the Target Item ID value, or the assigned Item ID value of the Pickup trigger or Pickup Item respectively.</summary>
            ItemID = 80,
            /// <summary>Represents the Block ID value of the Collision block <see cref="LevelObject"/>.</summary>
            BlockID = 80,
            /// <summary>Represents the Block A ID value of the Collision trigger.</summary>
            BlockAID = 80,
            /// <summary>Represents the Hold Mode value of the Touch trigger.</summary>
            HoldMode = 81,
            /// <summary>Represents the Toggle Mode value of the Touch trigger.</summary>
            ToggleMode = 82,
            /// <summary>Unknown feature with ID 83</summary>
            UnknownFeature83 = 83,
            /// <summary>Represents the Interval value of the Shake trigger.</summary>
            Interval = 84,
            /// <summary>Represents the Easing Rate value of the trigger.</summary>
            EasingRate = 85,
            /// <summary>Represents the Exclusive property of the Pulse trigger.</summary>
            Exclusive = 86,
            /// <summary>Represents the Multi Trigger property of the trigger.</summary>
            MultiTrigger = 87,
            /// <summary>Represents the Comarison value of the Instant Count trigger.</summary>
            Comparison = 88,
            /// <summary>Represents the Dual Mode property of the Touch trigger.</summary>
            DualMode = 89,
            /// <summary>Represents the Speed value of the Follow Player Y trigger.</summary>
            Speed = 90,
            /// <summary>Represents the Delay of the Follow Player Y Trigger.</summary>
            FollowDelay = 91,
            /// <summary>Represents the Offset value of the Follow Player Y trigger.</summary>
            YOffset = 92,
            /// <summary>Represents the Trigger On Exit property of the Collision trigger.</summary>
            TriggerOnExit = 93,
            /// <summary>Represents the Dynamic Block property of the Collision block.</summary>
            DynamicBlock = 94,
            /// <summary>Represents the Block B ID of the Collision trigger.</summary>
            BlockBID = 95,
            /// <summary>Determines whether the glow of the <see cref="LevelObject"/> is disabled or not.</summary>
            DisableGlow = 96,
            /// <summary>Represents the custom rotation speed of the rotating <see cref="LevelObject"/> in degrees per second.</summary>
            CustomRotationSpeed = 97,
            /// <summary>Determines whether the rotation of the rotating <see cref="LevelObject"/> is disabled or not.</summary>
            DisableRotation = 98,
            /// <summary>Represents the Multi Activate property of the Count trigger.</summary>
            MultiActivate = 99,
            /// <summary>Determines whether the Use Target of the Move trigger is enabled or not.</summary>
            EnableUseTarget = 100,
            /// <summary>Represents the coordinates that the <see cref="LevelObject"/> will follow the <see cref="LevelObject"/> in the Target Pos Group ID.</summary>
            TargetPosCoordinates = 101,
            /// <summary>Determines whether the Spawn trigger will be disabled while playtesting in the editor. (Currently unfunctional as of 2.01)</summary>
            EditorDisable = 102,
            /// <summary>Determines whether the <see cref="LevelObject"/> is only enabled in High Detail Mode.</summary>
            HighDetail = 103,
            /// <summary>Unknown feature with ID 104</summary>
            UnknownFeature104 = 104,
            /// <summary>Represents the coordinates that the <see cref="LevelObject"/> will follow the <see cref="LevelObject"/> in the Target Pos Group ID.</summary>
            MaxSpeed = 105,
            /// <summary>Determines whether the animated <see cref="LevelObject"/> will randomly start.</summary>
            RandomizeStart = 106,
            /// <summary>Represents the animation speed of the animated <see cref="LevelObject"/>.</summary>
            AnimationSpeed = 107,
            /// <summary>Represents the linked Group ID of the <see cref="LevelObject"/>.</summary>
            LinkedGroupID = 108,
            /// <summary>Represents whether the player switches direction of the orb <see cref="LevelObject"/>.</summary>
            OrbSwitchPlayerDirection = -1
        }
        /// <summary>This enumeration provides values for the Easing of a movement command of a trigger.</summary>
        public enum Easing
        {
            /// <summary>Represents no easing.</summary>
            None,
            /// <summary>Represents the Ease In Out easing.</summary>
            EaseInOut,
            /// <summary>Represents the Ease In easing.</summary>
            EaseIn,
            /// <summary>Represents the Ease Out easing.</summary>
            EaseOut,
            /// <summary>Represents the Elastic In Out easing.</summary>
            ElasticInOut,
            /// <summary>Represents the Elastic In easing.</summary>
            ElasticIn,
            /// <summary>Represents the Elastic Out easing.</summary>
            ElasticOut,
            /// <summary>Represents the Bounce In Out easing.</summary>
            BounceInOut,
            /// <summary>Represents the Bounce In easing.</summary>
            BounceIn,
            /// <summary>Represents the Bounce Out easing.</summary>
            BounceOut,
            /// <summary>Represents the Exponential In Out easing.</summary>
            ExponentialInOut,
            /// <summary>Represents the Exponential In easing.</summary>
            ExponentialIn,
            /// <summary>Represents the Exponential Out easing.</summary>
            ExponentialOut,
            /// <summary>Represents the Sine In Out easing.</summary>
            SineInOut,
            /// <summary>Represents the Sine In easing.</summary>
            SineIn,
            /// <summary>Represents the Sine Out easing.</summary>
            SineOut,
            /// <summary>Represents the Back In Out easing.</summary>
            BackInOut,
            /// <summary>Represents the Back In easing.</summary>
            BackIn,
            /// <summary>Represents the Back Out easing.</summary>
            BackOut
        }
        /// <summary>This enumeration provides values for the Target Type of a Pulse trigger.</summary>
        public enum PulseTargetType
        {
            /// <summary>Represents the value of the Color Channel Target Type.</summary>
            ColorChannel,
            /// <summary>Represents the value of the Group Target Type.</summary>
            Group
        }
        /// <summary>This enumeration provides values for the Pickup Mode of a Pickup item.</summary>
        public enum PickupItemPickupMode
        {
            /// <summary>Represents the value of the Pickup Item mode.</summary>
            PickupItemMode = 1,
            /// <summary>Represents the value of the Toggle Trigger mode.</summary>
            ToggleTriggerMode = 2
        }
        /// <summary>This enumeration provides values for the Toggle mode of a Touch trigger.</summary>
        public enum TouchToggleMode
        {
            /// <summary>Represents the default Toggle mode of a Touch Trigger.</summary>
            Default,
            /// <summary>Represents the Toggle On mode of a Touch Trigger.</summary>
            On,
            /// <summary>Represents the Toggle Off mode of a Touch Trigger.</summary>
            Off
        }
        /// <summary>This enumeration provides values for the comparison of an Instant Count trigger.</summary>
        public enum InstantCountComparison
        {
            /// <summary>Represents Equals on the comparison of an Instant Count trigger.</summary>
            Equals,
            /// <summary>Represents Larger on the comparison of an Instant Count trigger.</summary>
            Larger,
            /// <summary>Represents Smaller on the comparison of an Instant Count trigger.</summary>
            Smaller
        }
        /// <summary>This enumeration provides values for the coordinates to rely on the object in the Target Pos Group ID of a Move trigger.</summary>
        public enum MoveTargetPosCoordinates
        {
            /// <summary>Represents the value for both coordinates.</summary>
            Both,
            /// <summary>Represents the value for only the X coordinate.</summary>
            XOnly,
            /// <summary>Represents the value for only the Y coordinate.</summary>
            YOnly
        }
        /// <summary>This enumeration provides values for the Z Layer of a <see cref="LevelObject"/>.</summary>
        public enum ZLayer
        {
            /// <summary>Represents the value for the B4 Z Layer.</summary>
            B4 = -3,
            /// <summary>Represents the value for the B3 Z Layer.</summary>
            B3 = -1,
            /// <summary>Represents the value for the B2 Z Layer.</summary>
            B2 = 1,
            /// <summary>Represents the value for the B1 Z Layer.</summary>
            B1 = 3,
            /// <summary>Represents the value for the T1 Z Layer.</summary>
            T1 = 5,
            /// <summary>Represents the value for the T2 Z Layer.</summary>
            T2 = 7,
            /// <summary>Represents the value for the T3 Z Layer.</summary>
            T3 = 9,
            /// <summary>Represents the value for the Bot Z Layer. This has been renamed to B2 since 2.1.</summary>
            Bot = 1,
            /// <summary>Represents the value for the Mid Z Layer. This has been renamed to B1 since 2.1.</summary>
            Mid = 3,
            /// <summary>Represents the value for the Top Z Layer. This has been renamed to T1 since 2.1.</summary>
            Top = 5,
            /// <summary>Represents the value for the Top+ Z Layer. This has been renamed to T2 since 2.1.</summary>
            TopPlus = 7
        }
        /// <summary>This enumeration provides the Object ID values for the triggers.</summary>
        public enum Trigger
        {
            /// <summary>Represents the Object ID value of the BG Color trigger.</summary>
            BG = 29,
            /// <summary>Represents the Object ID value of the GRND Color trigger.</summary>
            GRND = 30,
            /// <summary>Represents the Object ID value of the Start Pos trigger.</summary>
            StartPos = 31,
            /// <summary>Represents the Object ID value of the Enable Trail trigger.</summary>
            EnableTrail = 32,
            /// <summary>Represents the Object ID value of the DisableTrail trigger.</summary>
            DisableTrail = 33,
            /// <summary>Represents the Object ID value of the Line Color trigger.</summary>
            Line = 104,
            /// <summary>Represents the Object ID value of the Obj Color trigger.</summary>
            Obj = 105,
            /// <summary>Represents the Object ID value of the Color 1 trigger.</summary>
            Color1 = 221,
            /// <summary>Represents the Object ID value of the Color 2 trigger.</summary>
            Color2 = 717,
            /// <summary>Represents the Object ID value of the Color 3 trigger.</summary>
            Color3 = 718,
            /// <summary>Represents the Object ID value of the Color 4 trigger.</summary>
            Color4 = 743,
            /// <summary>Represents the Object ID value of the 3DL Color trigger.</summary>
            ThreeDL = 744,
            /// <summary>Represents the Object ID value of the Color trigger.</summary>
            Color = 899,
            /// <summary>Represents the Object ID value of the GRND Color trigger.</summary>
            GRND2 = 900,
            /// <summary>Represents the Object ID value of the Move trigger.</summary>
            Move = 901,
            /// <summary>Represents the Object ID value of the Pulse trigger.</summary>
            Pulse = 1006,
            /// <summary>Represents the Object ID value of the Alpha trigger.</summary>
            Alpha = 1007,
            /// <summary>Represents the Object ID value of the Toggle trigger.</summary>
            Toggle = 1049,
            /// <summary>Represents the Object ID value of the Spawn trigger.</summary>
            Spawn = 1268,
            /// <summary>Represents the Object ID value of the Rotate trigger.</summary>
            Rotate = 1346,
            /// <summary>Represents the Object ID value of the Follow trigger.</summary>
            Follow = 1347,
            /// <summary>Represents the Object ID value of the Shake trigger.</summary>
            Shake = 1520,
            /// <summary>Represents the Object ID value of the Animate trigger.</summary>
            Animate = 1585,
            /// <summary>Represents the Object ID value of the Touch trigger.</summary>
            Touch = 1595,
            /// <summary>Represents the Object ID value of the Count trigger.</summary>
            Count = 1611,
            /// <summary>Represents the Object ID value of the Hide Player trigger.</summary>
            HidePlayer = 1612,
            /// <summary>Represents the Object ID value of the Show Player trigger.</summary>
            ShowPlayer = 1613,
            /// <summary>Represents the Object ID value of the Stop trigger.</summary>
            Stop = 1616,
            /// <summary>Represents the Object ID value of the Instant Count trigger.</summary>
            InstantCount = 1811,
            /// <summary>Represents the Object ID value of the On Death trigger.</summary>
            OnDeath = 1812,
            /// <summary>Represents the Object ID value of the Follow Player Y trigger.</summary>
            FollowPlayerY = 1814,
            /// <summary>Represents the Object ID value of the Collision trigger.</summary>
            Collision = 1815,
            /// <summary>Represents the Object ID value of the Pickup trigger.</summary>
            Pickup = 1817,
            /// <summary>Represents the Object ID value of the BG Effect On trigger.</summary>
            BGEffectOn = 1818,
            /// <summary>Represents the Object ID value of the BG Effect Off trigger.</summary>
            BGEffectOff = 1819,
            /// <summary>Represents the Object ID value of the Scale trigger. (Still does not exist, this is reserved for future use)</summary>
            Scale = -1,
            /// <summary>Represents the Object ID value of the Static Camera trigger. (Still does not exist, this is reserved for future use)</summary>
            StaticCamera = -1,
            /// <summary>Represents the Object ID value of the Zoom trigger. (Still does not exist, this is reserved for future use)</summary>
            Zoom = -1,
            /// <summary>Represents the Object ID value of the Camera Offset trigger. (Still does not exist, this is reserved for future use)</summary>
            CameraOffset = -1,
            /// <summary>Represents the Object ID value of the Random trigger. (Still does not exist, this is reserved for future use)</summary>
            Random = -1,
            /// <summary>Represents the Object ID value of the End trigger. (Still does not exist, this is reserved for future use)</summary>
            End = -1,
            // More to come soon:tm:
        }
        #endregion
        #region Constructors
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the Object ID parameter set to 1.</summary>
        public LevelObject() { this[ObjectParameter.ID] = 1; }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID) { this[ObjectParameter.ID] = objID; }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID and location.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID, double x, double y)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location and flipped values.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="flippedHorizontally">The flipped horizontally value of the <see cref="LevelObject"/>.</param>
        /// <param name="flippedVertically">The flipped vertically value of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID, double x, double y, bool flippedHorizontally, bool flippedVertically)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.FlippedHorizontally] = flippedHorizontally;
            this[ObjectParameter.FlippedVertically] = flippedVertically;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location and rotation.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="rotation">The rotation of the <see cref="LevelObject"/> in degrees. Positive is clockwise.</param>
        public LevelObject(int objID, double x, double y, double rotation)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.Rotation] = rotation;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location, rotation and scaling.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="rotation">The rotation of the <see cref="LevelObject"/> in degrees. Positive is clockwise.</param>
        /// <param name="sacling">The scaling ratio of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID, double x, double y, double rotation, double scaling)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.Rotation] = rotation;
            this[ObjectParameter.Scaling] = scaling;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location, rotation and flipped values.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="flippedHorizontally">The flipped horizontally value of the <see cref="LevelObject"/>.</param>
        /// <param name="flippedVertically">The flipped vertically value of the <see cref="LevelObject"/>.</param>
        /// <param name="rotation">The rotation of the <see cref="LevelObject"/> in degrees. Positive is clockwise.</param>
        public LevelObject(int objID, double x, double y, bool flippedHorizontally, bool flippedVertically, double rotation)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.FlippedHorizontally] = flippedHorizontally;
            this[ObjectParameter.FlippedVertically] = flippedVertically;
            this[ObjectParameter.Rotation] = rotation;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location, rotation, scaling and flipped values.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="flippedHorizontally">The flipped horizontally value of the <see cref="LevelObject"/>.</param>
        /// <param name="flippedVertically">The flipped vertically value of the <see cref="LevelObject"/>.</param>
        /// <param name="rotation">The rotation of the <see cref="LevelObject"/> in degrees. Positive is clockwise.</param>
        /// <param name="scaling">The scaling ratio of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID, double x, double y, bool flippedHorizontally, bool flippedVertically, double rotation, double scaling)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.FlippedHorizontally] = flippedHorizontally;
            this[ObjectParameter.FlippedVertically] = flippedVertically;
            this[ObjectParameter.Rotation] = rotation;
            this[ObjectParameter.Scaling] = scaling;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location and Editor Layer 1.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="EL1">The Editor Layer 1 of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID, double x, double y, int EL1)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.EL1] = EL1;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location and Editor Layers.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="EL1">The Editor Layer 1 of the <see cref="LevelObject"/>.</param>
        /// <param name="EL2">The Editor Layer 2 of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID, double x, double y, int EL1, int EL2)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.EL1] = EL1;
            this[ObjectParameter.EL2] = EL2;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location and Group IDs.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="groupIDs">The Group IDs of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID, double x, double y, int[] groupIDs)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.GroupIDs] = groupIDs;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location, Group IDs and Editor Layers.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="groupIDs">The Group IDs of the <see cref="LevelObject"/>.</param>
        /// <param name="EL1">The Editor Layer 1 of the <see cref="LevelObject"/>.</param>
        /// <param name="EL2">The Editor Layer 2 of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID, double x, double y, int[] groupIDs, int EL1, int EL2)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.GroupIDs] = groupIDs;
            this[ObjectParameter.EL1] = EL1;
            this[ObjectParameter.EL2] = EL2;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location, Color IDs, Group IDs and Editor Layers.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="mainColor">The Main Color ID of the <see cref="LevelObject"/>.</param>
        /// <param name="detailColor">The Detail Color ID of the <see cref="LevelObject"/>.</param>
        /// <param name="groupIDs">The Group IDs of the <see cref="LevelObject"/>.</param>
        /// <param name="EL1">The Editor Layer 1 of the <see cref="LevelObject"/>.</param>
        /// <param name="EL2">The Editor Layer 2 of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID, double x, double y, int mainColor, int detailColor, int[] groupIDs, int EL1, int EL2)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.Color1] = mainColor;
            this[ObjectParameter.Color2] = detailColor;
            this[ObjectParameter.GroupIDs] = groupIDs;
            this[ObjectParameter.EL1] = EL1;
            this[ObjectParameter.EL2] = EL2;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location, Color IDs, Group IDs, Editor Layers and glow attibute.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="mainColor">The Main Color ID of the <see cref="LevelObject"/>.</param>
        /// <param name="detailColor">The Detail Color ID of the <see cref="LevelObject"/>.</param>
        /// <param name="groupIDs">The Group IDs of the <see cref="LevelObject"/>.</param>
        /// <param name="EL1">The Editor Layer 1 of the <see cref="LevelObject"/>.</param>
        /// <param name="EL2">The Editor Layer 2 of the <see cref="LevelObject"/>.</param>
        /// <param name="disableGlow">The Disable Glow value of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID, double x, double y, int mainColor, int detailColor, int[] groupIDs, int EL1, int EL2, bool disableGlow)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.Color1] = mainColor;
            this[ObjectParameter.Color2] = detailColor;
            this[ObjectParameter.GroupIDs] = groupIDs;
            this[ObjectParameter.EL1] = EL1;
            this[ObjectParameter.EL2] = EL2;
            this[ObjectParameter.DisableGlow] = disableGlow;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location, Color IDs, Group IDs, Editor Layers and glow attibute, Z Order and Z Layer.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="mainColor">The Main Color ID of the <see cref="LevelObject"/>.</param>
        /// <param name="detailColor">The Detail Color ID of the <see cref="LevelObject"/>.</param>
        /// <param name="groupIDs">The Group IDs of the <see cref="LevelObject"/>.</param>
        /// <param name="EL1">The Editor Layer 1 of the <see cref="LevelObject"/>.</param>
        /// <param name="EL2">The Editor Layer 2 of the <see cref="LevelObject"/>.</param>
        /// <param name="disableGlow">The Disable Glow value of the <see cref="LevelObject"/>.</param>
        /// <param name="ZOrder">The Z Order value of the <see cref="LevelObject"/>.</param>
        /// <param name="ZLayer">The Z Layer value of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID, double x, double y, int mainColor, int detailColor, int[] groupIDs, int EL1, int EL2, bool disableGlow, int ZOrder, int ZLayer)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.Color1] = mainColor;
            this[ObjectParameter.Color2] = detailColor;
            this[ObjectParameter.GroupIDs] = groupIDs;
            this[ObjectParameter.EL1] = EL1;
            this[ObjectParameter.EL2] = EL2;
            this[ObjectParameter.DisableGlow] = disableGlow;
            this[ObjectParameter.ZOrder] = ZOrder;
            this[ObjectParameter.ZLayer] = ZLayer;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class with the specified Object ID, location, Color IDs, Group IDs, Editor Layers and glow attibute, Z Order and Z Layer.</summary>
        /// <param name="objID">The Object ID of the <see cref="LevelObject"/>.</param>
        /// <param name="x">The X position of the <see cref="LevelObject"/>.</param>
        /// <param name="y">The Y position of the <see cref="LevelObject"/>.</param>
        /// <param name="mainColor">The Main Color ID of the <see cref="LevelObject"/>.</param>
        /// <param name="detailColor">The Detail Color ID of the <see cref="LevelObject"/>.</param>
        /// <param name="groupIDs">The Group IDs of the <see cref="LevelObject"/>.</param>
        /// <param name="EL1">The Editor Layer 1 of the <see cref="LevelObject"/>.</param>
        /// <param name="EL2">The Editor Layer 2 of the <see cref="LevelObject"/>.</param>
        /// <param name="disableGlow">The Disable Glow value of the <see cref="LevelObject"/>.</param>
        /// <param name="ZOrder">The Z Order value of the <see cref="LevelObject"/>.</param>
        /// <param name="ZLayer">The Z Layer value of the <see cref="LevelObject"/>.</param>
        public LevelObject(int objID, double x, double y, int mainColor, int detailColor, int[] groupIDs, int EL1, int EL2, bool disableGlow, int ZOrder, ZLayer ZLayer)
        {
            this[ObjectParameter.ID] = objID;
            this[ObjectParameter.IsTrigger] = Gamesave.triggerList.Contains(objID);
            this[ObjectParameter.X] = x;
            this[ObjectParameter.Y] = y;
            this[ObjectParameter.Color1] = mainColor;
            this[ObjectParameter.Color2] = detailColor;
            this[ObjectParameter.GroupIDs] = groupIDs;
            this[ObjectParameter.EL1] = EL1;
            this[ObjectParameter.EL2] = EL2;
            this[ObjectParameter.DisableGlow] = disableGlow;
            this[ObjectParameter.ZOrder] = ZOrder;
            this[ObjectParameter.ZLayer] = (int)ZLayer;
        }
        /// <summary>Creates an instance of the <see cref="LevelObject"/> class from the specified object string. Most parameters will be stored with their <see cref="string"/> representation, so it is recommended to convert them to their appropriate form before usage.</summary>
        /// <param name="objectString">The object string of the <see cref="LevelObject"/>.</param>
        public LevelObject(string objectString)
        {
            string[] parameters = objectString.Split(',').RemoveEmptyElements();
            for (int i = 0; i < parameters.Length; i += 2)
            {
                int param = ToInt32(parameters[i]);
                if (param == (int)ObjectParameter.GroupIDs)
                    this.Parameters[param] = parameters[i + 1].Split('.').ToInt32Array();
                if (param == (int)ObjectParameter.Color1HSVValues || param == (int)ObjectParameter.Color2HSVValues || param == (int)ObjectParameter.CopiedColorHSVValues)
                {
                    string[] HSVParams = parameters[i + 1].Split('a');
                    this.Parameters[param] = new object[] { ToInt32(HSVParams[0]), ToDouble(HSVParams[1]), ToDouble(HSVParams[2]), HSVParams[3] == "1", HSVParams[4] == "1" };
                }
                else
                    this.Parameters[param] = parameters[i + 1]; // Parse the parameters as their string representations, the parameters should be converted to their appropriate format before usage to prevent errors.
            }
        }
        // Add more constructors
        #endregion

        /// <summary>Get a specific parameter of an object from its name.</summary>
        /// <param name="p">The parameter to get.</param>
        /// <returns>An object with the parameter that was requested.</returns>
        public object this[ObjectParameter p]
        {
            get => Parameters[(int)p];
            set { Parameters[(int)p] = value; }
        }

        /// <summary>Converts the <see cref="LevelObject"/> to its string representation in the gamesave.</summary>
        public override string ToString()
        {
            string objectString = "";
            for (int i = 1; i < ParameterCount; i++)
                if (Parameters[i] != null)
                {
                    string parameter = "";
                    if (i > 3)
                    {
                        if (Parameters[i] is int)
                        {
                            if ((int)Parameters[i] != 0)
                                parameter += i + "," + Parameters[i] + ",";
                        }
                        if (Parameters[i] is double)
                        {
                            if ((double)Parameters[i] != 0)
                                parameter += i + "," + Parameters[i] + ",";
                        }
                        else if (Parameters[i] is string)
                            parameter += i + $",{(i == (int)ObjectParameter.TextObjectText ? ToBase64String(Encoding.UTF8.GetBytes((string)Parameters[i])) : Parameters[i])},";
                        else if (Parameters[i] is bool)
                            parameter += i + "," + ToInt32(Parameters[i]) + ",";
                        else if (i == (int)ObjectParameter.GroupIDs)
                        {
                            if ((Parameters[i] as int[]).Length > 0)
                            {
                                parameter += i + ",";
                                for (int j = 0; j < (Parameters[i] as int[]).Length; j++)
                                    parameter += (Parameters[i] as int[])[j] + ".";
                                parameter = parameter.Remove(parameter.Length - 1);
                                parameter += ",";
                            }
                        }
                        else if (i == (int)ObjectParameter.Color1HSVValues || i == (int)ObjectParameter.Color2HSVValues || i == (int)ObjectParameter.CopiedColorHSVValues)
                        {
                            parameter += i + ",";
                            for (int j = 0; j < (Parameters[i] as Array).Length; j++)
                                parameter += ToDouble((Parameters[i] as object[])[j]) + "a";
                            parameter = parameter.Remove(parameter.Length - 1);
                            parameter += ",";
                        }
                    }
                    else
                        parameter += i + "," + Parameters[i] + ",";
                    objectString += parameter;
                }
            if (objectString.Length > 0)
                objectString = objectString.Remove(objectString.Length - 1);
            return objectString;
        }

        public static string GetObjectString(List<LevelObject> l)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < l.Count; i++)
                s.Append(l[i].ToString() + ";");
            return s.ToString();
        }
    }
}