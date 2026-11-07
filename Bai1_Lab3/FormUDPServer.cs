using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Bai1_Lab3
{
    public partial class FormUDPServer : Form
    {
        private UdpClient udpServer;
        private Thread listenThread;
        private bool isListening = false;
        public FormUDPServer()
        {
            InitializeComponent();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            try
            {
                string portStr = tbPort.Text.Trim();
                if (string.IsNullOrEmpty(portStr))
                {
                    MessageBox.Show("Vui lòng nhập số Port lắng nghe!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbPort.Focus();
                    return;
                }
                int port;
                if (!int.TryParse(portStr, out port) || port < 0 || port > 65535)
                {
                    MessageBox.Show("Port phải là một số nguyên từ 0 đến 65535!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbPort.Focus();
                    tbPort.SelectAll();
                    return;
                }
                isListening = true;
                listenThread = new Thread(() => StartServer(port));
                listenThread.IsBackground = true;
                listenThread.Start();
                MessageBox.Show($"Server đã bắt đầu lắng nghe tại cổng {port}...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnListen.Enabled = false;
                tbPort.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động Server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartServer(int port)
        {
            try
            {
                udpServer = new UdpClient(port);
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                while (isListening)
                {
                    try
                    {
                        byte[] receiveBytes = udpServer.Receive(ref remoteEndPoint);
                        string returnData = Encoding.UTF8.GetString(receiveBytes);
                        string message = $"{remoteEndPoint.Address}:{remoteEndPoint.Port}: {returnData}";
                        UpdateMessageList(message);
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (isListening)
                {
                    UpdateMessageList("Lỗi Server: " + ex.Message);
                }
            }
        }

        private void UpdateMessageList(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateMessageList), new object[] { message });
            }
            else
            {
                listViewReceivedMessages.Items.Add(message);
            }
        }

        private void FormUDPServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            isListening = false;
            try
            {
                udpServer?.Close();
            }
            catch
            {
            }
        }
    }
}
