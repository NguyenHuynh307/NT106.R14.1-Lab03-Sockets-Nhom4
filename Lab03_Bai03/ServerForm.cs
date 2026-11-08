using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Threading;
namespace Bai3
{
    public partial class ServerForm : Form
    {
        // 1. Cac bien quan ly Socket
        private Socket listenerSocket = null!;
        private Socket clientSocket = null!;

        // 2. Thread chay Server
        private Thread serverThread = null!;

        // Co danh dau Server dang duoc dong (dong Form) de khong bao loi gia
        private volatile bool stopping = false;

        public ServerForm()
        {
            InitializeComponent();
            UiKit.FillLastColumn(listViewCommand);
        }

        // 3. Su kien nhan nut Listen
        private void btnListen_Click(object sender, EventArgs e)
        {
            btnListen.Enabled = false;

            serverThread = new Thread(
                new ThreadStart(StartServer)
            );

            serverThread.IsBackground = true;

            serverThread.Start();
        }

        // 4. Khoi dong TCP Server
        private void StartServer()
        {
            try
            {
                // Tao TCP Socket
                listenerSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp
                );

                // Lang nghe tren moi card mang de may khac trong LAN ket noi duoc
                IPEndPoint serverEndPoint = new IPEndPoint(
                    IPAddress.Any,
                    8080
                );

                // Gan Socket voi IP va Port
                listenerSocket.Bind(serverEndPoint);

                // Bat dau lang nghe
                listenerSocket.Listen(10);

                InfoMessage("Server đang lắng nghe trên mọi card mạng (0.0.0.0:8080)");

                foreach (string ip in GetLocalIPv4())
                {
                    InfoMessage("Máy khác (Client) nhập IP: " + ip);
                }

                // 1 Server - 1 Client tai mot thoi diem. Client ngat ket noi thi
                // Server van lang nghe de Client ket noi lai, khong can bam Listen lai.
                while (!stopping)
                {
                    clientSocket = listenerSocket.Accept();

                    InfoMessage("Client đã kết nối từ " + clientSocket.RemoteEndPoint + ".");

                    ReceiveFromClient(clientSocket);

                    clientSocket.Close();
                    clientSocket = null!;
                }
            }
            catch (SocketException ex)
            {
                if (!stopping)
                {
                    InfoMessage("Lỗi Socket: " + ex.Message);
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                if (!stopping)
                {
                    InfoMessage("Lỗi: " + ex.Message);
                }
            }
            finally
            {
                // Dong Socket Client
                clientSocket?.Close();
                clientSocket = null!;

                // Dong Socket Server
                listenerSocket?.Close();
                listenerSocket = null!;

                // Cho phep nhan nut Listen lai
                if (!stopping)
                {
                    SetListenButtonEnabled(true);
                }
            }
        }

        // 4b. Nhan du lieu tu mot Client cho den khi Client ngat ket noi
        private void ReceiveFromClient(Socket client)
        {
            // Tao buffer nhan du lieu
            byte[] receiveBuffer = new byte[1024];

            // Danh sach gom cac byte cua mot thong diep
            List<byte> messageBytes = new List<byte>();

            try
            {
                // Nhan du lieu lien tuc
                while (true)
                {
                    int bytesReceived = client.Receive(receiveBuffer);

                    // Client ngat ket noi binh thuong
                    if (bytesReceived == 0)
                    {
                        InfoMessage("Client đã ngắt kết nối.");
                        break;
                    }

                    // Duyet cac byte thuc te nhan duoc
                    for (int i = 0; i < bytesReceived; i++)
                    {
                        // Gap ky tu ket thuc thong diep
                        if (receiveBuffer[i] == (byte)'\n')
                        {
                            string message = Encoding.UTF8.GetString(
                                messageBytes.ToArray()
                            );

                            InfoMessage("Client: " + message);

                            // Xoa du lieu cu
                            messageBytes.Clear();
                        }
                        else if (receiveBuffer[i] != (byte)'\r')
                        {
                            messageBytes.Add(receiveBuffer[i]);
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                // Client tat dot ngot: ghi nhan, Server van tiep tuc cho Client ke tiep
                if (!stopping)
                {
                    InfoMessage("Client mất kết nối: " + ex.Message);
                }
            }
        }

        // Lay cac dia chi IPv4 cua may de Client ben kia biet can nhap IP nao
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

        // Dong Form thi nha cong 8080 de mo Server moi o lan sau van Listen duoc
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            stopping = true;

            try { clientSocket?.Close(); } catch { }
            try { listenerSocket?.Close(); } catch { }

            base.OnFormClosing(e);
        }

        // 5. Hien thi thong diep len ListView an toan
        private void InfoMessage(string message)
        {
            if (IsDisposed)
            {
                return;
            }

            if (listViewCommand.InvokeRequired)
            {
                try
                {
                    listViewCommand.Invoke(
                        new Action(() => InfoMessage(message))
                    );
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
            item.SubItems.Add(message);
            listViewCommand.Items.Add(item);
            item.EnsureVisible();
        }

        // 6. Cap nhat trang thai nut Listen
        private void SetListenButtonEnabled(bool enabled)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    Invoke(
                        new Action(() => SetListenButtonEnabled(enabled))
                    );
                }
                catch (ObjectDisposedException)
                {
                }
                catch (InvalidOperationException)
                {
                }

                return;
            }

            btnListen.Enabled = enabled;
        }

        // 7. Su kien Form Load
        private void ServerForm_Load(object sender, EventArgs e)
        {
        }

        // 8. Su kien thay doi muc duoc chon trong ListView
        private void listViewCommand_SelectedIndexChanged(
            object sender, EventArgs e)
        {
        }
    }
}
