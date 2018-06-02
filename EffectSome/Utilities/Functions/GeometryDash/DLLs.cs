using System;
using System.Runtime.InteropServices;

namespace EffectSome
{
    public static class DLLs
    {
        public static class LimitBypasses
        {
            /// <summary>Bypasses the object limit by setting it to the maximum value. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassObjectLimit();
            /// <summary>Bypasses the object limit by setting it to a specified value. Returns true if the operation succeeded; otherwise false.</summary>
            /// <param name="newObjLim">The new value to set to the object limit.</param>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassObjectLimit(int newObjLim);
            /// <summary>Bypasses the custom object limit by setting it to the maximum value. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassCustomObjectLimit();
            /// <summary>Bypasses the custom object limit by setting it to a specified value. Returns true if the operation succeeded; otherwise false.</summary>
            /// <param name="newCustomObjLim">The new value to set to the custom object limit.</param>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassCustomObjectLimit(int newCustomObjLim);
            /// <summary>Bypasses the custom objects' object limit by setting it to the maximum value. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassCustomObjectsObjectLimit();
            /// <summary>Bypasses the custom objects' object limit by setting it to a specified value. Returns true if the operation succeeded; otherwise false.</summary>
            /// <param name="newCustomObjsObjLim">The new value to set to the custom objects' object limit.</param>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassCustomObjectsObjectLimit(int newCustomObjsObjLim);
            /// <summary>Bypasses the level editor buttons per row limit by setting it to values offering the biggest range. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassLevelEditorButtonsPerRowLimit();
            /// <summary>Bypasses the level editor buttons per row limit by setting the maximum and the minumum to specified values. Returns true if the operation succeeded; otherwise false.</summary>
            /// <param name="newMin">The new value to set to the minimum of the level editor buttons per row limit.</param>
            /// <param name="newMax">The new value to set to the maximum of the level editor buttons per row limit.</param>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassLevelEditorButtonsPerRowLimit(int newMin, int newMax);
            /// <summary>Bypasses the level editor button rows limit by setting it to values offering the biggest range. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassLevelEditorButtonRowsLimit();
            /// <summary>Bypasses the level editor button rows limit by setting the maximum and the minumum to specified values. Returns true if the operation succeeded; otherwise false.</summary>
            /// <param name="newMin">The new value to set to the minimum of the level editor button rows limit.</param>
            /// <param name="newMax">The new value to set to the maximum of the level editor button rows limit.</param>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassLevelEditorButtonRowsLimit(int newMin, int newMax);
            /// <summary>Bypasses the level editor zoom limit by setting it to values offering the biggest range. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassLevelEditorZoomLimit();
            /// <summary>Bypasses the level editor zoom limit by setting the maximum and the minumum to specified values. Returns true if the operation succeeded; otherwise false.</summary>
            /// <param name="newMin">The new value to set to the minimum of the level editor zoom limit.</param>
            /// <param name="newMax">The new value to set to the maximum of the level editor zoom limit.</param>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassLevelEditorZoomLimit(int newMin, int newMax);
            /// <summary>Bypasses the guidelines limit by setting it to the maximum value. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassGuidelinesLimit();
            /// <summary>Bypasses the guidelines limit by setting it to a specified value. Returns true if the operation succeeded; otherwise false.</summary>
            /// <param name="newGuidelinesLim">The new value to set to the guidelines limit.</param>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool BypassGuidelinesLimit(int newGuidelinesLim);
            /// <summary>Restores the object limit. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool RestoreObjectLimit();
            /// <summary>Restores the custom objects' limit. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool RestoreCustomObjectsLimit();
            /// <summary>Restores the custom objects' object limit. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool RestoreCustomObjectsObjectLimit();
            /// <summary>Restores the level editor buttons per row limit. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool RestoreLevelEditorButtonsPerRowLimit();
            /// <summary>Restores the level editor button rows limit. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool RestoreLevelEditorButtonRowsLimit();
            /// <summary>Restores the level editor zoom limit. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool RestoreLevelEditorZoomLimit();
            /// <summary>Restores the guidelines limit. Returns true if the operation succeeded; otherwise false.</summary>
            [DllImport("EffectSome/lib/Bypasses.dll")]
            public static extern bool RestoreGuidelinesLimit();
        }
        public static class Functions
        {
            /// <summary>Calls all the functions in the specified class</summary>
            /// <param name="T">The type of the class to call all the functions of.</param>
            [DllImport("EffectSome/lib/CallClassFunctions.dll", EntryPoint = "CallClassFunctions")]
            public static extern void CallAllMethodsInClass(Type T);
        }
        public static class ExtraLevelEditorFunctions
        {
            /// <summary>This function calls the in-game Create Base function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void CreateBaseInGame();
            /// <summary>This function calls the in-game Create Edges function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void CreateEdgesInGame();
            /// <summary>This function calls the in-game Create Outlines function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void CreateOutlinesInGame();
            /// <summary>This function calls the in-game Align X function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void AlignXInGame();
            /// <summary>This function calls the in-game Align Y function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void AlignYInGame();
            /// <summary>This function calls the in-game Select All function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void SelectAllInGame();
            /// <summary>This function calls the in-game Select All Left function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void SelectAllLeftInGame();
            /// <summary>This function calls the in-game Select All Right function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void SelectAllRightInGame();
            /// <summary>This function calls the in-game Unlock Layers function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void UnlockLayersInGame();
            /// <summary>This function calls the in-game Uncheck Portals function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void UncheckPortalsInGame();
            /// <summary>This function calls the in-game Reset Unused function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void ResetUnusedInGame();
            /// <summary>This function calls the external Create Base function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void CreateBaseExternal();
            /// <summary>This function calls the external Create Edges function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void CreateEdgesExternal();
            /// <summary>This function calls the external Create Outlines function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void CreateOutlinesExternal();
            /// <summary>This function calls the external Create Decoration Outlines function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void CreateDecorationOutlinesExternal();
            /// <summary>This function calls the external Create Cornerpieces function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void CreateCornerpiecesExternal();
            /// <summary>This function calls the external Reset Unused Color Channels function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void ResetUnusedColorChannelsExternal();
            /// <summary>This function calls the external Reset Unused Groups function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void ResetUnusedGroupsExternal();
            /// <summary>This function calls the external Uncheck Selected Portals function.</summary>
            [DllImport("EffectSome/lib/ExtraLevelEditorFunctions.dll")]
            public static extern void UncheckSelectedPortalsExternal();
        }
        public static class Misc
        {
            /// <summary>Inject the selected bytes.</summary>
            /// <param name="process">The process to inject.</param>
            /// <param name="address">The address in the <paramref name="process"/>.</param>
            /// <param name="bytes">The bytes to inject in the address in the process.</param>
            [DllImport("EffectSome/lib/inj.dll")]
            public static extern void Inject(IntPtr process, int address, byte[] bytes);
            /// <summary>Show a message box in the game.</summary>
            /// <param name="title">The title to show in the message box.</param>
            /// <param name="body">The body of the message box.</param>
            /// <param name="buttonText">The text in the button in the message box.</param>
            [DllImport("EffectSome/lib/mboxapi.dll")]
            public static extern void ShowGDMessageBox(string title, string body, string buttonText);
            /// <summary>Calls a function externally in the specified process.</summary>
            /// <param name="process">The process to call a function on.</param>
            /// <param name="funcAddress">The address of the function to call.</param>
            [DllImport("EffectSome/lib/extfuncall.dll")]
            public static extern void CallFunctionExternally(IntPtr process, int funcAddress);
        }
        public static class Editor
        {
            /// <summary>Returns an object containing all the currently selected objects.</summary>
            [DllImport("EffectSome/lib/editor.dll")]
            public static extern object GetCurrentlySelectedObjects(); // Maybe will require to change the returning data type.
            /// <summary>Returns the string representation of all the currently selected objects.</summary>
            [DllImport("EffectSome/lib/editor.dll")]
            public static extern string GetCurrentlySelectedObjectsString(); 
            /// <summary>Returns the number of the currently selected objects.</summary>
            [DllImport("EffectSome/lib/editor.dll")]
            public static extern int GetCurrentlySelectedObjectCount();
            /// <summary>Returns an array containing the unique Group IDs of the currently selected objects.</summary>
            [DllImport("EffectSome/lib/editor.dll")]
            public static extern int[] GetCurrentlySelectedObjectsGroupIDs();
            /// <summary>Returns an array containing the common Group IDs of the currently selected objects.</summary>
            [DllImport("EffectSome/lib/editor.dll")]
            public static extern int[] GetCurrentlySelectedObjectsCommonGroupIDs();
        }
    }
}