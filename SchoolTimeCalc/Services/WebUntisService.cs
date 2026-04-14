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

                string sessionId = response.Result.SessionId;

                // For now, placeholders for real fetching logic 
                // Zero knowledge - we get the data and don't store passwords, just the JSON payloads
                string subjectsJson = "[]";
                string teachersJson = "[]";
                string roomsJson = "[]";
                string lessonsJson = "[]";

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

                // Placeholder for logout request
                // var logoutRequest = new UntisRpcRequest { Method = "logout" };
                // await _client.LogoutAsync(logoutRequest, school);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
