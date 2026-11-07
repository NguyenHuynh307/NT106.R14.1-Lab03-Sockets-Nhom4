namespace Lab03.Bai05;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        if (args.Length > 0 && args[0] == "--server")
        {
            int port = args.Length > 1 && int.TryParse(args[1], out int requested) ? requested : 8085;
            using var server = new FoodTcpServer(port);
            server.Start();
            new ManualResetEventSlim(false).Wait();
            return;
        }
        Application.Run(new ExerciseLauncher());
    }
}
