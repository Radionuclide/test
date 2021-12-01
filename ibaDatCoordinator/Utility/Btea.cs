using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Utility
{
    public static class Btea
    {
        public static unsafe byte[] Encrypt(byte[] data, uint[] k)
        {
            if (data.Length <= 1)
                return data;

            byte[] dest = new byte[((data.Length + 3) / 4) * 4];
            Buffer.BlockCopy(data, 0, dest, 0, data.Length);

            fixed (byte* pDest = &dest[0])
            {
                uint* v = (uint*)pDest;
                int n = dest.Length / 4;

                unchecked
                {
                    const uint DELTA = 0x9e3779b9;
                    uint y = 0, z = 0, sum = 0, p = 0, rounds = 0, e = 0;
                    Func<uint> MX = () => (((z >> 5 ^ y << 2) + (y >> 3 ^ z << 4)) ^ ((sum ^ y) + (k[(p & 3) ^ e] ^ z)));

                    rounds = (uint)(6 + 52 / n);
                    z = v[n - 1];
                    do
                    {
                        sum += DELTA;
                        e = (sum >> 2) & 3;
                        for (p = 0; p < n - 1; p++)
                        {
                            y = v[p + 1];
                            z = v[p] += MX();
                        }
                        y = v[0];
                        z = v[n - 1] += MX();
                    } while (--rounds > 0);
                }
            }

            return dest;
        }

        public static unsafe byte[] Decrypt(byte[] data, uint[] k)
        {
            if (data.Length < 4)
                return data;

            byte[] dest = new byte[((data.Length + 3) / 4) * 4];
            Buffer.BlockCopy(data, 0, dest, 0, data.Length);

            fixed (byte* pDest = &dest[0])
            {
                uint* v = (uint*)pDest;
                int n = dest.Length / 4;

                unchecked
                {
                    const uint DELTA = 0x9e3779b9;
                    uint y = 0, z = 0, sum = 0, p = 0, rounds = 0, e = 0;
                    Func<uint> MX = () => (((z >> 5 ^ y << 2) + (y >> 3 ^ z << 4)) ^ ((sum ^ y) + (k[(p & 3) ^ e] ^ z)));

                    rounds = (uint)(6 + 52 / n);
                    sum = rounds * DELTA;
                    y = v[0];
                    do
                    {
                        e = (sum >> 2) & 3;
                        for (p = (uint)(n - 1); p > 0; p--)
                        {
                            z = v[p - 1];
                            y = v[p] -= MX();
                        }
                        z = v[n - 1];
                        y = v[0] -= MX();
                    } while ((sum -= DELTA) != 0);
                }
            }

            return dest;
        }
    }
}
