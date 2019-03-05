using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.InteropServices
{
    internal static class MemoryMarshal2
    {
        /// <summary>
        /// 检查memory的内存对象是否是完整数组（索引与长度均一致），如果是，则直接返回数组；否则利用ToArray()返回内存拷贝。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memory"></param>
        /// <returns></returns>
        public static T[] GetZeroIndexedArrayOrCopyedContent<T>(this ReadOnlyMemory<T> memory)
        {
            return (MemoryMarshal.TryGetArray(memory, out ArraySegment<T> segment) && segment.Offset == 0 && segment.Array.Length == memory.Length) ? segment.Array : memory.ToArray();
        }

        public static T[] GetZeroIndexedArrayOrCopyedContent<T>(this Memory<T> memory)
        {
            return ((ReadOnlyMemory<T>)memory).GetZeroIndexedArrayOrCopyedContent();
        }
    }
}
