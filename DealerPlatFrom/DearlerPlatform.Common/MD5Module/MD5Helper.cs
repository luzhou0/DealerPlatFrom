using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Common.MD5Module
{
    public static class MD5Helper
    {
        public static string ToMd5(this string str)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes= md5.ComputeHash(Encoding.Default.GetBytes(str+"Love@Zhaoxi"));
            var md5Str = BitConverter.ToString(bytes).Replace("-","");
            return md5Str;
        }
    }
}