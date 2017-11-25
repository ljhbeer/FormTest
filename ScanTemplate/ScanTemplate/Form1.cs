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
            string filename = nameList[0];
            TestAndCreateTemplate( filename,nameList);
        }
        private void TestAndCreateTemplate( string filename, List<string> namelist=null)
        {
            Bitmap bmp = (Bitmap)Bitmap.FromFile(filename);
            MyDetectFeatureRectAngle dr = new MyDetectFeatureRectAngle(bmp);

            if (dr.Detected())
            {
                ARTemplate.Template art = new ARTemplate.Template(filename, bmp, dr.CorrectRect);

                this.Hide();
                ARTemplate.FormTemplate f = new ARTemplate.FormTemplate(art);
                f.ShowDialog();
                this.Show();
                if(namelist!=null)
                DetectAllImgs(dr, namelist);
            }
        }
        private void DetectAllImgs(MyDetectFeatureRectAngle dr,List<string> nameList)
        {
            FileInfo fi = new FileInfo(nameList[0]);
            string dir = fi.Directory.FullName.Replace("LJH\\", "LJH\\Correct\\");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            foreach (string s in nameList)
            {
                DetectAllImg(dr, s);
            }
            //File.WriteAllText("allimport.txt", sb.ToString());
        }

        private void DetectAllImg(MyDetectFeatureRectAngle dr, string s)
        {
            Bitmap bmp1 = (Bitmap)Bitmap.FromFile(s);
            //MyDetectFeatureRectAngle dr = new MyDetectFeatureRectAngle(bmp);
            Rectangle CorrectRect = dr.Detected(bmp1);
            StringBuilder sb = new StringBuilder();
            sb.Append(s + "," + Recttostring(CorrectRect) + ",");
            if (CorrectRect.Width > 0)
            {
                bmp1 = (Bitmap)bmp1.Clone(CorrectRect, bmp1.PixelFormat);
                bmp1.Save(s.Replace("LJH\\", "LJH\\Correct\\"));
                bmp1.Dispose();
                bmp1 = null;
                //统计选择题
            }
            else
            {
                //检测失败
            }
            sb.AppendLine();
            //MessageBox.Show(sb.ToString());
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            string path = listBox1.SelectedItem.ToString();
            listBox2.Items.Clear();
            listBox2.Items.AddRange(NameListFromDir(path) .ToArray());
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBox2.SelectedIndex == -1) return;
            string filename = listBox2.SelectedItem.ToString();
            if (checkBoxDetectImg.Checked)
            {
                Bitmap bmp1 = (Bitmap)Bitmap.FromFile(listBox2.Items[0].ToString());
                MyDetectFeatureRectAngle dr = new MyDetectFeatureRectAngle(bmp1);
                if (dr.Detected())
                {
                    DetectAllImg(dr, listBox2.SelectedItem.ToString());
                }
            }
            else
            {
                TestAndCreateTemplate(filename);
            }
        }

        private string Recttostring(Rectangle r)
        {
            return "(" + r.X + "," + r.Y + "," + r.Width + "," + r.Height + ")";
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
