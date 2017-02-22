using FaceSysByMvvm.Services;
using FaceSysClient.ClassPool;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FaceSysByMvvm.ViewModel.ChannelManage
{
    public class ChannelCompareViewModel:BindableBase
    {
        public class CapInfo
        {
            public String Key { get; set; }
            public string Value { get; set; }
        }
        ThirftService thirft = new ThirftService();
        #region 定义属性

        private string id;
        /// <summary>
        /// 抓拍ID
        /// </summary>
        public string Id
        {
            get { return id; }
            set
            {
                this.SetProperty(ref id, value);
            }
        }

        private ImageSource image;

        /// <summary>
        /// 抓拍照片
        /// </summary>
        public ImageSource Image
        {
            get { return image; }
            set
            {
                this.SetProperty(ref image, value);
            }
        }

        //每页显示的行数
        private int selectedPageRow;
        public int SelectedPageRow
        {
            get { return selectedPageRow; }
            set
            {
                this.SetProperty(ref selectedPageRow, value);
            }
        }
        private List<string> pageRow;
        public List<string> PageRow
        {
            get { return pageRow; }
            set
            {
                this.SetProperty(ref pageRow, value);
            }
        }


        //比对的模版
        private List<MyFaceObj> myFaceObjList;
        public List<MyFaceObj> MyFaceObjList
        {
            get { return myFaceObjList; }
            set
            {
                this.SetProperty(ref myFaceObjList, value);
            }
        }

        //比对模版信息
        private List<CapInfo> capInfoList;
        public List<CapInfo> CapInfoList
        {
            get { return capInfoList; }
            set
            {
                this.SetProperty(ref capInfoList, value);
            }
        }

        //模版类型
        public List<string> Type { get; set; }

        //模版性别
        public List<string> Sex { get; set; }

        private List<string> templateCount;
        /// <summary>
        ///  显示的模版数量
        /// </summary>
        public List<string> TemplateCount
        {
            get { return templateCount; }
            set
            {
                this.SetProperty(ref templateCount, value);
            }
        }

        public string Day { get; set; }

        #endregion

        #region 初始化
        public ChannelCompareViewModel()
        {
            //抓拍ID
            Id = "";
            //初始化每页行数
            SelectedPageRow = 0;
            PageRow = new List<string>() { "5", "10", "15", "30", "60" };
            //初始化抓拍照片
            Image = new BitmapImage(new Uri("pack://application:,,,/Images/照片选取后前景.png"));
            //初始化模版类型
            Type = thirft.QueryDefFaceObjType();
            //初始化模版性别
            Sex = new List<string> { "全部", "男", "女" };
            //初始化模版数量
            TemplateCount = new List<string>();
            TemplateCount.Add("2");
            TemplateCount.Add("3");
            TemplateCount.Add("5");
            TemplateCount.Add("10");
            TemplateCount.Add("15");
            TemplateCount.Add("20");
            TemplateCount.Add("25");

            MyFaceObjList = new List<MyFaceObj>();
            CapInfoList = new List<CapInfo>();
        }
        #endregion
    }
}
