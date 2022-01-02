using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography
{
    class KeyPosGen
    {
        int password;
        Random r;
        int keyLen;
        List<int> msgLenPos;
        List<int>pFib;
        public KeyPosGen(int password)
        {
            this.password = password;
            r = new Random(password);
            msgLenPos = new List<int>();
            pFib = new List<int>();



        }
        public void GetPos(int CoverImgSize, int keyLen, out List<int> pos, int Channel)
        {
            pos = new List<int>();
            this.keyLen = keyLen;
            List<int> ll = new List<int>();
            int p;
            p = (password) % 10;
            while (pos.Count <= keyLen - 1)
            {

                pFib.Clear();
                generatePFibSeries(p, CoverImgSize);
                int fi;
                for (int i = 0; i < pFib.Count; i++)
                {
                    int mul = 1;
                    while (pos.Count <= keyLen - 1 && mul * pFib[i] < CoverImgSize)
                    {
                       if((Channel==2 && !msgLenPos.Contains(pFib[i] * mul)) || Channel != 2) pos.Add(pFib[i] * mul);
                     
                        mul++;


                    }
                    if (pos.Count > keyLen - 1) break;
                }

                if (pos.Count <= keyLen - 1 ) p=pos[pos.Count - 1] % 10;

            }
                  
        }

        void generatePFibSeries(int p,int imgLen)
        {
            int i = 0;
            int element;
            while (true)
            {

                element = Fib(p, i++);
                if (element * 2 >= imgLen)
                    break;
                pFib.Add(element);


            }

        }
        public void GetOnePos(int CoverImgSize, out int pos)
        {

            pos = -1;
            int pFib;

            bool found = false;
            pFib = r.Next(0, 10);
            int i = 0;
            while (!found)
            {


                int pFibCount = r.Next(0, 1);
                int fi = Fib(pFib, i++);
                if ((msgLenPos.Count > 0 && msgLenPos.Contains(fi)))
                {
                    // System.Console.Write(" con ");
                    continue;

                }
                else if (fi > CoverImgSize)
                {
                    // System.Console.Write(" break ");
                    pFib = r.Next(0, 10);
                }
                else
                {
                    //System.Console.Write(fi + "  ");
                    pos = fi;
                    msgLenPos.Add(pos);
                    found = true;
                }







            }
        }
        public int Fib(int p, int i)
        {
            if (i <= p + 1)
            {


                return 1;
            }
            else
            {
                int r = Fib(p, i - 1) + Fib(p, i - p - 1);

                return r;

            }

        }
    }
}
