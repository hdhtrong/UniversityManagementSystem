namespace NotificationService.API.Config
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Sender { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool UseSSL { get; set; }
        public bool UseStartTls { get; set; }
    }
}
