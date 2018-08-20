using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EffectSome.Objects.CopyPasteSettings;
using static EffectSome.Objects.CopyPasteSettings.GeneralCopyPasteSettings;
using static EffectSome.Utilities.Functions.GeometryDash.CopyPaste.CopyPasteSettingsWritingFunctions;
using static EffectSome.Editor;

namespace EffectSome
{
    public static class CopyPasteSettingsWriting
    {
        /// <summary>Injects the code for the copy-paste automation of the general parameters of all objects or just the selected ones.</summary>
        public static void WriteGeneralObjectCopyPasteAutomation
        (
            List<int> objectIDs, float hue1, float saturation1, float brightness1, float hue2, float saturation2, float brightness2, float rotation, float scaling,
            bool[] groupIDs, List<int> groupIDValues, List<int> color1IDValues, List<int> color2IDValues, float x, float y, int zOrder, int zLayer, int el1, int el2,
            AdjustmentMode color1IDAdjustmentMode, AdjustmentMode color2IDAdjustmentMode, AdjustmentMode groupIDAdjustmentMode
        )
        {
            ApplyNewSettings(objectIDs, x, y, hue1, saturation1, brightness1, hue2, saturation2, brightness2, scaling, rotation, zOrder, zLayer, el1, el2, color1IDValues, color2IDValues, groupIDValues, groupIDs,
                             color1IDAdjustmentMode, color2IDAdjustmentMode, groupIDAdjustmentMode);
            WriteAllObjectCopyPasteAutomationSettings();
        }
        #region Special Objects
        #region Orbs
        /// <summary>Injects the code for the copy-paste automation of orbs. This overload is for the adjustment of the selected Group IDs by a specified number.</summary>
        public static void InjectOrbsCopyPasteAutomation1(int groupIDsAdj, bool[] groupIDs, bool multiActivate, bool adjMultiActivate, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of orbs. This overload is for setting the Group IDs to a different number each time from an array.</summary>
        public static void InjectOrbsCopyPasteAutomation2(List<int> groupIDValues, bool[] groupIDs, bool multiActivate, bool adjMultiActivate, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of orbs. This overload is for setting the Group IDs to unused Group IDs.</summary>
        public static void InjectOrbsCopyPasteAutomation3(bool[] groupIDs, bool multiActivate, bool adjMultiActivate, float x, float y) { }
        #endregion
        #region Manipulation Portals
        /// <summary>Injects the code for the copy-paste automation of manipulation portals. This overload is for the adjustment of the selected Group IDs by a specified number.</summary>
        public static void InjectManipulationPortalsCopyPasteAutomation1(int groupIDsAdj, bool[] groupIDs, bool showBorders, bool adjShowBorders, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of manipulation portals. This overload is for setting the Group IDs to a different number each time from an array.</summary>
        public static void InjectManipulationPortalsCopyPasteAutomation2(List<int> groupIDValues, bool[] groupIDs, bool showBorders, bool adjShowBorders, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of manipulation portals. This overload is for setting the Group IDs to unused Group IDs.</summary>
        public static void InjectManipulationPortalsCopyPasteAutomation3(bool[] groupIDs, bool showBorders, bool adjShowBorders, float x, float y) { }
        #endregion
        #region Speed Portals
        /// <summary>Injects the code for the copy-paste automation of speed portals. This overload is for the adjustment of the selected Group IDs by a specified number.</summary>
        public static void InjectSpeedPortalsCopyPasteAutomation1(int groupIDsAdj, bool[] groupIDs, bool adjustGuidelines, bool adjAdjustGuidelines, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of speed portals. This overload is for setting the Group IDs to a different number each time from an array.</summary>
        public static void InjectSpeedPortalsCopyPasteAutomation2(List<int> groupIDValues, bool[] groupIDs, bool adjustGuidelines, bool adjAdjustGuidelines, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of speed portals. This overload is for setting the Group IDs to unused Group IDs.</summary>
        public static void InjectSpeedPortalsCopyPasteAutomation3(bool[] groupIDs, bool adjustGuidelines, bool adjAdjustGuidelines, float x, float y) { }
        #endregion
        #region Pulsating Animation Objects
        /// <summary>Injects the code for the copy-paste automation of pulsating animation objects. This overload is for the adjustment of the selected Group IDs by a specified number.</summary>
        public static void InjectPulsatingAnimationObjectsCopyPasteAutomation1(int groupIDsAdj, bool[] groupIDs, bool randomizeStart, bool adjRandomizeStart, float speed, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of pulsating animation objects. This overload is for setting the Group IDs to a different number each time from an array.</summary>
        public static void InjectPulsatingAnimationObjectsCopyPasteAutomation2(List<int> groupIDValues, bool[] groupIDs, bool randomizeStart, bool adjRandomizeStart, float speed, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of pulsating animation objects. This overload is for setting the Group IDs to unused Group IDs.</summary>
        public static void InjectPulsatingAnimationObjectsCopyPasteAutomation3(bool[] groupIDs, bool randomizeStart, bool adjRandomizeStart, float speed, float x, float y) { }
        #endregion
        #region Trigger Orbs
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for the adjustment of the selected Group IDs by a specified number and setting the Target Group ID to an unused Group ID.</summary>
        public static void InjectTriggerOrbsCopyPasteAutomation1(int groupIDsAdj, bool[] groupIDs, bool activateGroup, bool adjActivateGroup, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for setting the Group IDs to a different number each time from an array and setting the Target Group ID to an unused Group ID.</summary>
        public static void InjectTriggerOrbsCopyPasteAutomation2(List<int> groupIDValues, bool[] groupIDs, bool activateGroup, bool adjActivateGroup, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for setting the Group IDs to unused Group IDs and setting the Target Group ID to an unused Group ID.</summary>
        public static void InjectTriggerOrbsCopyPasteAutomation3(bool[] groupIDs, bool activateGroup, bool adjActivateGroup, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for the adjustment of the selected Group IDs by a specified number and adjusting the Target Group ID by a specified number.</summary>
        public static void InjectTriggerOrbsCopyPasteAutomation4(int groupIDsAdj, bool[] groupIDs, int targetGroupIDAdj, bool activateGroup, bool adjActivateGroup, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for setting the Group IDs to a different number each time from an array and adjusting the Target Group ID by a specified number.</summary>
        public static void InjectTriggerOrbsCopyPasteAutomation5(List<int> groupIDValues, bool[] groupIDs, int targetGroupIDAdj, bool activateGroup, bool adjActivateGroup, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for setting the Group IDs to unused Group IDs and adjusting the Target Group ID by a specified number.</summary>
        public static void InjectTriggerOrbsCopyPasteAutomation6(bool[] groupIDs, int targetGroupIDAdj, bool activateGroup, bool adjActivateGroup, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for the adjustment of the selected Group IDs by a specified number and setting the Target Group IDs to a different number each time from an array.</summary>
        public static void InjectTriggerOrbsCopyPasteAutomation7(int groupIDsAdj, bool[] groupIDs, List<int> targetGroupIDs, bool activateGroup, bool adjActivateGroup, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for setting the Group IDs to a different number each time from an array and setting the Target Group IDs to a different number each time from an array.</summary>
        public static void InjectTriggerOrbsCopyPasteAutomation8(List<int> groupIDValues, bool[] groupIDs, List<int> targetGroupIDs, bool activateGroup, bool adjActivateGroup, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for setting the Group IDs to unused Group IDs and setting the Target Group IDs to a different number each time from an array.</summary>
        public static void InjectTriggerOrbsCopyPasteAutomation9(bool[] groupIDs, List<int> targetGroupIDs, bool activateGroup, bool adjActivateGroup, float x, float y) { }
        #endregion
        #region Collision Blocks
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for the adjustment of the selected Group IDs by a specified number and setting the Block ID to unused Group IDs.</summary>
        public static void InjectCollisionBlocksCopyPasteAutomation1(int groupIDsAdj, bool[] groupIDs, bool dynamicBlock, bool adjDynamicBlock, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for setting the Group IDs to a different number each time from an array and setting the Block ID to unused Group IDs.</summary>
        public static void InjectCollisionBlocksCopyPasteAutomation2(List<int> groupIDValues, bool[] groupIDs, bool dynamicBlock, bool adjDynamicBlock, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for setting the Group IDs to unused Group IDs and setting the Block ID to unused Group IDs.</summary>
        public static void InjectCollisionBlocksCopyPasteAutomation3(bool[] groupIDs, bool dynamicBlock, bool adjDynamicBlock, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for the adjustment of the selected Group IDs by a specified number and adjusting the Block ID by a specified number.</summary>
        public static void InjectCollisionBlocksCopyPasteAutomation4(int groupIDsAdj, bool[] groupIDs, int blockIDAdj, bool dynamicBlock, bool adjDynamicBlock, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for setting the Group IDs to a different number each time from an array and adjusting the Block ID by a specified number.</summary>
        public static void InjectCollisionBlocksCopyPasteAutomation5(List<int> groupIDValues, bool[] groupIDs, int blockIDAdj, bool dynamicBlock, bool adjDynamicBlock, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for setting the Group IDs to unused Group IDs and adjusting the Block ID by a specified number.</summary>
        public static void InjectCollisionBlocksCopyPasteAutomation6(bool[] groupIDs, int blockIDAdj, bool dynamicBlock, bool adjDynamicBlock, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for the adjustment of the selected Group IDs by a specified number and setting the Block IDs to a different number each time from an array.</summary>
        public static void InjectCollisionBlocksCopyPasteAutomation7(int groupIDsAdj, bool[] groupIDs, List<int> blockIDs, bool dynamicBlock, bool adjDynamicBlock, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for setting the Group IDs to a different number each time from an array and setting the Block IDs to a different number each time from an array.</summary>
        public static void InjectCollisionBlocksCopyPasteAutomation8(List<int> groupIDValues, bool[] groupIDs, List<int> blockIDs, bool dynamicBlock, bool adjDynamicBlock, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of trigger orbs. This overload is for setting the Group IDs to unused Group IDs and setting the Block IDs to a different number each time from an array.</summary>
        public static void InjectCollisionBlocksCopyPasteAutomation9(bool[] groupIDs, List<int> blockIDs, bool dynamicBlock, bool adjDynamicBlock, float x, float y) { }
        #endregion
        #region Count Objects
        /// <summary>Injects the code for the copy-paste automation of count objects. This overload is for the adjustment of the selected Group IDs by a specified number and setting the Item ID to an unused Item ID.</summary>
        public static void InjectCountObjectsCopyPasteAutomation1(int groupIDsAdj, bool[] groupIDs, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of count objects. This overload is for setting the Group IDs to a different number each time from an array and setting the Item ID to an unused Group ID.</summary>
        public static void InjectCountObjectsCopyPasteAutomation2(List<int> groupIDValues, bool[] groupIDs, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of count objects. This overload is for setting the Group IDs to unused Group IDs and setting the Item ID to an unused Group ID.</summary>
        public static void InjectCountObjectsCopyPasteAutomation3(bool[] groupIDs, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of count objects. This overload is for the adjustment of the selected Group IDs by a specified number and adjusting the Item ID by a specified number.</summary>
        public static void InjectCountObjectsCopyPasteAutomation4(int groupIDsAdj, bool[] groupIDs, int itemIDAdj, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of count objects. This overload is for setting the Group IDs to a different number each time from an array and adjusting the Item ID by a specified number.</summary>
        public static void InjectCountObjectsCopyPasteAutomation5(List<int> groupIDValues, bool[] groupIDs, int itemIDAdj, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of count objects. This overload is for setting the Group IDs to unused Group IDs and adjusting the Item ID by a specified number.</summary>
        public static void InjectCountObjectsCopyPasteAutomation6(bool[] groupIDs, int itemIDAdj, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of count objects. This overload is for the adjustment of the selected Group IDs by a specified number and setting the Item IDs to a different number each time from an array.</summary>
        public static void InjectCountObjectsCopyPasteAutomation7(int groupIDsAdj, bool[] groupIDs, List<int> itemIDs, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of count objects. This overload is for setting the Group IDs to a different number each time from an array and setting the Item IDs to a different number each time from an array.</summary>
        public static void InjectCountObjectsCopyPasteAutomation8(List<int> groupIDValues, bool[] groupIDs, List<int> itemIDs, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of count objects. This overload is for setting the Group IDs to unused Group IDs and setting the Item IDs to a different number each time from an array.</summary>
        public static void InjectCountObjectsCopyPasteAutomation9(bool[] groupIDs, List<int> itemIDs, float x, float y) { }
        #endregion
        #region Pickup Items
        #region Set Target Item IDs to unused Item IDs
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to unused Group IDs, setting the Target Group ID to unused Group IDs and setting the Target Item IDs to unused Item IDs.</summary>
        public static void InjectPickupItemsCopyPasteAutomation1(bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for the adjustment of the selected Group IDs by a specified number, setting the Target Group ID to unused Group IDs and setting the Target Item IDs to unused Item IDs.</summary>
        public static void InjectPickupItemsCopyPasteAutomation2(int groupIDsAdj, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to a different number each time from an array, setting the Target Group ID to unused Group IDs and setting the Target Item IDs to unused Item IDs.</summary>
        public static void InjectPickupItemsCopyPasteAutomation3(List<int> groupIDValues, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to unused Group IDs, adjusting the Target Group ID by a specified number and setting the Target Item IDs to unused Item IDs.</summary>
        public static void InjectPickupItemsCopyPasteAutomation4(bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, int targetGroupIDAdj, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for the adjustment of the selected Group IDs by a specified number, adjusting the Target Group ID by a specified number and setting the Target Item IDs to unused Item IDs.</summary>
        public static void InjectPickupItemsCopyPasteAutomation5(int groupIDsAdj, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, int targetGroupIDAdj, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to a different number each time from an array, adjusting the Target Group ID by a specified number and setting the Target Item IDs to unused Item IDs.</summary>
        public static void InjectPickupItemsCopyPasteAutomation6(List<int> groupIDValues, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, int targetGroupIDAdj, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to unused Group IDs, setting the Target Group IDs to a different number each time from an array and setting the Target Item IDs to unused Item IDs.</summary>
        public static void InjectPickupItemsCopyPasteAutomation7(bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, List<int> targetGroupIDs, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for the adjustment of the selected Group IDs by a specified number, setting the Target Group IDs to a different number each time from an array and setting the Target Item IDs to unused Item IDs.</summary>
        public static void InjectPickupItemsCopyPasteAutomation8(int groupIDsAdj, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, List<int> targetGroupIDs, float x, float y) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to a different number each time from an array, setting the Target Group IDs to a different number each time from an array and setting the Target Item IDs to unused Item IDs.</summary>
        public static void InjectPickupItemsCopyPasteAutomation9(List<int> groupIDValues, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, List<int> targetGroupIDs, float x, float y) { }
        #endregion
        #region Adjust Target Item IDs by a specified number
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to unused Group IDs, setting the Target Group ID to unused Group IDs and adjusting the Target Item ID by a specified number.</summary>
        public static void InjectPickupItemsCopyPasteAutomation10(bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, float x, float y, int targetItemIDAdj) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for the adjustment of the selected Group IDs by a specified number, setting the Target Group ID to unused Group IDs and setting the Target Item IDs to unused Item IDs and adjusting the Target Item ID by a specified number.</summary>
        public static void InjectPickupItemsCopyPasteAutomation11(int groupIDsAdj, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, float x, float y, int targetItemIDAdj) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to a different number each time from an array, setting the Target Group ID to unused Group IDs and adjusting the Target Item ID by a specified number.</summary>
        public static void InjectPickupItemsCopyPasteAutomation12(List<int> groupIDValues, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, float x, float y, int targetItemIDAdj) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to unused Group IDs, adjusting the Target Group ID by a specified number and adjusting the Target Item ID by a specified number.</summary>
        public static void InjectPickupItemsCopyPasteAutomation13(bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, int targetGroupIDAdj, float x, float y, int targetItemIDAdj) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for the adjustment of the selected Group IDs by a specified number, adjusting the Target Group ID by a specified numbe rand adjusting the Target Item ID by a specified number.</summary>
        public static void InjectPickupItemsCopyPasteAutomation14(int groupIDsAdj, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, int targetGroupIDAdj, float x, float y, int targetItemIDAdj) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to a different number each time from an array, adjusting the Target Group ID by a specified number and adjusting the Target Item ID by a specified number.</summary>
        public static void InjectPickupItemsCopyPasteAutomation15(List<int> groupIDValues, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, int targetGroupIDAdj, float x, float y, int targetItemIDAdj) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to unused Group IDs, setting the Target Group IDs to a different number each time from an array and adjusting the Target Item ID by a specified number.</summary>
        public static void InjectPickupItemsCopyPasteAutomation16(bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, List<int> targetGroupIDs, float x, float y, int targetItemIDAdj) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for the adjustment of the selected Group IDs by a specified number, setting the Target Group IDs to a different number each time from an array and adjusting the Target Item ID by a specified number.</summary>
        public static void InjectPickupItemsCopyPasteAutomation17(int groupIDsAdj, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, List<int> targetGroupIDs, float x, float y, int targetItemIDAdj) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to a different number each time from an array, setting the Target Group IDs to a different number each time from an array and adjusting the Target Item ID by a specified number.</summary>
        public static void InjectPickupItemsCopyPasteAutomation18(List<int> groupIDValues, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, List<int> targetGroupIDs, float x, float y, int targetItemIDAdj) { }
        #endregion
        #region Set Target Item IDs to specified values from an array
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to unused Group IDs, setting the Target Group ID to unused Group IDsand setting the Target Item IDs to a different number each time from an array.</summary>
        public static void InjectPickupItemsCopyPasteAutomation19(bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, float x, float y, List<int> targetItemIDs) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for the adjustment of the selected Group IDs by a specified number, setting the Target Group ID to unused Group IDs and setting the Target Item IDs to unused Item IDs and setting the Target Item IDs to a different number each time from an array.</summary>
        public static void InjectPickupItemsCopyPasteAutomation20(int groupIDsAdj, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, float x, float y, List<int> targetItemIDs) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to a different number each time from an array, setting the Target Group ID to unused Group IDs and setting the Target Item IDs to a different number each time from an array.</summary>
        public static void InjectPickupItemsCopyPasteAutomation21(List<int> groupIDValues, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, float x, float y, List<int> targetItemIDs) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to unused Group IDs, adjusting the Target Group ID by a specified number and setting the Target Item IDs to a different number each time from an array.</summary>
        public static void InjectPickupItemsCopyPasteAutomation22(bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, int targetGroupIDAdj, float x, float y, List<int> targetItemIDs) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for the adjustment of the selected Group IDs by a specified number, adjusting the Target Group ID by a specified number and setting the Target Item IDs to a different number each time from an array.</summary>
        public static void InjectPickupItemsCopyPasteAutomation23(int groupIDsAdj, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, int targetGroupIDAdj, float x, float y, List<int> targetItemIDs) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to a different number each time from an array, adjusting the Target Group ID by a specified number and setting the Target Item IDs to a different number each time from an array.</summary>
        public static void InjectPickupItemsCopyPasteAutomation24(List<int> groupIDValues, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, int targetGroupIDAdj, float x, float y, List<int> targetItemIDs) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to unused Group IDs, setting the Target Group IDs to a different number each time from an array and setting the Target Item IDs to a different number each time from an array.</summary>
        public static void InjectPickupItemsCopyPasteAutomation25(bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, List<int> targetGroupIDs, float x, float y, List<int> targetItemIDs) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for the adjustment of the selected Group IDs by a specified number, setting the Target Group IDs to a different number each time from an array and setting the Target Item IDs to a different number each time from an array.</summary>
        public static void InjectPickupItemsCopyPasteAutomation26(int groupIDsAdj, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, List<int> targetGroupIDs, float x, float y, List<int> targetItemIDs) { }
        /// <summary>Injects the code for the copy-paste automation of pickup items. This overload is for setting the Group IDs to a different number each time from an array, setting the Target Group IDs to a different number each time from an array and setting the Target Item IDs to a different number each time from an array.</summary>
        public static void InjectPickupItemsCopyPasteAutomation27(List<int> groupIDValues, bool[] groupIDs, int targetType, bool subtractCount, bool toggleGroup, bool adjTargetType, bool adjAction, List<int> targetGroupIDs, float x, float y, List<int> targetItemIDs) { }
        #endregion
        #endregion
        #region Text Objects
        /// <summary>Injects the code for the copy-paste automation of text objects. This overload is for setting the Group IDs to unused Group IDs and setting the Used Color IDs to unused Group IDs.</summary>
        public static void InjectTextObjectsCopyPasteAutomation1
        (
            string textToApply, string[] customVariableNames, double[] customVariablesInitialValues, double[] customVariablesAdjustment,
            bool[] adjustedGroupIDs, float x, float y
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of text objects. This overload is for setting the Group IDs to unused Group IDs and adjusting the Used Color IDs by a specified number each time.</summary>
        public static void InjectTextObjectsCopyPasteAutomation2
        (
            string textToApply, string[] customVariableNames, double[] customVariablesInitialValues, double[] customVariablesAdjustment,
            bool[] adjustedGroupIDs, int colorIDAdjustment, float x, float y
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of text objects. This overload is for setting the Group IDs to unused Group IDs and setting the Used Color IDs to a different number each time from an array.</summary>
        public static void InjectTextObjectsCopyPasteAutomation3
        (
            string textToApply, string[] customVariableNames, double[] customVariablesInitialValues, double[] customVariablesAdjustment,
            bool[] adjustedGroupIDs, List<int> colorIDs, float x, float y
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of text objects. This overload is for adjusting the Group IDs by a specified number each time and setting the Used Color IDs to unused Group IDs.</summary>
        public static void InjectTextObjectsCopyPasteAutomation4
        (
            string textToApply, string[] customVariableNames, double[] customVariablesInitialValues, double[] customVariablesAdjustment,
            int adjustedGroupIDsAdjustment, bool[] adjustedGroupIDs, float x, float y
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of text objects. This overload is for adjusting the Group IDs by a specified number each time and adjusting the Used Color IDs by a specified number each time.</summary>
        public static void InjectTextObjectsCopyPasteAutomation5
        (
            string textToApply, string[] customVariableNames, double[] customVariablesInitialValues, double[] customVariablesAdjustment,
            int adjustedGroupIDsAdjustment, bool[] adjustedGroupIDs, int colorIDAdjustment, float x, float y
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of text objects. This overload is for adjusting the Group IDs by a specified number each time and setting the Used Color IDs to a different number each time from an array.</summary>
        public static void InjectTextObjectsCopyPasteAutomation6
        (
            string textToApply, string[] customVariableNames, double[] customVariablesInitialValues, double[] customVariablesAdjustment,
            int adjustedGroupIDsAdjustment, bool[] adjustedGroupIDs, List<int> colorIDs, float x, float y
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of text objects. This overload is for setting the Group IDs to a different number each time from an array and setting the Used Color IDs to unused Group IDs.</summary>
        public static void InjectTextObjectsCopyPasteAutomation7
        (
            string textToApply, string[] customVariableNames, double[] customVariablesInitialValues, double[] customVariablesAdjustment,
            List<int> groupIDs, bool[] adjustedGroupIDs, float x, float y
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of text objects. This overload is for setting the Group IDs to a different number each time from an array and adjusting the Used Color IDs by a specified number each time.</summary>
        public static void InjectTextObjectsCopyPasteAutomation8
        (
            string textToApply, string[] customVariableNames, double[] customVariablesInitialValues, double[] customVariablesAdjustment,
            List<int> groupIDs, bool[] adjustedGroupIDs, int colorIDAdjustment, float x, float y
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of text objects. This overload is for setting the Group IDs to a different number each time from an array and setting the Used Color IDs to a different number each time from an array.</summary>
        public static void InjectTextObjectsCopyPasteAutomation9
        (
            string textToApply, string[] customVariableNames, double[] customVariablesInitialValues, double[] customVariablesAdjustment,
            List<int> groupIDs, bool[] adjustedGroupIDs, List<int> colorIDs, float x, float y
        )
        { }
        #endregion
        #endregion
        #region Rotating Objects
        /// <summary>Injects the code for the copy-paste automation of the rotating objects. This overload is for adjusting the Degrees Per Second with the rotation setting being Custom.</summary>

        public static void InjectRotatingObjectsCopyPasteAutomation1(int DpS, bool randomizeDpS) { }
        /// <summary>Injects the code for the copy-paste automation of the rotating objects. This overload is for the default rotations.</summary>
        public static void InjectRotatingObjectsCopyPasteAutomation2(bool def, bool adjRotationDirection) { }
        #endregion
        #region Triggers
        #region Move
        /// <summary>Injects the code for the copy-paste automation of the Move triggers. This overload is for the adjustment of the Target Group ID by a specified number and setting the Easing to a specified value.</summary>
        public static void InjectMoveTriggerCopyPasteAutomation1
        (
            float moveTime, bool randomizeMoveTime,
            int moveX, bool randomizeMoveX, int moveY, bool randomizeMoveY, bool[] lockToPlayerXY,
            int easing, float easingRate, bool randomizeEasingRate,
            bool useTarget, int targetPosGroupIDAdj, bool[] targetPosXY,
            int targetGroupIDAdj, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Move triggers. This overload is for setting the Target Group IDs to a different number each time from an array and setting the Easing to a specified value.</summary>
        public static void InjectMoveTriggerCopyPasteAutomation2
        (
            float moveTime, bool randomizeMoveTime,            
            int moveX, bool randomizeMoveX, int moveY, bool randomizeMoveY, bool[] lockToPlayerXY,
            int easing, float easingRate, bool randomizeEasingRate,
            bool useTarget, int targetPosGroupIDAdj, bool[] targetPosXY,
            List<int> targetGroupIDValues, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Move triggers. This overload is for setting the Target Group IDs to unused Group IDs and setting the Easing to a specified value.</summary>

        public static void InjectMoveTriggerCopyPasteAutomation3
        (
            float moveTime, bool randomizeMoveTime,
            int moveX, bool randomizeMoveX, int moveY, bool randomizeMoveY, bool[] lockToPlayerXY,
            int easing, float easingRate, bool randomizeEasingRate,
            bool useTarget, int targetPosGroupIDAdj, bool[] targetPosXY,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Move triggers. This overload is for the adjustment of the Target Group ID by a specified number and randomizing the Easing or leaving as is.</summary>

        public static void InjectMoveTriggerCopyPasteAutomation4
        (
            float moveTime, bool randomizeMoveTime,
            int moveX, bool randomizeMoveX, int moveY, bool randomizeMoveY, bool[] lockToPlayerXY,
            bool randomizeEasing, float easingRate, bool randomizeEasingRate,
            bool useTarget, int targetPosGroupIDAdj, bool[] targetPosXY,
            int targetGroupIDAdj, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Move triggers. This overload is for setting the Target Group IDs to a different number each time from an array and randomizing the Easing or leaving as is.</summary>

        public static void InjectMoveTriggerCopyPasteAutomation5
        (
            float moveTime, bool randomizeMoveTime,
            int moveX, bool randomizeMoveX, int moveY, bool randomizeMoveY, bool[] lockToPlayerXY,
            bool randomizeEasing, float easingRate, bool randomizeEasingRate,
            bool useTarget, int targetPosGroupIDAdj, bool[] targetPosXY,
            List<int> targetGroupIDValues, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Move triggers. This overload is for setting the Target Group IDs to unused Group IDs and randomizing the Easing or leaving as is.</summary>

        public static void InjectMoveTriggerCopyPasteAutomation6
        (
            float moveTime, bool randomizeMoveTime,
            int moveX, bool randomizeMoveX, int moveY, bool randomizeMoveY, bool[] lockToPlayerXY,
            bool randomizeEasing, float easingRate, bool randomizeEasingRate,
            bool useTarget, int targetPosGroupIDAdj, bool[] targetPosXY,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #region Rotate
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for the adjustment of the Target Group ID by a specified number, setting the Center Group ID to unused Group IDs and setting the Easing to a specified value.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation1
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            int easing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            int targetGroupIDAdj, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for setting the Target Group IDs to a different number each time from an array, setting the Center Group ID to unused Group IDs and setting the Easing to a specified value.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation2
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            int easing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            List<int> targetGroupIDValues, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for setting the Target Group IDs to unused Group IDs, setting the Center Group ID to unused Group IDs and setting the Easing to a specified value.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation3
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            int easing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for the adjustment of the Target Group ID by a specified number, setting the Center Group ID to unused Group IDs and randomizing the Easing.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation4
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            bool randomizeEasing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            int targetGroupIDAdj, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for setting the Target Group IDs to a different number each time from an array, setting the Center Group ID to unused Group IDs and randomizing the Easing.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation5
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            bool randomizeEasing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            List<int> targetGroupIDValues, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for setting the Target Group IDs to unused Group IDs, setting the Center Group ID to unused Group IDs and randomizing the Easing.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation6
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            bool randomizeEasing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for the adjustment of the Target Group ID by a specified number, adjusting the Center Group ID by a specified number and setting the Easing to a specified value.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation7
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            int easing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            int targetGroupIDAdj, float x, float y, int centerGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for setting the Target Group IDs to a different number each time from an array, adjusting the Center Group ID by a specified number and setting the Easing to a specified value.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation8
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            int easing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            List<int> targetGroupIDValues, float x, float y, int centerGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for setting the Target Group IDs to unused Group IDs, adjusting the Center Group ID by a specified number and setting the Easing to a specified value.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation9
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            int easing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            float x, float y, int centerGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for the adjustment of the Target Group ID by a specified number, adjusting the Center Group ID by a specified number and randomizing the Easing.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation10
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            bool randomizeEasing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            int targetGroupIDAdj, float x, float y, int centerGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for setting the Target Group IDs to a different number each time from an array, adjusting the Center Group ID by a specified number and randomizing the Easing.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation11
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            bool randomizeEasing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            List<int> targetGroupIDValues, float x, float y, int centerGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for setting the Target Group IDs to unused Group IDs, adjusting the Center Group ID by a specified number and randomizing the Easing.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation12
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            bool randomizeEasing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            float x, float y, int centerGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for the adjustment of the Target Group ID by a specified number, setting the Center Group IDs to a different number each time from an array and setting the Easing to a specified value.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation13
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            int easing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            int targetGroupIDAdj, float x, float y, List<int> centerGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for setting the Target Group IDs to a different number each time from an array, setting the Center Group IDs to a different number each time from an array and setting the Easing to a specified value.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation14
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            int easing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            List<int> targetGroupIDValues, float x, float y, List<int> centerGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for setting the Target Group IDs to unused Group IDs, setting the Center Group IDs to a different number each time from an array and setting the Easing to a specified value.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation15
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            int easing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            float x, float y, List<int> centerGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for the adjustment of the Target Group ID by a specified number, setting the Center Group IDs to a different number each time from an array and randomizing the Easing.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation16
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            bool randomizeEasing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            int targetGroupIDAdj, float x, float y, List<int> centerGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for setting the Target Group IDs to a different number each time from an array, setting the Center Group IDs to a different number each time from an array and randomizing the Easing.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation17
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            bool randomizeEasing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            List<int> targetGroupIDValues, float x, float y, List<int> centerGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Rotate triggers. This overload is for setting the Target Group IDs to unused Group IDs, setting the Center Group IDs to a different number each time from an array and randomizing the Easing.</summary>

        public static void InjectRotateTriggerCopyPasteAutomation18
        (
            float rotationTime, bool randomizeRotationTime,
            int degrees, bool randomizeDegrees, int times360, bool randomizeTimes360,
            bool randomizeEasing, float easingRate, bool randomizeEasingRate,
            bool lockObjRotation, bool adjLockObjRotation,
            float x, float y, List<int> centerGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #region Stop
        /// <summary>Injects the code for the copy-paste automation of the Stop triggers. This overload is for the adjustment of the Target Group ID by a specified number.</summary>

        public static void InjectStopTriggerCopyPasteAutomation1(int targetGroupIDAdj, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Stop triggers. This overload is for setting the Target Group IDs to a different number each time from an array.</summary>

        public static void InjectStopTriggerCopyPasteAutomation2(List<int> targetGroupIDValues, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Stop triggers. This overload is for setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectStopTriggerCopyPasteAutomation3(float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        #endregion
        #region On Death
        /// <summary>Injects the code for the copy-paste automation of the On Death triggers. This overload is for the adjustment of the Target Group ID by a specified number.</summary>

        public static void InjectOnDeathTriggerCopyPasteAutomation1(bool activateGroup, bool adjActivateGroup, int targetGroupIDAdj, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the On Death triggers. This overload is for setting the Target Group IDs to a different number each time from an array.</summary>

        public static void InjectOnDeathTriggerCopyPasteAutomation2(bool activateGroup, bool adjActivateGroup, List<int> targetGroupIDValues, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the On Death triggers. This overload is for setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectOnDeathTriggerCopyPasteAutomation3(bool activateGroup, bool adjActivateGroup, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        #endregion
        #region Toggle
        /// <summary>Injects the code for the copy-paste automation of the Toggle triggers. This overload is for the adjustment of the Target Group ID by a specified number.</summary>

        public static void InjectToggleTriggerCopyPasteAutomation1(bool activateGroup, bool adjActivateGroup, int targetGroupIDAdj, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Toggle triggers. This overload is for setting the Target Group IDs to a different number each time from an array.</summary>

        public static void InjectToggleTriggerCopyPasteAutomation2(bool activateGroup, bool adjActivateGroup, List<int> targetGroupIDValues, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Toggle triggers. This overload is for setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectToggleTriggerCopyPasteAutomation3(bool activateGroup, bool adjActivateGroup, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        #endregion
        #region Spawn
        /// <summary>Injects the code for the copy-paste automation of the Spawn triggers. This overload is for the adjustment of the Target Group ID by a specified number.</summary>

        public static void InjectSpawnTriggerCopyPasteAutomation1(float delay, bool randomizeDelay, int targetGroupIDAdj, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Spawn triggers. This overload is for setting the Target Group IDs to a different number each time from an array.</summary>

        public static void InjectSpawnTriggerCopyPasteAutomation2(float delay, bool randomizeDelay, List<int> targetGroupIDValues, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Spawn triggers. This overload is for setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectSpawnTriggerCopyPasteAutomation3(float delay, bool randomizeDelay, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        #endregion
        #region Pickup
        /// <summary>Injects the code for the copy-paste automation of the Pickup triggers. This overload is for the adjustment of the Target Item ID by a specified number.</summary>

        public static void InjectPickupTriggerCopyPasteAutomation1(int count, bool randomizeCount, int targetItemIDAdj, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Pickup triggers. This overload is for setting the Target Item IDs to a different number each time from an array.</summary>

        public static void InjectPickupTriggerCopyPasteAutomation2(int count, bool randomizeCount, List<int> targetItemIDValues, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Pickup triggers. This overload is for setting the Target Item IDs to unused Item IDs.</summary>

        public static void InjectPickupTriggerCopyPasteAutomation3(int count, bool randomizeCount, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        #endregion
        #region Animate
        /// <summary>Injects the code for the copy-paste automation of the Animate triggers. This overload is for the adjustment of the Target Group ID by a specified number.</summary>

        public static void InjectAnimateTriggerCopyPasteAutomation1(int animationID, bool randomizeAnimationID, int targetGroupIDAdj, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Animate triggers. This overload is for setting the Target Group IDs to a different number each time from an array.</summary>

        public static void InjectAnimateTriggerCopyPasteAutomation2(int animationID, bool randomizeAnimationID, List<int> targetGroupIDValues, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Animate triggers. This overload is for setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectAnimateTriggerCopyPasteAutomation3(int animationID, bool randomizeAnimationID, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        #endregion
        #region Shake
        /// <summary>Injects the code for the copy-paste automation of the Animate triggers.</summary>

        public static void InjectShakeTriggerCopyPasteAutomation(float strength, float interval, float duration, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        #endregion
        #region Count
        /// <summary>Injects the code for the copy-paste automation of the Count triggers. This overload is for the adjustment of the Target Group ID by a specified number and setting the Target Item IDs to unused Item IDs.</summary>

        public static void InjectCountTriggerCopyPasteAutomation1
        (
            bool activateGroup, bool adjActivateGroup, bool multiActivate, bool adjMultiActivate, int targetCount,
            int targetGroupIDAdj, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Count triggers. This overload is for setting the Target Group IDs to a different number each time from an array and setting the Target Item IDs to unused Item IDs.</summary>

        public static void InjectCountTriggerCopyPasteAutomation2
        (
            bool activateGroup, bool adjActivateGroup, bool multiActivate, bool adjMultiActivate, int targetCount,
            List<int> targetGroupIDValues, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Count triggers. This overload is for setting the Target Group IDs to unused Group IDs and setting the Target Item IDs to unused Item IDs.</summary>

        public static void InjectCountTriggerCopyPasteAutomation3
        (
            bool activateGroup, bool adjActivateGroup, bool multiActivate, bool adjMultiActivate, int targetCount,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Count triggers. This overload is for the adjustment of the Target Group ID by a specified number and adjusting the Target Item IDs by a specified number.</summary>

        public static void InjectCountTriggerCopyPasteAutomation4
        (
            bool activateGroup, bool adjActivateGroup, bool multiActivate, bool adjMultiActivate, int targetCount,
            int targetGroupIDAdj, float x, float y, int targetItemIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Count triggers. This overload is for setting the Target Group IDs to a different number each time from an array and adjusting the Target Item IDs by a specified number.</summary>

        public static void InjectCountTriggerCopyPasteAutomation5
        (
            bool activateGroup, bool adjActivateGroup, bool multiActivate, bool adjMultiActivate, int targetCount,
            List<int> targetGroupIDValues, float x, float y, int targetItemIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Count triggers. This overload is for setting the Target Group IDs to unused Group IDs and adjusting the Target Item IDs by a specified number.</summary>

        public static void InjectCountTriggerCopyPasteAutomation6
        (
            bool activateGroup, bool adjActivateGroup, bool multiActivate, bool adjMultiActivate, int targetCount,
            float x, float y, int targetItemIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Count triggers. This overload is for the adjustment of the Target Group ID by a specified number and adjusting the Target Item IDs by a specified number.</summary>

        public static void InjectCountTriggerCopyPasteAutomation7
        (
            bool activateGroup, bool adjActivateGroup, bool multiActivate, bool adjMultiActivate, int targetCount,
            int targetGroupIDAdj, float x, float y, List<int> targetItemIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Count triggers. This overload is for setting the Target Group IDs to a different number each time from an array and adjusting the Target Item IDs by a specified number.</summary>

        public static void InjectCountTriggerCopyPasteAutomation8
        (
            bool activateGroup, bool adjActivateGroup, bool multiActivate, bool adjMultiActivate, int targetCount,
            List<int> targetGroupIDValues, float x, float y, List<int> targetItemIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Count triggers. This overload is for setting the Target Group IDs to unused Group IDs and adjusting the Target Item IDs by a specified number.</summary>

        public static void InjectCountTriggerCopyPasteAutomation9
        (
            bool activateGroup, bool adjActivateGroup, bool multiActivate, bool adjMultiActivate, int targetCount,
            float x, float y, List<int> targetItemIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #region Instant Count
        /// <summary>Injects the code for the copy-paste automation of the Instant Count triggers. This overload is for the adjustment of the Target Group ID by a specified number and setting the Target Item IDs to unused Item IDs.</summary>

        public static void InjectInstantCountTriggerCopyPasteAutomation1
        (
            bool activateGroup, bool adjActivateGroup,
            bool smaller, bool larger, bool equals, bool adjComparison, int targetCount,
            int targetGroupIDAdj, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Instant Count triggers. This overload is for setting the Target Group IDs to a different number each time from an array and setting the Target Item IDs to unused Item IDs.</summary>

        public static void InjectInstantCountTriggerCopyPasteAutomation2
        (
            bool activateGroup, bool adjActivateGroup,
            bool smaller, bool larger, bool equals, bool adjComparison, int targetCount,
            List<int> targetGroupIDValues, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Instant Count triggers. This overload is for setting the Target Group IDs to unused Group IDs and setting the Target Item IDs to unused Item IDs.</summary>

        public static void InjectInstantCountTriggerCopyPasteAutomation3
        (
            bool activateGroup, bool adjActivateGroup,
            bool smaller, bool larger, bool equals, bool adjComparison, int targetCount,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Instant Count triggers. This overload is for the adjustment of the Target Group ID by a specified number and adjusting the Target Item IDs by a specified number.</summary>

        public static void InjectInstantCountTriggerCopyPasteAutomation4
        (
            bool activateGroup, bool adjActivateGroup,
            bool smaller, bool larger, bool equals, bool adjComparison, int targetCount,
            int targetGroupIDAdj, float x, float y, int targetItemIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Instant Count triggers. This overload is for setting the Target Group IDs to a different number each time from an array and adjusting the Target Item IDs by a specified number.</summary>

        public static void InjectInstantCountTriggerCopyPasteAutomation5
        (
            bool activateGroup, bool adjActivateGroup,
            bool smaller, bool larger, bool equals, bool adjComparison, int targetCount,
            List<int> targetGroupIDValues, float x, float y, int targetItemIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Instant Count triggers. This overload is for setting the Target Group IDs to unused Group IDs and adjusting the Target Item IDs by a specified number.</summary>

        public static void InjectInstantCountTriggerCopyPasteAutomation6
        (
            bool activateGroup, bool adjActivateGroup,
            bool smaller, bool larger, bool equals, bool adjComparison, int targetCount,
            float x, float y, int targetItemIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Instant Count triggers. This overload is for the adjustment of the Target Group ID by a specified number and adjusting the Target Item IDs by a specified number.</summary>

        public static void InjectInstantCountTriggerCopyPasteAutomation7
        (
            bool activateGroup, bool adjActivateGroup,
            bool smaller, bool larger, bool equals, bool adjComparison, int targetCount,
            int targetGroupIDAdj, float x, float y, List<int> targetItemIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Instant Count triggers. This overload is for setting the Target Group IDs to a different number each time from an array and adjusting the Target Item IDs by a specified number.</summary>

        public static void InjectInstantCountTriggerCopyPasteAutomation8
        (
            bool activateGroup, bool adjActivateGroup,
            bool smaller, bool larger, bool equals, bool adjComparison, int targetCount,
            List<int> targetGroupIDValues, float x, float y, List<int> targetItemIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Instant Count triggers. This overload is for setting the Target Group IDs to unused Group IDs and adjusting the Target Item IDs by a specified number.</summary>

        public static void InjectInstantCountTriggerCopyPasteAutomation9
        (
            bool activateGroup, bool adjActivateGroup,
            bool smaller, bool larger, bool equals, bool adjComparison, int targetCount,
            float x, float y, List<int> targetItemIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #region Follow
        /// <summary>Injects the code for the copy-paste automation of the Follow triggers. This overload is for the adjustment of the Target Group ID by a specified number and setting the Follow Group IDs to unused Group IDs.</summary>

        public static void InjectFollowTriggerCopyPasteAutomation1
        (
            float xMod, float yMod, float moveTime,
            int targetGroupIDAdj, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Follow triggers. This overload is for setting the Target Group IDs to a different number each time from an array and setting the Follow Group IDs to unused Group IDs.</summary>

        public static void InjectFollowTriggerCopyPasteAutomation2
        (
            float xMod, float yMod, float moveTime,
            List<int> targetGroupIDValues, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Follow triggers. This overload is for setting the Target Group IDs to unused Group IDs and setting the Follow Group IDs to unused Group IDs.</summary>

        public static void InjectFollowTriggerCopyPasteAutomation3
        (
            float xMod, float yMod, float moveTime,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Follow triggers. This overload is for the adjustment of the Target Group ID by a specified number and adjusting the Follow Group IDs by a specified number.</summary>

        public static void InjectFollowTriggerCopyPasteAutomation4
        (
            float xMod, float yMod, float moveTime,
            int targetGroupIDAdj, float x, float y, int followGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Follow triggers. This overload is for setting the Target Group IDs to a different number each time from an array and adjusting the Follow Group IDs by a specified number.</summary>

        public static void InjectFollowTriggerCopyPasteAutomation5
        (
            float xMod, float yMod, float moveTime,
            List<int> targetGroupIDValues, float x, float y, int followGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Follow triggers. This overload is for setting the Target Group IDs to unused Group IDs and adjusting the Follow Group IDs by a specified number.</summary>

        public static void InjectFollowTriggerCopyPasteAutomation6
        (
            float xMod, float yMod, float moveTime,
            float x, float y, int followGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Follow triggers. This overload is for the adjustment of the Target Group ID by a specified number and adjusting the Follow Group IDs by a specified number.</summary>

        public static void InjectFollowTriggerCopyPasteAutomation7
        (
            float xMod, float yMod, float moveTime,
            int targetGroupIDAdj, float x, float y, List<int> followGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Follow triggers. This overload is for setting the Target Group IDs to a different number each time from an array and adjusting the Follow Group IDs by a specified number.</summary>

        public static void InjectFollowTriggerCopyPasteAutomation8
        (
            float xMod, float yMod, float moveTime,
            List<int> targetGroupIDValues, float x, float y, List<int> followGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Follow triggers. This overload is for setting the Target Group IDs to unused Group IDs and adjusting the Follow Group IDs by a specified number.</summary>

        public static void InjectFollowTriggerCopyPasteAutomation9
        (
            float xMod, float yMod, float moveTime,
            float x, float y, List<int> followGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #region Follow Player Y
        /// <summary>Injects the code for the copy-paste automation of the Follow Player Y triggers. This overload is for the adjustment of the Target Group ID by a specified number.</summary>

        public static void InjectFollowPlayerYTriggerCopyPasteAutomation1
        (
            float speed, float delay, float maxSpeed, int offset, float moveTime,
            int targetGroupIDAdj, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Follow Player Y triggers. This overload is for setting the Target Group IDs to a different number each time from an array.</summary>

        public static void InjectFollowPlayerYTriggerCopyPasteAutomation2
        (
            float speed, float delay, float maxSpeed, int offset, float moveTime,
            List<int> targetGroupIDValues, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Follow Player Y triggers. This overload is for setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectFollowPlayerYTriggerCopyPasteAutomation3
        (
            float speed, float delay, float maxSpeed, int offset, float moveTime,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #region Touch
        /// <summary>Injects the code for the copy-paste automation of the Touch triggers. This overload is for the adjustment of the Target Group ID by a specified number and setting the Easing to a specified value.</summary>

        public static void InjectTouchTriggerCopyPasteAutomation1(bool activateGroup, bool adjActivateGroup, bool holdMove, bool adjHoldMove, bool dualMove, bool adjDualMode, int targetGroupIDAdj, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Touch triggers. This overload is for setting the Target Group IDs to a different number each time from an array.</summary>

        public static void InjectTouchTriggerCopyPasteAutomation2(bool activateGroup, bool adjActivateGroup, bool holdMove, bool adjHoldMove, bool dualMove, bool adjDualMode, List<int> targetGroupIDValues, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Touch triggers. This overload is for setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectTouchTriggerCopyPasteAutomation3(bool activateGroup, bool adjActivateGroup, bool holdMove, bool adjHoldMove, bool dualMove, bool adjDualMode, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        #endregion
        #region Alpha
        /// <summary>Injects the code for the copy-paste automation of the Alpha triggers. This overload is for the adjustment of the Target Group ID by a specified number and setting the Easing to a specified value.</summary>

        public static void InjectAlphaTriggerCopyPasteAutomation1(float fadeTime, bool randomizeFadeTime, float opacity, bool randomizeOpacity, int targetGroupIDAdj, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Alpha triggers. This overload is for setting the Target Group IDs to a different number each time from an array.</summary>

        public static void InjectAlphaTriggerCopyPasteAutomation2(float fadeTime, bool randomizeFadeTime, float opacity, bool randomizeOpacity, List<int> targetGroupIDValues, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        /// <summary>Injects the code for the copy-paste automation of the Alpha triggers. This overload is for setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectAlphaTriggerCopyPasteAutomation3(float fadeTime, bool randomizeFadeTime, float opacity, bool randomizeOpacity, float x, float y, bool touchTriggered, bool spawnTriggered, bool multiTrigger) { }
        #endregion
        #region Collision
        #region Set Target Group IDs to unused Group IDs
        #region Set Block B IDs to unused Block IDs
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group ID to unused Group IDs, setting the Block A IDs to unused Block IDs and setting the Block B IDs to unused Block IDs.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation1
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group ID to unused Group IDs, adjusting the Block A IDs by a number each time and setting the Block B IDs to unused Block IDs.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation2
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            int blockAIDAdj,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group ID to unused Group IDs, setting the Block A IDs to a different number each time from an array and setting the Block B IDs to unused Block IDs.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation3
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            List<int> blockAIDValues,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #region Adjust Block B IDs by a specified number each time
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group ID to unused Group IDs, setting the Block A IDs to unused Block IDs and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation4
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            float x, float y,
            int blockBIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group ID to unused Group IDs, adjusting the Block A IDs by a number each time and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation5
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            int blockAIDAdj,
            float x, float y,
            int blockBIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group ID to unused Group IDs, setting the Block A IDs to a different number each time from an array and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation6
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            List<int> blockAIDValues,
            float x, float y,
            int blockBIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #region Set Block B IDs to a different number each time from an array
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group ID to unused Group IDs, setting the Block A IDs to unused Block IDs and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation7
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            float x, float y,
            List<int> blockBIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group ID to unused Group IDs, adjusting the Block A IDs by a number each time and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation8
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            int blockAIDAdj,
            float x, float y,
            List<int> blockBIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group ID to unused Group IDs, setting the Block A IDs to a different number each time from an array and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation9
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            List<int> blockAIDValues,
            float x, float y,
            List<int> blockBIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #endregion
        #region Adjust Target Group IDs by a specified number each time
        #region Set Block B IDs to unused Block IDs
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for adjusting the Target Group ID by a specified number, setting the Block A IDs to unused Block IDs and setting the Block B IDs to unused Block IDs.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation10
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int targetGroupIDAdj
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for adjusting the Target Group ID by a specified number, adjusting the Block A IDs by a number each time and setting the Block B IDs to unused Block IDs.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation11
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            int blockAIDAdj,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int targetGroupIDAdj
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for adjusting the Target Group ID by a specified number, setting the Block A IDs to a different number each time from an array and setting the Block B IDs to unused Block IDs.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation12
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            List<int> blockAIDValues,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int targetGroupIDAdj
        )
        { }
        #endregion
        #region Adjust Block B IDs by a specified number each time
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for adjusting the Target Group ID by a specified number, setting the Block A IDs to unused Block IDs and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation13
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            float x, float y,
            int blockBIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int targetGroupIDAdj
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for adjusting the Target Group ID by a specified number, adjusting the Block A IDs by a number each time and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation14
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            int blockAIDAdj,
            float x, float y,
            int blockBIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int targetGroupIDAdj
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for adjusting the Target Group ID by a specified number, setting the Block A IDs to a different number each time from an array and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation15
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            List<int> blockAIDValues,
            float x, float y,
            int blockBIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int targetGroupIDAdj
        )
        { }
        #endregion
        #region Set Block B IDs to a different number each time from an array
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for adjusting the Target Group ID by a specified number, setting the Block A IDs to unused Block IDs and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation16
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            float x, float y,
            List<int> blockBIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int targetGroupIDAdj
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for adjusting the Target Group ID by a specified number, adjusting the Block A IDs by a number each time and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation17
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            int blockAIDAdj,
            float x, float y,
            List<int> blockBIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int targetGroupIDAdj
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for adjusting the Target Group ID by a specified number, setting the Block A IDs to a different number each time from an array and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation18
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            List<int> blockAIDValues,
            float x, float y,
            List<int> blockBIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int targetGroupIDAdj
        )
        { }
        #endregion
        #endregion
        #region Set Target Group IDs to a different value from an array each time
        #region Set Block B IDs to unused Block IDs
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group IDs to a different number each time from an array, setting the Block A IDs to unused Block IDs and setting the Block B IDs to unused Block IDs.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation19
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> targetGroupIDValues
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group IDs to a different number each time from an array, adjusting the Block A IDs by a number each time and setting the Block B IDs to unused Block IDs.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation20
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            int blockAIDAdj,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> targetGroupIDValues
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group IDs to a different number each time from an array, setting the Block A IDs to a different number each time from an array and setting the Block B IDs to unused Block IDs.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation21
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            List<int> blockAIDValues,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> targetGroupIDValues
        )
        { }
        #endregion
        #region Adjust Block B IDs by a specified number each time
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group IDs to a different number each time from an array, setting the Block A IDs to unused Block IDs and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation22
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            float x, float y,
            int blockBIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> targetGroupIDValues
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group IDs to a different number each time from an array, adjusting the Block A IDs by a number each time and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation23
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            int blockAIDAdj,
            float x, float y,
            int blockBIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> targetGroupIDValues
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group IDs to a different number each time from an array, setting the Block A IDs to a different number each time from an array and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation24
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            List<int> blockAIDValues,
            float x, float y,
            int blockBIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> targetGroupIDValues
        )
        { }
        #endregion
        #region Set Block B IDs to a different number each time from an array
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group IDs to a different number each time from an array, setting the Block A IDs to unused Block IDs and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation25
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            float x, float y,
            List<int> blockBIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> targetGroupIDValues
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group IDs to a different number each time from an array, adjusting the Block A IDs by a number each time and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation26
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            int blockAIDAdj,
            float x, float y,
            List<int> blockBIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> targetGroupIDValues
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Collision triggers. This overload is for setting the Target Group IDs to a different number each time from an array, setting the Block A IDs to a different number each time from an array and adjusting the Block B IDs by a number each time.</summary>

        public static void InjectCollisionTriggerCopyPasteAutomation27
        (
            bool activateGroup, bool adjActivateGroup, bool triggerOnExit, bool adjTriggerOnExit,
            List<int> blockAIDValues,
            float x, float y,
            List<int> blockBIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> targetGroupIDValues
        )
        { }
        #endregion
        #endregion
        #endregion
        #region Pulse
        #region Set Copied Color IDs to unused Color IDs
        #region Set Target Group IDs to unused Group IDs
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color ID to unused Color IDs, setting the Target Color IDs to unused Color IDs and setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation1
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color ID to unused Color IDs, adjusting the Target Color IDs by a number each time and setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation2
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            int targetColorIDAdj,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color ID to unused Color IDs, setting the Target Color IDs to a different number each time from an array and setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation3
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            List<int> targetColorIDValues,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #region Adjust Target Group IDs by a specified number each time
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color ID to unused Color IDs, setting the Target Color IDs to unused Color IDs and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation4
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            float x, float y,
            int targetGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color ID to unused Color IDs, adjusting the Target Color IDs by a number each time and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation5
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            int targetColorIDAdj,
            float x, float y,
            int targetGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color ID to unused Color IDs, setting the Target Color IDs to a different number each time from an array and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation6
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            List<int> targetColorIDValues,
            float x, float y,
            int targetGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #region Set Target Group IDs to a different number each time from an array
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color ID to unused Color IDs, setting the Target Color IDs to unused Color IDs and setting the Target Group IDs to a different number each time from an array.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation7
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            float x, float y,
            List<int> targetGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color ID to unused Color IDs, adjusting the Target Color IDs by a number each time and setting the Target Group IDs to a different number each time from an array.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation8
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            int targetColorIDAdj,
            float x, float y,
            List<int> targetGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color ID to unused Color IDs, setting the Target Color IDs to a different number each time from an array and setting the Target Group IDs to a different number each time from an array.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation9
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            List<int> targetColorIDValues,
            float x, float y,
            List<int> targetGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #endregion
        #region Adjust Copied Color IDs by a specified number each time
        #region Set Target Group IDs to unused Group IDs
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for adjusting the Copied Color ID by a specified number, setting the Target Color IDs to unused Color IDs and setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation10
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int copiedColorIDAdj
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for adjusting the Copied Color ID by a specified number, adjusting the Target Color IDs by a number each time and setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation11
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            int targetColorIDAdj,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int copiedColorIDAdj
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for adjusting the Copied Color ID by a specified number, setting the Target Color IDs to a different number each time from an array and setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation12
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            List<int> targetColorIDValues,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int copiedColorIDAdj
        )
        { }
        #endregion
        #region Adjust Target Group IDs by a specified number each time
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for adjusting the Copied Color ID by a specified number, setting the Target Color IDs to unused Color IDs and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation13
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            float x, float y,
            int targetGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int copiedColorIDAdj
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for adjusting the Copied Color ID by a specified number, adjusting the Target Color IDs by a number each time and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation14
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            int targetColorIDAdj,
            float x, float y,
            int targetGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int copiedColorIDAdj
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for adjusting the Copied Color ID by a specified number, setting the Target Color IDs to a different number each time from an array and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation15
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            List<int> targetColorIDValues,
            float x, float y,
            int targetGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int copiedColorIDAdj
        )
        { }
        #endregion
        #region Set Target Group IDs to a different number each time from an array
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for adjusting the Copied Color ID by a specified number, setting the Target Color IDs to unused Color IDs and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation16
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            float x, float y,
            List<int> targetGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int copiedColorIDAdj
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for adjusting the Copied Color ID by a specified number, adjusting the Target Color IDs by a number each time and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation17
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            int targetColorIDAdj,
            float x, float y,
            List<int> targetGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int copiedColorIDAdj
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for adjusting the Copied Color ID by a specified number, setting the Target Color IDs to a different number each time from an array and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation18
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            List<int> targetColorIDValues,
            float x, float y,
            List<int> targetGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, int copiedColorIDAdj
        )
        { }
        #endregion
        #endregion
        #region Set Copied Color IDs to a different value from an array each time
        #region Set Target Group IDs to unused Group IDs
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color IDs to a different number each time from an array, setting the Target Color IDs to unused Color IDs and setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation19
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> copiedColorIDValues
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color IDs to a different number each time from an array, adjusting the Target Color IDs by a number each time and setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation20
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            int targetColorIDAdj,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> copiedColorIDValues
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color IDs to a different number each time from an array, setting the Target Color IDs to a different number each time from an array and setting the Target Group IDs to unused Group IDs.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation21
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            List<int> targetColorIDValues,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> copiedColorIDValues
        )
        { }
        #endregion
        #region Adjust Target Group IDs by a specified number each time
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color IDs to a different number each time from an array, setting the Target Color IDs to unused Block IDs and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation22
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            float x, float y,
            int targetGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> copiedColorIDValues
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color IDs to a different number each time from an array, adjusting the Target Color IDs by a number each time and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation23
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            int targetColorIDAdj,
            float x, float y,
            int targetGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> copiedColorIDValues
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color IDs to a different number each time from an array, setting the Target Color IDs to a different number each time from an array and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation24
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            List<int> targetColorIDValues,
            float x, float y,
            int targetGroupIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> copiedColorIDValues
        )
        { }
        #endregion
        #region Set Target Group IDs to a different number each time from an array
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color IDs to a different number each time from an array, setting the Target Color IDs to unused Block IDs and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation25
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            float x, float y,
            List<int> targetGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> copiedColorIDValues
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color IDs to a different number each time from an array, adjusting the Target Color IDs by a number each time and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation26
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            int targetColorIDAdj,
            float x, float y,
            List<int> targetGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> copiedColorIDValues
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Pulse triggers. This overload is for setting the Copied Color IDs to a different number each time from an array, setting the Target Color IDs to a different number each time from an array and adjusting the Target Group IDs by a number each time.</summary>

        public static void InjectPulseTriggerCopyPasteAutomation27
        (
            int targetType, bool[] mainDetailColor, bool adjMainDetail, float fadeIn, float hold, float fadeOut, int copyMode, int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            List<int> targetColorIDValues,
            float x, float y,
            List<int> targetGroupIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger, List<int> copiedColorIDValues
        )
        { }
        #endregion
        #endregion
        #endregion
        #region Color
        /// <summary>Injects the code for the copy-paste automation of the Color triggers. This overload is for setting the Copied Color IDs to unused Color IDs and setting the Target Color IDs to unused Color IDs.</summary>

        public static void InjectColorTriggerCopyPasteAutomation1
        (
            bool blending, bool adjBlending, float fadeTime, bool randomizeFadeTime, float opacity,
            int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Color triggers. This overload is for setting the Copied Color IDs to a different number each time from an array and setting the Target Color IDs to unused Color IDs.</summary>

        public static void InjectColorTriggerCopyPasteAutomation2
        (
            bool blending, bool adjBlending, float fadeTime, bool randomizeFadeTime, float opacity,
            int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            List<int> copiedColorIDValues, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Color triggers. This overload is for adjusting the Copied Color IDs by a number each time and setting the Target Color IDs to unused Color IDs.</summary>

        public static void InjectColorTriggerCopyPasteAutomation3
        (
            bool blending, bool adjBlending, float fadeTime, bool randomizeFadeTime, float opacity,
            int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            int copiedColorIDAdj, float x, float y,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Color triggers. This overload is for setting the Copied Color IDs to unused Color IDs and adjusting the Target Color IDs by a number each time.</summary>

        public static void InjectColorTriggerCopyPasteAutomation4
        (
            bool blending, bool adjBlending, float fadeTime, bool randomizeFadeTime, float opacity,
            int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            float x, float y, int targetColorIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Color triggers. This overload is for setting the Copied Color IDs to a different number each time from an array and adjusting the Target Color IDs by a number each time.</summary>

        public static void InjectColorTriggerCopyPasteAutomation5
        (
            bool blending, bool adjBlending, float fadeTime, bool randomizeFadeTime, float opacity,
            int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            List<int> copiedColorIDValues, float x, float y, int targetColorIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Color triggers. This overload is for adjusting the Copied Color IDs by a number each time and adjusting the Target Color IDs by a number each time.</summary>

        public static void InjectColorTriggerCopyPasteAutomation6
        (
            bool blending, bool adjBlending, float fadeTime, bool randomizeFadeTime, float opacity,
            int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            int copiedColorIDAdj, float x, float y, int targetColorIDAdj,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Color triggers. This overload is for setting the Copied Color IDs to unused Color IDs and setting the Target Color IDs to a different number each time from an array.</summary>

        public static void InjectColorTriggerCopyPasteAutomation7
        (
            bool blending, bool adjBlending, float fadeTime, bool randomizeFadeTime, float opacity,
            int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            float x, float y, List<int> targetColorIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Color triggers. This overload is for setting the Copied Color IDs to a different number each time from an array and setting the Target Color IDs to a different number each time from an array.</summary>

        public static void InjectColorTriggerCopyPasteAutomation8
        (
            bool blending, bool adjBlending, float fadeTime, bool randomizeFadeTime, float opacity,
            int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            List<int> copiedColorIDValues, float x, float y, List<int> targetColorIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        /// <summary>Injects the code for the copy-paste automation of the Color triggers. This overload is for adjusting the Copied Color IDs by a number each time and setting the Target Color IDs to a different number each time from an array.</summary>

        public static void InjectColorTriggerCopyPasteAutomation9
        (
            bool blending, bool adjBlending, float fadeTime, bool randomizeFadeTime, float opacity,
            int[] RGB, bool[] randomizeRGB, int[] HSV, bool[] randomizeHSV,
            int copiedColorIDAdj, float x, float y, List<int> targetColorIDValues,
            bool touchTriggered, bool spawnTriggered, bool multiTrigger
        )
        { }
        #endregion
        #endregion

        public static void ApplyNewSettings
        (
            List<int> objectIDs, float x, float y, float hue1, float saturation1, float brightness1, float hue2, float saturation2, float brightness2, float scaling, float rotation,
            int zOrder, int zLayer, int el1, int el2, List<int> color1IDs, List<int> color2IDs, List<int> values, bool[] groupIDs,
            AdjustmentMode color1IDAdjustmentMode, AdjustmentMode color2IDAdjustmentMode, AdjustmentMode groupIDAdjustmentMode
        )
        {
            // TODO: Fix the function below this line
            int index = ChangeCopyPasteAutomationSettings(objectIDs);
            if (index > -1)
            {
                EffectSome.CopyPasteSettings[index].X = x;
                EffectSome.CopyPasteSettings[index].Y = y;
                EffectSome.CopyPasteSettings[index].Hue1 = hue1;
                EffectSome.CopyPasteSettings[index].Saturation1 = saturation1;
                EffectSome.CopyPasteSettings[index].Brightness1 = brightness1;
                EffectSome.CopyPasteSettings[index].Hue2 = hue2;
                EffectSome.CopyPasteSettings[index].Saturation2 = saturation2;
                EffectSome.CopyPasteSettings[index].Brightness2 = brightness2;
                EffectSome.CopyPasteSettings[index].Scaling = scaling;
                EffectSome.CopyPasteSettings[index].Rotation = rotation;
                EffectSome.CopyPasteSettings[index].ZOrder = zOrder;
                EffectSome.CopyPasteSettings[index].ZLayer = zLayer;
                EffectSome.CopyPasteSettings[index].EL1 = el1;
                EffectSome.CopyPasteSettings[index].EL2 = el2;
                EffectSome.CopyPasteSettings[index].Color1IDs = color1IDs;
                EffectSome.CopyPasteSettings[index].Color1IDValueCounter = 0;
                EffectSome.CopyPasteSettings[index].Color1IDValueAdjustmentMode = color1IDAdjustmentMode;
                EffectSome.CopyPasteSettings[index].Color2IDs = color2IDs;
                EffectSome.CopyPasteSettings[index].Color2IDValueCounter = 0;
                EffectSome.CopyPasteSettings[index].Color2IDValueAdjustmentMode = color2IDAdjustmentMode;
                EffectSome.CopyPasteSettings[index].GroupIDs = values;
                for (int i = 0; i < groupIDs.Length; i++)
                {
                    if (groupIDs[i])
                        EffectSome.CopyPasteSettings[index].GroupIDValueCounters[i] = 0;
                    EffectSome.CopyPasteSettings[index].GroupIDValueAdjustmentModes[i] = groupIDs[i] ? AdjustmentMode.FlatAdjustment : groupIDAdjustmentMode;
                }
            }
            else
            {
                int[] gIDs = new int[10];
                AdjustmentMode[] adjModes = new AdjustmentMode[10];
                for (int i = 0; i < groupIDs.Length; i++)
                {
                    gIDs[i] = 0;
                    adjModes[i] = groupIDs[i] ? AdjustmentMode.FlatAdjustment : groupIDAdjustmentMode;
                }
                EffectSome.CopyPasteSettings[EffectSome.CopyPasteSettings.Count - 1] = new GeneralCopyPasteSettings
                {
                    X = x,
                    Y = y,
                    Hue1 = hue1,
                    Saturation1 = saturation1,
                    Brightness1 = brightness1,
                    Hue2 = hue2,
                    Saturation2 = saturation2,
                    Brightness2 = brightness2,
                    Scaling = scaling,
                    Rotation = rotation,
                    ZOrder = zOrder,
                    ZLayer = zLayer,
                    EL1 = el1,
                    EL2 = el2,
                    Color1IDs = color1IDs,
                    Color1IDValueCounter = 0,
                    Color1IDValueAdjustmentMode = color1IDAdjustmentMode,
                    Color2IDs = color2IDs,
                    Color2IDValueCounter = 0,
                    Color2IDValueAdjustmentMode = color2IDAdjustmentMode,
                    GroupIDs = values,
                    GroupIDValueCounters = gIDs,
                    GroupIDValueAdjustmentModes = adjModes,
                };
            }
        }
    }
}