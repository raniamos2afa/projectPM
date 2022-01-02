using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

                return (decompressed);


            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            
            File.WriteAllText(openFileDialog1.FileName.Remove(openFileDialog1.FileName.IndexOf('.'),4)+"Comp.txt", compression(File.ReadAllText(openFileDialog1.FileName)));
        }
    }
}
