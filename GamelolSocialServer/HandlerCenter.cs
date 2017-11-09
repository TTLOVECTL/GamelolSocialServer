using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AceNetFrame.ace;
using AceNetFrame.ace.auto;
namespace GamelolSocialServer
{
    public class HandlerCenter : AbsHandleCenter
    {
        private HanderInterface authenticationHandler = null;
        private HanderInterface propetryHandler = null;
        private HanderInterface matchHandle = null;
        private HanderInterface socialHandler = null;

        public static SortedDictionary<int, UserToken> playerToken = new SortedDictionary<int, UserToken>();

        public HandlerCenter() {
            
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
           

            }
        }
    }
}
