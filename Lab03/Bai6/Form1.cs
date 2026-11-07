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
            Text = "Bai 6 - Chat Room";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(450, 300);

            Label title = new Label();
            title.Text = "TCP CHAT ROOM";
            title.Font = new Font("Arial", 18, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(125, 45);

            Button btnServer = new Button();
            btnServer.Text = "Open Server";
            btnServer.Size = new Size(180, 45);
            btnServer.Location = new Point(125, 110);

            Button btnClient = new Button();
            btnClient.Text = "Open Client";
            btnClient.Size = new Size(180, 45);
            btnClient.Location = new Point(125, 175);

            btnServer.Click += BtnServer_Click;
            btnClient.Click += BtnClient_Click;

            Controls.Add(title);
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