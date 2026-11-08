using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bai6
{
    public partial class Form1 : Form
    {
        private ServerForm? serverForm;

        public Form1()
        {
            InitializeComponent();
            CreateInterface();
        }

        private void CreateInterface()
        {
            Text = "Bài 6 · Chat Room";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 246, 249);
            ClientSize = new Size(560, 240);

            Label title = new Label();
            title.Text = "Bài 6 · Chat Room (TCP)";
            title.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(32, 41, 56);
            title.AutoSize = true;
            title.Location = new Point(32, 24);

            Label hint = new Label();
            hint.Text = "Mở Server và bấm Start trước, sau đó mở Client để vào phòng chat (cổng 8080).";
            hint.Font = new Font("Segoe UI", 10F);
            hint.ForeColor = Color.FromArgb(100, 112, 130);
            hint.AutoSize = true;
            hint.Location = new Point(34, 66);

            Button btnServer = new Button();
            btnServer.Text = "Mở Server";
            btnServer.Size = new Size(236, 64);
            btnServer.Location = new Point(34, 118);
            btnServer.FlatStyle = FlatStyle.Flat;
            btnServer.FlatAppearance.BorderSize = 0;
            btnServer.BackColor = Color.FromArgb(37, 99, 160);
            btnServer.ForeColor = Color.White;
            btnServer.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnServer.Cursor = Cursors.Hand;
            btnServer.UseVisualStyleBackColor = false;

            Button btnClient = new Button();
            btnClient.Text = "Mở Client";
            btnClient.Size = new Size(236, 64);
            btnClient.Location = new Point(290, 118);
            btnClient.FlatStyle = FlatStyle.Flat;
            btnClient.FlatAppearance.BorderSize = 1;
            btnClient.FlatAppearance.BorderColor = Color.FromArgb(190, 198, 210);
            btnClient.BackColor = Color.White;
            btnClient.ForeColor = Color.FromArgb(32, 41, 56);
            btnClient.Font = new Font("Segoe UI", 10F);
            btnClient.Cursor = Cursors.Hand;
            btnClient.UseVisualStyleBackColor = false;

            btnServer.Click += BtnServer_Click;
            btnClient.Click += BtnClient_Click;

            Controls.Add(title);
            Controls.Add(hint);
            Controls.Add(btnServer);
            Controls.Add(btnClient);
        }

        private void BtnServer_Click(object? sender, EventArgs e)
        {
            if (serverForm == null || serverForm.IsDisposed)
            {
                serverForm = new ServerForm();
            }

            serverForm.Show();
            serverForm.BringToFront();
        }

        private void BtnClient_Click(object? sender, EventArgs e)
        {
            ClientForm clientForm = new ClientForm();
            clientForm.Show();
        }
    }
}