using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace FaceSysByMvvm.Model
{
    public class WarningMessageCmd
    {
        public static ICommand TestBtnCommand { get; private set; }
        public static ICommand SendBtnCommand { get; private set; }
        public static ICommand ClearBtnCommand { get; private set; }

        public static Action InitCmd()
        {
            Action act = () =>
            {
                TestBtnCommand = new DelegateCommand<object>(TestBtnCommandFunc);
                SendBtnCommand = new DelegateCommand<object>(SendBtnCommandFunc);
                ClearBtnCommand = new DelegateCommand<object>(ClearBtnCommandFunc);
            };
            return act;
        }

        private static void ClearBtnCommandFunc(object obj)
        {
            MessageBox.Show("congratulations xiaowen");
        }

        private static void SendBtnCommandFunc(object obj)
        {
            MessageBox.Show("congratulations xiaowen");
        }

        private static void TestBtnCommandFunc(object obj)
        {
            MessageBox.Show("congratulations xiaowen");
        }
    }
}
