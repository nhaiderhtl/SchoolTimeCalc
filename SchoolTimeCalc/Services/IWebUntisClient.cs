using System.Threading.Tasks;
using Refit;

namespace SchoolTimeCalc.Services
{
    public interface IWebUntisClient
    {
        [Post("/WebUntis/jsonrpc.do")]
        Task<UntisRpcResponse<UntisAuthResponse>> AuthenticateAsync([Body] UntisRpcRequest request, [Query] string school);
    }
}
