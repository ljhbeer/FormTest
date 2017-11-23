using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using Tools;
using System.IO;
using System.Linq;
using System.Text;

namespace FormTest
{
	public partial class FormFullScreenYJ : Form
	{
		public FormFullScreenYJ(LoadBitmapData lbmd)
		{
			InitializeComponent();
            this._loadbmpdata = lbmd;
            subject sub = new subject("特征点左上", new Rectangle(50, 80, 250, 100));
			comboBox1.Items.Add(sub);
			Init();
		}
		private void Init(){
			_dgvsize = dgvs.ClientSize;
			_cntx = 1;
			_cnty = 1;
			_activesubject = null;	
			_itemsize = new Size(1,1);            
		}

		private void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex == -1) return;
			_activesubject =(subject)comboBox1.SelectedItem;
            _loadbmpdata.SetBitMapRectangle(_activesubject.Rect);
            textBoxShow.Text = "本题未完成阅卷份数" + _loadbmpdata.Count(true);
			
			_imgsize = _activesubject.Rect.Size;
            _itemsize.Width = _imgsize.Width / 3;
			_itemsize.Height = _imgsize.Height/3;
			_cntx = (dgvs.Size.Width-15) / _itemsize.Width;
			_cnty = (dgvs.Size.Height-30)/_itemsize.Height;
			InitDtshow(_cntx);
			InitDgvUI();
			
			YueJuan();
		}
		private void YueJuan()
        {
			ShowItemsInDgv();
        }    

		private void InitDtshow(int cntx){
			List<string> titles = new List<string>();		
			for(int x=0; x<cntx; x++){
				string xx = x.ToString();
				titles.Add("kh"+xx);
				titles.Add("图片"+xx);				
			}
			_dtshow = Tools.DataTableTools.ConstructDataTable(titles.ToArray());
            dgvs.DataSource = null;
			dgvs.DataSource = _dtshow;			
		}
		private void InitDgvUI()
		{
			int index = 0;
			foreach (DataGridViewColumn dc in dgvs.Columns) {
				if (dc.Name.Contains("分")) {
					dgvs.Columns[index].Width = 27;
					if(!dc.Name.EndsWith("分"))
						dgvs.Columns[index].HeaderText = dc.Name.Substring(0,dc.Name.Length-1);
				} else if (dc.Name.ToUpper().Contains( "KH")) {
					;
					dgvs.Columns[index].Visible = false;
				} else if (dc.Name.Contains("图片")) {
					((DataGridViewImageColumn)(dgvs.Columns[index])).ImageLayout = DataGridViewImageCellLayout.Zoom;
					dc.Width = _imgsize.Width / 3;
				}
				index++;
			}
			dgvs.RowTemplate.Height = _imgsize.Height / 3;
		}
		private void ShowItemsInDgv(){
			int cntx = _cntx;
			int cnty = _cnty;
			_dtshow.Rows.Clear();
			string kh="";
			for(int x=0; x<cntx; x++){
				string xx = x.ToString();
				for(int y=0; y<cnty; y++){
                    kh = _loadbmpdata.GetNextKh();
					if(kh=="" ) break;
					if(x==0){
						DataRow drt = _dtshow.NewRow();
						_dtshow.Rows.Add(drt);
					}
					DataRow dr = _dtshow.Rows[y];
					dr["kh"+xx] = kh;
					dr["图片"+xx] = _loadbmpdata.GetBitmap(kh);
				}				
				if(kh=="") break;
			}			
		}
        private void buttonSubmitMulti_Click(object sender, EventArgs e)
        {
            _loadbmpdata.GoHead();
            ShowItemsInDgv();
        }
        private void buttonTest_Click(object sender, EventArgs e)
        {
            //_loadbmpdata.Test(
        }
        private void dgvs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                Bitmap bmp = (Bitmap)(dgvs[e.ColumnIndex, e.RowIndex].Value);
                Bitmap b =  Test(bmp);
                if (b != null)
                    dgvs[e.ColumnIndex, e.RowIndex].Value = b;
                else
                {
                    MessageBox.Show("检测失败");
                }
            }
        }

        private Bitmap Test(Bitmap bmp)
        {
            Rectangle rect = DetectFeatureRectAngle(bmp);
            if (rect.Width == 1 || rect.Height == 1)
            {
                MessageBox.Show("检测失败");
                return null;
            }
           
            Rectangle rect2 = DetectFeatureRectAngle2(bmp, rect);
            if (rect2.Width == 1 || rect2.Height == 1)
            {
                MessageBox.Show("检测2失败");
                return null;
            }
            if (rect2.Width != rect.Width || rect2.Height != rect.Height)
            {
                MessageBox.Show("检测3错误");

                //进一步精确检测 
                Size minsize = new Size(30, 30);
                int perheight = minsize.Height * 8 / 10;
                int maxlen = minsize.Width * 9 / 10;
                Rectangle outrect = new Rectangle();
                bool suss = false;
                for (int y = perheight; y < bmp.Height; y += perheight)
                {
                    Rectangle r = new Rectangle(0, y, bmp.Width, 2);
                    suss = DetectFeatureRectAngle3(bmp, r, maxlen, out outrect);
                    if (suss)
                        break;
                }
                if (!suss)
                    return null;
                {
                    int x = outrect.X + outrect.Width / 2;
                    Rectangle r = new Rectangle(x, 0, 2, bmp.Height);
                    maxlen = minsize.Height * 9 / 10;
                    bool su = DetectFeatureRectAngle4(bmp, r, maxlen, ref outrect);
                    if (!su)
                        return null;
                    rect = outrect;
                }
            }
            MessageBox.Show(Recttostring(rect) + Recttostring(rect2));
            Bitmap b = bmp.Clone(rect, bmp.PixelFormat);
            //b.Palette = bmp.Palette;
            //b.Save("test.tiff");
            //BitMapTo01Map(bmp, rect);

            return b;
        }
        private bool DetectFeatureRectAngle4(Bitmap bmp, Rectangle r, int maxlen, ref Rectangle outrect)
        {
            Rectangle rectyline = r;
            int[] yycnt = new int[r.Width];
            BitmapTools.CountYPixsum(bmp, rectyline, out yycnt);
            List<int> ycnt = yycnt.Select(rec => 2 - rec).ToList();

            //count Xpoint
            int Ypoint = 0;
            int len = 0;
            int yblackcnt = 0;
            int Ylen = 1;
            for (int i = 0; i < ycnt.Count; i++)
            {
                if (ycnt[i] > yblackcnt)
                {
                    if (len == 0)
                        Ypoint = i;
                    len++;
                }
                else
                {
                    if (len >= maxlen)
                    {
                        Ylen = len;
                        break;
                    }
                    else
                        len = 0;
                }
            }
            if (len > Ylen)
                Ylen = len;

            outrect.Y = Ypoint;
            outrect.Height = Ylen;
            if (Ylen >= maxlen)
                return true;
            return false;
        }
        private bool DetectFeatureRectAngle3(Bitmap bmp, Rectangle r, int maxlen, out Rectangle outrect)
        {
            Rectangle rectxline = r;
            int[] xxcnt = new int[r.Width];
            BitmapTools.CountXPixsum(bmp, rectxline, out xxcnt);
            List<int> xcnt = xxcnt.Select(rec => 2 - rec).ToList();

            //count Xpoint
            int Xpoint = 0;
            int len = 0;
            int xblackcnt = 0;
            int Xlen = 1;
            for (int i = 0; i < xcnt.Count; i++)
            {
                if (xcnt[i] > xblackcnt)
                {
                    if (len == 0)
                        Xpoint = i;
                    len++;
                }
                else
                {
                    if (len >= maxlen)
                    {
                        Xlen = len;
                        break;
                    }
                    else
                        len = 0;
                }
            }
            if (len > Xlen)
                Xlen = len;

            outrect = new Rectangle(Xpoint, 0, Xlen, 2);
            if(Xlen>= maxlen)
                return true;
            return false;
        }
        private Rectangle DetectFeatureRectAngle2(Bitmap bmp,Rectangle rect) //由图片限定
        {
            Rectangle rectxline = new Rectangle(0, rect.Height / 2, rect.Width, 2);
            Rectangle rectyline = new Rectangle(rect.Width / 2, 0, 2, rect.Height);
            rectxline.Offset(rect.Location);
            rectyline.Offset(rect.Location); 

            int[] xxcnt = new int[rect.Width];
            int[] yycnt = new int[rect.Height];
            BitmapTools.CountXPixsum(bmp, rectxline, out xxcnt);
            BitmapTools.CountYPixsum(bmp, rectyline, out yycnt);
            List<int> xcnt = xxcnt.Select(rec => 2 - rec).ToList();
            List<int> ycnt = yycnt.Select(rec => 2 - rec).ToList();


            //count Xpoint
            int Xpoint = 0;
            int len = 0;
            int xblackcnt = 0;
            int yblackcnt = 0;
            int Xlen = 1;
            int Ylen = 1;
            for (int i = 0; i < xcnt.Count; i++)
            {
                if (xcnt[i] > xblackcnt)
                {
                    if (len == 0)
                        Xpoint = i;
                    len++;
                }
                else
                {
                    if (len > yblackcnt)
                    {
                        Xlen = len;
                        break;
                    }
                    else
                        len = 0;
                }
            }
            if (len > Xlen)
                Xlen = len;

            int Ypoint = 0;
            len = 0;
            for (int i = 0; i < ycnt.Count; i++)
            {
                if (ycnt[i] > yblackcnt)
                {
                    if (len == 0)
                        Ypoint = i;
                    len++;
                }
                else
                {
                    if (len > xblackcnt)
                    {
                        Ylen = len;
                        break;
                    }
                    else
                        len = 0;
                }
            }
            if (len > Ylen)
                Ylen = len;
            //string str = string.Join(",", xxcnt) + "\r\n" + string.Join(",", yycnt);
            //File.WriteAllText("a.txt", str);
            return new Rectangle(Xpoint, Ypoint, Xlen, Ylen);            
        }
        private Rectangle DetectFeatureRectAngle(Bitmap bmp) //由图片限定
        {
            Size blacktag = new Size(33, 33);
            Rectangle r = new Rectangle(0, 0, bmp.Width, bmp.Height);
            int[] xxcnt = new int[r.Width];
            int[] yycnt = new int[r.Height];
            BitmapTools.CountXPixsum(bmp, r, out xxcnt);
            BitmapTools.CountYPixsum(bmp, r, out yycnt);

            List<int> xcnt = xxcnt.Select(rec =>
            {
                if ((r.Height - rec) > blacktag.Height / 3)
                    return r.Height - rec;
                return 0;
            }).ToList();
            List<int> ycnt = yycnt.Select(rec =>
            {
                if ((r.Width - rec) > blacktag.Width / 3)
                    return r.Width - rec;
                return 0;
            }).ToList();

            //count Xpoint
            int Xpoint = 0;
            int len = 0;
            int xblackcnt = blacktag.Height * 2 / 3;
            int yblackcnt = blacktag.Width * 2 / 3;
            int Xlen = 1;
            int Ylen = 1;
            for (int i = 0; i < xcnt.Count; i++)
            {
                if (xcnt[i] > xblackcnt)
                {
                    if (len == 0)
                        Xpoint = i;
                    len++;
                }
                else
                {
                    if (len > yblackcnt)
                    {
                        Xlen = len;
                        break;
                    }
                    else
                        len = 0;
                }
            }

            int Ypoint = 0;
            len = 0;
            for (int i = 0; i < ycnt.Count; i++)
            {
                if (ycnt[i] > yblackcnt)
                {
                    if (len == 0)
                        Ypoint = i;
                    len++;
                }
                else
                {
                    if (len > xblackcnt)
                    {
                        Ylen = len;
                        break;
                    }
                    else
                        len = 0;
                }
            }
            //string str = string.Join(",", xxcnt) + "\r\n" + string.Join(",", yycnt);
            //File.WriteAllText("a.txt", str);
            Rectangle rect = new Rectangle(Xpoint, Ypoint, Xlen, Ylen);
            return rect;
        }
        private static void BitMapTo01Map(Bitmap bmp, Rectangle rect)
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < rect.Height; y++)
            {
                for (int x = 0; x < rect.Width; x++)
                {
                    Color c = bmp.GetPixel(x + rect.X, y + rect.Y);
                    int cv = c.ToArgb();
                    if (cv == Color.White.ToArgb())
                        sb.Append("0");
                    else if (cv == Color.Black.ToArgb())
                        sb.Append("1");
                    else
                        sb.Append(".");
                }
                sb.AppendLine();
            }
            File.WriteAllText("bmp电子图.txt", sb.ToString());

        }
        private string  Recttostring(Rectangle r )
        {
            return "(" + r.X + "," + r.Y + "," + r.Width + "," + r.Height + ")";
        }

        private subject _activesubject;
        // for multiyj
        private Size _dgvsize;
        private Size _itemsize;
        private Size _imgsize;
        private DataTable _dtshow;
        //for layout
        public LoadBitmapData _loadbmpdata;
        private int _cnty;
        private int _cntx;



    }
}
