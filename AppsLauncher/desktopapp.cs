using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KralAppLauncher
{
    [XmlRoot(ElementName ="desktopapp")]
    public class desktopapp
    {
        [XmlAttribute]
        public int id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public List<manual> manuals { get; set; }
    }

    [XmlRoot(ElementName = "manual")]
    public class manual
    {
        public string name { get; set; }
        public string path { get; set; }
    }
}
