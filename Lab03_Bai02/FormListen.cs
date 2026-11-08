using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Bai2_Lab3
{
    public partial class FormListen : Form
    {
        private Socket serverSocket = null;
        private bool isRunning = false;
        private Thread serverThread;
        public FormListen()
        {
            InitializeComponent();
            UiKit.FillLastColumn(listViewMessage);
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            try
            {
                int port = 8080;
                isRunning = true;
                serverThread = new Thread(() => StartTcpServer(port));
                serverThread.IsBackground = true;
                serverThread.Start();
                btnListen.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động Server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartTcpServer(int port) {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(10);
                SetInfo($"Đang lắng nghe cổng {port}. Từ máy khác dùng: telnet <IP máy này> {port}");
                UpdateUI($"Telnet server đang nghe trên mọi card mạng (0.0.0.0:{port})");
                foreach (string ip in GetLocalIPv4())
                {
                    UpdateUI($"Từ máy khác: telnet {ip} {port}");
                }
                while (isRunning)
                {
                    Socket clientSocket = serverSocket.Accept();
                    Thread clientThread = new Thread(() => HandleClient(clientSocket));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }

            }
            catch (Exception ex)
            {
                if (isRunning)
                {
                    UpdateUI("Lỗi Server: " + ex.Message);

                    // Bind/Listen lỗi (ví dụ cổng 8080 đang bị bài khác dùng): cho phép bấm Listen lại
                    isRunning = false;
                    try { serverSocket?.Close(); } catch { }
                    SetListenEnabled(true);
                }
            }
        }

        private static List<string> GetLocalIPv4()
        {
            List<string> result = new List<string>();
            try
            {
                foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        result.Add(address.ToString());
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        private void SetListenEnabled(bool enabled)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(new Action<bool>(SetListenEnabled), new object[] { enabled });
                }
                catch (ObjectDisposedException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
            else
            {
                btnListen.Enabled = enabled;
            }
        }

        private void HandleClient(Socket clientSocket)
        {
            try
            {
                string clientIP = ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString();
                byte[] buffer = new byte[1024];
                List<byte> lineBytes = new List<byte>();
                int bytesRead;
                while ((bytesRead = clientSocket.Receive(buffer)) > 0)
                {
                    for (int i = 0; i < bytesRead; i++)
                    {
                        byte b = buffer[i];
                        if (b == 255)
                        {
                            i += 2; 
                            continue;
                        }
                        if (b == '\n' || b == '\r')
                        {
                            if (lineBytes.Count > 0)
                            {
                                string message = Encoding.UTF8.GetString(lineBytes.ToArray()).Trim();
                                if (!string.IsNullOrEmpty(message))
                                {
                                    UpdateUI(clientIP, message);
                                }
                                lineBytes.Clear(); 
                            }
                        }
                        else if (b == 8 || b == 127)
                        {
                            // phím Backspace của telnet: xóa ký tự vừa gõ
                            if (lineBytes.Count > 0)
                            {
                                lineBytes.RemoveAt(lineBytes.Count - 1);
                            }
                        }
                        else
                        {
                            lineBytes.Add(b);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateUI("Lỗi kết nối client: " + ex.Message);
            }
            finally
            {
                try
                {
                    clientSocket.Close();
                }
                catch { }
            }
        }

        private void UpdateUI(string message)
        {
            UpdateUI("Hệ thống", message);
        }

        private void UpdateUI(string source, string text)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action<string, string>(UpdateUI), source, text);
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
            listViewMessage.Items.Add(item);
            item.EnsureVisible();
        }

        private void SetInfo(string text)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action<string>(SetInfo), text);
                }
                catch (ObjectDisposedException)
                {
                }
                catch (InvalidOperationException)
                {
                }
                return;
            }

            lblInfo.ForeColor = UiKit.Good;
            lblInfo.Text = text;
        }

        private void FormListen_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
            try
            {
                serverSocket?.Close();
            }
            catch 
            { 
            }
        }
    }
}
