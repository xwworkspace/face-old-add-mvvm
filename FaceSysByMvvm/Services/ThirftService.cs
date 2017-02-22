using FaceSysClient.ClassPool;
using System;
using System.Collections.Generic;
using System.Linq;
using Thrift.Protocol;
using Thrift.Transport;
using static FaceSysByMvvm.ViewModel.CompOfRecords.CompOfRecordsViewModel;
using FaceSysByMvvm.ViewModel.CaptureRecordQuery;
using static FaceSysByMvvm.ViewModel.CaptureRecordQuery.CaptureRecordQueryViewModel;
using FaceSysByMvvm.View;
using FaceSysByMvvm.ViewModel.TemplateManager;
using FaceSysByMvvm.Common;

namespace FaceSysByMvvm.Services
{
    public class ThirftService : IThirtfService
    {
        public static string strIP = "";
        public static int nPort;
        public WriteLog _WriteLog = new WriteLog();

        /// <summary>
        /// 心跳
        /// </summary>
        /// <returns></returns>
        public int HearBeat()
        {
            TSocket tsocket = new TSocket(strIP, nPort);
            tsocket.Timeout = 1000;
            TTransport transport = tsocket;
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            int nSuccess = -1;
            try
            {
                if (!transport.IsOpen)
                {
                    
                    transport.Open();
                }
                nSuccess = _BusinessServerClient.HearBeat();
                transport.Close();
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("HearBeat", ex);
                transport.Close();
                return nSuccess;
            }
            return nSuccess;
        }

        /// <summary>
        /// 登录函数
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口</param>
        /// <returns></returns>
        public bool Login(string ip,int port)
        {
            TSocket tsocket = new TSocket(ip, port);
            tsocket.Timeout = 100;
            TTransport transport = tsocket;
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                #endregion
            }
            catch (Exception ex)
            {
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                return false;
            }
            transport.Close();
            strIP = ip;
            nPort = port;
            return true;
        }

        /// <summary>
        /// 获取所有频道
        /// </summary>
        /// <returns></returns>
        public List<MyChannelCfg> QueryAllChannel()
        {
            List<MyChannelCfg> myListChannelCfg = new List<MyChannelCfg>();
            List<ChannelCfg> ListChannelCfg = new List<ChannelCfg>();
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            try
            {
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                ListChannelCfg = _BusinessServerClient.QueryAllChannel();
                transport.Close();
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryAllChannel", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");         
            }
            //todo(暂时不需要) 包装返回类 使其返回需要的类
            foreach(ChannelCfg cc in ListChannelCfg)
            {
                myListChannelCfg.Add(new MyChannelCfg().ChannelCfgToMyChannelCfg(cc));
            }


            return myListChannelCfg;
        }

        /// <summary>
        /// 获取模版类型
        /// </summary>
        /// <returns></returns>
        public List<string> QueryDefFaceObjType()
        {
            List<string> listQueryDefFaceObjType = new List<string>();
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listQueryDefFaceObjType = _BusinessServerClient.QueryDefFaceObjType();

                if (transport.IsOpen)
                {
                    transport.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryDefFaceObjType", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                return null;
                //MyMessage.showYes("网络异常，稍后重试"); 
                //MyMessage.showYes("网络异常，稍后重试");
                //return;
            }
            return listQueryDefFaceObjType;
        }

        /// <summary>
        /// 获取模版数量
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="name">模板名称</param>
        /// <param name="type">模板类型</param>
        /// <param name="gender">性别</param>
        /// <param name="bage">起始年龄</param>
        /// <param name="eage">终止年龄</param>
        /// <param name="btime">起始时间</param>
        /// <param name="etime">终止时间</param>
        /// <returns></returns>
        public int QueryCmpRecordTotalCount(CompOfRecordsValue compOfRecordsValue)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            int _nComparisonRecordCounts = 0;
            try
            {
                #region
                //打开连接
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                _nComparisonRecordCounts = _BusinessServerClient.QueryCmpRecordTotalCount(compOfRecordsValue.ChannelValue, compOfRecordsValue.NameValue,
                   compOfRecordsValue.TypeValue, compOfRecordsValue.SexValue, compOfRecordsValue.LittleAgeValue, compOfRecordsValue.OldAgeValue, compOfRecordsValue.StartDayValue, compOfRecordsValue.EndDayValue);

                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryCmpRecordTotalCount", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                return 0;
            }
            return _nComparisonRecordCounts;
        }

        /// <summary>
        /// 查询比对记录数量(分表)
        /// </summary>
        /// <param name="compOfRecordsValue"></param>
        /// <returns></returns>
        public List<SCountInfo> QueryCmpRecordTotalCountH(CompOfRecordsValue compOfRecordsValue)
        {
            TTransport transport = new TSocket(strIP, nPort, 5000);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<SCountInfo> recordsCount = new List<SCountInfo>();
            try
            {
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                //得到总数
                recordsCount = _BusinessServerClient.QueryCmpRecordTotalCountH(compOfRecordsValue.ChannelValue, compOfRecordsValue.NameValue,
                   compOfRecordsValue.TypeValue, compOfRecordsValue.SexValue, compOfRecordsValue.LittleAgeValue, compOfRecordsValue.OldAgeValue, compOfRecordsValue.StartDayValue, compOfRecordsValue.EndDayValue);

                if (transport.IsOpen)
                {
                    transport.Close();
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("threadQueryCap", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");

                return null;
            }
            return recordsCount;
        }

        public List<SCountInfo> QueryCmpRecordTotalCountHSX(CompOfRecordsValue compOfRecordsValue,int pflag)
        {
            TTransport transport = new TSocket(strIP, nPort, 5000);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<SCountInfo> recordsCount = new List<SCountInfo>();
            try
            {
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                //得到总数
                recordsCount = _BusinessServerClient.QueryCmpRecordTotalCountHSX(compOfRecordsValue.ChannelValue, compOfRecordsValue.NameValue,
                   compOfRecordsValue.TypeValue, compOfRecordsValue.SexValue, compOfRecordsValue.LittleAgeValue, compOfRecordsValue.OldAgeValue, compOfRecordsValue.StartDayValue, compOfRecordsValue.EndDayValue, pflag);

                if (transport.IsOpen)
                {
                    transport.Close();
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("threadQueryCap", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");

                return null;
            }
            return recordsCount;
        }

        /// <summary>
        /// 查询比对记录
        /// </summary>
        /// <returns></returns>
        public List<MyCmpFaceLogWidthImg> QueryCmpLog(CompOfRecordsValue compOfRecordsValue)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<CmpFaceLog> listCmpFaceLog = new List<CmpFaceLog>();
            List<MyCmpFaceLogWidthImg> listMyCmpFaceLogWidthImg = new List<MyCmpFaceLogWidthImg>();
            try
            {
                #region
                //打开连接
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listCmpFaceLog = _BusinessServerClient.QueryCmpLog(compOfRecordsValue.ChannelValue, compOfRecordsValue.NameValue,
                   compOfRecordsValue.TypeValue, compOfRecordsValue.SexValue, compOfRecordsValue.LittleAgeValue, compOfRecordsValue.OldAgeValue, compOfRecordsValue.StartDayValue, compOfRecordsValue.EndDayValue, compOfRecordsValue.StartRowValue, compOfRecordsValue.PageRowValue);

                //比对结果            
                for (int i = listCmpFaceLog.Count - 1; i >= 0; i--)
                {
                    MyCmpFaceLogWidthImg _MyCmpFaceLogWidthImg = new MyCmpFaceLogWidthImg();
                    //获得序号
                    _MyCmpFaceLogWidthImg.num =  compOfRecordsValue.MaxCount - compOfRecordsValue.StartRowValue - i;
                    _MyCmpFaceLogWidthImg.ID = listCmpFaceLog[i].ID;// 标识ID
                    _MyCmpFaceLogWidthImg.name = listCmpFaceLog[i].Name;// 姓名

                    //获得通道名称 
                    foreach (MyChannelCfg mcc in QueryAllChannel())
                    {
                        if (listCmpFaceLog[i].Channel == mcc.TcChaneelID)
                        {
                            _MyCmpFaceLogWidthImg.channelName = mcc.Name;
                        }
                    }
                    //_MyCmpFaceLogWidthImg.channel = listCmpFaceLog[i].Channel;// 抓拍通道
                    long longTime = listCmpFaceLog[i].Time;
                    DateTime time = new DateTime(1970, 1, 1);
                    time = time.AddSeconds(longTime);
                    _MyCmpFaceLogWidthImg.time = time.ToString("yyyy/MM/dd HH:mm:ss");// 抓拍时间
                    foreach (var basicinfo in BasicInfo.DefFaceObjType)
                    {
                        if(basicinfo.Type == listCmpFaceLog[i].Type)
                        {
                            _MyCmpFaceLogWidthImg.type = basicinfo.Description;	// 类型  
                        }
                    }                   
                    _MyCmpFaceLogWidthImg.score = listCmpFaceLog[i].Score;// 相似度
                    _MyCmpFaceLogWidthImg.tcUuid = listCmpFaceLog[i].TcUuid; // uuid，模板ID

                    listMyCmpFaceLogWidthImg.Add(_MyCmpFaceLogWidthImg);
                }

                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryCmpLog", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                //MyMessage.showYes("网络异常，稍后重试");

                return null;
            }
            return listMyCmpFaceLogWidthImg;
        }

        /// <summary>
        /// 查询比对(分区域)
        /// </summary>
        /// <param name="compOfRecordsValue"></param>
        /// <param name="pflag"></param>
        /// <returns></returns>
        public List<MyCmpFaceLogWidthImg> QueryCmpLogSX(CompOfRecordsValue compOfRecordsValue,int pflag)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<CmpFaceLog> listCmpFaceLog = new List<CmpFaceLog>();
            List<MyCmpFaceLogWidthImg> listMyCmpFaceLogWidthImg = new List<MyCmpFaceLogWidthImg>();
            try
            {
                #region
                //打开连接
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listCmpFaceLog = _BusinessServerClient.QueryCmpLogSX(compOfRecordsValue.ChannelValue, compOfRecordsValue.NameValue,
                   compOfRecordsValue.TypeValue, compOfRecordsValue.SexValue, compOfRecordsValue.LittleAgeValue, compOfRecordsValue.OldAgeValue, compOfRecordsValue.StartDayValue, compOfRecordsValue.EndDayValue, compOfRecordsValue.StartRowValue, compOfRecordsValue.PageRowValue, pflag);

                //比对结果            
                for (int i = listCmpFaceLog.Count - 1; i >= 0; i--)
                {
                    MyCmpFaceLogWidthImg _MyCmpFaceLogWidthImg = new MyCmpFaceLogWidthImg();
                    //获得序号
                    _MyCmpFaceLogWidthImg.num = compOfRecordsValue.MaxCount - compOfRecordsValue.StartRowValue - i;
                    _MyCmpFaceLogWidthImg.ID = listCmpFaceLog[i].ID;// 标识ID
                    _MyCmpFaceLogWidthImg.name = listCmpFaceLog[i].Name;// 姓名

                    //获得通道名称 
                    foreach (MyChannelCfg mcc in QueryAllChannel())
                    {
                        if (listCmpFaceLog[i].Channel == mcc.TcChaneelID)
                        {
                            _MyCmpFaceLogWidthImg.channelName = mcc.Name;
                        }
                    }
                    //_MyCmpFaceLogWidthImg.channel = listCmpFaceLog[i].Channel;// 抓拍通道
                    long longTime = listCmpFaceLog[i].Time;
                    DateTime time = new DateTime(1970, 1, 1);
                    time = time.AddSeconds(longTime);
                    _MyCmpFaceLogWidthImg.time = time.ToString("yyyy/MM/dd HH:mm:ss");// 抓拍时间
                    foreach (var basicinfo in BasicInfo.DefFaceObjType)
                    {
                        if (basicinfo.Type == listCmpFaceLog[i].Type)
                        {
                            _MyCmpFaceLogWidthImg.type = basicinfo.Description;	// 类型  
                        }
                    }
                    _MyCmpFaceLogWidthImg.score = listCmpFaceLog[i].Score;// 相似度
                    _MyCmpFaceLogWidthImg.tcUuid = listCmpFaceLog[i].TcUuid; // uuid，模板ID

                    listMyCmpFaceLogWidthImg.Add(_MyCmpFaceLogWidthImg);
                }

                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryCmpLog", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                //MyMessage.showYes("网络异常，稍后重试");

                return null;
            }
            return listMyCmpFaceLogWidthImg;
        }
        

        /// <summary>
        /// 查询比对记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<byte[]> QueryCmpLogImage(string ID)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<byte[]> listImageBytes = new List<byte[]>();
            try
            {
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listImageBytes = _BusinessServerClient.QueryCmpLogImage(ID);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryCmpLogImage", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
               // MyMessage.showYes("网络异常，稍后重试");

                return null;
            }
            return listImageBytes;
        }

        public List<byte[]> QueryCmpLogImageH(string ID, string day)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<byte[]> listImageBytes = new List<byte[]>();
            try
            {
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listImageBytes = _BusinessServerClient.QueryCmpLogImageH(ID, day);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                // MyMessage.showYes("网络异常，稍后重试");

                return null;
            }
            return listImageBytes;
        }

        /// <summary>
        /// 查询模版照片
        /// </summary>
        public List<FaceObj> QueryFaceObj(string id)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<FaceObj> ListFaceObj = new List<FaceObj>();
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                ListFaceObj = _BusinessServerClient.QueryFaceObj(id, null, -1, -1, -1, -1, -1, -1, -1, -1);//查到被比对的人脸对象
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryFaceObj", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                //MyMessage.showYes("网络异常，稍后重试");

                return null;
            }
            return ListFaceObj;
        }

        /// <summary>
        /// 抓拍记录数量
        /// </summary>
        /// <param name="captureRecordQueryValue"></param>
        /// <returns></returns>
        public int QueryCapRecordTotalCount(CaptureRecordQueryValue captureRecordQueryValue)
        {
            //声明客户端内容
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            int recordsCount = 0;
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                //得到总数
                recordsCount = _BusinessServerClient.QueryCapRecordTotalCount(captureRecordQueryValue.ChannelValue, captureRecordQueryValue.StartDayValue, captureRecordQueryValue.EndDayValue);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryCapRecordTotalCount", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");

                return 0;
            }
            return recordsCount;
        }

        public List<SCountInfo> QueryCapRecordTotalCountH(CaptureRecordQueryValue captureRecordQueryValue)
        {
            TTransport transport = new TSocket(strIP, nPort, 5000);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<SCountInfo> recordsCount = new List<SCountInfo>();
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                //得到总数
                recordsCount = _BusinessServerClient.QueryCapRecordTotalCountH(captureRecordQueryValue.ChannelValue, captureRecordQueryValue.StartDayValue, captureRecordQueryValue.EndDayValue);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("threadQueryCap", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");

                return null;
            }
            return recordsCount;
        }

        /// <summary>
        /// 查询比对记录数(筛选)
        /// </summary>
        /// <param name="captureRecordQueryValue"></param>
        /// <returns></returns>
        public List<SCountInfo> QueryCapRecordTotalCountHSXC(CaptureRecordQueryValue captureRecordQueryValue,List<string> channelName)
        {
            TTransport transport = new TSocket(strIP, nPort, 5000);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<SCountInfo> recordsCount = new List<SCountInfo>();
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                //得到总数
                recordsCount = _BusinessServerClient.QueryCapRecordTotalCountHSXC(channelName, captureRecordQueryValue.StartDayValue, captureRecordQueryValue.EndDayValue);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("threadQueryCap", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");

                return null;
            }
            return recordsCount;
        }

        /// <summary>
        /// 抓拍记录数据
        /// </summary>
        /// <param name="captureRecordQueryValue"></param>
        /// <returns></returns>
        public List<MyCapFaceLogWithImg> QueryCapLog(CaptureRecordQueryValue captureRecordQueryValue)
        {
            List<CapFaceLog> listCapFaceLog = new List<CapFaceLog>();
            List<MyCapFaceLogWithImg> listMyCapFaceLogWithImg = new List<MyCapFaceLogWithImg>();
            //声明客户端内容
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            try
            {
                //获得查询数据 
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listCapFaceLog = _BusinessServerClient.QueryCapLog(captureRecordQueryValue.ChannelValue, captureRecordQueryValue.StartDayValue, captureRecordQueryValue.EndDayValue, captureRecordQueryValue.StartRowValue, captureRecordQueryValue.PageRowValue);

                
                for (int i = listCapFaceLog.Count - 1; i >= 0; i--)
                {
                    MyCapFaceLogWithImg _MyCapFaceLogWithImg = new MyCapFaceLogWithImg();
                    _MyCapFaceLogWithImg.Id = captureRecordQueryValue.MaxCount - captureRecordQueryValue.StartRowValue - i;
                    _MyCapFaceLogWithImg.ID = listCapFaceLog[i].ID;// 获得抓拍id
                    _MyCapFaceLogWithImg.ChannelID = listCapFaceLog[i].ChannelID;// 获得通道id

                    //获得通道名称 
                    foreach (MyChannelCfg mcc in QueryAllChannel())
                    {
                        if (listCapFaceLog[i].ChannelID == mcc.TcChaneelID)
                        {
                            _MyCapFaceLogWithImg.ChannelName = mcc.Name;
                        }
                    }

                    long longTime = listCapFaceLog[i].Time;
                    DateTime time = new DateTime(1970, 1, 1);
                    time = time.AddSeconds(longTime);
                    _MyCapFaceLogWithImg.time = time.ToString("yyyy/MM/dd HH:mm:ss"); ;// 获得抓拍时间

                    listMyCapFaceLogWithImg.Add(_MyCapFaceLogWithImg);
                }

                transport.Close();
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryCapLog", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试");
                return null;
            }
            return listMyCapFaceLogWithImg;
        }

        /// <summary>
        /// 查询抓拍记录(筛选)
        /// </summary>
        /// <param name="captureRecordQueryValue"></param>
        /// <param name="pflag"></param>
        /// <returns></returns>
        public List<MyCapFaceLogWithImg> QueryCapLogSXC(CaptureRecordQueryValue captureRecordQueryValue,List<string> channelList)
        {
            List<CapFaceLog> listCapFaceLog = new List<CapFaceLog>();
            List<MyCapFaceLogWithImg> listMyCapFaceLogWithImg = new List<MyCapFaceLogWithImg>();
            //声明客户端内容
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            try
            {
                //获得查询数据 
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listCapFaceLog = _BusinessServerClient.QueryCapLogSXC(channelList, captureRecordQueryValue.StartDayValue, captureRecordQueryValue.EndDayValue, captureRecordQueryValue.StartRowValue, captureRecordQueryValue.PageRowValue);


                for (int i = listCapFaceLog.Count - 1; i >= 0; i--)
                {
                    MyCapFaceLogWithImg _MyCapFaceLogWithImg = new MyCapFaceLogWithImg();
                    _MyCapFaceLogWithImg.Id = captureRecordQueryValue.MaxCount - captureRecordQueryValue.StartRowValue - i;
                    _MyCapFaceLogWithImg.ID = listCapFaceLog[i].ID;// 获得抓拍id
                    _MyCapFaceLogWithImg.ChannelID = listCapFaceLog[i].ChannelID;// 获得通道id

                    //获得通道名称 
                    foreach (MyChannelCfg mcc in QueryAllChannel())
                    {
                        if (listCapFaceLog[i].ChannelID == mcc.TcChaneelID)
                        {
                            _MyCapFaceLogWithImg.ChannelName = mcc.Name;
                        }
                    }

                    long longTime = listCapFaceLog[i].Time;
                    DateTime time = new DateTime(1970, 1, 1);
                    time = time.AddSeconds(longTime);
                    _MyCapFaceLogWithImg.time = time.ToString("yyyy/MM/dd HH:mm:ss"); ;// 获得抓拍时间

                    listMyCapFaceLogWithImg.Add(_MyCapFaceLogWithImg);
                }

                transport.Close();
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryCapLog", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试");
                return null;
            }
            return listMyCapFaceLogWithImg;
        }

        /// <summary>
        /// 抓拍记录照片
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<byte[]> QueryCapLogImage(string ID)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<byte[]> listImageBytes = new List<byte[]>();
            try
            {
                #region
                //打开连接
                if (!transport.IsOpen)
                {
                    transport.Open();

                }
                //调用接口获得抓拍照片
                listImageBytes = _BusinessServerClient.QueryCapLogImage(ID);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryCapLogImage", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");
                return null;
            }
            return listImageBytes;
        }

        public List<byte[]> QueryCapLogImageH(string ID, string day)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<byte[]> listImageBytes = new List<byte[]>();
            try
            {
                #region
                //打开连接
                if (!transport.IsOpen)
                {
                    transport.Open();

                }
                //调用接口获得抓拍照片
                listImageBytes = _BusinessServerClient.QueryCapLogImageH(ID, day);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryCapLogImage", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");
                return null;
            }
            return listImageBytes;
        }
        public List<byte[]> QuerySenceImg(string ID,string day)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<byte[]> listImageBytes = new List<byte[]>();
            try
            {
                #region
                //打开连接
                if (!transport.IsOpen)
                {
                    transport.Open();

                }
                //调用接口获得抓拍照片
                listImageBytes = _BusinessServerClient.QuerySenceImg(ID, day);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QuerySenceImg", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");
                return null;
            }
            return listImageBytes;
        }

        /// <summary>
        /// 模板管理记录总数
        /// </summary>
        /// <param name="templateManagerValue"></param>
        /// <returns></returns>
        public int QueryFaceObjTotalCount(TemplateManagerViewModel.TemplateManagerValue template)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            int _nTemplatePageCounts = 0;
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }

                //获得查询总量
                _nTemplatePageCounts = _BusinessServerClient.QueryFaceObjTotalCount(null,
                    template.NameValue, template.LittleAgeValue, template.OldAgeValue, template.SexValue, template.TypeValue,template.StartDayValue, template.EndDayValue);

                if (transport.IsOpen)
                {
                    transport.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryFaceObjTotalCount", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");

                return 0;
            }
            return _nTemplatePageCounts;
        }

        /// <summary>
        /// 查询模版
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public List<MyFaceObj> QueryFaceObj(TemplateManagerViewModel.TemplateManagerValue template)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<FaceObj> _ListFaceObj = new List<FaceObj>();
            List<MyFaceObj> _ListMyFaceObj = new List<MyFaceObj>();
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                //获得查询数据
                _ListFaceObj = _BusinessServerClient.QueryFaceObj(null,
                    template.NameValue,  template.LittleAgeValue, template.OldAgeValue, template.SexValue, template.TypeValue, template.StartDayValue, template.EndDayValue, template.StartRowValue, template.PageRowValue);

                List<string> sexList = new List<string>() { "未知", "男", "女" };

                for (int i = _ListFaceObj.Count - 1; i >= 0; i--)//循环得到人脸
                {
                    MyFaceObj _MyFaceObj = new MyFaceObj();
                    _MyFaceObj.faceObj = _ListFaceObj[i];
                    _MyFaceObj.ID = template.MaxCount - template.StartRowValue - i;
                    _MyFaceObj.fa_ob_tcUuid = _ListFaceObj[i].TcUuid;
                    _MyFaceObj.tcName = _ListFaceObj[i].TcName;// 姓名 
                    foreach (var basicinfo in BasicInfo.DefFaceObjType)
                    {
                        if (basicinfo.Type == _ListFaceObj[i].NType)
                        {
                            _MyFaceObj.nType = basicinfo.Description;	// 类型  
                        }
                    }
                   
                    _MyFaceObj.nSex = sexList[_ListFaceObj[i].NSex];// 性别（0，未知；1，男；2，女）
                    _MyFaceObj.nMain_ftID = _ListFaceObj[i].NMain_ftID;
                    _MyFaceObj.nAge = _ListFaceObj[i].NAge;		// 年龄 

                    long longTime = _ListFaceObj[i].DTm;
                    DateTime time = new DateTime(1970, 1, 1);
                    time = time.AddSeconds(longTime);
                    _MyFaceObj.fa_ob_dTm = time.ToString();

                    _MyFaceObj.fa_ob_tcRemarks = _ListFaceObj[i].TcRemarks;

                    _ListMyFaceObj.Add(_MyFaceObj);
                }


                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryFaceObj", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");

                return null;
            }
            return _ListMyFaceObj;
        }

        /// <summary>
        /// 修改模版
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<ErrorInfo> ModifyFaceObj(string id, FaceObj obj)
        {
            TTransport transport1 = new TSocket(strIP, nPort);
            TProtocol protocol1 = new TBinaryProtocol(transport1);
            BusinessServer.Client _BusinessServerClient1 = new BusinessServer.Client(protocol1);
            List<ErrorInfo> ListErrorInfo = new List<ErrorInfo>() ;
            try
            {
                if (!transport1.IsOpen)
                {
                    transport1.Open();
                }
                //调用修改接口，获得返回值
                ListErrorInfo = _BusinessServerClient1.ModifyFaceObj(id, obj);
                transport1.Close();
            }
            catch (Exception ex)
            {
                MyMessage.showYes("网络异常，稍后重试！");
                _WriteLog.WriteToLog("ModifyFaceObj", ex);
                if (transport1.IsOpen)
                {
                    transport1.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");
            }
            return ListErrorInfo;
        }

        /// <summary>
        /// 删除模版
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int DelFaceObj(string ID)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            int nSucess = -1;
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                nSucess = _BusinessServerClient.DelFaceObj(ID);
                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("DelFaceObj", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");
            }
            return nSucess;
        }

        /// <summary>
        /// 添加模版
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<ErrorInfo> AddFaceObj(FaceObj obj)
        {
            TTransport transport1 = new TSocket(strIP, nPort);
            TProtocol protocol1 = new TBinaryProtocol(transport1);
            BusinessServer.Client _BusinessServerClient1 = new BusinessServer.Client(protocol1);
            List<ErrorInfo> ListErrorInfo = new List<ErrorInfo>();
            try
            {
                if (!transport1.IsOpen)
                {
                    transport1.Open();
                }
                //调用修改接口，获得返回值
                ListErrorInfo = _BusinessServerClient1.AddFaceObj(obj);
                transport1.Close();
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("AddFaceObj", ex);
                if (transport1.IsOpen)
                {
                    transport1.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");
            }
            return ListErrorInfo;
        }

        /// <summary>
        /// 查询通道类型
        /// </summary>
        /// <returns></returns>
        public List<string> QueryDefChannelType()
        {
            List<string> listQueryDefChannelType = new List<string>();
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listQueryDefChannelType = _BusinessServerClient.QueryDefChannelType();

                if (transport.IsOpen)
                {
                    transport.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryDefChannelType", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                return null;
                //MyMessage.showYes("网络异常，稍后重试"); 
            }
            return listQueryDefChannelType;
        }

        /// <summary>
        /// 查询视频源类型
        /// </summary>
        /// <returns></returns>
        public List<string> QueryDefCameraType()
        {
            List<string> listQueryDefCameraType = new List<string>();
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listQueryDefCameraType = _BusinessServerClient.QueryDefCameraType();

                if (transport.IsOpen)
                {
                    transport.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryDefCameraType", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                return null;
                //MyMessage.showYes("网络异常，稍后重试"); 
            }
            return listQueryDefCameraType;
        }

        /// <summary>
        /// 添加频道
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public int AddChannel(ChannelCfg cfg)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            int nSucess = -1;
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                nSucess = _BusinessServerClient.AddChannel(cfg);
                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("AddChannel", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");
            }
            return nSucess;
        }

        /// <summary>
        /// 修改频道
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public int ModifyChannel(ChannelCfg cfg)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            int nSucess = -1;
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                nSucess = _BusinessServerClient.ModifyChannel(cfg);
                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("ModifyChannel", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");
            }
            return nSucess;
        }

        public List<MyFaceObj> QueryFaceObjByImg(byte[] image, int nThreshold, int nMaxCount)
        {
            List<FaceObj> _ListFaceObj = new List<FaceObj>();
            List<MyFaceObj> _ListMyFaceObj = new List<MyFaceObj>();
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                _ListFaceObj = _BusinessServerClient.QueryFaceObjByImg(image,nThreshold,nMaxCount);

                List<string> sexList = new List<string>() { "未知", "男", "女" };
                for (int i = _ListFaceObj.Count - 1; i >= 0; i--)//循环得到人脸
                {
                    MyFaceObj _MyFaceObj = new MyFaceObj();
                    _MyFaceObj.faceObj = _ListFaceObj[i];
                    _MyFaceObj.ID = _ListFaceObj.Count - i;
                    _MyFaceObj.fa_ob_tcUuid = _ListFaceObj[i].TcUuid;
                    _MyFaceObj.tcName = _ListFaceObj[i].TcName;// 姓名 
                    foreach (var basicinfo in BasicInfo.DefFaceObjType)
                    {
                        if (basicinfo.Type == _ListFaceObj[i].NType)
                        {
                            _MyFaceObj.nType = basicinfo.Description;	// 类型  
                        }
                    }
                    _MyFaceObj.nSex = sexList[_ListFaceObj[i].NSex];// 性别（0，未知；1，男；2，女）
                    _MyFaceObj.nMain_ftID = _ListFaceObj[i].NMain_ftID;
                    _MyFaceObj.nAge = _ListFaceObj[i].NAge;		// 年龄 

                    long longTime = _ListFaceObj[i].DTm;
                    DateTime time = new DateTime(1970, 1, 1);
                    time = time.AddSeconds(longTime);
                    _MyFaceObj.fa_ob_dTm = time.ToString();

                    _MyFaceObj.fa_ob_tcRemarks = _ListFaceObj[i].TcRemarks;

                    _ListMyFaceObj.Insert(0,_MyFaceObj);
                }

                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryFaceObjByImg", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");
            }
            return _ListMyFaceObj;     
        }

        public List<CmpFaceLogWidthImg> QueryCmpByCapIdWidthImg(string ID)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<CmpFaceLogWidthImg> listCmpFaceLogWidthImg = new List<CmpFaceLogWidthImg>();

            try
            {
                #region
                //打开连接
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listCmpFaceLogWidthImg = _BusinessServerClient.QueryCmpByCapIdWidthImg(ID);
                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryCmpByCapIdWidthImg", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                //MyMessage.showYes("网络异常，稍后重试");

                return null;
            }
            return listCmpFaceLogWidthImg;
        }

        public List<CmpFaceLogWidthImg> QueryCmpByCapIdWidthImgH(string ID,string day)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<CmpFaceLogWidthImg> listCmpFaceLogWidthImg = new List<CmpFaceLogWidthImg>();

            try
            {
                #region
                //打开连接
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listCmpFaceLogWidthImg = _BusinessServerClient.QueryCmpByCapIdWidthImgH(ID, day);
                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryCmpByCapIdWidthImgH", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                //MyMessage.showYes("网络异常，稍后重试");

                return null;
            }
            return listCmpFaceLogWidthImg;
        }

        /// <summary>
        /// 删除通道
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public int DelChannel(string channelID)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            int nSucess = -1;
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                nSucess = _BusinessServerClient.DelChannel(channelID);
                transport.Close();           
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("DelChannel", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("网络异常，稍后重试！");
            }
            return nSucess;
        }

        /// <summary>
        /// 设置新的域值
        /// </summary>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public int SetCMPthreshold(int threshold)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            int nSucess = -1;
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                nSucess = _BusinessServerClient.SetCMPthreshold(threshold);
                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("SetCMPthreshold", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("设置域值失败，稍后重试！");
            }
            return nSucess;
        }

        /// <summary>
        /// 查询当前的域值
        /// </summary>
        /// <returns></returns>
        public int QueryThreshold()
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            int nSucess = -1;
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                nSucess = _BusinessServerClient.QueryThreshold();
                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryThreshold", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("查询域值失败，稍后重试！");
            }
            return nSucess;
        }

        public int  UpdateCmpLog(string ID,string uuid, string day, int pflag)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            int nSucess = -1;
            try
            {
                #region
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                nSucess = _BusinessServerClient.UpdateCmpLog(ID,uuid,day,pflag);
                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("UpdateCmpLog", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
                //MyMessage.showYes("网络异常，稍后重试");
                MyMessage.showYes("更新比对记录失败，稍后重试！");
            }
            return nSucess;
        }

        /// <summary>
        /// 查询性别定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<STypeInfo> QueryDefGenderH(int id)
        {
            TTransport transport = new TSocket(strIP, nPort,1000);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<STypeInfo> listStypeInfo = new List<STypeInfo>();
            try
            {
                #region
                //打开连接
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listStypeInfo = _BusinessServerClient.QueryDefGenderH(id);
                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryDefGenderH", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
            }
            return listStypeInfo;
        }

        /// <summary>
        /// 查询人脸对象类型定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<STypeInfo> QueryDefFaceObjTypeH(int id)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<STypeInfo> listStypeInfo = new List<STypeInfo>();
            try
            {
                #region
                //打开连接
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listStypeInfo = _BusinessServerClient.QueryDefFaceObjTypeH(id);
                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryDefFaceObjTypeH", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
            }
            return listStypeInfo;
        }

        /// <summary>
        /// 查询通道类型定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<STypeInfo> QueryDefChannelTypeH(int id)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<STypeInfo> listStypeInfo = new List<STypeInfo>();
            try
            {
                #region
                //打开连接
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listStypeInfo = _BusinessServerClient.QueryDefChannelTypeH(id);
                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryDefChannelTypeH", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
            }
            return listStypeInfo;
        }

        /// <summary>
        /// 查询相机类型定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<STypeInfo> QueryDefCameraTypeH(int id)
        {
            TTransport transport = new TSocket(strIP, nPort);
            TProtocol protocol = new TBinaryProtocol(transport);
            BusinessServer.Client _BusinessServerClient = new BusinessServer.Client(protocol);
            List<STypeInfo> listStypeInfo = new List<STypeInfo>();
            try
            {
                #region
                //打开连接
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
                listStypeInfo = _BusinessServerClient.QueryDefChannelTypeH(id);
                transport.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("QueryDefCameraTypeH", ex);
                if (transport.IsOpen)
                {
                    transport.Close();
                }
            }
            return listStypeInfo;
        }
    }
}
