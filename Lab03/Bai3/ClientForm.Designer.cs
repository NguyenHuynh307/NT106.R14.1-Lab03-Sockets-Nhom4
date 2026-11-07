namespace Bai3
{
    partial class ClientForm
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
            txtIp = new TextBox();
            txtPort = new TextBox();
            txtMessage = new TextBox();
            btnConnect = new Button();
            btnDisconnect = new Button();
            btnSend = new Button();
            lblIp = new Label();
            lblPort = new Label();
            lblMessage = new Label();
            SuspendLayout();
            // 
            // txtIp
            // 
            txtIp.Location = new Point(162, 34);
            txtIp.Name = "txtIp";
            txtIp.Size = new Size(212, 27);
            txtIp.TabIndex = 0;
            txtIp.Text = "127.0.0.1";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(162, 115);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(212, 27);
            txtPort.TabIndex = 1;
            txtPort.Text = "8080";
            // 
            // txtMessage
            // 
            txtMessage.Location = new Point(152, 283);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(218, 27);
            txtMessage.TabIndex = 2;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(152, 195);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(94, 29);
            btnConnect.TabIndex = 3;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new Point(283, 195);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(94, 29);
            btnDisconnect.TabIndex = 4;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(152, 341);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(218, 29);
            btnSend.TabIndex = 5;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // lblIp
            // 
            lblIp.AutoSize = true;
            lblIp.Location = new Point(12, 41);
            lblIp.Name = "lblIp";
            lblIp.Size = new Size(126, 20);
            lblIp.TabIndex = 6;
            lblIp.Text = "Server IP Address:";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(12, 122);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(38, 20);
            lblPort.TabIndex = 7;
            lblPort.Text = "Port:";
            lblPort.Click += label2_Click;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Location = new Point(12, 290);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(70, 20);
            lblMessage.TabIndex = 8;
            lblMessage.Text = "Message:";
            // 
            // ClientForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblMessage);
            Controls.Add(lblPort);
            Controls.Add(lblIp);
            Controls.Add(btnSend);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            Controls.Add(txtMessage);
            Controls.Add(txtPort);
            Controls.Add(txtIp);
            Name = "ClientForm";
            Text = "ClientForm";
            Load += ClientForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtIp;
        private TextBox txtPort;
        private TextBox txtMessage;
        private Button btnConnect;
        private Button btnDisconnect;
        private Button btnSend;
        private Label lblIp;
        private Label lblPort;
        private Label lblMessage;
    }
}