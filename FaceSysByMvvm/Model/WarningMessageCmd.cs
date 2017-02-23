using FaceSysByMvvm.ViewModel.ChannelManage;
using FaceSysByMvvm.ZModel;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace FaceSysByMvvm.Model
{
    public class WarningMessageCmd
    {
        public static ICommand TestBtnCommand { get; private set; }
        public static ICommand SendBtnCommand { get; private set; }
        public static ICommand ClearBtnCommand { get; private set; }
        public static ICommand ClearAllBtnCommand { get; private set; }
        public static ICommand RefreshBtnCommand { get; private set; }
        public static ICommand BatchSendBtnCommand { get; private set; }

        public static Action InitCmd()
        {
            Action act = () =>
            {
                TestBtnCommand = new DelegateCommand<object>(TestBtnCommandFunc);
                SendBtnCommand = new DelegateCommand<object>(SendBtnCommandFunc);
                ClearBtnCommand = new DelegateCommand<object>(ClearBtnCommandFunc);
                ClearAllBtnCommand = new DelegateCommand<object>(ClearAllBtnCommandFunc);
                RefreshBtnCommand = new DelegateCommand<object>(RefreshBtnCommandFunc);
                BatchSendBtnCommand = new DelegateCommand<object>(BatchSendBtnCommandFunc);
            };
            return act;
        }

        /// <summary>
        /// Send Batch
        /// </summary>
        /// <param name="obj"></param>
        private static void BatchSendBtnCommandFunc(object obj)
        {
            for (int i = 0; i < ViewDataModel.WarningData.Property.CurCompareLogDatas.Count;)
            {
                MyCmpFaceLogWidthImgModel s = ViewDataModel.WarningData.Property.CurCompareLogDatas[0] as MyCmpFaceLogWidthImgModel;
                ViewDataModel.WarningData.Property.CompareLogDatas.Remove(s);
            }
            
            ViewDataModel.WarningData.Property.CurCompareLogDatas.Clear();
        }

        /// <summary>
        /// Refresh
        /// </summary>
        /// <param name="obj"></param>
        private static void RefreshBtnCommandFunc(object obj)
        {

        }

        /// <summary>
        /// Cleanning all
        /// </summary>
        /// <param name="obj"></param>
        private static void ClearAllBtnCommandFunc(object obj)
        {

        }

        /// <summary>
        /// clear single
        /// </summary>
        /// <param name="obj"></param>
        private static void ClearBtnCommandFunc(object obj)
        {
            ObservableCollection<MyCmpFaceLogWidthImgModel> clearData = obj as ObservableCollection<MyCmpFaceLogWidthImgModel>;
            clearData.Clear();
        }

        /// <summary>
        /// send single
        /// </summary>
        /// <param name="obj"></param>
        private static void SendBtnCommandFunc(object obj)
        {

        }

        private static void TestBtnCommandFunc(object obj)
        {
            MessageBox.Show("congratulations xiaowen");
        }
    }
}
