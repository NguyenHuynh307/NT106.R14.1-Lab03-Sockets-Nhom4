using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
namespace Bai3
{
    public partial class ClientForm : Form
    {

        public ClientForm()
        {
            InitializeComponent();
        }
        private TcpClient tcpClient;
        private NetworkStream ns;
        private void ClientForm_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string ip = txtIp.Text.Trim();
                int port = int.Parse(txtPort.Text.Trim());

                IPAddress ipAddress = IPAddress.Parse(ip);

                IPEndPoint ipEndPoint = new IPEndPoint(
                    ipAddress,
                    port
                );

                tcpClient = new TcpClient();

                tcpClient.Connect(ipEndPoint);

                ns = tcpClient.GetStream();

                MessageBox.Show("Connected to Server!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient == null || ns == null || !ns.CanWrite)
                {
                    MessageBox.Show("Vui long Connect Server truoc!");
                    return;
                }

                string message = txtMessage.Text.Trim();

                if (message == "")
                {
                    MessageBox.Show("Vui long nhap tin nhan!");
                    return;
                }

                byte[] data = Encoding.UTF8.GetBytes(message + "\n");

                ns.Write(data, 0, data.Length);

                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send failed: " + ex.Message);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (ns != null)
                {
                    ns.Close();
                    ns = null!;
                }

                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient = null!;
                }

                MessageBox.Show("Disconnected from Server!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Disconnect failed: " + ex.Message);
            }
        }
    }
}
