using System.Threading.Tasks;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Services
{
    public interface ICalculationService
    {
        Task<CalculationResult> CalculateRemainingTimeAsync(string username);
    }
}
