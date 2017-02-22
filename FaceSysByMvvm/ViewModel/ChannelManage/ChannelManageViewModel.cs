using FaceSysByMvvm.Services;
using FaceSysClient.ClassPool;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace FaceSysByMvvm.ViewModel
{
    public class ChannelManageViewModel:BindableBase
    {
        private int capImageCount ;
        /// <summary>
        /// 抓拍照片数量
        /// </summary>
        public int CapImageCount
        {
            get { return capImageCount; }
            set
            {
                SetProperty(ref capImageCount, value);
            }
        }

        private int comImageCount ;
        /// <summary>
        /// 比对照片数量
        /// </summary>
        public int ComImageCount
        {
            get { return comImageCount; }
            set
            {
                SetProperty(ref comImageCount, value);
            }
        }

        private List<ChannelListItemViewModel> channelList;

        public List<ChannelListItemViewModel> ChannelList
        {
            get { return channelList; }
            set
            {
                SetProperty(ref channelList, value);
            }
        }

        private int selectedThreshold ;
        /// <summary>
        /// 阈值选中项
        /// </summary>
        public int SelectedThreshold
        {
            get { return selectedThreshold; }
            set
            {
                SetProperty(ref selectedThreshold, value);
            }
        }

        private List<string> threshold ;
        /// <summary>
        /// 阈值
        /// </summary>
        public List<string> Threshold
        {
            get { return threshold; }
            set
            {
                SetProperty(ref threshold, value);
            }
        }
        

        public ChannelManageViewModel()
        {
            ChannelList = new List<ChannelListItemViewModel>();
            foreach (MyChannelCfg mcc in new ThirftService().QueryAllChannel())
            {
                ChannelListItemViewModel channel = new ChannelListItemViewModel();
                channel.MyChannelCfg = mcc;
                channel.IsOpened = false;
                ChannelList.Add(channel);
            }
            //RefreshChannelList();
            CapImageCount = 0;
            ComImageCount = 0;

            //初始化阈值
            Threshold = new List<string>();
            for (int i = 1; i < 100; i++)
            {
                Threshold.Add(i+"");
            }
            //初始化阈值选中项
            //SelectedThreshold = Convert.ToInt32(ConfigurationManager.AppSettings["阈值"]) - 1 ;
        }

        public void RefreshChannelList()
        {
            List<ChannelListItemViewModel> ChannelListTemp = new List<ChannelListItemViewModel>();
            foreach (MyChannelCfg mcc in new ThirftService().QueryAllChannel())
            {
                if (Login.ClientType == "1")
                {
                    if (mcc.Name.Contains(Login.ClientAreaName))
                    {
                        ChannelListItemViewModel channel = new ChannelListItemViewModel();
                        channel.MyChannelCfg = mcc;
                        channel.IsOpened = false;
                        ChannelListTemp.Add(channel);
                    }
                }
                else
                {
                    ChannelListItemViewModel channel = new ChannelListItemViewModel();
                    channel.MyChannelCfg = mcc;
                    channel.IsOpened = false;
                    ChannelListTemp.Add(channel);
                }
            }
            foreach(ChannelListItemViewModel cLI in ChannelList)
            {
                if(cLI.IsOpened == true)
                {
                    ChannelListTemp.Where(p => p.MyChannelCfg.TcChaneelID == cLI.MyChannelCfg.TcChaneelID).ToList()[0].IsOpened = true;
                }
            }
            ChannelList = ChannelListTemp;
        }
    }
}
