using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Online_Job_Portal.Models;

namespace Online_Job_Portal.Controllers
{
    public class JobController : Controller
    {
        //private  List<Job> JobList = new List<Job>();
        private IConfiguration Configuration;
        public JobController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IActionResult AddJobs()
        {
            string Message = " ";
            bool Error = false;
            if (Request.Method == "POST")
            {
                Job job = new Job();
                job.JobName = Request.Form["JobName"];
                job.JobDescription = Request.Form["JobDescription"];
                job.JobSalary = Convert.ToDouble(Request.Form["JobSalary"]);
                job.CompanyId = Convert.ToInt32(Request.Form["CompanyId"]);
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Configuration.GetConnectionString("Online_Job_Portal")))
                    {
                        sqlConnection.Open();
                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            Console.WriteLine(job.JobDescription);
                            cmd.CommandText = $"INSERT INTO JOBS VALUES('{job.JobName}','{job.JobDescription}',{job.JobSalary},{job.CompanyId})";
                            cmd.ExecuteNonQuery();
                            Message = "Added Successfully";
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Message="Error with SQL SERVER";
                    Console.WriteLine(ex.Message);
                    Error = true;
                }
                catch (Exception ex)
                {
                    Message = "Error While Adding Job";
                    Console.WriteLine(ex.Message);
                    Error = true;
                }
            
            }
            ViewData["Message"]=Message;
            ViewData["Error"]=Error;


            return View();
        }

        private List<Job> getJobs() {
           
            List<Job> JobsList = new List<Job>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Configuration.GetConnectionString("Online_Job_Portal")))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM JOBS INNER JOIN COMPANIES ON JOBS.COMPANY_ID=COMPANIES.COMPANY_ID";
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Job job = new Job();
                            job.JobId = (int)reader["JOB_ID"];
                            job.JobName = (string)reader["JOB_NAME"];
                            job.JobDescription = (string)reader["JOB_DESCRIPTION"];
                            job.JobSalary = (double)reader["JOB_SALARY"];
                            job.CompanyName = (string)reader["COMPANY_NAME"];
                            JobsList.Add(job);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return JobsList;
        } 
        public IActionResult DisplayJobs()
        {

            
            ViewBag.JobList = getJobs();
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Id = id;
            ViewBag.JobList = getJobs();
            return View();
        }


        public void updateJob(Job job)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Configuration.GetConnectionString("Online_Job_Portal")))
            {

                using (SqlCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "updateJob";

                    cmd.Parameters.AddWithValue("@JOB_ID", job.JobId);
                    cmd.Parameters.AddWithValue("@JOB_NAME", job.JobName);
                    cmd.Parameters.AddWithValue("@JOB_DESCRIPTION", job.JobDescription);
                    cmd.Parameters.AddWithValue("@JOB_SALARY", job.JobSalary);

                    // Not used in the procedure
                    cmd.Parameters.AddWithValue("@COMPANY_ID", job.CompanyId);

                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    sqlConnection.Close();

                }
            }
        }


        [HttpPost]
        public IActionResult Edit(int id, IFormCollection form)
        {
            try
            {

                Job job = new Job() {   JobId= Convert.ToInt32( form["jobId"]) ,JobDescription= form["jobDescription"], JobName= form["jobName"]   ,
                    
                    JobSalary = Convert.ToInt32(form["jobSalary"]),
                    CompanyId = Convert.ToInt32(form["companyId"])

                };
                updateJob(job);
                return RedirectToAction("DisplayJobs");
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }


        private void deleteJob(int jobId)
        {

            using (SqlConnection sqlConnection = new SqlConnection(Configuration.GetConnectionString("Online_Job_Portal")))
            {
                
                using (SqlCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText= "deleteJob";

                    cmd.Parameters.AddWithValue("@JOB_ID",jobId);

                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    sqlConnection.Close();

                }
            }

        }

        public IActionResult Delete(int id)
        {
            try
            {
                deleteJob(id);
                return RedirectToAction("DisplayJobs");
            }catch (Exception ex)
            {
                return View();
            }

        }
    }
}