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
using GamelolSocialServer.Util;

namespace GamelolSocialServer.HandlerTool
{
    public class FriendHandler : HanderInterface
    {
        public void ClientClose(UserToken token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 信息接受处理
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        public void MessageRecevie(UserToken token, SocketModel message)
        {
            switch ((FriendCommand)message.command) {
                case FriendCommand.ACCEPT_COMMAND:
                    Console.WriteLine("[好友接受请求指令]"+JsonMapper.ToJson(message));
                    AcceptFriendDeal(token, message);
                    break;
                case FriendCommand.ADD_COMMAND:
                    Console.WriteLine("[好友添加请求指令]" + JsonMapper.ToJson(message));
                    AddFriendDeal(token,message);
                    break;
                case FriendCommand.FIND_COMMAND:
                    Console.WriteLine("[好友查找请求指令]" + JsonMapper.ToJson(message));
                    FindFriendDeal(token, message);
                    break;
                case FriendCommand.GET_COMMAND:
                    Console.WriteLine("[好友信息获取指令]" + JsonMapper.ToJson(message));
                    GetFriendDeal(token,message);
                    break;
                case FriendCommand.INFORM_COMMADN:
                    Console.WriteLine("[好友上下线通知指令]" + JsonMapper.ToJson(message));
                    InformFriendDeal(token,message);
                    break;
                case FriendCommand.DELETE_COMMAND:
                    Console.WriteLine("[好友删除请求指令]" + JsonMapper.ToJson(message));
                    DeleteFriendDeal(token,message);
                    break;
            }
        }

        /// <summary>
        /// 添加好友请求，将好友请求转发给接受者
        /// </summary>
        /// <param name="toke"></param>
        /// <param name="model">model包含ApplyMessage信息</param>
        private void AddFriendDeal(UserToken toke, SocketModel model) {
            ApplyMessage applyMessage = JsonMapper.ToObject<ApplyMessage>(model.getMessage<string>());
            model.type = applyMessage.acceptId;
            bool flag = false;
            foreach (int item in HandlerCenter.playerOnline)
            {
                if (item == model.type)
                    flag = true;
            }
            PlayerBaseMessage playerBaseMessage = new BaseMessageDatabase().GetPlayerBaseMessageByPlayerId(applyMessage.sendId);
            applyMessage.headimage = playerBaseMessage.PlayerHeadImage;
            applyMessage.applyerName = playerBaseMessage.PlayerName;
            
            PlayerChatMessage playerChatMessage = new PlayerChatMessage();
            playerChatMessage.playerId = model.type;
            playerChatMessage.messageType = model.type;
            playerChatMessage.messageArea = model.area;
            playerChatMessage.messageCommand = model.command;
            playerChatMessage.messageContent = JsonMapper.ToJson(applyMessage);
            playerChatMessage.sentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            new ChatMessageDatabase().InsertPlayerChatMessage(playerChatMessage);
            
            if (flag)
            {
                PlayerChatMessage chatMessage = new ChatMessageDatabase().GetPlayerChatMessage(playerChatMessage.sentTime);
                applyMessage.messageid = chatMessage.messageId;
                model.message = JsonMapper.ToJson(applyMessage);
                foreach (UserToken value in HandlerCenter.centerServerToken)
                {
                    SendtoClient.write(value, model);
                }
            }
          
        }

        /// <summary>
        /// 好友查找处理，并将查新结果转发和请求者
        /// </summary>
        /// <param name="token"></param>
        /// <param name="model">model包含Friendmessage信息</param>
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
                friendMessage.isOnline =false;
                foreach (int item in HandlerCenter.playerOnline) {
                    if (item == friendMessage.friendId) {
                        friendMessage.isOnline = true;
                    }
                }
                friendMessage.isExit = true;
                if (new FriendMessageDatabase().CheachFriendMessage(model.type, friendMessage.friendId))
                {
                    friendMessage.isHave = true;
                }
                else {
                    friendMessage.isHave = false;
                }
            }
            string sendString = JsonMapper.ToJson(friendMessage);

            model.message = sendString;

            SendtoClient.write(token,model);

        }

        /// <summary>
        /// 接受或拒别人的申请请求，并将请求转发给申请者
        /// </summary>
        /// <param name="token"></param>
        /// <param name="model"></param>
        private void AcceptFriendDeal(UserToken token, SocketModel model) {
            Console.WriteLine(model.getMessage<string>());
            ApplyMessage applyMessage = JsonMapper.ToObject<ApplyMessage>(model.getMessage<string>());
            model.type = applyMessage.acceptId;
            new ChatMessageDatabase().DeletePlayerMessageByID(applyMessage.messageid);

            if (applyMessage.isAgree && applyMessage.isApply) {
                return;
            }

            bool flag = false;
            foreach (int item in HandlerCenter.playerOnline)
            {
                if (item == model.type)
                    flag = true;
            }

            PlayerBaseMessage playerBaseMessage = new BaseMessageDatabase().GetPlayerBaseMessageByPlayerId(applyMessage.sendId);
            applyMessage.headimage = playerBaseMessage.PlayerHeadImage;
            applyMessage.applyerName = playerBaseMessage.PlayerName;

            PlayerChatMessage playerChatMessage = new PlayerChatMessage();
            playerChatMessage.playerId = model.type;
            playerChatMessage.messageType = model.type;
            playerChatMessage.messageArea = model.area;
            playerChatMessage.messageCommand = model.command;
            playerChatMessage.messageContent = JsonMapper.ToJson(applyMessage);
            playerChatMessage.sentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            new ChatMessageDatabase().InsertPlayerChatMessage(playerChatMessage);

            if (applyMessage.isAgree)
            {
                PlayerFriendMessage playerFriendMessage = new PlayerFriendMessage();
                playerFriendMessage.playerId = applyMessage.sendId;
                playerFriendMessage.friendId = applyMessage.acceptId;
                new FriendMessageDatabase().InsertFriendMessage(playerFriendMessage);
                playerFriendMessage.playerId = applyMessage.acceptId;
                playerFriendMessage.friendId = applyMessage.sendId;
                new FriendMessageDatabase().InsertFriendMessage(playerFriendMessage);
            }

            
            if (flag)
            {
                PlayerChatMessage chatMessage = new ChatMessageDatabase().GetPlayerChatMessage(playerChatMessage.sentTime);
                applyMessage.messageid = chatMessage.messageId;
                model.message = JsonMapper.ToJson(applyMessage);
                foreach (UserToken value in HandlerCenter.centerServerToken)
                {
                    SendtoClient.write(value, model);
                }
            }
        }

        /// <summary>
        /// 获取好友列表的好友信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="model"></param>
        private void GetFriendDeal(UserToken token, SocketModel model) {
            List<PlayerFriendMessage> friendMessage = new FriendMessageDatabase().GetFriendListByPlayerId(model.getMessage<int>());
            string messageString=null;
            bool flag = true;
            foreach (PlayerFriendMessage item in friendMessage) {
                PlayerBaseMessage playerBaseMessage = new BaseMessageDatabase().GetPlayerBaseMessageByPlayerId(item.friendId);
                FriendMessage message = new FriendMessage();
                message.friendName = playerBaseMessage.PlayerName;
                message.friendId = playerBaseMessage.PlayerId;
                message.headImage = playerBaseMessage.PlayerHeadImage;
                bool temp = false;
                foreach (int value in HandlerCenter.playerOnline) {
                    if (value == item.friendId) {
                        temp = true;
                    }
                }
                if (temp)
                    message.isOnline = true;
                else
                    message.isOnline = false;
                if (flag)
                {
                    messageString = JsonMapper.ToJson(message);
                    flag = false;
                }
                else {
                    messageString += "/" + JsonMapper.ToJson(message);
                }
            }
            model.message = messageString;
            SendtoClient.write(token,model);
        }

        /// <summary>
        /// 好友上下线通知
        /// </summary>
        /// <param name="token"></param>
        /// <param name="model"></param>
        private void InformFriendDeal(UserToken token, SocketModel model) {
            List<PlayerFriendMessage> friendMessage = new FriendMessageDatabase().GetFriendListByPlayerId(model.type);
            OnlineMessage onlineMessage = JsonMapper.ToObject<OnlineMessage>(model.getMessage<string>());
            if (onlineMessage.isOnline)
            {
                HandlerCenter.playerOnline.Add(onlineMessage.onlinePlayerId);
                List<PlayerChatMessage> chatMessageList = new ChatMessageDatabase().GetPlayerChatMessageBuyPlayerId(onlineMessage.onlinePlayerId);
                if (chatMessageList.Count != 0) {
                    foreach (PlayerChatMessage value in chatMessageList) {
                        SocketModel socketModel = new SocketModel();
                        socketModel.type = value.messageType;
                        socketModel.area = value.messageArea;
                        socketModel.command = value.messageArea;
                        ApplyMessage applyMessage = JsonMapper.ToObject<ApplyMessage>(value.messageContent);
                        applyMessage.messageid = value.messageId;
                        socketModel.message = JsonMapper.ToJson(applyMessage);
                        SendtoClient.write(token,socketModel);
                    }
                }
            }
            else {
                foreach (int item in HandlerCenter.playerOnline) {
                    if (item == onlineMessage.onlinePlayerId) {
                        HandlerCenter.playerOnline.Remove(item);
                        break;
                    }
                }
            }
            foreach (PlayerFriendMessage item in friendMessage) {
                model.type = item.friendId;
                foreach (UserToken value in HandlerCenter.centerServerToken) {
                    SendtoClient.write(value,model);
                }
            }
        }

        /// <summary>
        /// 删除好友信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="model"></param>
        private void DeleteFriendDeal(UserToken token, SocketModel model) {
            FriendMessage friendMessage = JsonMapper.ToObject<FriendMessage>(model.getMessage<string>());
            new FriendMessageDatabase().DeleteFriendMessage(model.type,friendMessage.friendId);
            new FriendMessageDatabase().DeleteFriendMessage(friendMessage.friendId,model.type);
            model.message = true;
            SendtoClient.write(token,model);
        }
    }
}
