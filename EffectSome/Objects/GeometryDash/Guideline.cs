using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectSome
{
    public class Guideline
    {
        public double TimeStamp { get; set; }
        public double Color { get; set; }

        public Guideline() { }
        public Guideline(double timeStamp, double color)
        {
            TimeStamp = timeStamp;
            Color = color;
        }
        public Guideline(int timeStamp, int color)
        {
            TimeStamp = timeStamp;
            Color = color;
        }
        public Guideline(float timeStamp, float color)
        {
            TimeStamp = timeStamp;
            Color = color;
        }
        public Guideline(decimal timeStamp, decimal color)
        {
            TimeStamp = (double)timeStamp;
            Color = (double)color;
        }

        /// <summary>Converts the <see cref="Guideline"/> to its string representation in the gamesave.</summary>
        public override string ToString() => TimeStamp + "~" + Color;
    }
}