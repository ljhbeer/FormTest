﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Tools;
using System.Windows.Forms;

namespace ScanTemplate
{
    class MyDetectFeatureRectAngle
    {
        public Rectangle CorrectRect { get; set; }
        public MyDetectFeatureRectAngle(System.Drawing.Bitmap bmp)
        {
            this._src = bmp;
            _listsubjects = new List<subject>(){
                new subject("左上",new  Rectangle(50, 80, 250, 100)),
                new subject("右上",new  Rectangle(bmp.Width-300, 80, 300, 100)),
                new subject("左下",new  Rectangle(50, bmp.Height-280, 250, 100)) };
            _listFeatureRectangles = new List<Rectangle>();
            foreach (subject sub in _listsubjects)
            {
                Rectangle r = DetectFeatureRect(sub); 
                if (r.Width == 0)
                    break;
                _listFeatureRectangles.Add(r);
            }
            if (_listFeatureRectangles.Count != 3)
            {
                MessageBox.Show("特征点检测失败");
                return;
            }
            
            CorrectRect = new Rectangle( _listFeatureRectangles[0].Location, 
                new Size( _listFeatureRectangles[1].Right-_listFeatureRectangles[0].Left,
                    _listFeatureRectangles[2].Bottom - _listFeatureRectangles[0].Top));

            //Bitmap newbmp = (Bitmap)_src.Clone(CorrectRect, _src.PixelFormat);
            //newbmp.Save("correct.tif");
           
        }
        public bool Detected()
        {
            return CorrectRect.Width > 0;
        }
        private Rectangle DetectFeatureRect(subject sub)
        {
            Size minsize = new Size(30, 30);
            Bitmap bmp =_src.Clone(sub.Rect, _src.PixelFormat);

            bmp.Save(sub.ToString() + ".tif");
            Rectangle rect = DetectFeatureRectAngle(bmp);
            if (rect.Width == 1 || rect.Height == 1)
                return new Rectangle();
            Rectangle rect2 = DetectFeatureRectAngle2(bmp, rect);
            if (rect2.Width == 1 || rect2.Height == 1)
                return new Rectangle();
            if(rect2.Width == rect.Width && rect2.Height == rect.Height){
                rect.Offset(sub.Rect.Location);
                return rect;
            }

            if (rect2.Width != rect.Width || rect2.Height != rect.Height)
            {
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
                if (suss)
                {
                    int x = outrect.X + outrect.Width / 2;
                    Rectangle r = new Rectangle(x, 0, 2, bmp.Height);
                    maxlen = minsize.Height * 9 / 10;
                    bool su = DetectFeatureRectAngle4(bmp, r, maxlen, ref outrect);
                    if (su)
                        return outrect;
                }
            }
            return new Rectangle();
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
            if (Xlen >= maxlen)
                return true;
            return false;
        }
        private Rectangle DetectFeatureRectAngle2(Bitmap bmp, Rectangle rect) //由图片限定
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
        private string Recttostring(Rectangle r)
        {
            return "(" + r.X + "," + r.Y + "," + r.Width + "," + r.Height + ")";
        }

        private System.Drawing.Bitmap _src;
        private List<subject> _listsubjects;
        private List<Rectangle> _listFeatureRectangles;
    }
}
