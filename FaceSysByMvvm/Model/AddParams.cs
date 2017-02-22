using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DZVideoWpf;

namespace FaceSysClient.ClassPool
{
    class AddParams
    {
        public int RowCount, ColCount, NscreenNum;
        public Grid _Grid;
        public int I;
        public FaceObj FaceObj;
        public string SstrChannelName, SstrTemplateName;
        public int SstrTemplateType, SstrTemplateSex, SstrTemplateStartAge, SstrTemplateEndAge;
        public long LlongdtPkCompRecordStarTime, LlongdtPkCompRecordEndTime;

        public byte[] TemplatePhoto;
        public int nThreshold;
        public int nMaxCount;

        public string SstrChannel;
        public long LlongDPCapStartTime, LlongDPCapEndTime;

        public string SnTxtUuid, SstrTxtName;
        public int NnCombStartAge, NnCombEndAge, NnCombSex, NnCombType;
        public long LlongDPTemplateStarTime, LlongDpTemplateEndTime;

        public AddParams(int rowCount, int colCount, int nscreenNum)
        {
            RowCount = rowCount;
            ColCount = colCount; 
            NscreenNum = nscreenNum;
        }

        public AddParams(string strChannelName, string strTemplateName,
                int strTemplateType, int strTemplateSex, int strTemplateStartAge, int strTemplateEndAge, long longdtPkCompRecordStarTime, long longdtPkCompRecordEndTime)
        {
            SstrChannelName = strChannelName;
            SstrTemplateName = strTemplateName;
            SstrTemplateType = strTemplateType;
            SstrTemplateSex = strTemplateSex;
            SstrTemplateStartAge = strTemplateStartAge;
            SstrTemplateEndAge = strTemplateEndAge;
            LlongdtPkCompRecordStarTime = longdtPkCompRecordStarTime;
            LlongdtPkCompRecordEndTime = longdtPkCompRecordEndTime;
        }

        public AddParams(string strChannel, long longDPCapStartTime, long longDPCapEndTime)
        {
            SstrChannel = strChannel;
            LlongDPCapStartTime = longDPCapStartTime;
            LlongDPCapEndTime = longDPCapEndTime;
        }
        public AddParams(int i, FaceObj _FaceObj)
        {
            I = i;
            FaceObj = _FaceObj;
        }
        public AddParams(byte[] templatePhoto, int Threshold, int MaxCount)
        {
            TemplatePhoto = templatePhoto;
            nThreshold = Threshold;
            nMaxCount = MaxCount; 
        }

        public UserControl1 _UserControl1;
        public string TcUID, TcPSW, TcAddr;
         public int NPort;
		 public int CamType; // add by imq 2016.8.10
         public AddParams(UserControl1 userControl1, string tcAddr, int nPort, string uid, string psw)
        {
            _UserControl1 = userControl1;
            TcUID = uid;
            TcPSW = psw;
            TcAddr = tcAddr;
            NPort = nPort;
        }
		 public AddParams(UserControl1 userControl1, int nCamType, string tcAddr, int nPort, string uid, string psw)
		 {
			 _UserControl1 = userControl1;
			 CamType = nCamType;
			 TcUID = uid;
			 TcPSW = psw;
			 TcAddr = tcAddr;
			 NPort = nPort;
		 }
    }
}
