using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analysis2
{
    class Compression
    {
        public static string compress(string inputStr)
        {

            byte[] inputBytes = Encoding.UTF8.GetBytes(inputStr);

            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gZipStream.Write(inputBytes, 0, inputBytes.Length);

                var outputBytes = outputStream.ToArray();

                var outputbase64 = Convert.ToBase64String(outputBytes);
                return (outputbase64);


            }
        }

        public static string decompressed(string inputStr)
        {


            byte[] inputBytes = Convert.FromBase64String(inputStr);

            using (var inputStream = new MemoryStream(inputBytes))
            using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gZipStream))
            {
                var decompressed = streamReader.ReadToEnd();

                return (decompressed);


            }
        }
    }
}
