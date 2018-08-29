namespace FormTest
{
    partial class FormImportStudent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonReadMe = new System.Windows.Forms.Button();
            this.buttonverify = new System.Windows.Forms.Button();
            this.buttonreinput = new System.Windows.Forms.Button();
            this.buttonsaveas = new System.Windows.Forms.Button();
            this.buttonsaveasdefault = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvstu = new System.Windows.Forms.DataGridView();
            this.dgvclass = new System.Windows.Forms.DataGridView();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonRemoveByKH = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvstu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvclass)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(580, 438);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttonReadMe);
            this.flowLayoutPanel1.Controls.Add(this.buttonverify);
            this.flowLayoutPanel1.Controls.Add(this.buttonreinput);
            this.flowLayoutPanel1.Controls.Add(this.buttonsaveas);
            this.flowLayoutPanel1.Controls.Add(this.buttonsaveasdefault);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(574, 29);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // buttonReadMe
            // 
            this.buttonReadMe.Location = new System.Drawing.Point(3, 3);
            this.buttonReadMe.Name = "buttonReadMe";
            this.buttonReadMe.Size = new System.Drawing.Size(67, 26);
            this.buttonReadMe.TabIndex = 0;
            this.buttonReadMe.Text = "说明";
            this.buttonReadMe.UseVisualStyleBackColor = true;
            this.buttonReadMe.Click += new System.EventHandler(this.buttonReadMe_Click);
            // 
            // buttonverify
            // 
            this.buttonverify.Location = new System.Drawing.Point(76, 3);
            this.buttonverify.Name = "buttonverify";
            this.buttonverify.Size = new System.Drawing.Size(67, 26);
            this.buttonverify.TabIndex = 0;
            this.buttonverify.Text = "校验";
            this.buttonverify.UseVisualStyleBackColor = true;
            this.buttonverify.Click += new System.EventHandler(this.buttonverify_Click);
            // 
            // buttonreinput
            // 
            this.buttonreinput.Location = new System.Drawing.Point(149, 3);
            this.buttonreinput.Name = "buttonreinput";
            this.buttonreinput.Size = new System.Drawing.Size(67, 26);
            this.buttonreinput.TabIndex = 0;
            this.buttonreinput.Text = "重新输入";
            this.buttonreinput.UseVisualStyleBackColor = true;
            this.buttonreinput.Click += new System.EventHandler(this.buttonreinput_Click);
            // 
            // buttonsaveas
            // 
            this.buttonsaveas.Location = new System.Drawing.Point(222, 3);
            this.buttonsaveas.Name = "buttonsaveas";
            this.buttonsaveas.Size = new System.Drawing.Size(81, 26);
            this.buttonsaveas.TabIndex = 0;
            this.buttonsaveas.Text = "另存为名单";
            this.buttonsaveas.UseVisualStyleBackColor = true;
            this.buttonsaveas.Click += new System.EventHandler(this.buttonsaveas_Click);
            // 
            // buttonsaveasdefault
            // 
            this.buttonsaveasdefault.Location = new System.Drawing.Point(309, 3);
            this.buttonsaveasdefault.Name = "buttonsaveasdefault";
            this.buttonsaveasdefault.Size = new System.Drawing.Size(101, 26);
            this.buttonsaveasdefault.TabIndex = 0;
            this.buttonsaveasdefault.Text = "保存为默认名单";
            this.buttonsaveasdefault.UseVisualStyleBackColor = true;
            this.buttonsaveasdefault.Click += new System.EventHandler(this.buttonsaveasdefault_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(574, 397);
            this.panel1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(574, 397);
            this.splitContainer1.SplitterDistance = 230;
            this.splitContainer1.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(230, 397);
            this.textBox1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.44855F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.55145F));
            this.tableLayoutPanel2.Controls.Add(this.dgvstu, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.dgvclass, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBox2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(340, 397);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // dgvstu
            // 
            this.dgvstu.AllowUserToAddRows = false;
            this.dgvstu.AllowUserToDeleteRows = false;
            this.dgvstu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvstu.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvstu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvstu.Location = new System.Drawing.Point(3, 36);
            this.dgvstu.Name = "dgvstu";
            this.dgvstu.ReadOnly = true;
            this.dgvstu.RowTemplate.Height = 23;
            this.dgvstu.Size = new System.Drawing.Size(233, 358);
            this.dgvstu.TabIndex = 0;
            // 
            // dgvclass
            // 
            this.dgvclass.AllowUserToAddRows = false;
            this.dgvclass.AllowUserToDeleteRows = false;
            this.dgvclass.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvclass.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvclass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvclass.Location = new System.Drawing.Point(242, 36);
            this.dgvclass.Name = "dgvclass";
            this.dgvclass.ReadOnly = true;
            this.dgvclass.RowTemplate.Height = 23;
            this.dgvclass.Size = new System.Drawing.Size(95, 358);
            this.dgvclass.TabIndex = 1;
            this.dgvclass.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvclass_CellClick);
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(242, 3);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(95, 26);
            this.textBox2.TabIndex = 2;
            this.textBox2.Text = "班级统计信息";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonRemoveByKH);
            this.panel2.Controls.Add(this.buttonAdd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(233, 27);
            this.panel2.TabIndex = 3;
            // 
            // buttonRemoveByKH
            // 
            this.buttonRemoveByKH.Location = new System.Drawing.Point(76, 1);
            this.buttonRemoveByKH.Name = "buttonRemoveByKH";
            this.buttonRemoveByKH.Size = new System.Drawing.Size(67, 26);
            this.buttonRemoveByKH.TabIndex = 0;
            this.buttonRemoveByKH.Text = "删除";
            this.buttonRemoveByKH.UseVisualStyleBackColor = true;
            this.buttonRemoveByKH.Click += new System.EventHandler(this.buttonRemoveByKH_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(3, 1);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(67, 26);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "添加";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // FormImportStudent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 438);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormImportStudent";
            this.Text = "FormImportStudent";
            this.Load += new System.EventHandler(this.FormImportStudent_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvstu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvclass)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonReadMe;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView dgvstu;
        private System.Windows.Forms.DataGridView dgvclass;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonRemoveByKH;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonverify;
        private System.Windows.Forms.Button buttonreinput;
        private System.Windows.Forms.Button buttonsaveas;
        private System.Windows.Forms.Button buttonsaveasdefault;
    }
}