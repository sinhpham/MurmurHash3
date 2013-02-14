using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurmurHash3
{
    class Program
    {
        static void Main(string[] args)
        {
            var h = new MurmurHash3_x64_128();

            var s = "The quick brown fox jumps over the lazy cog";
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(s);
                    writer.Flush();
                    stream.Position = 0;
                    var x = h.ComputeHash(stream);
                    var str = BitConverter.ToString(x);
                    Console.WriteLine(str);
                }
            }
        }
    }
}
