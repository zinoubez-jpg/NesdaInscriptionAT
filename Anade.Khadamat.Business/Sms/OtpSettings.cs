namespace Anade.Khadamat.Business.Sms
{
    public class OtpSettings
    {
        public int ValidityMinutes { get; set; } = 2;
        public int CodeLength { get; set; } = 6;
    }
}
