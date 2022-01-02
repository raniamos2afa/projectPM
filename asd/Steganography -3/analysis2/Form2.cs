using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace analysis2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string imgPath = "";
            Bitmap img;
            MessageBox.Show("Select Image");
            openFileDialog1.Title = "Select Image ";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                imgPath = openFileDialog1.FileName;
            }
           img =new Bitmap( Image.FromFile(imgPath).GetGrayScaleVersion());
          
            Bitmap bmpHist = img.GetGrayHistogramBitmap();
            //save it
            imgPath= imgPath.Remove(imgPath.IndexOf('.'), imgPath.Length - imgPath.IndexOf('.'));
            imgPath += "Histogram.png";
            bmpHist.Save(imgPath);
        }
    }
}
