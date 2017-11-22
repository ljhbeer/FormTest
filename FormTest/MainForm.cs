using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace FormTest
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
            textBoxWorkPath.Text = @"E:\Temp\JPG\694.jpg"; //468.jpg  697.jpg
		}		
		void ButtonTestClick(object sender, EventArgs e)
		{
            System.DateTime dt1 = System.DateTime.Now;

            FormatJpgToTif();

            System.DateTime dt2 = System.DateTime.Now;
            System.TimeSpan ts = dt2 - dt1;
            MessageBox.Show("耗时"+ts.Minutes*60+ts.Seconds+"."+ts.Milliseconds*1.0/1000.0+"秒");
		}

        private void FormatJpgToTif()
        {
            string imgfilename = textBoxWorkPath.Text;
            Bitmap bmp =(Bitmap) Bitmap.FromFile(imgfilename);

            //Tools.BitmapTools.Gray(bmp);
            Rectangle r = new Rectangle(0, 0, bmp.Width, bmp.Height);
            Rectangle r1 = new Rectangle(0, 0, bmp.Width / 2, bmp.Height);
            int gramm = Tools.BitmapTools.GetstatisticGamma(bmp, 5, r1);

            Bitmap bmpdst = new Bitmap(r.Width, r.Height);
            Tools.BitmapTools.GammaImg(bmpdst, bmp, gramm, r);







            bmpdst.Save( imgfilename.Replace(".jpg","_gramm_1.jpg"));

            MessageBox.Show("OK");
        }

	}
	
}
