using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


            return "暂未实现" + sb.ToString();
        }
    }
}
