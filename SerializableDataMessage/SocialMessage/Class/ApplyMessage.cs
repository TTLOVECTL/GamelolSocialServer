using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializableDataMessage.SocialMessage.Class
{
    /// <summary>
    /// 好友申请信息
    /// </summary>
    public class ApplyMessage
    {
        /// <summary>
        /// 申请者ID
        /// </summary>
        public int sendId;

        /// <summary>
        /// 被申请者ID
        /// </summary>
        public int acceptId;

        /// <summary>
        /// 申请信息ID
        /// </summary>
        public int messageid;

        /// <summary>
        /// 申请者昵称
        /// </summary>
        public string applyerName;

        /// <summary>
        /// 申请者头像
        /// </summary>
        public string headimage;

        /// <summary>
        /// 被申请者是否同意
        /// </summary>
        public bool isAgree;

        /// <summary>
        /// 是否是请求加好友信息
        /// </summary>
        public bool isApply;
    }
}
