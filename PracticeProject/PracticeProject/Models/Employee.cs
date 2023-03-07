namespace PracticeProject.Models
{
    public class Employee:EntityBase
    {
        public string? Name { get; set; }
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public int Salary { get; set; }
    }
}
