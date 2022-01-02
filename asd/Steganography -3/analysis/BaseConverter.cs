using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography
{
    public class BaseConverter
    {
        public string chars =
"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ+=";
        //from value to representation (10 to any base)
        public string v2r(int value, int @base = 10)
        {
            var sb = new StringBuilder();
            do
            {
                int m = value % @base;
                sb.Insert(0, chars[m]);
                value = value / @base;
            } while (value > 0);
            return sb.ToString();
        }
        //from representation to value(any base to 10)
        public int r2v(string digits, int @base = 10)
        {
            int n = 0;
            foreach (char d in digits)
                n = n * @base + Array.IndexOf(chars.ToCharArray(), d);
            return n;
        }
        //base to base
        public string b2b(string digits, int b1, int b2)
        {
            return v2r(r2v(digits, b1), b2);
        }
    }

}
