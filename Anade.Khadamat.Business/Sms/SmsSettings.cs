namespace Anade.Khadamat.Business.Sms
{
    public class SmsSettings
    {
        public string BaseUrl { get; set; } = "https://smspro-plus.com/playsms/index.php";
        public string Username { get; set; }
        public string Token { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
