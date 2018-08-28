using System.Linq;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System;
using FormTest;

namespace Tools
{   
    public class PrePapers
    {
        public PrePapers()
        {
            _PrePapers = new List<PrePaper>();
            _dic = null;
            _Changed = false;
        }
        public PrePapers(List<PrePaper> PrePapers)
        {
            _PrePapers = new List<PrePaper>();
            foreach (PrePaper p in PrePapers)
                AddPrePaper(p);
            _dic = null;
            _Changed = false;
        }
        public void AddPrePaper(PrePaper p)
        {
            _PrePapers.Add(p);
            _Changed = true;
        }
        public void SavePrePapers(string Datafilename)
        {
            //string str = Tools.JsonFormatTool.ConvertJsonString(
            //    Newtonsoft.Json.JsonConvert.SerializeObject(_PrePapers));
            //File.WriteAllText(Datafilename, str);
            //_Changed = false;
        }
        public void LoadPrePapers(string Datafilename)
        {
            //Clear();
            //_PrePapers =
            //Newtonsoft.Json.JsonConvert.DeserializeObject<List<PrePaper>>(File.ReadAllText(Datafilename));
            //_Changed = false;
        }
        public void Clear()
        {
            _PrePapers.Clear();
            if (_dic != null)
            {
                _dic.Clear();
                _dic = null;
            }
        }
        public bool AllDetected()
        {
            if (_PrePapers.Count == 0)
                return false;
            return !_PrePapers.Exists(r => !r.Detected());
        }
        public bool AllDetectedSize()
        {
            if (_PrePapers.Count == 0) return false;
            if (_PrePapers[0].Detectdata == null) return false;
            Size size = _PrePapers[0].Detectdata.CorrectRect.Size;
            foreach (PrePaper p in _PrePapers)
            {
                if (!p.Detected()) return false;
                Size size1 = p.Detectdata.CorrectRect.Size;
                double xrate = size.Width * 1.0 / size1.Width;
                double yrate = size.Height * 1.0 / size1.Height;
                if (xrate > 1.1 || xrate < 0.9 || yrate > 1.1 || yrate < 0.9)
                {
                    return false;
                }
            }
            return true;
        }
        public void CheckSizeInfo()
        {
            PrePaper okp = GetFirstCorrectPaper();
            if (okp == null) return;
            Size size = okp.Detectdata.CorrectRect.Size;
            foreach (PrePaper p in _PrePapers)
            {
                if (!p.Detected()) continue;
                Size size1 = p.Detectdata.CorrectRect.Size;
                double xrate = size.Width * 1.0 / size1.Width;
                double yrate = size.Height * 1.0 / size1.Height;
                if (xrate > 1.1 || xrate < 0.9 || yrate > 1.1 || yrate < 0.9)
                    p.Msg = "模板大小不匹配";
                else
                    p.Msg = "";
            }
        }
        public bool AllDetectedB()
        {
            if (_PrePapers.Count == 0)
                return false;
            if (_PrePapers.Exists(r => r.BPrePaper == null))
                return false;
            return !_PrePapers.Exists(r => r.BPrePaper.Detected());
        }
        
        public PrePaper GetPrepaper(string s)
        {
            if (_dic == null)
            {
                _dic = new Dictionary<string, PrePaper>();
                foreach (PrePaper p in _PrePapers)
                    _dic[p.ImgFilename] = p;
            }
            if (_dic.ContainsKey(s))
                return _dic[s];
            return null;
        }
        public void ReContructPapers(List<SubFile> list)
        {
            _Changed = true;
            _dic = _PrePapers.ToDictionary(r => r.ImgFilename, r => r);
            _PrePapers.Clear();
            foreach (SubFile s in list)
            {
                if (_dic.ContainsKey(s.FileName))
                    _PrePapers.Add(_dic[s.FileName]);
                else
                    _PrePapers.Add(new PrePaper(s));
            }
            _dic.Clear();
            _dic = null;
        }
        public PrePaper GetFirstCorrectPaper()
        {
            foreach (PrePaper p in PrePaperList)
                if (p.Detected())
                    return p;
            return null;
        }
        public void CheckSavePrePapers(string p)
        {
            if (_Changed)
            {
                SavePrePapers(p);
                _Changed = false;
            }
        }
        
        //[JsonProperty]
        private List<PrePaper> _PrePapers;
        private Dictionary<string, PrePaper> _dic;
        public List<PrePaper> PrePaperList { get { return _PrePapers; } }
        public bool _Changed { get; set; }
    }
    //[JsonObject(MemberSerialization.OptOut)]
    public class PrePaper
    {
        public PrePaper()
        {
            this._IFN = null;
            this._imgfilename = "";
            Detectdata = null;
            BPrePaper = null;
        }
        public PrePaper(string imgfilename)
        {
            this._IFN = null;
            this._imgfilename = imgfilename;
            Detectdata = null;
            BPrePaper = null;
        }
        public PrePaper(SubFile ifn)
        {
            this._IFN = ifn;
            this._imgfilename = ifn.FullName;
            Detectdata = null;
            BPrePaper = null;
        }
        public void SetNewFileName(string imgfilename)
        {
            _imgfilename = imgfilename;
        }
        public override string ToString()
        {
            string sif = _imgfilename;
            if (sif.Contains("\\"))
            {
                sif = sif.Substring(sif.LastIndexOf("\\"));
            }
            return (Detected() ? "" : "检测错误 ") + Msg + " " + sif;
        }
        public string ToJsonString()
        {
            //return Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(this));
            return "";
        }
        public bool Detected()
        {
            if (Detectdata == null)
                return false;
            return Detectdata.Detected;
        }
        public void ReleaseSrc()
        {
            if (_src != null)
            {
                _src.Dispose();
                _src = null;
            }
        }
        //[JsonIgnore]
        public string ImgFilename { get { return _imgfilename; } }
        //[JsonIgnore]
        public Bitmap Src
        {
            get
            {
                if (_src == null)
                {
                    if (System.IO.File.Exists(_imgfilename))
                        _src = (Bitmap)Bitmap.FromFile(_imgfilename);
                }
                return _src;
            }
        }
        //[JsonIgnore]
        public List<Rectangle> listFeatures
        {
            get
            {
                if (Detectdata == null)
                    return new List<Rectangle>();
                return Detectdata.ListFeature;
            }
        }
        //[JsonProperty]
        public DetectData Detectdata { get; set; }
        //[JsonProperty]
        private string _imgfilename;
        //[JsonIgnore]
        private Bitmap _src;
        //[JsonIgnore]
        private SubFile _IFN;
        //[JsonIgnore]
        public SubFile SubFile { get; set; }
        //[JsonIgnore]
        public string Msg { get; set; }
        public PrePaper BPrePaper { get; set; }
    }
    public class ImageFileName
    {
        public ImageFileName(string FileName, string Path)
        {
            this.FileName = FileName;
            this.Path = Path;
        }
        public ImageFileName(FileInfo fi)
        {
            this.FileName = fi.Name;
            this.Path = fi.Directory.FullName;
        }
        public ImageFileName(string FullName)
        {
            FileInfo fi = new FileInfo(FullName);
            this.FileName = fi.Name;
            this.Path = fi.Directory.FullName;
        }
        public string FileName;
        public string Path; //不含后缀
        public string FullName { get { return Path + "\\" + FileName; } }
        public FileInfo Fi { get { return new FileInfo(FullName); } }
    }
    public class FileDir // 同一类型
    {
        public FileDir(string path, string ext)
        {
            this._path = path;
            this._ext = ext;
            this.ABPage = 0;
            Prepapers = null;
            SubListfiles = new List<SubFile>();
        }
        public void ConstructSubListFile() 
        {
            SubListfiles.Clear();
            List<System.IO.FileInfo> namelist = FileTools.FileListFromDir(FullPath, Ext);
            foreach (System.IO.FileInfo fi in namelist)
                SubListfiles.Add(new SubFile(fi.Name, this));
        }
        private void ConstructSubListFile(List<string> list, string ext) // 构造AB面
        {
            SubListfiles.Clear();
            foreach (string s in list)
            {
                SubFile sf = new SubFile(s + "A" + ext, this);
                SubFile sfb = new SubFile(s + "B" + ext, this);
                sf.AddBPages(sfb);
                SubListfiles.Add(sf);
            }
        }
        private void ConstructSubListFile(List<List<string>> list, string ext) // 构造ABCD面
        {
            SubListfiles.Clear();
            foreach (List<string> s in list)
            {
                if (s.Count > 0)
                {
                    SubFile sf = new SubFile(s[0] + ext, this);
                    for (int i = 1; i < s.Count; i++)
                    {
                        SubFile sfb = new SubFile(s[i] + ext, this);
                        sf.AddBPages(sfb);
                    }
                    SubListfiles.Add(sf);
                }
            }
        }
        public List<SubFile> SubListfiles { get; set; }
        public string FullPath { get { return _path; } }
        public string Ext { get { return _ext; } }
        private string _path;
        private string _ext; //包含  .

        public static FileDir ExistImageABPage(  List<SubFile>  list) // 只有A //采用不同的算法
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            if (list.Count == 0)
                return null;
            foreach (SubFile  f in list)
            {
                string name = f.FileNameWithoutExt;
                char ch = name[name.Length - 1];
                if (!"ABab".Contains(ch))
                    return null;
                if (name.Length < 2)
                    return null;
                string conname = name.Substring(0, name.Length - 1);
                if (dic.ContainsKey(conname))
                {
                    dic[conname] = 3;
                }
                else
                {
                    if (ch == 'A' || ch == 'a')
                        dic[conname] = 1;
                    else
                        dic[conname] = 2;
                }
            }
            foreach (int iv in dic.Values)
                if (iv != 3)
                    return null;

            FileDir ABfd = new FileDir(list[0].Parent.FullPath, list[0].Parent.Ext);
            //ABIF = new ABImageFiles(Fullpath, dic.Keys.ToList(), ".tif");
            ABfd.ConstructSubListFile(dic.Keys.ToList(), ".tif");
            return ABfd;
        }
        public int ABPage { get; set; }
        public PrePapers Prepapers { get; set; }
    }
    public class SubFile
    {
        public SubFile(string FileName, FileDir filedir )
        {
            this._filename = FileName;
            this._parent = filedir;
            this._bpages = new List<SubFile>();
        }
        public string FileNameWithoutExt { get { return _filename.Substring(0,_filename.Length-_parent.Ext.Length); } }
        public string FileName { get {
            if (_parent.ABPage - 1 > 0 && _parent.ABPage - 1 < _bpages.Count)
                return _bpages[_parent.ABPage - 1]._filename;
            else if (_parent.ABPage == 0 || _parent.ABPage == 1 )
                return _filename;
            return _filename;           
        } }
        
        public void AddBPages(SubFile sfb)
        {
            _bpages.Add(sfb);
        }

        public string Path { get { return _parent.FullPath; } }
        public string FullName { get { return Path + "\\" + FileName; } }
        public FileInfo Fi { get { return new FileInfo(FullName); } }
        public FileDir Parent { get { return _parent; } }

        public string BFullName
        {
            get
            {
                if (_bpages.Count > 0)
                    return  Path + "\\" + _bpages[0]._filename;
                return FullName;
            }
        }
        public SubFile B { get { if (_bpages.Count > 0) return _bpages[0]; return null; } }
        private string _filename;
        private FileDir _parent;
        private List<SubFile> _bpages;

    }
    public class FileTools
    {
        public static string GetLastestSubDirectory(string path)
        {
            List<string> sudirects = GetLastestSubDirectorys(path);
            if (sudirects.Count > 0)
                return sudirects.Max();
            return "";
        }
        public static List<string> GetLastestSubDirectorys(string path)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("[0-9]{8}-[0-9]+");
            if (System.IO.Directory.Exists(path))
            {
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
                List<string> sudirects = new List<string>();
                foreach (System.IO.DirectoryInfo d in dir.GetDirectories())
                {
                    if (r.IsMatch(d.Name))
                    {
                        sudirects.Add(d.FullName);
                    }
                }
                return sudirects;
            };
            return new List<string>();
        }
        public static List<System.IO.FileInfo> FileListFromDir(string fidir, string ext = ".tif")
        {
            List<System.IO.FileInfo> namelist = new List<System.IO.FileInfo>();
            System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(fidir);
            //string ext = fi.Extension;
            foreach (System.IO.FileInfo f in dirinfo.GetFiles())
                if (f.Extension.ToLower() == ext)
                    namelist.Add(f);
            return namelist;
        }
      
        public static bool ExistImageFileInDir(string Fullpath, string ext = ".tif")
        {
            System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(Fullpath);
            foreach (System.IO.FileInfo f in dirinfo.GetFiles())
                if (f.Extension.ToLower() == ext)
                    return true;
            return false;
        }
        public static List<string> SubDirNameListFromDir(string fidir)
        {
            List<string> namelist = new List<string>();
            System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(fidir);
            //string ext = fi.Extension;
            foreach (System.IO.DirectoryInfo f in dirinfo.GetDirectories())
                namelist.Add(f.Name);
            return namelist;
        }

    }
}
