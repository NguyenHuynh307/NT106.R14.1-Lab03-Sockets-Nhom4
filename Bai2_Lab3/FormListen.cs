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
                UpdateUI($"Telnet running on 127.0.0.1:{port}");
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
                }
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
                                string message = Encoding.Default.GetString(lineBytes.ToArray()).Trim();
                                if (!string.IsNullOrEmpty(message))
                                {
                                    UpdateUI($"{clientIP}: {message}");
                                }
                                lineBytes.Clear(); 
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
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateUI), new object[] { message });
            }
            else
            {
                listViewMessage.Items.Add(message);
                if (listViewMessage.Items.Count > 0)
                {
                    listViewMessage.EnsureVisible(listViewMessage.Items.Count - 1);
                }
            }
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
