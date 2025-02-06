using Anade.Business.Core;
using System.Text;

namespace Anade.Khadamat.Web
{
    public static class BusinessResultExtensions
    {
        public static string ToBootstrapAlerts(this BusinessResult businessResult)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var msg in businessResult.Messages)
            {
                sb.AppendFormat("<div class='alert alert-{0} alert-dismissible fade show' role='alert'><button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>{1}</div>", msg.MessageType == MessageType.Error ? "danger" : msg.MessageType.ToString().ToLower(), msg.Message);
            }

            return sb.ToString();
        }
    }
}
