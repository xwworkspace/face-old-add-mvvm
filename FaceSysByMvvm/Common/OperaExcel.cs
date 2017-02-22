using FaceSysByMvvm.View;
using NPOI.XSSF.UserModel;
using System;
using System.IO;

namespace FaceSysClient.ClassPool
{
    class OperaExcel
    {
        public WriteLog _WriteLog = new WriteLog();
        /// <summary>
        /// 读取excel文件，返回总信息条数
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public void nNumReadExcel(System.IO.FileInfo[] files, ref int nNumExcel)
        {
            try
            {
                #region
                //遍历文件
                for (int i = 0; i < files.Length; i++)
                {
                    string strPathName = (files[i].DirectoryName + "\\" + files[i].Name);
                    if (!File.Exists(strPathName))
                    {
                        //MyMessage.showYes("文件不存在！");
                        MyMessage.showYes("文件不存在");
                        return;
                    }

                    try//捕获Excel异常
                    {
                        using (var far = new FileStream(strPathName, FileMode.Open, FileAccess.Read))
                        {
                            int countOfExcel = 0; //获得
                            XSSFWorkbook xBook = new XSSFWorkbook(far);
                            int x = xBook.GetCTWorkbook().sheets.sheet.Count;
                            for (int o = 0; o < x; o++)
                            {
                                ReadNumFromExcel(xBook, o, ref countOfExcel);
                                nNumExcel = nNumExcel + countOfExcel;
                                countOfExcel = 0;
                            }

                            far.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        _WriteLog.WriteToLog("nNumReadExcel", ex);
                        return;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("nNumReadExcel", ex);
                //System.Windows.MessageBox.Show(ex.Message); 
            }
        }
        private void ReadNumFromExcel(XSSFWorkbook xBook, int o, ref int countOfExcel)
        {
            try
            {
                XSSFSheet xSheet = (XSSFSheet)xBook.GetSheetAt(o);
                int i = 1;
                while (xSheet.GetRow(i) != null)                                    // 遍历行
                {
                    i++;
                }
                countOfExcel = i - 1;
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("ReadNumFromExcel", ex);
                return;
            }
        }
        /// <summary>
        /// 读取csv文件，返回总信息条数
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public void nNumReadCsv(System.IO.FileInfo[] files, ref int nNunCsv)
        {
            try
            {
                for (int i = 0; i < files.Length; i++)
                {
                    int nCountCsv = 0;
                    string strPathName = (files[i].DirectoryName + "\\" + files[i].Name);
                    if (!File.Exists(strPathName))
                    {
                        //MyMessage.showYes("文件不存在！");
                        MyMessage.showYes("文件不存在");
                        return;
                    }

                    OpenCSVReadNum(strPathName, ref nCountCsv);
                    nNunCsv = nNunCsv + nCountCsv;
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("nNumReadCsv", ex);
                return;
            }
        }
        public void OpenCSVReadNum(string filePath, ref int nCountCsv)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    int i = 0;
                    StreamReader sr = new StreamReader(fs);
                    //记录每次读取的一行记录
                    string strLine = "";
                    //记录每行记录中的各字段内容
                    //string[] aryLine = null;
                    string[] tableHead = null;
                    //标示列数
                    int columnCount = 0;
                    //标示是否是读取的第一行
                    bool IsFirst = true;
                    //逐行读取CSV中的数据
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        i++;
                    }
                    nCountCsv = i;
                    sr.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("OpenCSVReadNum", ex);
                return;
            }
        }
        /// <summary>
        /// 获得指定路径中的文件
        /// </summary>
        /// <param name="folder">指定的路径</param>
        /// <returns></returns>
        public FileInfo[] GetFiles(string folder, bool bIsContainsSubFold)//传入参数是文件夹路径
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(folder);
                if (directory.Exists)
                {
                    //文件夹及子文件夹下的所有文件的全路径
                    FileInfo[] files = null;
                    if (bIsContainsSubFold)
                    {
                        files = directory.GetFiles("*.xlsx", SearchOption.AllDirectories);
                    }
                    else
                    {
                        files = directory.GetFiles("*.xlsx");
                    }
                    return files;
                }
                //MyMessage.showYes("文件夹不存在");
                MyMessage.showYes("文件不存在");
                return null;
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("OpenCSVReadNum", ex);
                return null;
            }

        }
        public FileInfo[] GetFilescsv(string folder, bool bIsContainsSubFold)//传入参数是文件夹路径
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(folder);
                if (directory.Exists)
                {
                    //文件夹及子文件夹下的所有文件的全路径
                    FileInfo[] files = null;
                    if (bIsContainsSubFold)
                    {
                        files = directory.GetFiles("*.csv", SearchOption.AllDirectories);
                    }
                    else
                    {
                        files = directory.GetFiles("*.csv");
                    }
                    return files;
                }
                //MyMessage.showYes("文件夹不存在");
                MyMessage.showYes("文件不存在");
                return null;
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("OpenCSVReadNum", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取文件夹的所有图片文件
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="bIsContainsSubFold"></param>
        /// <returns></returns>
        public FileInfo[] GetFilesPic(string folder, bool bIsContainsSubFold)//传入参数是文件夹路径
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(folder);
                if (directory.Exists)
                {
                    //文件夹及子文件夹下的所有文件的全路径
                    FileInfo[] files = null;
                    if (bIsContainsSubFold)
                    {
                        files = directory.GetFiles("*.jpg", SearchOption.AllDirectories);
                    }
                    else
                    {
                        files = directory.GetFiles("*.jpg");
                    }
                    return files;
                }
                //MyMessage.showYes("文件夹不存在");
                MyMessage.showYes("文件不存在");
                return null;
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("GetFilesPic", ex);
                return null;
            }
        }

        /// <summary>
        /// 读取图片文件，返回总信息条数
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public void nNumReadPic(System.IO.FileInfo[] files, ref int nNunPic)
        {
            try
            {
                for (int i = 0; i < files.Length; i++)
                {
                    string strPathName = (files[i].DirectoryName + "\\" + files[i].Name);
                    if (File.Exists(strPathName))
                    {
                        nNunPic++;
                    }
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("OpenCSVReadNum", ex);
                return;
            }
        }
        public void ReadExcel(ref FaceObj _FaceObj, System.IO.FileInfo[] files)
        {
            try
            {
                #region
                //遍历文件
                for (int i = 0; i < files.Length; i++)
                {
                    string strPathName = (files[i].DirectoryName + "\\" + files[i].Name);
                    if (!File.Exists(strPathName))
                    {
                        //MyMessage.showYes("文件不存在！");
                        MyMessage.showYes("文件不存在");
                        return;
                    }

                    try//捕获Excel异常
                    {
                        using (var far = new FileStream(strPathName, FileMode.Open, FileAccess.Read))
                        {
                            XSSFWorkbook xBook = new XSSFWorkbook(far);
                            int x = xBook.GetCTWorkbook().sheets.sheet.Count;
                            for (int o = 0; o < x; o++)
                                //ReadSheetFromExcel(xBook, o, ref _FaceObj);
                                far.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        _WriteLog.WriteToLog("ReadExcel", ex);
                        return;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("ReadExcel", ex);
                //System.Windows.MessageBox.Show(ex.Message); 
            }
        }
    }
}
