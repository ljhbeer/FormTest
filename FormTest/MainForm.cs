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
            System.DateTime dt1 = System.DateTime.Now;
          


            System.DateTime dt2 = System.DateTime.Now;
            System.TimeSpan ts = dt2 - dt1;
            MessageBox.Show("耗时"+ts.Minutes*60+ts.Seconds+"."+ts.Milliseconds*1.0/1000.0+"秒");
		}

	}
	
}
