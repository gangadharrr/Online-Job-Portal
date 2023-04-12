namespace Online_Job_Portal.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }

        public double JobSalary { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set;}
    }
}
