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
            string get_sql = "select * from tb_playerfriendmessage where playerid="+playerID.ToString();
            MySqlCommand mySqlCommand = new MySqlCommand(get_sql, mySqlConnection);
            MySqlDataReader reader = null;
            try
            {
                mySqlConnection.Open();
                reader = mySqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    PlayerFriendMessage playerFriendMessage = new PlayerFriendMessage();
                    playerFriendMessage.friendId = int.Parse(reader[2].ToString());
                    playerFriendMessage.playerId = playerID;
                    list.Add(playerFriendMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                mySqlCommand.Dispose();
                reader.Close();
                mySqlConnection.Close();
            }

            return list;
        }

        public void InsertFriendMessage(PlayerFriendMessage playerFriendMessage) {

            string insert_sql = "insert into tb_playerfriendmessage(playerid,friendid) values (" + playerFriendMessage.playerId +
               "," + playerFriendMessage.friendId +  ")";
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


        public bool CheachFriendMessage(int playerID, int friendid) {
            List<PlayerFriendMessage> list = new List<PlayerFriendMessage>();
            string get_sql = "select * from tb_playerfriendmessage where playerid=" + playerID.ToString()+ " and friendid = "+friendid.ToString();
            MySqlCommand mySqlCommand = new MySqlCommand(get_sql, mySqlConnection);
            MySqlDataReader reader = null;
            try
            {
                mySqlConnection.Open();
                reader = mySqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    PlayerFriendMessage playerFriendMessage = new PlayerFriendMessage();
                    playerFriendMessage.friendId = int.Parse(reader[2].ToString());
                    playerFriendMessage.playerId = playerID;
                    list.Add(playerFriendMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                mySqlCommand.Dispose();
                reader.Close();
                mySqlConnection.Close();
            }

            if (list.Count > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
