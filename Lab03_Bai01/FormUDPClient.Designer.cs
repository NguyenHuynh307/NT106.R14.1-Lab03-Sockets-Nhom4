namespace Bai1_Lab3
{
    partial class FormUDPClient
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
            lblIP = new Label();
            tbIP = new TextBox();
            lblPort = new Label();
            tbPort = new TextBox();
            lblMessage = new Label();
            rtbMessage = new RichTextBox();
            btnSend = new Button();
            lblStatus = new Label();
            SuspendLayout();
            //
            // lblIP
            //
            lblIP.AutoSize = true;
            lblIP.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblIP.ForeColor = UiKit.Ink;
            lblIP.Location = new Point(24, 20);
            lblIP.Name = "lblIP";
            lblIP.Text = "IP máy nhận (Server)";
            //
            // tbIP
            //
            tbIP.Font = new Font("Segoe UI", 10F);
            tbIP.Location = new Point(24, 46);
            tbIP.Name = "tbIP";
            tbIP.Size = new Size(380, 29);
            tbIP.TabIndex = 0;
            tbIP.Text = "127.0.0.1";
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPort.ForeColor = UiKit.Ink;
            lblPort.Location = new Point(424, 20);
            lblPort.Name = "lblPort";
            lblPort.Text = "Cổng";
            //
            // tbPort
            //
            tbPort.Font = new Font("Segoe UI", 10F);
            tbPort.Location = new Point(424, 46);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(120, 29);
            tbPort.TabIndex = 1;
            tbPort.Text = "8080";
            //
            // lblMessage
            //
            lblMessage.AutoSize = true;
            lblMessage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMessage.ForeColor = UiKit.Ink;
            lblMessage.Location = new Point(24, 94);
            lblMessage.Name = "lblMessage";
            lblMessage.Text = "Nội dung tin nhắn";
            //
            // rtbMessage
            //
            rtbMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtbMessage.Font = new Font("Segoe UI", 10.5F);
            rtbMessage.Location = new Point(24, 120);
            rtbMessage.Name = "rtbMessage";
            rtbMessage.Size = new Size(712, 250);
            rtbMessage.TabIndex = 2;
            rtbMessage.Text = "";
            //
            // btnSend
            //
            btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSend.Location = new Point(24, 388);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(150, 38);
            btnSend.TabIndex = 3;
            btnSend.Text = "Gửi";
            btnSend.Click += btnSend_Click;
            UiKit.Primary(btnSend);
            //
            // lblStatus
            //
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10F);
            lblStatus.ForeColor = UiKit.Muted;
            lblStatus.Location = new Point(190, 397);
            lblStatus.Name = "lblStatus";
            lblStatus.Text = "Sẵn sàng.";
            //
            // FormUDPClient
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = UiKit.Page;
            ClientSize = new Size(760, 450);
            Controls.Add(lblStatus);
            Controls.Add(rtbMessage);
            Controls.Add(btnSend);
            Controls.Add(lblMessage);
            Controls.Add(tbPort);
            Controls.Add(lblPort);
            Controls.Add(tbIP);
            Controls.Add(lblIP);
            MinimumSize = new Size(620, 400);
            Name = "FormUDPClient";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bài 1 · UDP Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblIP;
        private TextBox tbIP;
        private Label lblPort;
        private TextBox tbPort;
        private Label lblMessage;
        private RichTextBox rtbMessage;
        private Button btnSend;
        private Label lblStatus;
    }
}
