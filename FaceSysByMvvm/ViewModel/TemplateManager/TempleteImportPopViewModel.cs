using FaceSysByMvvm.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceSysByMvvm.ViewModel.TemplateManager
{
    public class TempleteImportPopViewModel:BindableBase
    {
        ThirftService thirft = new ThirftService();
        #region 属性的定义


        //是否包含子文件夹      
        private int selectedAllDocu ;
        public int SelectedAllDocu
        {
            get { return selectedAllDocu; }
            set
            {
                SetProperty(ref selectedAllDocu, value);
            }
        }
        private List<string> allDocu ;
        public List<string> AllDocu
        {
            get { return allDocu; }
            set
            {
                SetProperty(ref allDocu, value);
            }
        }

        //模版类型      
        private int selectedType ;
        public int SelectedType
        {
            get { return selectedType; }
            set
            {
                SetProperty(ref selectedType, value);
            }
        }
        private List<string> type ;
        public List<string> Type
        {
            get { return type; }
            set
            {
                SetProperty(ref type, value);
            }
        }

        //模版性别
        private int selectedSex ;
        public int SelectedSex
        {
            get { return selectedSex; }
            set
            {
                SetProperty(ref selectedSex, value);
            }
        }
        private List<string> sex ;
        public List<string> Sex
        {
            get { return sex; }
            set
            {
                SetProperty(ref sex, value);
            }
        }

        //进度条当前进度
        private int currentLength ;
        public int CurrentLength
        {
            get { return currentLength; }
            set
            {
                SetProperty(ref currentLength, value);
            }
        }

        //进度条最大刻度
        private int maxLength ;
        public int MaxLength
        {
            get { return maxLength; }
            set
            {
                SetProperty(ref maxLength, value);
            }
        }

        //错误信息
        private string errorInfo ;
        public string ErrorInfo
        {
            get { return errorInfo; }
            set
            {
                SetProperty(ref errorInfo, value);
            }
        }
        //成功数量
        private int successCount ;
        public int SuccessCount
        {
            get { return successCount; }
            set
            {
                SetProperty(ref successCount, value);
            }
        }


        //错误数量
        private int errorCount ;
        public int ErrorCount
        {
            get { return errorCount; }
            set
            {
                SetProperty(ref errorCount, value);
            }
        }

        //错误照片存放地址
        private string errorAddress ;
        public string ErrorAddress
        {
            get { return errorAddress; }
            set
            {
                SetProperty(ref errorAddress, value);
            }
        }

        //导入进度描述
        private string importInfo ;
        public string ImportInfo
        {
            get { return importInfo; }
            set
            {
                SetProperty(ref importInfo, value);
            }
        }

        #endregion

        #region 属性初始化
        public TempleteImportPopViewModel()
        {
            //初始化是否包含子文件夹
            AllDocu = new List<string> { "是", "否" };
            SelectedAllDocu = 0;

            //初始化模版类型
            Type = thirft.QueryDefFaceObjType();
            SelectedType = 0;

            //初始化模版性别
            SelectedSex = 0;
            Sex = new List<string> { "全部", "男", "女" };

            //初始化进度条当前进度
            CurrentLength = 0;
            //初始化进度条最大进度
            MaxLength = 100;
            //初始化错误照片数量
            errorCount = 0;
            //初始化错误信息
            errorInfo = "";
            //初始化错误照片保存地址
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            const string strDirDest = "\\ImageError";
            errorAddress = dir + strDirDest;
            //初始化导入进度描述
            ImportInfo = "点击导入模板文件按钮，导入模板";
        }
        #endregion
    }
}
