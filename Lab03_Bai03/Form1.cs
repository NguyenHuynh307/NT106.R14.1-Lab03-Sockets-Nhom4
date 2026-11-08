namespace Bai3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpenServer_Click(object sender, EventArgs e)
        {
            ServerForm server = new ServerForm();
            server.Show();
        }

        private void btnOpenClient_Click(object sender, EventArgs e)
        {
            ClientForm client = new ClientForm();
            client.Show();

        }
    }
}
