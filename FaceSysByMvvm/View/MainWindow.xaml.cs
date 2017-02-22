using FaceSysByMvvm.Common;
using FaceSysByMvvm.Services;
using FaceSysByMvvm.View.CaptureRecordQuery;
using FaceSysByMvvm.View.ChannelManage;
using FaceSysByMvvm.View.CompOfRecords;
using FaceSysByMvvm.View.TemplateManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Thrift.Protocol;
using Thrift.Transport;

namespace FaceSysByMvvm.ResourcesDictionary
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        ChannelManage _CM = new ChannelManage();
        CompOfRecords _COR = new CompOfRecords();
        CaptureRecordQuery _CRQ = new CaptureRecordQuery();
        TemplateManager _TM = new TemplateManager();

        DispatcherTimer timer = new DispatcherTimer();//心跳
        ThirftService thirft = new ThirftService();
        bool isMaxSize = false;
        Rect rcnormal;
        public MainWindow()
        {
            InitializeComponent();
            
            //默认显示功能
            Init();
            //设置标题
            string title = ConfigurationManager.AppSettings["程序标题"];
            List<char> listString = title.ToList<char>();
            StringBuilder spaceStr = new StringBuilder();
            foreach (char c in listString)
            {
                spaceStr.Append(c.ToString());
                spaceStr.Append(" ");
            }
            TxtTitle.Text = spaceStr.ToString();
            //心跳
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromSeconds(1);   //设置刷新的间隔时间
            timer.Start();
            //最大化带任务栏
            MaxSizeWindow();
            //允许拖动
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            //关闭窗体
            this.Closed += MainWindow_Closed;
        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// 窗体拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(isMaxSize)
            {
                return;
            }
            this.DragMove();
        }

        /// <summary>
        /// 初始化显示模块
        /// </summary>
        public void Init()
        {
            FuncationArea.Content = _CM;
        }

        public void ClearMainWindow()
        {
            ChannelManageToggleButton.IsEnabled = true;
            CompOfRecordsToggleButton.IsEnabled = true;
            CaptureRecordQueryToggleButton.IsEnabled = true;
            TemplateManagerToggleButton.IsEnabled = true;
            ChannelManageToggleButton.IsChecked = false;
            CompOfRecordsToggleButton.IsChecked = false;
            CaptureRecordQueryToggleButton.IsChecked = false;
            TemplateManagerToggleButton.IsChecked = false;
            ChannelManagePolyline.Visibility = Visibility.Hidden;
            CompOfRecordsPolyline.Visibility = Visibility.Hidden;
            CaptureRecordQueryPolyline.Visibility = Visibility.Hidden;
            TemplateManagerPolyline.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 切换功能区按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FuncationToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearMainWindow();
            ToggleButton tb = sender as ToggleButton;
            tb.IsEnabled = false;
            tb.IsChecked = true;
            switch (tb.Tag.ToString())
                {
                    case "0":
                    ChannelManagePolyline.Visibility = Visibility.Visible;
                    FuncationArea.Content = _CM;
                    break;
                    case "1":
                    CompOfRecordsPolyline.Visibility = Visibility.Visible;
                    _COR.RefreshChannelComboBox();
                    FuncationArea.Content = _COR;
                    break;
                    case "2":
                    CaptureRecordQueryPolyline.Visibility = Visibility.Visible;
                    _CRQ.RefreshChannelComboBox();
                    FuncationArea.Content = _CRQ;
                    break;
                    case "3":
                    TemplateManagerPolyline.Visibility = Visibility.Visible;
                    _TM.QueryTemplateCmpDelegate = TMQueryTemplateCmp;
                    FuncationArea.Content = _TM;
                    break;
                }           
        }

        /// <summary>
        /// 根据模版返回比对记录
        /// </summary>
        /// <param name="obj"></param>
        private void TMQueryTemplateCmp(string Name)
        {
            FuncationArea.Content = _COR;
            _COR.cORViewModel.StartDay = "";
            _COR.cORViewModel.Name = Name;
            MouseButtonEventArgs args = new MouseButtonEventArgs(Mouse.PrimaryDevice,0, MouseButton.Left);
            args.RoutedEvent = Button.ClickEvent;
            _COR.btnCompOfRecordsQuery.RaiseEvent(args);
        }


        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            thirft.HearBeat();
        }

        /// <summary>
        /// 最大化
        /// </summary>
        private void MaxSizeWindow()
        {
            this.Left = -8;//设置位置
            this.Top = -8;
            Rect rc = SystemParameters.WorkArea;//获取工作区大小
            this.Width = rc.Width + 16;
            this.Height = rc.Height + 16;
            isMaxSize = true;
        }

        /// <summary>
        /// 右上角功能按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt.Name == "btnMiniSize")
            {
                this.WindowState = WindowState.Minimized;
            }
            if (bt.Name == "btnMaxSize")
            {
                rcnormal = new Rect(this.Left, this.Top, this.Width, this.Height);//保存下当前位置与大小
                MaxSizeWindow();
                this.btnMaxSize.Visibility = Visibility.Collapsed;
                this.btnNormSize.Visibility = Visibility.Visible;
                isMaxSize = true;
            }
            if (bt.Name == "btnNormSize")
            {
                if (rcnormal.Width == 0)
                {
                    this.Width = 1500;
                    this.Height = 800;
                }
                else
                {
                    this.Left = rcnormal.Left;
                    this.Top = rcnormal.Top;
                    this.Width = rcnormal.Width;
                    this.Height = rcnormal.Height;
                }
                this.btnMaxSize.Visibility = Visibility.Visible;
                this.btnNormSize.Visibility = Visibility.Collapsed;
                isMaxSize = false;
            }
            if (bt.Name == "btnCloiSize")
            {
                Environment.Exit(0);
            }
        }

        /// <summary>

        /// 双击标题栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(isMaxSize == false)
            {
                rcnormal = new Rect(this.Left, this.Top, this.Width, this.Height);//保存下当前位置与大小
                MaxSizeWindow();
                this.btnMaxSize.Visibility = Visibility.Collapsed;
                this.btnNormSize.Visibility = Visibility.Visible;
                isMaxSize = true;
            }
            else
            {
                if (rcnormal.Width == 0)
                {
                    this.Width = 1500;
                    this.Height = 800;
                }
                else
                {
                    this.Left = rcnormal.Left;
                    this.Top = rcnormal.Top;
                    this.Width = rcnormal.Width;
                    this.Height = rcnormal.Height;
                }
                this.btnMaxSize.Visibility = Visibility.Visible;
                this.btnNormSize.Visibility = Visibility.Collapsed;
                isMaxSize = false;
            }
        }
    }
}
