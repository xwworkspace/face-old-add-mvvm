using FaceSysByMvvm.Common;
using FaceSysByMvvm.ResourcesDictionary;
using FaceSysByMvvm.Services;
using FaceSysByMvvm.View;
using FaceSysByMvvm.View.ChannelManage;
using FaceSysClient.ClassPool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace FaceSysByMvvm
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        WriteLog _WriteLog = new WriteLog();
        DispatcherTimer autoLogin = new DispatcherTimer();//自动登录
        string isAutoLOgin = "";
        //客户端类型 0为筛选 1为接收
        public static string ClientType = "";
        //客户端所属区域
        public static int  ClientArea = -1;
        public static string ClientAreaName = "";
        public Login()
        {
            try
            {
                InitializeComponent();
                this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
                SetIpAndPort();
                autoLogin.Tick += new EventHandler(AutoLogin);
                autoLogin.Interval = TimeSpan.FromSeconds(5);   //设置刷新的间隔时间

                isAutoLOgin = ConfigurationManager.AppSettings["是否自动登录"];
                if (isAutoLOgin == "0")
                {
                    autoLogin.Start();
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("Login", ex.Message);
            }
        }

        private void AutoLogin(object sender, EventArgs e)
        {
            Button_Click(null, null);           
        }

        /// <summary>
        /// 初始化IP和Port
        /// </summary>
        private void SetIpAndPort()
        {
            XmlDocument xmlDoc = new XmlDocument();
            // 2.读取的xml文档加载进来
            string strXmlPath = System.Environment.CurrentDirectory + @"\XMl\port.xml";
            xmlDoc.Load(strXmlPath);

            // 3.读取你指定的节点
            XmlNodeList lis = xmlDoc.GetElementsByTagName("IpAdress");
            //获得节点下的所有子节点
            XmlNodeList xndList = lis[0].ChildNodes;

            //获得第一个子节点,得到抓拍ID
            XmlNode xnd0 = xndList[0];
            string strIp = xnd0.InnerText.ToString();
            IP.Text = strIp;
            XmlNode xnd1 = xndList[1];
            string strPort = xnd1.InnerText.ToString();
            Port.Text = strPort;
        }

        /// <summary>
        /// 拖动窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// 点击登录按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WarningMessageWindow warn = new WarningMessageWindow();
            warn.Show();
            return;
            IThirtfService its = new ThirftService();
            try
            {
                if (its.Login(IP.Text.ToString().Trim(), int.Parse(Port.Text.Trim())))
                {
                    if (isAutoLOgin == "0")
                    {
                        autoLogin.Stop();
                    }   
                    RemberIPAndPort();
                    ClientType = ConfigurationManager.AppSettings["功能选择"];
                    ClientArea = Convert.ToInt32(ConfigurationManager.AppSettings["所属区域"]);
                    BasicInfo.SetBasicInfo();
                    foreach (var config in BasicInfo.ConfigList)
                    {
                        if (config.AreaType == ClientArea)
                        {
                            ClientAreaName = config.AreaName;
                        }
                    }
                    MainWindow _mainWindow = new MainWindow();
                    this.Hide();
                    _mainWindow.Show();
                }
                else
                {
                    if (isAutoLOgin != "0")
                    {
                        MessageBox.Show("失败");
                    }             
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("登录失败");
                _WriteLog.WriteToLog("登录失败", ex.Message);
            }
        }

        /// <summary>
        /// 保存IP和密端口
        /// </summary>
        public void RemberIPAndPort()
        {
            try
            {
                // 1.创建一个XmlDocument类的对象
                XmlDocument xmlDoc = new XmlDocument();
                // 2.读取的xml文档加载进来
                string strXmlPath = System.Environment.CurrentDirectory + @"\XMl\port.xml";
                xmlDoc.Load(strXmlPath);

                XmlNode root = xmlDoc.SelectSingleNode("Root");//查找<Root>  
                //当超过10页时删除第一条
                XmlNodeList lis = xmlDoc.GetElementsByTagName("IpAdress");
                for (int i = 0; i < lis.Count; i++)
                {
                    root.RemoveChild(lis[i]);
                }

                XmlElement xe1 = xmlDoc.CreateElement("IpAdress");//创建一个<CaptureRealTimeInfo>节点
                //xe1.SetAttribute("Id", Id.ToString());//设置该节点genre属性

                XmlElement xesub0 = xmlDoc.CreateElement("IP");
                xesub0.InnerText = IP.Text.ToString().Trim();//设置文本节点
                xe1.AppendChild(xesub0);//添加到<CaptureRealTimeInfo>节点中

                XmlElement xesub1 = xmlDoc.CreateElement("Port");
                xesub1.InnerText = Port.Text.Trim();//设置文本节点
                xe1.AppendChild(xesub1);//添加到<CaptureRealTimeInfo>节点中

                root.AppendChild(xe1);//添加到<bookstore>节点中
                xmlDoc.Save(strXmlPath);
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("保存用户信息失败", ex);
                //MyMessage.showYes(ex.Message); 
            }
        }


    }
}
