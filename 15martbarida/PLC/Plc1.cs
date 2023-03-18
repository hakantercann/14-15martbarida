using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S7.Net;
using System.Data;

namespace _15martbarida.PLC
{
    public class Plc1
    {
        public Plc client;
        #region Singleton
        private static readonly Lazy<Plc1> _instance = new Lazy<Plc1>(() => new Plc1());


        public static Plc1 Instance
        { get { return _instance.Value; } }
        
        private Plc1()
        {
            client = new Plc(CpuType.S71200, "192.168.1.178", 0, 1);
        }
        #endregion

        public void Connect()
        {
            ////while(true)
            ////{
            ////    try
            ////    {
            ////        PingReply reply = p.Send("127.0.0.1");
            ////       // if(reply.Status) 
            ////      }
            ////    catch (PingException)
            ////    {
            ////    }
            ////}
            client.Open(); 
        }

        public void Disconnect()
        {
           client.Close();
        }
    }
}
