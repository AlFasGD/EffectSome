using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace EffectSome
{
    public static class Memory
    {
        public static int GetAddressFromPointers(int baseAddress, params int[] offsets)
        {
            int value = BitConverter.ToInt32(MemoryEdit.ReadMemory(baseAddress, 4, (int)EffectSome.processHandle), 0);
            for (int i = 0; i < offsets.Length - 1; i++)
                value = BitConverter.ToInt32(MemoryEdit.ReadMemory(value + offsets[i], 4, (int)EffectSome.processHandle), 0);
            return value + offsets[offsets.Length - 1];
        }

        public static int GetIntFromPointers(int baseAddress, params int[] offsets)
        {
            int value = BitConverter.ToInt32(MemoryEdit.ReadMemory(baseAddress, 4, (int)EffectSome.processHandle), 0);
            for (int i = 0; i < offsets.Length; i++)
                value = BitConverter.ToInt32(MemoryEdit.ReadMemory(value + offsets[i], 4, (int)EffectSome.processHandle), 0);
            return value;
        }
        public static float GetFloatFromPointers(int baseAddress, params int[] offsets)
        {
            int value = BitConverter.ToInt32(MemoryEdit.ReadMemory(baseAddress, 4, (int)EffectSome.processHandle), 0);
            for (int i = 0; i < offsets.Length - 1; i++)
                value = BitConverter.ToInt32(MemoryEdit.ReadMemory(value + offsets[i], 4, (int)EffectSome.processHandle), 0);
            float result = BitConverter.ToSingle(MemoryEdit.ReadMemory(value + offsets[offsets.Length - 1], 4, (int)EffectSome.processHandle), 0);
            return result;
        }
        public static bool GetBoolFromPointers(int baseAddress, params int[] offsets)
        {
            int value = BitConverter.ToInt32(MemoryEdit.ReadMemory(baseAddress, 4, (int)EffectSome.processHandle), 0);
            for (int i = 0; i < offsets.Length - 1; i++)
                value = BitConverter.ToInt32(MemoryEdit.ReadMemory(value + offsets[i], 4, (int)EffectSome.processHandle), 0);
            bool result = BitConverter.ToBoolean(MemoryEdit.ReadMemory(value + offsets[offsets.Length - 1], 4, (int)EffectSome.processHandle), 0);
            return result;
        }

        public static void SetValueFromPointers(int baseAddress, byte[] bytes, params int[] offsets)
        {
            int address = BitConverter.ToInt32(MemoryEdit.ReadMemory(baseAddress, 4, (int)EffectSome.processHandle), 0);
            for (int i = 0; i < offsets.Length - 1; i++)
                address = BitConverter.ToInt32(MemoryEdit.ReadMemory(address + offsets[i], 4, (int)EffectSome.processHandle), 0);
            address += offsets[offsets.Length - 1];
            MemoryEdit.WriteMemory(address, bytes, (int)EffectSome.processHandle);
        }
        public static void SetIntFromPointers(int baseAddress, int value, params int[] offsets)
        {
            int address = BitConverter.ToInt32(MemoryEdit.ReadMemory(baseAddress, 4, (int)EffectSome.processHandle), 0);
            for (int i = 0; i < offsets.Length - 1; i++)
                address = BitConverter.ToInt32(MemoryEdit.ReadMemory(address + offsets[i], 4, (int)EffectSome.processHandle), 0);
            address += offsets[offsets.Length - 1];
            MemoryEdit.WriteMemory(address, BitConverter.GetBytes(value), (int)EffectSome.processHandle);
        }
        public static void SetFloatFromPointers(int baseAddress, float value, params int[] offsets)
        {
            int address = BitConverter.ToInt32(MemoryEdit.ReadMemory(baseAddress, 4, (int)EffectSome.processHandle), 0);
            for (int i = 0; i < offsets.Length - 1; i++)
                address = BitConverter.ToInt32(MemoryEdit.ReadMemory(address + offsets[i], 4, (int)EffectSome.processHandle), 0);
            address += offsets[offsets.Length - 1];
            MemoryEdit.WriteMemory(address, BitConverter.GetBytes(value), (int)EffectSome.processHandle);
        }

        public static void EditInt(int baseAddress, NumericUpDown NUD, params int[] offsets)
        {
            SetValueFromPointers(baseAddress, BitConverter.GetBytes(GetIntFromPointers(baseAddress, offsets) + (int)NUD.Value), offsets);
        }
        public static void EditFloat(int baseAddress, NumericUpDown NUD, params int[] offsets)
        {
            SetValueFromPointers(baseAddress, BitConverter.GetBytes(GetIntFromPointers(baseAddress, offsets) + (float)NUD.Value), offsets);
        }
    }
}