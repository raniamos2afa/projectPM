using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography
{
    class Steganography
    {
        string msg;
       List< string> msgB;
        Image img;
        List<byte> imgR, imgG, imgB;
        List<int> RKey, Gkey, BKey;
        KeyPosGen gen;
        int password,Base;

        int msgLen;
        public Steganography(string msg,Image coverImg,int pass,int b)
        {
            this.msg = msg;
            img = coverImg;
            password = pass;
            gen = new KeyPosGen(password);
            Base = b;
            Helper.convertImgToBytes(img, out imgR, out imgG, out imgB);

        }
        public Steganography(Image StegoImg, int pass, int b)
        {
          
            img = StegoImg;
            password = pass;
            gen = new KeyPosGen(password);
            Base = b;
            Helper.convertImgToBytes(img, out imgR, out imgG, out imgB);

        }
        void PrepairToEmbedding()
        {
            
            Helper.ConvertStrToB(msg,Base,out msgB);
            
            
            int msgLen = msgB.Count * 8;
  
            gen.GetPos(imgR.Count,(msgLen/3)+( (msgLen % 3>=1)?1:0), out RKey,false);
            gen.GetPos(imgG.Count, (msgLen / 3) + ((msgLen % 3 >= 2) ? 1 : 0), out Gkey,false);
            gen.GetPos(imgB.Count,(msgLen / 3), out BKey,true);
           
        }
        
        void PrepairToExtracting()
        {
            
          
           
            int msgLen = this.msgLen * 8;
            gen.GetPos(imgR.Count, (msgLen / 3) + ((msgLen % 3 >= 1) ? 1 : 0), out RKey,false);
            gen.GetPos(imgG.Count, (msgLen / 3) + ((msgLen % 3 >= 2) ? 1 : 0), out Gkey,false);
            gen.GetPos(imgB.Count, (msgLen / 3), out BKey,true);
          
            //Console.WriteLine();
            //for (int i = 0; i < BKey.Count; i++)
            //    Console.Write(BKey[i] + " ");
        }

        public void EmbedMsgLen()
        {
           List< string> msgLenB;
            //add , to terminate msgLen for extract process
            Helper.ConvertStrToB(msg.Length+",", Base, out msgLenB);
            List<int> msgLenKey = new List<int>();
         
            byte f; int s;
            int d;
            int p;


           // Console.WriteLine(msgLenB.Count + "  msgLenB.Count");
            for (int i = 0; i < msgLenB.Count; i++)
            {

                {
                    foreach (char c in msgLenB[i])
                    {
                           s = Convert.ToInt16(c + "");
                        
                            gen.GetOnePos(img.Width*img.Height,out p);
                       // Console.WriteLine("  GetOnePos");
                        f = Convert.ToByte(imgB[p] % Base);
                            d = s - f;
                            imgB[p] = (d > Base / 2) ? Convert.ToByte(imgB[p] - (Base - d)) : Convert.ToByte(imgB[p] + d);

                       
                       
                    }
                }
   
            }
          
        }
        void extractMsgLen()
        {
            List<string> msgLenB = new List<string>();


            byte f;
         
            int p;
            string ch = "";
            string x ;
            int temp;
            do
            {
                if (ch.Length == 8)
                {
                    msgLenB.Add(ch + "");
                    Helper.ConvertBToStr(msgLenB, Base, out x);
                    foreach (var z in msgLenB)
                    {
                        Console.WriteLine(z + " hhhh ");
                    }
                    if (!Int32.TryParse(x, out temp))
                    {
                        msgLenB.RemoveAt(msgLenB.Count - 1);
                        break;
                    }
                    
                    ch = "";
                }
                
                
                    gen.GetOnePos(img.Width*img.Height, out p);
                    f = Convert.ToByte(imgB[p] % Base);
                    ch += f;
                
                
            } while (true);
            string msgLenStr;
            Helper.ConvertBToStr(msgLenB, Base, out msgLenStr);

          
            this.msgLen = Int32.Parse(msgLenStr);
            Console.WriteLine(msgLen + " hhhh ");



        }
        public void EmbeddedProc()
        {
            EmbedMsgLen();
            PrepairToEmbedding();
            //channel=0 then embedd in red ,ch=1 emb in g, ch=2 emb in b
            int channel = 0;
            int RKeyInd = 0, GKeyInd = 0, BKeyInd = 0;
            byte f;int s;
            int d;
            int p;
           
           
           
            for (int i = 0; i < msgB.Count; i++)
            {

                {
                    foreach (char c in msgB[i])
                    {
                        s = Convert.ToInt16(c + "");
                        if (channel == 0)
                        {
                            p = RKey[RKeyInd++];
                            f = Convert.ToByte(imgR[p] % (Base));
                            d = s - f;
                            imgR[p] = (d > Base/2) ? Convert.ToByte(imgR[p] -(Base- d)) : Convert.ToByte(imgR[p] + d);

                        }
                        else if (channel == 1)
                        {
                            p = Gkey[GKeyInd++];
                            f = Convert.ToByte(imgG[p] % Base);
                            d = s - f;
                           
                            imgG[p] = (d > Base / 2) ? Convert.ToByte(imgG[p] - (Base - d)) : Convert.ToByte(imgG[p] + d);

                        }
                        else
                        {
                            p = BKey[BKeyInd++];
                            f = Convert.ToByte(imgB[p] % Base);
                            d = s - f;
                            imgB[p] = (d > Base / 2) ? Convert.ToByte(imgB[p] - (Base - d)) : Convert.ToByte(imgB[p] + d);

                        }
                        channel = (channel + 1) % 3;
                    }
                }
                Helper.convertBytesToImg(imgR, imgG, imgB, img.Width, img.Height, out img);
            }

        }
        public void save(string path)
        {
            img.Save(path);

        }

        public void ExtractProc()
        {
             extractMsgLen();
             PrepairToExtracting();
            msgB = new List<string>();
            //channel=0 then embedd in red ,ch=1 emb in g, ch=2 emb in b
         
            int RKeyInd = 0, GKeyInd = 0, BKeyInd = 0,channel=0;
            byte f, s;
            int d;
            int p;
            string ch = "";
            for (int i = 0; i < this.msgLen*8; i++)
            {
                if (ch.Length == 8)
                {
                    msgB.Add(ch + "");
                    ch = "";
                }
                if (channel == 0)
                {
                    p = RKey[RKeyInd++];
                    f = Convert.ToByte(imgR[p] % (Base));
                   ch+=f;
                }
                else if (channel == 1)
                {
                    p = Gkey[GKeyInd++];
                    f = Convert.ToByte(imgG[p] % Base);
                    ch += f;
                }
                else
                {
                    p = BKey[BKeyInd++];
                    f = Convert.ToByte(imgB[p] % Base);
                    ch += f;
                }
                channel = (channel + 1) % 3;
            }
            Helper.ConvertBToStr(msgB,Base,out msg);

        }
        public string GetMsg()
        {
            return msg;

        }
    }
}
