using StudentExercises;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesAPI.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Slack Name")]
        public string SlackName { get; set; }

        public int CohortId { get; set; }

        public Cohort Cohort { get; set; }

        public List<Exercise> Exercises = new List<Exercise>();

        public override string ToString()
        {
            return $"{FirstName} {LastName} @{SlackName}";
        }
    }
}