using System;

namespace Shared.Domains
{
    public struct SecureReadInfo
    {
        public SecureReadInfo(bool escaped)
        {
            Length  = 0;
            Value = Array.Empty<byte>();
            Escaped = escaped;
        }

        public SecureReadInfo(Memory<byte> value, int length)
        {
            Length  = length;
            Value = value;
            Escaped = false;
        }

        public int Length {get; }
        public Memory<byte> Value {get;}
        public bool Escaped {get;}
    }
}
