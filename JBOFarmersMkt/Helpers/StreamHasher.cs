using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace JBOFarmersMkt.Helpers
{
    public class StreamHasher
    {
        public static string ComputeHash(Stream s)
        {
            SHA256Managed sha = new SHA256Managed();
            Byte[] hash = sha.ComputeHash(s);
            s.Position = 0; // Reset the stream so it can be read from again.
            return Convert.ToBase64String(hash);
        }
    }
}