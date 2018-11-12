using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.InteropServices
{
    public static class MemoryMarshal2
    {
        /// <summary>
        /// 如果memory是直接从array转换得到的，直接拿到array的原引用;否则利用ToArray()返回拷贝的数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memory"></param>
        /// <returns></returns>
        public static T[] GetArray<T>(this Memory<T> memory)
        {
            return MemoryMarshal.TryGetArray(memory, out ArraySegment<T> segment) ? segment.Array : memory.ToArray();
        }
    }
}
