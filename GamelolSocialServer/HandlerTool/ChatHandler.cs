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
    public class ChatHandler : HanderInterface
    {
        public void ClientClose(UserToken token)
        {
            throw new NotImplementedException();
        }

        public void MessageRecevie(UserToken token, SocketModel message)
        {
            switch ((ChatCommand)message.command)
            {
                case ChatCommand.ALL_COMMAND:

                    break;
                case ChatCommand.FRIEND_COMMAND:
                    FriendChatDeal(token,message);
                    break;
            }
        }


        /// <summary>
        /// 世界频道聊天信息处理
        /// </summary>
        /// <param name="token"></param>
        /// <param name="model"></param>
        private void AllChatDeal(UserToken token, SocketModel model)
        {
            ChatMessage chatMessage = JsonMapper.ToObject<ChatMessage>(model.getMessage<string>());
            foreach (UserToken userToken in HandlerCenter.centerServerToken)
            {
                SendtoClient.write(userToken, model);
            }
        }

        /// <summary>
        /// 好友频道信息聊天处理
        /// </summary>
        /// <param name="token"></param>
        /// <param name="model"></param>
        private void FriendChatDeal(UserToken token, SocketModel model)
        {
            ChatMessage chatMessage = JsonMapper.ToObject<ChatMessage>(model.getMessage<string>());
            Console.WriteLine(JsonMapper.ToJson(model));
            model.type = chatMessage.reciverId;
            bool flag = false;
            foreach (int item in HandlerCenter.playerOnline) {
                if (item == model.type)
                    flag = true;
            }
            Console.WriteLine(flag);
            if (!flag) {
                PlayerChatMessage playerChatMessage = new PlayerChatMessage();
                playerChatMessage.playerId = model.type;
                playerChatMessage.messageContent = JsonMapper.ToJson(model);
                playerChatMessage.sentTime = chatMessage.sendTime;
                //new ChatMessageDatabase().InsertPlayerChatMessage(playerChatMessage);
                return;
            }
            foreach (UserToken userToken in HandlerCenter.centerServerToken) {
                SendtoClient.write(userToken,model);
            }
        }

    }
}
