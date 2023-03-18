using _15martbarida.Classes;
using _15martbarida.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _15martbarida.Pages
{
    public partial class LoginPage : Form
    {
        LoginDB loginDB;
        public LoginPage()
        {
            InitializeComponent();
            loginDB = new LoginDB();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if(TemporaryMemory.IsLoginned)
            {
                this.Close();
                return;
            }

            if (loginDB.Login(usernameText.Text, userpassText.Text))
            {
            }
            else
            {
                // MessageBox.Show("Giriş başarısız");
            }
            
            this.Close();

 
        }

        private void LoginPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
