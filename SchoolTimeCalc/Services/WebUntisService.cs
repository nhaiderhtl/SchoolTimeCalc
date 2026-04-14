using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolTimeCalc.Data;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Services
{
    public class WebUntisService
    {
        private readonly IWebUntisClient _client;
        private readonly ApplicationDbContext _dbContext;
        private readonly MockAuthService _authService;

        public WebUntisService(IWebUntisClient client, ApplicationDbContext dbContext, MockAuthService authService)
        {
            _client = client;
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<bool> AuthenticateAndSyncAsync(string server, string school, string username, string password)
        {
            string? sessionId = null;
            try
            {
                var request = new UntisRpcRequest
                {
                    Id = "auth-1",
                    Method = "authenticate",
                    Params = new UntisAuthRequest
                    {
                        User = username,
                        Password = password,
                        Client = "SchoolTimeCalc"
                    }
                };

                var response = await _client.AuthenticateAsync(request, school);
                if (response == null || response.Result == null || string.IsNullOrEmpty(response.Result.SessionId))
                {
                    return false;
                }

                sessionId = response.Result.SessionId;
                string cookie = $"JSESSIONID={sessionId}";

                // 1. Fetch data
                var subjReq = new UntisRpcRequest { Id = "subj-1", Method = "getSubjects" };
                var subjRes = await _client.GetSubjectsAsync(subjReq, school, cookie);

                var teachReq = new UntisRpcRequest { Id = "teach-1", Method = "getTeachers" };
                var teachRes = await _client.GetTeachersAsync(teachReq, school, cookie);

                var roomsReq = new UntisRpcRequest { Id = "rooms-1", Method = "getRooms" };
                var roomsRes = await _client.GetRoomsAsync(roomsReq, school, cookie);

                var ttReq = new UntisRpcRequest
                {
                    Id = "tt-1",
                    Method = "getTimetable",
                    Params = new
                    {
                        options = new
                        {
                            element = new
                            {
                                id = response.Result.PersonType == 5 ? response.Result.PersonId : response.Result.ClassId,
                                type = response.Result.PersonType
                            },
                            startDate = int.Parse(DateTime.Now.ToString("yyyyMM01")),
                            endDate = int.Parse(DateTime.Now.ToString("yyyyMM") + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
                        }
                    }
                };
                var ttRes = await _client.GetTimetableAsync(ttReq, school, cookie);

                // Serialize the JsonElement results to string
                string subjectsJson = subjRes != null && subjRes.Result.ValueKind != JsonValueKind.Undefined ? JsonSerializer.Serialize(subjRes.Result) : "[]";
                string teachersJson = teachRes != null && teachRes.Result.ValueKind != JsonValueKind.Undefined ? JsonSerializer.Serialize(teachRes.Result) : "[]";
                string roomsJson = roomsRes != null && roomsRes.Result.ValueKind != JsonValueKind.Undefined ? JsonSerializer.Serialize(roomsRes.Result) : "[]";
                string lessonsJson = ttRes != null && ttRes.Result.ValueKind != JsonValueKind.Undefined ? JsonSerializer.Serialize(ttRes.Result) : "[]";

                var user = await _authService.GetCurrentUserAsync();

                var data = await _dbContext.WebUntisData.FirstOrDefaultAsync(d => d.ApplicationUserId == user.Id);
                if (data == null)
                {
                    data = new WebUntisData { ApplicationUserId = user.Id };
                    _dbContext.WebUntisData.Add(data);
                }

                data.SchoolName = school;
                data.SubjectsJson = subjectsJson;
                data.TeachersJson = teachersJson;
                data.RoomsJson = roomsJson;
                data.LessonsJson = lessonsJson;

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    try
                    {
                        var logoutRequest = new UntisRpcRequest { Id = "logout-1", Method = "logout" };
                        await _client.LogoutAsync(logoutRequest, school, $"JSESSIONID={sessionId}");
                    }
                    catch { /* Ignore logout errors */ }
                }
            }
        }
    }
}
