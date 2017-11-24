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

namespace ScanTemplate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _workpath = textBoxWorkPath.Text;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //AutoLoadLatestImg(_workpath);
            foreach (string s in GetLastestSubDirectorys(_workpath))
            {
                //TODO: 使用类，显示相关信息
                listBox1.Items.Add(s); 
            }
        }

        private void buttonworkpath_Click(object sender, EventArgs e)
        {
            string str = textBoxWorkPath.Text;
            if (Directory.Exists(str))
                _workpath = textBoxWorkPath.Text;
        }
        private void buttonGo_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            string path = listBox1.SelectedItem.ToString();

            List<string> nameList = NameListFromDir( path);
            if(nameList.Count==0) return;

            //string msg = "共有文件" + nameList.Count + "个" + string.Join("\r\n", nameList);
            //MessageBox.Show(msg);
            //TODO：检测是否存在模板文件
            Bitmap bmp =(Bitmap) Bitmap.FromFile( nameList[0]);
            MyDetectFeatureRectAngle dr = new MyDetectFeatureRectAngle(bmp);

            if (dr.Detected())
            {
                ARTemplate.Template art = new ARTemplate.Template(nameList[0], bmp, dr.CorrectRect);

                this.Hide();
                ARTemplate.FormTemplate f = new ARTemplate.FormTemplate(art);
                f.ShowDialog();
                this.Show();
            }
        }

       
        private static string GetLastestSubDirectory(string path)
        {
            List<string> sudirects = GetLastestSubDirectorys(path);               
            if (sudirects.Count > 0)
                return sudirects.Max();
            return "";
        }
        private static List<string> GetLastestSubDirectorys(string path)
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
                return sudirects;
            };
            return new List<string>();
        }

        private List<string> NameListFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                FileInfo fi = new FileInfo(filename);
                string fidir = fi.Directory.FullName;
                return NameListFromDir(fidir);
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
        private string _workpath;
    }
}
