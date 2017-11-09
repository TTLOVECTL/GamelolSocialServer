using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializableDataMessage.SocialMessage.Class
{
    /// <summary>
    /// 好友信息类
    /// </summary>
    public class FriendMessage
    {
        /// <summary>
        /// 好友ID
        /// </summary>
        public int friendId;

        /// <summary>
        /// 好友昵称
        /// </summary>
        public String friendName;

        /// <summary>
        /// 好友是否在线
        /// </summary>
        public string isOnline;

        /// <summary>
        /// 好友头像
        /// </summary>
        public byte[] headImage;
    }
}
