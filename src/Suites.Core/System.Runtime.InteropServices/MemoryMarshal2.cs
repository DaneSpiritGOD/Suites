using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.InteropServices
{
    public static class MemoryMarshal2
    {
        /// <summary>
        /// 检查memory的内存对象是否是数组切片，如果是，则返回数组；否则利用ToArray()返回内存拷贝。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memory"></param>
        /// <returns></returns>
        public static T[] GetZeroIndexedArrayOrCopyedContent<T>(this Memory<T> memory)
        {
            return MemoryMarshal.TryGetArray(memory, out ArraySegment<T> segment) ? segment.Array : memory.ToArray();
        }
    }
}
