using FaceSysByMvvm.Services;
using FaceSysClient.ClassPool;
using Prism.Mvvm;
using System.Collections.Generic;

namespace FaceSysByMvvm.ViewModel.CaptureRecordQuery
{
    public class CaptureRecordQueryViewModel:BindableBase
    {
        ThirftService thirft = new ThirftService();
        #region 属性和命令的声明
        public class CaptureRecordQueryValue
        {
            /// <summary>
            /// 选择的通道
            /// </summary>
            public string ChannelValue { get; set; }
            /// <summary>
            /// 开始日期
            /// </summary>
            public long StartDayValue { get; set; }
            /// <summary>
            /// 结束日期
            /// </summary>
            public long EndDayValue { get; set; }
            /// <summary>this.SetProperty(ref id, value);
            /// 每页现实的行数
            /// </summary>
            public int PageRowValue { get; set; }
            /// <summary>
            /// 从第几行开始获取
            /// </summary>
            public int StartRowValue { get; set; }

            public int MaxCount { get; set; }
        }

        //通道      
        private int selectedChannel ;
        public int SelectedChannel
        {
            get { return selectedChannel; }
            set
            {
                this.SetProperty(ref selectedChannel, value);
            }
        }
        private List<string> channel ;
        public List<string> Channel
        {
            get { return channel; }
            set
            {
                this.SetProperty(ref channel, value);
            }
        }

        public List<string> ChannelId ;
        //开始日期
        private string startDay ;
        public string StartDay
        {
            get { return startDay; }
            set
            {
                this.SetProperty(ref startDay, value);
            }
        }

        //开始时辰
        private int selectedStartHour ;
        public int SelectedStartHour
        {
            get { return selectedStartHour; }
            set
            {
                SetProperty(ref selectedStartHour, value);
            }
        }
        private List<string> startHour ;
        public List<string> StartHour
        {
            get { return startHour; }
            set
            {
                SetProperty(ref startHour, value);
            }
        }


        private List<string> startMinutes ;
        /// <summary>
        /// 开始分钟
        /// </summary>
        public List<string> StartMinutes
        {
            get { return startMinutes; }
            set
            {
                SetProperty(ref startMinutes, value);
            }
        }

        //结束日期
        private string endDay ;
        public string EndDay
        {
            get { return endDay; }
            set
            {
                SetProperty(ref endDay, value);
            }
        }

        //结束时辰
        private int selectedEndHour ;
        public int SelectedEndHour
        {
            get { return selectedEndHour; }
            set
            {
                SetProperty(ref selectedEndHour, value);
            }
        }
        private List<string> endHour ;
        public List<string> EndHour
        {
            get { return endHour; }
            set
            {
                SetProperty(ref endHour, value);
            }
        }

        private List<string> endMinutes ;
        /// <summary>
        /// 结束分钟
        /// </summary>
        public List<string> EndMinutes
        {
            get { return endMinutes; }
            set
            {
                SetProperty(ref endMinutes, value);
            }
        }

        //loading动画显示
        private string loadingVisiblity ;
        public string LoadingVisiblity
        {
            get { return loadingVisiblity; }
            set
            {
                SetProperty(ref loadingVisiblity, value);
            }
        }

        //查询结果总数量
        private int maxCount ;
        public int MaxCount
        {
            get { return maxCount; }
            set
            {
                SetProperty(ref maxCount, value);
            }
        }

        //当前页码
        private int currPage ;
        public int CurrPage
        {
            get { return currPage; }
            set
            {
                SetProperty(ref currPage, value);
            }
        }

        //总的页码数
        private int maxPage ;
        public int MaxPage
        {
            get { return maxPage; }
            set
            {
                SetProperty(ref maxPage, value);
            }
        }

        //每页显示的行数
        private int selectedPageRow ;
        public int SelectedPageRow
        {
            get { return selectedPageRow; }
            set
            {
                SetProperty(ref selectedPageRow, value);
            }
        }
        private List<string> pageRow ;
        public List<string> PageRow
        {
            get { return pageRow; }
            set
            {
                SetProperty(ref pageRow, value);
            }
        }
        //抓拍记录结果
        private List<MyCapFaceLogWithImg> listMyCapFaceLogWithImg ;
        public List<MyCapFaceLogWithImg> ListMyCapFaceLogWithImg
        {
            get { return listMyCapFaceLogWithImg; }
            set
            {
                SetProperty(ref listMyCapFaceLogWithImg, value);
            }
        }
        //查询时条件值
        public CaptureRecordQueryValue captureRecordQueryValue;
        #endregion

        #region 初始化和命令的执行和执行条件
        public CaptureRecordQueryViewModel()
        {
            //初始化条件的初值
            captureRecordQueryValue = new CaptureRecordQueryValue();

            //初始化通道
            Channel = new List<string>();
            ChannelId = new List<string>();
            RefreshChannelList();
            //foreach (MyChannelCfg mcc in thirft.QueryAllChannel())
            //{
            //    if (Login.ClientType != "0")
            //    {
            //        if (mcc.Name.Contains(Login.ClientAreaName))
            //        {
            //            Channel.Add(mcc.Name);
            //            ChannelId.Add(mcc.TcChaneelID);
            //        }
            //    }
            //    else
            //    {
            //        Channel.Add(mcc.Name);
            //        ChannelId.Add(mcc.TcChaneelID);
            //    }
            //}
            //Channel.Insert(0, "全部");
            SelectedChannel = 0;

            //初始化开始日期
            StartDay = System.DateTime.Today.ToShortDateString();

            //初始化结束日期
            EndDay = "";

            //初始化开始时辰和结束时辰和选择时间
            StartHour = new List<string>();
            EndHour = new List<string>();
            for (int i = 0; i <= 23; i++)
            {
                StartHour.Add(i + ":00");
                EndHour.Add(i + 1 + ":00");
            }
            SelectedEndHour = 23;
            SelectedStartHour = 0;
            //初始化开始和结束分钟
            StartMinutes = new List<string>();
            EndMinutes = new List<string>();
            for (int i = 0; i <= 59; i++)
            {
                StartMinutes.Add(i + "");
                EndMinutes.Add(i + 1 + "");
            }

            //初始化loading图片 Hidden Visible
            LoadingVisiblity = "Hidden";

            //初始化每页行数
            SelectedPageRow = 2;
            PageRow = new List<string>() { "5", "10", "15", "30", "60" };

            //初始化当前页
            CurrPage = 0;
            //初始化最大页
            MaxPage = 0;
            //初始化最大查询数
            MaxCount = 0;
        }

        internal void RefreshChannelList()
        {
            var ChannelTemp = new List<string>();
            var ChannelIdTemp = new List<string>();
            foreach (MyChannelCfg mcc in thirft.QueryAllChannel())
            {
                if (Login.ClientType == "1")
                {
                    if (mcc.Name.Contains(Login.ClientAreaName))
                    {
                        ChannelTemp.Add(mcc.Name);
                        ChannelIdTemp.Add(mcc.TcChaneelID);
                    }
                }
                else
                {
                    ChannelTemp.Add(mcc.Name);
                    ChannelIdTemp.Add(mcc.TcChaneelID);
                }
            }
            ChannelTemp.Insert(0, "全部");
            Channel = ChannelTemp;
            ChannelId = ChannelIdTemp;
        }


        #endregion

    }
}
