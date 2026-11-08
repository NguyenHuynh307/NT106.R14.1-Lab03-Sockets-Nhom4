using System.Net.Sockets;

namespace Lab03.Bai04;

/// <summary>Nối TCP có giới hạn thời gian, để client không bị treo giao diện khi server không phản hồi.</summary>
internal static class TcpHelper
{
    public static TcpClient ConnectWithTimeout(string host, int port, int milliseconds = 3000)
    {
        var client = new TcpClient();
        try
        {
            if (!client.ConnectAsync(host, port).Wait(milliseconds))
            {
                client.Dispose();
                throw new TimeoutException("Hết thời gian chờ server " + host + ":" + port + ". Kiểm tra IP, cổng và Windows Firewall.");
            }
            client.SendTimeout = 5000;
            client.ReceiveTimeout = 5000;
            return client;
        }
        catch (AggregateException ex)
        {
            client.Dispose();
            throw ex.GetBaseException();
        }
    }
}
