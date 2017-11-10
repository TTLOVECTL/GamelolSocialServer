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

        public static SortedDictionary<int, UserToken> playerToken = new SortedDictionary<int, UserToken>();

        public HandlerCenter() {
            friendHandler = new FriendHandler();
            charHandler = new ChatHandler();
        }
        public override void ClientClose(UserToken token, string error)
        {
            Console.WriteLine("玩家ID:"+token.playerId+"断开了连接");
            if (HandlerCenter.playerToken.ContainsKey(token.playerId)) {
                HandlerCenter.playerToken.Remove(token.playerId);
            }
            
        }

        public override void MessageRecive(UserToken token, object message)
        {
            SocketModel model = message as SocketModel;
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
