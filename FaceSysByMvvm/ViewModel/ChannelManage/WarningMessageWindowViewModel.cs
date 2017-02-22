using FaceSysByMvvm.Model;
using Prism.Mvvm;

namespace FaceSysByMvvm.ViewModel.ChannelManage
{
    public class WarningMessageWindowViewModel : BindableBase
    {
        private WarningMessageCmd _cmd = new WarningMessageCmd();
        private WarningMessageModel _property = new WarningMessageModel();
        public WarningMessageCmd Cmd
        {
            get { return _cmd; }
            private set { SetProperty(ref _cmd, value); }
        }

        public WarningMessageModel Property
        {
            get { return _property; }
            private set { SetProperty(ref _property, value); }
        }

        public WarningMessageWindowViewModel()
        {
            WarningMessageCmd.InitCmd()();
            Property.MyProperty = "MyTitle";

            Property.CompareLogDatas = new System.Collections.Generic.List<object>();
            Property.CompareLogData = new object();
            Property.CompareLogData = "1234567890";
            Property.CompareLogDatas.Add(Property.CompareLogData);
        }
    }
}
