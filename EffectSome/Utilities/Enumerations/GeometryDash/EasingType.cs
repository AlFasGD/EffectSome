using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectSome.Utilities.Enumerations.GeometryDash
{
    public enum EasingType
    {
        None = 0,
        Ease = 1 << 4,
        Elastic = 1 << 5,
        Bounce = 1 << 6,
        Exponential = 1 << 7,
        Sine = 1 << 8,
        Back = 1 << 9,

        // Easing Modifiers
        In = 1,
        Out = 2,
        InOut = In | Out,
    }
}