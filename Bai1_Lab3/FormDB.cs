namespace Bai1_Lab3
{
    public partial class FormDB : Form
    {
        public FormDB()
        {
            InitializeComponent();
        }

        private void btnUDPServer_Click(object sender, EventArgs e)
        {
            FormUDPServer serverForm = new FormUDPServer();
            serverForm.Show();
        }

        private void btnUDPClient_Click(object sender, EventArgs e)
        {
            FormUDPClient clientForm = new FormUDPClient();
            clientForm.Show();
        }
    }
}
