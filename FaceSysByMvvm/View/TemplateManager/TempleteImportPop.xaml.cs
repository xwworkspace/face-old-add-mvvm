using FaceSysByMvvm.Common;
using FaceSysByMvvm.Services;
using FaceSysByMvvm.ViewModel.TemplateManager;
using FaceSysClient.ClassPool;
using FuncationTest;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FaceSysByMvvm.View.TemplateManager
{
    /// <summary>
    /// TempleteImportPop.xaml 的交互逻辑
    /// </summary>
    public partial class TempleteImportPop : Window
    {
        TempleteImportPopViewModel tIPViewModel;
        public System.IO.FileInfo[] filesPic;
        WriteLog _WriteLog = new WriteLog();
        ThirftService thirft = new ThirftService();
        OperaExcel m_OperaExcel = new OperaExcel();
        bool bIsContainsSubFold = true;
        string strSex = "";
        string strType = "";

        public delegate List<ErrorInfo> ThreadImportPicIntoDbDelegate(FaceObj _FaceObj);
        ThreadImportPicIntoDbDelegate AddFaceObjDelegate;
        int upingCount = 0;//当前的正在上传模板数量
        int upedCount = 0;
        int maxUpCount = 50;

        List<string> templeteCol = new List<string>();
        List<int> ExcelColNo = new List<int>();

        Thread ThreadStarServer;
        public TempleteImportPop()
        {
            InitializeComponent();
            tIPViewModel = new TempleteImportPopViewModel();
            this.DataContext = tIPViewModel;
            AddFaceObjDelegate = new ThreadImportPicIntoDbDelegate(delegate (FaceObj _FaceObj) {   
                return thirft.AddFaceObj(_FaceObj); ;
            });
            this.MouseLeftButtonDown += TempleteImportPop_MouseLeftButtonDown;
            InitTempleteCol();
        }

        /// <summary>
        /// 初始化导入模版excel相关初始化
        /// </summary>
        private void InitTempleteCol()
        {
            templeteCol.Add("人脸uuid");
            templeteCol.Add("姓名");
            templeteCol.Add("人脸首选模板标识序号");
            templeteCol.Add("类型");
            templeteCol.Add("敏感等级");
            templeteCol.Add("额外信息");
            templeteCol.Add("性别");
            templeteCol.Add("年龄");
            templeteCol.Add("备注");
            templeteCol.Add("模板1");
            templeteCol.Add("模板2");
            templeteCol.Add("模板3");
            templeteCol.Add("模板4");
            templeteCol.Add("模板5");
            foreach (var item in templeteCol)
            {
                int no = 0;
                try
                {
                    no = Convert.ToInt32(ConfigurationManager.AppSettings[item]);
                }
                catch (Exception)
                {

                }
                finally
                {
                    ExcelColNo.Add(no);
                } 
            }
        }

        private void TempleteImportPop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// 批量导入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportTemplateFile_Click(object sender, RoutedEventArgs e)
        {
            bIsContainsSubFold = tIPViewModel.SelectedAllDocu == 0 ? true : false;
            strSex = tIPViewModel.Sex[tIPViewModel.SelectedSex];
            strType = tIPViewModel.Type[tIPViewModel.SelectedType];
            string StrPath = "";
            System.Windows.Forms.FolderBrowserDialog _FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (_FolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StrPath = _FolderBrowserDialog.SelectedPath;
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                if (dir.Equals(StrPath))
                {
                    MyMessage.showYes("未选择");
                    return;
                }
            }
            else
            {
                return;
            }

            filesPic = m_OperaExcel.GetFilesPic(StrPath, bIsContainsSubFold);
            int maxCount = 0;
            m_OperaExcel.nNumReadPic(filesPic, ref maxCount);
            tIPViewModel.MaxLength = maxCount;
            tIPViewModel.CurrentLength = 0;
            tIPViewModel.SuccessCount = 0;
            tIPViewModel.ErrorCount = 0;

            ThreadStarServer = new System.Threading.Thread(new ParameterizedThreadStart(StartImport));
            ThreadStarServer.SetApartmentState(ApartmentState.STA);
            ThreadStarServer.Start();

            //filesPic = m_OperaExcel.GetFilesPic(StrPath, bIsContainsSubFold);
            ////int maxCount = 0;
            //m_OperaExcel.nNumReadPic(filesPic, ref maxCount);
            //tIPViewModel.MaxLength = maxCount;

            //导入excel 测试部分
            //tIPViewModel.CurrentLength = 0;
            //tIPViewModel.SuccessCount = 0;
            //tIPViewModel.ErrorCount = 0;

            //ThreadStarServer = new System.Threading.Thread(new ParameterizedThreadStart(StartExcelImport));
            //ThreadStarServer.SetApartmentState(ApartmentState.STA);
            //ThreadStarServer.Start();
        }

        /// <summary>
        /// 开始导入
        /// </summary>
        /// <param name="obj"></param>
        private void StartImport(object obj)
        {
            for (int i = 0; i < filesPic.Length; i++)
            {
                try
                {
                    ThreadImportPicIntoDb(i, 0);
                }
                catch (Exception ex)
                {
                    //MyMessage.showYes(ex.Message);
                    continue;
                }
            }
        }

        /// <summary>
        /// 读取图片生成人脸对象
        /// </summary>
        /// <param name="i"></param>
        /// <param name="type">导入类型:0图片;1excel</param>
        public void ThreadImportPicIntoDb(int i,int  type)
        {
            try
            {
                if (type == 0)
                {
                    string strPathName = "";
                    strPathName = (filesPic[i].DirectoryName + "\\" + filesPic[i].Name);

                    if (File.Exists(strPathName))
                    {
                        #region
                        FaceObj _FaceObj = new FaceObj();//实例OrderInf，添加到新的窗口的list中
                                                         //人脸对象添加时间
                        DateTime dt0 = Convert.ToDateTime(DateTime.Now);
                        TimeSpan dt0Span = new TimeSpan(dt0.Ticks);

                        DateTime dt01 = new DateTime(1970, 1, 1);
                        TimeSpan dt01Span = new TimeSpan(dt01.Ticks);

                        long longDPTemplateStarTime0 = Convert.ToInt64(dt0Span.TotalSeconds - dt01Span.TotalSeconds);
                        _FaceObj.DTm = longDPTemplateStarTime0;
                        //生成人脸对象uuid
                        //生成uuid
                        string StringName = System.Guid.NewGuid().ToString();
                        StringName = StringName.Replace("-", "");
                        _FaceObj.TcUuid = StringName;
                        //人脸名称 
                        _FaceObj.TcName = filesPic[i].Name.Substring(0, filesPic[i].Name.Length - 4);

                        _FaceObj.Tmplate = new List<FaceObjTemplate>();
                        //for (int k = 0; k < 5; k++)
                        //{
                        //    FaceObjTemplate _FaceObjTemplate = new FaceObjTemplate();
                        //    _FaceObj.Tmplate.Add(_FaceObjTemplate);
                        //}

                        FaceObjTemplate _FaceObjTemplate = new FaceObjTemplate();
                        _FaceObj.Tmplate.Add(_FaceObjTemplate);

                        _FaceObj.NMain_ftID = 0;//设置主模板ID
                        string strPath2 = strPathName;//获得文件地址
                        if (strPath2 != "")
                        {
                            _FaceObj.Tmplate[0].Img = System.IO.File.ReadAllBytes(strPath2);
                        }
                        //判断统一指定类型
                        if ((strSex == "全部" && strType == "请选择"))
                        {
                            _FaceObj.NSex = 0;
                            _FaceObj.NType = 1;
                        }
                        else
                        {
                            if (strSex != "" && strType != "")
                            {
                                _FaceObj.NSex = tIPViewModel.SelectedSex;
                                foreach (var basicinfo in BasicInfo.DefFaceObjType)
                                {
                                    if (basicinfo.Description == tIPViewModel.Type[tIPViewModel.SelectedType])
                                    {
                                        _FaceObj.NType = basicinfo.Type == 0 ? -1 : basicinfo.Type;
                                    }
                                }
                            }
                            else
                            {
                                if (strSex != "")//如果指定了性别
                                {
                                    _FaceObj.NSex = tIPViewModel.SelectedSex;
                                }
                                if (strType != "")//如果指定了类型
                                {
                                    foreach (var basicinfo in BasicInfo.DefFaceObjType)
                                    {
                                        if (basicinfo.Description == tIPViewModel.Type[tIPViewModel.SelectedType])
                                        {
                                            _FaceObj.NType = basicinfo.Type == 0 ? -1 : basicinfo.Type;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                      //补足模板中不足的字段
                        for (int k = 0; k < _FaceObj.Tmplate.Count; k++)
                        {
                            if (_FaceObj.Tmplate[k].Img != null)//判断模板照片存在
                            {
                                //生成uuid
                                StringName = System.Guid.NewGuid().ToString();
                                StringName = StringName.Replace("-", "");
                                _FaceObj.Tmplate[k].TcUuid = StringName;
                                //所属FaceObj的uuid
                                _FaceObj.Tmplate[k].TcObjid = _FaceObj.TcUuid;
                                //模板序号
                                _FaceObj.Tmplate[k].NIndex = k;
                                // 模板添加时间
                                DateTime dt1 = Convert.ToDateTime(DateTime.Now);
                                TimeSpan dt1Span = new TimeSpan(dt1.Ticks);

                                DateTime dt2 = new DateTime(1970, 1, 1);
                                TimeSpan dt2Span = new TimeSpan(dt2.Ticks);

                                long longDPTemplateStarTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                                _FaceObj.Tmplate[k].DTm = longDPTemplateStarTime;
                            }
                        }

                        while (true)
                        {
                            if (upingCount <= maxUpCount)
                            {
                                //Console.WriteLine("第" + i + "次运行");
                                AddFaceObjDelegate.BeginInvoke(_FaceObj, new AsyncCallback(PicHandle), _FaceObj);
                                upingCount++;
                                break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("ThreadImportPicIntoDb", ex);
                return;
            }
        }

        /// <summary>
        /// 将人脸对象批量上传的回调函数(错误的保存)
        /// </summary>
        /// <param name="ar"></param>
        private void PicHandle(IAsyncResult ar)
        {
            lock(tIPViewModel)
            {
                try
                {
                    upedCount++;
                    FaceObj _FaceObj = ar.AsyncState as FaceObj;
                    AsyncResult a = (AsyncResult)ar;
                    ThreadImportPicIntoDbDelegate trys = (ThreadImportPicIntoDbDelegate)a.AsyncDelegate;
                    List<ErrorInfo> ListErrorInfo = trys.EndInvoke(ar);
                    if (ListErrorInfo.Count > 0)
                    {
                        for (int l = 0; l < ListErrorInfo.Count; l++)
                        {
                            Console.WriteLine(ListErrorInfo[l].ErrCode + "");
                            if (ListErrorInfo[l].ErrCode == -1)//小于0，注册失败
                            {
                                
                                byte[] photo = _FaceObj.Tmplate[0].Img;
                                MemoryStream stream = new MemoryStream(photo);
                                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                                //判断文件夹存在 
                                DirectoryInfo directory = new DirectoryInfo(tIPViewModel.ErrorAddress);
                                if (!directory.Exists)
                                {
                                    //重新生成文件夹
                                    Directory.CreateDirectory(tIPViewModel.ErrorAddress);
                                }

                                string strPath = "";

                                strPath = tIPViewModel.ErrorAddress + @"\" + _FaceObj.TcName + @".jpg";
                                img.Save(strPath, img.RawFormat);
                                tIPViewModel.ErrorCount++;
                            }
                            if (ListErrorInfo[l].ErrCode == -3 || ListErrorInfo[l].ErrCode == -2)//图片不存在
                            {
                                tIPViewModel.ErrorCount++;
                                string message = "第" + upedCount + "行注册失败";
                            }
                        }
                    }
                    else
                    {
                        ++tIPViewModel.SuccessCount;
                    }
                    
                }
                catch (Exception ex)
                {
                    _WriteLog.WriteToLog("PicHandle", ex);
                    tIPViewModel.ErrorCount++;
                    //System.Windows.MessageBox.Show("PicHandle" + ex.Message);
                }
                finally
                {
                    ++tIPViewModel.CurrentLength;
                    tIPViewModel.ImportInfo = "总数量:" + tIPViewModel.MaxLength + "  上传数量:" + tIPViewModel.CurrentLength + "成功数量:" + tIPViewModel.SuccessCount + "";
                    //Console.WriteLine("总数量:" + tIPViewModel.MaxLength + "  上传数量:" + tIPViewModel.CurrentLength + "成功数量:" + tIPViewModel.SuccessCount + "");
                    upingCount--;
                    if (tIPViewModel.MaxLength == tIPViewModel.CurrentLength)
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            MyMessage.showYes("模版已全部导入完成！");
                        }));

                    }
                }
            }        
        }

        /// <summary>
        /// 关闭当前界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if(ThreadStarServer!=null)
            {
                ThreadStarServer.Abort();
            }
            this.Close();
        }

        /// <summary>
        /// 选择错误图片保存地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnsaveWrongPhotos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region
                System.Windows.Forms.FolderBrowserDialog _FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                if (_FolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    const string strDirDest = "\\ImageError";
                    string strPath = _FolderBrowserDialog.SelectedPath + strDirDest;
                    if (!String.IsNullOrEmpty(strPath))
                    {
                        if (!Directory.Exists(strPath))
                            Directory.CreateDirectory(strPath);
                        tIPViewModel.ErrorAddress = strPath;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("btnsaveWrongPhotos_Click", ex);
                //System.Windows.MessageBox.Show(ex.Message); 
            }
        }

        /// <summary>
        /// 打开路径文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnErrorPhotoAddress_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                #region
                string strPath = btnErrorPhotoAddress.Content.ToString();
                System.Diagnostics.Process.Start("explorer.exe ", tIPViewModel.ErrorAddress);
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("btnErrorPhotoAddress_Click_1", ex);
                //System.Windows.MessageBox.Show(ex.Message); 
            }
        }

        /// <summary>
        /// 导入帮助
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            if(gridImportHelp.Visibility == Visibility.Visible)
            {
                gridImportHelp.Visibility = Visibility.Collapsed;
            }
            else
            {
                gridImportHelp.Visibility = Visibility.Visible;
            }           
        }

        private void StartExcelImport(object filePathObj)
        {
            string filePath = @"C:\Users\Tsu\Desktop\2个插件\测试.xls";
            IWorkbook workbook = null;
            ISheet sheet = null;
            try
            {
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                if (filePath.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (filePath.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);
                if (sheet == null) //取第一个sheet
                {
                    sheet = workbook.GetSheetAt(0);
                }
                //excel总行数
                int rowCount = sheet.LastRowNum;
                tIPViewModel.MaxLength = rowCount;
                var pictureList = sheet.GetAllPictureInfos();
                for (int i = 2; i < rowCount; i++)
                {
                    
                    IRow curRow = sheet.GetRow(i);
                    if(curRow.Cells.Count == 0)
                    {
                        Console.WriteLine("NO."+i+"行为空行!");
                        continue;
                    }
                    FaceObj _FaceObj = new FaceObj();//实例OrderInf，添加到新的窗口的list中
                    _FaceObj.Tmplate = new List<FaceObjTemplate>();
                    //人脸对象添加时间
                    DateTime dt0 = Convert.ToDateTime(DateTime.Now);
                    TimeSpan dt0Span = new TimeSpan(dt0.Ticks);

                    DateTime dt01 = new DateTime(1970, 1, 1);
                    TimeSpan dt01Span = new TimeSpan(dt01.Ticks);

                    long longDPTemplateStarTime0 = Convert.ToInt64(dt0Span.TotalSeconds - dt01Span.TotalSeconds);
                    _FaceObj.DTm = longDPTemplateStarTime0;
                    #region 读取excel 生成人脸对象
                    foreach (var item in ExcelColNo)
                    {
                        switch (ExcelColNo.IndexOf(item))
                        {
                            //人脸uuid
                            case 0:
                                string StringName = System.Guid.NewGuid().ToString();
                                StringName = StringName.Replace("-", "");
                                _FaceObj.TcUuid = StringName;
                                break;
                            //姓名
                            case 1:
                                if (item != 0)
                                {
                                    _FaceObj.TcName = curRow.GetCell(item).ToString();
                                    Console.WriteLine(i + _FaceObj.TcName);
                                }
                                else
                                {
                                    _FaceObj.TcName = _FaceObj.TcUuid;
                                }
                                break;
                            //人脸首选模板标识序号
                            case 2:
                                _FaceObj.NMain_ftID = 0;
                                break;
                            //类型
                            case 3:
                                if (item != 0)
                                {
                                    _FaceObj.NType = 0;
                                }
                                break;
                            //敏感等级
                            case 4:
                                if (item != 0)
                                {
                                    _FaceObj.NSST = Convert.ToInt32(curRow.GetCell(item).ToString());
                                }
                                break;
                            //额外信息
                            case 5:
                                if (item != 0)
                                {
                                    _FaceObj.NExten = Convert.ToInt32(curRow.GetCell(item).ToString());
                                }
                                break;
                            //性别
                            case 6:
                                if (item != 0)
                                {
                                    _FaceObj.NSex = 0;
                                }
                                break;
                            //年龄
                            case 7:
                                if (item != 0)
                                {
                                    _FaceObj.NAge = Convert.ToInt32(curRow.GetCell(item).ToString());
                                }
                                break;
                            //备注
                            case 8:
                                if (item != 0)
                                {
                                    _FaceObj.TcRemarks = curRow.GetCell(item).ToString();
                                }
                                break;
                            //模板1
                            case 9:
                                if (item != 0)
                                {
                                    FaceObjTemplate _FaceObjTemplate = new FaceObjTemplate();
                                    var picture = GetPictureByRowCol(pictureList, i, item);
                                    if (picture != null)
                                    {
                                        _FaceObj.Tmplate.Add(_FaceObjTemplate);
                                        _FaceObj.Tmplate.Last().Img = picture.PictureData;                                      
                                    }
                                }
                                break;
                            //模板2
                            case 10:
                                if (item != 0)
                                {
                                    FaceObjTemplate _FaceObjTemplate = new FaceObjTemplate();
                                    var picture = GetPictureByRowCol(pictureList, i, item);
                                    if (picture != null)
                                    {
                                        _FaceObj.Tmplate.Add(_FaceObjTemplate);
                                        _FaceObj.Tmplate.Last().Img = picture.PictureData;
                                    }
                                }
                                break;
                            //模板3
                            case 11:
                                if (item != 0)
                                {
                                    FaceObjTemplate _FaceObjTemplate = new FaceObjTemplate();
                                    var picture = GetPictureByRowCol(pictureList, i, item);
                                    if (picture != null)
                                    {
                                        _FaceObj.Tmplate.Add(_FaceObjTemplate);
                                        _FaceObj.Tmplate.Last().Img = picture.PictureData;
                                    }
                                }
                                break;
                            //模板4
                            case 12:
                                if (item != 0)
                                {
                                    FaceObjTemplate _FaceObjTemplate = new FaceObjTemplate();
                                    var picture = GetPictureByRowCol(pictureList, i, item);
                                    if (picture != null)
                                    {
                                        _FaceObj.Tmplate.Add(_FaceObjTemplate);
                                        _FaceObj.Tmplate.Last().Img = picture.PictureData;
                                    }
                                }
                                break;
                            //模板5
                            case 13:
                                if (item != 0)
                                {
                                    FaceObjTemplate _FaceObjTemplate = new FaceObjTemplate();
                                    var picture = GetPictureByRowCol(pictureList, i, item);
                                    if (picture != null)
                                    {
                                        _FaceObj.Tmplate.Add(_FaceObjTemplate);
                                        _FaceObj.Tmplate.Last().Img = picture.PictureData;
                                    }
                                }
                                break;
                        }
                    }
                    #endregion
                    //异步上传人脸对象
                    while (true)
                    {
                        if (upingCount <= maxUpCount)
                        {
                            //Console.WriteLine("第" + i + "次运行");
                            AddFaceObjDelegate.BeginInvoke(_FaceObj, new AsyncCallback(PicHandle), _FaceObj);
                            upingCount++;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public  PicturesInfo GetPictureByRowCol(List<PicturesInfo> picturesInfo, int row, int col)
        {
            foreach (var picture in picturesInfo)
            {
                if (picture.Row == row && picture.Col == col)
                {
                    return picture;
                }
            }
            return null;
        }
    }
}
