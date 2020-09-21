using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomowaBiblioteka.Models
{
    public static class StreamExtensions
    {
        public static string ConvertToBase64FromPath(string blobPath)
        {
            if (System.IO.File.Exists(blobPath))
            {
                byte[] file = File.ReadAllBytes(blobPath);
                Stream stream = new MemoryStream(file);
                return ConvertToBase64(stream);
            }
            return null;
        }


        public static string ConvertToBase64(Stream stream)
        {
            var file = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(file, 0, (int)stream.Length);

            return Convert.ToBase64String(file);
        }


        public static Stream ConvertFromBase64(string base64string)
        {
            byte[] newBytes = Convert.FromBase64String(base64string);
            return new MemoryStream(newBytes);
        }

    }
}
