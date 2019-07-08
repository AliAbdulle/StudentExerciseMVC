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

namespace StudentExercise.Controllers
{
    public class InstructorsClassController : Controller
    {
        private readonly IConfiguration _config;

        public InstructorsClassController(IConfiguration config)
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

        // GET: InstructorsClass
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT ins.Id,
                                        ins.FirstName,
                                        ins.LastName,
                                        ins.SlackHandle,
                                        ins.CohortOneId
                                        FROM InstructorsClass ins";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<InstructorsClass> instructorsClasses = new List<InstructorsClass>();
                    while (reader.Read())
                    {
                        InstructorsClass instructorsClass = new InstructorsClass
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortOneId = reader.GetInt32(reader.GetOrdinal("CohortOneId"))
                        };

                        instructorsClasses.Add(instructorsClass);
                    }

                    reader.Close();

                    return View(instructorsClasses);
                }
            }
        }

    

    // GET: InstructorsClass/Details/5
    public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT ins.Id,
                                        ins.FirstName,
                                        ins.LastName,
                                        ins.SlackHandle,
                                        ins.CohortOneId
                                        FROM InstructorsClass ins";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    InstructorsClass instructorsClass = null;
                    if (reader.Read())
                    {
                        instructorsClass = new InstructorsClass
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortOneId = reader.GetInt32(reader.GetOrdinal("CohortOneId"))
                        };

                      
                    reader.Close();
                    return View(instructorsClass);
                    }
                    else
                    {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);
                    }


                }
            }
        }

        // GET: InstructorsClass/Create
        public ActionResult Create()
        {
            InstructorsClassCreateViewModel viewModel = new InstructorsClassCreateViewModel();
            return View();
        }

        // POST: InstructorsClass/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InstructorsClassCreateViewModel model)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO InstructorsClass
                ( Id,FirstName, LastName, SlackHandle, CohortOneId )
                VALUES
                ( @Id, @FirstName, @LastName, @SlackHandle, @CohortOneId )";
                    cmd.Parameters.Add(new SqlParameter("@Id", model.instructorsClass.Id));
                    cmd.Parameters.Add(new SqlParameter("@FirstName", model.instructorsClass.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", model.instructorsClass.LastName));
                    cmd.Parameters.Add(new SqlParameter("@SlackHandle", model.instructorsClass.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@CohortOneId", model.instructorsClass.CohortOneId));
                    await cmd.ExecuteNonQueryAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
        }

        // GET: InstructorsClass/Edit/5
        public ActionResult Edit(int id)
        {
            InstructorsClass instructorsClasses = GetInstructorsClassById(id);
            List<CohortOne> cohorts = GetAllCohorts();
            InstructorsClassEditViewModel viewModel = new InstructorsClassEditViewModel();
            viewModel.instructorsClass = instructorsClasses;
            viewModel.AvailableCohorts = cohorts;
            return View(viewModel);
        }

        // POST: InstructorsClass/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InstructorsClassEditViewModel viewModel)
        {
            InstructorsClass instructorsClasses = viewModel.instructorsClass;
            try
            {
                // TODO: Add update logic here

                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                                    UPDATE InstructorsClass
                                        SET 
                                        FirstName = @FirstName,
                                        LastName = @LastName,
                                        SlackHandle = @SlackHandle,
                                        CohortOneId = @CohortOneId
                                        Where Id = @id";

                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd.Parameters.Add(new SqlParameter("@FirstName", instructorsClasses.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", instructorsClasses.LastName));
                        cmd.Parameters.Add(new SqlParameter("@SlackHandle", instructorsClasses.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@CohortOneId", instructorsClasses.CohortOneId));


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

        // GET: InstructorsClass/Delete/5
        public ActionResult Delete(int id)
        {
            InstructorsClass instructorsClasses = GetInstructorsClassById(id);
            return View(instructorsClasses);
        }

        // POST: InstructorsClass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // TODO: Add delete logic here
                using(SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @" Delete from InstructorsClass Where InstructorsClassId = @id;
                                            Delete from CohortOne Where Id = @id ";

                        cmd.Parameters.Add(new SqlParameter("@Id", id));

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
        private InstructorsClass GetInstructorsClassById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
            SELECT ins.Id,
                ins.FirstName,
                ins.LastName,
                ins.SlackHandle,
                ins.CohortOneId
            FROM InstructorsClass ins
            WHERE ins.Id=@id
        ";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    InstructorsClass instructorsClasses = null;
                    if (reader.Read())
                    {
                        instructorsClasses = new InstructorsClass
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortOneId = reader.GetInt32(reader.GetOrdinal("CohortOneId"))
                        };
                    }

                    reader.Close();

                    return instructorsClasses;
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