using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercise.Models;
using StudentExercise.Models.ViewModel;

namespace StudentExerciseMVC.Controllers
{
    public class StudentController : Controller
    {
     
            private readonly IConfiguration _config;

            public StudentController(IConfiguration config)
            {
                _config = config;
            }

            public SqlConnection Connection
            {
                get
                {
                    return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                }
            }
        
        // GET: Student
        public ActionResult Index()
        {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                                    SELECT s.Id,
                                        s.FirstName,
                                        s.LastName,
                                        s.SlackHandle,
                                        s.CohortOneId
                                        FROM Students s";

                        SqlDataReader reader = cmd.ExecuteReader();

                        List<Students> students = new List<Students>();
                        while (reader.Read())
                        {
                            Students student = new Students
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                                CohortOneId = reader.GetInt32(reader.GetOrdinal("CohortOneId"))
                            };

                            students.Add(student);
                        }

                        reader.Close();

                        return View(students);
                    }
                }
            }

        

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection  conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT s.Id,
                                        s.FirstName,
                                        s.LastName,
                                        s.SlackHandle,
                                        s.CohortOneId
                                        FROM Students s
                                        Where Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Students students = null;
                    if (reader.Read())
                    {
                        students = new Students
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortOneId = reader.GetInt32(reader.GetOrdinal("CohortOneId"))
                        };

                      
                    reader.Close();

                    return View(students);
                    }
                    else
                    {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);
                    }

                }
            }
        }

        

        // GET: Student/Create
        public ActionResult Create()
        {
            StudentCreateViewModel viewModel = new StudentCreateViewModel();

            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StudentCreateViewModel model)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Students
                ( Id,FirstName, LastName, SlackHandle, CohortOneId )
                VALUES
                ( @Id, @FirstName, @LastName, @SlackHandle, @CohortOneId )";
                    cmd.Parameters.Add(new SqlParameter("@Id", model.student.Id));
                    cmd.Parameters.Add(new SqlParameter("@FirstName", model.student.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", model.student.LastName));
                    cmd.Parameters.Add(new SqlParameter("@SlackHandle", model.student.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@CohortOneId", model.student.CohortOneId));
                    await cmd.ExecuteNonQueryAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
        }
        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            Students students = GetStudentById(id);
            List<CohortOne> cohorts = GetAllCohorts();
            StudentEditViewModel viewModel = new StudentEditViewModel();
            viewModel.student = students;
            viewModel.AvailableCohorts = cohorts;

            return View(viewModel);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, StudentEditViewModel viewModel)
        {
            Students students = viewModel.student;
            try
            {
                // TODO: Add update logic here
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                                    UPDATE Students
                                        SET 
                                        FirstName = @FirstName,
                                        LastName = @LastName,
                                        SlackHandle = @SlackHandle,
                                        CohortOneId = @CohortOneId
                                        Where Id = @id";

                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd.Parameters.Add(new SqlParameter("@FirstName", students.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", students.LastName));
                        cmd.Parameters.Add(new SqlParameter("@SlackHandle", students.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@CohortOneId", students.CohortOneId));


                        cmd.ExecuteNonQuery();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
                
            
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            Students students = GetStudentById(id);
            return View(students);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // TODO: Add delete logic here
                using (SqlConnection conn = Connection )
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"Delete  from StudentForExercises Where StudentsId = @id;
                                            Delete from Students Where Id = @id";

                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            catch
            {
                return View();
            }
        }

        private Students GetStudentById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
            SELECT s.Id,
                s.FirstName,
                s.LastName,
                s.SlackHandle,
                s.CohortOneId
            FROM Students s
            WHERE s.Id=@id
        ";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Students students = null;
                    if (reader.Read())
                    {
                        students = new Students
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortOneId = reader.GetInt32(reader.GetOrdinal("CohortOneId"))
                        };
                    }

                    reader.Close();

                    return students;
                }
            }
        }
        private List<CohortOne> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @" SELECT c.Id, c.Name FROM CohortOne c";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<CohortOne> cohortOnes = new List<CohortOne>();
                    while (reader.Read())
                    {
                        CohortOne cohortOne = new CohortOne
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        cohortOnes.Add(cohortOne);
                    }
                    reader.Close();

                    return cohortOnes;

                }
            }
        }
    }
}
  
    
