using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MurmurHash3
{
    class MurmurHash3_x86_32 : HashAlgorithm
    {
        public override void Initialize()
        {

        }

        public UInt32 Seed { get; set; }

        private UInt32 _h1 = 0;

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            byte[] data = array;
            var len = cbSize;
            var nblocks = len / 4;

            _h1 = Seed;

            const UInt32 c1 = 0xcc9e2d51;
            const UInt32 c2 = 0x1b873593;

            //----------
            // body
            UInt32 k1 = 0;
            for (int i = 0; i < nblocks; ++i)
            {
                k1 = BitConverter.ToUInt32(data, i * 4);

                k1 *= c1;
                k1 = RotateLeft32(k1, 15);
                k1 *= c2;

                _h1 ^= k1;
                _h1 = RotateLeft32(_h1, 13);
                _h1 = _h1 * 5 + 0xe6546b64;
            }

            //----------
            // tail
            k1 = 0;
            var tailIdx = nblocks * 4;
            switch (len & 3)
            {
                case 3: k1 ^= (UInt32)(data[tailIdx + 2]) << 16;
                    goto case 2;
                case 2: k1 ^= (UInt32)(data[tailIdx + 1]) << 8;
                    goto case 1;
                case 1: k1 ^= (UInt32)(data[tailIdx + 0]);
                    k1 *= c1; k1 = RotateLeft32(k1, 15); k1 *= c2; _h1 ^= k1;
                    break;
            };

            //----------
            // finalization
            _h1 ^= (UInt32)len;
            _h1 = FMix(_h1);
        }

        UInt32 RotateLeft32(UInt32 x, byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        UInt32 FMix(UInt32 h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;

            return h;
        }

        protected override byte[] HashFinal()
        {
            HashValue = new byte[4];
            Array.Copy(BitConverter.GetBytes(_h1), 0, HashValue, 0, 4);
            return HashValue;
        }
    }
}
