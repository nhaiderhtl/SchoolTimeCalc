using System.Threading;
using System.Threading.Tasks;

namespace SchoolTimeCalc.Services
{
    public interface IHolidaySyncService
    {
        Task SyncHolidaysAsync(string server, string schoolName, string username, string password, CancellationToken cancellationToken = default);
    }
}
