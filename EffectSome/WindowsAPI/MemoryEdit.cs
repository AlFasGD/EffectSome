using System;
using System.Runtime.InteropServices;

namespace EffectSome
{
    public static class MemoryEdit
    {
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, ref uint lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, ref uint lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("psapi.dll")]
        unsafe public static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule, char* lpBaseName, uint nSize);

        [DllImport("psapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int EnumProcessModules(IntPtr hProcess, [Out] IntPtr lphModule, uint cb, out uint lpcbNeeded);

        #region Process Memory Permissions
        public const int PROCESS_CREATE_THREAD = 0x0002;
        public const int PROCESS_QUERY_INFORMATION = 0x0400;
        public const int PROCESS_VM_OPERATION = 0x0008;
        public const int PROCESS_VM_WRITE = 0x0020;
        public const int PROCESS_VM_READ = 0x0010;
        public const int PROCESS_WM_READ = 0x0010;
        public const int MEM_RELEASE = 0x800;
        public const int MEM_COMMIT = 0x00001000;
        public const int MEM_RESERVE = 0x00002000;
        public const int PAGE_READWRITE = 4;
        public const int PROCESS_ALL_ACCESS = PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ;
        #endregion

        public static byte[] ReadMemory(int address, int processSize, int processHandle)
        {
            byte[] buffer = new byte[processSize];
            uint shit = 0;
            ReadProcessMemory(new IntPtr(processHandle), new IntPtr(address), buffer, (uint)processSize, ref shit);
            return buffer;
        }
        public static void WriteMemory(int address, byte[] processBytes, int processHandle)
        {
            uint shit = 0;
            ChangeMemoryProtection(address, (uint)processBytes.Length, processHandle);
            WriteProcessMemory(processHandle, address, processBytes, processBytes.Length, ref shit);
        }
        public static void ChangeMemoryProtection(int address, uint size, int processHandle)
        {
            VirtualProtectEx(new IntPtr(processHandle), new IntPtr(address), new UIntPtr(size), PAGE_READWRITE, out uint shit);
        }
    }
}