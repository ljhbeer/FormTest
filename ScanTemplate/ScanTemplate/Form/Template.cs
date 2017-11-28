using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;

namespace ARTemplate
{
    public class Template
    {
        // "特征点", "考号","姓名", "选择题", "非选择题", "选区变黑", "选区变白"
        public Template(string imgpath, Bitmap bmp, Rectangle CorrectRect)
        {
            this._imagefilename = imgpath;
            this._src = bmp.Clone( CorrectRect, bmp.PixelFormat);
            this.Correctrect = CorrectRect;
            _dic = new Dictionary<string, List<Area>>();
    
        }
        public void ResetData()
        {
            foreach (string s in new string[] { "特征点", "考号", "姓名", "选择题", "非选择题", "选区变黑", "选区变白" })
                _dic[s].Clear();
        }
        public bool CheckEmpty()
        {
            return _dic["选择题"].Count == 0;
        }
        public void AddArea(Area area, string name)
        {
            if (!_dic.ContainsKey(name))
                _dic[name] = new List<Area>();
            _dic[name].Add(area);
        }
       
        public void Save(String xmlFileName)
        {
            if (xmlFileName.ToLower().EndsWith(".xml"))
            {
                XmlDocument xmlDoc = SaveToXmlDoc();
                xmlDoc.Save(xmlFileName);
            }
        }
        public XmlDocument SaveToXmlDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement(NodeName);
            xmlDoc.AppendChild(root);
            XmlNode path = xmlDoc.CreateElement("BASE");
            root.AppendChild(path);
            path.InnerXml = "<SIZE>" + Imgsize.Width + "," + Imgsize.Height + "</SIZE>"
                            + "<PATH>" + _imagefilename + "</PATH>";

            foreach (string s in new string[] { "特征点-FEATUREPOINTSAREA", "考号-KAOHAOAREA", "姓名-NAMEAREA", "选择题-SINGLECHOICES", "非选择题-UNCHOOSES", "选区变黑-BLACKAREA", "选区变白-WHITEAREA" })
            {
                string name = s.Substring(0, s.IndexOf("-"));
                string ENname = s.Substring(s.IndexOf("-"));
                XmlNode  list = xmlDoc.CreateElement("");
                root.AppendChild(list);
                int i = 0;
                foreach (Area I in _dic[name])
                {
                    XmlElement xe = xmlDoc.CreateElement(ENname);
                    xe.SetAttribute("ID", i.ToString());
                    xe.InnerXml = I.ToXmlString();
                    list.AppendChild(xe);
                    i++;
                }
            }
            return xmlDoc;
        }     
        public bool Load(String xmlFileName)
        {
            ResetData();
            if (!xmlFileName.ToLower().EndsWith(".xml")) return false;
            if (!File.Exists(xmlFileName)) return false;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFileName);
                _imagefilename = xmlDoc.SelectSingleNode(NodeName + "/BASE/PATH").InnerText;
                Size imgsize =Tools.StringTools. StringToSize(xmlDoc.SelectSingleNode(NodeName + "/BASE/SIZE").InnerText);
                //////Bitmap bitmap =(Bitmap) Bitmap.FromFile(_imagefilename);
                //////if (bitmap.Size != imgsize)
                //////    return false;
                //////_src = bitmap;
                foreach (string s in new string[] { "特征点-FEATUREPOINTSAREA", "考号-KAOHAOAREA", "姓名-NAMEAREA", "选择题-SINGLECHOICES", "非选择题-UNCHOOSES", "选区变黑-BLACKAREA", "选区变白-WHITEAREA" })
                {
                    string name = s.Substring(0, s.IndexOf("-"));
                    string ENname = s.Substring(s.IndexOf("-"));
                    string path = (NodeName + "/[]/*").Replace("[]",ENname);
                    XmlNodeList  list = xmlDoc.SelectNodes(path);
                    _dic[name] = new List<Area>();
                    foreach (XmlNode node in list)
                    {
                        if (ENname == "KAOHAOAREA")
                        {
                            Rectangle rect = Tools.StringTools.StringToRectangle(node.SelectSingleNode("RECTANGLE").InnerText);
                            Size ssize = Tools.StringTools.StringToSize(node.SelectSingleNode("SIZE").InnerText);
                            List<List<Point>> llistp = new List<List<Point>>();
                            foreach (XmlNode node1 in node.ChildNodes)
                            {
                                if (node1.Name == "SINGLE")
                                {
                                    List<Point> listp = new List<Point>();
                                    foreach (XmlNode node2 in node1.ChildNodes)
                                    {
                                        listp.Add(Tools.StringTools.StringToPoint(node2.InnerText));
                                    }
                                    llistp.Add(listp);
                                }
                            }
                            _dic[name].Add(new KaoHaoChoiceArea(rect, name, "条形码"));
                        }else if(ENname == "FEATUREPOINTSAREA")
                        {
                        }
                        else if (ENname == "NAMEAREA")
                        {
                        }
                        else if (ENname == "SINGLECHOICES")
                        {
                            Rectangle rect = Tools.StringTools.StringToRectangle(node.SelectSingleNode("RECTANGLE").InnerText);
                            Size ssize = Tools.StringTools.StringToSize(node.SelectSingleNode("SIZE").InnerText);
                            List<List<Point>> llistp = new List<List<Point>>();
                            foreach (XmlNode node1 in node.ChildNodes)
                            {
                                if (node1.Name == "SINGLE")
                                {
                                    List<Point> listp = new List<Point>();
                                    foreach (XmlNode node2 in node1.ChildNodes)
                                    {
                                        listp.Add(Tools.StringTools.StringToPoint(node2.InnerText));
                                    }
                                    llistp.Add(listp);
                                }
                            }
                            _dic[name].Add(new SingleChoiceArea(rect, name, llistp, ssize));
                        }
                        else if (ENname == "UNCHOOSES")
                        {
                        }
                        else if (ENname == "BLACKAREA")
                        {
                        }
                        else if (ENname == "WHITEAREA")
                        {
                        }
                    }
                }
            }
            catch
            {
                ResetData();
                return false;
            }
            return true;
        }       
        public void SetDataToNode(TreeNode m_tn)
        {
            foreach (string s in new string[] { "特征点", "考号", "姓名", "选择题", "非选择题", "选区变黑", "选区变白" })
            {
                TreeNodeCollection tc = m_tn.Nodes[s].Nodes;
                if(_dic.ContainsKey(s))
                foreach (Area I in _dic[s])
                {
                    TreeNode t = new TreeNode();
                    int cnt = tc.Count + 1;
                    t.Name = t.Text =s + cnt;
                    t.Tag =I;
                    tc.Add(t);
                }
            }               
        }
        
        public String NodeName{ get { return "TEMPLATE"; } }
        public Bitmap Image { get { return _src; } }
        public List<Area> SingleAreas {get { return _dic["选择题"];}}
        public Size Imgsize
        {
            get { return _src.Size; }
        }
        public String Filename
        {
            get { return _imagefilename; }
        }
        public Rectangle Correctrect
        {
            get;
            set;
        }
        private Bitmap _src;
        private string _imagefilename;
        private Dictionary< string, List<Area>> _dic;      
    }
}
