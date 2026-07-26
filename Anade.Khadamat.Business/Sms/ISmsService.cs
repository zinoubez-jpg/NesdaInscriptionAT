using System.Threading.Tasks;

namespace Anade.Khadamat.Business.Sms
{
    public interface ISmsService
    {
        Task<SmsSendResult> SendSmsAsync(string to, string message);
    }
}
