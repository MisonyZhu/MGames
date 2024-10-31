using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public static class BufferPool
    {
        private static Dictionary<int, Stack<byte[]>> Pools = new Dictionary<int, Stack<byte[]>>();

        public static byte[] Get(int size, bool powOf2 = true)
        {
            if (size == 0)
                return Array.Empty<byte>();
            if (powOf2)
                size = Mathf.NextPowerOfTwo(size);
            lock (BufferPool.Pools)
            {
                Stack<byte[]> numArrayStack;
                if (BufferPool.Pools.TryGetValue(size, out numArrayStack))
                {
                    if (numArrayStack.Count > 0)
                        return numArrayStack.Pop();
                }
            }
            return new byte[size];
        }

        public static void Recycle(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return;
            int length = buffer.Length;
            lock (BufferPool.Pools)
            {
                Stack<byte[]> numArrayStack;
                if (!BufferPool.Pools.TryGetValue(length, out numArrayStack))
                {
                    numArrayStack = new Stack<byte[]>();
                    BufferPool.Pools.Add(length, numArrayStack);
                }
                numArrayStack.Push(buffer);
            }
        }
    }
}