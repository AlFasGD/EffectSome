using EffectSome.Utilities.Enumerations.GeometryDash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectSome.Utilities.Functions.GeometryDash
{
    public static class Easing
    {
        public static int GetEasingValue(int easingType, bool enableIn, bool enableOut)
        {
            if (easingType > 0 && easingType < 7)
            {
                if (enableIn || enableOut)
                    return (easingType - 1) * 3 + 1 + (enableIn ? 0 : 2) + (enableOut ? 0 : 1);
                else
                    throw new ArgumentException("The easing in and out parameters were both false which is invalid.");
            }
            else if (easingType == 0)
                return 0;
            else
                throw new ArgumentException("The easing type value was beyond the easing type range.");
        }

        public static EasingType GetEasingType(int easingType, bool enableIn, bool enableOut)
        {
            if (easingType > 0 && easingType < 7)
            {
                if (enableIn || enableOut)
                    return (EasingType)(1 << (easingType + 3)) | (enableIn ? EasingType.In : EasingType.None) | (enableIn ? EasingType.In : EasingType.None);
                else
                    throw new ArgumentException("The easing in and out parameters were both false which is invalid.");
            }
            else if (easingType == 0)
                return EasingType.None;
            else
                throw new ArgumentException("The easing type value was beyond the easing type range.");
        }
    }
}