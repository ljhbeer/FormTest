using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace  Tools
{
    public class Student
    {
        //public Student(DataRow dr, int XZTcount)
        //{
        //    ValueTag vt = (ValueTag)dr["序号"];
        //    Paper paper = (Paper)vt.Tag;
        //    _BFilename = "";
        //    _bsrc = null;
        //    if (paper.BPaper != null)
        //    {
        //        _BFilename = paper.BPaper.ImgFilename;
        //        _bSrcCorrectRect = paper.BPaper.SrcCorrectRect;
        //    }
        //    this._imgfilename = dr["文件名"].ToString();
        //    FileInfo fi = new FileInfo(_imgfilename);
        //    string str = RefineNumber(fi.Name);
        //    if (str == "")
        //        str = ChangeToNumber(fi.Name);
        //    _id = Convert.ToInt32(str);
        //    _id %= 10000;

        //    str = dr["CorrectRect"].ToString();
        //    _SrcCorrectRect = Tools.StringTools.StringToRectangle(str, '-');
        //    this.Angle = Convert.ToDouble(dr["校验角度"].ToString());
        //    this.Name = dr["姓名"].ToString();
        //    if (dr.Table.Columns.Contains("考号"))
        //    {
        //        string skh = dr["考号"].ToString();
        //        if (skh.Contains("-"))
        //            this.KH = 0;
        //        else
        //            this.KH = Convert.ToInt32(dr["考号"].ToString());
        //    }
        //    this._XZT = new List<string>();
        //    for (int i = 1; i < XZTcount + 1; i++)
        //    {
        //        _XZT.Add(dr["x" + i].ToString());
        //    }
        //    _src = null;
        //    Sort = new StudentSort();
        //    BackScore = -1;
        //    Tag = null;
        //    TagInfor = "";
        //}

        public Student()
        {
            //BackScore = -1;
        }
       
        public string ResultInfo()
        {
            return ID + "," + KH + "," + Name + ",";
        }
        
      public static string ResultTitle()
        {
            return "ID,考号,姓名,";
        }

        public static string RefineNumber(string name)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in name)
                if (c >= '0' && c <= '9')
                    sb.Append(c);
            string str = sb.ToString();
            if (str.Length > 8)
                str = str.Substring(str.Length - 8);
            return str;
        }
        private static string ChangeToNumber(string name)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in name)
            {
                int ic = c % 10;
                sb.Append(ic);
            }
            string str = sb.ToString();
            if (str.Length > 8)
                str = str.Substring(str.Length - 8);
            return str;
        }
        //ForB
        //[JsonProperty]
        //private string _BFilename;
        //private Bitmap _bsrc;
        //[JsonProperty]
        //private Rectangle _bSrcCorrectRect;
        //[JsonIgnore]
        //public Rectangle BSrcCorrectRect
        //{
        //    get
        //    {
        //        return _bSrcCorrectRect;
        //    }
        //}
        //[JsonIgnore]
        //public Bitmap BSrc
        //{
        //    get
        //    {
        //        if (_bsrc == null)
        //        {
        //            if (System.IO.File.Exists(_BFilename))
        //                _bsrc = (Bitmap)Bitmap.FromFile(_BFilename);
        //        }
        //        if (_bsrc != null)
        //        {
        //            if (_bsrc.VerticalResolution != 200 || _bsrc.HorizontalResolution != 200)
        //                _bsrc.SetResolution(200, 200);
        //        }
        //        return _bsrc;
        //    }
        //}
        //[JsonIgnore]
        //public string BImgFilename { get { return _BFilename; } }

        //[JsonIgnore]
        public int ID { get { return _id; } }
        public double Angle { get; set; }
        public int KH { get; set; }
        public string Name { get; set; }
        //[JsonIgnore]
        public string ImgFilename { get { return _imgfilename; } }
        public int Index { get; set; }

        //[JsonIgnore]
        //public int BackScore { get; set; }
        //[JsonIgnore]
        //public StudentSort Sort { get; set; }
        //[JsonIgnore]
        //public Bitmap Src
        //{
        //    get
        //    {
        //        if (_src == null)
        //        {
        //            if (System.IO.File.Exists(_imgfilename))
        //                _src = (Bitmap)Bitmap.FromFile(_imgfilename);
        //        }
        //        if (_src != null)
        //        {
        //            if (_src.VerticalResolution != 200 || _src.HorizontalResolution != 200)
        //                _src.SetResolution(200, 200);
        //        }
        //        return _src;
        //    }
        //}
        //[JsonIgnore]
        //public Rectangle SrcCorrectRect
        //{
        //    get
        //    {
        //        return _SrcCorrectRect;
        //    }
        //}
        //[JsonProperty]
        private string _imgfilename;
        //[JsonProperty]
        //private List<string> _XZT;
        private int _id;
        //[JsonProperty]
        //private Rectangle _SrcCorrectRect;
        //private Bitmap _src;

        //public bool SelectOption(string r, int index)
        //{
        //    if (index >= 0 && index < _XZT.Count)
        //        return r == _XZT[index];
        //    return false;
        //}
        //[JsonIgnore]
        //public PaperResult Tag { get; set; }
        //[JsonIgnore]
        //public string TagInfor { get; set; }
    }
    public class StudentBases
    {
        public StudentBases(string listfilename)
        {
            InitStudentBase(listfilename);
        }
        public void InitStudentBase(string listfilename)
        {
            HasStudentBase = false;
            string msg = "";
            if (File.Exists(listfilename))
            {
                String[] ss = File.ReadAllLines(listfilename);
                if (ss.Length > 2 && ss[0].Contains("班级,姓名,考号"))
                {
                    bool haserror = false;
                    HasStudentBase = true;
                    _studentbases = new List<StudentBase>();
                    _khbasedic = new Dictionary<int, StudentBase>();
                    _classiddic = new Dictionary<int, List<StudentBase>>();
                    for (int i = 1; i < ss.Length; i++)
                    {
                        string[] items = ss[i].Split(',');
                        if (items.Length == 3)
                        {
                            int classid = Convert.ToInt32(items[0]);
                            int kh = Convert.ToInt32(items[2]);
                            StudentBase sb = new StudentBase(classid, items[1], kh);
                            _studentbases.Add(sb);
                            if (!_khbasedic.ContainsKey(kh))
                                _khbasedic[kh] = sb;
                            if (!_classiddic.ContainsKey(classid))
                                _classiddic[classid] = new List<StudentBase>();
                            if (!_classiddic[classid].Contains(sb))
                                _classiddic[classid].Add(sb);
                            else
                            {
                                haserror = true;
                                msg += "Line: " + i + "\t重复添加对象：kh=" + sb.KH + "\r\n";
                            }

                        }
                        else
                        {
                            haserror = true;
                            msg += "Line: " + i + "\t" + ss[i] + "\r\n";
                        }
                    }
                    if (haserror)
                    {
                        MessageBox.Show("以下条目中存在错误，每行超过3个项目\r\n" + msg);
                    }
                }
            }
        }
        public List<StudentBase> GetClassStudent(int classid)
        {
            if (HasStudentBase && _classiddic.ContainsKey(classid))
                return _classiddic[classid];
            return new List<StudentBase>();
        }
        public string GetName(int kh)
        {
            if (HasStudentBase && _khbasedic.ContainsKey(kh))
                return _khbasedic[kh].Name;
            return "-";
        }

        public bool HasStudentBase { get; set; }
        private List<StudentBase> _studentbases;
        private Dictionary<int, StudentBase> _khbasedic;
        private Dictionary<int, List<StudentBase>> _classiddic;

        public List<StudentBase> Studentbase { get { return _studentbases; } }
        public bool ContainsKey(int kh)
        {
            return _khbasedic.ContainsKey(kh);
        }
        public int GetClass(Student r)
        {
            if (HasStudentBase && _khbasedic.ContainsKey(r.KH))
                return _khbasedic[r.KH].Classid;
            return 0;
        }
        public StudentBase GetStudent(int kh)
        {
            if (HasStudentBase && _khbasedic.ContainsKey(kh))
                return _khbasedic[kh];
            return null;
        }
        public int GetClass(int kh)
        {
            if (HasStudentBase && _khbasedic.ContainsKey(kh))
                return _khbasedic[kh].Classid;
            return 0;
        }
    }
    public class StudentBase
    {
        public StudentBase(int classid, string name, int kh)
        {
            this.Classid = classid;
            this.Name = name;
            this.KH = kh;
            this.PYCode = PYTool.GetChineseSpell(name);
        }
        public int Classid { get; set; }
        public string Name { get; set; }
        public int KH { get; set; }
        public string PYCode { get; set; }
    }
}
