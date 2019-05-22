using System;

namespace StudentExercises
{
    public class Exercise
    {
        // private classes to keep exercises unique
        public string ExerciseName { get; }
        public string ExerciseLanguage { get; set; }

        // when an exercise is instantiated, its name should be created
        // it's language COULD change so at this point it is a public class

        public Exercise(string name) {
            ExerciseName = name;
        }

        public override string ToString() {
            return ExerciseName;
        }
    }
}