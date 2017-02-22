using model = FaceSysByMvvm.ZModel;
using System.Windows;

namespace FaceSysByMvvm.View.ChannelManage
{
    /// <summary>
    /// Interaction logic for WarningMessageWindow.xaml
    /// </summary>
    public partial class WarningMessageWindow : Window
    {
        public WarningMessageWindow()
        {
            InitializeComponent();
            this.DataContext = model.ViewDataModel.WarningData;
        }
    }
}
