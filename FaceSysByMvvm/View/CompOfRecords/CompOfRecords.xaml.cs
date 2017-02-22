using FaceSysByMvvm.Common;
using FaceSysByMvvm.Services;
using FaceSysByMvvm.ViewModel.CompOfRecords;
using FaceSysClient.ClassPool;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FaceSysByMvvm.View.CompOfRecords
{
    /// <summary>
    /// CompOfRecords.xaml 的交互逻辑
    /// </summary>
    public partial class CompOfRecords : UserControl
    {
        public CompOfRecordsViewModel cORViewModel;
        ThirftService thirft = new ThirftService();
        WriteLog _WriteLog = new WriteLog();
        string currDay = "";
        string exportCurrDay = "";
        //查询委托
        delegate void GetCompOfRecordsDelegate(int page);
        GetCompOfRecordsDelegate getCompOfRecordsDelegate;

        BitmapImage cmpTemp = new BitmapImage();
        BitmapImage capTemp = new BitmapImage();
        public CompOfRecords()
        {
            InitializeComponent();
            cORViewModel = new CompOfRecordsViewModel();
            this.DataContext = cORViewModel;
            getCompOfRecordsDelegate = GetAllInfo;
            //combTemplateSex.ItemsSource = cORViewModel.Sex;
            //combCompRecordStarTimeHour.ItemsSource = cORViewModel.StartHour;
            //combCompRecordEndTimeHour.ItemsSource = cORViewModel.EndHour;
        }

        private void btnCompOfRecordsQuery_Click(object sender, RoutedEventArgs e)
        {
            cORViewModel.compOfRecordsValue.ChannelValue = cORViewModel.SelectedChannel == 0 ? "" : cORViewModel.ChannelId[cORViewModel.SelectedChannel - 1];
            cORViewModel.compOfRecordsValue.NameValue = cORViewModel.Name;
            if (cORViewModel.SelectedType == -1 || cORViewModel.SelectedType == 0)
            {
                cORViewModel.compOfRecordsValue.TypeValue = -1;
            }
            else
            {
                foreach (var basicinfo in BasicInfo.DefFaceObjType)
                {
                    if (basicinfo.Description == cORViewModel.Type[cORViewModel.SelectedType])
                    {
                        cORViewModel.compOfRecordsValue.TypeValue = basicinfo.Type == 0 ? -1 : basicinfo.Type;
                    }
                }
            }
            cORViewModel.compOfRecordsValue.SexValue = cORViewModel.SelectedSex == 0 ? -1 : cORViewModel.SelectedSex;
            cORViewModel.compOfRecordsValue.PageRowValue = Convert.ToInt32(cORViewModel.PageRow[cORViewModel.SelectedPageRow]);
            cORViewModel.compOfRecordsValue.LittleAgeValue = cORViewModel.LittleAge == "" ? -1 : Convert.ToInt32(cORViewModel.LittleAge); ;
            cORViewModel.compOfRecordsValue.OldAgeValue = cORViewModel.OldAge == "" ? -1 : Convert.ToInt32(cORViewModel.OldAge);
            IntiQueryTime();
            if (cORViewModel.compOfRecordsValue.StartDayValue == -1 || (cORViewModel.compOfRecordsValue.EndDayValue - cORViewModel.compOfRecordsValue.StartDayValue > 60 * 60 * 24 * 7))
            {
                MyMessage.showYes("起始时间不能为空且起始和终止时间相差不能超过七天!");
                return;
            }

            getCompOfRecordsDelegate.BeginInvoke(1, null, null);
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="page"></param>
        private void GetAllInfo(int page)
        {
            try
            {
                List<int> pageSplit = new List<int>();
                int curpage = 0;
                int index = 0;
                cORViewModel.LoadingVisiblity = "Visible";
                List<MyCapFaceLogWithImg> listMyCapFaceLogWithImg = new List<MyCapFaceLogWithImg>();
                List<SCountInfo> queryCount = new List<SCountInfo>();
                if (Login.ClientType != "1")
                {
                    queryCount = thirft.QueryCmpRecordTotalCountH(cORViewModel.compOfRecordsValue);
                }
                else
                {
                    queryCount = thirft.QueryCmpRecordTotalCountHSX(cORViewModel.compOfRecordsValue,Login.ClientArea);
                }
                if (queryCount.Count <= 0)
                {
                    return;
                }
                cORViewModel.MaxCount = queryCount[0].Count;
                for (int no = queryCount[0].Dayarr.Count - 1; no >= 0; no--)
                {
                    var dayary = queryCount[0].Dayarr[no];
                    curpage += dayary.Count % cORViewModel.compOfRecordsValue.PageRowValue != 0 ? dayary.Count / cORViewModel.compOfRecordsValue.PageRowValue + 1 : dayary.Count / cORViewModel.compOfRecordsValue.PageRowValue;
                    pageSplit.Add(curpage);
                }
                cORViewModel.MaxPage = curpage;
                //根据页数判断是属于哪一天
                foreach (var dayPage in pageSplit)
                {
                    if (page <= dayPage)
                    {
                        index = pageSplit.IndexOf(dayPage);
                        //修改当前的时间 和 最大的结果数量
                        currDay = queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Daystr;
                        DateTime dt1 = Convert.ToDateTime(queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Daystr.Insert(6, "/").Insert(4, "/"));
                        TimeSpan dt1Span = new TimeSpan(dt1.Ticks);
                        DateTime dt2 = new DateTime(1970, 1, 1);
                        TimeSpan dt2Span = new TimeSpan(dt2.Ticks);
                        long longdtPkCompRecordStarTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                        if (longdtPkCompRecordStarTime > cORViewModel.compOfRecordsValue.StartDayValue)
                        {
                            cORViewModel.compOfRecordsValue.StartDayValue = longdtPkCompRecordStarTime;
                        }
                        if (page != cORViewModel.MaxPage)
                        {
                            long longdtPkCompRecordEndTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds) + 24 * 60 * 60;
                            var todayEndtime = Convert.ToInt64(new TimeSpan(dt1.AddDays(1).Ticks).TotalSeconds - dt2Span.TotalSeconds);
                            if (longdtPkCompRecordEndTime > todayEndtime)
                            {
                                longdtPkCompRecordEndTime = todayEndtime;
                            }
                            cORViewModel.compOfRecordsValue.EndDayValue = longdtPkCompRecordEndTime;
                        }
                        cORViewModel.compOfRecordsValue.MaxCount = queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Count;
                        break;
                    }
                }
                if (page > 1)
                {
                    int pageTem = 0;
                    if (index == 0)
                    {
                        pageTem = 0;
                    }
                    else
                    {
                        pageTem = pageSplit[index - 1];
                    }
                    cORViewModel.compOfRecordsValue.StartRowValue = cORViewModel.compOfRecordsValue.MaxCount - ((page - pageTem) * cORViewModel.compOfRecordsValue.PageRowValue);
                    if (cORViewModel.compOfRecordsValue.StartRowValue < 0)
                    {
                        cORViewModel.compOfRecordsValue.StartRowValue = 0;
                    }
                }
                else
                {
                    cORViewModel.compOfRecordsValue.StartRowValue = cORViewModel.compOfRecordsValue.MaxCount - (cORViewModel.compOfRecordsValue.PageRowValue);
                    if (cORViewModel.compOfRecordsValue.StartRowValue < 0)
                    {
                        cORViewModel.compOfRecordsValue.StartRowValue = 0;
                    }
                }

                cORViewModel.CurrPage = page;
                int countTem = 0;
                for (int n = 0; n <= index; n++)
                {
                    var dayary = queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - n].Count;
                    countTem += dayary;
                }
                cORViewModel.compOfRecordsValue.MaxCount = countTem;
                if (Login.ClientType != "1")
                {
                    cORViewModel.ListMyCmpFaceLogWidthImg = thirft.QueryCmpLog(cORViewModel.compOfRecordsValue);
                }
                else
                {
                    cORViewModel.ListMyCmpFaceLogWidthImg = thirft.QueryCmpLogSX(cORViewModel.compOfRecordsValue, Login.ClientArea);
                }
                cORViewModel.LoadingVisiblity = "Hidden";
                this.Dispatcher.BeginInvoke(
                            new Action(() =>
                            {
                                if (listComparisonRecord.Items.Count > 0)
                                {
                                    listComparisonRecord.ScrollIntoView(listComparisonRecord.Items[0]);
                                }
                            }));
            }
            catch (Exception ex)
            {
                MyMessage.showYes(ex.Message);
                _WriteLog.WriteToLog("GetAllInfo", ex);
            }
            finally
            {
                cORViewModel.LoadingVisiblity = "Hidden";
            }
        }

        /// <summary>
        /// 刷新通道下拉列表
        /// </summary>
        internal void RefreshChannelComboBox()
        {
            cORViewModel.RefreshChannelList();
        }

        /// <summary>
        /// 换页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPageChange_Click(object sender, RoutedEventArgs e)
        {
            Button pageBtn = sender as Button;
            int page = 0;
            IntiQueryTime();
            switch (pageBtn.Name)
            {
                case "btnComparisonRecordFirstPage":
                    page = 1;
                    break;
                case "btnComparisonRecordPastPage":
                    page = cORViewModel.CurrPage - 1;
                    break;
                case "btnComparisonRecordNextPage":
                    page = cORViewModel.CurrPage + 1;
                    break;
                case "btnComparisonRecordLastPage":
                    page = cORViewModel.MaxPage;
                    break;
                case "btnComparisonRecordGoToPage":
                    try
                    {
                        page = Convert.ToInt32(txtComparisonRecordGoToPage.Text.ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        MyMessage.showYes("请输入正确的跳转页码!");
                        return;
                    }
                    break;
                default:
                    break;
            }
            if (page <= 0)
            {
                MyMessage.showYes("已经是首页!");
                return;
            }
            if (page > cORViewModel.MaxPage)
            {
                MyMessage.showYes("已经是尾页！");
                return;
            }
            getCompOfRecordsDelegate.BeginInvoke(page, null, null);
        }

        /// <summary>
        /// 控制gridView的列填充
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listComparisonRecord_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            try
            {
                #region
                //获得比对结果listview
                GridView gv = listComparisonRecord.View as GridView;
                if (gv != null)
                {
                    if (buttonSend.Visibility == Visibility.Visible)
                    {
                        if (Login.ClientType == "1" || Login.ClientType == "2")
                        {
                            gv.Columns.RemoveAt(gv.Columns.Count - 1);
                            buttonSend.Visibility = Visibility.Hidden;
                        }
                    }
                    
                    //循环设置每列宽度
                    foreach (GridViewColumn gvc in gv.Columns)
                    {
                        gvc.Width = (listComparisonRecord.ActualWidth - 4) / gv.Columns.Count;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                //_WriteLog.WriteToLog("listComparisonRecord_SizeChanged_1", ex);
                //MyMessage.showYes(ex.Message); 
            }
        }

        /// <summary>
        /// 抓拍记录选中项变动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listComparisonRecord_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            MyCmpFaceLogWidthImg selectedRow = lv.SelectedItem as MyCmpFaceLogWidthImg;
            if (selectedRow == null)
            {
                return;
            }
            Thread threadQueryTemplate = new Thread(new ParameterizedThreadStart(QueryAllCmpLog));
            threadQueryTemplate.SetApartmentState(ApartmentState.STA);
            threadQueryTemplate.Start(selectedRow);
            Thread threadQuery = new Thread(new ParameterizedThreadStart(threadlistComparisonRecord));
            threadQuery.SetApartmentState(ApartmentState.STA);
            threadQuery.Start(selectedRow);
        }

        /// <summary>
        /// 获取抓拍比对模板
        /// </summary>
        /// <param name="obj"></param>
        public void QueryAllCmpLog(object obj)
        {
            try
            {
                MyCmpFaceLogWidthImg _MyCmpFaceLogWidthImg = (MyCmpFaceLogWidthImg)obj;
                #region 得到模板照片
                try
                {
                    lock (this)
                    {
                        List<FaceObj> ListFaceObj = thirft.QueryFaceObj(_MyCmpFaceLogWidthImg.tcUuid);
                        if (ListFaceObj.Count > 0)
                        {
                            //查找主模板ID是否存在
                            if (ListFaceObj[0].NMain_ftID > 0)//存在
                            {
                                //如果存在，显示主模板照片的注册信息
                                for (int i = 0; i < ListFaceObj[0].Tmplate.Count; i++)//遍历查找主模板照片
                                {
                                    if (ListFaceObj[0].NMain_ftID - 1 == ListFaceObj[0].Tmplate[i].NIndex)
                                    {
                                        #region
                                        try
                                        {
                                            //读入MemoryStream对象
                                            byte[] imageBytes = ListFaceObj[0].Tmplate[0].Img;
                                            if (imageBytes.Length > 0)
                                            {
                                                GridCmpPic.Dispatcher.BeginInvoke(new Action(() =>
                                                {
                                                    GridCmpPic.Background = new ImageBrush
                                                    {

                                                        ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/比对照片纯背景.png"))
                                                    };
                                                    GridAfterbtnRegisterPhoto.Visibility = Visibility.Visible;
                                                }));
                                                btnRegisterPhoto.Dispatcher.BeginInvoke(new Action(() =>
                                                {
                                                    btnRegisterPhoto.Visibility = Visibility.Visible;
                                                    btnRegisterPhoto.Content = "";
                                                    //读入MemoryStream对象 
                                                    BitmapImage myBitmapImage = new BitmapImage();
                                                    myBitmapImage.BeginInit();
                                                    myBitmapImage.StreamSource = new System.IO.MemoryStream(imageBytes);
                                                    myBitmapImage.EndInit();
                                                    cmpTemp = myBitmapImage;
                                                    btnRegisterPhoto.Background = new ImageBrush { ImageSource = myBitmapImage };
                                                    myBitmapImage = null;
                                                }));
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            //MyMessage.showYes("未查询到结果");
                                            //MyMessage.showYes("未查询到结果");
                                        }
                                        listViewRegisterPhoto.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            // //注册信息
                                            listViewRegisterPhoto.Items.Clear();

                                            RegisterPhoto _RegisterPhoto1 = new RegisterPhoto();
                                            _RegisterPhoto1.key = "人脸uuidID";
                                            _RegisterPhoto1.value = ListFaceObj[0].TcUuid.ToString();
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto1);

                                            RegisterPhoto _RegisterPhoto2 = new RegisterPhoto();
                                            _RegisterPhoto2.key = "姓名";
                                            _RegisterPhoto2.value = ListFaceObj[0].TcName.ToString();
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto2);
                                            //todo(已完成 未测试)       记得修改类型和性别
                                            RegisterPhoto _RegisterPhoto3 = new RegisterPhoto();
                                            _RegisterPhoto3.key = "类型";
                                            _RegisterPhoto3.value = BasicInfo.GetTypeById(ListFaceObj[0].NType);
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto3);

                                            RegisterPhoto _RegisterPhoto4 = new RegisterPhoto();
                                            _RegisterPhoto4.key = "性别";
                                            _RegisterPhoto4.value = cORViewModel.Sex[ListFaceObj[0].NSex];
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto4);

                                            RegisterPhoto _RegisterPhoto5 = new RegisterPhoto();
                                            _RegisterPhoto5.key = "人脸对象添加时间";

                                            long _longtime = ListFaceObj[0].DTm;
                                            DateTime s = new DateTime(1970, 1, 1);
                                            s = s.AddSeconds(_longtime);
                                            _RegisterPhoto5.value = s.ToString();
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto5);

                                            RegisterPhoto _RegisterPhoto6 = new RegisterPhoto();
                                            _RegisterPhoto6.key = "模板识别的年龄";
                                            _RegisterPhoto6.value = ListFaceObj[0].NAge.ToString();
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto6);

                                            RegisterPhoto _RegisterPhoto7 = new RegisterPhoto();
                                            _RegisterPhoto7.key = "人脸备注";
                                            _RegisterPhoto7.value = ListFaceObj[0].TcRemarks.ToString();
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto7);

                                            RegisterPhoto _RegisterPhoto8 = new RegisterPhoto();
                                            _RegisterPhoto8.key = "模板uuid";
                                            _RegisterPhoto8.value = ListFaceObj[0].Tmplate[i].ToString();
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto8);

                                            RegisterPhoto _RegisterPhoto9 = new RegisterPhoto();
                                            _RegisterPhoto9.key = "所属FaceObj的uuid";
                                            _RegisterPhoto9.value = ListFaceObj[0].Tmplate[i].TcObjid.ToString();
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto9);

                                            RegisterPhoto _RegisterPhoto10 = new RegisterPhoto();
                                            _RegisterPhoto10.key = "模板标识键";
                                            _RegisterPhoto10.value = ListFaceObj[0].Tmplate[i].TcKey.ToString();
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto10);

                                            RegisterPhoto _RegisterPhoto11 = new RegisterPhoto();
                                            _RegisterPhoto11.key = "模板序号";
                                            _RegisterPhoto11.value = ListFaceObj[0].Tmplate[i].NIndex.ToString();
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto11);

                                            RegisterPhoto _RegisterPhoto12 = new RegisterPhoto();
                                            _RegisterPhoto12.key = "模板添加时间";
                                            _longtime = ListFaceObj[0].Tmplate[i].DTm;
                                            s = new DateTime(1970, 1, 1);
                                            s = s.AddSeconds(_longtime);
                                            _RegisterPhoto12.value = s.ToString();
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto12);

                                            RegisterPhoto _RegisterPhoto13 = new RegisterPhoto();
                                            _RegisterPhoto13.key = "模板备注";
                                            _RegisterPhoto13.value = ListFaceObj[0].Tmplate[i].TcRemarks.ToString();
                                            listViewRegisterPhoto.Items.Add(_RegisterPhoto13);

                                            // //RegisterPhoto _RegisterPhoto14 = new RegisterPhoto();
                                            // //_RegisterPhoto14.key = "人脸特征数据的文件存储路径";
                                            // //_RegisterPhoto14.value = _face_Comp_record.ft_fea.ToString();
                                            // //listViewRegisterPhoto.Items.Add(_RegisterPhoto14);
                                        }));
                                        #endregion
                                        break;//跳出循环
                                    }
                                }
                            }
                            else
                            {
                                //如果不存在，显示第一张模板照片的注册信息
                                #region
                                try
                                {
                                    //读入MemoryStream对象
                                    byte[] imageBytes = ListFaceObj[0].Tmplate[0].Img;
                                    if (imageBytes.Length > 0)
                                    {
                                        GridCmpPic.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            GridCmpPic.Background = new ImageBrush
                                            {
                                                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/比对照片纯背景.png"))
                                            };
                                            GridAfterbtnRegisterPhoto.Visibility = Visibility.Visible;
                                        }));
                                        btnRegisterPhoto.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            btnRegisterPhoto.Visibility = Visibility.Visible;
                                            //读入MemoryStream对象 
                                            BitmapImage myBitmapImage = new BitmapImage();
                                            myBitmapImage.BeginInit();
                                            myBitmapImage.StreamSource = new System.IO.MemoryStream(imageBytes);
                                            myBitmapImage.EndInit();
                                            cmpTemp = myBitmapImage;
                                            btnRegisterPhoto.Background = new ImageBrush { ImageSource = myBitmapImage };
                                            myBitmapImage = null;
                                        }));
                                    }
                                }
                                catch (Exception)
                                {
                                    //MyMessage.showYes("未查询到结果");
                                    //MyMessage.showYes("未查询到结果");
                                    btnRegisterPhoto.Background = null;
                                }

                                // //注册信息 
                                listViewRegisterPhoto.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    // //注册信息
                                    listViewRegisterPhoto.Items.Clear();

                                    RegisterPhoto _RegisterPhoto1 = new RegisterPhoto();
                                    _RegisterPhoto1.key = "人脸uuidID";
                                    _RegisterPhoto1.value = ListFaceObj[0].TcUuid.ToString();
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto1);

                                    RegisterPhoto _RegisterPhoto2 = new RegisterPhoto();
                                    _RegisterPhoto2.key = "姓名";
                                    _RegisterPhoto2.value = ListFaceObj[0].TcName.ToString();
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto2);
                                    //todo(已完成 未测试) 类型 性别
                                    RegisterPhoto _RegisterPhoto3 = new RegisterPhoto();
                                    _RegisterPhoto3.key = "类型";
                                    _RegisterPhoto3.value = BasicInfo.GetTypeById(ListFaceObj[0].NType);
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto3);

                                    RegisterPhoto _RegisterPhoto4 = new RegisterPhoto();
                                    _RegisterPhoto4.key = "性别";
                                    _RegisterPhoto4.value = cORViewModel.Sex[ListFaceObj[0].NSex];
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto4);

                                    RegisterPhoto _RegisterPhoto5 = new RegisterPhoto();
                                    _RegisterPhoto5.key = "人脸对象添加时间";
                                    long _longtime = ListFaceObj[0].DTm;
                                    DateTime s = new DateTime(1970, 1, 1);
                                    s = s.AddSeconds(_longtime);
                                    _RegisterPhoto5.value = s.ToString();
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto5);

                                    RegisterPhoto _RegisterPhoto6 = new RegisterPhoto();
                                    _RegisterPhoto6.key = "模板识别的年龄";
                                    _RegisterPhoto6.value = ListFaceObj[0].NAge.ToString();
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto6);

                                    RegisterPhoto _RegisterPhoto7 = new RegisterPhoto();
                                    _RegisterPhoto7.key = "人脸备注";
                                    _RegisterPhoto7.value = ListFaceObj[0].TcRemarks.ToString();
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto7);

                                    RegisterPhoto _RegisterPhoto8 = new RegisterPhoto();
                                    _RegisterPhoto8.key = "模板uuid";
                                    _RegisterPhoto8.value = ListFaceObj[0].Tmplate[0].TcUuid.ToString();
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto8);

                                    RegisterPhoto _RegisterPhoto9 = new RegisterPhoto();
                                    _RegisterPhoto9.key = "所属FaceObj的uuid";
                                    _RegisterPhoto9.value = ListFaceObj[0].Tmplate[0].TcObjid.ToString();
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto9);

                                    RegisterPhoto _RegisterPhoto10 = new RegisterPhoto();
                                    _RegisterPhoto10.key = "模板标识键";
                                    _RegisterPhoto10.value = ListFaceObj[0].Tmplate[0].TcKey.ToString();
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto10);

                                    RegisterPhoto _RegisterPhoto11 = new RegisterPhoto();
                                    _RegisterPhoto11.key = "模板序号";
                                    _RegisterPhoto11.value = ListFaceObj[0].Tmplate[0].NIndex.ToString();
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto11);

                                    RegisterPhoto _RegisterPhoto12 = new RegisterPhoto();
                                    _RegisterPhoto12.key = "模板添加时间";
                                    _longtime = ListFaceObj[0].Tmplate[0].DTm;
                                    s = new DateTime(1970, 1, 1);
                                    s = s.AddSeconds(_longtime);
                                    _RegisterPhoto12.value = s.ToString();
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto12);

                                    RegisterPhoto _RegisterPhoto13 = new RegisterPhoto();
                                    _RegisterPhoto13.key = "模板备注";
                                    _RegisterPhoto13.value = ListFaceObj[0].Tmplate[0].TcRemarks.ToString();
                                    listViewRegisterPhoto.Items.Add(_RegisterPhoto13);

                                    // //RegisterPhoto _RegisterPhoto14 = new RegisterPhoto();
                                    // //_RegisterPhoto14.key = "人脸特征数据的文件存储路径";
                                    // //_RegisterPhoto14.value = _face_Comp_record.ft_fea.ToString();
                                    // //listViewRegisterPhoto.Items.Add(_RegisterPhoto14);
                                }));
                                #endregion

                            }
                        }
                        else
                        {
                            listViewRegisterPhoto.Dispatcher.BeginInvoke(new Action(() => { cmpTemp = null; btnRegisterPhoto.Background = null; listViewRegisterPhoto.Items.Clear(); }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _WriteLog.WriteToLog("MainWindow", ex);
                    return;
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("threadlistComparisonRecordTemplate", ex);
                //MyMessage.showYes(ex.Message); 
            }
            Thread.CurrentThread.Abort();
        }

        /// <summary>
        /// 获取抓拍照片
        /// </summary>
        /// <param name="obj"></param>
        public void threadlistComparisonRecord(object obj)
        {
            try
            {
                MyCmpFaceLogWidthImg _MyCmpFaceLogWidthImg = (MyCmpFaceLogWidthImg)obj;
                try
                {
                    List<byte[]> listImageBytes = thirft.QueryCmpLogImageH(_MyCmpFaceLogWidthImg.ID, currDay);
                    lock (this)
                    {
                        try
                        {
                            btnCapturePhoto.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                this.btnCapturePhoto.Content = "";
                            }));
                            if (listImageBytes.Count > 0)
                            {
                                if (listImageBytes[0].Length > 0)
                                {
                                    GridCapPic.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        GridCapPic.Background = new ImageBrush
                                        {
                                            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/抓拍照片纯背景.png"))
                                        };
                                        GridAfterbtnCapturePhoto.Visibility = Visibility.Visible;
                                    }));
                                    //读入MemoryStream对象
                                    btnCapturePhoto.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        btnCapturePhoto.Visibility = Visibility.Visible;
                                        BitmapImage myBitmapImage = new BitmapImage();
                                        myBitmapImage.BeginInit();
                                        myBitmapImage.StreamSource = new System.IO.MemoryStream(listImageBytes[0]);
                                        myBitmapImage.EndInit();
                                        capTemp = myBitmapImage;
                                        this.btnCapturePhoto.Background = new ImageBrush { ImageSource = myBitmapImage };
                                        myBitmapImage = null;
                                    }));
                                }

                            }
                            //else
                            //{
                            //    //MyMessage.showYes("未查询到结果");
                            //    MyMessage.showYes("未查询到结果");
                            //}
                        }
                        catch (Exception ex)
                        {
                            _WriteLog.WriteToLog("listComparisonRecord_SelectionChangedInner", ex);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _WriteLog.WriteToLog("listComparisonRecord_SelectionChanged", ex);
                    return;
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("threadlistComparisonRecord", ex);
                //MyMessage.showYes(ex.Message); 
            }
            Thread.CurrentThread.Abort();
        }

        /// <summary>
        /// 注册信息listview列宽自动变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewRegisterPhoto_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            try
            {
                #region
                //获得注册信息listview
                GridView gv = listViewRegisterPhoto.View as GridView;
                if (gv != null)
                {
                    //设置第一列宽度
                    GridViewColumn gvc = gv.Columns[0];
                    gvc.Width = (listViewRegisterPhoto.ActualWidth - 4) / 4;
                    //设置第二列宽度
                    gvc = gv.Columns[1];
                    gvc.Width = (listViewRegisterPhoto.ActualWidth - 4) / 4 * 3;
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("listViewRegisterPhoto_SizeChanged_1", ex);
                //MyMessage.showYes(ex.Message); 
            }
        }

        /// <summary>
        /// 双击选中项 弹出当前帧窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listComparisonRecord_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MyCmpFaceLogWidthImg cmpFaceLogWidthImg = listComparisonRecord.SelectedItem as MyCmpFaceLogWidthImg;
                List<byte[]> senceImg = thirft.QuerySenceImg(cmpFaceLogWidthImg.ID, cmpFaceLogWidthImg.time.Split(' ')[0].Replace(@"/", "").Replace(@"/", ""));
                if (senceImg != null && senceImg.Count > 0 && senceImg[0].Length > 0)
                {
                    CompInfo comInfoWin = new CompInfo();
                    BitmapImage bitImage = new BitmapImage();
                    bitImage.BeginInit();
                    bitImage.StreamSource = new System.IO.MemoryStream(senceImg[0]);
                    bitImage.EndInit();
                    comInfoWin.SetCmpInfo(capTemp, cmpTemp, bitImage, cmpFaceLogWidthImg.score, cmpFaceLogWidthImg.name, cmpFaceLogWidthImg.type, cmpFaceLogWidthImg.time, cmpFaceLogWidthImg.channelName);
                    comInfoWin.Show();
                }
                else
                {
                    MyMessage.showYes("获取抓拍实时帧失败!");
                }
            }
            catch (Exception ex)
            {
                MyMessage.showYes(ex.Message);
                _WriteLog.WriteToLog("listComparisonRecord_MouseDoubleClick", ex);
            }

        }

        /// <summary>
        /// 初始化查询时间
        /// </summary>
        private void IntiQueryTime()
        {
            long longdtPkCompRecordStarTime = -1;
            if (cORViewModel.StartDay != string.Empty)
            {
                string strCompRecordStarTime = cORViewModel.StartDay;
                DateTime dt1 = Convert.ToDateTime(strCompRecordStarTime);
                TimeSpan dt1Span = new TimeSpan(dt1.Ticks);

                DateTime dt2 = new DateTime(1970, 1, 1);
                TimeSpan dt2Span = new TimeSpan(dt2.Ticks);

                longdtPkCompRecordStarTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                if (cORViewModel.SelectedStartHour != -1)
                {
                    longdtPkCompRecordStarTime = longdtPkCompRecordStarTime + int.Parse(cORViewModel.SelectedStartHour.ToString()) * 60 * 60 + combCompRecordStarTimeMinutes.SelectedIndex * 60;
                }
            }
            cORViewModel.compOfRecordsValue.StartDayValue = longdtPkCompRecordStarTime;
            //结束时间
            long longdtPkCompRecordEndTime = -1;
            if (cORViewModel.EndDay != string.Empty)
            {
                string strCompRecordEndTime = cORViewModel.EndDay;
                DateTime dt1 = Convert.ToDateTime(strCompRecordEndTime);
                TimeSpan dt1Span = new TimeSpan(dt1.Ticks);

                DateTime dt2 = new DateTime(1970, 1, 1);
                TimeSpan dt2Span = new TimeSpan(dt2.Ticks);

                longdtPkCompRecordEndTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                if (cORViewModel.SelectedEndHour != -1)
                {
                    longdtPkCompRecordEndTime = longdtPkCompRecordEndTime + int.Parse(cORViewModel.SelectedEndHour.ToString()) * 60 * 60 + combCompRecordEndTimeMinutes.SelectedIndex * 60 + 3660;
                }
            }
            else
            {
                string strCompRecordEndTime = System.DateTime.Now.ToString();
                DateTime dt1 = Convert.ToDateTime(strCompRecordEndTime);
                TimeSpan dt1Span = new TimeSpan(dt1.Ticks);

                DateTime dt2 = new DateTime(1970, 1, 1);
                TimeSpan dt2Span = new TimeSpan(dt2.Ticks);
                longdtPkCompRecordEndTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
            }
            cORViewModel.compOfRecordsValue.EndDayValue = longdtPkCompRecordEndTime;
        }

        /// <summary>
        /// 清除时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearDatePickerTime(object sender, MouseButtonEventArgs e)
        {
            DatePicker dp = sender as DatePicker;
            dp.Text = "";
        }

        /// <summary>
        /// 推送结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cORViewModel.ListMyCmpFaceLogWidthImg.Count < 1)
                {
                    return;
                }
                foreach (var cmpface in cORViewModel.ListMyCmpFaceLogWidthImg)
                {
                    if (cmpface.IsChecked == true)
                    {
                        RealtimeCmpInfo info = new RealtimeCmpInfo();
                        info.CapID = cmpface.ID;
                        info.ObjID = cmpface.tcUuid;
                        info.Name = cmpface.name;
                        info.Channel = cmpface.channelName;
                        List<byte[]> listImageBytes = thirft.QueryCmpLogImageH(cmpface.ID, cmpface.time.Split(' ')[0].Replace(@"/", "").Replace(@"/", ""));
                        if (listImageBytes.Count > 0 && listImageBytes[0].Length > 0)
                        {
                            info.CapImg = listImageBytes[0];
                            Console.WriteLine("抓拍照片大小为:" + listImageBytes[0].Length);
                        }
                        List<FaceObj> ListFaceObj = thirft.QueryFaceObj(cmpface.tcUuid);
                        if (ListFaceObj.Count > 0)
                        {
                            info.Name += "|" + ListFaceObj[0].TcRemarks;
                            if (ListFaceObj[0].NMain_ftID > 0)
                            {
                                for (int i = 0; i < ListFaceObj[0].Tmplate.Count; i++)//遍历查找主模板照片
                                {
                                    if (ListFaceObj[0].NMain_ftID - 1 == ListFaceObj[0].Tmplate[i].NIndex)
                                    {
                                        info.ObjImg = ListFaceObj[0].Tmplate[i].Img;
                                        Console.WriteLine("模版照片大小为:" + ListFaceObj[0].Tmplate[i].Img.Length);
                                    }
                                }
                            }
                            else
                            {
                                info.ObjImg = ListFaceObj[0].Tmplate[0].Img;
                                Console.WriteLine("模版照片大小为:" + ListFaceObj[0].Tmplate[0].Img.Length);
                            }
                        }
                        DateTime dt1 = Convert.ToDateTime(cmpface.time);
                        TimeSpan dt1Span = new TimeSpan(dt1.Ticks);
                        DateTime dt2 = new DateTime(1970, 1, 1);
                        TimeSpan dt2Span = new TimeSpan(dt2.Ticks);
                        info.Time = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                        info.Type = cORViewModel.Type.IndexOf(cmpface.type) + 1;
                        info.Score = cmpface.score;
                        SendCmpToClient(info);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessage.showYes(ex.Message);
                _WriteLog.WriteToLog("SendFile_Click", ex);
            }
        }

        /// <summary>
        /// 推送比对记录
        /// </summary>
        /// <param name="info"></param>
        private void SendCmpToClient(RealtimeCmpInfo info)
        {
            try
            {
                string errMsg = "";
                foreach (var area in BasicInfo.ConfigList)
                {
                    if(info.Channel.Contains(area.AreaName))
                    {
                        foreach (var ip in area.ReceiveIPOfArea)
                        {
                            if (UpdateCmp(info, ip) != 0)
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
                //foreach (var key in ConfigurationManager.AppSettings.AllKeys)
                //{
                //    if (key.StartsWith("接受推送客户端"))
                //    {
                //        if (UpdateCmp(info, ConfigurationManager.AppSettings[key]) != 0)
                //        {
                //            errMsg += ConfigurationManager.AppSettings[key] + ";";
                //        }
                //    }
                //}
                if (!string.IsNullOrEmpty(errMsg))
                {
                    MyMessage.showYes("往" + errMsg + "推送失败!");
                }
            }
            catch (Exception ex)
            {
                MyMessage.showYes("推送比对信息出错");
                _WriteLog.WriteToLog("SendCmpToClient", ex);
            }
        }

        /// <summary>
        /// 更新比对记录标识
        /// </summary>
        /// <param name="info"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        public int UpdateCmp(RealtimeCmpInfo info, string IP)
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
                _BusinessServerClient.UpdateRealtimeCmp(info, "##@" + info.Channel);
                transport.Close();
                return 0;
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("UpdateCmp", ex);
                return -1;
            }

        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExportExcelFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string saveExcelPath = ConfigurationManager.AppSettings["Excel保存地址"];
                if (string.IsNullOrEmpty(saveExcelPath))
                {
                    saveExcelPath = System.Windows.Forms.Application.StartupPath + @"\比对记录导出";
                }
                else
                {
                    saveExcelPath += @"\比对记录导出";
                }
                await Task.Run(
                      () =>
                      {
                          XSSFWorkbook workbook = new XSSFWorkbook();
                          XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
                          style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                          style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                          style.BorderBottom = BorderStyle.Thin;
                          style.BorderLeft = BorderStyle.Thin;
                          style.BorderRight = BorderStyle.Thin;
                          style.BorderTop = BorderStyle.Thin;
                          XSSFCellStyle style2 = (XSSFCellStyle)workbook.CreateCellStyle();
                          style2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                          style2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                          style2.BorderBottom = BorderStyle.Thin;
                          style2.BorderLeft = BorderStyle.Thin;
                          style2.BorderRight = BorderStyle.Thin;
                          style2.BorderTop = BorderStyle.Thin;
                          XSSFFont font = (XSSFFont)workbook.CreateFont();
                          font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                          style2.SetFont(font);
                          XSSFCellStyle style0 = (XSSFCellStyle)workbook.CreateCellStyle();
                          style0.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                          style0.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                          style0.BorderBottom = BorderStyle.Thin;
                          style0.BorderLeft = BorderStyle.Thin;
                          style0.BorderRight = BorderStyle.Thin;
                          style0.BorderTop = BorderStyle.Thin;
                          IDataFormat dataformat = workbook.CreateDataFormat();
                          style0.DataFormat = dataformat.GetFormat("yyyy年MM月dd日 h:mm:ss 上午/下午");
                          XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet("比对记录");
                          int row = 0;
                          List<MyCmpFaceLogWidthImg> allCmpInfo = new List<MyCmpFaceLogWidthImg>();
                          allCmpInfo = GetCmpFaceLogByPage(1);
                          foreach (var item in allCmpInfo)
                          {
                              if (row == 0)
                              {
                                  sheet.CreateRow(row).CreateCell(0).SetCellValue("序号");
                                  sheet.GetRow(row).CreateCell(1).SetCellValue("抓拍通道");
                                  sheet.GetRow(row).CreateCell(2).SetCellValue("抓拍时间");
                                  sheet.GetRow(row).CreateCell(3).SetCellValue("模版姓名");
                                  sheet.GetRow(row).CreateCell(4).SetCellValue("注册类型");
                                  sheet.GetRow(row).CreateCell(5).SetCellValue("相似度");
                                  sheet.GetRow(row).CreateCell(6).SetCellValue("抓拍照片");
                                  sheet.GetRow(row).CreateCell(7).SetCellValue("模版照片");
                                  sheet.GetRow(row).GetCell(0).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(1).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(2).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(3).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(4).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(5).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(6).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(7).CellStyle = style2;
                                  sheet.SetColumnWidth(2, 30 * 256);
                                  sheet.SetColumnWidth(6, 30 * 256);
                                  sheet.SetColumnWidth(7, 30 * 256);
                                  row++;
                              }
                              sheet.CreateRow(row).CreateCell(0).SetCellValue(row);
                              sheet.GetRow(row).Height = 100 * 40;
                              sheet.GetRow(row).CreateCell(1).SetCellValue(item.channelName);
                              sheet.GetRow(row).CreateCell(2).SetCellValue(item.time);
                              sheet.GetRow(row).CreateCell(3).SetCellValue(item.name);
                              sheet.GetRow(row).CreateCell(4).SetCellValue(item.type);
                              sheet.GetRow(row).CreateCell(5).SetCellValue(item.score);
                              sheet.GetRow(row).CreateCell(6).SetCellValue("");
                              sheet.GetRow(row).CreateCell(7).SetCellValue("");
                              sheet.GetRow(row).GetCell(0).CellStyle = style;
                              sheet.GetRow(row).GetCell(1).CellStyle = style;
                              sheet.GetRow(row).GetCell(2).CellStyle = style0;
                              sheet.GetRow(row).GetCell(3).CellStyle = style;
                              sheet.GetRow(row).GetCell(4).CellStyle = style;
                              sheet.GetRow(row).GetCell(5).CellStyle = style;
                              sheet.GetRow(row).GetCell(6).CellStyle = style;
                              sheet.GetRow(row).GetCell(7).CellStyle = style;
                              if (true)
                              {
                                  XSSFDrawing patriarch = (XSSFDrawing)sheet.CreateDrawingPatriarch();
                                  List<byte[]> capImage = new List<byte[]>();
                                  capImage = thirft.QueryCmpLogImageH(item.ID, exportCurrDay);
                                  if (capImage.Count > 0)
                                  {
                                      byte[] bytes = capImage[0];
                                      int pictureIdx = workbook.AddPicture(bytes, PictureType.JPEG);
                                      XSSFClientAnchor anchor = new XSSFClientAnchor(0, 0, 0, 0, 6, row, 7, row + 1);
                                      XSSFPicture pict = (XSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
                                  }
                                  var faceObject = thirft.QueryFaceObj(item.tcUuid);
                                  if (faceObject.Count > 0 && faceObject[0].Tmplate.Count > 0)
                                  {
                                      byte[] bytes2 = faceObject[0].Tmplate[0].Img;
                                      int pictureIdx2 = workbook.AddPicture(bytes2, PictureType.JPEG);
                                      XSSFClientAnchor anchor2 = new XSSFClientAnchor(0, 0, 0, 0, 7, row, 8, row + 1);
                                      XSSFPicture pict2 = (XSSFPicture)patriarch.CreatePicture(anchor2, pictureIdx2);
                                  }
                              }
                              row++;
                              Thread.Sleep(1);
                          }
                          FileStream fs = new FileStream(saveExcelPath + ".xlsx", FileMode.Create, FileAccess.Write);
                          workbook.Write(fs);
                          workbook = null;
                      });
                MyMessage.showYes("导出完成!");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// 根据页数获取抓拍记录
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private List<MyCmpFaceLogWidthImg> GetCmpFaceLogByPage(int page)
        {
            int curpage = 0;
            int index = 0;
            List<int> pageSplit = new List<int>();
            var queryCount = thirft.QueryCmpRecordTotalCountH(cORViewModel.compOfRecordsValue);
            if (queryCount.Count <= 0)
            {
                return null;
            }
            cORViewModel.MaxCount = queryCount[0].Count;
            for (int no = queryCount[0].Dayarr.Count - 1; no >= 0; no--)
            {
                var dayary = queryCount[0].Dayarr[no];
                curpage += dayary.Count % cORViewModel.compOfRecordsValue.PageRowValue != 0 ? dayary.Count / cORViewModel.compOfRecordsValue.PageRowValue + 1 : dayary.Count / cORViewModel.compOfRecordsValue.PageRowValue;
                pageSplit.Add(curpage);
            }
            cORViewModel.MaxPage = curpage;
                foreach (var dayPage in pageSplit)
                {
                    if (page <= dayPage)
                    {
                        index = pageSplit.IndexOf(dayPage);
                        exportCurrDay = queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Daystr;
                        DateTime dt1 = Convert.ToDateTime(queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Daystr.Insert(6, "/").Insert(4, "/"));
                        TimeSpan dt1Span = new TimeSpan(dt1.Ticks);
                        DateTime dt2 = new DateTime(1970, 1, 1);
                        TimeSpan dt2Span = new TimeSpan(dt2.Ticks);
                        long longdtPkCompRecordStarTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                        if (longdtPkCompRecordStarTime > cORViewModel.compOfRecordsValue.StartDayValue)
                        {
                            cORViewModel.compOfRecordsValue.StartDayValue = longdtPkCompRecordStarTime;
                        }
                        if (page != cORViewModel.MaxPage)
                        {
                            long longdtPkCompRecordEndTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds) + 24 * 60 * 60;
                            var todayEndtime = Convert.ToInt64(new TimeSpan(dt1.AddDays(1).Ticks).TotalSeconds - dt2Span.TotalSeconds);
                            if (longdtPkCompRecordEndTime > todayEndtime)
                            {
                                longdtPkCompRecordEndTime = todayEndtime;
                            }
                            cORViewModel.compOfRecordsValue.EndDayValue = longdtPkCompRecordEndTime;
                        }
                        cORViewModel.compOfRecordsValue.MaxCount = queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Count;
                        break;
                    }
                }
                if (page > 1)
                {
                    int pageTem = 0;
                    if (index == 0)
                    {
                        pageTem = 0;
                    }
                    else
                    {
                        pageTem = pageSplit[index - 1];
                    }
                    cORViewModel.compOfRecordsValue.StartRowValue = cORViewModel.compOfRecordsValue.MaxCount - ((page - pageTem) * cORViewModel.compOfRecordsValue.PageRowValue);
                    if (cORViewModel.compOfRecordsValue.StartRowValue < 0)
                    {
                        cORViewModel.compOfRecordsValue.StartRowValue = 0;
                    }
                }
                else
                {
                    cORViewModel.compOfRecordsValue.StartRowValue = cORViewModel.compOfRecordsValue.MaxCount - (cORViewModel.compOfRecordsValue.PageRowValue);
                    if (cORViewModel.compOfRecordsValue.StartRowValue < 0)
                    {
                        cORViewModel.compOfRecordsValue.StartRowValue = 0;
                    }
                }
            return thirft.QueryCmpLog(cORViewModel.compOfRecordsValue);
        }
    }
}