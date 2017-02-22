using FaceSysByMvvm.Common;
using FaceSysByMvvm.Services;
using FaceSysByMvvm.ViewModel.TemplateManager;
using FaceSysClient.ClassPool;
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

namespace FaceSysByMvvm.View.TemplateManager
{
    /// <summary>
    /// TempleteInfoPop.xaml 的交互逻辑
    /// </summary>
    public partial class TempleteInfoPop : Window
    {
        public validationRule _validationRule = new validationRule();
        TempleteInfoPopViewModel tIPViewModel;
        ThirftService thirft = new ThirftService();
        WriteLog _WriteLog = new WriteLog();
        private string[] fileName = new string[5];//保存五张照片的地址
        private string strBtnName;//记录被选中的模板照片
        private byte[] _strPhotoMain;//保存主照片
        public byte[][] strPhoto = new byte[5][];//5张照片的数据
        private string[] fileNameChoice; //选择的照片地址
        public TempleteInfoPop()
        {
            try
            {
                InitializeComponent();
                tIPViewModel = new TempleteInfoPopViewModel();
                this.DataContext = tIPViewModel;
                for (int i = 0; i < fileName.Length; i++)
                {
                    fileName[i] = "空";
                }
                this.Loaded += TempleteInfoPop_Loaded;
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("AddTemplate：AddTemplate", ex);
                return;
            }

        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TempleteInfoPop_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //判断人脸对象不为空，那么显示具体信息
                if (tIPViewModel._FaceObj != null)
                {
                    //判断朱模板ID是否存在，显示主模板照片
                    if (tIPViewModel._FaceObj.NMain_ftID != 0)
                    {
                        for (int i = 0; i < tIPViewModel._FaceObj.Tmplate.Count; i++)
                        {
                            if (tIPViewModel._FaceObj.Tmplate[i].NIndex == tIPViewModel._FaceObj.NMain_ftID - 1)
                            {
                                System.Windows.Controls.Image img1 = new System.Windows.Controls.Image();
                                BitmapImage myBitmapImage = new BitmapImage();
                                myBitmapImage.BeginInit();
                                myBitmapImage.StreamSource = new System.IO.MemoryStream(tIPViewModel._FaceObj.Tmplate[i].Img);
                                myBitmapImage.EndInit();
                                img1.Source = myBitmapImage;
                                this.btnPicMain.Content = img1;
                                //btnPicMain.Background = new ImageBrush { ImageSource = myBitmapImage };
                                myBitmapImage = null;
                                _strPhotoMain = tIPViewModel._FaceObj.Tmplate[i].Img;
                                this.btnPicMain.IsEnabled = true;
                            }
                        }
                    }
                    //第一张照片
                    if (tIPViewModel._FaceObj.Tmplate.Count > 0 && tIPViewModel._FaceObj.Tmplate[0].Img.Length > 0)
                    {
                        for (int i = 0; i < tIPViewModel._FaceObj.Tmplate.Count; i++)//遍历
                        {
                            if (tIPViewModel._FaceObj.Tmplate[i].NIndex == 0)//查找nindex等于0的模板
                            {
                                System.Windows.Controls.Image img1 = new System.Windows.Controls.Image();
                                BitmapImage myBitmapImage = new BitmapImage();
                                myBitmapImage.BeginInit();
                                myBitmapImage.StreamSource = new System.IO.MemoryStream(tIPViewModel._FaceObj.Tmplate[i].Img);
                                myBitmapImage.EndInit();
                                img1.Source = myBitmapImage;
                                myBitmapImage = null;
                                btnPicFirst.Content = img1;
                                strPhoto[0] = tIPViewModel._FaceObj.Tmplate[i].Img;
                                fileName[0] = "1";
                                this.btnPicFirst.IsEnabled = true;
                            }
                        }
                    }
                    //第二张照片
                    if (tIPViewModel._FaceObj.Tmplate.Count > 1 && tIPViewModel._FaceObj.Tmplate[1].Img.Length > 0)
                    {
                        for (int i = 0; i < tIPViewModel._FaceObj.Tmplate.Count; i++)//遍历
                        {
                            if (tIPViewModel._FaceObj.Tmplate[i].NIndex == 1)//查找nindex等于0的模板
                            {
                                System.Windows.Controls.Image img1 = new System.Windows.Controls.Image();
                                BitmapImage myBitmapImage = new BitmapImage();
                                myBitmapImage.BeginInit();
                                myBitmapImage.StreamSource = new System.IO.MemoryStream(tIPViewModel._FaceObj.Tmplate[i].Img);
                                myBitmapImage.EndInit();
                                img1.Source = myBitmapImage;
                                myBitmapImage = null;
                                btnPicSecond.Content = img1;
                                strPhoto[1] = tIPViewModel._FaceObj.Tmplate[i].Img;
                                fileName[1] = "1";
                                this.btnPicSecond.IsEnabled = true;
                            }
                        }
                    }
                    //第三张照片
                    if (tIPViewModel._FaceObj.Tmplate.Count > 2 && tIPViewModel._FaceObj.Tmplate[2].Img.Length > 0)
                    {
                        for (int i = 0; i < tIPViewModel._FaceObj.Tmplate.Count; i++)//遍历
                        {
                            if (tIPViewModel._FaceObj.Tmplate[i].NIndex == 2)//查找nindex等于0的模板
                            {
                                System.Windows.Controls.Image img1 = new System.Windows.Controls.Image();
                                BitmapImage myBitmapImage = new BitmapImage();
                                myBitmapImage.BeginInit();
                                myBitmapImage.StreamSource = new System.IO.MemoryStream(tIPViewModel._FaceObj.Tmplate[i].Img);
                                myBitmapImage.EndInit();
                                img1.Source = myBitmapImage;
                                myBitmapImage = null;
                                btnPicThird.Content = img1;
                                strPhoto[2] = tIPViewModel._FaceObj.Tmplate[i].Img;
                                fileName[2] = "1";
                                this.btnPicThird.IsEnabled = true;
                            }
                        }
                    }
                    //第四张照片
                    if (tIPViewModel._FaceObj.Tmplate.Count > 3 && tIPViewModel._FaceObj.Tmplate[3].Img.Length > 0)
                    {
                        for (int i = 0; i < tIPViewModel._FaceObj.Tmplate.Count; i++)//遍历
                        {
                            if (tIPViewModel._FaceObj.Tmplate[i].NIndex == 3)//查找nindex等于0的模板
                            {
                                System.Windows.Controls.Image img1 = new System.Windows.Controls.Image();
                                BitmapImage myBitmapImage = new BitmapImage();
                                myBitmapImage.BeginInit();
                                myBitmapImage.StreamSource = new System.IO.MemoryStream(tIPViewModel._FaceObj.Tmplate[i].Img);
                                myBitmapImage.EndInit();
                                img1.Source = myBitmapImage;
                                myBitmapImage = null;
                                btnPicFourth.Content = img1;
                                strPhoto[3] = tIPViewModel._FaceObj.Tmplate[i].Img;
                                fileName[3] = "1";
                                this.btnPicFourth.IsEnabled = true;
                            }
                        }
                    }
                    //第五张照片
                    if (tIPViewModel._FaceObj.Tmplate.Count > 4 && tIPViewModel._FaceObj.Tmplate[4].Img.Length > 0)
                    {
                        for (int i = 0; i < tIPViewModel._FaceObj.Tmplate.Count; i++)//遍历
                        {
                            if (tIPViewModel._FaceObj.Tmplate[i].NIndex == 4)//查找nindex等于0的模板
                            {
                                System.Windows.Controls.Image img1 = new System.Windows.Controls.Image();
                                BitmapImage myBitmapImage = new BitmapImage();
                                myBitmapImage.BeginInit();
                                myBitmapImage.StreamSource = new System.IO.MemoryStream(tIPViewModel._FaceObj.Tmplate[i].Img);
                                myBitmapImage.EndInit();
                                img1.Source = myBitmapImage;
                                myBitmapImage = null;
                                btnPicFifth.Content = img1;
                                strPhoto[4] = tIPViewModel._FaceObj.Tmplate[i].Img;
                                fileName[4] = "1";
                                this.btnPicFifth.IsEnabled = true;
                            }
                        }
                    }
                }
                else
                {
                    //图片1
                    if (strPhoto[0] != null)
                    {
                        System.Windows.Controls.Image img1 = new System.Windows.Controls.Image();
                        BitmapImage myBitmapImage = new BitmapImage();
                        myBitmapImage.BeginInit();
                        myBitmapImage.StreamSource = new System.IO.MemoryStream(strPhoto[0]);
                        myBitmapImage.EndInit();
                        img1.Source = myBitmapImage;
                        myBitmapImage = null;
                        this.btnPicFirst.Content = img1;
                        this.btnPicFirst.IsEnabled = true;
                    }
                    if (strPhoto[1] == null && fileName[1] != "空")
                    {
                        byte[] Photo = System.IO.File.ReadAllBytes(fileName[1]);
                        strPhoto[1] = Photo;

                        ////图片2
                        System.Windows.Controls.Image img2 = new System.Windows.Controls.Image();
                        BitmapImage myBitmapImage = new BitmapImage();
                        myBitmapImage.BeginInit();
                        myBitmapImage.StreamSource = new System.IO.MemoryStream(strPhoto[1]);
                        myBitmapImage.EndInit();
                        img2.Source = myBitmapImage;
                        myBitmapImage = null;
                        this.btnPicSecond.Content = img2;
                        this.btnPicSecond.IsEnabled = true;
                    }

                    //图片3
                    if (strPhoto[2] == null && fileName[2] != "空")
                    {
                        byte[] Photo = System.IO.File.ReadAllBytes(fileName[2]);
                        strPhoto[2] = Photo;

                        System.Windows.Controls.Image img3 = new System.Windows.Controls.Image();
                        BitmapImage myBitmapImage = new BitmapImage();
                        myBitmapImage.BeginInit();
                        myBitmapImage.StreamSource = new System.IO.MemoryStream(strPhoto[2]);
                        myBitmapImage.EndInit();
                        img3.Source = myBitmapImage;
                        myBitmapImage = null;
                        this.btnPicThird.Content = img3;
                        this.btnPicThird.IsEnabled = true;
                    }
                    //图片4
                    if (strPhoto[3] == null && fileName[3] != "空")
                    {
                        byte[] Photo = System.IO.File.ReadAllBytes(fileName[3]);
                        strPhoto[3] = Photo;

                        System.Windows.Controls.Image img4 = new System.Windows.Controls.Image();
                        BitmapImage myBitmapImage = new BitmapImage();
                        myBitmapImage.BeginInit();
                        myBitmapImage.StreamSource = new System.IO.MemoryStream(strPhoto[3]);
                        myBitmapImage.EndInit();
                        img4.Source = myBitmapImage;
                        myBitmapImage = null;
                        this.btnPicFourth.Content = img4;
                        this.btnPicFourth.IsEnabled = true;
                    }

                    //图片5
                    if (strPhoto[4] == null && fileName[4] != "空")
                    {
                        byte[] Photo = System.IO.File.ReadAllBytes(fileName[4]);
                        strPhoto[4] = Photo;

                        System.Windows.Controls.Image img5 = new System.Windows.Controls.Image();
                        BitmapImage myBitmapImage = new BitmapImage();
                        myBitmapImage.BeginInit();
                        myBitmapImage.StreamSource = new System.IO.MemoryStream(strPhoto[4]);
                        myBitmapImage.EndInit();
                        img5.Source = myBitmapImage;
                        myBitmapImage = null;
                        this.btnPicFifth.Content = img5;
                        this.btnPicFifth.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("AddTemplate:Window_Loaded_1", ex);
                return;
            }
        }


        /// <summary>
        /// 点击下面5张照片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                strBtnName = button.Name;
                this.btnAddMainPhoto.IsEnabled = true;//激活添加主照片按钮
                //获得照片的路径
                switch (strBtnName)
                {
                    case "btnPicFirst":
                        _strPhotoMain = strPhoto[0];
                        break;
                    case "btnPicSecond":
                        _strPhotoMain = strPhoto[1];
                        break;
                    case "btnPicThird":
                        _strPhotoMain = strPhoto[2];
                        break;
                    case "btnPicFourth":
                        _strPhotoMain = strPhoto[3];
                        break;
                    case "btnPicFifth":
                        _strPhotoMain = strPhoto[4];
                        break;
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("AddTemplate：btnAddMainPhoto_Click", ex);
                return;
            }
        }


        /// <summary>
        /// 设置初始值
        /// </summary>
        /// <param name="myFaceObj"></param>
        /// <param name="i">1:添加模版 2:修改模版 3:抓拍修改模版</param>
        /// <param name="image">从抓拍处添加模版,传入的图片</param>
        public void SetTempleteInfo(MyFaceObj myFaceObj,int i, byte[] image)
        {
            if (i==2)
            {
                tIPViewModel.Id = myFaceObj.fa_ob_tcUuid + "";
                tIPViewModel.ImportTime = myFaceObj.fa_ob_dTm;
                tIPViewModel.Remark = myFaceObj.fa_ob_tcRemarks;
                tIPViewModel.Age = myFaceObj.nAge + "";
                tIPViewModel.Name = myFaceObj.tcName;
                tIPViewModel.SelectedSex = tIPViewModel.Sex.IndexOf(myFaceObj.nSex);
                tIPViewModel.SelectedType = tIPViewModel.Type.IndexOf(myFaceObj.nType);
                tIPViewModel._FaceObj = myFaceObj.faceObj;
                tIPViewModel.Title = @"..\..\Images\子菜单修改模板.png";
            }
            else
            {
                string StringName = System.Guid.NewGuid().ToString();
                StringName = StringName.Replace("-", "");
                tIPViewModel.Id = StringName;
                tIPViewModel.ImportTime = DateTime.Now.ToString();
                if(i == 3)
                {
                    tIPViewModel._FaceObj.Tmplate = new List<FaceObjTemplate>();
                    FaceObjTemplate faceObjectTemplate = new FaceObjTemplate();
                    faceObjectTemplate.Img = image;
                    tIPViewModel._FaceObj.Tmplate.Add(faceObjectTemplate);
                }
                tIPViewModel.Title = @"..\..\Images\子菜单添加模板.png";
            }
        }

        /// <summary>
        /// 右上角关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 浏览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                openFileDialog.Title = "选择文件";
                openFileDialog.Filter = "jpg|*.jpg|bmp|*.bmp";
                openFileDialog.FileName = string.Empty;
                openFileDialog.Multiselect = true;
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                System.Windows.Forms.DialogResult _DialogResult = openFileDialog.ShowDialog();
                if (_DialogResult == System.Windows.Forms.DialogResult.OK)
                {

                    fileNameChoice = openFileDialog.FileNames;

                    int count = 0;
                    for (int i = 0; i < fileName.Length; i++)
                    {
                        if (fileName[i] != "空")
                        {
                            count++;
                        }
                    }
                    if (count + fileNameChoice.Length > 5)//选择的文件个数多于5个
                    {
                        //MyMessage.showYes("总共最多添加5张照片!");
                        MyMessage.showYes("总共最多添加5张照片!");
                        fileNameChoice = null;
                    }
                    else
                    {
                        //将选择的照片加入要展示的列表
                        for (int i = 0; i < fileNameChoice.Length; i++)
                        {
                            for (int j = 0; j < fileName.Length; j++)
                            {
                                string str = fileName[j];
                                if (str == "空")
                                {
                                    fileName[j] = fileNameChoice[i];
                                    break;
                                }
                            }
                        }
                        ShowTemplatePic();
                    }
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("AddTemplate：btnBrowse_Click", ex);
                return;
            }
        }

        /// <summary>
        /// 显示5张模板图片
        /// </summary>
        private void ShowTemplatePic()
        {
            try
            {
                //图片1
                if (strPhoto[0] == null && fileName[0] != "空")
                {
                    byte[] Photo = System.IO.File.ReadAllBytes(fileName[0]);
                    strPhoto[0] = Photo;

                    System.Windows.Controls.Image img1 = new System.Windows.Controls.Image();
                    BitmapImage myBitmapImage = new BitmapImage();
                    myBitmapImage.BeginInit();
                    myBitmapImage.StreamSource = new System.IO.MemoryStream(strPhoto[0]);
                    myBitmapImage.EndInit();
                    img1.Source = myBitmapImage;
                    myBitmapImage = null;
                    this.btnPicFirst.Content = img1;
                    this.btnPicFirst.IsEnabled = true;
                }
                if (strPhoto[1] == null && fileName[1] != "空")
                {
                    byte[] Photo = System.IO.File.ReadAllBytes(fileName[1]);
                    strPhoto[1] = Photo;

                    ////图片2
                    System.Windows.Controls.Image img2 = new System.Windows.Controls.Image();
                    BitmapImage myBitmapImage = new BitmapImage();
                    myBitmapImage.BeginInit();
                    myBitmapImage.StreamSource = new System.IO.MemoryStream(strPhoto[1]);
                    myBitmapImage.EndInit();
                    img2.Source = myBitmapImage;
                    myBitmapImage = null;
                    this.btnPicSecond.Content = img2;
                    this.btnPicSecond.IsEnabled = true;
                }

                //图片3
                if (strPhoto[2] == null && fileName[2] != "空")
                {
                    byte[] Photo = System.IO.File.ReadAllBytes(fileName[2]);
                    strPhoto[2] = Photo;

                    System.Windows.Controls.Image img3 = new System.Windows.Controls.Image();
                    BitmapImage myBitmapImage = new BitmapImage();
                    myBitmapImage.BeginInit();
                    myBitmapImage.StreamSource = new System.IO.MemoryStream(strPhoto[2]);
                    myBitmapImage.EndInit();
                    img3.Source = myBitmapImage;
                    myBitmapImage = null;
                    this.btnPicThird.Content = img3;
                    this.btnPicThird.IsEnabled = true;
                }
                //图片4
                if (strPhoto[3] == null && fileName[3] != "空")
                {
                    byte[] Photo = System.IO.File.ReadAllBytes(fileName[3]);
                    strPhoto[3] = Photo;

                    System.Windows.Controls.Image img4 = new System.Windows.Controls.Image();
                    BitmapImage myBitmapImage = new BitmapImage();
                    myBitmapImage.BeginInit();
                    myBitmapImage.StreamSource = new System.IO.MemoryStream(strPhoto[3]);
                    myBitmapImage.EndInit();
                    img4.Source = myBitmapImage;
                    myBitmapImage = null;
                    this.btnPicFourth.Content = img4;
                    this.btnPicFourth.IsEnabled = true;
                }

                //图片5
                if (strPhoto[4] == null && fileName[4] != "空")
                {
                    byte[] Photo = System.IO.File.ReadAllBytes(fileName[4]);
                    strPhoto[4] = Photo;

                    System.Windows.Controls.Image img5 = new System.Windows.Controls.Image();
                    BitmapImage myBitmapImage = new BitmapImage();
                    myBitmapImage.BeginInit();
                    myBitmapImage.StreamSource = new System.IO.MemoryStream(strPhoto[4]);
                    myBitmapImage.EndInit();
                    img5.Source = myBitmapImage;
                    myBitmapImage = null;
                    this.btnPicFifth.Content = img5;
                    this.btnPicFifth.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("AddTemplate：btnBrowse_Click", ex);
                return;
            }
        }

        /// <summary>
        /// 将5张模板图片移除按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (strBtnName)
                {
                    case "btnPicFirst":
                        //将第一张图片删除
                        btnPicFirst.Content = "";
                        //btnPicFirst.IsEnabled = false;
                        if (_strPhotoMain != null)
                        {
                            if (_strPhotoMain.Equals(strPhoto[0]))
                            {
                                tIPViewModel._FaceObj.NMain_ftID = 0;//主照片ID
                                btnCancelMainPhoto_Click(null,null);
                            }
                        }
                        fileName[0] = "空";
                        strPhoto[0] = null;
                        break;
                    case "btnPicSecond":
                        btnPicSecond.Content = "";
                        //btnPicSecond.IsEnabled = false;
                        if (_strPhotoMain != null)
                        {
                            if (_strPhotoMain.Equals(strPhoto[1]))
                            {
                                tIPViewModel._FaceObj.NMain_ftID = 0;//主照片ID
                                btnCancelMainPhoto_Click(null, null);
                            }
                        }
                        fileName[1] = "空";
                        strPhoto[1] = null;
                        break;
                    case "btnPicThird":
                        btnPicThird.Content = "";
                        //btnPicThird.IsEnabled = false;
                        if (_strPhotoMain != null)
                        {
                            if (_strPhotoMain.Equals(strPhoto[2]))
                            {
                                tIPViewModel._FaceObj.NMain_ftID = 0;//主照片ID
                                btnCancelMainPhoto_Click(null, null);
                            }
                        }
                        fileName[2] = "空";
                        strPhoto[2] = null;
                        break;
                    case "btnPicFourth":
                        btnPicFourth.Content = "";
                        //btnPicFourth.IsEnabled = false;
                        if (_strPhotoMain != null)
                        {
                            if (_strPhotoMain.Equals(strPhoto[3]))
                            {
                                tIPViewModel._FaceObj.NMain_ftID = 0;//主照片ID
                                btnCancelMainPhoto_Click(null, null);
                            }
                        }
                        fileName[3] = "空";
                        strPhoto[3] = null;
                        break;
                    case "btnPicFifth":
                        btnPicFifth.Content = "";
                        //btnPicFifth.IsEnabled = false;
                        if (_strPhotoMain != null)
                        {
                            if (_strPhotoMain.Equals(strPhoto[4]))
                            {
                                tIPViewModel._FaceObj.NMain_ftID = 0;//主照片ID
                                btnCancelMainPhoto_Click(null, null);
                            }
                        }
                        fileName[4] = "空";
                        strPhoto[4] = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("AddTemplate：btnRemove_Click", ex);
                return;
            }
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetermineAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            List<ErrorInfo> ListErrorInfo ;
            try
            {
                #region 添加输入项
                if (btnUuid.Content.ToString() != string.Empty)
                {
                    tIPViewModel._FaceObj.TcUuid = btnUuid.Content.ToString();
                }
                else
                {
                    //MyMessage.showYes("人员ID是必输项！");
                    MyMessage.showYes("人员ID是必输项！");
                    return;
                }
                if (txtName.Text.Trim() != string.Empty)
                {
                    tIPViewModel._FaceObj.TcName = txtName.Text.Trim();
                }
                else
                {
                    //MyMessage.showYes("人员名称是必输项！");
                    MyMessage.showYes("人员名称是必输项！");
                    return;
                }
                if (txtAge.Text.Trim() != string.Empty)
                {
                    string message = _validationRule.intValidationRule(this.txtAge.Text.Trim());
                    if (message != "")
                    {
                        //MyMessage.showYes(message);
                        MyMessage.showYes(message);
                        return;
                    }
                    tIPViewModel._FaceObj.NAge = int.Parse(txtAge.Text.Trim());
                }
                else
                {
                    //MyMessage.showYes("人员年龄是必输项！");
                    MyMessage.showYes("人员年龄是必输项！");
                    return;
                }
                if (combSex.SelectedIndex != 0)
                {
                    tIPViewModel._FaceObj.NSex = combSex.SelectedIndex;
                }
                else
                {
                    //MyMessage.showYes("人员性别是必输项！");
                    MyMessage.showYes("人员性别是必输项！");
                    return;
                }

                //暂时没确定
                if (combType.SelectedIndex != 0)
                {
                    foreach (var basicinfo in BasicInfo.DefFaceObjType)
                    {
                        if (basicinfo.Description == tIPViewModel.Type[combType.SelectedIndex])
                        {
                            tIPViewModel._FaceObj.NType = basicinfo.Type;	// 类型  
                        }
                    }
                }
                else
                {
                    //MyMessage.showYes("人员类型是必输项！");
                    MyMessage.showYes("人员类型是必输项！");
                    return;
                }

                tIPViewModel._FaceObj.TcRemarks = txtRemarks.Text.Trim();
                #endregion
                tIPViewModel._FaceObj.Tmplate = new List<FaceObjTemplate>();
                //遍历照片存放数组，必须添加至少一张照片
                bool bIsPic = false;
                for (int i = 0; i < 5; i++)
                {
                    if (strPhoto[i] != null)
                    {
                        bIsPic = true;
                        break;
                    }
                }
                if (!bIsPic)
                {
                    //MyMessage.showYes("至少要一张照片才能注册成功");
                    MyMessage.showYes("至少要一张照片才能注册成功！");
                    return;
                }
                int nj = -1;
                for (int i = 0; i < 5; i++)
                {
                    if (strPhoto[i] != null)
                    {
                        FaceObjTemplate _FaceObjTemplate = new FaceObjTemplate();
                        string StringName = System.Guid.NewGuid().ToString();
                        StringName = StringName.Replace("-", "");
                        _FaceObjTemplate.TcUuid = StringName;
                        _FaceObjTemplate.TcObjid = tIPViewModel._FaceObj.TcUuid;

                        //判断前面是不是有空位置，有那么排到前面去
                        for (int j = 0; j < i; j++)
                        {
                            if (strPhoto[j] == null)
                            {
                                _FaceObjTemplate.NIndex = j;
                                nj = j;
                                break;
                            }
                        }
                        if (_FaceObjTemplate.NIndex == 0)
                        {
                            if (nj == -1)
                            {
                                _FaceObjTemplate.NIndex = i;
                            }
                            else
                            {
                                _FaceObjTemplate.NIndex = nj;
                            }
                        }
                        //时间 
                        string strNow = DateTime.Now.ToString();
                        DateTime dt1 = Convert.ToDateTime(strNow);
                        TimeSpan dt1Span = new TimeSpan(dt1.Ticks);

                        DateTime dt2 = new DateTime(1970, 1, 1);
                        TimeSpan dt2Span = new TimeSpan(dt2.Ticks);

                        long longAddTemplateTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                        _FaceObjTemplate.DTm = longAddTemplateTime;//模板时间
                        if (tIPViewModel._FaceObj.DTm == 0)
                        {
                            tIPViewModel._FaceObj.DTm = longAddTemplateTime;
                        }

                        if (_strPhotoMain != null)
                        {
                            if (_strPhotoMain.Equals(strPhoto[i]))
                            {
                                tIPViewModel._FaceObj.NMain_ftID = i + 1;//主照片ID
                            }
                            else
                            {
                                if (i == tIPViewModel._FaceObj.NMain_ftID - 1)
                                {
                                    tIPViewModel._FaceObj.NMain_ftID = _FaceObjTemplate.NIndex + 1;//主照片ID
                                }
                            }
                        }
                        else
                        {
                            tIPViewModel._FaceObj.NMain_ftID = 0;
                        }
                        _FaceObjTemplate.Img = strPhoto[i];

                        tIPViewModel._FaceObj.Tmplate.Add(_FaceObjTemplate);

                        if (nj != -1)
                        {
                            strPhoto[nj] = strPhoto[i];
                            strPhoto[i] = null;
                            nj = -1;
                        }
                    }
                }
                //具体的操作(添加,修改)
                if(tIPViewModel.Title.Contains("添加"))
                {
                    ListErrorInfo = thirft.AddFaceObj(tIPViewModel._FaceObj);
                }
                else
                {
                    ListErrorInfo = thirft.ModifyFaceObj(tIPViewModel.Id,tIPViewModel._FaceObj);
                }
                //判断是否成功
                if (ListErrorInfo.Count == 0)//判断成功，提示成功
                {
                    MyMessage.showYes("成功！");
                    this.Close();
                }
                else//添加不成功，显示错误
                {
                    for (int j = 0; j < ListErrorInfo.Count; j++)
                    {
                        string strError = "添加模板ID：" + ListErrorInfo[j].ID + "失败,失败信息：" + ListErrorInfo[j].ErrCode;
                        MyMessage.showYes(strError);
                    }
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("AddTemplate：btnRemove_Click", ex);
                return;
            }
        }

        /// <summary>
        /// 点击添加图片为主图片按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddMainPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_strPhotoMain.Length > 0)
                {
                    System.Windows.Controls.Image img1 = new System.Windows.Controls.Image();
                    BitmapImage myBitmapImage = new BitmapImage();
                    myBitmapImage.BeginInit();
                    myBitmapImage.StreamSource = new System.IO.MemoryStream(_strPhotoMain);
                    myBitmapImage.EndInit();
                    img1.Source = myBitmapImage;
                    
                    this.btnPicMain.Content = img1;
                    this.btnPicMain.IsEnabled = true;
                }
                else
                {
                    //MyMessage.showYes("未选中将要添加为主照片的模板照片！");
                    MyMessage.showYes("未选中将要添加为主照片的模板照片!");
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("AddTemplate：btnAddMainPhoto_Click", ex);
                return;
            }
        }

        /// <summary>
        /// 取消主照片按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelMainPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region
                this.btnCancelMainPhoto.IsEnabled = true;
                this.btnPicMain.Content = "";
                _strPhotoMain = null;
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("btnCancelMainPhoto_Click", ex);
                //System.Windows.MessageBox.Show(ex.Message); 
            }
        }
    }
}
