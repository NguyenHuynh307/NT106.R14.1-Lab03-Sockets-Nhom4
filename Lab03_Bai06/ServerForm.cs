using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai6
{
    public partial class ServerForm : Form
    {
        private const int PORT = 8080;
        private const int MAX_FILE_SIZE = 5 * 1024 * 1024;

        private TcpListener? server;
        private CancellationTokenSource? serverCts;

        private readonly ConcurrentDictionary<string, ClientConnection>
            clients = new ConcurrentDictionary<string, ClientConnection>(
                StringComparer.OrdinalIgnoreCase
            );

        private Button btnStart = null!;
        private Button btnStop = null!;
        private Label lblStatus = null!;
        private ListBox listLog = null!;
        private ListBox listUsers = null!;

        public ServerForm()
        {
            InitializeComponent();
            CreateInterface();
        }

        // =========================
        // GIAO DIEN SERVER
        // =========================

        private void CreateInterface()
        {
            Text = "Bài 6 · Chat Room · Server";
            Size = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;

            Label lblTitle = new Label();
            lblTitle.Text = "CHAT ROOM SERVER - PORT 8080";
            lblTitle.Font = new Font("Arial", 15, FontStyle.Bold);
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;

            btnStart = new Button();
            btnStart.Text = "Start Server";
            btnStart.Location = new Point(20, 55);
            btnStart.Size = new Size(130, 40);
            btnStart.Click += BtnStart_Click;

            btnStop = new Button();
            btnStop.Text = "Stop Server";
            btnStop.Location = new Point(160, 55);
            btnStop.Size = new Size(130, 40);
            btnStop.Enabled = false;
            btnStop.Click += BtnStop_Click;

            lblStatus = new Label();
            lblStatus.Text = "Status: Stopped";
            lblStatus.Location = new Point(320, 65);
            lblStatus.AutoSize = true;

            Label lblLog = new Label();
            lblLog.Text = "SERVER LOG";
            lblLog.Location = new Point(20, 120);
            lblLog.AutoSize = true;
            lblLog.Font = new Font("Arial", 10, FontStyle.Bold);

            listLog = new ListBox();
            listLog.Location = new Point(20, 150);
            listLog.Size = new Size(600, 370);
            listLog.Anchor = AnchorStyles.Top |
                             AnchorStyles.Bottom |
                             AnchorStyles.Left |
                             AnchorStyles.Right;

            Label lblUsers = new Label();
            lblUsers.Text = "ONLINE USERS";
            lblUsers.Location = new Point(650, 120);
            lblUsers.AutoSize = true;
            lblUsers.Font = new Font("Arial", 10, FontStyle.Bold);

            listUsers = new ListBox();
            listUsers.Location = new Point(650, 150);
            listUsers.Size = new Size(220, 370);
            listUsers.Anchor = AnchorStyles.Top |
                               AnchorStyles.Bottom |
                               AnchorStyles.Right;

            Controls.Add(lblTitle);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(lblStatus);
            Controls.Add(lblLog);
            Controls.Add(listLog);
            Controls.Add(lblUsers);
            Controls.Add(listUsers);

            FormClosing += ServerForm_FormClosing;
        }

        // =========================
        // START SERVER
        // =========================

        private async void BtnStart_Click(object? sender, EventArgs e)
        {
            if (server != null)
                return;

            try
            {
                serverCts = new CancellationTokenSource();

                server = new TcpListener(IPAddress.Any, PORT);
                server.Start();

                btnStart.Enabled = false;
                btnStop.Enabled = true;
                lblStatus.Text = "Status: Listening";

                AddLog("Server started on port " + PORT);

                await AcceptClientsAsync(serverCts.Token);
            }
            catch (OperationCanceledException)
            {
                AddLog("Server stopped.");
            }
            catch (SocketException ex)
            {
                if (serverCts?.IsCancellationRequested != true)
                    AddLog("Socket error: " + ex.Message);
            }
            catch (Exception ex)
            {
                AddLog("Server error: " + ex.Message);
            }
            finally
            {
                await CleanupServerAsync();

                if (!IsDisposed)
                {
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    lblStatus.Text = "Status: Stopped";
                }
            }
        }

        // =========================
        // ACCEPT MULTIPLE CLIENTS
        // =========================

        private async Task AcceptClientsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                TcpClient tcpClient =
                    await server!.AcceptTcpClientAsync(token);

                AddLog("New connection: " +
                       tcpClient.Client.RemoteEndPoint);

                _ = HandleClientAsync(tcpClient, token);
            }
        }

        // =========================
        // HANDLE CLIENT
        // =========================

        private async Task HandleClientAsync(
            TcpClient tcpClient,
            CancellationToken token)
        {
            ClientConnection? connection = null;
            bool registered = false;

            StreamReader? reader = null;
            StreamWriter? writer = null;

            try
            {
                NetworkStream stream = tcpClient.GetStream();

                reader = new StreamReader(
                    stream,
                    new UTF8Encoding(false),
                    false,
                    1024,
                    true
                );

                writer = new StreamWriter(
                    stream,
                    new UTF8Encoding(false),
                    1024,
                    true
                );

                writer.AutoFlush = true;

                string? firstLine = await reader.ReadLineAsync(token);

                if (string.IsNullOrWhiteSpace(firstLine))
                    return;

                ChatPacket? hello =
                    JsonSerializer.Deserialize<ChatPacket>(firstLine);

                if (hello == null || hello.Type != "join")
                    return;

                string username = hello.From.Trim();

                if (username.Length == 0 || username.Length > 20)
                {
                    await SendRawPacketAsync(writer, new ChatPacket
                    {
                        Type = "error",
                        Message = "Invalid username."
                    });

                    return;
                }

                connection = new ClientConnection(
                    username,
                    tcpClient,
                    reader,
                    writer
                );

                if (!clients.TryAdd(username, connection))
                {
                    await SendRawPacketAsync(writer, new ChatPacket
                    {
                        Type = "error",
                        Message = "Username already exists."
                    });

                    return;
                }

                registered = true;

                AddLog(username + " joined the chat room.");
                UpdateUsers();

                await SendPacketAsync(connection, new ChatPacket
                {
                    Type = "system",
                    Message = "Welcome " + username + "!"
                });

                await BroadcastAsync(new ChatPacket
                {
                    Type = "system",
                    Message = username + " joined the chat room."
                }, username);

                await BroadcastAsync(new ChatPacket
                {
                    Type = "users",
                    Users = clients.Keys.OrderBy(name => name).ToArray()
                }, string.Empty);

                while (!token.IsCancellationRequested)
                {
                    string? line = await reader.ReadLineAsync(token);

                    if (line == null)
                        break;

                    ChatPacket? packet =
                        JsonSerializer.Deserialize<ChatPacket>(line);

                    if (packet == null)
                        continue;

                    if (packet.Type == "all")
                    {
                        if (string.IsNullOrWhiteSpace(packet.Message))
                            continue;

                        await BroadcastAsync(new ChatPacket
                        {
                            Type = "all",
                            From = username,
                            Message = packet.Message
                        }, username);
                    }
                    else if (packet.Type == "private")
                    {
                        await SendPrivateMessageAsync(
                            connection,
                            packet.To,
                            packet.Message
                        );
                    }
                    else if (packet.Type == "file")
                    {
                        await HandleFileAsync(connection, packet);
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (IOException)
            {
            }
            catch (SocketException ex)
            {
                AddLog("Client socket error: " + ex.Message);
            }
            catch (JsonException ex)
            {
                AddLog("Invalid message: " + ex.Message);
            }
            catch (Exception ex)
            {
                AddLog("Client error: " + ex.Message);
            }
            finally
            {
                if (connection != null)
                {
                    if (registered)
                    {
                        clients.TryRemove(connection.Username, out _);

                        AddLog(connection.Username + " disconnected.");
                        UpdateUsers();

                        await BroadcastAsync(new ChatPacket
                        {
                            Type = "system",
                            Message = connection.Username +
                                      " left the chat room."
                        }, connection.Username);

                        await BroadcastAsync(new ChatPacket
                        {
                            Type = "users",
                            Users = clients.Keys.OrderBy(name => name).ToArray()
                        }, string.Empty);
                    }

                    connection.Close();
                }
                else
                {
                    reader?.Dispose();
                    writer?.Dispose();
                    tcpClient.Close();
                }
            }
        }

        // =========================
        // HANDLE FILE
        // =========================

        private async Task HandleFileAsync(
            ClientConnection sender,
            ChatPacket packet)
        {
            string fileName = Path.GetFileName(packet.FileName ?? "");
            string fileData = packet.FileData ?? "";

            string extension = Path.GetExtension(fileName).ToLowerInvariant();

            bool validExtension =
                extension == ".jpg" ||
                extension == ".jpeg" ||
                extension == ".png" ||
                extension == ".txt";

            if (!validExtension || string.IsNullOrWhiteSpace(fileName))
            {
                await SendErrorAsync(sender, "File type is not supported.");
                return;
            }

            int maxBase64Length =
                ((MAX_FILE_SIZE + 2) / 3) * 4;

            if (fileData.Length > maxBase64Length)
            {
                await SendErrorAsync(sender, "File exceeds 5 MB.");
                return;
            }

            try
            {
                byte[] fileBytes = Convert.FromBase64String(fileData);

                if (fileBytes.Length > MAX_FILE_SIZE)
                {
                    await SendErrorAsync(sender, "File exceeds 5 MB.");
                    return;
                }
            }
            catch (FormatException)
            {
                await SendErrorAsync(sender, "Invalid file data.");
                return;
            }

            ChatPacket outgoing = new ChatPacket
            {
                Type = "file",
                From = sender.Username,
                To = packet.To,
                FileName = fileName,
                FileData = fileData
            };

            // GUI FILE NHOM
            if (string.IsNullOrWhiteSpace(packet.To))
            {
                await BroadcastAsync(outgoing, sender.Username);

                AddLog(sender.Username +
                       " shared file with everyone: " + fileName);
            }
            // GUI FILE RIENG
            else
            {
                if (!clients.TryGetValue(
                    packet.To,
                    out ClientConnection? receiver))
                {
                    await SendErrorAsync(
                        sender,
                        "User not found or offline."
                    );

                    return;
                }

                await SendPacketAsync(receiver, outgoing);

                AddLog("[Private File] " +
                       sender.Username + " -> " +
                       receiver.Username + ": " + fileName);
            }
        }

        private async Task SendErrorAsync(
            ClientConnection client,
            string message)
        {
            await SendPacketAsync(client, new ChatPacket
            {
                Type = "error",
                Message = message
            });
        }

        // =========================
        // BROADCAST
        // =========================

        private async Task BroadcastAsync(
            ChatPacket packet,
            string excludedUsername)
        {
            foreach (ClientConnection client in clients.Values.ToArray())
            {
                if (client.Username.Equals(
                    excludedUsername,
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                try
                {
                    await SendPacketAsync(client, packet);
                }
                catch
                {
                    client.TcpClient.Close();
                }
            }

            if (packet.Type == "all")
                AddLog(packet.From + ": " + packet.Message);
        }

        // =========================
        // PRIVATE MESSAGE
        // =========================

        private async Task SendPrivateMessageAsync(
            ClientConnection sender,
            string receiverName,
            string message)
        {
            if (string.IsNullOrWhiteSpace(receiverName) ||
                string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            if (sender.Username.Equals(
                receiverName,
                StringComparison.OrdinalIgnoreCase))
            {
                await SendErrorAsync(sender, "You cannot message yourself.");
                return;
            }

            if (!clients.TryGetValue(
                receiverName,
                out ClientConnection? receiver))
            {
                await SendErrorAsync(sender, "User not found or offline.");
                return;
            }

            ChatPacket packet = new ChatPacket
            {
                Type = "private",
                From = sender.Username,
                To = receiver.Username,
                Message = message
            };

            await SendPacketAsync(receiver, packet);
            await SendPacketAsync(sender, packet);

            AddLog("[Private] " + sender.Username +
                   " -> " + receiver.Username + ": " + message);
        }

        // =========================
        // SEND PACKET
        // =========================

        private async Task SendPacketAsync(
            ClientConnection client,
            ChatPacket packet)
        {
            await client.SendLock.WaitAsync();

            try
            {
                string json = JsonSerializer.Serialize(packet);
                await client.Writer.WriteLineAsync(json);
            }
            finally
            {
                client.SendLock.Release();
            }
        }

        private async Task SendRawPacketAsync(
            StreamWriter writer,
            ChatPacket packet)
        {
            string json = JsonSerializer.Serialize(packet);
            await writer.WriteLineAsync(json);
        }

        // =========================
        // UPDATE ONLINE USERS
        // =========================

        private void UpdateUsers()
        {
            string[] usernames = clients.Keys
                .OrderBy(name => name)
                .ToArray();

            RunOnUi(() =>
            {
                listUsers.Items.Clear();
                listUsers.Items.AddRange(usernames);
            });
        }

        // =========================
        // SERVER LOG
        // =========================

        private void AddLog(string message)
        {
            RunOnUi(() =>
            {
                listLog.Items.Add(
                    DateTime.Now.ToString("HH:mm:ss") +
                    " - " + message
                );

                listLog.TopIndex = listLog.Items.Count - 1;
            });
        }

        // =========================
        // UI THREAD
        // =========================

        private void RunOnUi(Action action)
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(action);
                }
                catch (InvalidOperationException)
                {
                }
            }
            else
            {
                action();
            }
        }

        // =========================
        // STOP SERVER
        // =========================

        private void BtnStop_Click(object? sender, EventArgs e)
        {
            serverCts?.Cancel();
            server?.Stop();

            foreach (ClientConnection client in clients.Values)
            {
                client.TcpClient.Close();
            }
        }

        private Task CleanupServerAsync()
        {
            serverCts?.Cancel();
            server?.Stop();

            foreach (ClientConnection client in clients.Values)
            {
                client.TcpClient.Close();
            }

            clients.Clear();
            server = null;

            serverCts?.Dispose();
            serverCts = null;

            UpdateUsers();

            return Task.CompletedTask;
        }

        private void ServerForm_FormClosing(
            object? sender,
            FormClosingEventArgs e)
        {
            serverCts?.Cancel();
            server?.Stop();

            foreach (ClientConnection client in clients.Values)
            {
                client.TcpClient.Close();
            }
        }

        // =========================
        // CLIENT CONNECTION
        // =========================

        private sealed class ClientConnection
        {
            public string Username { get; }
            public TcpClient TcpClient { get; }
            public StreamReader Reader { get; }
            public StreamWriter Writer { get; }

            public SemaphoreSlim SendLock { get; } =
                new SemaphoreSlim(1, 1);

            public ClientConnection(
                string username,
                TcpClient tcpClient,
                StreamReader reader,
                StreamWriter writer)
            {
                Username = username;
                TcpClient = tcpClient;
                Reader = reader;
                Writer = writer;
            }

            public void Close()
            {
                try { Reader.Dispose(); } catch { }
                try { Writer.Dispose(); } catch { }
                try { TcpClient.Close(); } catch { }
            }
        }
    }
}
