using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EffectSome
{
    public static class Extensions
    {
        public static int Find(this string s, string match)
        {
            for (int i = 0; i <= s.Length - match.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < match.Length && found; j++)
                    if (s[i + j] != match[j])
                        found = false;
                if (found)
                    return i;
            }
            return -1;
        }
        public static int Find(this string s, string match, int occurence)
        {
            int occurences = 0;
            for (int i = 0; i <= s.Length - match.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < match.Length && found; j++)
                    if (s[i + j] != match[j])
                        found = false;
                if (found)
                {
                    occurences++;
                    if (occurences == occurence)
                        return i;
                }
            }
            return -1;
        }
        public static int Find(this string s, string match, int start, int end)
        {
            for (int i = start; i <= end - match.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < match.Length && found; j++)
                    if (s[i + j] != match[j])
                        found = false;
                if (found)
                    return i;
            }
            return -1;
        }
        public static int FindFromEnd(this string s, string match)
        {
            for (int i = s.Length - match.Length; i >= 0; i--)
            {
                bool found = true;
                for (int j = 0; j < match.Length && found; j++)
                    if (s[i + j] != match[j])
                        found = false;
                if (found)
                    return i;
            }
            return -1;
        }
        public static int FindFromEnd(this string s, string match, int occurence)
        {
            int occurences = 0;
            for (int i = s.Length - match.Length; i >= 0; i--)
            {
                bool found = true;
                for (int j = 0; j < match.Length && found; j++)
                    if (s[i + j] != match[j])
                        found = false;
                if (found)
                {
                    occurences++;
                    if (occurences == occurence)
                        return i;
                }
            }
            return -1;
        }
        public static int FindFromEnd(this string s, string match, int start, int end)
        {
            for (int i = s.Length - match.Length; i >= 0; i--)
            {
                bool found = true;
                for (int j = 0; j < match.Length && found; j++)
                    if (s[i + j] != match[j])
                        found = false;
                if (found)
                    return i;
            }
            return -1;
        }
        public static int GetLastNumber(this string s)
        {
            int n = 0;
            bool isLastNumber = false;
            for (int i = 0; i < s.Length; i++)
            {
                isLastNumber = int.TryParse(s.Substring(i), out n);
                if (isLastNumber)
                    return n;
            }
            throw new ArgumentException("The string has no number in the end.");
        }
        public static int[] FindAll(this string s, string match)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i <= s.Length - match.Length; i++)
            {
                string sub = s.Substring(i, match.Length);
                if (sub == match)
                    indices.Add(i);
            }
            return indices.ToArray();
        }
        public static int[] FindAll(this string s, string match, int start, int end)
        {
            List<int> indices = new List<int>();
            for (int i = start; i <= end - match.Length; i++)
            {
                string sub = s.Substring(i, match.Length);
                if (sub == match)
                    indices.Add(i);
            }
            return indices.ToArray();
        }
        public static string FixBase64String(this string s)
        {
            int lastNumber = 0;
            int lastInvalidCharacter = 0;
            bool continueChecking = true;
            while (continueChecking)
            {
                if (s[s.Length - lastNumber - 1].IsNumber())
                    lastNumber++;
                else if (!s[s.Length - lastNumber - 1].IsBase64Character())
                    lastInvalidCharacter = ++lastNumber;
                else
                    continueChecking = false;
            }
            s = s.Substring(0, s.Length - lastInvalidCharacter);
            while (s.Length % 4 != 0)
                s += "=";
            return s;
        }
        public static string Substring(this string s, string from, string to)
        {
            int startIndex = s.Find(from) + from.Length;
            int endIndex = s.Find(to);
            int length = endIndex - startIndex;
            return s.Substring(startIndex, length);
        }
        public static string RemoveLastNumber(this string s)
        {
            string l = s;
            for (int i = 0; i < s.Length; i++)
            {
                l = s.Substring(i);
                if (int.TryParse(l, out int shit))
                    return s.Substring(0, i);
            }
            return s;
        }
        public static string Replace(this string originalString, string stringToReplaceWith, int startIndex, int length)
        {
            string result = originalString;
            result = result.Remove(startIndex, length);
            result = result.Insert(startIndex, stringToReplaceWith);
            return result;
        }
        public static string ReplaceWholeWord(this string originalString, string oldString, string newString)
        {
            for (int i = originalString.Length - oldString.Length; i >= 0; i--)
            {
                if (originalString.Substring(i, oldString.Length) == oldString)
                    if ((i > 0 ? (!originalString[i - 1].IsLetterOrNumber()) : true) && (i < originalString.Length - oldString.Length ? (!originalString[i + oldString.Length].IsLetterOrNumber()) : true))
                    {
                        originalString = originalString.Replace(newString, i, oldString.Length);
                        i -= oldString.Length;
                    }
            }
            return originalString;
        }
        public static string[] Split(this string s, string separator)
        {
            int[] occurences = s.FindAll(separator);
            string[] result = new string[occurences.Length + 1];
            for (int i = 0; i < result.Length; i++)
            {
                int startIndex = i == 0 ? 0 : occurences[i - 1] + separator.Length;
                int endIndex = i >= occurences.Length ? s.Length : occurences[i];
                result[i] = s.Substring(startIndex, endIndex - startIndex);
            }
            return result;
        }
        public static bool ContainsWholeWord(this string s, string match)
        {
            for (int i = s.Length - match.Length; i >= 0; i--)
            {
                if (s.Substring(i, match.Length) == match)
                    if ((i > 0 ? (!s[i - 1].IsLetterOrNumber()) : true) && (i < s.Length - match.Length ? (!s[i + match.Length].IsLetterOrNumber()) : true))
                        return true;
            }
            return false;
        }
        public static bool MatchesStringCaseFree(this string s, string match)
        {
            if (s.Length != match.Length)
                return false;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].IsUpperCaseLetter())
                    if (s[i] + 32 != match[i] && s[i] != match[i])
                        return false;
                else if (s[i].IsLowerCaseLetter())
                    if (s[i] - 32 != match[i] && s[i] != match[i])
                        return false;
            }
            return true;
        }
        public static bool IsBase64Character(this char c)
        {
            return IsNumber(c) || IsLetter(c) || c == '/' || c == '+' || c == '=';
        }
        public static bool IsLetter(this char c) => (c >= 65 && c <= 90 || c >= 97 && c <= 122);
        public static bool IsLowerCaseLetter(this char c) => (c >= 97 && c <= 122);
        public static bool IsNumber(this char c) => (c >= 48 && c <= 57);
        public static bool IsUpperCaseLetter(this char c) => (c >= 65 && c <= 90);
        public static bool IsLetterOrNumber(this char c) => IsLetter(c) || IsNumber(c);

        public static int[] FindOccurences(this string[] a, string match)
        {
            if (a != null)
            {
                List<int> occurences = new List<int>();
                for (int i = 0; i < a.Length; i++)
                    if (a[i] == match)
                        occurences.Add(i);
                return occurences.ToArray();
            }
            else return new int[0];
        }
        public static string[] Replace(this string[] a, char oldChar, char newChar)
        {
            for (int i = 0; i < a.Length; i++)
                a[i] = a[i].Replace(oldChar, newChar);
            return a;
        }
        public static string[] Replace(this string[] a, string oldString, string newString)
        {
            for (int i = 0; i < a.Length; i++)
                a[i] = a[i].Replace(oldString, newString);
            return a;
        }
        public static string[] ReplaceWholeWord(this string[] a, string oldString, string newString)
        {
            for (int i = 0; i < a.Length; i++)
                a[i] = a[i].ReplaceWholeWord(oldString, newString);
            return a;
        }
        public static string[] RemoveEmptyElements(this string[] a)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < a.Length; i++)
                if (a[i].Length > 0)
                    result.Add(a[i]);
            return result.ToArray();
        }
        public static bool ContainsAtLeast(this string[] a, string match)
        {
            if (a != null)
            {
                for (int i = 0; i < a.Length; i++)
                    if (a[i].Contains(match))
                        return true;
                return false;
            }
            return false;
        }
        public static bool ContainsAtLeastWholeWord(this string[] a, string match)
        {
            if (a != null)
            {
                for (int i = 0; i < a.Length; i++)
                    if (a[i].ContainsWholeWord(match))
                        return true;
                return false;
            }
            return false;
        }
        public static string Combine(this string[] s)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
                str = str.Append(s[i]);
            return str.ToString();
        }
        public static string Combine(this string[] s, string separator)
        {
            if (s.Length > 0)
            {
                StringBuilder str = new StringBuilder();
                str = str.Append(s[0]);
                for (int i = 1; i < s.Length; i++)
                    str = str.Append(separator + s[i]);
                return str.ToString();
            }
            else return "";
        }
        public static string[,] Split(this string[] s, char separator)
        {
            List<string[]> separated = new List<string[]>();
            for (int i = 0; i < s.Length; i++)
                separated.Add(s[i].Split(separator));
            return separated.ToTwoDimensionalArray();
        }

        public static string ShowValuesWithRanges(this int[] s)
        {
            s = s.Sort(); // Sort the values
            string result = "";
            if (s.Length > 0)
            {
                int lastShownValue = s[0];
                int lastValueInCombo = s[0];
                result += lastShownValue.ToString();
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] > lastValueInCombo) // Determines whether the next value is not in a row
                    {
                        result += (result[result.Length - 1] == '-' ? s[i - 1].ToString() : "") + ", " + s[i].ToString();
                        lastValueInCombo = lastShownValue = s[i] + 1;
                    }
                    else if (s[i] == lastValueInCombo) // Determines whether the next value is in a row
                    {
                        if (i < s.Length - 1) // Determines whether the current index is not the last value
                        {
                            if (lastShownValue == lastValueInCombo) // Determines whether this is the start of a new combo
                                result += "-";
                        }
                        else if (i == s.Length - 1) // Determines whether the current index is the last value
                            result += s[i].ToString();
                        lastValueInCombo = s[i] + 1; // Set the last index in the combo to the current index
                    }
                }
            }
            return result;
        }

        public static bool Contains<T>(this T[] a, T item)
        {
            for (int i = 0; i < a.Length; i++)
            {
                try
                {
                    var x = Expression.Constant(a[i], typeof(T));
                    var y = Expression.Constant(item, typeof(T));
                    Expression shit = Expression.Convert(Expression.IsTrue(Expression.Equal(x, y)), typeof(bool));
                    if (Expression.Lambda<Func<bool>>(shit).Compile()())
                        return true;
                }
                catch { if (a[i].Equals(item)) return true; }
            }
            return false;
        }
        public static bool Contains(this int[] a, int item)
        {
            for (int i = 0; i < a.Length; i++)
                if (a[i] == item)
                    return true;
            return false;
        }
        public static bool Contains(this Enum e, int value)
        {
            return ((int[])(Enum.GetValues(e.GetType()))).Contains(value);
        }

        public static bool MatchIndices(this List<int> l)
        {
            for (int i = 0; i < l.Count; i++)
                if (l[i] != i)
                    return false;
            return true;
        }
        public static bool MatchIndicesFromEnd(this List<int> l, int length)
        {
            for (int i = l.Count - 1; i >= 0; i--)
                if (l[i] != length - l.Count + i)
                    return false;
            return true;
        }
        public static int FindIndexInList(this List<int> l, int match)
        {
            for (int i = 0; i < l.Count; i++)
                if (l[i] == match)
                    return i;
            return -1;
        }
        public static List<int> BubbleSortList(this List<int> list)
        {
            int temp = 0;
            List<int> l = list;
            for (int i = 0; i < l.Count - 1; i++)
                for (int j = 0; j < l.Count - i - 1; j++)
                    if (l[j] > l[j + 1])
                    {
                        temp = l[j + 1];
                        l[j + 1] = l[j];
                        l[j] = temp;
                    }
            return l;
        }
        public static List<int> RemoveDuplicates(this List<int> l)
        {
            List<int> newList = new List<int>();
            for (int i = 0; i < l.Count; i++)
                if (!newList.Contains(l[i]))
                    newList.Add(l[i]);
            return newList;
        }
        public static List<int> RemoveNegatives(this List<int> l)
        {
            List<int> newList = new List<int>();
            for (int i = 0; i < l.Count; i++)
                if (l[i] >= 0)
                    newList.Add(l[i]);
            return newList;
        }
        public static int Max(this List<int> l)
        {
            int max = int.MinValue;
            for (int i = 0; i < l.Count; i++)
                if (l[i] > max)
                    max = l[i];
            return max;
        }
        public static int Min(this List<int> l)
        {
            int min = int.MaxValue;
            for (int i = 0; i < l.Count; i++)
                if (l[i] < min)
                    min = l[i];
            return min;
        }
        public static int FindOccurences(this List<int> l, int match)
        {
            int result = 0;
            for (int i = 0; i < l.Count; i++)
                if (l[i] == match)
                    result++;
            return result;
        }

        public static double Max(this List<double> l)
        {
            double max = double.MinValue;
            for (int i = 0; i < l.Count; i++)
                if (l[i] > max)
                    max = l[i];
            return max;
        }
        public static double Min(this List<double> l)
        {
            double min = double.MaxValue;
            for (int i = 0; i < l.Count; i++)
                if (l[i] < min)
                    min = l[i];
            return min;
        }

        public static List<List<decimal>> Copy(this List<List<decimal>> l)
        {
            List<List<decimal>> result = new List<List<decimal>>();
            for (int i = 0; i < l.Count; i++)
            {
                List<decimal> newItem = new List<decimal>();
                for (int j = 0; j < l[i].Count; j++)
                    newItem.Add(l[i][j]);
                result.Add(newItem);
            }
            return result;
        }

        public static List<Guideline> Add(this List<Guideline> l, double timeStamp, double color)
        {
            Guideline g = new Guideline(timeStamp, color);
            l.Add(g);
            return l;
        }
        public static List<Guideline> Insert(this List<Guideline> l, int index, double timeStamp, double color)
        {
            Guideline g = new Guideline(timeStamp, color);
            l.Insert(index, g);
            return l;
        }
        public static int FindIndexToInsertGuideline(this List<Guideline> l, double timeStamp)
        {
            if (l.Count == 0)
                return 0;
            for (int i = 0; i < l.Count; i++)
                if (l[i].TimeStamp > timeStamp)
                    return i;
            return l.Count;
        }

        public static List<GlobalParameterSettingsPreset> SortPresetList(this List<GlobalParameterSettingsPreset> l)
        {
            List<GlobalParameterSettingsPreset> result = new List<GlobalParameterSettingsPreset>();
            if (l.Count > 0)
            {
                List<string> sortedPresetNames = new List<string>();
                for (int i = 0; i < l.Count; i++)
                    sortedPresetNames.Add(l[i].PresetName);
                sortedPresetNames.Sort();
                for (int i = 0; i < sortedPresetNames.Count; i++)
                    for (int j = 0; j < l.Count; j++)
                        if (sortedPresetNames[i] == l[j].PresetName)
                        {
                            result.Add(l[j]);
                            break;
                        }
            }
            return result;
        }
        public static List<GlobalParameterSettingsPreset> DeletePreset(this List<GlobalParameterSettingsPreset> l, string presetName)
        {
            for (int i = 0; i < l.Count; i++)
                if (l[i].PresetName == presetName)
                    l.RemoveAt(i);
            return l;
        }
        public static List<GlobalParameterSettingsPreset> RenamePreset(this List<GlobalParameterSettingsPreset> l, string oldPresetName, string newPresetName)
        {
            for (int i = 0; i < l.Count; i++)
                if (l[i].PresetName == oldPresetName)
                    l[i].PresetName = newPresetName;
            return l;
        }
        public static int FindPresetIndex(this List<GlobalParameterSettingsPreset> l, string presetName)
        {
            for (int i = 0; i < l.Count; i++)
                if (l[i].PresetName == presetName)
                    return i;
            return -1;
        }

        public static int[] GetIndexOfDataGridViewCell(this DataGridView d, DataGridViewCell cell)
        {
            for (int i = 0; i < d.ColumnCount; i++)
                for (int j = 0; j < d.RowCount; j++)
                    if (d[i, j] == cell)
                        return new int[] { i, j };
            return null;
        }
        public static int GetSelectedIndex(this DataGridViewComboBoxCell d)
        {
            for (int i = 0; i < d.Items.Count; i++)
                if (d.Value.ToString() == d.Items[i].ToString())
                    return i;
            return -1;
        }

        public static int OneOrGreater(this double d)
        {
            int i = (int)d;
            if (d < 1)
                i = 1;
            return i;
        }
        public static int OneOrGreater(this int a)
        {
            int i = a;
            if (a < 1)
                i = 1;
            return i;
        }
        public static int ZeroOrGreater(this int a)
        {
            int i = a;
            if (a < 0)
                i = 0;
            return i;
        }
        public static int WithinBounds(this int i, int min, int max)
        {
            if (i < min) return min;
            else if (i > max) return max;
            return i;
        }

        public static T[] GetInnerArray<T>(this T[,] ar, int innerArrayIndex)
        {
            T[] innerAr = new T[ar.GetLength(1)];
            for (int i = 0; i < innerAr.Length; i++)
                innerAr[i] = ar[innerArrayIndex, i];
            return innerAr;
        }
        public static int[] GetLengths<T>(this List<T[]> l)
        {
            int[] lengths = new int[l.Count];
            for (int i = 0; i < l.Count; i++)
                lengths[i] = l[i].Length;
            return lengths;
        }
        public static int[] GetLengths<T>(this T[,] ar)
        {
            int[] lengths = new int[ar.Length];
            for (int i = 0; i < ar.Length; i++)
                lengths[i] = ar.GetInnerArray(i).Length;
            return lengths;
        }

        public static T[] Append<T>(this T[] a, T item)
        {
            if (a != null)
            {
                T[] result = new T[a.Length + 1];
                for (int i = 0; i < a.Length; i++)
                    result[i] = a[i];
                result[a.Length] = item;
                return result;
            }
            else return new T[] { item };
        }
        public static T[] CopyArray<T>(this T[] a)
        {
            T[] result = new T[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i];
            return result;
        }
        public static T[] ExcludeFirst<T>(this T[] a, int excludedItemsCount)
        {
            if (excludedItemsCount <= a.Length)
            {
                T[] ar = new T[a.Length - excludedItemsCount];
                for (int i = excludedItemsCount; i < a.Length; i++)
                    ar[i - excludedItemsCount] = a[i];
                return ar;
            }
            else
                throw new ArgumentException("The excluded items must be less or equal to the size of the array.", "excludedItemsCount");
        }
        public static T[] ExcludeLast<T>(this T[] a, int excludedItemsCount)
        {
            if (excludedItemsCount <= a.Length)
            {
                T[] ar = new T[a.Length - excludedItemsCount];
                for (int i = 0; i < ar.Length; i++)
                    ar[i] = a[i];
                return ar;
            }
            else
                throw new ArgumentException("The excluded items must be less or equal to the size of the array.", "excludedItemsCount");
        }
        public static T[] InsertAt<T>(this T[] a, int index, T item)
        {
            if (a != null)
            {
                T[] result = new T[a.Length + 1];
                result[index] = item;
                for (int i = 0; i < index; i++)
                    result[i] = a[i];
                for (int i = index; i < a.Length; i++)
                    result[i + 1] = a[i];
                return result;
            }
            else return new T[] { item };
        }
        public static T[] InsertAtStart<T>(this T[] a, T item)
        {
            if (a != null)
            {
                T[] result = new T[a.Length + 1];
                result[0] = item;
                for (int i = 0; i < a.Length; i++)
                    result[i + 1] = a[i];
                return result;
            }
            else return new T[] { item };
        }
        public static T[] MoveElement<T>(this T[] a, int from, int to)
        {
            T[] result = new T[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i];
            result = result.RemoveAt(from);
            result = result.InsertAt(to, a[from]);
            return result;
        }
        public static T[] MoveElementToEnd<T>(this T[] a, int from)
        {
            T[] result = new T[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i];
            result = result.RemoveAt(from);
            result = result.InsertAt(result.Length, a[from]);
            return result;
        }
        public static T[] MoveElementToStart<T>(this T[] a, int from)
        {
            T[] result = new T[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i];
            result = result.RemoveAt(from);
            result = result.InsertAt(0, a[from]);
            return result;
        }
        public static T[] RemoveAt<T>(this T[] a, int index)
        {
            T[] result = new T[a.Length - 1];
            for (int i = 0; i < index; i++)
                result[i] = a[i];
            for (int i = index + 1; i < a.Length; i++)
                result[i - 1] = a[i];
            return result;
        }
        public static T[] RemoveDuplicates<T>(this T[] a)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < a.Length; i++)
                if (!result.Contains(a[i]))
                    result.Add(a[i]);
            return result.ToArray();
        }
        public static T[] Reverse<T>(this T[] a)
        {
            T[] result = new T[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[a.Length - i - 1] = a[i];
            return result;
        }
        public static T[] Sort<T>(this T[] ar)
        {
            List<T> sorted = ar.ToList();
            sorted.Sort();
            return sorted.ToArray();
        }
        public static T[] Swap<T>(this T[] ar, int a, int b)
        {
            T[] result = CopyArray(ar);
            T t = ar[a];
            ar[a] = ar[b];
            ar[b] = t;
            return result;
        }
        public static List<T> InsertAtStart<T>(this List<T> a, T item)
        {
            if (a != null)
            {
                a.Insert(0, item);
                return a;
            }
            else return new List<T> { item };
        }
        public static List<T> MoveElement<T>(this List<T> a, int from, int to)
        {
            a.Insert(to, a[from]);
            a.RemoveAt(from + (from > to ? 1 : 0));
            return a;
        }
        public static List<T> MoveElementToEnd<T>(this List<T> a, int from)
        {
            a.Insert(a.Count, a[from]);
            a.RemoveAt(from);
            return a;
        }
        public static List<T> MoveElementToStart<T>(this List<T> a, int from)
        {
            a.Insert(0, a[from]);
            a.RemoveAt(from + 1);
            return a;
        }
        public static List<T> Swap<T>(this List<T> l, int a, int b)
        {
            T t = l[a];
            l[a] = l[b];
            l[b] = t;
            return l;
        }

        public static List<T> Clone<T>(this List<T> l)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < l.Count; i++)
                result.Add(l[i]);
            return result;
        }
        public static Pair<T1, T2> Clone<T1, T2>(this Pair<T1, T2> p)
        {
            return new Pair<T1, T2>(p.Item1, p.Item2);
        }
        public static Pair<List<T1>, T2> Clone<T1, T2>(this Pair<List<T1>, T2> p)
        {
            Pair<List<T1>, T2> result = new Pair<List<T1>, T2>();
            result.Item1 = new List<T1>();
            for (int i = 0; i < p.Item1.Count; i++)
                result.Item1.Add(p.Item1[i]);
            result.Item2 = p.Item2;
            return result;
        }
        public static Pair<List<T1>, List<T2>> Clone<T1, T2>(this Pair<List<T1>, List<T2>> p)
        {
            Pair<List<T1>, List<T2>> result = new Pair<List<T1>, List<T2>>();
            result.Item1 = new List<T1>();
            for (int i = 0; i < p.Item1.Count; i++)
                result.Item1.Add(p.Item1[i]);
            result.Item2 = new List<T2>();
            for (int i = 0; i < p.Item2.Count; i++)
                result.Item2.Add(p.Item2[i]);
            return result;
        }
        public static bool ContainsAll<T>(this List<T> list, List<T> containedList)
        {
            if (containedList.Count != list.Count)
                return false;
            List<T> tempList = list.Clone();
            List<T> tempContained = containedList.Clone();
            for (int i = 0; i < tempContained.Count; i++)
                tempList.Remove(tempContained[i]);
            return tempList.Count == 0;
        }
        public static bool ContainsOrdered(this List<bool> list, List<bool> containedList)
        {
            for (int i = 0; i < list.Count - containedList.Count; i++)
            {
                bool found = true;
                for (int j = 0; j < containedList.Count && found; j++)
                    found = list[i + j] == containedList[j];
                if (found) return true;
            }
            return false;
        }
        public static bool ContainsOrdered(this List<int> list, List<int> containedList)
        {
            for (int i = 0; i < list.Count - containedList.Count; i++)
            {
                bool found = true;
                for (int j = 0; j < containedList.Count && found; j++)
                    found = list[i + j] == containedList[j];
                if (found) return true;
            }
            return false;
        }
        public static bool ContainsOrdered(this List<float> list, List<float> containedList)
        {
            for (int i = 0; i < list.Count - containedList.Count; i++)
            {
                bool found = true;
                for (int j = 0; j < containedList.Count && found; j++)
                    found = list[i + j] == containedList[j];
                if (found) return true;
            }
            return false;
        }
        // Probably O(n) complexity? Or is it O(n^2) because it's looping on an O(n) complexity function? I don't know how this works, someone teach me
        // This is probably O(n^2) complexity, my past self
        public static bool ContainsUnordered<T>(this List<T> list, List<T> containedList)
        {
            List<T> tempList = containedList.Clone();
            List<T> tempContained = containedList.Clone();
            for (int i = 0; i < tempList.Count; i++)
                tempList.Remove(tempContained[i]);
            return list.Count - containedList.Count == tempList.Count;
        }

        public static TValue ValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey key) => d.ContainsKey(key) ? d[key] : default;

        public static int Max(this int[] numbers)
        {
            int max = int.MinValue;
            for (int i = 0; i < numbers.Length; i++)
                if (numbers[i] > max)
                    max = numbers[i];
            return max;
        }
        public static int Min(this int[] numbers)
        {
            int min = int.MaxValue;
            for (int i = 0; i < numbers.Length; i++)
                if (numbers[i] < min)
                    min = numbers[i];
            return min;
        }
        public static int[] Decrement(this int[] a, int decrement)
        {
            for (int i = 0; i < a.Length; i++)
                a[i] -= decrement;
            return a;
        }
        public static int[] Decrement(this int[] a, int decrement, int from, int to)
        {
            for (int i = from; i <= to; i++)
                a[i] -= decrement;
            return a;
        }
        public static int[] DecrementByOne(this int[] a)
        {
            for (int i = 0; i < a.Length; i++)
                a[i]--;
            return a;
        }
        public static int[] DecrementByOne(this int[] a, int from, int to)
        {
            for (int i = from; i <= to; i++)
                a[i]--;
            return a;
        }
        public static int[] Increment(this int[] a, int increment)
        {
            for (int i = 0; i < a.Length; i++)
                a[i] += increment;
            return a;
        }
        public static int[] Increment(this int[] a, int increment, int from, int to)
        {
            for (int i = from; i <= to; i++)
                a[i] += increment;
            return a;
        }
        public static int[] IncrementByOne(this int[] a)
        {
            for (int i = 0; i < a.Length; i++)
                a[i]++;
            return a;
        }
        public static int[] IncrementByOne(this int[] a, int from, int to)
        {
            for (int i = from; i <= to; i++)
                a[i]++;
            return a;
        }
        public static int[] RemoveNegatives(this int[] a)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < a.Length; i++)
                if (a[i] >= 0)
                    result.Add(a[i]);
            return result.ToArray();
        }
        public static int[] RemoveElementsMatchingIndices(this int[] a)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < a.Length; i++)
                if (a[i] != i)
                    result.Add(a[i]);
            return result.ToArray();
        }
        public static int[] RemoveElementsMatchingIndicesFromEnd(this int[] a, int length)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < a.Length; i++)
                if (a[i] != length - a.Length + i)
                    result.Add(a[i]);
            return result.ToArray();
        }

        public static int[] GetIndicesOfMatchingValues(this int[] a, int value)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < a.Length; i++)
                if (a[i] == value)
                    indices.Add(i);
            return indices.ToArray();
        }
        public static int[] GetIndicesOfMatchingValues(this bool[] a, bool value)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < a.Length; i++)
                if (a[i] == value)
                    indices.Add(i);
            return indices.ToArray();
        }

        public static int[] GetInt32ArrayFromMultidimensionalInt32Array(int[,] a, int dimension, int index)
        {
            int[] ar = new int[a.GetLength(dimension)];
            for (int i = 0; i < ar.Length; i++)
            {
                if (dimension == 0)
                    ar[i] = a[index, i];
                else if (dimension == 1)
                    ar[i] = a[i, index];
            }
            return ar;
        }
        public static int[] ToInt32Array(this bool[] a)
        {
            int[] ar = new int[a.Length];
            for (int i = 0; i < a.Length; i++)
                ar[i] = Convert.ToInt32(a[i]);
            return ar;
        }
        public static int[] ToInt32Array(this string[] s)
        {
            int[] ar = new int[s.Length];
            for (int i = 0; i < s.Length; i++)
                ar[i] = Convert.ToInt32(s[i]);
            return ar;
        }
        public static int[] ToInt32Array(this ListBox.ObjectCollection o)
        {
            int[] result = new int[o.Count];
            for (int i = 0; i < o.Count; i++)
                result[i] = (int)o[i];
            return result;
        }
        public static int[,] ToInt32Array(this string[,] s)
        {
            int a = s.Length;
            int b = s.GetLengths().Max();
            int[,] ar = new int[a, b];
            for (int i = 0; i < a; i++)
                for (int j = 0; j < b; j++)
                    ar[a, b] = Convert.ToInt32(s[a, b]);
            return ar;
        }
        public static double[,] ToDoubleArray(this string[,] s)
        {
            int a = s.Length;
            int b = s.GetLengths().Max();
            double[,] ar = new double[a, b];
            for (int i = 0; i < a; i++)
                for (int j = 0; j < b; j++)
                    ar[i, j] = Convert.ToDouble(s[i, j]);
            return ar;
        }
        public static decimal[] ToDecimalArray(this string[] s)
        {
            decimal[] ar = new decimal[s.Length];
            for (int i = 0; i < s.Length; i++)
                ar[i] = Convert.ToDecimal(s[i]);
            return ar;
        }
        public static decimal[,] ToDecimalArray(this string[,] s)
        {
            int a = s.GetLength(0);
            int b = s.GetLength(1);
            decimal[,] ar = new decimal[a, b];
            for (int i = 0; i < a; i++)
                for (int j = 0; j < b; j++)
                    ar[i, j] = Convert.ToDecimal(s[i, j]);
            return ar;
        }
        public static bool[] ToBooleanArray(this string[] s)
        {
            bool[] ar = new bool[s.Length];
            for (int i = 0; i < s.Length; i++)
                ar[i] = Convert.ToBoolean(s[i]);
            return ar;
        }
        public static bool[] ToBooleanArray(this int[] a)
        {
            bool[] ar = new bool[a.Length];
            for (int i = 0; i < a.Length; i++)
                ar[i] = Convert.ToBoolean(a[i]);
            return ar;
        }
        public static string[] ToStringArray(this int[] a)
        {
            string[] result = new string[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i].ToString();
            return result;
        }
        public static string[] ToStringArray(this decimal[] a)
        {
            string[] result = new string[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i].ToString();
            return result;
        }
        public static string[] ToStringArray(this double[] a)
        {
            string[] result = new string[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i].ToString();
            return result;
        }
        public static T[,] ToTwoDimensionalArray<T>(this List<T[]> l)
        {
            T[,] ar = new T[l.Count, l.GetLengths().Max()];
            for (int i = 0; i < l.Count; i++)
                for (int j = 0; j < l[i].Length; j++)
                    ar[i, j] = l[i][j];
            return ar;
        }
        public static List<List<T>> ToList<T>(this T[,] ar)
        {
            List<List<T>> l = new List<List<T>>();
            for (int i = 0; i < ar.GetLength(0); i++)
            {
                List<T> temp = new List<T>();
                for (int j = 0; j < ar.GetLength(1); j++)
                    temp.Add(ar[i, j]);
                l.Add(temp);
            }
            return l;
        }
        public static List<int> ToInt32List(this ListBox.ObjectCollection o)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < o.Count; i++)
                result.Add((int)o[i]);
            return result;
        }
        public static List<int> ToInt32List(this string[] s)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < s.Length; i++)
                result.Add(Convert.ToInt32(s[i]));
            return result;
        }
    }
}