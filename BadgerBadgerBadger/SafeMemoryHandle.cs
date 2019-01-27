using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace BadgerBadgerBadger
{
    public class SafeMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public int MemorySize
        {
            get; private set;
        }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        public byte[] GetBytes()
        {
            if (IsInvalid || IsClosed)
            {
                throw new InvalidOperationException("The Handle is invalid");
            }
            byte[] Data = new byte[MemorySize];
            Marshal.Copy(handle, Data, 0, MemorySize);
            return Data;
        }

        public void FillBytes(byte[] Data, int Start, int Count)
        {
            if (IsInvalid || IsClosed)
            {
                throw new InvalidOperationException("The Handle is invalid");
            }
            Marshal.Copy(handle, Data, Start, Count);
        }

        public void SetBytes(byte[] Data, int Start, int Count)
        {
            if (IsInvalid || IsClosed)
            {
                throw new InvalidOperationException("The Handle is invalid");
            }
            if (Count > MemorySize)
            {
                throw new InsufficientMemoryException($"Tried to write too many bytes. Want={Count} Is={MemorySize}");
            }
            Marshal.Copy(Data, Start, handle, Count);
        }

        protected override bool ReleaseHandle()
        {
            if (handle != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(handle);
                handle = IntPtr.Zero;
            }
            return true;
        }

        public SafeMemoryHandle(IntPtr MemoryPtr, int Size) : base(true)
        {
            handle = MemoryPtr;
            MemorySize = Size;
        }

        public SafeMemoryHandle(int ByteCount) : base(true)
        {
            handle = Marshal.AllocHGlobal(ByteCount);
            MemorySize = ByteCount;
        }
    }
}
