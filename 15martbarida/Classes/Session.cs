using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _15martbarida.Classes
{
    public static class Session
    {
        public static int i { get; set; }
        private static Thread thread;
        private static int timeout;
        ////public static bool isStart = false;
        public static void startSession()
        {
            ////if (isStart)
            ////{
            ////    return;
            ////}
            switch (TemporaryMemory.Role)
            {
                case Roles.none:
                    timeout = 0;
                    break;
                case Roles._operator:
                    timeout = 6000;
                    break;
                case Roles.admin:
                    timeout = 6000;
                    break;
                case Roles.bakımcı:
                    timeout = 6000;
                    break;
            }
            thread = new Thread(SessionCallBack);
            thread.Start();
            Form1.Instance.sessionInfoLabel.Text = Enum.GetName(typeof(Roles), TemporaryMemory.Role);
            i = 0;

            ////isStart = true;
        }

        private static void SessionCallBack()
        {
            while (true)
            {
                i++;
                Form1.Instance.sessionTime.Text = (600 - (i / 10)).ToString();
                Thread.Sleep(90);
                if (i > timeout)
                {
                    Console.WriteLine("Oturum Sonlandırıldı");

                    stopSession();
                }
            }
        }
        public static void stopSession()
        {
            //emporaryMemory.VideoCaptureDevice = null;
            if (!TemporaryMemory.IsLoginned)
            {
                return;
            }
            TemporaryMemory.Role = Roles.none;
            TemporaryMemory.IsLoginned = false;
            TemporaryMemory.name = string.Empty;
             i = 0;
            Form1.Instance.sessionTime.Text = i.ToString();
            Form1.Instance.sessionInfoLabel.Text = "None";
            ////isStart = false;
            ////Form1.Instance.isCaptured = false;
            ////Form1.Instance.uc_panel.Controls.Clear();
            thread.Abort();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
