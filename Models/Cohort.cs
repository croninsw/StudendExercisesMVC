using StudentExercisesAPI.Models;
using System;
using System.Collections.Generic;

namespace StudentExercises
{
    public class Cohort
    {
        public int Id { get; set; }
        // private string to make cohort unique
        public string CohortName { get; set; }

        // public list of students currently enrolled
        public List<Student> Students = new List<Student>();

        // public list of instructors currently working
        public List<Instructor> Instructors = new List<Instructor>();
    }
}