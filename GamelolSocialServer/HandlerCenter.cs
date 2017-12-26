using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AceNetFrame.ace;
using AceNetFrame.ace.auto;
using GamelolSocialServer.HandlerTool;
using SerializableDataMessage.SocialMessage.Enum;
namespace GamelolSocialServer
{
    public class HandlerCenter : AbsHandleCenter
    {
        private HanderInterface friendHandler = null;
        private HanderInterface charHandler = null;

        public static List<UserToken> centerServerToken = new List<UserToken>();
        public static List<int> playerOnline = new List<int>();
        public HandlerCenter() {
            friendHandler = new FriendHandler();
            charHandler = new ChatHandler();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="error"></param>
        public override void ClientClose(UserToken token, string error)
        {
            Console.WriteLine("中心服务器断开了连接");
            centerServerToken.Remove(token);
            
        }

        public override void MessageRecive(UserToken token, object message)
        {
            SocketModel model = message as SocketModel;
            //Console.WriteLine(LitJson.JsonMapper.ToJson(model));
            switch ((SocialArea)model.area) {
                case SocialArea.FRIEND_AREA:
                    friendHandler.MessageRecevie(token,model);
                    break;
                case SocialArea.CHAT_AREA:
                    charHandler.MessageRecevie(token,model);
                    break;
            }
        }
    }
}
