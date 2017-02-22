using FaceSysByMvvm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace FaceSysByMvvm.Common
{
    public class BasicInfo
    {
        public class SARConfig
        {
            public string AreaName { get; set; }
            public int AreaType { get; set; }
            private List<string> receiveIPOfArea = new List<string>();
            public List<string> ReceiveIPOfArea
            {
                get { return receiveIPOfArea; }
                set { receiveIPOfArea = value; }
            }
            public int Alarmnum { get; set; }
            public int Threshold { get; set; }
        }

        public static List<STypeInfo> DefGender = new List<STypeInfo>();
        public static List<STypeInfo> DefFaceObjType = new List<STypeInfo>();
        public static List<STypeInfo> DefChannelType = new List<STypeInfo>();
        public static List<STypeInfo> DefCameraType = new List<STypeInfo>();
        public static List<SARConfig> ConfigList = new List<SARConfig>();
        static ThirftService thirft = new ThirftService();

        public static void SetBasicInfo()
        {
            DefGender = thirft.QueryDefGenderH(-1);
            DefFaceObjType = thirft.QueryDefFaceObjTypeH(-1);
            DefChannelType = thirft.QueryDefChannelTypeH(-1);
            DefCameraType = thirft.QueryDefCameraTypeH(-1);
            ConfigList = GetAllConfigList();
        }

        public static List<SARConfig> GetAllConfigList()
        {
            List<SARConfig> configList = new List<SARConfig>();
            XmlDocument xmlDoc = new XmlDocument();
            // 2.读取的xml文档加载进来
            string strXmlPath = System.Environment.CurrentDirectory + @"\XMl\AreaInfo.xml";
            xmlDoc.Load(strXmlPath);
            XmlNode xn = xmlDoc.SelectSingleNode("xml");
            // 得到根节点的所有子节点
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xn1 in xnl)
            {
                SARConfig config = new SARConfig();
                XmlElement xe = (XmlElement)xn1;
                XmlNodeList xnl0 = xe.ChildNodes;
                config.AreaType = Convert.ToInt32(xnl0.Item(0).InnerText);
                config.AreaName = xnl0.Item(1).InnerText;
                XmlNodeList xml02 = xnl0.Item(2).ChildNodes;
                foreach (XmlNode receiveIP in xml02)
                {
                    string IPAddressFormartRegex = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
                    // 检查输入的字符串是否符合IP地址格式
                    if (Regex.IsMatch(receiveIP.InnerText, IPAddressFormartRegex))
                    {
                        config.ReceiveIPOfArea.Add(receiveIP.InnerText);
                    }   
                }
                config.Alarmnum = Convert.ToInt32(xnl0.Item(3).InnerText);
                config.Threshold = Convert.ToInt32(xnl0.Item(4).InnerText);
                configList.Add(config);
            }
            return configList;
        }

        public static string GetTypeById(int typeId)
        {
            foreach (var basicinfo in BasicInfo.DefFaceObjType)
            {
                if (basicinfo.Type == typeId)
                {
                    return basicinfo.Description; // 类型  
                }
            }
            return "";
        }
    }
}
