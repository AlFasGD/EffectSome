using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EffectSome.Objects.CopyPasteSettings;
using static EffectSome.EffectSome;
using static EffectSome.Gamesave;
using static EffectSome.Utilities.Functions.General.UsefulFunctions;
using static System.IO.File;
using Newtonsoft.Json;

namespace EffectSome.Utilities.Functions.GeometryDash.CopyPaste
{
    public static class CopyPasteSettingsWritingFunctions
    {
        public static readonly string CPSettingsPath = $@"{GDLocalData}\tmp\cpa\settings.json";

        public static void WriteObjectCopyPasteAutomationSettings()
        {
            WriteAllText(CPSettingsPath, JsonConvert.SerializeObject(CopyPasteSettings, Formatting.Indented));
        }
    }
}