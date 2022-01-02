using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography
{
    //string msg;
    //List<byte> msgInXBase;
    //List<byte> imgR;
    //List<byte> imgG;
    //List<byte> imgB;
    //Image img;
    //public Helper(string msg, Image img)
    //{
    //    this.msg = msg;
    //    this.img = img;
    //    imgR = new List<byte>();
    //    imgG = new List<byte>();
    //    imgB = new List<byte>();
    //}
    class Program
    {

        static void Main(string[] args)
        {
            bool em = true; ;
            if (em)
            {
                string s = "The technologies for building softwa. ";
                Image img2 = Image.FromFile(@"C:\Users\SAYTECH\Desktop\imagesssss.jpeg");
                Steganography st = new Steganography(img2, 123, 3);
                Console.WriteLine(s.Length + "asd");
                st.ExtractProc();

                Console.WriteLine(st.GetMsg() + "asd");
            }
            else
            {
                Console.WriteLine("Embedd");
                Image img = Image.FromFile(@"C:\Users\SAYTECH\Desktop\images.jpeg");

                string s = "The technologies for building softwa. ";
                Steganography st1 = new Steganography(s, img, 123, 3);
                st1.EmbeddedProc();
                st1.save(@"C:\Users\SAYTECH\Desktop\imagesssss.jpeg");

            }

        }
    }
}