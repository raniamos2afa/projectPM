using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace analysis2
{
    public partial class Form1 : Form
    {
        int[] mod = { 2, 3, 4, 5, 6, 7, 8 };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string secretMsg="", coverImgPath="", folder="";
            int password;
            bool validPass;
            Image coverImg, stegoImg;
            Steganography.Steganography st;
            Stopwatch sw;
            string times = "";

            //open msg file
            MessageBox.Show("select secretMsg");
            OpenFileDialog msgFile = new OpenFileDialog();
            msgFile.Filter = "Text Files|*.txt";
            msgFile.Title = "Select Secret Message File";

            if (msgFile.ShowDialog() == DialogResult.OK)
            {
                secretMsg = File.ReadAllText(msgFile.FileName);
            }

            //open cover img
            MessageBox.Show("Select Cover Image");
            openFileDialog1.Title = "Select Cover Image ";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                coverImgPath = openFileDialog1.FileName;
            }

            //get password
            validPass = Int32.TryParse(textBox1.Text, out password);

            //fill picture box
            pictureBox1.Image = Image.FromFile(coverImgPath);
            coverImg = Image.FromFile(coverImgPath);

            //start embeing process
            if (validPass && secretMsg != "" && coverImg != null)
            {
                
                //compress the msg
                secretMsg = Compression.compress(secretMsg);
               
                //embed the msg with diffrent mod rang from 2 to 8
                for (int i = 2; i <= 8; i++)
                {
                  
                    st = new Steganography.Steganography(secretMsg, coverImg, password, i);
                     sw = Stopwatch.StartNew();
                    sw.Start();
                    if (st.EmbedProc())
                    {
                        sw.Stop();
                        times+= "mod_"+i+" : " + sw.Elapsed+Environment.NewLine;
                        if (i == 2)
                        {
                            MessageBox.Show("select irector to save stego img into it");
                            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                saveFileDialog1.FileName.LastIndexOf('\\');
                                folder = saveFileDialog1.FileName.Remove(saveFileDialog1.FileName.LastIndexOf('\\'), saveFileDialog1.FileName.Length - saveFileDialog1.FileName.LastIndexOf('\\') );

                               
                            }
                        }
                          st.save(folder + "\\mod" + i + ".tiff");

                        
                      
                        
                    }


                   

                }
                //print times
                textBox2.Text = times;
                File.AppendAllLines(folder + "\\res.txt", textBox2.Lines);
                

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string  coverImgPath = "", folder = "";
           
          
            Image coverImg, stegoImg;
            
            string psnrT = "";

            //open cover img
            MessageBox.Show("Select Cover Image");
            openFileDialog1.Title = "Select Cover Image ";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                coverImgPath = openFileDialog1.FileName;
            }
            coverImg = Image.FromFile(coverImgPath);
            //calculate psnr for stego from 2 to 8 mod
            for (int i = 2; i <= 8; i++)
            {
                
                    if (i == 2)
                    {
                        MessageBox.Show("select director to choose stego img from");
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            saveFileDialog1.FileName.LastIndexOf('\\');
                            folder = saveFileDialog1.FileName.Remove(saveFileDialog1.FileName.LastIndexOf('\\'), saveFileDialog1.FileName.Length - saveFileDialog1.FileName.LastIndexOf('\\'));


                        }
                    }
                stegoImg = Image.FromFile(folder+"\\mod"+i+".tiff");
                PSNR p = new PSNR(coverImg, stegoImg);
                psnrT+="psnr mod_"+i+" : "+ p.getPSNR() + ""+Environment.NewLine;

                







            }
            //print times
            textBox3.Text = psnrT;
            psnrT = "";
            File.AppendAllLines(folder + "\\res.txt", textBox3.Lines);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }
    }
    }

