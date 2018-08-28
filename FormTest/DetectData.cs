using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FormTest
{
    public class DetectData
    {
        public List<System.Drawing.Rectangle> ListFeature { get; set; }
        public Rectangle CorrectRect { get; set; }

        public bool Detected { get; set; }
    }
}
