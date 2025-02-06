namespace Anade.Business.Core
{
    public class ApiBusinessResult : BusinessResult
    {
        public object Result { get; set; }
        public ApiBusinessResult(bool succeeded) : base(succeeded)
        {
        }

    }
}
