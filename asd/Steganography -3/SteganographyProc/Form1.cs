using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Steganography;
using System.Diagnostics;
using System.IO.Compression;

namespace SteganographyProc
{
    public partial class baseCombo2 : Form
    {
        string secretMsg="",extractedMsg="";
        string coverImgPath="", stegoImgPath = "",msgFilePath="";
        int password;
        bool passEntered = false;
        Image coverImg, stegoImg;
        public baseCombo2()
        {
            InitializeComponent();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
           
            RadioButton rb = (RadioButton)sender;
            if (browseMsg.Checked)
            {
                msgTxt.Enabled = false;
                msgTxt.ForeColor = System.Drawing.SystemColors.WindowFrame;
                msgTxt.Text = "Enter Your Message Here";
                OpenFileDialog msgFile = new OpenFileDialog();
                msgFile.Filter = "Text Files|*.txt";
                msgFile.Title = "Select Secret Message File";

                if (msgFile.ShowDialog() == DialogResult.OK)
                {
                    secretMsg = File.ReadAllText(msgFile.FileName);
                }
            }
            else
            {
                msgTxt.Enabled = true;
                msgTxt.Text = "";
                msgTxt.ForeColor =Color.Black;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            passEntered = true;
            EmbedBtn.Enabled = true;


        }

        private void pathRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Name.Equals("browseRadio"))
            {
               panel1.Enabled = false;
               OpenFileDialog imgFile = new OpenFileDialog();
              //  imgFile.Filter = "Image Files (*.jpeg; *.png; *.bmp)|*.jpg; *.png; *.bmp";
                
                imgFile.Title = "Select Cover Image";

                if (imgFile.ShowDialog() == DialogResult.OK)
                {
                    coverImgPath = imgFile.FileName;
                }
            }
            else
            {
                panel1.Enabled = true;
                pathTxt.Text = "";
              
                
            }
        }
        private string compression(string inputStr)
        {

            byte[] inputBytes = Encoding.UTF8.GetBytes(inputStr);

            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gZipStream.Write(inputBytes, 0, inputBytes.Length);

                var outputBytes = outputStream.ToArray();

                var outputbase64 = Convert.ToBase64String(outputBytes);
               return (outputbase64);


            }
        }

        private string decompressed(string inputStr)
        {

            
            byte[] inputBytes = Convert.FromBase64String(inputStr);

            using (var inputStream = new MemoryStream(inputBytes))
            using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gZipStream))
            {
                var decompressed = streamReader.ReadToEnd();

                return(decompressed);
               

            }
        }
        private void EmbedBtn_Click(object sender, EventArgs e)
        {
           
            bool validPass = false;
            validPass = Int32.TryParse(passTxt.Text, out password);

            if (writeMsg.Checked)
            {
                secretMsg = msgTxt.Text;

            }

            if (pathRadio.Checked)
            {
                coverImgPath = @pathTxt.Text;
            }
            coverImg = Image.FromFile(coverImgPath);

            ///////////Embed Proc
            if (validPass && secretMsg != "" && coverImg != null)
            {

              //  if(secretMsg.Length*8>(coverImg.Width*coverImg.Height*3 - secretMsg.Length.ToString().Length*8))
                    MessageBox.Show(secretMsg.Length+" "+ coverImg.Width * coverImg.Height );
                // if (secretMsg.Length * 8 + ((secretMsg.Length + "").Length * 8) > coverImg.Width * coverImg.Height) MessageBox.Show("secert msg length is to big ");
                secretMsg = compression(secretMsg);
                Steganography.Steganography st = new Steganography.Steganography(secretMsg, coverImg, password,Int32.Parse( baseBox.SelectedItem+""));
                Stopwatch sw = Stopwatch.StartNew();
                sw.Start();
                if (st.EmbedProc())
                {
                    sw.Stop();
                  textBox2.Text=""+ sw.Elapsed;
                    SaveFileDialog sv = new SaveFileDialog();
                    if(sv.ShowDialog()==DialogResult.OK)
                        st.save(sv.FileName);
                }
            }

        }

        private void showOrSave(object sender, EventArgs e)
        {
            
            RadioButton rb = (RadioButton)sender;
            if (rb.Name.Equals("saveMsg"))
            {
                ExMsgTxt.Enabled = false;
                ExMsgTxt.ForeColor = System.Drawing.SystemColors.WindowFrame;
                ExMsgTxt.Text = "Your Message Will Appear Here";
                SaveFileDialog msgFile = new SaveFileDialog();
                msgFile.Filter = "Text Files|*.txt";
                msgFile.Title = "Select  File";

                if (msgFile.ShowDialog() == DialogResult.OK)
                {

                    msgFilePath = msgFile.FileName;
                }
            }
            else
            {
                ExMsgTxt.Enabled = true;
                ExMsgTxt.Text = "";
                ExMsgTxt.ForeColor = Color.Black;
            }
        }

        private void extract_Click(object sender, EventArgs e)
        {
            bool validPass = false;
            validPass = Int32.TryParse(passTxt2.Text, out password);

            
            if (stegoPath.Checked)
            {
                stegoImgPath = stegoPath.Text;
            }
            stegoImg = Image.FromFile(stegoImgPath);

            ///////////Embed Proc
            if (validPass  && stegoImg != null && (msgFilePath!="" || showMsg.Checked))
            {

               
                Steganography.Steganography st = new Steganography.Steganography( stegoImg, password, Int32.Parse(baseBox2.SelectedItem + ""));
                try
                {
                    st.ExtractProc();
                    string secretMsg = decompressed(st.GetMsg());
                    //secretMsg =(st.GetMsg());

                    if (showMsg.Checked)
                    {
                        ExMsgTxt.Text = secretMsg; ;

                    }
                    else
                    {
                        File.WriteAllText(msgFilePath, secretMsg);
                        
                    }
                    MessageBox.Show("Done");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            secretMsg = ""; extractedMsg = "";
            coverImgPath = ""; stegoImgPath = ""; msgFilePath = "";
            int password;
            bool passEntered = false;
            coverImg = null; stegoImg=null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileD = new OpenFileDialog();
            MessageBox.Show("brows the cover img");
            fileD.ShowDialog();
            coverImg = Image.FromFile(fileD.FileName);
            pictureBox1.Image = coverImg;
            
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            histogram h = new histogram();
            h.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileD = new OpenFileDialog();
            MessageBox.Show("brows the stego img");
            fileD.ShowDialog();
            stegoImg = Image.FromFile(fileD.FileName);
            pictureBox2.Image = stegoImg;
        }

        private void button5_Click(object sender, EventArgs e)
        {
        
            PSNR p = new PSNR(coverImg, stegoImg);
            textBox1.Text = p.getPSNR() + "";
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void passTxt2_TextChanged(object sender, EventArgs e)
        {
            passEntered = true;
            ExtractBtn.Enabled = true;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("assd");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            baseBox.SelectedItem = baseBox.Items[2];
            baseBox2.SelectedItem = baseBox2.Items[2];
            List<char> l = new List<char>();
            Helper.ConvertStrToB("abc", 5,out l );
        }

        private void stegoPath_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Name.Equals("browseStego"))
            {
                panel2.Enabled = false;
                OpenFileDialog imgFile = new OpenFileDialog();
               // imgFile.Filter = "Image Files (*.jpeg; *.png; *.bmp)|*.jpg; *.png; *.bmp";

                imgFile.Title = "Select Stego Image";

                if (imgFile.ShowDialog() == DialogResult.OK)
                {
                    stegoImgPath = imgFile.FileName;
                }
            }
            else
            {
                panel2.Enabled = true;
                stegoTxtB.Text = "";


            }
        }

        
    }
}
