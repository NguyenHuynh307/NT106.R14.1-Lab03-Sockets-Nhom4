namespace Bai6
{
    public class ChatPacket
    {
        public string Type { get; set; } = "";
        public string From { get; set; } = "";
        public string To { get; set; } = "";
        public string Message { get; set; } = "";
        public string[]? Users { get; set; }

        public string? FileName { get; set; }
        public string? FileData { get; set; }
    }
}
