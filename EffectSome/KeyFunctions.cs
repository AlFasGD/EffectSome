using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;

namespace EffectSome
{
    class KeyFunctions
    {
        public static string ConvertKeyToString(Keys k)
        {
            if (k == Keys.A)
                return "A";
            else if (k == Keys.B)
                return "B";
            else if (k == Keys.C)
                return "C";
            else if (k == Keys.D)
                return "D";
            else if (k == Keys.E)
                return "E";
            else if (k == Keys.F)
                return "F";
            else if (k == Keys.G)
                return "G";
            else if (k == Keys.H)
                return "H";
            else if (k == Keys.I)
                return "I";
            else if (k == Keys.J)
                return "J";
            else if (k == Keys.K)
                return "K";
            else if (k == Keys.L)
                return "L";
            else if (k == Keys.M)
                return "M";
            else if (k == Keys.N)
                return "N";
            else if (k == Keys.O)
                return "O";
            else if (k == Keys.P)
                return "P";
            else if (k == Keys.Q)
                return "Q";
            else if (k == Keys.R)
                return "R";
            else if (k == Keys.S)
                return "S";
            else if (k == Keys.T)
                return "T";
            else if (k == Keys.U)
                return "U";
            else if (k == Keys.V)
                return "V";
            else if (k == Keys.W)
                return "W";
            else if (k == Keys.X)
                return "X";
            else if (k == Keys.Y)
                return "Y";
            else if (k == Keys.Z)
                return "Z";
            else if (k == Keys.D0 || k == Keys.NumPad0)
                return "0";
            else if (k == Keys.D1 || k == Keys.NumPad1)
                return "1";
            else if (k == Keys.D2 || k == Keys.NumPad2)
                return "2";
            else if (k == Keys.D3 || k == Keys.NumPad3)
                return "3";
            else if (k == Keys.D4 || k == Keys.NumPad4)
                return "4";
            else if (k == Keys.D5 || k == Keys.NumPad5)
                return "5";
            else if (k == Keys.D6 || k == Keys.NumPad6)
                return "6";
            else if (k == Keys.D7 || k == Keys.NumPad7)
                return "7";
            else if (k == Keys.D8 || k == Keys.NumPad8)
                return "8";
            else if (k == Keys.D9 || k == Keys.NumPad9)
                return "9";
            else if (k == Keys.F1)
                return "F1";
            else if (k == Keys.F2)
                return "F2";
            else if (k == Keys.F3)
                return "F3";
            else if (k == Keys.F4)
                return "F4";
            else if (k == Keys.F5)
                return "F5";
            else if (k == Keys.F6)
                return "F6";
            else if (k == Keys.F7)
                return "F7";
            else if (k == Keys.F8)
                return "F8";
            else if (k == Keys.F9)
                return "F9";
            else if (k == Keys.F10)
                return "F10";
            else if (k == Keys.F11)
                return "F11";
            else if (k == Keys.F12)
                return "F12";
            else if (k == Keys.F13)
                return "F13";
            else if (k == Keys.F14)
                return "F14";
            else if (k == Keys.F15)
                return "F15";
            else if (k == Keys.F16)
                return "F16";
            else if (k == Keys.F17)
                return "F17";
            else if (k == Keys.F18)
                return "F18";
            else if (k == Keys.F19)
                return "F19";
            else if (k == Keys.F20)
                return "F20";
            else if (k == Keys.F21)
                return "F21";
            else if (k == Keys.F22)
                return "F22";
            else if (k == Keys.F23)
                return "F23";
            else if (k == Keys.F24)
                return "F24";
            else if (k == Keys.Home)
                return "Home";
            else if (k == Keys.Enter)
                return "Enter";
            else if (k == Keys.Shift)
                return "Shift";
            else if (k == Keys.Delete)
                return "Delete";
            else if (k == Keys.PageUp)
                return "Page Up";
            else if (k == Keys.PageDown)
                return "Page Down";
            else if (k == Keys.End)
                return "End";
            else if (k == Keys.Tab)
                return "Tab";
            else if (k == Keys.Up)
                return "Up";
            else if (k == Keys.Down)
                return "Down";
            else if (k == Keys.Left)
                return "Left";
            else if (k == Keys.Right)
                return "Right";
            else if (k == Keys.CapsLock)
                return "Caps Lock";
            else if (k == Keys.Add)
                return "+";
            else if (k == Keys.Insert)
                return "Insert";
            else
                return "Unrecognised key";
            // Add more keys
        }
        public static string ConvertKeyCodeToString(string keyCode)
        {
            string[] Pages = { "PageDown", "PageUp" };
            string[] DNum = { "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9" };
            string[] NumPadNum = { "NumPad0", "NumPad1", "NumPad2", "NumPad3", "NumPad4", "NumPad5", "NumPad6", "NumPad7", "NumPad8", "NumPad9" };
            string[,] OEMs =
            {
                { "OemBackslash", "OemCloseBrackets", "Oemcomma", "OemMinus", "OemOpenBrackets", "OemPeriod", "Oemplus", "OemQuestion", "OemQuotes", "OemSemicolon", "Oemtilde" },
                { "\\", "}", ",", "-", "{", ".", "+", "?", "\"", ":", "~" }
            };
            string[,] MathematicalOperators =
            {
                { "Add", "Subtract", "Multiply", "Divide" },
                { "+", "-", "*", "/" }
            };
            for (int i = 0; i < DNum.Length; i++)
                if (DNum[i] == keyCode)
                    return keyCode.Remove(0, 1);
            for (int i = 0; i < NumPadNum.Length; i++)
                if (NumPadNum[i] == keyCode)
                    return keyCode.Remove(0, 6);
            for (int i = 0; i < OEMs.GetLength(0); i++)
                if (OEMs[0, i] == keyCode)
                    return OEMs[1, i];
            for (int i = 0; i < MathematicalOperators.GetLength(0); i++)
                if (MathematicalOperators[0, i] == keyCode)
                    return MathematicalOperators[1, i];
            for (int i = 0; i < Pages.Length; i++)
                if (Pages[i] == keyCode)
                    return keyCode.Insert(4, " ");
            if (keyCode == "Next")
                return "Page Down";
            return keyCode;
        }
    }
}
