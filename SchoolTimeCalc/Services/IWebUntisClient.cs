using System.Text.Json;
using System.Threading.Tasks;
using Refit;

namespace SchoolTimeCalc.Services
{
    public interface IWebUntisClient
    {
        [Post("/WebUntis/jsonrpc.do")]
        Task<UntisRpcResponse<UntisAuthResponse>> AuthenticateAsync([Body] UntisRpcRequest request, [Query] string school);

        [Post("/WebUntis/jsonrpc.do")]
        Task<UntisRpcResponse<JsonElement>> GetSubjectsAsync([Body] UntisRpcRequest request, [Query] string school, [Header("Cookie")] string cookie);

        [Post("/WebUntis/jsonrpc.do")]
        Task<UntisRpcResponse<JsonElement>> GetTeachersAsync([Body] UntisRpcRequest request, [Query] string school, [Header("Cookie")] string cookie);

        [Post("/WebUntis/jsonrpc.do")]
        Task<UntisRpcResponse<JsonElement>> GetRoomsAsync([Body] UntisRpcRequest request, [Query] string school, [Header("Cookie")] string cookie);

        [Post("/WebUntis/jsonrpc.do")]
        Task<UntisRpcResponse<JsonElement>> GetTimetableAsync([Body] UntisRpcRequest request, [Query] string school, [Header("Cookie")] string cookie);

        [Post("/WebUntis/jsonrpc.do")]
        Task<UntisRpcResponse<object>> LogoutAsync([Body] UntisRpcRequest request, [Query] string school, [Header("Cookie")] string cookie);
    }
}
