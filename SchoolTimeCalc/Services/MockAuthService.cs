using Microsoft.EntityFrameworkCore;
using SchoolTimeCalc.Data;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Services
{
    public class MockAuthService
    {
        private readonly ApplicationDbContext _dbContext;

        public MockAuthService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var testUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
            
            if (testUser == null)
            {
                testUser = new ApplicationUser { Username = "testuser", Bundesland = "Wien" };
                _dbContext.Users.Add(testUser);
                await _dbContext.SaveChangesAsync();
            }

            return testUser;
        }
    }
}
