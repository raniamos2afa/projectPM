using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SteganographyProc
{
    class PSNR
    {
        Image img1, img2;
        public PSNR(Image img1, Image img2)
        {
            this.img1 = img1;
            this.img2 = img2;

        }
        //void thProc(int start, int end, int thN)
        //{
        //    int sum;
        //    for (int i= 0; i < img1.Height; i++)
        //        for (int j = start; j < end; j++)
                
        //        { }
        //}
             public   double getMSE()
        {
            double sum=0,diff;
            using (Bitmap bm1 = new Bitmap(img1),bm2= new Bitmap(img2))
            {
                Color c1,c2;
                for (int i = 0; i < bm1.Width; i++)
                    for (int j = 0; j < bm1.Height; j++)
                    {
                        c1 = bm1.GetPixel(i, j);
                        c2=bm2.GetPixel(i, j);
                        diff = ((c1.R + c1.G + c1.B) / 3) - ((c2.R + c2.G + c2.B) / 3);
                        sum += Math.Pow( diff,2);
                        

                    }

            }
            sum /= (img1.Width * img1.Height);
            return sum;
        }

        public double getPSNR()
        {

            return 10 * Math.Log10((255 * 255) / getMSE());

        }
    }
}
