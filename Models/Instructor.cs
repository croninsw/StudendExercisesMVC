using StudentExercisesAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentExercises
{
    public class Instructor
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Slack Name")]
        public string SlackName { get; set; }

        [Display(Name = "Specialty")]
        public string Specialty { get; set; }

        public int CohortId { get; set; }

        public Cohort Cohort { get; set; }

        public List<Exercise> Exercises = new List<Exercise>();

        public override string ToString()
        {
            return $"{FirstName} {LastName} @{SlackName}";
        }

        // method to add exercise to list of exercises instructor is currently workin on
        public void AddExercise(Student student, Exercise exercise)
        {
            student.Exercises.Add(exercise);
        }

    }
}