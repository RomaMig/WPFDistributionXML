using Parser.Interface;
using System;
using System.IO;
using System.Xml;

namespace Parser
{
    public class XmlParser : IXmlParser
    {
        private XmlDocument xDoc;
        public void Load(string path)
        {
            xDoc = new XmlDocument();
            xDoc.Load(path);
        }

        public IPage Parse()
        {
            Page page = new Page();
            XmlElement xRoot = xDoc.DocumentElement;
            if (xRoot == null)
                return null;

            foreach (XmlElement xnode in xRoot)
            {
                XmlNode formatVersion = xnode.Attributes.GetNamedItem("FormatVersion");
                XmlNode from = xnode.Attributes.GetNamedItem("from");
                XmlNode to = xnode.Attributes.GetNamedItem("to");
                page.FormatVersion = int.Parse(formatVersion?.Value);
                page.From = from?.Value;
                page.To = to?.Value;

                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    XmlNode id = childnode.Attributes.GetNamedItem("id");
                    page.Id = int.Parse(id?.Value);

                    foreach (XmlNode cn in childnode.ChildNodes)
                    {
                        if (cn.Name == "text")
                        {
                            XmlNode color = cn.Attributes.GetNamedItem("color");
                            page.Text = cn.InnerText;
                            page.TextColor = color?.Value;
                        }
                        if (cn.Name == "image")
                        {
                            page.Image = cn.InnerText;
                        }
                    }
                }
            }
            return page;
        }
    }
}
