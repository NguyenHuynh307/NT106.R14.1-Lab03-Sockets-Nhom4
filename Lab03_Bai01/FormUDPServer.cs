using System;
using System.Collections.Generic;
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
        private UdpClient? udpServer;
        private Thread? listenThread;
        private volatile bool isListening = false;

        public FormUDPServer()
        {
            InitializeComponent();
            UiKit.FillLastColumn(listViewReceivedMessages);
        }

        private void btnListen_Click(object? sender, EventArgs e)
        {
            string portStr = tbPort.Text.Trim();
            if (string.IsNullOrEmpty(portStr))
            {
                MessageBox.Show("Vui lòng nhập số cổng lắng nghe.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbPort.Focus();
                return;
            }

            if (!int.TryParse(portStr, out int port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Cổng phải là số nguyên từ 1 đến 65535.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPort.Focus();
                tbPort.SelectAll();
                return;
            }

            UdpClient server;
            try
            {
                // Bind cổng ngay tại đây để lỗi (ví dụ cổng đang bị chiếm) báo ra ngay.
                server = new UdpClient(port);
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Không mở được cổng " + port + ": " + ex.Message, "Lỗi khởi động Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            udpServer = server;
            isListening = true;
            listenThread = new Thread(() => StartServer(server));
            listenThread.IsBackground = true;
            listenThread.Start();

            btnListen.Enabled = false;
            tbPort.Enabled = false;
            lblInfo.ForeColor = UiKit.Good;
            lblInfo.Text = "Đang lắng nghe cổng " + port + "  ·  IP máy này: " + LocalAddresses();
        }

        private void StartServer(UdpClient server)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (isListening)
            {
                try
                {
                    byte[] receiveBytes = server.Receive(ref remoteEndPoint);
                    string text = Encoding.UTF8.GetString(receiveBytes).Replace("\r", " ").Replace("\n", " ");
                    AddRow(remoteEndPoint.Address + ":" + remoteEndPoint.Port, text);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (SocketException ex)
                {
                    if (!isListening)
                    {
                        break;
                    }
                    // Lỗi tạm thời (ví dụ ICMP trả về khi gửi tới máy tắt): báo rồi nghe tiếp.
                    AddRow("Hệ thống", "Lỗi socket: " + ex.Message);
                }
            }
        }

        private void AddRow(string source, string text)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action<string, string>(AddRow), source, text);
                }
                catch (ObjectDisposedException)
                {
                }
                catch (InvalidOperationException)
                {
                }
                return;
            }

            ListViewItem item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
            item.SubItems.Add(source);
            item.SubItems.Add(text);
            listViewReceivedMessages.Items.Add(item);
            item.EnsureVisible();
        }

        private static string LocalAddresses()
        {
            List<string> list = new List<string>();
            try
            {
                foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        list.Add(address.ToString());
                    }
                }
            }
            catch (SocketException)
            {
            }
            return list.Count > 0 ? string.Join("  ", list) : "(không đọc được)";
        }

        private void FormUDPServer_FormClosing(object? sender, FormClosingEventArgs e)
        {
            isListening = false;
            try
            {
                udpServer?.Close();
            }
            catch (SocketException)
            {
            }
        }
    }
}
