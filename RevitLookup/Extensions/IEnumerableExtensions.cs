using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections
{
    public static class EnumerableExtensions
    {
        public static List<T> ToList<T>(this IEnumerable set)
        {
            List<T> list = new List<T>();

            foreach (T element in set)
            {
                list.Add(element);
            }

            return list;
        }
    }
}