using StudentExercisesAPI.Models;
using System;
using System.Collections.Generic;

namespace StudentExercises
{
    public class Instructor : NSSPerson
    {
        // private classes to set instructors cohort
        private Cohort _cohort { get; set; }

        // public class to allow for a instructor to be moved between cohorts
        public Cohort cohort
        {
            get
            {
                return _cohort;
            }
            set
            {
                if (_cohort != null)
                {
                    _cohort.Instructors.Remove(this);
                    _cohort = value;
                    _cohort.Instructors.Add(this);
                }
                else
                {
                    _cohort = value;
                    _cohort.Instructors.Add(this);
                }
            }
        }

        public Instructor(string FirstName, string LastName, string SlackName) : base(FirstName, LastName, SlackName){}

        // method to add exercise to list of exercises instructor is currently workin on
        public void AddExercise(Student student, Exercise exercise)
        {
            student.Exercises.Add(exercise);
        }

    }
}