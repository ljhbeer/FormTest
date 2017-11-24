namespace ScanTemplate
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxWorkPath = new System.Windows.Forms.TextBox();
            this.buttonworkpath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(17, 37);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(212, 304);
            this.listBox1.TabIndex = 0;
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(15, 349);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(214, 32);
            this.buttonGo.TabIndex = 1;
            this.buttonGo.Text = "创建模板";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "工作目录";
            // 
            // textBoxWorkPath
            // 
            this.textBoxWorkPath.Location = new System.Drawing.Point(78, 4);
            this.textBoxWorkPath.Name = "textBoxWorkPath";
            this.textBoxWorkPath.Size = new System.Drawing.Size(515, 21);
            this.textBoxWorkPath.TabIndex = 3;
            this.textBoxWorkPath.Text = "E:\\Scan\\LJH\\s1025";
            // 
            // buttonworkpath
            // 
            this.buttonworkpath.Location = new System.Drawing.Point(600, 2);
            this.buttonworkpath.Name = "buttonworkpath";
            this.buttonworkpath.Size = new System.Drawing.Size(43, 22);
            this.buttonworkpath.TabIndex = 4;
            this.buttonworkpath.Text = "更改工作目录";
            this.buttonworkpath.UseVisualStyleBackColor = true;
            this.buttonworkpath.Click += new System.EventHandler(this.buttonworkpath_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 393);
            this.Controls.Add(this.buttonworkpath);
            this.Controls.Add(this.textBoxWorkPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.listBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxWorkPath;
        private System.Windows.Forms.Button buttonworkpath;
    }
}

