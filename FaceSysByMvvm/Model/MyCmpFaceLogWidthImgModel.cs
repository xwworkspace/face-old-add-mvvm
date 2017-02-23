using FaceSysClient.ClassPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FaceSysByMvvm.Model
{
    public class MyCmpFaceLogWidthImgModel : MyCmpFaceLogWidthImg
    {
        ImageSource _SnapImage;
        /// <summary>
        /// 抓拍人像
        /// </summary>
        public ImageSource SnapImage
        {
            get
            {
                return _SnapImage;
            }

            set
            {
                _SnapImage = value;
            }
        }
        /// <summary>
        /// 数据库人像
        /// </summary>
        public ImageSource DBImage
        {
            get
            {
                return _DBImage;
            }

            set
            {
                _DBImage = value;
            }
        }

        ImageSource _DBImage;
    }
}
