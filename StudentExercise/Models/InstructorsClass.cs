using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercise.Models
{
    public class InstructorsClass
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SlackHandle { get; set; }
        public int CohortOneId { get; set; }
        public CohortOne cohortOne { get; set; }

        List<ExerciseL> Exercises = new List<ExerciseL>();
    }
}
