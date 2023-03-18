using System;

namespace _15martbarida.Pages
{
    partial class LoginPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.usernameText = new System.Windows.Forms.TextBox();
            this.userpassText = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // usernameText
            // 
            this.usernameText.Location = new System.Drawing.Point(44, 47);
            this.usernameText.Name = "usernameText";
            this.usernameText.Size = new System.Drawing.Size(107, 22);
            this.usernameText.TabIndex = 0;
            // 
            // userpassText
            // 
            this.userpassText.Location = new System.Drawing.Point(44, 95);
            this.userpassText.Name = "userpassText";
            this.userpassText.Size = new System.Drawing.Size(107, 22);
            this.userpassText.TabIndex = 1;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(44, 157);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(69, 31);
            this.loginButton.TabIndex = 2;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // LoginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(175, 231);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.userpassText);
            this.Controls.Add(this.usernameText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LoginPage";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginPage_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox usernameText;
        private System.Windows.Forms.TextBox userpassText;
        private System.Windows.Forms.Button loginButton;
    }
}