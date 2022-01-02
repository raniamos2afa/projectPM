using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.IO.Compression;
using System.Windows.Forms;
using System.IO;

namespace Steganography
{
    class Helper
    {

       
        public static int bitsNum(int Base)
        {
            if (Base == 2) return 8;
            else if (Base == 3) return 6;
            else if (Base > 3 && Base < 7) return 4;
            else
                return 3;


        }
        public static void ConvertStrToB(string msg, int baseB, out List<char> msgB)
        {
            BaseConverter con = new BaseConverter();
            msgB = new List<char>();
            string charB = "";
            int BN = bitsNum(baseB);
            foreach (char c in msg)
            {
                //convert each char to decimal then convert it to baseB
                //each char should represented in 8 digit so i can retrive the char from base b number
                Int32 ch = Convert.ToInt32(c );
               
                charB = con.v2r(ch, baseB);
                if (charB.Length < BN)
                    charB = charB.PadLeft(BN, '0');
               // Debug.WriteLine(c+" "+charB);
                msgB.AddRange(charB.ToArray());
               
                //MessageBox.Show(c + " " + ch+"   "+charB);
            }


        }
        public static void ConvertBToStr(List<char> msgB, int baseB, out string str)
        {
            BaseConverter con = new BaseConverter();
            str = "";
            string temp = "";
           
            int BN = bitsNum(baseB);
            foreach (char c in msgB)
            {
                temp += c;
                if ((temp.Length== BN ))
                //{
                //    if (i == msgB.Count - 1)
                //        temp += c;
                {
                    str += Convert.ToChar(con.r2v(temp, baseB));
                   // Debug.WriteLine(temp);
                  // Debug.WriteLine(Convert.ToChar(con.r2v(temp, baseB))+"   "+temp);
                    temp =  "";
                }
                //else
                //{
                //    temp += c;
                //}
                //i++;
            }


        }

        public static void convertImgToBytes(Image img, out List<byte> r, out List<byte> g, out List<byte> b)
        {
            r = new List<byte>();
            g = new List<byte>();
            b = new List<byte>();
            using (Bitmap bm = new Bitmap(img))
            {
                Color c;
                for (int i = 0; i < bm.Width; i++)
                    for (int j = 0; j < bm.Height; j++)
                    {
                        c = bm.GetPixel(i, j);
                        r.Add(c.R);
                        g.Add(c.G);
                        b.Add(c.B);

                    }

            }

        }

        public static void convertBytesToImg(List<byte> r, List<byte> g, List<byte> b, int w, int h, out Image img)
        {
            Bitmap bm = new Bitmap(w, h);
            int c = 0;

            for (int i = 0; i < bm.Width; i++)
                for (int j = 0; j < bm.Height; j++)
                {


                    bm.SetPixel(i, j, Color.FromArgb(r[c], g[c], b[c]));
                    c++;
                }
            img = (Image)(bm);
        }

    }
}

