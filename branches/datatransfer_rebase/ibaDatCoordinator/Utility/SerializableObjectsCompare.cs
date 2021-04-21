using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace iba.Utility
{
    public class SerializableObjectsCompare
    {
        public static bool Compare(Object obj1, Object obj2)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream str1 = new MemoryStream();
                bf.Serialize(str1, obj1);

                MemoryStream str2 = new MemoryStream();
                bf.Serialize(str2, obj2);

                //if (str1.Length != str2.Length)
                //    return false;

                byte[] ar1 = str1.GetBuffer();
                byte[] ar2 = str2.GetBuffer();
                //File.WriteAllBytes(@"d:\klad\ar1.bin", ar1);
                //File.WriteAllBytes(@"d:\klad\ar2.bin", ar2);
                for (int i = 0; i < (int) str1.Length;i++)
                    if (ar1[i] != ar2[i]) return false;
                return true;
            }
            catch //(Exception ex)
            {
                //File.WriteAllText(@"d:\klad\errormessage", ex.Message);
                return false;
            }
        }
    }
}
