using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Steganography
{
    class Steganography
    {
        string msg;
        List<char> msgB;
        Image img;
        List<byte> imgR, imgG, imgB;
        List<List<byte>> allRGB;
        List<int> RKey, Gkey, BKey;
        List<List<int>> allKeys;
        List<int> temp = new List<int>();
      KeyPosGen gen;
        int password, Base;

        int msgLen;
        public Steganography(string msg, Image coverImg, int pass, int b)
        {
            this.msg = msg;
            img = coverImg;
            password = pass;
            Base = b;
            gen = new KeyPosGen(password);
            allKeys = new List<List<int>>();
            allRGB = new List<List<byte>>();
            
            Helper.convertImgToBytes(img, out imgR, out imgG, out imgB);
            allRGB.Add(imgR); allRGB.Add(imgG); allRGB.Add(imgB);
            
        }
        public Steganography(Image StegoImg, int pass, int b)
        {

            Base = b;
            img = StegoImg;
            password = pass;
            gen = new KeyPosGen(password);
            allKeys = new List<List<int>>();
            allRGB = new List<List<byte>>();
            
            Helper.convertImgToBytes(img, out imgR, out imgG, out imgB);
            allRGB.Add(imgR); allRGB.Add(imgG); allRGB.Add(imgB);
        }
        void PrepairToEmbedding()
        {
            Thread[] th = new Thread[3];
            for (int i = 0; i < 3; i++)
            {
                int ind = i;
                if (ind == 0)
                    th[ind] = new Thread(() => gen.GetPos(imgR.Count, (msgLen / 3) + ((msgLen % 3 > ind) ? 1 : 0), out RKey, ind));
                else if(ind==1)
                {
                    th[ind] = new Thread(() => gen.GetPos(imgR.Count, (msgLen / 3) + ((msgLen % 3 > ind) ? 1 : 0), out Gkey, ind));

                }
                else
                    th[ind] = new Thread(() => gen.GetPos(imgR.Count, (msgLen / 3) + ((msgLen % 3 > ind) ? 1 : 0), out BKey, ind));


                th[ind].Start();

            }
            for (int i = 0; i < 3; i++) { th[i].Join(); }

            allKeys.Add(RKey);
            allKeys.Add(Gkey);
            allKeys.Add(BKey);
        }

        void PrepairToExtracting()
        {
            int msgLen = this.msgLen ;
            Thread[] th = new Thread[3];
            for (int i = 0; i < 3; i++)
            {
                int ind = i;
                if (ind == 0)
                    th[ind] = new Thread(() => gen.GetPos(imgR.Count, (msgLen / 3) + ((msgLen % 3 > ind) ? 1 : 0), out RKey, ind));
                else if (ind == 1)
                {
                    th[ind] = new Thread(() => gen.GetPos(imgR.Count, (msgLen / 3) + ((msgLen % 3 > ind) ? 1 : 0), out Gkey, ind));

                }
                else
                    th[ind] = new Thread(() => gen.GetPos(imgR.Count, (msgLen / 3) + ((msgLen % 3 > ind) ? 1 : 0), out BKey, ind));

                th[ind].Start();

            }
            for (int i = 0; i < 3; i++) { th[i].Join(); }
            allKeys.Add(RKey);
            allKeys.Add(Gkey);
            allKeys.Add(BKey);

           
        }

        public void EmbedMsgLen()
        {
            List<char> msgLenB;
            //add , to terminate msgLen for extract process
            Helper.ConvertStrToB(this.msgLen + ",", Base, out msgLenB);
            
            List<int> msgLenKey = new List<int>();
            byte f; int s;
            int d;
            int p;

            for (int i = 0; i < msgLenB.Count; i++)
                 {
                       s = Convert.ToInt16(msgLenB[i] + "");               
                       gen.GetOnePos(img.Width * img.Height, out p);               
                       f = Convert.ToByte(imgB[p] % Base);
                       d = s - f;
            
                       allRGB[2][p] = (d > Base / 2 || allRGB[2][p] == 255) && allRGB[2][p] != 0 ? Convert.ToByte(allRGB[2][p] - (Base - d)) : Convert.ToByte(allRGB[2][p] + d);
                   }
           
         }

            
        void extractMsgLen()
        {
            List<char> msgLenB = new List<char>();
            byte f; int p, temp; string ch = "",x;
            int BN = Helper.bitsNum(Base);
            do
            {
                if (ch.Length == BN)
                {
                    msgLenB.AddRange(ch);
                  
                    Helper.ConvertBToStr(msgLenB, Base, out x);
                    if (!Int32.TryParse(x, out temp))
                    {
                        
                        msgLenB.RemoveRange(msgLenB.Count-BN,BN);
                        break;
                    }               
                  ch = "";
                }
                gen.GetOnePos(img.Width * img.Height, out p);
            
                f = Convert.ToByte(allRGB[2][p] % Base);
                ch += f;
            } while (true);
            string msgLenStr;
            Helper.ConvertBToStr(msgLenB, Base, out msgLenStr);
            this.msgLen = Int32.Parse(msgLenStr);                      
        }
        void thProc(int start,int end,int thN,bool embedProc)
        {
            int s, p, f,d;
            int channel; ;
            int keyInd;
            for (int i = start; i < end; i++)
            {
                channel = i % 3;
                keyInd =(int)Math.Ceiling( (i+ 1)/3.0 )-1;
                p = allKeys[channel][keyInd];
                if (embedProc)
                {
                    s = Convert.ToInt16(msgB[i] + "");
                 
                    f = Convert.ToByte(allRGB[channel][p] % (Base));
                    d = s - f;
                    allRGB[channel][p] = ((d > Base / 2 || allRGB[channel][p] == 255) && (allRGB[channel][p] - (Base - d)) >= 0) ? Convert.ToByte(allRGB[channel][p] - (Base - d)) : Convert.ToByte(allRGB[channel][p] + d);
                }
                else
                {
                    

                    f = Convert.ToByte(allRGB[channel][p] % Base);
                
                    msgB[i] = (f+"")[0];
                }
            }

            }

       

       
        public bool EmbedProc()
        {
            Helper.ConvertStrToB(msg, Base, out msgB);
            // MessageBox.Show("msg converted");
            

            this.msgLen = msgB.Count;

            EmbedMsgLen();
           // MessageBox.Show("len emb");

            PrepairToEmbedding();
            //MessageBox.Show("pos generated");

            int ThN = 50;//  msgB.Count/100;
            Thread[] th = new Thread[ThN];

            int r = msgB.Count % ThN;
            for (int i = 0; i < ThN; i++)
            {
                int ind = i;
                int Block = msgB.Count / ThN + ((r > ind) ? 1 : 0);
                int msgStartInd = Block  * ind + ((ind >= r) ? (r) : 0);
                int msgEndInd = msgStartInd + (Block);
                th[i] = new Thread(() => thProc(msgStartInd, msgEndInd, ind,true));
                th[i].Start();
            }
            for (int i = 0; i < ThN; i++)
                th[i].Join();
           // MessageBox.Show("msg emb");
            Helper.convertBytesToImg(imgR, imgG, imgB, img.Width, img.Height, out img);

            return true;
        }
        

        public void save(string path)
        {
            img.Save(path);

        }

      
        public void ExtractProc()
        {
            extractMsgLen();

            PrepairToExtracting();
            char[] t = new char[msgLen];
            msgB = new List<char>();
            msgB.AddRange(t);
           

            int ThN =  msgB.Count / 30;
            Thread[] th = new Thread[ThN];

            int r = msgLen % ThN;
         
            for (int i = 0; i < ThN; i++)
            {
                int ind = i;
                int Block = msgLen / ThN + ((r > ind) ? 1 : 0);
                int msgStartInd = Block * ind + ((ind >= r) ? (r) : 0);
                int msgEndInd = msgStartInd + (Block);
                th[i] = new Thread(() => thProc(msgStartInd, msgEndInd, ind, false));
                th[i].Start();
            }
            for (int i = 0; i < ThN; i++)
                th[i].Join();
            
            Helper.ConvertBToStr(msgB, Base, out msg);

            
        }
        public string GetMsg()
        {
            return msg;

        }

       
    }
}
