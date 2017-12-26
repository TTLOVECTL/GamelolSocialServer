using System;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using AceNetFrame.ace;
using AceNetFrame.ace.auto;
using System.Threading;
using GamelolSocialServer.Util;
using LitJson;
using DatabaseConnection.Database;
using DatabaseConnection.DataMessage;
using System.Collections.Generic;

namespace GamelolSocialServer
{
    class Program
    {

        static void Main(string[] args)
        {

            try
            {
                NetServer server = new NetServer(1000);
                server.lengthEncode = LengthEncoding.encode;
                server.lengthDecode = LengthEncoding.decode;
                server.serDecode = MessageEncoding.Decode;
                server.serEncode = MessageEncoding.Encode;
                server.center = new HandlerCenter();
                server.init();
                server.Start(int.Parse(ConfigurationSetting.GetConfigurationValue("socialServerPort")));
            }
            catch (Exception e)
            {
                Console.WriteLine("Server Error " + e.TargetSite);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.Message);
            }
            

            while (true)
            {
                Thread.Sleep(60000000);
            }
            

            //SystemLogSystem.Instance.SendMessageToLogServer();



        }


    }
}
