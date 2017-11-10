using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseConnection.DataMessage;
using MySql.Data.MySqlClient;

namespace DatabaseConnection.Database
{
    /// <summary>
    /// 玩家符文信息数据库的读写
    /// </summary>
    public class InscriptionMessageDatabase
    {
        private MySqlConnection mySqlConnection = null;

        public InscriptionMessageDatabase()
        {
            mySqlConnection = DatabaseConnnection.Instcance.GetMyConnection();
        }

        /// <summary>
        /// 根据玩家Id获取玩家拥有的符文
        /// </summary>
        /// <param name="playerid"></param>
        /// <returns></returns>
        public List<PlayerInscriptionMessage> GetPlayerInscriptionListById(int playerid) {
            List<PlayerInscriptionMessage> inscriptionList =new List<PlayerInscriptionMessage>();
            string get_sql = "select * from tb_playerinscriptionmessage where playerid=" + playerid.ToString() +" and inscriptionnumber > 0";
            MySqlCommand mySqlCommand = new MySqlCommand(get_sql, mySqlConnection);
            MySqlDataReader reader = null;
            try
            {
                mySqlConnection.Open();
                reader = mySqlCommand.ExecuteReader();
                while (reader.Read()) {
                    PlayerInscriptionMessage item = new PlayerInscriptionMessage();
                    item.PlayerId = int.Parse(reader[1].ToString());
                    item.InscriptionId = int.Parse(reader[2].ToString());
                    item.InscriptionNumber = int.Parse(reader[3].ToString());
                    item.InscriptionUserNumber = int.Parse(reader[3].ToString());
                    inscriptionList.Add(item);
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
            return inscriptionList;
        }

        /// <summary>
        /// 更新指定玩家，指定符文的数量
        /// </summary>
        /// <param name="playerid"></param>
        /// <param name="inscriptionid"></param>
        /// <param name="inscriptionnumber"></param>
        public void UpdatePlayerInscriptionNumber(int playerid, int inscriptionid, int inscriptionnumber) {
            string update_sql = "update tb_playerinscriptionmessage set inscriptionnumber=inscriptionnumber +" + inscriptionnumber + 
                " where playerid =" + playerid.ToString() + " and inscriptionid ="+inscriptionid.ToString();
            MySqlCommand cmd = new MySqlCommand(update_sql, mySqlConnection);
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
        /// 更新指定玩家，指定符文使用的数量
        /// </summary>
        /// <param name="playerid"></param>
        /// <param name="inscriptionid"></param>
        /// <param name="inscriptionnumber"></param>
        public void UpdatePlayerInscriptionUseNumber(int playerid, int inscriptionid, int inscriptionnumber)
        {
            string update_sql = "update tb_playerinscriptionmessage set inscriptionusenumber = inscriptionusenumber + " + 
                inscriptionnumber + "where playerid =" + playerid.ToString() + " and inscriptionid =" + inscriptionid.ToString();
            MySqlCommand cmd = new MySqlCommand(update_sql, mySqlConnection);
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
        /// 向数据库中插入数据
        /// </summary>
        /// <param name="playerid"></param>
        /// <param name="playerInscriptionMessage"></param>
        public void InserPlayerInscription(int playerid, PlayerInscriptionMessage playerInscriptionMessage) {
            string get_sql = "insert into tb_playerinscriptionmessage(playerid,inscriptionid,inscriptionnumber,inscriptionusenumber) values" +
                "("+playerid+","+playerInscriptionMessage.InscriptionId+","+playerInscriptionMessage.InscriptionNumber+",0)";
            MySqlCommand mySqlCommand = new MySqlCommand(get_sql, mySqlConnection);
            MySqlDataReader reader = null;
            bool flag = false;
            mySqlConnection.Open();
            try
            {
                reader = mySqlCommand.ExecuteReader();
                if (reader == null)
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                reader.Close();
            }
            if (flag)
            {
                UpdatePlayerInscriptionNumber(playerid, playerInscriptionMessage.InscriptionId, playerInscriptionMessage.InscriptionNumber);
            }
            else {
                string insert_sql = "insert into tb_playerinscriptionmessage(playerid,inscriptionid,inscriptionnumber, inscriptionusenumber) values(" +
                    playerid +"," + playerInscriptionMessage.InscriptionId + "," + playerInscriptionMessage.InscriptionNumber + ",0)";
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
        }

        /// <summary>
        /// 查询指定玩家指定符文的信息是否存在
        /// </summary>
        /// <param name="playerid"></param>
        /// <param name="inscriptionId"></param>
        /// <returns></returns>
        public bool FindPlayerInscription(int playerid, int inscriptionId) {
            string get_sql = "select * from tb_playerinscriptionmessage where playerid=" + playerid.ToString() + " and inscriptionid="+inscriptionId;
            MySqlCommand mySqlCommand = new MySqlCommand(get_sql, mySqlConnection);
            MySqlDataReader reader = null;
            int count = 0;
            try
            {
                mySqlConnection.Open();
                reader = mySqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    count++;
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
            if (count > 0)
            {

                return true;
            }
            else {

                return false;
            }
        }


        public void UpdatePlayerInscription(int playerid, int inscriptionid,int inscriptionNumber ) {
            if (FindPlayerInscription(playerid, inscriptionid)) {
                UpdatePlayerInscriptionNumber(playerid,inscriptionid,inscriptionNumber);
            }
            else{
                PlayerInscriptionMessage playerInscriptionMessage = new PlayerInscriptionMessage();
                playerInscriptionMessage.PlayerId = playerid;
                playerInscriptionMessage.InscriptionId = inscriptionid;
                playerInscriptionMessage.InscriptionNumber = inscriptionNumber;
                playerInscriptionMessage.InscriptionUserNumber = 0;
                InserPlayerInscription(playerid,playerInscriptionMessage);
            }

        }
    }
}
