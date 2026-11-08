namespace Bai3
{
    partial class ServerForm
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
            listViewCommand = new ListView();
            chTime = new ColumnHeader();
            columnHeader1 = new ColumnHeader();
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
            lblInfo.Text = "Cổng 8080 (TCP). Mỗi lần chỉ nhận 1 Client.";
            //
            // listViewCommand
            //
            listViewCommand.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listViewCommand.Columns.AddRange(new ColumnHeader[] { chTime, columnHeader1 });
            listViewCommand.Font = new Font("Segoe UI", 10F);
            listViewCommand.FullRowSelect = true;
            listViewCommand.GridLines = true;
            listViewCommand.HideSelection = false;
            listViewCommand.Location = new Point(24, 70);
            listViewCommand.Name = "listViewCommand";
            listViewCommand.Size = new Size(712, 366);
            listViewCommand.TabIndex = 1;
            listViewCommand.UseCompatibleStateImageBehavior = false;
            listViewCommand.View = View.Details;
            listViewCommand.SelectedIndexChanged += listViewCommand_SelectedIndexChanged;
            //
            // chTime / columnHeader1
            //
            chTime.Text = "Thời gian";
            chTime.Width = 90;
            columnHeader1.Text = "Thông báo từ Server";
            columnHeader1.Width = 590;
            //
            // ServerForm
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = UiKit.Page;
            ClientSize = new Size(760, 460);
            Controls.Add(listViewCommand);
            Controls.Add(lblInfo);
            Controls.Add(btnListen);
            MinimumSize = new Size(640, 400);
            Name = "ServerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bài 3 · TCP Server";
            Load += ServerForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnListen;
        private Label lblInfo;
        private ListView listViewCommand;
        private ColumnHeader chTime;
        private ColumnHeader columnHeader1;
    }
}
