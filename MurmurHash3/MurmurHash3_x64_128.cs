using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MurmurHash3
{
    class MurmurHash3_x64_128 : HashAlgorithm
    {
        public override void Initialize()
        {

        }

        UInt64 seed = 0;
        UInt64 h1 = 0;
        UInt64 h2 = 0;

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            byte[] data = array;
            var len = cbSize;
            int nblocks = len / 16;

            h1 = seed;
            h2 = seed;

            const UInt64 c1 = 0x87c37b91114253d5;
            const UInt64 c2 = 0x4cf5ad432745937f;

            //----------
            // body

            UInt64 k1 = 0;
            UInt64 k2 = 0;

            for (int i = 0; i < nblocks; i++)
            {
                k1 = BitConverter.ToUInt64(data, i * 16);//GetBlock(blocks,i*2+0);
                k2 = BitConverter.ToUInt64(data, i * 16 + 8);//GetBlock(blocks,i*2+1);

                k1 *= c1; k1 = RotateLeft64(k1, 31); k1 *= c2; h1 ^= k1;

                h1 = RotateLeft64(h1, 27); h1 += h2; h1 = h1 * 5 + 0x52dce729;

                k2 *= c2; k2 = RotateLeft64(k2, 33); k2 *= c1; h2 ^= k2;

                h2 = RotateLeft64(h2, 31); h2 += h1; h2 = h2 * 5 + 0x38495ab5;
            }

            //----------
            // tail

            var tailIdx = nblocks * 16;
            //const UInt8 * tail = (const UInt8*)(data + nblocks*16);
            k1 = 0;
            k2 = 0;


            switch (len & 15)
            {
                case 15: k2 ^= (UInt64)(data[tailIdx + 14]) << 48;
                    goto case 14;
                case 14: k2 ^= (UInt64)(data[tailIdx + 13]) << 40;
                    goto case 13;
                case 13: k2 ^= (UInt64)(data[tailIdx + 12]) << 32;
                    goto case 12;
                case 12: k2 ^= (UInt64)(data[tailIdx + 11]) << 24;
                    goto case 11;
                case 11: k2 ^= (UInt64)(data[tailIdx + 10]) << 16;
                    goto case 10;
                case 10: k2 ^= (UInt64)(data[tailIdx + 9]) << 8;
                    goto case 9;
                case 9: k2 ^= (UInt64)(data[tailIdx + 8]) << 0;
                    k2 *= c2; k2 = RotateLeft64(k2, 33); k2 *= c1; h2 ^= k2;
                    goto case 8;
                case 8: k1 ^= (UInt64)(data[tailIdx + 7]) << 56;
                    goto case 7;
                case 7: k1 ^= (UInt64)(data[tailIdx + 6]) << 48;
                    goto case 6;
                case 6: k1 ^= (UInt64)(data[tailIdx + 5]) << 40;
                    goto case 5;
                case 5: k1 ^= (UInt64)(data[tailIdx + 4]) << 32;
                    goto case 4;
                case 4: k1 ^= (UInt64)(data[tailIdx + 3]) << 24;
                    goto case 3;
                case 3: k1 ^= (UInt64)(data[tailIdx + 2]) << 16;
                    goto case 2;
                case 2: k1 ^= (UInt64)(data[tailIdx + 1]) << 8;
                    goto case 1;
                case 1: k1 ^= (UInt64)(data[tailIdx + 0]) << 0;
                    k1 *= c1; k1 = RotateLeft64(k1, 31); k1 *= c2; h1 ^= k1;
                    break;
            };

            //----------
            // finalization
            h1 ^= (UInt64)len; h2 ^= (UInt64)len;

            h1 += h2;
            h2 += h1;

            h1 = FMix(h1);
            h2 = FMix(h2);

            h1 += h2;
            h2 += h1;

            
            //((uint64_t*)out)[0] = h1;
            //((uint64_t*)out)[1] = h2;
        }

        protected override byte[] HashFinal()
        {
            var HashValue = new byte[16];
            Array.Copy(BitConverter.GetBytes(h1), 0, HashValue, 0, 8);
            Array.Copy(BitConverter.GetBytes(h2), 0, HashValue, 8, 8);
            return HashValue;
        }

        private UInt64 FMix(UInt64 k)
        {
            k ^= k >> 33;
            k *= 0xff51afd7ed558ccd;
            k ^= k >> 33;
            k *= 0xc4ceb9fe1a85ec53;
            k ^= k >> 33;

            return k;
        }

        private UInt64 RotateLeft64(UInt64 x, byte r)
        {
            return (x << r | x >> (64 - (r & 63)));
            //return (x << r) | (x >> (64 - r));
        }

        public override int HashSize
        {
            get
            {
                return 128;
            }
        }
    }
}
