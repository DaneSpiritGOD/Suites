using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace NanomsgPlus
{
    public sealed class SpecifiedLengthMemoryPool<T> : MemoryPool<T>
    {
        public static new SpecifiedLengthMemoryPool<T> Shared { get; } = new SpecifiedLengthMemoryPool<T>();

        private readonly MemoryPool<T> _internalPool;
        public override int MaxBufferSize => _internalPool.MaxBufferSize;

        private SpecifiedLengthMemoryPool()
        {
            _internalPool = MemoryPool<T>.Shared;
        }

        public override IMemoryOwner<T> Rent(int specifiedLength)
        {
            return new SpecifiedLengthArrayMemoryPoolBuffer(_internalPool.Rent(specifiedLength), specifiedLength);
        }

        protected override void Dispose(bool disposing)
        {
        }

        private sealed class SpecifiedLengthArrayMemoryPoolBuffer : IMemoryOwner<T>, IDisposable
        {
            private readonly IMemoryOwner<T> _owner;
            private readonly int _size;

            public Memory<T> Memory => _owner.Memory.Slice(0, _size);

            public SpecifiedLengthArrayMemoryPoolBuffer(IMemoryOwner<T> owner, int size)
            {
                _owner = owner;
                _size = size;
            }

            public void Dispose()
            {
                _owner.Dispose();
            }
        }
    }
}
