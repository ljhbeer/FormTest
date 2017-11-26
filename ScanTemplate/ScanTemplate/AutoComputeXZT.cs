using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARTemplate;
using System.Drawing.Imaging;
using System.Drawing;
namespace ScanTemplate
{
    class AutoComputeXZT
    {
        private ARTemplate.Template _artemplate;
        private AutoAngle _angle;
        private System.Drawing.Bitmap _src;

        public AutoComputeXZT(ARTemplate.Template _artemplate, AutoAngle _angle, System.Drawing.Bitmap bmp)
        {            
            this._artemplate = _artemplate;
            this._angle = _angle;
            this._src = bmp;
        }

        public string  ComputeXZT()
        {
            // TODO: ComputeXZT  unComplete
            StringBuilder sb = new StringBuilder();
            foreach(SingleChoiceArea sca in _artemplate.SingleChoiceAreas)
            {
            	Point L = sca.imgselection.Location;            
            	Point nL = _angle.GetCorrectPoint( L.X,L.Y);
            	((Bitmap)_src.Clone(new Rectangle(nL,sca.ImgSelection().Size),_src.PixelFormat)).Save( "f:\\1.jpg");
            	
            	Point offset = nL;
            	offset.Offset(-L.X,-L.Y);   
            	Rectangle r = sca.imgselection;
            	((Bitmap)_src.Clone(r,_src.PixelFormat)).Save( "f:\\2.jpg");
            	
            	r.Offset(offset);
            	((Bitmap)_src.Clone(r,_src.PixelFormat)).Save( "f:\\3.jpg");
            	
            	Bitmap b = (Bitmap)_src.Clone(r,_src.PixelFormat);
            	//TODO:debug
            	BitmapData bmpdata = _src.LockBits(r,ImageLockMode.ReadOnly,_src.PixelFormat);
            	Rectangle rp = new Rectangle(0,0,sca.size.Width,sca.size.Height);            	
            	foreach(List<Point> lp in sca.list){
            		List<int> blackpixs = new List<int>();
            		foreach(Point p in lp){
            			rp.Location = p;
            			blackpixs.Add( CountBlackPixs( bmpdata, rp));
            		}
            		// 计算涂卡结果
            		sb.Append( GetOptions(blackpixs)+",");
            	}
            	_src.UnlockBits(bmpdata);
            }
            return "暂未实现" + sb.ToString();
        }
        public int CountBlackPixs(BitmapData bmpdata, Rectangle  rp){
        	return 0;
        }
        public string GetOptions(List<int> blackpixs){
        
        	return "X";
        }
    }
}
