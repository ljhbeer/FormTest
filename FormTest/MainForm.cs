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
			textBoxWorkPath.Text = @"E:\Project\SWAR\back\C_IMAGES\200001_00_1_p1.TIF";
		}
		
		void ButtonTestClick(object sender, EventArgs e)
		{
			string imgfilename = textBoxWorkPath.Text;
            List<string> namelist = NameList( imgfilename );
            namelist = namelist.Take(1).ToList();

            Rectangle imgrect = new Rectangle(1904,634,392,143);            
            if (File.Exists("img.data"))
                File.Delete("img.data");
            System.DateTime dt1 = System.DateTime.Now;
            List<datainfo> di = ConstructImgData1(namelist, ref imgrect);
            System.DateTime dt2 = System.DateTime.Now;
            System.TimeSpan ts = dt2 - dt1;
            MessageBox.Show("耗时"+ts.Minutes*60+ts.Seconds+"."+ts.Milliseconds*1.0/1000.0+"秒");
            ReadBitmap2(di);
		}

        private static List<datainfo> ConstructImgData1(List<string> namelist, ref Rectangle imgrect)
        {
            FileStream fs = new FileStream("img.data", FileMode.Append, FileAccess.Write);  
            BufferedStream bs = new BufferedStream(fs, 40960);

            Rectangle bigrect = new Rectangle( 1800, 600,  2712,  1629);
            int startpos = 0;
            List<datainfo> di = new List<datainfo>();
            for (int cnt = 0; cnt < namelist.Count; cnt++)
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(namelist[cnt]);
                Bitmap bmpc = bmp.Clone(imgrect, bmp.PixelFormat);
                bmpc.Save("bmpc.tif");
                BitmapData bmpdata = bmp.LockBits(imgrect, ImageLockMode.ReadOnly, bmp.PixelFormat);
                BitmapData bmpcdata = bmpc.LockBits(new Rectangle(0, 0, imgrect.Width, imgrect.Height), ImageLockMode.ReadOnly, bmpc.PixelFormat);

                unsafe
                {
                    byte* bmpPtr = (byte*)bmpdata.Scan0.ToPointer();
                    byte* bmpcPtr = (byte*)bmpcdata.Scan0.ToPointer();

                    byte[] buff = new byte[bmpcdata.Stride * bmpcdata.Height];
                   
                    int cols = bmpdata.Width / 8;
                    for (int y = 0; y < bmpdata.Height; y++)
                    {
                        for (int i = 0; i < cols; i++)
                        {
                            if (bmpPtr[i] != bmpcPtr[i])
                            {
                                MessageBox.Show("数据不一致");
                            }
                            buff[y * bmpcdata.Stride + i] = bmpcPtr[i];
                        }
                        bmpPtr += bmpdata.Stride;
                        bmpcPtr += bmpcdata.Stride;
                    }

                    bs.Write(buff, 0, buff.Length);
                }
                MessageBox.Show("bmptr 与 bmpcPtr一致");


                bmp.UnlockBits(bmpdata);
                bmpc.UnlockBits(bmpcdata);
                
                di.Add(new datainfo(namelist[cnt], startpos, bs.Length));
                startpos += (int)bs.Length;
                //if (buff.Length != bs.Length)
                //{
                //    MessageBox.Show("Error: buff.length=" + buff.Length + ",bs.length=" + bs.Length);
                //}
            }
            bs.Flush();
            fs.Flush();
            bs.Close();
            fs.Close();
            return di;
        }
        private static List<datainfo> ConstructImgData2(List<string> namelist, ref Rectangle imgrect) //Test bitmapdata.Lockbit
        {
            FileStream fs = new FileStream("img.data", FileMode.Append, FileAccess.Write);
            BufferedStream bs = new BufferedStream(fs, 40960);

            Rectangle bigrect = new Rectangle(1800, 600, 2712, 1629);
            int startpos = 0;
            List<datainfo> di = new List<datainfo>();
            for (int cnt = 0; cnt < namelist.Count; cnt++)
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(namelist[cnt]);
                Bitmap bmpc = bmp.Clone(imgrect, bmp.PixelFormat);

                BitmapData bmpalldata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
                BitmapData bmpcdata = bmpc.LockBits(new Rectangle(0, 0, imgrect.Width, imgrect.Height), ImageLockMode.ReadOnly, bmpc.PixelFormat);

                unsafe  // Format1bppIndexed
                {
                    byte* bmpallPtr = (byte*)bmpalldata.Scan0.ToPointer();
                    byte* bmpcPtr = (byte*)bmpcdata.Scan0.ToPointer();
                   
                    bmpallPtr += imgrect.Y * bmpalldata.Stride + imgrect.X/8;
                    for (int y = 0; y < bmpcdata.Height; y++)
                    {
                        for (int i = 0; i < bmpcdata.Width / 8; i++)
                        {
                            if (bmpallPtr[i] != bmpcPtr[i])
                            {
                                MessageBox.Show("数据不一致");
                            }
                        }
                        bmpallPtr += bmpalldata.Stride;
                        bmpcPtr += bmpcdata.Stride;
                    }
                }
                    MessageBox.Show(cnt + " bmpallptr 与bmpcPtr 一致");

                bmp.UnlockBits(bmpalldata);
                bmpc.UnlockBits(bmpcdata);

                di.Add(new datainfo(namelist[cnt], startpos, bs.Length));
                startpos += (int)bs.Length;
                //if (buff.Length != bs.Length)
                //{
                //    MessageBox.Show("Error: buff.length=" + buff.Length + ",bs.length=" + bs.Length);
                //}
            }

            bs.Flush();
            fs.Flush();
            bs.Close();
            fs.Close();
            return di;
        } 
        private static List<datainfo> ConstructImgData3(List<string> namelist, ref Rectangle imgrect)
        {
            List<datainfo> di = new List<datainfo>();
            for (int cnt = 0; cnt < namelist.Count; cnt++)
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(namelist[cnt]);
                Bitmap bmpc = bmp.Clone(imgrect, bmp.PixelFormat);
                BitmapData bmpalldata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
                BitmapData bmpcdata = bmpc.LockBits(new Rectangle(0, 0, imgrect.Width, imgrect.Height), ImageLockMode.ReadOnly, bmpc.PixelFormat);
                unsafe  // Format1bppIndexed
                {
                    byte* bmpallPtr = (byte*)bmpalldata.Scan0.ToPointer();
                    byte* bmpcPtr = (byte*)bmpcdata.Scan0.ToPointer();

                    bmpallPtr += imgrect.Y * bmpalldata.Stride + imgrect.X / 8;
                    for (int y = 0; y < bmpcdata.Height; y++)
                    {
                        for (int i = 0; i < bmpcdata.Width / 8; i++)
                        {
                            if (bmpallPtr[i] != bmpcPtr[i])
                            {
                                MessageBox.Show("数据不一致");
                            }
                        }
                        bmpallPtr += bmpalldata.Stride;
                        bmpcPtr += bmpcdata.Stride;
                    }
                }
                bmp.UnlockBits(bmpalldata);
                bmpc.UnlockBits(bmpcdata);
            }
            return di;
        }
        private static void ReadBitmap1(List<datainfo> di)
        {
            FileStream fs = new FileStream("img.data", FileMode.Open, FileAccess.Read);
            BufferedStream bs = new BufferedStream(fs, 4096);
            for (int i = di.Count - 1; i >= 0; i--)
            {
                byte[] buffer = new byte[di[i].count];
                bs.Position = di[i].startpos;
                bs.Read(buffer, 0, buffer.Length);

                Stream sss = new MemoryStream();
                sss.Write(buffer, 0, buffer.Length);
                Bitmap imgc = (Bitmap)Bitmap.FromStream(sss);
                imgc.Save(i + "_.tif");
                sss.Dispose();
                sss.Close();
            }
            bs.Close();
            fs.Close();
        }
        private static void ReadBitmap2(List<datainfo> di)
        {
            FileStream fs = new FileStream("img.data", FileMode.Open, FileAccess.Read);
            BufferedStream bs = new BufferedStream(fs, 4096);
            for (int i = di.Count - 1; i >= 0; i--)
            {
                byte[] buffer = new byte[di[i].count];
                bs.Position = di[i].startpos;
                bs.Read(buffer, 0, buffer.Length);
                unsafe{
                    fixed( byte* p =  &buffer[0])
                    {
                        Bitmap Bmp = new Bitmap(392, 143, 52, PixelFormat.Format1bppIndexed, (IntPtr)p);
                        Bmp.Save("load.tif");
                    }
                }
                //Stream sss = new MemoryStream();
                //sss.Write(buffer, 0, buffer.Length);
                //Bitmap imgc = (Bitmap)Bitmap.FromStream(sss);
                //imgc.Save(i + "_.tif");
                //sss.Dispose();
                //sss.Close();
            }
            bs.Close();
            fs.Close();
        }		

        ///////////
        private static List<datainfo> ConstructImgData(List<string> namelist, ref Rectangle imgrect)
        {
            //FileStream fs = new FileStream("img.data", FileMode.Append, FileAccess.Write);
            MemoryStream ms = new MemoryStream();
            BufferedStream bs = new BufferedStream(ms, 40960);

            int startpos = 0;
            List<datainfo> di = new List<datainfo>();
            for (int cnt = 0; cnt < namelist.Count; cnt++)
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(namelist[cnt]);
                Bitmap imgc = bmp.Clone(imgrect, bmp.PixelFormat);
                imgc.Save(bs, System.Drawing.Imaging.ImageFormat.Tiff);
                bs.Flush();
                byte[] buff = ms.ToArray();
                //fs.Write(buff, 0, buff.Length);

                //di.Add(new datainfo(namelist[cnt], startpos, bs.Length));
                //startpos += (int)bs.Length;

            }
            ms.Flush();
            bs.Flush();
            ms.Close();
            bs.Close();
            //fs.Flush();
            //fs.Close();
            return di;
        }
        private static void ReadBitmap(List<datainfo> di)
        {
            FileStream fs = new FileStream("img.data", FileMode.Open, FileAccess.Read);
            BufferedStream bs = new BufferedStream(fs, 4096);
            for (int i = di.Count - 1; i >= 0; i--)
            {
                byte[] buffer = new byte[di[i].count];
                bs.Position = di[i].startpos;
                bs.Read(buffer, 0, buffer.Length);

                Stream sss = new MemoryStream();
                sss.Write(buffer, 0, buffer.Length);
                Bitmap imgc = (Bitmap)Bitmap.FromStream(sss);
                imgc.Save(i + "_.tif");
                sss.Dispose();
                sss.Close();
            }
            bs.Close();
            fs.Close();
        }		
		private List<string> NameList(string filename){        	 
        	List<string> namelist = new List<string>();
        	if (File.Exists(filename)){
	        	FileInfo fi = new FileInfo(filename);
	            DirectoryInfo dirinfo = fi.Directory;
	            string ext = fi.Extension;	            
	            foreach (FileInfo f in dirinfo.GetFiles())
	            	if (f.Extension.ToLower() == ".tif")
	            		namelist.Add( f.FullName);    
        	}
        	return namelist;
        }
        public class datainfo
        {
            public datainfo(string kh, int startpos, long count)
            {
                this.kh = kh;
                this.startpos = startpos;
                this.count = count;
            }
            public string kh;
            public int startpos;
            public long count;

            public override string ToString()
            {
                return kh + " " + startpos + " " + count;
            }
        }
	}
	
}
