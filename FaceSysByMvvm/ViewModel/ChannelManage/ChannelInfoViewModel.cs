using FaceSysByMvvm.Services;
using Prism.Mvvm;
using System.Collections.Generic;

namespace FaceSysByMvvm.ViewModel.ChannelManage
{
    public class ChannelInfoViewModel : BindableBase
    {
        ThirftService thirft = new ThirftService();
        #region 属性的声明
        private string title ;
        /// <summary>
        /// 标题栏
        /// </summary>
        public string Title
        {
            get { return title; }
            set
            {
                this.SetProperty(ref title, value);
            }
        }

        #region 通道参数设置

        private string channelId ;
        /// <summary>
        /// 通道编号
        /// </summary>
        public string ChannelId
        {
            get { return channelId; }
            set
            {
                this.SetProperty(ref channelId, value);
            }
        }

        private string channelName ;
        /// <summary>
        /// 通道名称
        /// </summary>
        public string ChannelName
        {
            get { return channelName; }
            set
            {
                this.SetProperty(ref channelName, value);
            }
        }

        private int selectedChannelType ;
        /// <summary>
        /// 通道类型选中项
        /// </summary>
        public int SelectedChannelType
        {
            get { return selectedChannelType; }
            set
            {
                this.SetProperty(ref selectedChannelType, value);
            }
        }

        private List<string> channelType ;
        /// <summary>
        /// 通道类型
        /// </summary>
        public List<string> ChannelType
        {
            get { return channelType; }
            set
            {
                this.SetProperty(ref channelType, value);
            }
        }

        private string captureAddr ;
        /// <summary>
        /// 抓拍服务器地址
        /// </summary>
        public string CaptureAddr
        {
            get { return captureAddr; }
            set
            {
                SetProperty(ref captureAddr, value);
            }
        }


        private string capturePort ;
        /// <summary>
        /// 视频源端口
        /// </summary>
        public string CapturePort
        {
            get { return capturePort; }
            set
            {
                SetProperty(ref capturePort, value);
            }
        }

        private string remark ;
        /// <summary>
        /// 通道备注
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set
            {
                SetProperty(ref remark, value);
            }
        }

        #endregion

        #region 图像源参数设置

        private int selectedType ;
        /// <summary>
        /// 视频源类型选中项
        /// </summary>
        public int SelectedType
        {
            get { return selectedType; }
            set
            {
                SetProperty(ref selectedType, value);
            }
        }

        private List<string> captureType ;
        /// <summary>
        /// 视频源类型
        /// </summary>
        public List<string> CaptureType
        {
            get { return captureType; }
            set
            {
                SetProperty(ref captureType, value);
            }
        }

        private string videoAddr ;
        /// <summary>
        /// 视频源地址
        /// </summary>
        public string VideoAddr
        {
            get { return videoAddr; }
            set
            {
                SetProperty(ref videoAddr, value);
            }
        }


        private string videoPort ;
        /// <summary>
        /// 视频源端口
        /// </summary>
        public string VideoPort
        {
            get { return videoPort; }
            set
            {
                SetProperty(ref videoPort, value);
            }
        }


        private string uID ;
        /// <summary>
        /// 登录相机用户名
        /// </summary>
        public string UID
        {
            get { return uID; }
            set
            {
                SetProperty(ref uID, value);
            }
        }


        private string pSW ;
        /// <summary>
        /// 登录相机端密码
        /// </summary>
        public string PSW
        {
            get { return pSW; }
            set
            {
                SetProperty(ref pSW, value);
            }
        }


        #endregion

        #region 人脸抓拍参数设置

        private string minFace ;
        /// <summary>
        /// 最小检测尺寸
        /// </summary>
        public string MinFace
        {
            get { return minFace; }
            set
            {
                SetProperty(ref minFace, value);
            }
        }

        private string minQuality ;
        /// <summary>
        /// 最小有效图像质量
        /// </summary>
        public string MinQuality
        {
            get { return minQuality; }
            set
            {
                SetProperty(ref minQuality, value);
            }
        }

        private string minCapDistance ;
        /// <summary>
        /// 最小采集帧间隔
        /// </summary>
        public string MinCapDistance
        {
            get { return minCapDistance; }
            set
            {
                SetProperty(ref minCapDistance, value);
            }
        }

        private string maxFaceSaveDistance ;
        /// <summary>
        /// 最大人脸存储间隔
        /// </summary>
        public string MaxFaceSaveDistance
        {
            get { return maxFaceSaveDistance; }
            set
            {
                SetProperty(ref maxFaceSaveDistance, value);
            }
        }

        private string yaw ;
        /// <summary>
        /// x轴 俯仰角
        /// </summary>
        public string Yaw
        {
            get { return yaw; }
            set
            {
                SetProperty(ref yaw, value);
            }
        }

        private string pitch ;
        /// <summary>
        /// y轴 偏航角
        /// </summary>
        public string Pitch
        {
            get { return pitch; }
            set
            {
                SetProperty(ref pitch, value);
            }
        }

        private string yoll ;
        /// <summary>
        /// z轴 翻滚角
        /// </summary>
        public string Yoll
        {
            get { return yoll; }
            set
            {
                SetProperty(ref yoll, value);
            }
        }
        #endregion

        #endregion

        #region 初始化
        public ChannelInfoViewModel()
        {
            Title = "添加通道";
            //初始化通道参数
            ChannelId = System.Guid.NewGuid().ToString().Replace("-", "");
            ChannelName = "";
            ChannelType = thirft.QueryDefChannelType();
            SelectedChannelType = 0;
            CaptureAddr = "";
            CapturePort = "";
            Remark = "";
            //初始化图像源参数
            CaptureType = thirft.QueryDefCameraType();
            SelectedType = 3;
            VideoAddr = "";
            VideoPort = "";
            UID = "";
            PSW = "";
            ResetFaceCap();
        }

        public void ResetFaceCap()
        {
            //初始化人脸抓拍参数
            MinFace = "96";
            MinQuality = "5";
            MinCapDistance = "2";
            MaxFaceSaveDistance = "0";
            Yaw = "30";
            Pitch = "30";
            Yoll = "30";
        }
        #endregion
    }
}
