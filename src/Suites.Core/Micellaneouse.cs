using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suites.Core
{
    public static class Micellaneouse
    {
        public static T[] CombineToArray<T>(params T[] singles)
        {
            return singles;
        }

        public static T[] CombineToArray<T>(T[] head,params T[] singles)
        {
            return head.Concat(singles).ToArray();
        }
    }
}
