using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbGuestList
{
    public static class Logger
    {
        private static log4net.ILog Log { get; set; }
        static Logger()
        {
            Log = log4net.LogManager.GetLogger(typeof(Logger));
        }
        public static void Error(object msg)
        {
            Log.Error(msg);
            //EmailLayer.SendError("airplus@kustotech.in", "siva@kustotech.in", "saran@kustotech.in", "Airplus Error", msg.ToString()).Wait();
        }

        public static void Error(object msg, Exception ex)
        {
            Log.Error(msg, ex);
            //EmailLayer.SendError("airplus@kustotech.in","siva@kustotech.in","saran@kustotech.in","Airplus Error",mailmsg).Wait();
        }

        public static void Error(Exception ex)
        {
            Log.Error(ex.Message, ex);
            //EmailLayer.SendError("airplus@kustotech.in", "siva@kustotech.in", "saran@kustotech.in", "Airplus Error", mailmsg).Wait();
        }

        public static void Info(object msg)
        {
            Log.Info(msg);
        }
    }
}
