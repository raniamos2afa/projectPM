using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteganographyProc
{
    public partial class histogram : Form
    {
        public histogram()
        {
            InitializeComponent();
        }

        private void histogram_Load(object sender, EventArgs e)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic["asd"] = 5;
            MessageBox.Show(dic["asd"]+"");
            //    Random ramd = new Random();
            //    int[] arr = new int[500];
            //    for (int i = 0; i < 500; i++)
            //        arr[i] = ramd.Next(0, 255);
            //    chart1.DataSource=(arr);
            
        }
    }
}
