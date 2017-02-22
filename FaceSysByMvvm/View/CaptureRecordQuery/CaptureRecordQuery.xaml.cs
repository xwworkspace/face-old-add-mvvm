using FaceSysByMvvm.Services;
using FaceSysByMvvm.View.TemplateManager;
using FaceSysByMvvm.ViewModel.CaptureRecordQuery;
using FaceSysClient.ClassPool;
using System;
using System.Collections.Generic;
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

namespace FaceSysByMvvm.View.CaptureRecordQuery
{
    /// <summary>
    /// CaptureRecordQuery.xaml 的交互逻辑
    /// </summary>
    public partial class CaptureRecordQuery : UserControl
    {
        CaptureRecordQueryViewModel cRQViewModel;
        ThirftService thirft = new ThirftService();
        WriteLog _WriteLog = new WriteLog();
        delegate void GetCaptureRecordDelegate(int page);
        GetCaptureRecordDelegate cetCaptureRecordDelegate;
        string currDay = "";
        public CaptureRecordQuery()
        {
            InitializeComponent();
            cRQViewModel = new CaptureRecordQueryViewModel();
            cetCaptureRecordDelegate = GetCaptureRecord;
            this.DataContext = cRQViewModel;
        }

        /// <summary>
        /// 获得抓拍记录信息
        /// </summary>
        /// <param name="page">查询的页码</param>
        /// <returns></returns>
        public void GetCaptureRecord(int page)
        {
            try
            {
                List<int> pageSplit = new List<int>();
                int curpage = 0;
                int index = 0;
                cRQViewModel.LoadingVisiblity = "Visible";
                List<MyCapFaceLogWithImg> listMyCapFaceLogWithImg = new List<MyCapFaceLogWithImg>();
                List<SCountInfo> queryCount = new List<SCountInfo>();
                if (Login.ClientType != "1")
                {
                    queryCount = thirft.QueryCapRecordTotalCountH(cRQViewModel.captureRecordQueryValue);
                }
                else
                {
                    if (cRQViewModel.captureRecordQueryValue.ChannelValue != "")
                    {
                        List<string> channelTemp = new List<string>();
                        channelTemp.Add(cRQViewModel.captureRecordQueryValue.ChannelValue);
                        queryCount = thirft.QueryCapRecordTotalCountHSXC(cRQViewModel.captureRecordQueryValue, channelTemp);
                    }
                    else
                    {
                        queryCount = thirft.QueryCapRecordTotalCountHSXC(cRQViewModel.captureRecordQueryValue, cRQViewModel.ChannelId);
                    }
                }

                if (queryCount.Count <= 0)
                {
                    return;
                }
                cRQViewModel.MaxCount = queryCount[0].Count;
                for (int no = queryCount[0].Dayarr.Count - 1; no >= 0; no--)
                {
                    var dayary = queryCount[0].Dayarr[no];
                    curpage += dayary.Count % cRQViewModel.captureRecordQueryValue.PageRowValue != 0 ? dayary.Count / cRQViewModel.captureRecordQueryValue.PageRowValue + 1 : dayary.Count / cRQViewModel.captureRecordQueryValue.PageRowValue;
                    pageSplit.Add(curpage);
                }
                cRQViewModel.MaxPage = curpage;
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
                        if (longdtPkCompRecordStarTime > cRQViewModel.captureRecordQueryValue.StartDayValue)
                        {
                            cRQViewModel.captureRecordQueryValue.StartDayValue = longdtPkCompRecordStarTime;
                        }
                        if (page != cRQViewModel.MaxPage)
                        {
                            long longdtPkCompRecordEndTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds) + 24 * 60 * 60;
                            var todayEndtime = Convert.ToInt64(new TimeSpan(dt1.AddDays(1).Ticks).TotalSeconds - dt2Span.TotalSeconds);
                            if (longdtPkCompRecordEndTime > todayEndtime)
                            {
                                longdtPkCompRecordEndTime = todayEndtime;
                            }
                            cRQViewModel.captureRecordQueryValue.EndDayValue = longdtPkCompRecordEndTime;
                        }
                        cRQViewModel.captureRecordQueryValue.MaxCount = queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Count;
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
                    cRQViewModel.captureRecordQueryValue.StartRowValue = cRQViewModel.captureRecordQueryValue.MaxCount - ((page - pageTem) * cRQViewModel.captureRecordQueryValue.PageRowValue);
                    if (cRQViewModel.captureRecordQueryValue.StartRowValue < 0)
                    {
                        cRQViewModel.captureRecordQueryValue.StartRowValue = 0;
                    }
                }
                else
                {
                    cRQViewModel.captureRecordQueryValue.StartRowValue = cRQViewModel.captureRecordQueryValue.MaxCount - (cRQViewModel.captureRecordQueryValue.PageRowValue);
                    if (cRQViewModel.captureRecordQueryValue.StartRowValue < 0)
                    {
                        cRQViewModel.captureRecordQueryValue.StartRowValue = 0;
                    }
                }
                cRQViewModel.CurrPage = page;
                int countTem = 0;
                for (int n = 0; n <= index; n++)
                {
                    var dayary = queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - n].Count;
                    countTem += dayary;
                }
                cRQViewModel.captureRecordQueryValue.MaxCount = countTem;

                if (Login.ClientType != "1")
                {
                    cRQViewModel.ListMyCapFaceLogWithImg = thirft.QueryCapLog(cRQViewModel.captureRecordQueryValue);
                }
                else
                {
                    if (cRQViewModel.captureRecordQueryValue.ChannelValue != "")
                    {
                        List<string> channelTemp = new List<string>();
                        channelTemp.Add(cRQViewModel.captureRecordQueryValue.ChannelValue);
                        cRQViewModel.ListMyCapFaceLogWithImg = thirft.QueryCapLogSXC(cRQViewModel.captureRecordQueryValue, channelTemp);
                    }
                    else
                    {
                        cRQViewModel.ListMyCapFaceLogWithImg = thirft.QueryCapLogSXC(cRQViewModel.captureRecordQueryValue, cRQViewModel.ChannelId);
                    }
                }
                cRQViewModel.LoadingVisiblity = "Hidden";
                this.Dispatcher.BeginInvoke(
                            new Action(() =>
                            {
                                if (listViewCaptureRecord.Items.Count > 0)
                                {
                                    listViewCaptureRecord.ScrollIntoView(listViewCaptureRecord.Items[0]);
                                }
                            }));
            }
            catch (Exception ex)
            {
                MyMessage.showYes(ex.Message);
                _WriteLog.WriteToLog("GetCaptureRecord", ex);
            }
            finally
            {
                cRQViewModel.LoadingVisiblity = "Hidden";
            }
        }

        /// <summary>
        /// 刷新通道下拉列表
        /// </summary>
        internal void RefreshChannelComboBox()
        {
            cRQViewModel.RefreshChannelList();
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCaptureRecordQuery_Click(object sender, RoutedEventArgs e)
        {
            cRQViewModel.captureRecordQueryValue.ChannelValue = cRQViewModel.SelectedChannel == 0 ? "" : cRQViewModel.ChannelId[cRQViewModel.SelectedChannel-1];
            cRQViewModel.captureRecordQueryValue.PageRowValue = Convert.ToInt32(cRQViewModel.PageRow[cRQViewModel.SelectedPageRow]);
            IntiQueryTime();
            if (cRQViewModel.captureRecordQueryValue.StartDayValue == -1 || (cRQViewModel.captureRecordQueryValue.EndDayValue - cRQViewModel.captureRecordQueryValue.StartDayValue > 60 * 60 * 24 * 7))
            {
                MyMessage.showYes("起始时间不能为空且起始和终止时间相差不能超过七天!");
                return;
            }
            cetCaptureRecordDelegate.BeginInvoke(1, null, null);
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
                case "btnCaptureRecordFirstPage":
                    page = 1;
                    break;
                case "btnCaptureRecordPastPage":
                    page = cRQViewModel.CurrPage -1;
                    break;
                case "btnCaptureRecordNextPage":
                    page = cRQViewModel.CurrPage+1 ;
                    break;
                case "btnCaptureRecordLastPage":
                    page = cRQViewModel.MaxPage;
                    break;
                case "btnCaptureRecordGotoPage":
                    try
                    {
                        page = Convert.ToInt32(txtCaptureRecordGotoPage.Text.ToString().Trim());
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
            if (page > cRQViewModel.MaxPage)
            {
                MyMessage.showYes("已经是尾页！");
                return;
            }
            cetCaptureRecordDelegate.BeginInvoke(page, null, null);
        }

        /// <summary>
        /// 抓拍记录变更选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewCaptureRecord_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyCapFaceLogWithImg _MyCapFaceLogWithImg = listViewCaptureRecord.SelectedItem as MyCapFaceLogWithImg;
            Thread threadQuery = new Thread(new ParameterizedThreadStart(threadlistViewCaptureRecord));
            threadQuery.SetApartmentState(ApartmentState.STA);
            threadQuery.Start(_MyCapFaceLogWithImg);
        }

        /// <summary>
        /// 自动变更listview列宽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewCaptureRecord_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            try
            {
                //获得抓拍listview
                GridView gv = listViewCaptureRecord.View as GridView;
                if (gv != null)
                {
                    //循环获得列，并且设置列宽
                    foreach (GridViewColumn gvc in gv.Columns)
                    {
                        gvc.Width = (listViewCaptureRecord.ActualWidth - 4) / 3;
                    }
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("listViewCaptureRecord_SizeChanged_1", ex);
                //MyMessage.showYes(ex.Message); 
            }
        }

        public void threadlistViewCaptureRecord(object obj)
        {
            try
            {
                MyCapFaceLogWithImg _MyCapFaceLogWithImg = (MyCapFaceLogWithImg)obj;;
                List<byte[]> listImageBytes = new List<byte[]>();
                listImageBytes = thirft.QueryCapLogImageH(_MyCapFaceLogWithImg.ID, currDay);
                //得到图片
                if (listImageBytes[0].Length > 0)
                {
                    GridCapCapPic.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        GridCapCapPic.Background = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/抓拍照片纯背景.png"))
                        };
                        GridAfterbtnPicCaptureRecord.Visibility = Visibility.Visible;
                    }));
                    //读入MemoryStream对象
                    btnPicCaptureRecord.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        btnPicCaptureRecord.Visibility = Visibility.Visible;
                        BitmapImage myBitmapImage = new BitmapImage();
                        myBitmapImage.BeginInit();
                        myBitmapImage.StreamSource = new System.IO.MemoryStream(listImageBytes[0]);
                        myBitmapImage.EndInit();
                        btnPicCaptureRecord.Background = new ImageBrush { ImageSource = myBitmapImage };
                        myBitmapImage = null;
                    }));
                    List<byte[]> senceImg = thirft.QuerySenceImg(_MyCapFaceLogWithImg.ID, _MyCapFaceLogWithImg.time.Split(' ')[0].Replace(@"/", "").Replace(@"/", ""));
                    if (senceImg != null && senceImg.Count > 0 && senceImg[0].Length > 0)
                    {
                        btnPicCaptureRecord.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            BitmapImage bitImage = new BitmapImage();
                            bitImage.BeginInit();
                            bitImage.StreamSource = new System.IO.MemoryStream(senceImg[0]);
                            bitImage.EndInit();
                            image_SenceImg.Source = bitImage;
                        }));                       
                    }
                    else
                    {
                        btnPicCaptureRecord.Dispatcher.BeginInvoke(new Action(() =>
                        { image_SenceImg.Source = null; }));   
                    }
                }
                else
                {
                    btnPicCaptureRecord.Dispatcher.BeginInvoke(new Action(() =>
                    { btnPicCaptureRecord.Background = null; }));
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("threadlistViewCaptureRecord", ex); 
            }
            Thread.CurrentThread.Abort();
        }

        /// <summary>
        /// 双击添加模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewCaptureRecord_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Login.ClientType == "1")
            {
                return;
            }
            MyCapFaceLogWithImg cmpFaceLogWidthImg = listViewCaptureRecord.SelectedItem as MyCapFaceLogWithImg;
            if (cmpFaceLogWidthImg == null)
            {
                return;
            }
            List<byte[]> listImageBytes = new List<byte[]>();
            listImageBytes = thirft.QueryCapLogImageH(cmpFaceLogWidthImg.ID, currDay);
            TempleteInfoPop tIP = new TempleteInfoPop();
            tIP.SetTempleteInfo(null,3, listImageBytes[0]);
            tIP.ShowDialog(); 
        }

        /// <summary>
        /// 初始化时间
        /// </summary>
        private void IntiQueryTime()
        {
            //开始时间
            long longdtPkCompRecordStarTime = -1;
            if (cRQViewModel.StartDay != string.Empty)
            {
                string strCompRecordStarTime = cRQViewModel.StartDay;
                DateTime dt1 = Convert.ToDateTime(strCompRecordStarTime);
                TimeSpan dt1Span = new TimeSpan(dt1.Ticks);

                DateTime dt2 = new DateTime(1970, 1, 1);
                TimeSpan dt2Span = new TimeSpan(dt2.Ticks);

                longdtPkCompRecordStarTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                if (cRQViewModel.SelectedStartHour != -1)
                {
                    longdtPkCompRecordStarTime = longdtPkCompRecordStarTime + int.Parse(cRQViewModel.SelectedStartHour.ToString()) * 60 * 60 + combCompRecordStarTimeMinutes.SelectedIndex * 60;
                }
            }
            cRQViewModel.captureRecordQueryValue.StartDayValue = longdtPkCompRecordStarTime;
            //结束时间
            long longdtPkCompRecordEndTime = -1;
            if (cRQViewModel.EndDay != string.Empty)
            {
                string strCompRecordEndTime = cRQViewModel.EndDay;
                DateTime dt1 = Convert.ToDateTime(strCompRecordEndTime);
                TimeSpan dt1Span = new TimeSpan(dt1.Ticks);

                DateTime dt2 = new DateTime(1970, 1, 1);
                TimeSpan dt2Span = new TimeSpan(dt2.Ticks);

                longdtPkCompRecordEndTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                if (cRQViewModel.SelectedEndHour != -1)
                {
                    longdtPkCompRecordEndTime = longdtPkCompRecordEndTime + int.Parse(cRQViewModel.SelectedEndHour.ToString()) * 60 * 60 + combCompRecordEndTimeMinutes.SelectedIndex * 60 + 3660;
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
            cRQViewModel.captureRecordQueryValue.EndDayValue = longdtPkCompRecordEndTime;
        }

        /// <summary>
        /// 清空时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearDatePickerTime(object sender, MouseButtonEventArgs e)
        {
            DatePicker dp = sender as DatePicker;
            dp.Text = "";
        }


    }
}
