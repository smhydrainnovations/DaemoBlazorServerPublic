namespace DaemoNETDemo.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }  // Foreign Key to Employee
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }  // Enum: Pending, Approved, Rejected

        public virtual Employee Employee { get; set; }
        public string LeaveType { get; set; }
    }
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
