using FaceSysByMvvm.Model;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace FaceSysByMvvm.ViewModel.ChannelManage
{
    public class WarningMessageWindowViewModel : BindableBase
    {
        private WarningMessageCmd _cmd = new WarningMessageCmd();
        private WarningMessageModel _property;
        WarningMessageWindowViewModel _wmv;
        public WarningMessageCmd Cmd
        {
            get { return _cmd; }
            set { SetProperty(ref _cmd, value); }
        }

        public WarningMessageModel Property
        {
            get { return _property; }
            set { SetProperty(ref _property, value); }
        }

        public WarningMessageWindowViewModel Wmv
        {
            get { return _wmv; }
            set
            {
                _wmv = value;
                SetProperty(ref _wmv, value);
            }
        }

        public WarningMessageWindowViewModel()
        {
            WarningMessageCmd.InitCmd()();
            Property = new WarningMessageModel();
            Property.CompareLogDatas = new ObservableCollection<MyCmpFaceLogWidthImgModel>();
        }

        public void RefreshProperty()
        {
            OnPropertyChanged("Property");
        }
    }
}
