
namespace Anade.Business.Core
{
    public interface IApiServiceFactory
    {
        IApiService<T> GetApiService<T>() where T : class;

    }
}
