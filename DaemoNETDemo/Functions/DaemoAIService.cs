using Daemo.SDK;
using DaemoNETDemo.Data;
using DaemoNETDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DaemoNETDemo.Functions
{
    
    public class DaemoAIService
    {
        private readonly ApplicationDbContext _db;
        public DaemoAIService(ApplicationDbContext db)
        {
            _db = db;
                
        }

        [DaemoFunction("This function retreives list of all Employees", Roles = new[] { "Admin" })]
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _db.Employees
                .Include(e => e.ApplicationUser) // Include related data if needed
                .AsNoTracking()
                .ToListAsync();
        }

        [DaemoFunction("This function retreives the date of joining for an employee", Roles = new[] { "Admin" ,"Manager","User"})]
        public DateTime? GetJoiningDateSingle([FromDaemoContext("applicationUserId")] string applicationUserId)
        {
            return _db.Employees
                .Where(e => e.ApplicationUserId == applicationUserId)
                .Select(e => e.DateOfJoining)
                .FirstOrDefault();
        }

    }
}
