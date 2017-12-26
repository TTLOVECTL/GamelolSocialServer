using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseConnection.Database;
using MySql.Data.MySqlClient;
using DatabaseConnection.DataMessage;
namespace DatabaseConnection.Database
{
    /// <summary>
    /// 数据库处理：聊天信息
    /// </summary>
    public class ChatMessageDatabase
    {
        private MySqlConnection mySqlConnection = null;

        public ChatMessageDatabase() {
            mySqlConnection = DatabaseConnnection.Instcance.GetMyConnection();
        }

        /// <summary>
        /// 根据指定玩家获取器其所有的聊天信息
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public List<PlayerChatMessage> GetPlayerChatMessageBuyPlayerId(int playerId) {
            List<PlayerChatMessage> chatMessageList = new List<PlayerChatMessage>();
            string get_sql = "select * from tb_playerchatmessage where playerid=" + playerId.ToString();
            MySqlCommand mySqlCommand = new MySqlCommand(get_sql, mySqlConnection);
            MySqlDataReader reader = null;
            try
            {
                mySqlConnection.Open();
                reader = mySqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    PlayerChatMessage playerChatMessage = new PlayerChatMessage();
                    playerChatMessage.messageId = int.Parse(reader[0].ToString());
                    playerChatMessage.playerId = int.Parse(reader[1].ToString());
                    playerChatMessage.messageType = int.Parse(reader[2].ToString());
                    playerChatMessage.messageArea = int.Parse(reader[3].ToString());
                    playerChatMessage.messageCommand = int.Parse(reader[4].ToString());
                    playerChatMessage.messageContent = reader[5].ToString();
                    playerChatMessage.sentTime = reader[6].ToString();
                    chatMessageList.Add(playerChatMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                mySqlCommand.Dispose();
                reader.Close();
                mySqlConnection.Close();
            }
            return chatMessageList;
        }

        /// <summary>
        /// 向数据库中插入玩家的聊天信息
        /// </summary>
        /// <param name="playerChatMessage"></param>
        public void InsertPlayerChatMessage(PlayerChatMessage playerChatMessage) {
            string insert_sql = "insert into tb_playerchatmessage(playerid,messagetype,messagearea," +
                "messagecommmand,messagecontent,sendtime) values (" + playerChatMessage.playerId + "," +
                playerChatMessage.messageType + "," + playerChatMessage.messageArea + "," + playerChatMessage.messageCommand +
                ",'" + playerChatMessage.messageContent + "','" + playerChatMessage.sentTime + "')";
            MySqlCommand mySqlCommand = new MySqlCommand(insert_sql, mySqlConnection);
            try
            {
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                mySqlConnection.Close();
                mySqlCommand.Dispose();
            }

        }

        /// <summary>
        /// 删除指定玩家的聊天信息
        /// </summary>
        /// <param name="playerid"></param>
        public void DeletePlayerChatMessage(int playerid) {
            string delete_sql = "delete from tb_playerchatmessage where playerid=" + playerid.ToString();
            MySqlCommand cmd = new MySqlCommand(delete_sql, mySqlConnection);
            try
            {
                mySqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 删除指定ID的信息
        /// </summary>
        /// <param name="id"></param>
        public void DeletePlayerMessageByID(int id)
        {

            string delete_sql = "delete from tb_playerchatmessage where id=" + id.ToString();
            MySqlCommand cmd = new MySqlCommand(delete_sql, mySqlConnection);
            try
            {
                mySqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentMessage"></param>
        /// <returns></returns>
        public PlayerChatMessage GetPlayerChatMessage(string sendtime) {
            PlayerChatMessage playerChatMessage = null;
            string get_sql = "select * from tb_playerchatmessage where sendtime='" + sendtime+"'";
            MySqlCommand mySqlCommand = new MySqlCommand(get_sql, mySqlConnection);
            MySqlDataReader reader = null;
            try
            {
                mySqlConnection.Open();
                reader = mySqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    playerChatMessage = new PlayerChatMessage();
                    playerChatMessage.messageId = int.Parse(reader[0].ToString());
                    playerChatMessage.playerId = int.Parse(reader[1].ToString());
                    playerChatMessage.messageType = int.Parse(reader[2].ToString());
                    playerChatMessage.messageArea = int.Parse(reader[3].ToString());
                    playerChatMessage.messageCommand = int.Parse(reader[4].ToString());
                    playerChatMessage.messageContent = reader[5].ToString();
                    playerChatMessage.sentTime = reader[6].ToString();
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[GetPlayerChatMessage]" + ex.Message);

            }
            finally
            {
                mySqlCommand.Dispose();
                reader.Close();
                mySqlConnection.Close();
            }
            return playerChatMessage;
        }
    }
}
