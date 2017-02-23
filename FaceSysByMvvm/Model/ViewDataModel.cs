using FaceSysByMvvm.ViewModel.ChannelManage;

namespace FaceSysByMvvm.ZModel
{
    public class ViewDataModel
    {
        static WarningMessageWindowViewModel _warningData = new WarningMessageWindowViewModel();
        public static WarningMessageWindowViewModel WarningData
        {
            get { return _warningData; }
            set { _warningData = value; }
        }
    }
}
