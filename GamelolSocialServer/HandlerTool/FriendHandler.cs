using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AceNetFrame.ace;
using AceNetFrame.ace.auto;
using SerializableDataMessage.SocialMessage.Enum;
using SerializableDataMessage.SocialMessage.Class;
using LitJson;
using DatabaseConnection.Database;
using DatabaseConnection.DataMessage;
namespace GamelolSocialServer.HandlerTool
{
    public class FriendHandler : HanderInterface
    {
        public void ClientClose(UserToken token)
        {
            throw new NotImplementedException();
        }

        public void MessageRecevie(UserToken token, SocketModel message)
        {
            switch ((FriendCommand)message.command) {
                case FriendCommand.ACCEPT_COMMAND:
                    break;
                case FriendCommand.ADD_COMMAND:
                    break;
                    
                case FriendCommand.FIND_COMMAND:
                    FindFriendDeal(token, message);
                    break;
                case FriendCommand.GET_COMMAND:
                    break;
                case FriendCommand.INFORM_COMMADN:
                    break;
            }
        }

        private void AddFriendDeal(UserToken toke, SocketModel model) {
            ApplyMessage applyMessage = JsonMapper.ToObject<ApplyMessage>(model.getMessage<string>());
        }

        /// <summary>
        /// 好友查找处理
        /// </summary>
        /// <param name="token"></param>
        /// <param name="model"></param>
        private void FindFriendDeal(UserToken token, SocketModel model) {
            FriendMessage friendMessage = JsonMapper.ToObject<FriendMessage>(model.getMessage<string>());
            PlayerBaseMessage playerBaseMessage = new BaseMessageDatabase().GetPlayerBaseMessageByPlayerName(friendMessage.friendName);
            if (playerBaseMessage == null)
            {
                friendMessage.isExit = false;
            }
            else
            {
                friendMessage.friendId = playerBaseMessage.PlayerId;
                friendMessage.headImage = playerBaseMessage.PlayerHeadImage;
                friendMessage.isOnline = true;//此属性待判断
                friendMessage.isExit = true;
            }
            string sendString = JsonMapper.ToJson(friendMessage);
            model.message = sendString;
            SendtoClient.write(token,model);

        }


    }
}
