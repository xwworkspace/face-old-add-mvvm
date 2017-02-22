using FaceSysByMvvm.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceSysByMvvm.ViewModel.TemplateManager
{
    public class TempleteInfoPopViewModel:BindableBase
    {
        ThirftService thirft = new ThirftService();
        public FaceObj _FaceObj;
        #region 属性的声明
        //窗体标题
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
            }
        }

        //模版姓名     
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
            }
        }
        //模版ID     
        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                SetProperty(ref id, value);
            }
        }
        //模版年龄
        private string age;
        public string Age
        {
            get { return age; }
            set
            {
                SetProperty(ref age, value);
            }
        }
        //模版导入时间    
        private string importTime;
        public string ImportTime
        {
            get { return importTime; }
            set
            {
                SetProperty(ref importTime, value);
            }
        }
        //模版备注 
        private string remark;
        public string Remark
        {
            get { return remark; }
            set
            {
                SetProperty(ref remark, value);
            }
        }

        //模版类型      
        private int selectedType;
        public int SelectedType
        {
            get { return selectedType; }
            set
            {
                SetProperty(ref selectedType, value);
            }
        }
        private List<string> type;
        public List<string> Type
        {
            get { return type; }
            set
            {
                SetProperty(ref type, value);
            }
        }

        //模版性别
        private int selectedSex;
        public int SelectedSex
        {
            get { return selectedSex; }
            set
            {
                SetProperty(ref selectedSex, value);
            }
        }
        private List<string> sex;
        public List<string> Sex
        {
            get { return sex; }
            set
            {
                SetProperty(ref sex, value);
            }
        }

        #endregion

        #region 初始化
        public TempleteInfoPopViewModel()
        {
            _FaceObj = new FaceObj();
            //初始化模版姓名
            Name = "";
            //初始化标题
            Title = "";
            //初始化ID
            Id = "";
            //初始化年龄
            Age = "";
            //初始化导入时间
            ImportTime = "";
            //初始化备注
            Remark = "";
            //初始化模版类型
            Type = thirft.QueryDefFaceObjType();
            SelectedType = 0;

            //初始化模版性别
            SelectedSex = 0;
            Sex = new List<string> { "未知", "男", "女" };
        }
        #endregion
    }
}
