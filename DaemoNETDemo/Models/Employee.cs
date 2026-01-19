using DaemoNETDemo.Data;

namespace DaemoNETDemo.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }  // Foreign Key to ApplicationUser
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public DateTime DateOfJoining { get; set; }
     //   public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
