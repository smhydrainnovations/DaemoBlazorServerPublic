namespace DaemoNETDemo.Models
{
    public class Payroll
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }  // Foreign Key to Employee
        public decimal Salary { get; set; }
        public DateTime SalaryDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
