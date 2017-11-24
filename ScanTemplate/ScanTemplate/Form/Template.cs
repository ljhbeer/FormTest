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
        public Template(string imgpath, Bitmap bmp, Rectangle CorrectRect)
        {
            this._imagefilename = imgpath;
            this._src = bmp;
            this._correctrect = CorrectRect;
            m_singlechoice = new List<SingleChoice>();
            m_singlechoicearea = new List<SingleChoiceArea>();
            m_kaohaochoicearea = new List<KaoHaoChoiceArea>();           
        }
        public void ResetData()
        {
            m_singlechoice.Clear();
            m_singlechoicearea.Clear();
            m_kaohaochoicearea.Clear();
        }
        public void AddSingleChoice(SingleChoice singleChoice)
        {
            m_singlechoice.Add(singleChoice);
        }
        public void AddSingleChoiceArea(SingleChoiceArea singleChoicearea)
        {
            m_singlechoicearea.Add(singleChoicearea);
        }
        public void AddKaoHaoSingleChoiceArea(KaoHaoChoiceArea singleChoiceArea)
        {
            m_kaohaochoicearea.Add(singleChoiceArea);
        }
        public bool CheckEmpty()
        {
            return m_singlechoicearea.Count == 0;
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
            XmlNode fplist = xmlDoc.CreateElement("FEATUREPOINTS");
            XmlNode khlist = xmlDoc.CreateElement("KAOHAOCHOICES");
            XmlNode sclist = xmlDoc.CreateElement("SINGLECHOICES");
            XmlNode uclist = xmlDoc.CreateElement("UNCHOOSES");
            root.AppendChild(path);
            root.AppendChild(khlist);
            root.AppendChild(fplist);
            root.AppendChild(sclist);
            root.AppendChild(uclist);
            {
                path.InnerXml = "<SIZE>" + Imgsize.Width + "," + Imgsize.Height + "</SIZE>"
                                + "<PATH>" + _imagefilename + "</PATH>";
            }
            int i = 0;
            //foreach (TriAngleFeature t in m_fp)
            //{
            //    XmlElement xe = xmlDoc.CreateElement("FPOINTS");
            //    xe.SetAttribute("ID", i.ToString());
            //    xe.InnerXml = t.ToXmlString();
            //    fplist.AppendChild(xe);
            //    i++;
            //}
            //i = 0;
            foreach (KaoHaoChoiceArea sc in m_kaohaochoicearea)
            {
                XmlElement xe = xmlDoc.CreateElement("SCHOICE");
                xe.SetAttribute("ID", i.ToString());
                xe.InnerXml = sc.ToXmlString();
                khlist.AppendChild(xe);
                i++;
            }
            i = 0;
            foreach (SingleChoiceArea sc in m_singlechoicearea)
            {
                XmlElement xe = xmlDoc.CreateElement("SCHOICE");
                xe.SetAttribute("ID", i.ToString());
                xe.InnerXml = sc.ToXmlString();
                sclist.AppendChild(xe);
                i++;
            }
            i = 0;           
            return xmlDoc;
        }
        //public XmlDocument SaveToSupperXmlDoc()
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    XmlElement supperroot = xmlDoc.CreateElement("SUPPERROOT");
        //    xmlDoc.AppendChild(supperroot);
        //    String name = NodeName.Substring(1, NodeName.Count() - 1);
        //    XmlElement root = xmlDoc.CreateElement(name);
        //    supperroot.AppendChild(root);
        //    XmlNode path = xmlDoc.CreateElement("BASE");
        //    XmlNode fplist = xmlDoc.CreateElement("FEATUREPOINTS");
        //    XmlNode sclist = xmlDoc.CreateElement("SINGLECHOICES");
        //    XmlNode uclist = xmlDoc.CreateElement("UNCHOOSES");
        //    root.AppendChild(path);
        //    root.AppendChild(fplist);
        //    root.AppendChild(sclist);
        //    root.AppendChild(uclist);
        //    {
        //        path.InnerXml = "<SIZE>" + Imgsize.Width + "," + Imgsize.Height + "</SIZE>"
        //                        + "<PATH>" + _imagefilename + "</PATH>";
        //    }
        //    int i = 0;
        //    foreach (TriAngleFeature t in m_fp)
        //    {
        //        XmlElement xe = xmlDoc.CreateElement("FPOINTS");
        //        xe.SetAttribute("ID", i.ToString());
        //        xe.InnerXml = t.ToXmlString();
        //        fplist.AppendChild(xe);
        //        i++;
        //    }
        //    i = 0;
        //    foreach (SingleChoiceArea sc in m_singlechoicearea)
        //    {
        //        XmlElement xe = xmlDoc.CreateElement("SCHOICE");
        //        xe.SetAttribute("ID", i.ToString());
        //        xe.InnerXml = sc.ToXmlString();
        //        sclist.AppendChild(xe);
        //        i++;
        //    }
        //    i = 0;
        //    foreach (UnChoose uc in m_unchoose)
        //    {
        //        XmlElement xe = xmlDoc.CreateElement("SCHOICE");
        //        xe.SetAttribute("ID", i.ToString());
        //        xe.InnerXml = uc.ToXmlString();
        //        uclist.AppendChild(xe);
        //        i++;
        //    }
        //    return xmlDoc;
        //}
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
                Bitmap bitmap =(Bitmap) Bitmap.FromFile(_imagefilename);
                if (bitmap.Size != imgsize)
                    return false;
                _src = bitmap;

                //XmlNodeList fplist = xmlDoc.SelectNodes(NodeName + "/FEATUREPOINTS/*");
                XmlNodeList khlist = xmlDoc.SelectNodes(NodeName + "/KAOHAOCHOICES/*");
                XmlNodeList sclist = xmlDoc.SelectNodes(NodeName + "/SINGLECHOICES/*");

               
                //=====================================
                foreach (XmlNode node in khlist)
                {
                    Rectangle rect =Tools.StringTools. StringToRectangle(node.SelectSingleNode("RECTANGLE").InnerText);
                    string name = node.SelectSingleNode("NAME").InnerText;
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
                    m_kaohaochoicearea.Add(new  KaoHaoChoiceArea(rect, name, llistp, ssize));
                }
                //=====================================
                foreach (XmlNode node in sclist)
                {
                    Rectangle rect = Tools.StringTools.StringToRectangle(node.SelectSingleNode("RECTANGLE").InnerText);
                    string name = node.SelectSingleNode("NAME").InnerText;
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
                    m_singlechoicearea.Add(new SingleChoiceArea(rect, name, llistp, ssize));
                }
                

            }
            catch
            {
                ResetData();
                return false;
            }
            return true;
        }
        public bool LoadXml(XmlNode xmlDocTemplate)
        {
            ResetData();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                //XmlElement root = xmlDoc.CreateElement("TEMPLATECONFIG");
                //xmlDoc.AppendChild(root);
                xmlDoc.AppendChild(xmlDoc.ImportNode(xmlDocTemplate, true));
                //xmlDoc.AppendChild(xmlDocTemplate);
                _imagefilename = xmlDoc.SelectSingleNode(NodeName + "/BASE/PATH").InnerText;
                Size  Imgsize = Tools.StringTools.StringToSize(xmlDoc.SelectSingleNode(NodeName + "/BASE/SIZE").InnerText);
                Bitmap bitmap = (Bitmap)Bitmap.FromFile(_imagefilename);
                if (bitmap.Size != Imgsize)
                    return false;

              ;
                XmlNodeList sclist = xmlDoc.SelectNodes(NodeName + "/SINGLECHOICES/*");
                

                foreach (XmlNode node in sclist)
                {
                    Rectangle rect = Tools.StringTools.StringToRectangle(node.SelectSingleNode("RECTANGLE").InnerText);
                    string name = node.SelectSingleNode("NAME").InnerText;
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
                    m_singlechoicearea.Add(new SingleChoiceArea(rect, name, llistp, ssize));
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
            String keyname = "特征点";
            TreeNodeCollection fp = m_tn.Nodes[keyname].Nodes;
            //foreach (TriAngleFeature ft in m_fp)
            //{
            //    TreeNode t = new TreeNode();
            //    int pointnum = fp.Count + 1;
            //    t.Name = t.Text = keyname + pointnum;
            //    t.Tag = ft;
            //    fp.Add(t);
            //}
            keyname = "考号";
            TreeNodeCollection kh = m_tn.Nodes[keyname].Nodes;
            foreach (KaoHaoChoiceArea sca in m_kaohaochoicearea)
            {
                TreeNode t = new TreeNode();
                int pointnum = kh.Count + 1;
                t.Name = t.Text = sca.Name;
                t.Tag = sca;
                kh.Add(t);
            }
            keyname = "选择题";
            TreeNodeCollection sc = m_tn.Nodes[keyname].Nodes;
            foreach (SingleChoiceArea sca in m_singlechoicearea)
            {
                TreeNode t = new TreeNode();
                int pointnum = sc.Count + 1;
                t.Name = t.Text = sca.Name;
                t.Tag = sca;
                sc.Add(t);
            }
        }
        
        public String NodeName{ get { return "TEMPLATE"; } }
        public Size Imgsize
        {
            get { return _src.Size; }
        }
        public String Filename
        {
            get { return _imagefilename; }
        }

        private List<SingleChoice> m_singlechoice;
        private List<SingleChoiceArea> m_singlechoicearea;
        private List<KaoHaoChoiceArea> m_kaohaochoicearea;
        private Bitmap _src;
        private string _imagefilename;
        private Rectangle _correctrect;



        public Bitmap Image { get { return _src; } }
    }
}
