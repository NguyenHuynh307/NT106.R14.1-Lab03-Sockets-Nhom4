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

        public ServerForm()
        {
            InitializeComponent();
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

                // Tao dia chi IP va Port
                IPEndPoint serverEndPoint = new IPEndPoint(
                    IPAddress.Parse("127.0.0.1"),
                    8080
                );

                // Gan Socket voi IP va Port
                listenerSocket.Bind(serverEndPoint);

                // Bat dau lang nghe
                listenerSocket.Listen(10);

                InfoMessage("Server dang lang nghe tai 127.0.0.1:8080");

                // Chap nhan ket noi Client
                clientSocket = listenerSocket.Accept();

                InfoMessage("Client da ket noi!");

                // Tao buffer nhan du lieu
                byte[] receiveBuffer = new byte[1024];

                // Danh sach gom cac byte cua mot thong diep
                List<byte> messageBytes = new List<byte>();

                // Nhan du lieu lien tuc
                while (true)
                {
                    int bytesReceived =
                        clientSocket.Receive(receiveBuffer);

                    // Client ngat ket noi binh thuong
                    if (bytesReceived == 0)
                    {
                        InfoMessage("Client da ngat ket noi!");
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
                InfoMessage("Loi Socket: " + ex.Message);
            }
            catch (Exception ex)
            {
                InfoMessage("Loi: " + ex.Message);
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
                SetListenButtonEnabled(true);
            }
        }

        // 5. Hien thi thong diep len ListView an toan
        private void InfoMessage(string message)
        {
            if (listViewCommand.InvokeRequired)
            {
                listViewCommand.Invoke(
                    new Action(() => InfoMessage(message))
                );

                return;
            }

            listViewCommand.Items.Add(
                new ListViewItem(message)
            );
        }

        // 6. Cap nhat trang thai nut Listen
        private void SetListenButtonEnabled(bool enabled)
        {
            if (InvokeRequired)
            {
                Invoke(
                    new Action(() => SetListenButtonEnabled(enabled))
                );

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
