using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceSysClient.ClassPool
{
    public class WriteLog
    {
        public void WriteToLog(string MethodName, Exception ex)
        {
            try
            {
                #region
                DateTime dateTime = DateTime.Now;
                string strMessage = dateTime.ToString() + ",函数名：" + MethodName + "(),出现异常：" + ex.Message + "\r\n";

                string strDirDest = System.AppDomain.CurrentDomain.BaseDirectory + @"日志";
                //string strDirDest = @"C:\windows\temp";
                string strSavePathName = strDirDest;
                if (!Directory.Exists(strSavePathName))
                    Directory.CreateDirectory(strSavePathName);
                strSavePathName = strSavePathName + "\\log.txt";
                FileStream file = new FileStream(strSavePathName, FileMode.Append);
                StreamWriter sw = new StreamWriter(file);

                sw.WriteLine(strMessage);
                sw.Close();
                file.Close();
                #endregion
            }
            catch (Exception)
            { 
            }
        }
        public void WriteToLog(string MethodName, string ex)
        {
            try
            {
                #region
                DateTime dateTime = DateTime.Now;
                string strMessage = dateTime.ToString() + ",函数名：" + MethodName + "(),出现异常：" + ex + "\r\n";

                string strDirDest = System.AppDomain.CurrentDomain.BaseDirectory + @"日志";
                //string strDirDest = @"C:\windows\temp";
                string strSavePathName = strDirDest;
                if (!Directory.Exists(strSavePathName))
                    Directory.CreateDirectory(strSavePathName);
                strSavePathName = strSavePathName + "\\log.txt";
                FileStream file = new FileStream(strSavePathName, FileMode.Append);
                StreamWriter sw = new StreamWriter(file);

                sw.WriteLine(strMessage);
                sw.Close();
                file.Close();
                #endregion
            }
            catch (Exception)
            {
            }
        }
    }
}
