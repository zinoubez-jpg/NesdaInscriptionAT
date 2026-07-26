namespace Anade.Khadamat.Business.Sms
{
    public class SmsSendResult
    {
        public bool Succeeded { get; set; }
        public string RawResponse { get; set; }
        public int? HttpStatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public static SmsSendResult Success(string rawResponse, int httpStatusCode)
        {
            return new SmsSendResult
            {
                Succeeded = true,
                RawResponse = rawResponse,
                HttpStatusCode = httpStatusCode
            };
        }

        public static SmsSendResult Failure(string rawResponse, int? httpStatusCode, string errorMessage)
        {
            return new SmsSendResult
            {
                Succeeded = false,
                RawResponse = rawResponse,
                HttpStatusCode = httpStatusCode,
                ErrorMessage = errorMessage
            };
        }
    }
}
