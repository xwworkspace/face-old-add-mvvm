using DZVideoWpf;
using FaceSysByMvvm.Common;
using FaceSysByMvvm.ResourcesDictionary;
using FaceSysByMvvm.Services;
using FaceSysByMvvm.View.CompOfRecords;
using FaceSysByMvvm.View.TemplateManager;
using FaceSysByMvvm.ViewModel;
using FaceSysClient.ClassPool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using Thrift.Server;
using Thrift.Transport;

namespace FaceSysByMvvm.View.ChannelManage
{
    /// <summary>
    /// ChannelManage.xaml 的交互逻辑
    /// </summary>
    public partial class ChannelManage : UserControl
    {
        ThirftService thirft = new ThirftService();
        WriteLog _WriteLog = new WriteLog();
        UIServerInter _UIServerInter = new UIServerInter();
        public TThreadPoolServer server;

        public static ManualResetEvent ResetServerRealtimeCapInfo;          //监听服务器实时上传的抓拍信息
        public static ManualResetEvent ResetServerRealtimeCmpInfo;          //监听服务器实时上传的比对信息 
        public static MyCapFaceLogWithImg _MyCapFaceLogWithImg = new MyCapFaceLogWithImg();//保存服务器上传的抓拍信息
        public static List<MyCapFaceLogWithImg> _ListMyCapFaceLogWithImg = new List<MyCapFaceLogWithImg>();
        public static byte[] CapimageByteRealtimeCapInfo;                   //传递抓拍实时图片
        public static IdentifyResults _IdentifyResults = new IdentifyResults();//保存服务器上传的识别信息
        public static ObservableCollection<IdentifyResults> _ListIdentifyResults = new ObservableCollection<IdentifyResults>();
        public static byte[] CapimageByteRealtimeCmpInfo;                   //传递实时识别抓拍图片
        public static byte[] CmpimageByteRealtimeCmpInfo;                   //传递实时识别注册图片
        ChannelManageViewModel _ChannelManageViewModel;
        List<WindowsFormsHost> wFHList = new List<WindowsFormsHost>();
        int currentVideo = 0;                                               //当前正在播放的视频数量
        int currentMaxvideo = 0;                                            //当前分屏允许的最大分屏数量

        Dictionary<string, long> cmpInfoDictionary = new Dictionary<string, long>();
        public ChannelManage()
        {
            InitializeComponent();
            _ChannelManageViewModel = new ChannelManageViewModel();
            this.DataContext = _ChannelManageViewModel;
            listViewCaptureResults.ItemsSource = _ListMyCapFaceLogWithImg;
            listViewContIdentifyResults.ItemsSource = _ListIdentifyResults;
            ResetServerRealtimeCapInfo = new ManualResetEvent(false);
            ResetServerRealtimeCmpInfo = new ManualResetEvent(false);
            //初始化OCX控件载体
            for (int i = 0;i< 16;i++)
            {
                WindowsFormsHost wfh = new WindowsFormsHost();
                wfh.Tag = null;
                wFHList.Add(wfh);
            }
            //初始化默认分屏
            SetVideoGridScreen(1);
            //打开客户端服务，接收业务服务器上传的实时抓拍和实时识别结果
            Thread ThreadStarServer = new System.Threading.Thread(new ParameterizedThreadStart(StartServer));
            ThreadStarServer.SetApartmentState(ApartmentState.STA);
            ThreadStarServer.Start();
            //域值设置
            int threshold = thirft.QueryThreshold();
            if (threshold == -1)
            {
                _ChannelManageViewModel.SelectedThreshold = Convert.ToInt32(ConfigurationManager.AppSettings["阈值"]) - 1;
            }
            else
            {
                if (Login.ClientType != "1")
                {
                    _ChannelManageViewModel.SelectedThreshold = threshold - 1;
                }
                else
                {
                    double trueThreshold = Math.Sqrt(threshold) * 10;
                    trueThreshold = Math.Round(trueThreshold, 0, MidpointRounding.AwayFromZero);
                    _ChannelManageViewModel.SelectedThreshold = Convert.ToInt32(trueThreshold) -1;
                }
            }
            //根据客户端类型来修改客户端
            if (Login.ClientType == "1")
            {
                btnAddPassageWay.Visibility = Visibility.Hidden;
                btnModifyPassageWay.Visibility = Visibility.Hidden;
                btnDeletePassageWay.Visibility = Visibility.Hidden;
                comboAutoSend.Visibility = Visibility.Hidden;
                comboThreshold.IsEnabled = false;
                _ChannelManageViewModel.RefreshChannelList();
            }
            if (Login.ClientType == "2")
            {
                comboAutoSend.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 切换抓拍和比对
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            
            SnapshotToggleButton.IsEnabled = true;
            DistinguishToggleButton.IsEnabled = true;
            SnapshotToggleButton.IsChecked = false;
            DistinguishToggleButton.IsChecked = false;
            ToggleButton tb = sender as ToggleButton;
            tb.IsEnabled = false;
            tb.IsChecked = true;
            if(tb.Name == "SnapshotToggleButton")
            {
                Snapshot.Visibility = Visibility.Visible;
                Distinguish.Visibility = Visibility.Collapsed;
            }
            else
            {
                Snapshot.Visibility = Visibility.Collapsed;
                Distinguish.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 双击通道打开视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelManageListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ChannelListItemViewModel lbi = (sender as ListBox).SelectedItem as ChannelListItemViewModel;
                if (lbi == null)
                {
                    return;
                }
                string ip = lbi.MyChannelCfg.CaptureCfg.TcAddr;
                if (ip == string.Empty)
                {
                    System.Windows.MessageBox.Show("摄像机IP为空！");
                    return;
                }
                if (true)
                {
                    if (lbi.IsOpened == false)
                    {
                        lbi.IsOpened = true;
                        currentVideo++;
                        UserControl1 usercontrol = new UserControl1();
                        usercontrol.opencamera(lbi.MyChannelCfg.CaptureCfg.NCaptureType, lbi.MyChannelCfg.CaptureCfg.TcAddr +"|" + lbi.MyChannelCfg.TcDescription, (uint)lbi.MyChannelCfg.CaptureCfg.NPort, lbi.MyChannelCfg.CaptureCfg.TcUID, lbi.MyChannelCfg.CaptureCfg.TcPSW, 1, 1);
                        foreach (WindowsFormsHost wfh in wFHList)
                        {
                            if (wfh.Tag == null)
                            {
                                wfh.Child = usercontrol;
                                wfh.Tag = lbi.MyChannelCfg.TcChaneelID;
                                break;
                            }
                        }
                        if (currentVideo > currentMaxvideo)
                        {
                            SetVideoGridScreen(currentVideo);
                        }
                        this.UpdateLayout();
                    }
                    else
                    {
                        currentVideo--;
                        foreach (WindowsFormsHost wfh in wFHList)
                        {
                            if (wfh.Tag != null && wfh.Tag.ToString() == lbi.MyChannelCfg.TcChaneelID)
                            {
                                (wfh.Child as UserControl1).closecamera();
                                wfh.Child = null;
                                wfh.Tag = null;
                                break;
                            }
                        }
                        lbi.IsOpened = false;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("摄像机无法连接！");
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("ChannelManageListBox_MouseDoubleClick", ex);
                MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// 测试通道IP是否能连通
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool Ping(string ip)
        {
            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();
            options.DontFragment = true;
            string data = "Test Data!";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 1000; // Timeout 时间，单位：毫秒
            System.Net.NetworkInformation.PingReply reply = p.Send(ip, timeout, buffer, options);
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 开始抓拍
        /// </summary>
        /// <param name="ob"></param>
        public void StartServer(object ob)
        {
            try
            {
                //在新线程中创建监听，接收服务器实时抓拍数据
                Thread ThListenServerRealtimeCapInfo = new Thread(new ParameterizedThreadStart(ListenWaitServerRealtimeCapInfo));
                ThListenServerRealtimeCapInfo.SetApartmentState(ApartmentState.STA);
                ThListenServerRealtimeCapInfo.IsBackground = true;
                ThListenServerRealtimeCapInfo.Start();

                //在新线程中创建监听，接收服务器实时比对数据
                Thread ThListenServerRealtimeCmpInfo = new Thread(new ParameterizedThreadStart(ListenWaitServerRealtimeCmpInfo));
                ThListenServerRealtimeCmpInfo.SetApartmentState(ApartmentState.STA);
                ThListenServerRealtimeCmpInfo.IsBackground = true;
                ThListenServerRealtimeCmpInfo.Start();

                try//捕获异常
                {
                    // 1.创建一个XmlDocument类的对象
                    XmlDocument xmlDoc = new XmlDocument();
                    // 2.读取的xml文档加载进来
                    string strXmlPath = System.Environment.CurrentDirectory + @"\XMl\port.xml";
                    xmlDoc.Load(strXmlPath);

                    // 3.读取你指定的节点
                    XmlNodeList lis = xmlDoc.GetElementsByTagName("启动端口");
                    //获得节点下的所有子节点
                    XmlNodeList xndList = lis[0].ChildNodes;

                    //获得第一个子节点,得到抓拍ID
                    XmlNode xnd0 = xndList[0];
                    int port = 0;
                    port = int.Parse(xnd0.InnerText);
                    TServerSocket serverTransport = new TServerSocket(port, 0, false);
                    UIServer.Processor processor = new UIServer.Processor(_UIServerInter);
                    server = new TThreadPoolServer(processor, serverTransport);
                    server.Serve();
                }
                catch (Exception ex)
                {
                    MyMessage.showYes(ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("StartServer", ex);
                return;
            }

        }

        /// <summary>
        /// 抓拍监听
        /// </summary>
        /// <param name="obj"></param>
        public void ListenWaitServerRealtimeCapInfo(object obj)
        {
            try
            {
                while (true)
                {
                    if (ResetServerRealtimeCapInfo.WaitOne())//设置监听等待
                    {

                        //刷新抓拍列表 
                        listViewCaptureResults.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                if (CapimageByteRealtimeCapInfo.Length == 0)
                                {
                                    return;
                                }
                                if (CapimageByteRealtimeCapInfo.Length >= 0)
                                {
                                    //读入MemoryStream对象 
                                    BitmapImage myBitmapImage = new BitmapImage();
                                    myBitmapImage.BeginInit();
                                    myBitmapImage.StreamSource = new System.IO.MemoryStream(CapimageByteRealtimeCapInfo);
                                    myBitmapImage.EndInit();
                                    _MyCapFaceLogWithImg.img = myBitmapImage;
                                    _ListMyCapFaceLogWithImg.Insert(0, _MyCapFaceLogWithImg);
                                    _ChannelManageViewModel.CapImageCount++;
                                    if (_ListMyCapFaceLogWithImg.Count > 100)
                                    {
                                        _ListMyCapFaceLogWithImg.RemoveRange(9, 90);
                                    }
                                }
                                else
                                {
                                    string strMessage = "抓拍照片为空";
                                }
                                listViewCaptureResults.Items.Refresh();
                            }
                            catch (Exception ex)
                            {
                                _WriteLog.WriteToLog("listViewCaptureResultsDispatcher", ex);
                                return;
                            }
                        }));
                        ResetServerRealtimeCapInfo.Reset();
                    }
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("ListenWaitServerRealtimeCapInfo", ex);
            }
        }

        /// <summary>
        /// 比对监听
        /// </summary>
        /// <param name="obj"></param>
        public void ListenWaitServerRealtimeCmpInfo(object obj)
        {
            bool isCmpSend = false;
            try
            {
                while (true)
                {
                    if (ResetServerRealtimeCmpInfo.WaitOne())//设置监听等待
                    {
                        lock (ChannelManage._IdentifyResults)
                        {
                            IdentifyResults tempIdentifyResults = new IdentifyResults();
                            tempIdentifyResults = _IdentifyResults;
                            byte[] tempCapimageByteRealtimeCmpInfo = CapimageByteRealtimeCmpInfo;
                            byte[] tempCmpimageByteRealtimeCmpInfo = CmpimageByteRealtimeCmpInfo;
                            listViewContIdentifyResults.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                //刷新识别结果列表
                                try
                                {
                                    if (tempCapimageByteRealtimeCmpInfo.Length != 0 && tempCmpimageByteRealtimeCmpInfo.Length != 0)
                                    {
                                        BitmapImage myBitmapImage = new BitmapImage();
                                        myBitmapImage.BeginInit();
                                        myBitmapImage.StreamSource = new System.IO.MemoryStream(tempCapimageByteRealtimeCmpInfo);
                                        myBitmapImage.EndInit();
                                        tempIdentifyResults.CapImg = myBitmapImage;

                                        BitmapImage myBitmapImage2 = new BitmapImage();
                                        myBitmapImage2.BeginInit();
                                        myBitmapImage2.StreamSource = new System.IO.MemoryStream(tempCmpimageByteRealtimeCmpInfo);
                                        myBitmapImage2.EndInit();
                                        tempIdentifyResults.RegImg = myBitmapImage2;
                                        Console.WriteLine("注册名称:" + tempIdentifyResults.Info.Name);
                                        _ListIdentifyResults.Insert(0, tempIdentifyResults);
                                        if (tempIdentifyResults.ChannelName.StartsWith("##@"))
                                        {
                                            isCmpSend = true;
                                        }
                                        tempIdentifyResults.ChannelName = tempIdentifyResults.ChannelName.Replace("##", "").Replace("@", "");

                                        _ChannelManageViewModel.ComImageCount++;
                                        if (_ListIdentifyResults.Count > 100)
                                        {
                                            _ListIdentifyResults.Clear();
                                        }
                                        //弹窗部分
                                        if (Login.ClientType == "1" || Login.ClientType == "0")
                                        {
                                            if (comboAutoSend.SelectedIndex == 0)
                                            {
                                                //弹窗
                                                listViewContIdentifyResults.Dispatcher.BeginInvoke(new Action(() =>
                                                {
                                                    try
                                                    {
                                                        WarningMessage wm = new WarningMessage();
                                                        if (!isCmpSend)
                                                        {
                                                            if (cmpInfoDictionary.ContainsKey(tempIdentifyResults.Info.Name)) // True 
                                                            {
                                                                if (tempIdentifyResults.Info.Time - cmpInfoDictionary[tempIdentifyResults.Info.Name] <= 120)
                                                                {
                                                                    wm.SetWarningRed();
                                                                    cmpInfoDictionary[tempIdentifyResults.Info.Name] = tempIdentifyResults.Info.Time;
                                                                }
                                                                cmpInfoDictionary[tempIdentifyResults.Info.Name] = tempIdentifyResults.Info.Time;
                                                            }
                                                            else
                                                            {
                                                                cmpInfoDictionary.Add(tempIdentifyResults.Info.Name, tempIdentifyResults.Info.Time);
                                                            }
                                                        }
                                                        if (tempIdentifyResults.CapImg == null || tempIdentifyResults.RegImg == null)
                                                        {
                                                            tempIdentifyResults.CapImg = myBitmapImage;
                                                            tempIdentifyResults.RegImg = myBitmapImage2;
                                                        }
                                                        myBitmapImage = null;
                                                        myBitmapImage2 = null;
                                                        wm.SetWarningInfo(tempIdentifyResults.Info.Name, tempIdentifyResults.ChannelName, tempIdentifyResults.Info.Time, tempIdentifyResults.TemplateType, tempIdentifyResults.Info.Score + "", tempIdentifyResults.CapImg, tempIdentifyResults.RegImg, tempIdentifyResults.Info);
                                                        wm.Show();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        _WriteLog.WriteToLog("listViewContIdentifyResultsDispatcher", ex);
                                                    }
                                                }));
                                            }
                                            else
                                            {
                                                //自动推送
                                                foreach (var area in BasicInfo.ConfigList)
                                                {
                                                    if (tempIdentifyResults.ChannelName.Contains(area.AreaName))
                                                    {
                                                        if (area.Alarmnum != 1)
                                                        {
                                                            if (cmpInfoDictionary.ContainsKey(tempIdentifyResults.Info.Name)) // True 
                                                            {
                                                                if (tempIdentifyResults.Info.Time - cmpInfoDictionary[tempIdentifyResults.Info.Name] <= 120)
                                                                {
                                                                    tempIdentifyResults.Info.Channel = tempIdentifyResults.ChannelName;
                                                                    if (tempIdentifyResults.Info.Score > area.Threshold)
                                                                    {
                                                                        foreach (var ip in area.ReceiveIPOfArea)
                                                                        {
                                                                            if (UpdateCmp(tempIdentifyResults.Info, ip) == 0)
                                                                            {
                                                                                thirft.UpdateCmpLog(tempIdentifyResults.Info.CapID, tempIdentifyResults.Info.ObjID, System.DateTime.Now.ToString("yyyyMMdd"), area.AreaType);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                cmpInfoDictionary[tempIdentifyResults.Info.Name] = tempIdentifyResults.Info.Time;
                                                            }
                                                            else
                                                            {
                                                                cmpInfoDictionary.Add(tempIdentifyResults.Info.Name, tempIdentifyResults.Info.Time);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            tempIdentifyResults.Info.Channel = tempIdentifyResults.ChannelName;
                                                            if (tempIdentifyResults.Info.Score > area.Threshold)
                                                            {
                                                                foreach (var ip in area.ReceiveIPOfArea)
                                                                {
                                                                    if (UpdateCmp(tempIdentifyResults.Info, ip) == 0)
                                                                    {
                                                                        thirft.UpdateCmpLog(tempIdentifyResults.Info.CapID, tempIdentifyResults.Info.ObjID, System.DateTime.Now.ToString("yyyyMMdd"), area.AreaType);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }              
                                            }
                                        }
                                    }
                                    if (CapimageByteRealtimeCmpInfo.Length <= 0)
                                    {
                                        string strMessage = "抓拍照片为空";
                                        _WriteLog.WriteToLog("ListenWaitServerRealtimeCmpInfo", strMessage);
                                    }
                                    if (CmpimageByteRealtimeCmpInfo.Length <= 0)
                                    {
                                        string strMessage = "模板照片为空";
                                        _WriteLog.WriteToLog("ListenWaitServerRealtimeCmpInfo", strMessage);
                                    }
                                    Thread thread = new Thread(new ThreadStart(Play));
                                    thread.Start();
                                }
                                catch (Exception ex)
                                {
                                    _WriteLog.WriteToLog("ListenWaitServerRealtimeCmpInfo", ex);
                                }
                            }));
                            ResetServerRealtimeCmpInfo.Reset();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("ListenWaitServerRealtimeCmpInfo", ex);
                //System.Windows.MessageBox.Show(ex.Message); 
            }
        }

        /// <summary>
        /// 推送比对记录
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

        public SoundPlayer soundPlayer;
        /// <summary>
        /// 播放报警音频
        /// </summary>
        /// <param name="obj"></param>
        private void Play()
        {
            try
            {
                int i = 4;
                string path = System.Environment.CurrentDirectory + @"\Resources\Audio\ALARM" + i + ".wav";
                soundPlayer = new SoundPlayer(path);
                //或者
                //SoundPlayer soundPlayer = new SoundPlayer(@"Resources\Audio\didi.wav");

                soundPlayer.Play();
                //Thread.CurrentThread.Abort();
            }
            catch (Exception ex)
            {
                //_WriteLog.WriteToLog("Play", ex);
            }
        }

        /// <summary>
        /// 设置分屏,并添加子项
        /// </summary>
        /// <param name="i">几分屏</param>
        public void SetVideoGridScreen(int sceenCount)
        {
            currentMaxvideo = sceenCount;
            try
            {
                //设置分屏,添加行列
                int rowCount = 0;
                int ColCount = 0;
                foreach (Grid thing in VideoPartGrid.Children)
                {
                    thing.Children.Clear();
                } 
                VideoPartGrid.Children.Clear();
                VideoPartGrid.RowDefinitions.Clear();
                VideoPartGrid.ColumnDefinitions.Clear();
                switch (sceenCount)
                {
                    case 1:
                        rowCount = 1;
                        ColCount = 1;
                        break;
                    case 2:
                        rowCount = 2;
                        ColCount = 1;
                        break;
                    case 3:
                        rowCount = 2;
                        ColCount = 2;
                        break;
                    case 4:
                        rowCount = 2;
                        ColCount = 2;
                        break;
                    case 5:
                    case 6:
                        rowCount = 3;
                        ColCount = 2;
                        break;
                    case 7:
                    case 8:
                    case 9:
                        rowCount = 3;
                        ColCount = 3;
                        break;
                    case 10:
                    case 11:
                    case 12:
                        rowCount = 4;
                        ColCount = 3;
                        break;
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                        rowCount = 4;
                        ColCount = 4;
                        break;
                }
                for (int i = 1; i <= rowCount; i++)
                {
                    VideoPartGrid.RowDefinitions.Add(new RowDefinition());
                }
                for (int i = 1; i <= ColCount; i++)
                {
                    VideoPartGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }

                //添加子项
                for (int i = 0; i < sceenCount; i++)
                {
                    Grid screenGrid = new Grid();
                    screenGrid.Background = this.FindResource("ViedoBackground") as ImageBrush;
                    if(sceenCount == 3&&i==2)
                    {
                        screenGrid.SetValue(Grid.RowProperty, i / ColCount);
                        screenGrid.SetValue(Grid.ColumnProperty, i % ColCount);
                        screenGrid.SetValue(Grid.ColumnSpanProperty, 2);
                    }
                    else
                    {
                        screenGrid.SetValue(Grid.RowProperty, i / ColCount);
                        screenGrid.SetValue(Grid.ColumnProperty, i % ColCount);
                    }
                    screenGrid.Children.Add(wFHList[i]);
                    VideoPartGrid.Children.Add(screenGrid);
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("SetVideoGridScreen", ex);
            }
        }

        /// <summary>
        /// 设置分屏按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetScreen_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Button bt = sender as Button;
                string strBtnName = bt.Name;
                //判断按钮名称
                switch (strBtnName)
                {
                    case "btnOneScreen":
                        SetVideoGridScreen(1);
                        break;
                    case "btnTowScreen":
                        SetVideoGridScreen(2);
                        break;
                    case "btnThreeScreen":
                        SetVideoGridScreen(3);
                        break;
                    case "btnFourScreen":
                        SetVideoGridScreen(4);
                        break;
                    case "btnSixScreen":
                        SetVideoGridScreen(6);
                        break;
                    case "btnNineScreen":
                        SetVideoGridScreen(9);
                        break;
                    case "btnTwelveScreen":
                        SetVideoGridScreen(12);
                        break;
                    case "btnSixteenScreen":
                        SetVideoGridScreen(16);
                        break;
                }
                Pop.IsOpen = false;

            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("btnSetScreen_Click", ex);
            }
        }

        /// <summary>
        /// 增加频道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPassageWay_Click(object sender, RoutedEventArgs e)
        {
            ChannelInfo channelInfo = new ChannelInfo();
            channelInfo.RefreshChannelDelegate = _ChannelManageViewModel.RefreshChannelList;
            channelInfo.ShowDialog();
        }
        /// <summary>
        /// 修改频道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyPassageWay_Click(object sender, RoutedEventArgs e)
        {
            //todo(已经完成未测试) 先关闭视频 再进行修改
            if (ChannelManageListBox.SelectedIndex < 0)
            {
                MyMessage.showYes("请选中需要修改的通道!");
                return;
            }
            
            ChannelListItemViewModel lbi = ChannelManageListBox.SelectedItem as ChannelListItemViewModel;
            ChannelInfo channelInfo = new ChannelInfo();
            channelInfo.RefreshChannelDelegate = _ChannelManageViewModel.RefreshChannelList;
            channelInfo.CloseVideoDelegate = CloseVideo;
            channelInfo.SetChannelInfo(lbi.MyChannelCfg);
            channelInfo.ShowDialog();
        }

        /// <summary>
        /// listview宽度自动变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewContIdentifyResults_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            try
            {
                //获得识别结果listview
                GridView gv = listViewContIdentifyResults.View as GridView;
                if (gv != null)//listview存在
                {
                    if (listViewContIdentifyResults.ActualWidth <= 0)
                    {
                        return;
                    }
                    //获得第一列
                    GridViewColumn gvc = gv.Columns[0];
                    //设置第一列列宽
                    gvc.Width = (listViewContIdentifyResults.ActualWidth - 4) / 3;

                    //获得第二列
                    gvc = gv.Columns[1];
                    //设置第二列列宽
                    gvc.Width = (listViewContIdentifyResults.ActualWidth - 4) / 3;

                    //获得第三列
                    gvc = gv.Columns[2];
                    //设置第三列列宽
                    gvc.Width = (listViewContIdentifyResults.ActualWidth - 4) / 3;
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("listViewContIdentifyResults_SizeChanged_1", ex);
                return;
            }
        }

        /// <summary>
        /// 比对双击 显示比对详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewContIdentifyResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IdentifyResults _IdentifyResults = listViewContIdentifyResults.SelectedItem as IdentifyResults;
            if(_IdentifyResults == null)
            {
                return;
            }
            //比对窗口
            //ChannelCompare channelCompare = new ChannelCompare();
            //channelCompare.SetIdentifyResults(_IdentifyResults);
            //channelCompare.ShowDialog();
            //实时帧窗口
            CompInfo comInfoWin = new CompInfo();
            comInfoWin.SetIdentifyResults(_IdentifyResults);
            comInfoWin.ShowDialog();
        }

        /// <summary>
        /// 删除频道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeletePassageWay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int success = -1;
                if (ChannelManageListBox.SelectedIndex < 0)
                {
                    MyMessage.showYes("请选中需要删除的通道!");
                    return;
                }
                ChannelListItemViewModel lbi = ChannelManageListBox.SelectedItem as ChannelListItemViewModel;
                bool? result = MyMessage.Show("是否删除通道?");
                if (result == true)
                {
                    success = thirft.DelChannel(lbi.MyChannelCfg.TcChaneelID);
                    if (success == 0)
                    {
                        MyMessage.showYes("删除通道成功！");
                        CloseVideo();
                        _ChannelManageViewModel.RefreshChannelList();
                    }
                    else
                    {
                        MyMessage.showYes("删除通道失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessage.showYes("删除通道失败！");
                _WriteLog.WriteToLog("btnDeletePassageWay_Click", ex);
            }
            
            
        }

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        private void CloseVideo()
        {
            try
            {
                ChannelListItemViewModel lbi = ChannelManageListBox.SelectedItem as ChannelListItemViewModel;
                if (lbi.IsOpened == true)
                {
                    currentVideo--;
                    foreach (WindowsFormsHost wfh in wFHList)
                    {
                        if (wfh.Tag != null && wfh.Tag.ToString() == lbi.MyChannelCfg.TcChaneelID)
                        {
                            (wfh.Child as UserControl1).closecamera();
                            wfh.Child = null;
                            wfh.Tag = null;
                        }
                    }
                    lbi.IsOpened = false;
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("CloseVideo", ex);
            }
            

        }

        /// <summary>
        /// 双击抓拍照片 添加模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewCaptureResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(Login.ClientType == "1")
            {
                return;
            }
            MyCapFaceLogWithImg cmpFaceLogWidthImg = listViewCaptureResults.SelectedItem as MyCapFaceLogWithImg;
            if(cmpFaceLogWidthImg == null)
            {
                return;
            }
            List<byte[]> listImageBytes = new List<byte[]>();
            listImageBytes = thirft.QueryCapLogImageH(cmpFaceLogWidthImg.ID,cmpFaceLogWidthImg.time.Split(' ')[0].Replace(@"/", "").Replace(@"/", ""));
            TempleteInfoPop tIP = new TempleteInfoPop();
            if (listImageBytes.Count <= 0)
            {
                return;
            }
            else
            {
                tIP.SetTempleteInfo(null, 3, listImageBytes[0]);
            }
            
            tIP.ShowDialog();
        }

        /// <summary>
        /// 刷新通道列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshChannel_Click(object sender, RoutedEventArgs e)
        {
            _ChannelManageViewModel.RefreshChannelList();
        }

        /// <summary>
        /// 重新设置阈值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Login.ClientType == "1")
            {
                return;
            }
            ComboBox cb = sender as ComboBox;
            thirft.SetCMPthreshold(Convert.ToInt32(_ChannelManageViewModel.SelectedThreshold  + 1));
        }
    }
}
