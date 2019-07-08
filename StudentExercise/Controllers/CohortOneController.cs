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
    public class CohortOneController : Controller
    {
        private readonly IConfiguration _config;

        public CohortOneController(IConfiguration config)
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


        // GET: CohortOne
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT c.Id,
                                           c.Name
                                        FROM CohortOne c";

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

                    return View(cohortOnes);
                }
            }
        }

    

    // GET: CohortOne/Details/5
    public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT c.Id,
                                        c.Name  
                                        FROM CohortOne c
                                        Where Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    CohortOne cohortOne = null;
                    if (reader.Read())
                    {
                        cohortOne = new CohortOne
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };


                        reader.Close();

                        return View(cohortOne);
                    }
                    else
                    {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);
                    }

                }
            }
        }

        // GET: CohortOne/Create
        public ActionResult Create()
        {
            CohortOneCreateViewModel viewModel = new CohortOneCreateViewModel();
            return View();
        }

        // POST: CohortOne/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CohortOneCreateViewModel model  )
        {
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                       
                            // TODO: Add insert logic here
                            cmd.CommandText = @"INSERT INTO CohortOne
                                        ( Id, Name ) VALUES ( @Id, @Name)";
                            cmd.Parameters.Add(new SqlParameter("@Id", model.cohortOne.Id));
                            cmd.Parameters.Add(new SqlParameter("@Name", model.cohortOne.Name));

                            cmd.ExecuteNonQueryAsync();

                            return RedirectToAction(nameof(Index));
                        
                    }
                }
            }
        }
        // GET: CohortOne/Edit/5
        public ActionResult Edit(int id)
        {
            CohortOne cohortOne = GetCohortOneById(id);
            List<CohortOne> cohorts = GetAllCohortOnes();
            CohortOneEditViewModel viewModel = new CohortOneEditViewModel();
            viewModel.cohorts = cohortOne;
            viewModel.AvailableCohorts = cohorts;
            return View(viewModel);
        }

        // POST: CohortOne/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CohortOneEditViewModel viewModel)
        {
            CohortOne cohortOne = viewModel.cohorts;
            try
            { 
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    UPDATE CohortOne
                                        SET 
                                        Id = @id,
                                        Name = @Name
                                        Where Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.Parameters.Add(new SqlParameter("@Name", cohortOne.Name));

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

        // GET: CohortOne/Delete/5
        public ActionResult Delete(int id)
        {
            CohortOne cohortOne = GetCohortOneById(id);
            return View(cohortOne);
        }

        // POST: CohortOne/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            { 
             using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                        cmd.CommandText = @"Delete  from CohortOne Where Id = @id";

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

        private CohortOne GetCohortOneById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
            SELECT c.Id,
                c.Name     
            FROM CohortOne c
            WHERE c.Id=@id
        ";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    CohortOne cohortOne = null;
                    if (reader.Read())
                    {
                        cohortOne = new CohortOne
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }

                    reader.Close();

                    return cohortOne;
                }
            }
        }
        private List<CohortOne> GetAllCohortOnes()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @" SELECT c.Id, c.Name FROM CohortOne c";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<CohortOne> cohortOne = new List<CohortOne>();
                    while (reader.Read())
                    {
                        CohortOne cohortOnes = new CohortOne
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        cohortOne.Add(cohortOnes);
                    }

                    reader.Close();

                    return cohortOne;
                }
            }
        }
    }
}