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
        Random r ;
       int keyLen;
        List<int> msgLenPos;

        public  KeyPosGen(int password  )
        {
            this.password = password;
            r = new Random(password);
            msgLenPos = new List<int>();
            


        }
        public void GetPos(int CoverImgSize, int keyLen, out List<int> pos,bool blueChannel)
        {
            pos = new List<int>();
            this.keyLen = keyLen;
            List<int> ll = new List<int>();
            while (pos.Count <= keyLen - 1)
            {
                int pFib;
                pFib = r.Next(0, 10);

                int pFibCount = r.Next(0, keyLen);
                // System.Console.Write(pFib+" "+pFibCount+"  ");
                int fi;
                for (int i = 0; i < pFibCount; i++)
                {

                    fi = Fib(pFib, i);
                    if ((pos.Count > 0 && pos.Contains(fi) )||(blueChannel && msgLenPos.Contains(fi)))
                    {

                        pFibCount++;

                    }
                    else if (fi > CoverImgSize)
                    {
                        // System.Console.Write(" break ");
                        break;
                    }
                    else
                    {
                        //System.Console.Write(fi + "  ");
                        pos.Add(fi);
                    }


                }


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
        public  int Fib(int p, int i)
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
