using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Steganography
{
    class Helper
    {


        public static void ConvertStrToB(string msg, int baseB,out List<string> msgB)
        {
            BaseConverter con = new BaseConverter();
            msgB =new List<string>();
            string charB = "";
            foreach (char c in msg)
            {
                //convert each char to decimal then convert it to baseB
                //each char should represented in 8 digit so i can retrive the char from base b number
         
                charB = con.v2r(Convert.ToByte(c), baseB);
                if (charB.Length < 8)
                   charB= charB.PadLeft(8, '0');
               
                msgB.Add(charB);
            }
          

        }
      public static  void ConvertBToStr(List<string> msgB, int baseB, out string str)
        {
            BaseConverter con = new BaseConverter();
            str = "";
            foreach (string c in msgB)
            {


               str+=Convert.ToChar(con.r2v(c,baseB));
            }


        }

        public static void convertImgToBytes(Image img,out List<byte>r, out List<byte> g, out List<byte>b)
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

        public static void convertBytesToImg( List<byte> r, List<byte> g, List<byte> b,int w,int h,out Image img)
        {
            Bitmap bm = new Bitmap(w, h);
            int c = 0;
          
                for (int i = 0; i < bm.Width; i++)
                    for (int j = 0; j < bm.Height; j++)
                    {

                   
                    bm.SetPixel(i, j,Color.FromArgb(r[c],g[c],b[c]));
                    c++;
                }
            img = (Image)(bm);
            }

        }
    }

