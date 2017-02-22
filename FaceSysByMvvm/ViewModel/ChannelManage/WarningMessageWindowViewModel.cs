using FaceSysByMvvm.Model;
using Prism.Mvvm;

namespace FaceSysByMvvm.ViewModel.ChannelManage
{
    public class WarningMessageWindowViewModel : BindableBase
    {
        private WarningMessageCmd _cmd;
        public WarningMessageCmd Cmd
        {
            get { return _cmd; }
            private set { SetProperty(ref _cmd, value); }
        }


        public WarningMessageWindowViewModel()
        {
            Cmd = new WarningMessageCmd();
            WarningMessageCmd.InitCmd()();
        }
    }
}
