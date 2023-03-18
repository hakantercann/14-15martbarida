using _15martbarida.Classes;
using _15martbarida.Databases;
using _15martbarida.Pages;
using _15martbarida.PLC;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;

namespace _15martbarida
{
    public partial class Form1 : Form
    {

        #region değişkneler
        DateTime first_time;
        bool counterIsStart = false;
        int count = 0;
        byte[] data;
        List<DB1Model> models;
        LoginDB loginDB;
        Thread mainThread;
        Thread thr1;
        Thread sqlThread;
        public bool flashorPlc1 = false;
        public bool flashorSQL = false;
        public bool plc1Ready = false;

        #endregion

        #region Form1 instance
        private static Form1 _instance = new Form1();


        public static Form1 Instance
        {
            get
            {
                return _instance;
            }
        }
        private Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            loginDB = new LoginDB();
            mainThread = new Thread(mainMethod);
            sqlThread = new Thread(sqlConnMethod);
            sqlThread.Priority = ThreadPriority.BelowNormal;
            models = new List<DB1Model>();
            mainThread.Priority = ThreadPriority.Highest;

            thr1 = new Thread(method1);
        }
        #endregion

        #region when form1 started

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Form1_Shown(object sender, EventArgs e)
        {

            mainThread.Start();
            thr1.Start();
            sqlThread.Start();
        }
        #endregion


        private void mainMethod()
        {

            while (true)
            {
                try
                {
                    if (Plc1.Instance.client.IsConnected)
                    {
                        if (counterIsStart)
                        {
                            var time = Convert.ToInt32((DateTime.Now - first_time).TotalSeconds);
                            if (time > 99999)
                            {
                                first_time = DateTime.Now;
                            }

                            Plc1.Instance.client.Write(DataType.DataBlock, 1, 12, time);

                        }
                    }
                    else
                    {
                        if (plc1Ready)
                        {
                            Plc1.Instance.Connect();
                        }
                        else
                        {
                            try
                            {
                                new Thread(delegate ()
                                {
                                    try
                                    {
                                        Ping ping = new Ping();

                                        ping.PingCompleted += new PingCompletedEventHandler(PingCompleted1);

                                        ping.SendAsync("192.168.1.178", 500);
                                    }
                                    catch { }
                                }).Start();
                            }
                            catch (PingException)
                            {
                            }
                            catch (NullReferenceException)
                            {
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch { }
            }
        }

        #region Sql Conn ControlThread
        private void sqlConnMethod()
        {
            while (true)
            {

                try
                {
                    SqlConnection conn = Database.GetSqlConnection();
                    if (conn != null)
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                            flashorSQL = !flashorSQL;
                            if (flashorSQL)
                            {
                                sqlState.BackColor = Color.Lime;
                            }
                            else
                            {
                                sqlState.BackColor = Color.Green;
                            }
                        }
                    }
                    else
                    {
                        Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 0, false);
                        flashorSQL = !flashorSQL;
                        if (flashorSQL)
                        {
                            sqlState.BackColor = Color.Orange;
                        }
                        else
                        {
                            sqlState.BackColor = Color.Red;
                        }
                    }
                }
                catch
                {

                    throw;
                }
                Thread.Sleep(2000);
            }
        }

        #endregion





        #region plc conn control and get data Thread
        private void method1()
        {

            while (true)
            {


                if (Plc1.Instance.client.IsConnected)
                {
                    try
                    {
                        data = Plc1.Instance.client.ReadBytes(S7.Net.DataType.DataBlock, 1, 0, 33);

                        var pcAktifBit = S7.Net.Types.Bit.FromByte(data[0], 0);
                        var plcAktifBit = S7.Net.Types.Bit.FromByte(data[0], 1);
                        var receteDegisti = S7.Net.Types.Bit.FromByte(data[0], 2);
                        var receteHatasi = S7.Net.Types.Bit.FromByte(data[0], 3);
                        var rezerve_1 = S7.Net.Types.Bit.FromByte(data[0], 4);
                        var rezerve_2 = S7.Net.Types.Bit.FromByte(data[0], 5);
                        var rezerve_3 = S7.Net.Types.Bit.FromByte(data[0], 6);
                        var gelenReceteNo = S7.Net.Types.Int.FromByteArray(data.Skip(2).Take(2).ToArray());
                        var gidenReceteNo = S7.Net.Types.Int.FromByteArray(data.Skip(4).Take(2).ToArray());
                        var makinaDurumBilgisi = S7.Net.Types.Int.FromByteArray(data.Skip(6).Take(2).ToArray());
                        var cycleTime = S7.Net.Types.DInt.FromByteArray(data.Skip(8).Take(4).ToArray());
                        var counter = S7.Net.Types.DInt.FromByteArray(data.Skip(12).Take(4).ToArray());
                        var ad = S7.Net.Types.String.FromByteArray(data.Skip(18).Take(10).ToArray());
                        var realDeneme = S7.Net.Types.Real.FromByteArray(data.Skip(28).Take(4).ToArray());
                        var kayitYap = S7.Net.Types.Bit.FromByte(data[32], 2);
                        var kayitYapilamadi = S7.Net.Types.Bit.FromByte(data[32], 1);
                        var kayitYapildi = S7.Net.Types.Bit.FromByte(data[32], 0);
                        var _s = pcAktifBit ? dx00Value.BackColor = Color.Green : dx00Value.BackColor = Color.Red;
                        ////if (pcAktifBit) dx00Value.BackColor = Color.Green;
                        ////else dx00Value.BackColor = Color.Red;
                        if (plcAktifBit) dx01Value.BackColor = Color.Green;
                        else dx01Value.BackColor = Color.Red;
                        if (receteDegisti) dx02Value.BackColor = Color.Green;
                        else dx02Value.BackColor = Color.Red;
                        if (receteHatasi) dx03Value.BackColor = Color.Green;
                        else dx03Value.BackColor = Color.Red;
                        if (rezerve_1) dx04Value.BackColor = Color.Green;
                        else dx04Value.BackColor = Color.Red;
                        if (rezerve_2) dx05Value.BackColor = Color.Green;
                        else dx05Value.BackColor = Color.Red;
                        if (rezerve_3) dx06Value.BackColor = Color.Green;
                        else dx06Value.BackColor = Color.Red;
                        var _1 = kayitYap ? kayitYapL.BackColor = Color.Green : kayitYapL.BackColor = Color.Red;
                        var _2 = kayitYapilamadi ? kayitYapilamadiL.BackColor = Color.Green : kayitYapilamadiL.BackColor = Color.Red;
                        var _3 = kayitYapildi ? kayitYapildiL.BackColor = Color.Green : kayitYapildiL.BackColor = Color.Red;
                        dint20.Text = gelenReceteNo.ToString();
                        dint40.Text = gidenReceteNo.ToString();
                        dint60.Text = makinaDurumBilgisi.ToString();
                        ddint80.Text = cycleTime.ToString();
                        ddint120.Text = counter.ToString();
                        dstring160.Text = ad.Trim();
                        real28.Text = realDeneme.ToString();
                        if (kayitYap && !kayitYapildi && !kayitYapilamadi)
                        {
                            string ad1 = "as";
                            SqlConnection conn = Database.GetSqlConnection();
                            try
                            {

                                SqlCommand cmd = new SqlCommand("insert into roles(rolename) values('" + ad1 + "')", conn);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                                Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 32, 0, true);
                            }
                            catch (Exception ex)
                            {
                                Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 32, 1, true);
                                Console.WriteLine(ex.ToString());
                            }
                            finally { conn.Close(); }
                            ////if (pcAktifBit)
                            ////{
                            ////    dx00Value.BackColor = Color.Green;
                            ////}
                            ////else
                            ////{
                            ////    dx00Value.BackColor = Color.Red;
                            ////}

                        }
                        Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 4, true);
                    }
                    catch
                    {
                        try
                        {
                            Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 5, true);
                        }
                        catch
                        {
                            plc1Ready = false;
                        }
                    }
                    flashorPlc1 = !flashorPlc1;
                    if (flashorPlc1)
                    {
                        plc1State.BackColor = Color.Lime;
                    }
                    else
                    {
                        plc1State.BackColor = Color.Green;
                    }

                }
                else
                {
                    flashorPlc1 = !flashorPlc1;
                    if (flashorPlc1)
                    {
                        plc1State.BackColor = Color.Orange;
                    }
                    else
                    {
                        plc1State.BackColor = Color.Red;
                    }
                }

                Thread.Sleep(100);
            }
        }

        #endregion
        private void PingCompleted1(object sender, PingCompletedEventArgs e)
        {
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                plc1Ready = true;

                plc1PingState.Text = "PİNG OK";
                first_time = DateTime.Now;
                counterIsStart = true;
                //  plc1PingReply.Text = "Ping OK";
            }
            else
            {
                plc1Ready = false;
                plc1PingState.Text = "PİNG FAİLURE";
            }
            // plc1PingReply.Text = "Ping Failure";
        }

        #region Write true false to bit 
        private void kayitYapL_Click(object sender, EventArgs e)
        {
            if (kayitYapL.BackColor == Color.Red)
            {
                Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 32, 2, true);
            }
            else
            {
                Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 32, 2, false);
            }
        }

        private void kayitYapildiL_Click(object sender, EventArgs e)
        {
            if (kayitYapildiL.BackColor == Color.Red)
            {
                Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 32, 0, true);
            }
            else
            {
                Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 32, 0, false);
            }
        }

        private void kayitYapilamadiL_Click(object sender, EventArgs e)
        {
            if (kayitYapilamadiL.BackColor == Color.Red)
            {
                Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 32, 1, true);
            }
            else
            {
                Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 32, 1, false);
            }
        }
        private void dx00Value_Click(object sender, EventArgs e)
        {
            try
            {
                if (dx00Value.BackColor == Color.Red)
                {


                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 0, true);
                }
                else
                {
                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 0, false);
                }
            }
            catch { }
        }

        private void dx01Value_Click(object sender, EventArgs e)
        {
            try
            {
                if (dx01Value.BackColor == Color.Red)
                {
                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 1, true);

                }
                else
                {
                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 1, false);
                }
            }
            catch { }
        }

        private void dx02Value_Click(object sender, EventArgs e)
        {
            try
            {
                if (dx02Value.BackColor == Color.Red)
                {
                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 2, true);
                }
                else
                {
                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 2, false);
                }
            }
            catch { }
        }

        private void dx03Value_Click(object sender, EventArgs e)
        {
            try
            {
                if (dx03Value.BackColor == Color.Red)
                {

                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 3, true);
                }
                else
                {
                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 3, false);
                }
            }
            catch { }
        }

        private void dx04Value_Click(object sender, EventArgs e)
        {
            try
            {
                if (dx04Value.BackColor == Color.Red)
                {

                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 4, true);
                }
                else
                {
                    Plc1.Instance.client.Write(S7.Net.DataType.DataBlock, 1, 4, false);
                }
            }
            catch { }
        }

        private void dx05Value_Click(object sender, EventArgs e)
        {
            try
            {
                if (dx05Value.BackColor == Color.Red)
                {
                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 5, true);
                }
                else
                {
                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 5, false);
                }
            }
            catch { }
        }

        private void dx06Value_Click(object sender, EventArgs e)
        {
            try
            {
                if (dx06Value.BackColor == Color.Red)
                {
                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 6, true);
                }
                else
                {
                    Plc1.Instance.client.WriteBit(DataType.DataBlock, 1, 0, 6, false);
                }
            }
            catch { }
        }


        #endregion
        #region menu transfer 

        private void closeSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Session.stopSession();
        }
        private void denemeAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Enum.GetName(typeof(Roles), TemporaryMemory.Role).Equals("admin"))
            {
                Yetki_Deneme yetki_Deneme = new Yetki_Deneme();
                yetki_Deneme.Show();

                /*       foreach (Control crtls in uc_panel.Controls)
                       {
                           crtls.Dispose();
                       }
                       uc_panel.Controls.Add(new UC_denemeAdmin());*/
            }
        }
        private void manuelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show("Çıkmak İstediğinizden Emin misiniz ?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (sonuc == DialogResult.No)
            {
                return;
            }
            ////mainThread.Abort();
            ////thr1.Abort();
            ////sqlThread.Abort();
            ////GC.Collect();
            ////GC.WaitForPendingFinalizers();
            Application.Exit();
            Environment.Exit(0);
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        #endregion

        #region Write data to plc
        #region Write int values
        private void dint20_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Plc1.Instance.client.Write(DataType.DataBlock, 1, 2, Convert.ToInt16(textBox1.Text));
        }



        private void dint40_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Plc1.Instance.client.Write(DataType.DataBlock, 1, 4, Convert.ToInt16(textBox1.Text));
        }

        private void dint60_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            Plc1.Instance.client.Write(DataType.DataBlock, 1, 6, Convert.ToInt16(textBox1.Text));
        }
        #endregion
        #region Write DoubleInt values
        private void ddint80_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Plc1.Instance.client.Write(DataType.DataBlock, 1, 8, Convert.ToInt32(textBox1.Text));
        }



        #endregion

        #region Write String values
        private void dstring160_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            Plc1.Instance.client.Write(DataType.DataBlock, 1, 18, textBox1.Text + "        ");
        }


        #endregion


        #region Write real values
        private void real28_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Plc1.Instance.client.Write(DataType.DataBlock, 1, 28, float.Parse(textBox1.Text));
        }
        #endregion


        #endregion


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Session.stopSession();
            mainThread.Abort();
            thr1.Abort();
            sqlThread.Abort();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}