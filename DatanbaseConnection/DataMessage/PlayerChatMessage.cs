using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.DataMessage
{
    /// <summary>
    /// 聊天信息类
    /// </summary>
    public class PlayerChatMessage
    {
        /// <summary>
        /// 聊天信息ID
        /// </summary>
        public int messageId;

        /// <summary>
        /// 玩家Id
        /// </summary>
        public int playerId;

        /// <summary>
        /// 信息类型
        /// </summary>
        public int messageType;

        /// <summary>
        /// 信息区域
        /// </summary>
        public int messageArea;

        /// <summary>
        /// 信息的指令
        /// </summary>
        public int messageCommand;
        /// <summary>
        /// 信息内容
        /// </summary>
        public string messageContent;

        /// <summary>
        /// 发送时间
        /// </summary>
        public string sentTime;
    }
}
