using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tools;
using System.IO;
namespace FormTest
{
    public partial class FormImportStudent : Form
    {
        public FormImportStudent()
        {
            InitializeComponent();
            InitFirstDgvs();
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel2Collapsed = true;
            buttonverify.Enabled = true;
            buttonreinput.Enabled = false;
        }
        private void InitFirstDgvs()
        {
            DataTable dtclass = new DataTable("班级列表");
            DataTable dtstudents = new DataTable("学生列表");
            foreach (string name in new string[] { "班级", "姓名", "考号" })
            {
                DataColumn dc = new DataColumn(name);
                dtstudents.Columns.Add(dc);
            }
            foreach (string name in new string[] { "班级", "人数" })
            {
                DataColumn dc = new DataColumn(name);
                dtclass.Columns.Add(dc);
            }
            dgvstu.DataSource = dtstudents;
            dgvclass.DataSource = dtclass;
            dgvstu.Columns["班级"].DataPropertyName = "Classid";
            dgvstu.Columns["姓名"].DataPropertyName = "Name";
            dgvstu.Columns["考号"].DataPropertyName = "KH";
            dgvclass.Columns["班级"].DataPropertyName = "Classid";
            dgvclass.Columns["人数"].DataPropertyName = "StudentCount";
            InitDgvUI(dgvclass);
            InitDgvUI(dgvstu);
        }
        private void InitDgvUI(DataGridView dgv)
        {
            dgv.RowHeadersVisible = false;
            dgv.AutoGenerateColumns = false;
            foreach (DataGridViewColumn dc in dgv.Columns)
            {
                dc.Width = 60;               
            }          
        }
        private void FormImportStudent_Load(object sender, EventArgs e)
        {
            textBox1.Text = File.ReadAllText(@"E:\JHScanPaper\Data\StudentList\StudentBaseList.txt");
        }
        private void buttonReadMe_Click(object sender, EventArgs e)
        {
            this.Hide();
            (new FormShowPicture()).ShowDialog();
            this.Show();
        }
        private void buttonverify_Click(object sender, EventArgs e)
        {
            if (Verifyed())
            {
                InitDgvs();
                splitContainer1.Panel1Collapsed = true;
                splitContainer1.Panel2Collapsed = false;
                buttonverify.Enabled = false;
                buttonreinput.Enabled = true;
            }
        }
        private bool Verifyed()
        {
            input = textBox1.Text;
            _studentbases = StudentBases.StudentBaseFromString(input);
            if (!_studentbases.HasStudentBase)
            {
                MessageBox.Show(_studentbases.Msg);
                _studentbases = null;
                return false;
            }           
            if (_studentbases.HasError)
            {
                MessageBox.Show(_studentbases.Msg);
                return false;
            }
            return true;
        }
        private void InitDgvs()
        {
            List<jhclass> cls = new List<jhclass>();
            foreach (KeyValuePair<int, List<StudentBase>> kv in _studentbases.DicClassids)
                cls.Add( new jhclass(kv.Key, _studentbases));
            dgvclass.DataSource = cls;
            
            dgvstu.DataSource = _studentbases.Studentbase;            
        }
        private void buttonreinput_Click(object sender, EventArgs e)
        {
            input = DgvToTxt();
            textBox1.Text = input;
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel2Collapsed = true;
            buttonverify.Enabled = true;
            buttonreinput.Enabled = false;
        }
        private string DgvToTxt()
        {
            return _studentbases.ToString("allstudent");
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {

        }
        private void buttonRemoveByKH_Click(object sender, EventArgs e)
        {
            if (dgvstu.SelectedCells.Count == 1)
            {
                if (dgvstu.SelectedCells[0].RowIndex != -1)
                {
                    StudentBase s = (StudentBase)(dgvstu.Rows[dgvstu.SelectedCells[0].RowIndex].DataBoundItem);
                    if(MessageBox.Show("你确定要删除" + s.ToString()+"吗","删除学生",MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes){
                        if (_studentbases.RemoveStudent(s))
                            MessageBox.Show("已删除");
                        else
                            MessageBox.Show("不存在该同学");
                        dgvclass.Invalidate();
                        dgvstu.Invalidate();
                    }
                }
            }

        }
        private void dgvclass_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            jhclass jhc = (jhclass)(dgvclass.Rows[e.RowIndex].DataBoundItem);
            dgvstu.DataSource = jhc.Studentlist;            
        }
        private void buttonsaveas_Click(object sender, EventArgs e)
        {
            if (buttonverify.Enabled)
            {
                MessageBox.Show("数据必须校验后才能保存");
                return;
            }
        }
        private void buttonsaveasdefault_Click(object sender, EventArgs e)
        {
            if (buttonverify.Enabled)
            {
                MessageBox.Show("数据必须校验后才能保存");
                return;
            }

        }
        private string input;
        private StudentBases _studentbases;


    }
    public class jhclass
    {
        public jhclass(int cid, StudentBases stubases)
        {
            this.Classid = cid;
            this._stubases = stubases;
        }

        public int Classid { get; set; }
        public int StudentCount { get {  return  _stubases.GetClassStudent(Classid).Count; } }
        public List<StudentBase> Studentlist { get { return _stubases.GetClassStudent(Classid); } }
        private StudentBases _stubases;
    }

}
