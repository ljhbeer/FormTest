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
using System.Text.RegularExpressions;

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
            //FormatJpgToTif();
            //AutoLoadLatestImg();
            AutoShowTZD();

            System.DateTime dt2 = System.DateTime.Now;
            System.TimeSpan ts = dt2 - dt1;
            MessageBox.Show("耗时"+(ts.Minutes*60+ts.Seconds + ts.Milliseconds*1.0/1000.0)+"秒");
		}

        private void AutoShowTZD(string path = "E:\\Scan\\LJH\\s1025")
        {

            string latestpath = GetLastestSubDirectory(path);
            if (latestpath != "")
            {
                this.Hide();
                LoadBitmapData lbmd = new LoadBitmapData(latestpath);
                FormFullScreenYJ fs = new FormFullScreenYJ(lbmd);
                fs.ShowDialog();
                this.Show();
            }
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
        private void AutoLoadLatestImg(string path = "E:\\Scan\\LJH\\s1025")
        {
            string latestpath = GetLastestSubDirectory(path);
            if (latestpath != "")
            {
                List<string> nameList = NameListFromDir(latestpath);

                string msg = "共有文件" + nameList.Count + "个" +  string.Join("\r\n", nameList);
                MessageBox.Show(msg);
            }
        }
        private static string GetLastestSubDirectory(string path)
        {
            Regex r = new Regex("[0-9]{8}-[0-9]+");
            if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                List<string> sudirects = new List<string>();
                foreach (DirectoryInfo d in dir.GetDirectories())
                {
                    if (r.IsMatch(d.Name))
                    {
                        sudirects.Add(d.FullName);
                    }
                }
                if (sudirects.Count > 0)
                    return sudirects.Max();
            };
                return "";
        }
        private List<string> NameListFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                FileInfo fi = new FileInfo(filename);
                string fidir = fi.Directory.FullName;
                return NameListFromDir( fidir);
            }
            return new List<string>();
        }
        public static List<string> NameListFromDir(string fidir)
        {
            List<string> namelist = new List<string>();
            DirectoryInfo dirinfo = new DirectoryInfo(fidir);
            //string ext = fi.Extension;
            foreach (FileInfo f in dirinfo.GetFiles())
                if (f.Extension.ToLower() == ".tif")
                    namelist.Add(f.FullName);
            return namelist;
        }

	}
	
}
