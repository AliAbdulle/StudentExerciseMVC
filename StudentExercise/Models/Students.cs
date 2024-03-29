﻿using System.Collections.Generic;

namespace StudentExercise.Models
{
    public class Students
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SlackHandle { get; set; }
        public int CohortOneId { get; set; }
        public CohortOne cohortOne { get; set; }

        List<ExercisesL> Exercises = new List<ExercisesL>();
    }
}
