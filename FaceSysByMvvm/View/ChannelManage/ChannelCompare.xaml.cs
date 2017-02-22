using FaceSysByMvvm.ViewModel.ChannelManage;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using FaceSysClient.ClassPool;
using FaceSysByMvvm.Services;
using static FaceSysByMvvm.ViewModel.ChannelManage.ChannelCompareViewModel;
using FaceSysByMvvm.Common;

namespace FaceSysByMvvm.View.ChannelManage
{
    /// <summary>
    /// ChannelCompare.xaml 的交互逻辑
    /// </summary>
    public partial class ChannelCompare : Window
    {
        ChannelCompareViewModel cCViewModel;
        ThirftService thirft = new ThirftService();
        List<CmpFaceLogWidthImg> CmpFaceList = new List<CmpFaceLogWidthImg>();
        List<MyFaceObj> MyFaceObjListTemp = new List<MyFaceObj>();
        WriteLog _WriteLog = new WriteLog();
        public ChannelCompare()
        {
            InitializeComponent();
            cCViewModel = new ChannelCompareViewModel();
            this.DataContext = cCViewModel;
            combNPerson.SelectedIndex = 0;
        }

        public void GetCmpByCapId()
        {
            try
            {
                CmpFaceList = thirft.QueryCmpByCapIdWidthImgH(cCViewModel.Id,cCViewModel.Day);
                List<FaceObj> ListFaceObj = new List<FaceObj>();
                int i = 0;
                foreach (var cmpFaceLogWidthImg in CmpFaceList)
                {
                    List<FaceObj> listFaceObj = thirft.QueryFaceObj(cmpFaceLogWidthImg.TcUuid);
                    foreach (var faceObj in listFaceObj)
                    {
                        ListFaceObj.Add(faceObj);
                    }
                }
                foreach (FaceObj cmpFace in ListFaceObj)
                {
                    for (int j = 0; j < cmpFace.Tmplate.Count; j++)
                    {          
                        MyFaceObj myFaceObj = new MyFaceObj();
                        //读入MemoryStream对象 
                        BitmapImage myBitmapImage = new BitmapImage();
                        myBitmapImage.BeginInit();
                        myBitmapImage.StreamSource = new System.IO.MemoryStream(cmpFace.Tmplate[j].Img);
                        myFaceObj.img = myBitmapImage;
                        myBitmapImage.EndInit();
                        myBitmapImage = null;
                        myFaceObj.fa_ob_tcUuid = cmpFace.TcUuid;
                        myFaceObj.tcName = cmpFace.TcName;
                        myFaceObj.nAge = CmpFaceList[i].Score;
                        MyFaceObjListTemp.Add(myFaceObj);
                        i++;
                    }
                }
                ShowFaceObject();
            }
            catch(Exception ex)
            {
                _WriteLog.WriteToLog("GetCmpByCapId", ex);
            }
            
        }

        private void ShowFaceObject()
        {
            int count = 0;
            cCViewModel.MyFaceObjList.Clear();
            if (combNPerson.SelectedIndex < 0)
            {
                count = 2;
            }
            else
            {
                count = Convert.ToInt32(cCViewModel.TemplateCount[combNPerson.SelectedIndex]);
            }
            for (int i = 0; i < count && i< MyFaceObjListTemp.Count; i++)
            {
                cCViewModel.MyFaceObjList.Add(MyFaceObjListTemp[i]);
            }
            ListFaceObject.Items.Refresh();
        }

        public void SetIdentifyResults(IdentifyResults _IdentifyResults)
        {
            cCViewModel.Image = _IdentifyResults.CapImg;
            long _longtime = _IdentifyResults.Info.Time;
            DateTime s = new DateTime(1970, 1, 1);
            s = s.AddSeconds(_longtime);
            cCViewModel.Day = s.ToString("yyyy/MM/dd HH:mm:ss").Split(' ')[0].Replace("/", "").Replace("/", "");
            cCViewModel.Id = _IdentifyResults.ID;
            GetCmpByCapId();
            List<byte[]> senceImg = thirft.QuerySenceImg(_IdentifyResults.ID, cCViewModel.Day);
            if (senceImg != null && senceImg.Count > 0 && senceImg[0].Length > 0)
            {
                BitmapImage bitImage = new BitmapImage();
                bitImage.BeginInit();
                bitImage.StreamSource = new System.IO.MemoryStream(senceImg[0]);
                bitImage.EndInit();
                image_SenceImg.Source = bitImage;
            }
            else
            {
                MyMessage.showYes("获取抓拍实时帧失败!");
            }
        }

        private void ListFaceObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyFaceObj myFaceObj = ListFaceObject.SelectedItem as MyFaceObj;
            List<FaceObj> faceObj = thirft.QueryFaceObj(myFaceObj.fa_ob_tcUuid);
            for (int i=0;i<13;i++)
            {
                CapInfo capInfo = new CapInfo();
                cCViewModel.CapInfoList.Add(capInfo);
            }
            cCViewModel.CapInfoList[0].Key = "人脸uuidID";
            cCViewModel.CapInfoList[0].Value = faceObj[0].TcUuid.ToString();

            cCViewModel.CapInfoList[1].Key = "姓名";
            cCViewModel.CapInfoList[1].Value = faceObj[0].TcName.ToString();
            //
            cCViewModel.CapInfoList[2].Key = "类型";
            cCViewModel.CapInfoList[2].Value = BasicInfo.GetTypeById(faceObj[0].NType);
            //
            cCViewModel.CapInfoList[3].Key = "性别";
            cCViewModel.CapInfoList[3].Value = cCViewModel.Sex[faceObj[0].NSex];

            cCViewModel.CapInfoList[4].Key = "人脸对象添加时间";
            cCViewModel.CapInfoList[4].Value = faceObj[0].DTm.ToString();

            cCViewModel.CapInfoList[5].Key = "模板识别的年龄";
            cCViewModel.CapInfoList[5].Value = faceObj[0].NAge.ToString();

            cCViewModel.CapInfoList[6].Key = "人脸备注";
            cCViewModel.CapInfoList[6].Value = faceObj[0].TcRemarks.ToString();

            cCViewModel.CapInfoList[7].Key = "模板uuid";
            cCViewModel.CapInfoList[7].Value = faceObj[0].Tmplate[0].TcUuid.ToString();

            cCViewModel.CapInfoList[8].Key = "所属FaceObj的uuid";
            cCViewModel.CapInfoList[8].Value = faceObj[0].Tmplate[0].TcObjid.ToString();

            cCViewModel.CapInfoList[9].Key = "模板标识键";
            cCViewModel.CapInfoList[9].Value = faceObj[0].Tmplate[0].TcKey.ToString();

            cCViewModel.CapInfoList[10].Key = "模板序号";
            cCViewModel.CapInfoList[10].Value = faceObj[0].Tmplate[0].NIndex.ToString();

            cCViewModel.CapInfoList[11].Key = "模板添加时间";
            cCViewModel.CapInfoList[11].Value = faceObj[0].Tmplate[0].DTm.ToString();

            cCViewModel.CapInfoList[12].Key = "模板备注";
            cCViewModel.CapInfoList[12].Value = faceObj[0].Tmplate[0].TcRemarks.ToString();

            ListViewRegisterInfo.Items.Refresh();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 切换显示个数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combNPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowFaceObject();
        }

        private void ListFaceObject_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            try
            {
                #region
                //获得注册信息listview
                GridView gv = ListFaceObject.View as GridView;
                if (gv != null)
                {
                    //设置第一列宽度
                    GridViewColumn gvc = gv.Columns[0];
                    gvc.Width = (ListFaceObject.ActualWidth - 4) / 4 * 2.3;
                    //设置第二列宽度
                    gvc = gv.Columns[1];
                    gvc.Width = (ListFaceObject.ActualWidth - 4) / 4 * 0.5;
                    //设置第二列宽度
                    gvc = gv.Columns[1];
                    gvc.Width = (ListFaceObject.ActualWidth - 4) / 4 * 1.2;
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("ListFaceObject_SizeChanged_1", ex);
                //System.Windows.MessageBox.Show(ex.Message); 
            }
        }
    }
}
