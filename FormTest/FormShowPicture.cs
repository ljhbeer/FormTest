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
	public partial class FormShowPicture : Form
	{
		public FormShowPicture()
		{
			InitializeComponent();			
		}		
		void ButtonTestClick(object sender, EventArgs e)
		{

		}

        private void FormShowPicture_Load(object sender, EventArgs e)
        {
            if (File.Exists("ImportStudentImg\\readme.jpg"))
            {
                pictureBox1.Image = Bitmap.FromFile("ImportStudentImg\\readme.jpg");
            }
        }

	}
	
}
