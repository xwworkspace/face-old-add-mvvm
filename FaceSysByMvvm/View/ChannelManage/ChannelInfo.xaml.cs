using FaceSysByMvvm.Services;
using FaceSysByMvvm.ViewModel.ChannelManage;
using FaceSysClient.ClassPool;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FaceSysByMvvm.View.ChannelManage
{
    /// <summary>
    /// ChannelInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ChannelInfo : Window
    {
        MyChannelCfg _ChannelCfg = new MyChannelCfg();
        ChannelInfoViewModel cIViewModel;
        public System.IO.FileInfo[] filesPic;
        WriteLog _WriteLog = new WriteLog();
        ThirftService thirft = new ThirftService();
        validationRule _validationRule = new validationRule();
        public delegate void NoArgsMethodDelegate();
        public NoArgsMethodDelegate RefreshChannelDelegate;
        public NoArgsMethodDelegate CloseVideoDelegate;
        public ChannelInfo()
        {
            InitializeComponent();
            cIViewModel = new ChannelInfoViewModel();
            this.DataContext = cIViewModel;
            this.MouseLeftButtonDown += ChannelInfo_MouseLeftButtonDown;
            btnChannelNumGeneration.IsEnabled = true;
        }

        private void ChannelInfo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// 重新生成编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChannelNumGeneration_Click(object sender, RoutedEventArgs e)
        {
            cIViewModel.ChannelId = System.Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirmAdd_Click(object sender, RoutedEventArgs e)
        {
            int nSucess = -1;
            //判断是添加还是修改
            if(!CheckInfo())
            {
                return;
            }
            ChannelCfg channelCfg = new ChannelCfg();
            channelCfg = _ChannelCfg.MyChannelCfgToChannelCfg(_ChannelCfg);
            if (cIViewModel.Title.Equals("添加通道"))
            {
                nSucess = thirft.AddChannel(channelCfg);
            }
            else
            {
                nSucess = thirft.ModifyChannel(channelCfg);
                
            }
            if (nSucess == 0)
            {
                MyMessage.showYes("操作成功！");
                //修改通道 如果通道已经被打开则关闭
                if(!cIViewModel.Title.Equals("添加通道"))
                {
                    CloseVideoDelegate();
                }
                this.Close();
            }
            else
            {
                MyMessage.showYes("操作失败！");
            }
            RefreshChannelDelegate();
        }

        /// <summary>
        /// 检测各项的值
        /// </summary>
        private bool CheckInfo()
        {
            try
            {
                //通道uuid，默认生成
                if (!string.IsNullOrEmpty(cIViewModel.ChannelId))
                {
                    _ChannelCfg.TcChaneelID = cIViewModel.ChannelId;
                }
                //通道名称，必填项
                if (!string.IsNullOrEmpty(cIViewModel.ChannelName))
                {
                    _ChannelCfg.Name = cIViewModel.ChannelName;
                }
                else
                {
                    //MyMessage.showYes("通道名称必填！");
                    MyMessage.showYes("通道名称必填");
                    return false;
                }
                //备注
                if (!string.IsNullOrEmpty(cIViewModel.Remark))
                {
                    _ChannelCfg.TcDescription = cIViewModel.Remark;
                }
                //抓拍服务器IP，必须项
                if (!string.IsNullOrEmpty(cIViewModel.CaptureAddr))
                {
                    string message = _validationRule.ipValidationRule(cIViewModel.CaptureAddr);
                    if (message != "")
                    {
                        //MyMessage.showYes(message);
                        MyMessage.showYes(message);
                        return false;
                    }
                    _ChannelCfg.Addr = cIViewModel.CaptureAddr;
                }
                else
                {
                    MyMessage.showYes("抓拍服务器IP必填！");
                    return false;
                }
                //抓拍服务器端口必输项
                if (!string.IsNullOrEmpty(cIViewModel.CapturePort))
                {
                    string message = _validationRule.intValidationRule(cIViewModel.CapturePort);
                    if (message != "")
                    {
                        //MyMessage.showYes(message);
                        MyMessage.showYes(message);
                        return false;
                    }
                    _ChannelCfg.Port = int.Parse(cIViewModel.CapturePort);
                }
                else
                {
                    MyMessage.showYes("抓拍服务器端口必填！");
                    return false;
                }

                //视频源类型
                _ChannelCfg.CaptureCfg = new CaptureCfg();
                _ChannelCfg.CaptureCfg.NCaptureType = cIViewModel.SelectedType;
                //视频源地址
                if (!string.IsNullOrEmpty(cIViewModel.VideoAddr))
                {
                    string message = _validationRule.ipValidationRule(this.txttcAddr.Text.Trim());
                    if (message != "")
                    {
                        MyMessage.showYes(message);
                        return false;
                    }
                    _ChannelCfg.CaptureCfg.TcAddr = cIViewModel.VideoAddr;
                }
                else
                {
                    if (cIViewModel.SelectedType != 2)
                    {
                        MyMessage.showYes("视频源地址必输项");
                        return false;
                    }
                }

                //登录相机用户名
                if (!string.IsNullOrEmpty(cIViewModel.UID))
                {
                    _ChannelCfg.CaptureCfg.TcUID = cIViewModel.UID;
                }
                else
                {
                    if (cIViewModel.SelectedType != 2)
                    {
                        //MyMessage.showYes("登录相机用户名必输项");
                        MyMessage.showYes("登录相机用户名必输项");
                        return false;
                    }
                }
                //登录相机端密码
                if (!string.IsNullOrEmpty(cIViewModel.PSW))
                {
                    _ChannelCfg.CaptureCfg.TcPSW = cIViewModel.PSW;
                }
                else
                {
                    if (cIViewModel.SelectedType != 2)
                    {
                        MyMessage.showYes("登录相机端密码必输项");
                        return false;
                    }
                }
                //视频源端口
                if (!string.IsNullOrEmpty(cIViewModel.VideoPort))
                {
                    string message = _validationRule.intValidationRule(this.txtnPort.Text.Trim());
                    if (message != "")
                    {
                        //MyMessage.showYes(message);
                        MyMessage.showYes(message);
                        return false;
                    }
                    _ChannelCfg.CaptureCfg.NPort = int.Parse(cIViewModel.VideoPort);
                }
                else
                {
                        MyMessage.showYes("视频源端口必输项");
                        return false;
                }

                //人脸参数判断
                _ChannelCfg.CatchFaceCfg = new CatchFaceCfg();
                _ChannelCfg.CatchFaceCfg.NMinFace = Convert.ToInt32(cIViewModel.MinFace);
                _ChannelCfg.CatchFaceCfg.NMinQuality = Convert.ToInt32(cIViewModel.MinQuality);
                _ChannelCfg.CatchFaceCfg.NMinCapDistance = Convert.ToInt32(cIViewModel.MinCapDistance);
                _ChannelCfg.CatchFaceCfg.NMaxFaceSaveDistance = Convert.ToInt32(cIViewModel.MaxFaceSaveDistance);
                _ChannelCfg.CatchFaceCfg.NYaw = Convert.ToInt32(cIViewModel.Yaw);
                _ChannelCfg.CatchFaceCfg.NPitch = Convert.ToInt32(cIViewModel.Pitch);
                _ChannelCfg.CatchFaceCfg.NYoll = Convert.ToInt32(cIViewModel.Yoll);

            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("ChannelInfo:btnConfirmAdd_Click", ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btmPhotoSource_Click(object sender, RoutedEventArgs e)
        {
            if(ImageSetGrid.Visibility == Visibility.Visible)
            {
                ImageSetGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                ImageSetGrid.Visibility = Visibility.Visible;
            }
        }
        
        private void btnSetFaceCaptureParameters_Click(object sender, RoutedEventArgs e)
        {
            if (CapSetGrid.Visibility == Visibility.Visible)
            {
                CapSetGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                CapSetGrid.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 初始化修改通道信息
        /// </summary>
        /// <param name="_ChannelCfg"></param>
        public void SetChannelInfo(MyChannelCfg _ChannelCfg)
        {
            btnChannelNumGeneration.IsEnabled = false;
            cIViewModel.Title = "修改通道";
            cIViewModel.ChannelId = _ChannelCfg.TcChaneelID;
            cIViewModel.ChannelName = _ChannelCfg.Name;
            cIViewModel.Remark = _ChannelCfg.TcDescription;
            cIViewModel.CaptureAddr = _ChannelCfg.Addr;
            cIViewModel.CapturePort = ""+_ChannelCfg.Port;
            cIViewModel.SelectedType = _ChannelCfg.CaptureCfg.NCaptureType;
            cIViewModel.VideoAddr = _ChannelCfg.CaptureCfg.TcAddr;
            cIViewModel.UID = _ChannelCfg.CaptureCfg.TcUID;
            cIViewModel.PSW = _ChannelCfg.CaptureCfg.TcPSW ;
            cIViewModel.VideoPort = ""+_ChannelCfg.CaptureCfg.NPort;
            cIViewModel.MinFace = "" + _ChannelCfg.CatchFaceCfg.NMinFace;
            cIViewModel.MinQuality = "" + _ChannelCfg.CatchFaceCfg.NMinQuality;
            cIViewModel.MinCapDistance = "" + _ChannelCfg.CatchFaceCfg.NMinCapDistance;
            cIViewModel.MaxFaceSaveDistance = "" + _ChannelCfg.CatchFaceCfg.NMaxFaceSaveDistance;
            cIViewModel.Yaw = "" + _ChannelCfg.CatchFaceCfg.NYaw;
            cIViewModel.Pitch = "" + _ChannelCfg.CatchFaceCfg.NPitch;
            cIViewModel.Yoll = "" + _ChannelCfg.CatchFaceCfg.NYoll;
        }

        private void btnResetToDefault_Click(object sender, RoutedEventArgs e)
        {
            cIViewModel.ResetFaceCap();
        }

        /// <summary>
        /// 切换视频源类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combCaptureType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cIViewModel.SelectedType == 2)
            {
                txttcAddr.IsReadOnly = true;
                txttcUID.IsReadOnly = true;
                txttcPSW.IsReadOnly = true;
            }
            else
            {
                txttcAddr.IsReadOnly = false;
                txttcUID.IsReadOnly = false;
                txttcPSW.IsReadOnly = false;
            }
        }
    }
}
