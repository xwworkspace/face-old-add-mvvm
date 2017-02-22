using FaceSysByMvvm.Common;
using FaceSysByMvvm.Services;
using FaceSysByMvvm.ViewModel.TemplateManager;
using FaceSysClient.ClassPool;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FaceSysByMvvm.View.TemplateManager
{
    /// <summary>
    /// TemplateManager.xaml 的交互逻辑
    /// </summary>
    public partial class TemplateManager : UserControl
    {
        TemplateManagerViewModel tMViewModel;
        ThirftService thirft = new ThirftService();
        WriteLog _WriteLog = new WriteLog();
        delegate void GetTemplateManagerDelegate(int page);
        GetTemplateManagerDelegate cetTemplateManagerDelegate;
        delegate void GetTemplateByImageDelegate(int threshold,int count);
        GetTemplateByImageDelegate getTemplateByImageDelegate;

        public Action<string> QueryTemplateCmpDelegate;

        byte[] TemplatePhoto;

        public TemplateManager()
        {
            InitializeComponent();
            tMViewModel = new TemplateManagerViewModel();
            this.DataContext = tMViewModel;
            cetTemplateManagerDelegate = GetAllInfo;
            getTemplateByImageDelegate = GetTemplateByImage;
            if(tMViewModel.ListMyFaceObj.Count <= 0)
            {
                btnTemplateQuery_Click(null,null);
            }
            if (Login.ClientType == "1")
            {
                gridTempleteControl.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 根据图片查询模版
        /// </summary>
        private void GetTemplateByImage(int threshold, int count)
        {
            tMViewModel.LoadingVisiblity = "Visible";
            tMViewModel.ScanVisiblity = "Visible";
            tMViewModel.ListMyFaceObj = thirft.QueryFaceObjByImg(TemplatePhoto, threshold, count);
            tMViewModel.CurrPage = 1;
            tMViewModel.MaxPage = 1;
            tMViewModel.MaxCount = tMViewModel.ListMyFaceObj.Count;
            tMViewModel.LoadingVisiblity = "Hidden";
            tMViewModel.ScanVisiblity = "Hidden";
        }

        /// <summary>
        /// 查询模版的具体实现
        /// </summary>
        /// <param name="page"></param>
        private void GetAllInfo(int page)
        {
            //page = page - 1;
            tMViewModel.LoadingVisiblity = "Visible";
            List<MyCapFaceLogWithImg> listMyCapFaceLogWithImg = new List<MyCapFaceLogWithImg>();
            tMViewModel.MaxCount = thirft.QueryFaceObjTotalCount(tMViewModel.templateManagerValue);
            tMViewModel.MaxPage = tMViewModel.MaxCount % tMViewModel.templateManagerValue.PageRowValue != 0 ? tMViewModel.MaxCount / tMViewModel.templateManagerValue.PageRowValue + 1 : tMViewModel.MaxCount / tMViewModel.templateManagerValue.PageRowValue;
            tMViewModel.templateManagerValue.MaxCount = tMViewModel.MaxCount;
            if (page > 1 )
            {
                tMViewModel.templateManagerValue.StartRowValue = tMViewModel.MaxCount - (page * tMViewModel.templateManagerValue.PageRowValue);
                if (tMViewModel.templateManagerValue.StartRowValue < 0)
                {
                    tMViewModel.templateManagerValue.StartRowValue = 0;
                }
                
            }
            else
            {
                tMViewModel.templateManagerValue.StartRowValue = tMViewModel.MaxCount - (tMViewModel.templateManagerValue.PageRowValue);
                if (tMViewModel.templateManagerValue.StartRowValue < 0)
                {
                    tMViewModel.templateManagerValue.StartRowValue = 0;
                }
            }
            tMViewModel.CurrPage = page;
            tMViewModel.ListMyFaceObj = thirft.QueryFaceObj(tMViewModel.templateManagerValue);
            tMViewModel.LoadingVisiblity = "Hidden";
            this.Dispatcher.BeginInvoke(
                new Action(() => {
                    if(listViewTemplate.Items.Count > 0)
                    {
                        listViewTemplate.ScrollIntoView(listViewTemplate.Items[0]);
                    }                 
                } ));
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
            switch (pageBtn.Name)
            {
                case "btnTemplateFirstPage":
                    page = 1;
                    break;
                case "btnTemplatePastPage":
                    page = tMViewModel.CurrPage - 1;
                    break;
                case "btnTemplateNextPage":
                    page = tMViewModel.CurrPage + 1;
                    break;
                case "btnTemplateLastPage":
                    page = tMViewModel.MaxPage;
                    break;
                case "btnGoToPage":
                    try
                    {
                        page = Convert.ToInt32(txtGoToPage.Text.ToString().Trim());
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
            if (page> tMViewModel.MaxPage)
            {
                MyMessage.showYes("已经是尾页！");
                return;
            }
            cetTemplateManagerDelegate.BeginInvoke(page, null, null);
        }

        /// <summary>
        /// 模板各列宽度自动变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewTemplate_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            try
            {
                #region
                //获得模板listview
                GridView gv = listViewTemplate.View as GridView;
                if (gv != null)
                {
                    //循环获得类，设置列的宽度
                    foreach (GridViewColumn gvc in gv.Columns)
                    {
                        gvc.Width = (listViewTemplate.ActualWidth - 4) / 7;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("listViewTemplate_SizeChanged_1", ex);
                //MyMessage.showYes(ex.Message); 
            }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTemplateQuery_Click(object sender, RoutedEventArgs e)
        {
            tMViewModel.templateManagerValue.NameValue = tMViewModel.Name;
            if(combType.SelectedIndex == -1 || combType.SelectedIndex == 0)
            {
                tMViewModel.templateManagerValue.TypeValue = -1;
            }
            else
            {
                foreach (var basicinfo in BasicInfo.DefFaceObjType)
                {
                    if (basicinfo.Description == tMViewModel.Type[combType.SelectedIndex])
                    {
                        tMViewModel.templateManagerValue.TypeValue = basicinfo.Type == 0 ? -1 : basicinfo.Type;
                    }
                }
            }

            tMViewModel.templateManagerValue.SexValue = tMViewModel.SelectedSex == 0 ? -1 : tMViewModel.SelectedSex;
            tMViewModel.templateManagerValue.PageRowValue = Convert.ToInt32(tMViewModel.PageRow[tMViewModel.SelectedPageRow]);
            //todo(已完成 未测试) 修改年龄和时间
            tMViewModel.templateManagerValue.LittleAgeValue = tMViewModel.LittleAge == "" ? -1 : Convert.ToInt32(tMViewModel.LittleAge);
            tMViewModel.templateManagerValue.OldAgeValue = tMViewModel.OldAge == "" ? -1 : Convert.ToInt32(tMViewModel.OldAge);

            //开始时间
            long longdtPkCompRecordStarTime = -1;
            if (!string.IsNullOrEmpty (tMViewModel.StartDay))
            {
                string strCompRecordStarTime = tMViewModel.StartDay;
                DateTime dt1 = Convert.ToDateTime(strCompRecordStarTime);
                TimeSpan dt1Span = new TimeSpan(dt1.Ticks);

                DateTime dt2 = new DateTime(1970, 1, 1);
                TimeSpan dt2Span = new TimeSpan(dt2.Ticks);

                longdtPkCompRecordStarTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                if (tMViewModel.SelectedStartHour != -1)
                {
                    longdtPkCompRecordStarTime = longdtPkCompRecordStarTime + int.Parse(tMViewModel.SelectedStartHour.ToString()) * 60 * 60;
                }
            }
            tMViewModel.templateManagerValue.StartDayValue = longdtPkCompRecordStarTime;
            long longdtPkCompRecordEndTime = -1;
            if (!string.IsNullOrEmpty( tMViewModel.EndDay))
            {
                string strCompRecordEndTime = tMViewModel.EndDay;
                DateTime dt1 = Convert.ToDateTime(strCompRecordEndTime);
                TimeSpan dt1Span = new TimeSpan(dt1.Ticks);

                DateTime dt2 = new DateTime(1970, 1, 1);
                TimeSpan dt2Span = new TimeSpan(dt2.Ticks);

                longdtPkCompRecordEndTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                if (tMViewModel.SelectedEndHour != -1)
                {
                    longdtPkCompRecordEndTime = longdtPkCompRecordEndTime + (int.Parse(tMViewModel.SelectedEndHour.ToString()) +1) * 60 * 60;
                }
            }
            tMViewModel.templateManagerValue.EndDayValue = longdtPkCompRecordEndTime;

            //tMViewModel.templateManagerValue.StartDayValue = -1;
            //tMViewModel.templateManagerValue.EndDayValue = -1;

            cetTemplateManagerDelegate.BeginInvoke(1, null, null);
        }

        /// <summary>
        /// 切换选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewTemplate_SelectionChanged(object sender, SelectionChangedEventArgs eventArgs)
        {
            MyFaceObj myFaceObj = listViewTemplate.SelectedItem as MyFaceObj;
            if(myFaceObj ==null)
            {
                return;
            }
            //获得模板id和所属的人脸Id
            string nTemplateFaceID = myFaceObj.fa_ob_tcUuid;
            int nTemplateId = myFaceObj.nMain_ftID;
            List<FaceObj> ListFaceObj = new List<FaceObj>();
            ListFaceObj.Add(myFaceObj.faceObj);
            byte[] imageBytes = new byte[10];

            btnTemplatePhoto.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.btnTemplatePhoto.Content = "";
                //遍历得到对应模板ID下的照片
                if (ListFaceObj.Count != 0)
                {
                    if (ListFaceObj[0].NMain_ftID == nTemplateId)
                    {
                        try
                        {
                            if (nTemplateId == 0)
                            {
                                nTemplateId = 1;
                            }
                            for (int i = 0; i < ListFaceObj[0].Tmplate.Count; i++)
                            {
                                if (ListFaceObj[0].Tmplate[i].NIndex == nTemplateId - 1)
                                {
                                    GridTemplatePic.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        GridTemplatePic.Background = new ImageBrush
                                        {
                                            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/抓拍照片纯背景.png"))
                                        };
                                        GridAfterbtnTemplatePhoto.Visibility = Visibility.Visible;
                                    }));
                                    btnTemplatePhoto.Visibility = Visibility.Visible;
                                    imageBytes = ListFaceObj[0].Tmplate[i].Img;
                                    System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes);//img是从数据库中读取出来的字节数组
                                                                                                       //读入MemoryStream对象 
                                    BitmapImage myBitmapImage = new BitmapImage();
                                    myBitmapImage.BeginInit();
                                    myBitmapImage.StreamSource = ms;
                                    myBitmapImage.EndInit();
                                    this.btnTemplatePhoto.Background = new ImageBrush { ImageSource = myBitmapImage };
                                    myBitmapImage = null;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            if (e.InnerException.Message.Contains("0x88982F72"))
                            {
                                try
                                {
                                    FileStream fs = new FileStream(System.Windows.Forms.Application.StartupPath + "\\转存的图片.jpg", FileMode.OpenOrCreate);
                                    fs.Write(imageBytes, 0, imageBytes.Length);

                                    fs.Flush();
                                    fs.Close();
                                    fs.Dispose();

                                    System.Drawing.Image img = new System.Drawing.Bitmap(System.Windows.Forms.Application.StartupPath + "\\转存的图片.jpg");
                                    MemoryStream stream = new MemoryStream();
                                    img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                                    BinaryReader br = new BinaryReader(stream);
                                    byte[] photo = stream.ToArray();
                                    stream.Close();
                                    stream.Dispose();
                                    System.IO.MemoryStream ms = new System.IO.MemoryStream(photo);//img是从数据库中读取出来的字节数组
                                                                                                  //读入MemoryStream对象 
                                    BitmapImage myBitmapImage = new BitmapImage();
                                    myBitmapImage.BeginInit();
                                    myBitmapImage.StreamSource = ms;
                                    myBitmapImage.EndInit();
                                    this.btnTemplatePhoto.Background = new ImageBrush { ImageSource = myBitmapImage };
                                    myBitmapImage = null;
                                    //File.Delete(System.Windows.Forms.Application.StartupPath + "\\转存的图片.jpg");
                                }
                                catch (Exception ex)
                                {
                                    MyMessage.showYes("您当前的操作过于频繁,请稍后重试");
                                }

                            }
                            else
                            {
                                //imageBytes;
                                MyMessage.showYes("获得数据库图片错误！" + e.InnerException.Message);
                            }
                        }
                    }
                }
            }));
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            TempleteInfoPop tIP = new TempleteInfoPop();
            tIP.SetTempleteInfo(null,1,null);
            tIP.ShowDialog();
        }
        
        /// <summary>
        /// 修改模板
        /// </summary>
        private void UpdateTemplate()
        {
            if (listViewTemplate.SelectedIndex < 0)
            {
                MyMessage.showYes("请选中需要修改的模版!");
                return;
            }
            MyFaceObj myFaceObj = listViewTemplate.SelectedItem as MyFaceObj;
            TempleteInfoPop tIP = new TempleteInfoPop();
            tIP.SetTempleteInfo(myFaceObj,2,null);
            tIP.ShowDialog();
        }

        /// <summary>
        /// 双击修改模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewTemplate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Login.ClientType == "1")
            {
                return;
            }
            UpdateTemplate();
        }

        /// <summary>
        /// 删除模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTemplate.SelectedIndex < 0)
            {
                MyMessage.showYes("请选中需要删除的模版!");
                return;
            }
           
            MyFaceObj myFaceObj = listViewTemplate.SelectedItem as MyFaceObj;
            int nSucess = -1;
            if (myFaceObj != null)
            {
                bool? result = MyMessage.Show("是否删除模板！");
                if (result == true)//点击确定
                {
                    nSucess = thirft.DelFaceObj(myFaceObj.fa_ob_tcUuid);
                    if (nSucess == 0)
                    {
                        MyMessage.showYes("删除模板" + myFaceObj.tcName + "成功！");
                    }
                    if (nSucess == -1)
                    {
                        MyMessage.showYes("删除模板失败！");
                    }
                }             
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 批量导入模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBatchImportTemplate_Click(object sender, RoutedEventArgs e)
        {
            TempleteImportPop templeteImportPop = new TempleteImportPop();
            templeteImportPop.ShowDialog();
        }

        /// <summary>
        /// 指定查询照片按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgSpecifyQueryPhoto_Click(object sender, RoutedEventArgs e)
        {
            //捕获异常
            try
            {
                //打开选择文件对话框
                System.Windows.Forms.OpenFileDialog _openFileDialog = new System.Windows.Forms.OpenFileDialog();
                _openFileDialog.Filter = "jpg|*.jpg|bmp|*.bmp";
                System.Windows.Forms.DialogResult result = _openFileDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string StrPath = _openFileDialog.FileName;
                    btnPIcPath.Content = StrPath;
                    TemplatePhoto = System.IO.File.ReadAllBytes(StrPath);
                    //显示选择的图片
                    System.Windows.Controls.Image img2 = new System.Windows.Controls.Image();
                    BitmapImage myBitmapImage = new BitmapImage();
                    myBitmapImage.BeginInit();
                    myBitmapImage.StreamSource = new System.IO.MemoryStream(TemplatePhoto);
                    myBitmapImage.EndInit();
                    img2.Source = myBitmapImage;
                    myBitmapImage = null;
                    this.imgSpecifyQueryPhoto.Content = img2;
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("btnSpecifyQueryPhoto_Click", ex);
                return;
            }
        }

        /// <summary>
        /// 清空图片查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveQueryPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region
                //清空查询条件
                this.imgSpecifyQueryPhoto.Content = "";
                btnPIcPath.Content = "";
                TemplatePhoto = null;
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("btnRemoveQueryPhoto_Click", ex);
                //System.Windows.MessageBox.Show(ex.Message); 
            }
        }

        /// <summary>
        /// 根据照片返回相似结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryPhotoForScroe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TemplatePhoto == null || TemplatePhoto.Length <= 0)
                {
                    MyMessage.showYes("请指定照片");
                    //this.imgSpecifyQueryPhoto.Content = "请指定照片";
                    return;
                }

                //获得相似度阈值
                int nThreshold = -1;
                if (combTemplateScroe.SelectedItem == null)
                {
                    nThreshold = 10;
                }
                else
                {
                    nThreshold = int.Parse(combTemplateScroe.SelectedItem.ToString());
                }
                //获得最大返回行数
                int nMaxCount = -1;
                if (combTemplateMaxBack.SelectedItem == null)
                {
                    nMaxCount = 5;
                }
                else
                {
                    nMaxCount = int.Parse(combTemplateMaxBack.SelectedItem.ToString());
                }
              
                getTemplateByImageDelegate.BeginInvoke(nThreshold, nMaxCount, null, null);

                //AddParams add = new AddParams(TemplatePhoto, nThreshold, nMaxCount);
                //Thread thread = new Thread(new ParameterizedThreadStart(QueryPhotoForScroe));
                //thread.SetApartmentState(ApartmentState.STA);
                //thread.Start(add);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 切换查询方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryTypeChange_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if(btn == btnTiaojianQuery)
            {
                tMViewModel.TiaojianQueryVisiblity = "Visible";
                tMViewModel.PicQueryVisiblity = "Hidden";
            }
            else
            {
                tMViewModel.TiaojianQueryVisiblity = "Hidden";
                tMViewModel.PicQueryVisiblity = "Visible";
            }
        }

        /// <summary>
        /// 点击修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyTemplate_Click(object sender, RoutedEventArgs e)
        {
            UpdateTemplate();
        }

        /// <summary>
        /// 查询模版的比对记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryTemplateCmp_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTemplate.SelectedIndex < 0)
            {
                MyMessage.showYes("请选中需要查询的模版!");
                return;
            }
            MyFaceObj myFaceObj = listViewTemplate.SelectedItem as MyFaceObj;
            QueryTemplateCmpDelegate(myFaceObj.tcName);
        }
    }
}
