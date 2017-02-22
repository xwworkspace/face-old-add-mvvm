using FaceSysByMvvm.Common;
using FaceSysByMvvm.Services;
using FaceSysClient.ClassPool;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FaceSysByMvvm.View.ChannelManage
{
    /// <summary>
    /// WarningMessage.xaml 的交互逻辑
    /// </summary>
    public partial class WarningMessage : Window
    {
        string ChannelName = "";
        RealtimeCmpInfo info;
        WriteLog _WriteLog = new WriteLog();
        ThirftService thirft = new ThirftService();
        public WarningMessage()
        {
            InitializeComponent();
            if(Login.ClientType == "1")
            {
                buttonSend.Visibility = Visibility.Hidden;
            }
            this.MouseLeftButtonDown += WarningMessage_MouseLeftButtonDown;
        }

        private void WarningMessage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //关闭弹窗
        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 设置报警弹窗信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="channelName"></param>
        /// <param name="time"></param>
        /// <param name="type"></param>
        /// <param name="socre"></param>
        /// <param name="capImg"></param>
        /// <param name="regImg"></param>
        /// <param name="realtimeCmpInfo"></param>
        internal void SetWarningInfo(string name, string channelName, long time, string type, string socre, ImageSource capImg, ImageSource regImg, RealtimeCmpInfo realtimeCmpInfo)
        {
            try
            {
                label_Socre.Text = label_Socre.Text.ToString().Replace("Socre", socre);
                label_TemplateName.Text = label_TemplateName.Text.ToString().Replace("TemplateName", name);
                label_TemplateType.Text = label_TemplateType.Text.ToString().Replace("TemplateType", type);
                long _longtime = time;
                DateTime s = new DateTime(1970, 1, 1);
                s = s.AddSeconds(_longtime);
                label_CapTime.Text = label_CapTime.Text.ToString().Replace("CapTime", s.ToString());
                label_CapChannel.Text = label_CapChannel.Text.ToString().Replace("CapChannel", channelName);
                image_CapImage.Source = capImg;
                image_RegImage.Source = regImg;
                info = realtimeCmpInfo;
                ChannelName = channelName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SendCmpToClient(object sender, RoutedEventArgs e)
        {
            try
            {
                string errMsg = "";
                foreach (var area in BasicInfo.ConfigList)
                {
                    if (ChannelName.Contains(area.AreaName))
                    {
                        foreach (var ip in area.ReceiveIPOfArea)
                        {
                            if (UpdateCmp(ip) != 0)
                            {
                                errMsg += ip + ";";
                            }
                            else
                            {
                                thirft.UpdateCmpLog(info.CapID, info.ObjID, System.DateTime.Now.ToString("yyyyMMdd"), area.AreaType);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessage.showYes("推送比对信息出错");
                _WriteLog.WriteToLog("SendCmpToClient", ex);
            }
        }

        public void SetWarningRed()
        {
            BodyGrid.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/添加通道子菜单（修改）.png"))
            };
            label_title.Foreground = new SolidColorBrush(Colors.White);
            label_Socre.Foreground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// 推送比对记录
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public int UpdateCmp(string IP)
        {
            try
            {
                // 生成socket套接字；
                Thrift.Transport.TSocket tsocket = new Thrift.Transport.TSocket(IP, 6000);
                //设置连接超时为100；
                tsocket.Timeout = 3000;
                //生成客户端对象
                Thrift.Transport.TTransport transport = tsocket;
                Thrift.Protocol.TProtocol protocol = new Thrift.Protocol.TBinaryProtocol(transport);
                UIServer.Client _BusinessServerClient = new UIServer.Client(protocol);
                transport.Open();
                _BusinessServerClient.UpdateRealtimeCmp(info, "##" + ChannelName);
                transport.Close();
                return 0;
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("UpdateCmp", ex);
                return -1;
            }

        }

        private void RefreshPicture(object sender, MouseButtonEventArgs e)
        {
            var imageCap = image_CapImage.Source;
            image_CapImage.Source = null;
            image_CapImage.Source = imageCap;
            var imageReg = image_RegImage.Source;
            image_RegImage.Source = null;
            image_RegImage.Source = imageReg;
        }
    }
}
