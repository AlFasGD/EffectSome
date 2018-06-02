using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectSome
{
    public class CustomLevelObject
    {
        public List<LevelObject> LevelObjects;

        public CustomLevelObject(List<LevelObject> levelObjects)
        {
            // Probably no need to clone the entire thing as they are not managed anywhere else after being read
            LevelObjects = levelObjects.Clone();
            if (LevelObjects.Count > 0)
            {
                double avgX = 0;
                double avgY = 0;
                for (int i = 0; i < LevelObjects.Count; i++)
                {
                    avgX += (double)LevelObjects[i][LevelObject.ObjectParameter.X];
                    avgY += (double)LevelObjects[i][LevelObject.ObjectParameter.Y];
                }
                avgX /= LevelObjects.Count;
                avgY /= LevelObjects.Count;
                for (int i = 0; i < LevelObjects.Count; i++)
                {
                    LevelObjects[i][LevelObject.ObjectParameter.X] = (double)LevelObjects[i][LevelObject.ObjectParameter.X] - avgX;
                    LevelObjects[i][LevelObject.ObjectParameter.Y] = (double)LevelObjects[i][LevelObject.ObjectParameter.Y] - avgY;
                }
            }
        }
    }
}