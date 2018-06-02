using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EffectSome.EffectSome;
using static EffectSome.Memory;

namespace EffectSome
{
    public static class Editor
    {
        public static int[] ObjectArrayOffsets = { 0x168, 0x234, 0x20, 0x8, 0 };
        public static int[] ObjectAmountOffsets = { 0x168, 0x3A0 };

        public static List<int> GetCurrentlySelectedObjectIDs()
        {
            List<int> objIDs = new List<int>();
            List<int> result = new List<int>();
            // Getting pointer values
            int ObjectArrayAddress = GetAddressFromPointers(baseAddress, ObjectArrayOffsets);
            int ObjectAmountAddress = GetAddressFromPointers(baseAddress, ObjectAmountOffsets);
            if (ObjectArrayAddress == 0 || ObjectAmountAddress == 0) return result;

            unsafe
            {
                int total = GetIntFromPointers(0, ObjectAmountAddress);
                for (int i = 0; i < total; i++) //	for each object
                {
                    int address = GetIntFromPointers(0, ObjectArrayAddress + (i * 4));
                    if (GetBoolFromPointers(0, address + 0x3DA)) // if object selected
                    {
                        int ID = GetIntFromPointers(0, address + 0x360); // read object id
                        objIDs.Add(ID);
                    }
                }
            }
            for (int i = 0; i < objIDs.Count; i++)
                if (!result.Contains(objIDs[i]))
                    result.Add(objIDs[i]);
            return result;
        }
    }
}