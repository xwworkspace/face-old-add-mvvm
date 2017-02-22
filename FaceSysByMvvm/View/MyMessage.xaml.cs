using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FaceSysByMvvm.View
{
    /// <summary>
    /// MyMessage.xaml 的交互逻辑
    /// </summary>
    public partial class MyMessage : Window
    {
        [DllImport("user32.dll ", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter,
        int X, int Y, int cx, int cy, int uFlags);

        public MyMessage()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            SetWindowPos(new WindowInteropHelper(this).Handle, -1, 0, 0, 0, 0, 0x4000 | 0x0001 | 0x0002);
        }
        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //public new string Title
        //{
        //    get { return this.lblTitle.Text; }
        //    set { this.lblTitle.Text = value; }
        //}

        public string Message
        {
            get { return this.txtInfo.Text.ToString(); }
            set { this.txtInfo.Text = value; }
        }

        //public string Message1
        //{
        //    get { return this.txtMsg1.Text; }
        //    set { this.txtMsg1.Text = value; }
        //}

        /// <summary>
        /// 静态方法 
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static bool? Show(string msg)
        {
            var msgBox = new MyMessage();
            msgBox.Message = msg;
            return msgBox.ShowDialog();
        }

        private void btnConfirm_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }


        public static void showYes(string msg)
        {
            var msgBox = new MyMessage();
            msgBox.Message = msg;
            msgBox.ColumCancel.Width = new GridLength(0);
            msgBox.Topmost = true;
            msgBox.ShowDialog();
        }
    }
}
