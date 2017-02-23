using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;

namespace FaceSysByMvvm.Model
{
    public class WarningMessageModel
    {
        IList<MyCmpFaceLogWidthImgModel> _compareLogDatas;
        public IList<MyCmpFaceLogWidthImgModel> CompareLogDatas
        {
            get
            {
                return _compareLogDatas;
            }
            set
            {
                _compareLogDatas = value;
            }
        }

        MyCmpFaceLogWidthImgModel _compareLogData;
        public MyCmpFaceLogWidthImgModel CompareLogData
        {
            get
            {
                return _compareLogData;
            }
            set
            {
                _compareLogData = value;
            }
        }

        IList _curCompareLogDatas
            = new ObservableCollection<MyCmpFaceLogWidthImgModel>();
        public IList CurCompareLogDatas
        {
            get
            {
                return _curCompareLogDatas;
            }
            set
            {
                _curCompareLogDatas = value;
            }
        }

        int _flag;
        public int Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
    }
}
