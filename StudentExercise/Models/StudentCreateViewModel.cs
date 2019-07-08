using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercise.Models.ViewModel
{
    public class StudentCreateViewModel
    {
        public List<SelectListItem> CohortOne { get; set; }
        public Students student { get; set; }

        private string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public StudentCreateViewModel() { }

        public StudentCreateViewModel(string connectionString)
        {
            _connectionString = connectionString;

            CohortOne = GetAllCohorts()
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                    .ToList();
        }

        private List<CohortOne> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM CohortOne ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<CohortOne> cohortOne = new List<CohortOne>();
                    if (reader.Read())
                    {
                        cohortOne.Add(new CohortOne
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        });
                    }

                    reader.Close();

                    return cohortOne;
                }
            }
        }
    }
}





