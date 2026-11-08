namespace Bai3
{
    partial class ClientForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblIp = new Label();
            txtIp = new TextBox();
            lblPort = new Label();
            txtPort = new TextBox();
            btnConnect = new Button();
            btnDisconnect = new Button();
            lblStatus = new Label();
            lblMessage = new Label();
            txtMessage = new TextBox();
            btnSend = new Button();
            SuspendLayout();
            //
            // lblIp
            //
            lblIp.AutoSize = true;
            lblIp.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblIp.ForeColor = UiKit.Ink;
            lblIp.Location = new Point(24, 20);
            lblIp.Name = "lblIp";
            lblIp.Text = "IP của Server";
            //
            // txtIp
            //
            txtIp.Font = new Font("Segoe UI", 10F);
            txtIp.Location = new Point(24, 46);
            txtIp.Name = "txtIp";
            txtIp.Size = new Size(300, 29);
            txtIp.TabIndex = 0;
            txtIp.Text = "127.0.0.1";
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPort.ForeColor = UiKit.Ink;
            lblPort.Location = new Point(344, 20);
            lblPort.Name = "lblPort";
            lblPort.Text = "Cổng";
            //
            // txtPort
            //
            txtPort.Font = new Font("Segoe UI", 10F);
            txtPort.Location = new Point(344, 46);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(110, 29);
            txtPort.TabIndex = 1;
            txtPort.Text = "8080";
            //
            // btnConnect
            //
            btnConnect.Location = new Point(24, 96);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(130, 36);
            btnConnect.TabIndex = 2;
            btnConnect.Text = "Kết nối";
            btnConnect.Click += btnConnect_Click;
            UiKit.Primary(btnConnect);
            //
            // btnDisconnect
            //
            btnDisconnect.Location = new Point(166, 96);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(130, 36);
            btnDisconnect.TabIndex = 3;
            btnDisconnect.Text = "Ngắt kết nối";
            btnDisconnect.Click += btnDisconnect_Click;
            UiKit.Secondary(btnDisconnect);
            //
            // lblStatus
            //
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10F);
            lblStatus.ForeColor = UiKit.Muted;
            lblStatus.Location = new Point(312, 105);
            lblStatus.Name = "lblStatus";
            lblStatus.Text = "Chưa kết nối.";
            //
            // lblMessage
            //
            lblMessage.AutoSize = true;
            lblMessage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMessage.ForeColor = UiKit.Ink;
            lblMessage.Location = new Point(24, 158);
            lblMessage.Name = "lblMessage";
            lblMessage.Text = "Tin nhắn";
            //
            // txtMessage
            //
            txtMessage.Font = new Font("Segoe UI", 10F);
            txtMessage.Location = new Point(24, 184);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(430, 29);
            txtMessage.TabIndex = 4;
            //
            // btnSend
            //
            btnSend.Location = new Point(24, 228);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(130, 36);
            btnSend.TabIndex = 5;
            btnSend.Text = "Gửi";
            btnSend.Click += btnSend_Click;
            UiKit.Primary(btnSend);
            //
            // ClientForm
            //
            AcceptButton = btnSend;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = UiKit.Page;
            ClientSize = new Size(480, 290);
            Controls.Add(btnSend);
            Controls.Add(txtMessage);
            Controls.Add(lblMessage);
            Controls.Add(lblStatus);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            Controls.Add(txtPort);
            Controls.Add(lblPort);
            Controls.Add(txtIp);
            Controls.Add(lblIp);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ClientForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bài 3 · TCP Client";
            Load += ClientForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblIp;
        private TextBox txtIp;
        private Label lblPort;
        private TextBox txtPort;
        private Button btnConnect;
        private Button btnDisconnect;
        private Label lblStatus;
        private Label lblMessage;
        private TextBox txtMessage;
        private Button btnSend;
    }
}
