using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DatabaseConnection.DataMessage;

namespace DatabaseConnection.Database
{
    public class FriendMessageDatabase
    {
        private MySqlConnection mySqlConnection = null;

        public FriendMessageDatabase()
        {
            mySqlConnection =DatabaseConnnection.Instcance.GetMyConnection();
        }

        public List<PlayerFriendMessage> GetFriendListByPlayerId(int playerID) {
            List<PlayerFriendMessage> list = new List<PlayerFriendMessage>();

            return list;
        }

        public void InsertFriendMessage(PlayerFriendMessage playerFriendMessage) {

            string insert_sql = "insert into tb_playerfriendmessage(playerid,frienid) values(" + playerFriendMessage.playerId +
               ",'" + playerFriendMessage.friendId +  ")";
            MySqlCommand cmd = new MySqlCommand(insert_sql, mySqlConnection);
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

        public void DeleteFriendMessage(int playerid,int friendid) {
            string delete_sql = "delete from tb_playerfriendmessage where playerid=" + playerid.ToString()+ " and friendid="+friendid.ToString();
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
    }
}
