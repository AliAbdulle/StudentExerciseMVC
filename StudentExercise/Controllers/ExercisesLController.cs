using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercise.Models;

namespace StudentExercise.Controllers
{
    public class ExercisesLController : Controller
    {
        private readonly IConfiguration _config;

        public ExercisesLController(IConfiguration config)
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


        // GET: ExercisesL
        public ActionResult Index()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using( SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @" Select e.Id, e.Name, e.Language from ExercisesL e";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<ExercisesL> exercises = new List<ExercisesL>();
                    while (reader.Read())
                    {
                        ExercisesL exercisesL = new ExercisesL
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };
                        exercises.Add(exercisesL);
                    }

                    reader.Close();

                    return View(exercises);
                }
            }
        }

    
    

        // GET: ExerciseL/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @" Select e.Id, e.Name, e.Language from ExercisesL e";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    ExercisesL exercises = null;
                    if (reader.Read())
                    {
                        exercises = new ExercisesL
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };

                        reader.Close();
                        return View(exercises);
                    }
                
                    else
                    {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);

                    }

                }
            }
        }

    

    // GET: ExercisesL/Create
    public ActionResult Create()
        {
            return View();
        }

        // POST: ExercisesL/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExercisesL/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ExerciseL/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExercisesL/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ExercisesL/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}