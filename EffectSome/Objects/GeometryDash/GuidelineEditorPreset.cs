using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectSome
{
    public class GuidelineEditorPreset
    {
        public string Name;
        public double BPM;
        public (int Beats, int Denominator) TimeSignature;
        public List<List<decimal>> Colors;

        public GuidelineEditorPreset()
        {

        }
        public GuidelineEditorPreset(string name, double bpm, (int, int) timeSignature, List<List<decimal>> colors)
        {
            Name = name;
            BPM = bpm;
            TimeSignature = timeSignature;
            Colors = colors;
        }

        public GuidelineEditorPreset Clone() => new GuidelineEditorPreset(Name, BPM, TimeSignature, Colors);
    }
}