using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace  Tools
{
    public class StudentBases
    {
        public StudentBases( )
        {
            
        }
        public static StudentBases StudentBaseFromFile(string listfilename)
        {
            if(File.Exists(listfilename))
                return StudentBaseFromString(File.ReadAllText(listfilename));
            return null;
        }
        public static StudentBases StudentBaseFromString(string strcontent)
        {
            StudentBases stb = new StudentBases();
            stb.HasStudentBase = false;
            bool haserror = false;
            string msg = "";
            if (strcontent!="")
            {
                strcontent = strcontent.Replace("\t", ",");
                String[] ss = strcontent.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length > 2 && ss[0].Contains("班级,姓名,考号"))
                {
                    stb.HasStudentBase = true;
                    stb._studentbases = new List<StudentBase>();
                    stb._khbasedic = new Dictionary<int, StudentBase>();
                    stb._classiddic = new Dictionary<int, List<StudentBase>>();
                    for (int i = 1; i < ss.Length; i++)
                    {
                        string[] items = ss[i].Split(',');
                        if (items.Length == 3)
                        {
                            int classid = Convert.ToInt32(items[0]);
                            int kh = Convert.ToInt32(items[2]);
                            StudentBase sb = new StudentBase(classid, items[1], kh);
                            stb._studentbases.Add(sb);
                            if (!stb._khbasedic.ContainsKey(kh))
                                stb._khbasedic[kh] = sb;
                            else
                            {
                                haserror = true;
                                StudentBase sb1 = stb._khbasedic[kh];
                                msg += "Line: " + i + "\t两位同学的考号重复\r\n" + sb+"\r\n" + sb1+ "\r\n";
                            }

                            if (!stb._classiddic.ContainsKey(classid))
                                stb._classiddic[classid] = new List<StudentBase>();

                            if (!stb._classiddic[classid].Contains(sb))
                                stb._classiddic[classid].Add(sb);
                            else
                            { // Bug , Cannt come here
                                haserror = true;
                                msg += "Line: " + i + "\t重复添加对象：kh=" + sb.KH + "\r\n";
                            }

                        }
                        else
                        {
                            haserror = true;
                            msg += "Line: " + i + "\t以下条目中存在错误，每行超过3个项目\r\n"  + ss[i] + "\r\n";
                        }
                    }
                }
            }
            if (stb.HasStudentBase || haserror )
                stb.Msg = msg;
            stb.HasError = haserror;
            return stb;
            //return null;
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
        public Dictionary<int, List<StudentBase>> DicClassids { get { return _classiddic; } }
        public Dictionary<int, StudentBase> DicKH { get { return _khbasedic; } }
        public bool ContainsKey(int kh)
        {
            return _khbasedic.ContainsKey(kh);
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

        public bool RemoveStudent(StudentBase s)
        {
            if (_khbasedic.ContainsKey(s.KH))
            {
                _studentbases.Remove(s);
                _khbasedic.Remove(s.KH);
                _classiddic[s.Classid].Remove(s);
                return true;
            }            
                return false;
        }

        public string ToString(string type)
        {
            if (type == "allstudent")
            {
                return "班级\t姓名\t考号\r\n" + string.Join("\r\n",
                    _studentbases.Select(r => r.ToString("")));
            }
            return "";
        }

        public string Msg { get; set; }

        public bool HasError { get; set; }
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
        public override string ToString()
        {
            return "班级：" + Classid + " 姓名：" + Name + " 考号：" + KH;
        }

        public string ToString(string type)
        {
            if(type == "")
                return Classid + "\t" + Name + "\t" + KH;
            return ToString();
        }
    }
}
