namespace Domain.Settings
{
    public class EmailSettings
    {
        public int Port { get; set; }

        public string Host { get; set; }

        public string Sender { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public bool EnableSSL { get; set; }
    }
}
