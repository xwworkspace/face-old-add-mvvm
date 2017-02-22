using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FaceSysClient.ClassPool
{
    #region 实际使用到的
    /// <summary>
    /// 模板管理listView显示
    /// </summary>
    //public class face_object
    //{
    //    //模板对应的人脸标识，全局唯一
    //    public string fa_obj_uuid { get; set; }
    //    //标识ID
    //    public string uuid { get; set; }
    //    //人脸首选模板标识 ID
    //    public int main_ftID { get; set; }
    //    //名称
    //    public string name { get; set; }
    //    //类型
    //    public int type { get; set; }
    //    //敏感等级
    //    public int sst { get; set; }
    //    //额外信息，报警信息是否输出等
    //    public int exten { get; set; }
    //    //性别（0，未知；1，男；2，女）
    //    public int sex { get; set; }
    //    //sex    
    //    public int age { get; set; }
    //    //人脸对象添加时间
    //    public double time { get; set; }
    //    //备注信息
    //    public string remarks { get; set; }
    //    //照片
    //    public string img;
    //    //照片
    //    public int score;
    //}
    //public class CaptureRecord
    //{
    //    // 抓拍id
    //    public string _ID;
    //    //通道名称
    //    public string channel_name { get; set; }
    //    //抓拍时间戳 
    //    public string fcap_time { get; set; }

    //    //抓拍 人脸图片 文件存储路径
    //    public string fcap_obj_image { get; set; }
    //}
    ///// <summary>
    ///// 比对记录表
    ///// </summary>
    //public class face_Comp_record
    //{
    //    // 抓拍id
    //    public string _ID;
    //    //注册名称 
    //    public string name { get; set; }
    //    //抓拍通道 
    //    public string channel_name { get; set; }
    //    //抓拍时间 
    //    public string fcap_time { get; set; }
    //    //注册类型 
    //    public int type { get; set; }
    //    //相似度 
    //    public int fcmp_socre { get; set; }
    //    //抓拍类型
    //    public int fcap_type { get; set; }
    //    //抓拍 人脸图片 
    //    public string ft_image { get; set; }

    //    //标识ID
    //    public string uuid { get; set; }
    //    //所属人脸对象的标识 ID
    //    public string obj_id { get; set; }
    //    //模板标识键（抓拍工作产生的人脸模板时，设置对应的设备通道标识 ID
    //    public string ft_dkey { get; set; }
    //    //模板类型
    //    public int ft_type { get; set; }
    //    //模板识别的性别
    //    public int ft_sex { get; set; }
    //    //模板识别的年龄
    //    public int ft_age { get; set; }
    //    //模板添加时间
    //    public string ft_time { get; set; }
    //    //模板质量
    //    public int ft_quality { get; set; }
    //    //模板人脸x坐标
    //    public string face_x { get; set; }
    //    //模板人脸y坐标
    //    public string face_y { get; set; }
    //    //模板人脸宽度
    //    public int face_cx { get; set; }
    //    //模板人脸高度
    //    public int face_cy { get; set; }
    //    //模板备注信息
    //    public string ft_remarks { get; set; }
    //    //人脸照片的文件存储路径
    //    public string fcap_obj_image { get; set; }
    //    //人脸特征数据的文件存储路径 
    //    public string ft_fea { get; set; }
    //}
    #endregion
    #region 与接口中的表对应
    #region 识别结果信息显示
    public class IdentifyResults//来自于比对记录查询和模板
    {
        public string ID { get; set; } 		// 标识ID，抓拍id，提醒邓工
        public string RegID { get; set; } 		// 标识ID，抓拍id，提醒邓工
        public ImageSource CapImg { get; set; }		// 抓拍照片
        public ImageSource RegImg { get; set; }		// 注册照片
        public string RegInfo { get; set; }    //注册照片信息集合 
                                               //public string
        public string ChannelName { get; set; }

        public string TemplateType { get; set; }
        public RealtimeCmpInfo Info { get; set; } //相关信息
    }
    #endregion
    #region 通道
    public class MyChannelCfg
    {
        public string TcChaneelID { get; set; }
        public string TcUID { get; set; }
        public string TcPSW { get; set; }
        public string Name { get; set; }
        public string TcDescription { get; set; }
        public CaptureCfg CaptureCfg { get; set; }
        public CatchFaceCfg CatchFaceCfg { get; set; }
        public string Addr { get; set; }
        public int Port { get; set; }
        public ImageSource ImageSource { get; set; }
        public ChannelCfg MyChannelCfgToChannelCfg(MyChannelCfg _MyChannelCfg)
        {
            ChannelCfg _ChannelCfg = new ChannelCfg();
            _ChannelCfg.TcChaneelID = _MyChannelCfg.TcChaneelID;
            _ChannelCfg.TcUID = _MyChannelCfg.TcUID;
            _ChannelCfg.TcPSW = _MyChannelCfg.TcPSW;
            _ChannelCfg.Name = _MyChannelCfg.Name;
            _ChannelCfg.TcDescription = _MyChannelCfg.TcDescription;
            _ChannelCfg.CaptureCfg = _MyChannelCfg.CaptureCfg;
            _ChannelCfg.CatchFaceCfg = _MyChannelCfg.CatchFaceCfg;
            _ChannelCfg.Addr = _MyChannelCfg.Addr;
            _ChannelCfg.Port = _MyChannelCfg.Port;
            return _ChannelCfg;
        }
        public MyChannelCfg ChannelCfgToMyChannelCfg(ChannelCfg _ChannelCfg)
        {
            MyChannelCfg _MyChannelCfg = new MyChannelCfg();
            _MyChannelCfg.TcChaneelID = _ChannelCfg.TcChaneelID;
            _MyChannelCfg.TcUID = _ChannelCfg.TcUID;
            _MyChannelCfg.TcPSW = _ChannelCfg.TcPSW;
            _MyChannelCfg.Name = _ChannelCfg.Name;
            _MyChannelCfg.TcDescription = _ChannelCfg.TcDescription;
            _MyChannelCfg.CaptureCfg = _ChannelCfg.CaptureCfg;
            _MyChannelCfg.CatchFaceCfg = _ChannelCfg.CatchFaceCfg;
            _MyChannelCfg.Addr = _ChannelCfg.Addr;
            _MyChannelCfg.Port = _ChannelCfg.Port;
            return _MyChannelCfg;
        }
    }
    #endregion
    #region 识别结果弹出窗口
    public class MyIdentifyResultsWinShow
    {
        public string ID { get; set; }//模板ID

        public ImageSource img { get; set; }//模板照片

        public string name { get; set; }//模板名称

        public int score { get; set; }//相似度
    }
    #endregion
    #region 比对记录查询
    public class MyCmpFaceLogWidthImg
    {
        public int num { get; set; }
        public string ID { get; set; } 		// 标识ID，抓拍id，提醒邓工
        public string name { get; set; }	 	// 姓名
        public string channel { get; set; }	// 抓拍通道
        public string channelName { get; set; }	// 抓拍通道名称
        public string time { get; set; }	 	// 抓拍时间
        public string type { get; set; }		// 注册类型
        public int score { get; set; }	 	// 相似度
        public ImageSource img { get; set; }		// 照片
        public string tcUuid { get; set; }   // uuid，模板对象ID
        public bool IsChecked { get; set; } //是否选中推送
    }
    #endregion
    #region 抓拍记录查询
    public class MyCapFaceLogWithImg
    {
        public int Id { get; set; }
        public string ID { get; set; }		// 抓拍id
        public string ChannelID { get; set; } // 通道id 
        public string ChannelName { get; set; } // 通道名称
        public string time { get; set; }		// 抓拍时间
        public string timeIn { get; set; }	// 对象进入镜头时间
        public string timeOut { get; set; }	// 对象离开镜头时间
        public int quality { get; set; }		// 图像质量
        public int age { get; set; }			// 年龄
        public int gender { get; set; }		// 性别
        public ImageSource img { get; set; } 
    }
    #endregion
    #region 模板管理显示
    public class MyFaceObj
    {
        public FaceObj faceObj { get; set; }
        public int ID { get; set; }
        public string fa_ob_tcUuid { get; set; }	// 人脸uuid
        public string tcName { get; set; }		// 姓名
        public int nMain_ftID { get; set; }		// 人脸首选模板标识序号
        public string nType { get; set; }			// 类型
        public int nSST { get; set; }			// 敏感等级
        public int nExten { get; set; }			// 额外信息，报警信息是否输出等
        public string nSex { get; set; }			// 性别（0，未知；1，男；2，女）
        public int nAge { get; set; }			// 年龄
        public string fa_ob_dTm { get; set; }			// 人脸对象添加时间
        public string fa_ob_tcRemarks { get; set; }  	// 备注
        // 照片模板
        public string temp_tcUuid { get; set; }		// 模板uuid
        public string tcObjid { get; set; }		// 所属FaceObj的uuid
        public string tcKey { get; set; }		// 模板标识键（抓拍工作产生的人脸模板时，设置对应的设备通道标识 ID）
        public int nIndex { get; set; }			// 模板序号
        public DateTime temp_dTm { get; set; }			// 模板添加时间
        public string temp_tcRemarks { get; set; }	// 模板备注
        public ImageSource img { get; set; }			// 图像
    }
    #endregion
    public class RegisterPhoto
    {
        public string key { get; set; }
        public string value { get; set; }
    }
    #endregion
}
