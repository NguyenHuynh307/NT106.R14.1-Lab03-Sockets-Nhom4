namespace Bai1_Lab3
{
    partial class FormUDPServer
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
            lblPort = new Label();
            tbPort = new TextBox();
            btnListen = new Button();
            lblInfo = new Label();
            lblReceivedMessages = new Label();
            listViewReceivedMessages = new ListView();
            chTime = new ColumnHeader();
            chFrom = new ColumnHeader();
            chText = new ColumnHeader();
            SuspendLayout();
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Font = new Font("Segoe UI", 10F);
            lblPort.Location = new Point(24, 25);
            lblPort.Name = "lblPort";
            lblPort.Text = "Cổng:";
            //
            // tbPort
            //
            tbPort.Font = new Font("Segoe UI", 10F);
            tbPort.Location = new Point(76, 21);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(96, 29);
            tbPort.TabIndex = 1;
            tbPort.Text = "8080";
            //
            // btnListen
            //
            btnListen.Location = new Point(188, 19);
            btnListen.Name = "btnListen";
            btnListen.Size = new Size(170, 34);
            btnListen.TabIndex = 2;
            btnListen.Text = "Bắt đầu lắng nghe";
            btnListen.Click += btnListen_Click;
            UiKit.Primary(btnListen);
            //
            // lblInfo
            //
            lblInfo.AutoSize = true;
            lblInfo.Font = new Font("Segoe UI", 10F);
            lblInfo.ForeColor = UiKit.Muted;
            lblInfo.Location = new Point(374, 25);
            lblInfo.Name = "lblInfo";
            lblInfo.Text = "Chưa lắng nghe.";
            //
            // lblReceivedMessages
            //
            lblReceivedMessages.AutoSize = true;
            lblReceivedMessages.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblReceivedMessages.ForeColor = UiKit.Ink;
            lblReceivedMessages.Location = new Point(24, 72);
            lblReceivedMessages.Name = "lblReceivedMessages";
            lblReceivedMessages.Text = "Tin nhắn nhận được";
            //
            // listViewReceivedMessages
            //
            listViewReceivedMessages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listViewReceivedMessages.Columns.AddRange(new ColumnHeader[] { chTime, chFrom, chText });
            listViewReceivedMessages.Font = new Font("Segoe UI", 10F);
            listViewReceivedMessages.FullRowSelect = true;
            listViewReceivedMessages.GridLines = true;
            listViewReceivedMessages.HideSelection = false;
            listViewReceivedMessages.Location = new Point(24, 100);
            listViewReceivedMessages.Name = "listViewReceivedMessages";
            listViewReceivedMessages.Size = new Size(712, 336);
            listViewReceivedMessages.TabIndex = 4;
            listViewReceivedMessages.UseCompatibleStateImageBehavior = false;
            listViewReceivedMessages.View = View.Details;
            //
            // chTime / chFrom / chText
            //
            chTime.Text = "Thời gian";
            chTime.Width = 90;
            chFrom.Text = "Nguồn gửi";
            chFrom.Width = 170;
            chText.Text = "Nội dung";
            chText.Width = 420;
            //
            // FormUDPServer
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = UiKit.Page;
            ClientSize = new Size(760, 460);
            Controls.Add(listViewReceivedMessages);
            Controls.Add(lblReceivedMessages);
            Controls.Add(lblInfo);
            Controls.Add(btnListen);
            Controls.Add(tbPort);
            Controls.Add(lblPort);
            MinimumSize = new Size(640, 400);
            Name = "FormUDPServer";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bài 1 · UDP Server";
            FormClosing += FormUDPServer_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPort;
        private TextBox tbPort;
        private Button btnListen;
        private Label lblInfo;
        private Label lblReceivedMessages;
        private ListView listViewReceivedMessages;
        private ColumnHeader chTime;
        private ColumnHeader chFrom;
        private ColumnHeader chText;
    }
}
