namespace Bai2_Lab3
{
    partial class FormListen
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
            btnListen = new Button();
            lblInfo = new Label();
            listViewMessage = new ListView();
            chTime = new ColumnHeader();
            chFrom = new ColumnHeader();
            chText = new ColumnHeader();
            SuspendLayout();
            //
            // btnListen
            //
            btnListen.Location = new Point(24, 19);
            btnListen.Name = "btnListen";
            btnListen.Size = new Size(170, 34);
            btnListen.TabIndex = 0;
            btnListen.Text = "Bắt đầu lắng nghe";
            btnListen.Click += btnListen_Click;
            UiKit.Primary(btnListen);
            //
            // lblInfo
            //
            lblInfo.AutoSize = true;
            lblInfo.Font = new Font("Segoe UI", 10F);
            lblInfo.ForeColor = UiKit.Muted;
            lblInfo.Location = new Point(212, 25);
            lblInfo.Name = "lblInfo";
            lblInfo.Text = "Cổng 8080 (TCP). Dùng lệnh telnet để gửi tin tới đây.";
            //
            // listViewMessage
            //
            listViewMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listViewMessage.Columns.AddRange(new ColumnHeader[] { chTime, chFrom, chText });
            listViewMessage.Font = new Font("Segoe UI", 10F);
            listViewMessage.FullRowSelect = true;
            listViewMessage.GridLines = true;
            listViewMessage.HideSelection = false;
            listViewMessage.Location = new Point(24, 70);
            listViewMessage.Name = "listViewMessage";
            listViewMessage.Size = new Size(712, 366);
            listViewMessage.TabIndex = 1;
            listViewMessage.UseCompatibleStateImageBehavior = false;
            listViewMessage.View = View.Details;
            //
            // chTime / chFrom / chText
            //
            chTime.Text = "Thời gian";
            chTime.Width = 90;
            chFrom.Text = "Nguồn gửi";
            chFrom.Width = 150;
            chText.Text = "Nội dung";
            chText.Width = 440;
            //
            // FormListen
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = UiKit.Page;
            ClientSize = new Size(760, 460);
            Controls.Add(listViewMessage);
            Controls.Add(lblInfo);
            Controls.Add(btnListen);
            MinimumSize = new Size(640, 400);
            Name = "FormListen";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bài 2 · Lắng nghe Telnet";
            FormClosing += FormListen_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnListen;
        private Label lblInfo;
        private ListView listViewMessage;
        private ColumnHeader chTime;
        private ColumnHeader chFrom;
        private ColumnHeader chText;
    }
}
