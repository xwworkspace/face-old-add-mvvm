using FaceSysByMvvm.Services;
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

namespace FaceSysByMvvm.View.CompOfRecords
{
    /// <summary>
    /// CompInfo.xaml 的交互逻辑
    /// </summary>
    public partial class CompInfo : Window
    {
        ThirftService thirft = new ThirftService();
        public CompInfo()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += CompInfo_MouseLeftButtonDown;
        }

        private void CompInfo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        internal void SetCmpInfo(BitmapImage capTemp, BitmapImage cmpTemp, BitmapImage senceImg, int score, string name, string type, string time, string channelName)
        {
            image_capImage.Source = capTemp;
            image_cmpImage.Source = cmpTemp;
            image_SenceImg.Source = senceImg;
            label_Socre.Text = label_Socre.Text.ToString().Replace("Socre", score+"");
            label_TemplateName.Text = label_TemplateName.Text.ToString().Replace("TemplateName", name);
            label_TemplateType.Text = label_TemplateType.Text.ToString().Replace("TemplateType", type);
            label_CapTime.Text = label_CapTime.Text.ToString().Replace("CapTime", time);
            label_CapChannel.Text = label_CapChannel.Text.ToString().Replace("CapChannel", channelName);
        }

        public void SetIdentifyResults(IdentifyResults _IdentifyResults)
        {
            image_capImage.Source = _IdentifyResults.CapImg;
            image_cmpImage.Source = _IdentifyResults.RegImg;
            long _longtime = _IdentifyResults.Info.Time;
            DateTime s = new DateTime(1970, 1, 1);
            s = s.AddSeconds(_longtime);
            label_Socre.Text = label_Socre.Text.ToString().Replace("Socre", _IdentifyResults.Info.Score + "");
            label_TemplateName.Text = label_TemplateName.Text.ToString().Replace("TemplateName", _IdentifyResults.Info.Name);
            label_TemplateType.Text = label_TemplateType.Text.ToString().Replace("TemplateType", _IdentifyResults.TemplateType);
            label_CapTime.Text = s.ToString("yyyy/MM/dd HH:mm:ss");
            label_CapChannel.Text = label_CapChannel.Text.ToString().Replace("CapChannel", _IdentifyResults.ChannelName);
            List<byte[]> senceImg = thirft.QuerySenceImg(_IdentifyResults.ID, s.ToString("yyyy/MM/dd HH:mm:ss").Split(' ')[0].Replace("/", "").Replace("/", ""));
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
    }
}
