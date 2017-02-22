using FaceSysByMvvm.ViewModel.ChannelManage;

namespace FaceSysByMvvm.ZModel
{
    internal class ViewDataModel
    {
        static object _warningData = new WarningMessageWindowViewModel();
        public static object WarningData
        {
            get { return _warningData; }
            private set { _warningData = value; }
        }
    }
}
