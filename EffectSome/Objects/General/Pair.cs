using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EffectSome
{
    public class Pair<T1, T2>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public Pair() { }
        public Pair(T1 first, T2 second)
        {
            Item1 = first;
            Item2 = second;
        }
    }
    public class Pair
    {
        public List<Type> Types
        {
            get
            {
                List<Type> result = new List<Type>();
                foreach (object o in Items)
                    result.Add(o.GetType());
                return result;
            }
        }
        public List<object> Items;

        public Pair() { }
        public Pair(params object[] items)
        {
            foreach (object o in items)
                Items.Add(o);
        }
    }

    // The following code block is the representation of the above class in CA#
    // This still needs to be worked on as the structure gets a bit weird
#if CASHARP
    public class Pair<params T[]>
    {
        public T[][] Items;

        public Pair() { }
        public Pair(params T[][] items)
        {
            for (int i = 0; i < items.Length; i++)
                Items[i] = items[i];
        }
    }
#endif
}
