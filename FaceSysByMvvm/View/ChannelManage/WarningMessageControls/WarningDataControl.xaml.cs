using FaceSysByMvvm.Model;
using FaceSysByMvvm.ZModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace FaceSysByMvvm.View.ChannelManage.WarningMessageControls
{
    /// <summary>
    /// Interaction logic for WarningDataControl.xaml
    /// </summary>
    public partial class WarningDataControl : UserControl
    {
        public WarningDataControl()
        {
            InitializeComponent();
        }

        private void cameraList_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListView item = sender as ListView;

            ViewDataModel.WarningData.Property.CurCompareLogDatas = item.SelectedItems;

        }
    }
}
