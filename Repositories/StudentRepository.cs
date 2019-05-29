using Microsoft.Extensions.Configuration;
using StudentExercises;
using StudentExercisesAPI.Models;
using StudentExercisesMVC.Models.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace StudentExercisesMVC.Repositories
{
    public class StudentRepository
    {
        private static IConfiguration _config;

        public static void SetConfig(IConfiguration configuration)
        {
            _config = configuration;
        }

        public static SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // provides a list of students
        public static List<Student> GetAllStudents()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
            SELECT s.Id,
                s.FirstName,
                s.LastName,
                s.SlackName,
                s.CohortId
            FROM Student s
             ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> students = new List<Student>();
                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackName = reader.GetString(reader.GetOrdinal("SlackName")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                        };

                        students.Add(student);
                    }

                    reader.Close();

                    return students;
                }
            }
        }

        // return a list of all cohorts
        public static List<Cohort> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, CohortName 
                                        from Cohort;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();

                    while (reader.Read())
                    {
                        cohorts.Add(new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        });
                    }
                    reader.Close();

                    return cohorts;
                }
            }
        }

        // provides the details of a single student by Student.Id
        public static Student StudentDetail(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName, s.SlackName, s.CohortId
                                        FROM Student s
                                        WHERE s.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Student student = null;

                    if (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackName = reader.GetString(reader.GetOrdinal("SlackName")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                        };
                    }

                    reader.Close();

                    return student;
                }
            }
        }

        // create a new instance of a student using a view model to join separate tables of information
        public static StudentCreateViewModel CreateStudent(StudentCreateViewModel viewModel)
        {
            using (SqlConnection conn = Connection)
            {

                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Student (FirstName, LastName, SlackName, CohortId)
                                                OUTPUT INSERTED.Id
                                                     VALUES (@FirstName, @LastName, @SlackName, @CohortId)";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", viewModel.Student.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", viewModel.Student.LastName));
                    cmd.Parameters.Add(new SqlParameter("@SlackName", viewModel.Student.SlackName));
                    cmd.Parameters.Add(new SqlParameter("@CohortId", viewModel.Student.CohortId));


                    int newId = (int)cmd.ExecuteScalar();
                    viewModel.Student.Id = newId;


                    return viewModel;
                }
            }
        }

        // edit a currently instantiated student using a unique identifier
        public static StudentEditViewModel EditStudent(int id, StudentEditViewModel viewModel)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Student
                                            SET FirstName = @FirstName,
                                                LastName = @LastName,
                                                SlackName = @SlackName,
                                                CohortId = @CohortId
                                            WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@FirstName", viewModel.Student.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", viewModel.Student.LastName));
                    cmd.Parameters.Add(new SqlParameter("@SlackName", viewModel.Student.SlackName));
                    cmd.Parameters.Add(new SqlParameter("@CohortId", viewModel.Student.CohortId));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                    //return RedirectToAction(nameof(Index));
                    return viewModel;
                }
            }
        }

        // delete a student using a unique identifier
        public static Student DeleteStudent(int id, Student student)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM StudentExercise 
                                             WHERE StudentId = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();

                }
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Student 
                                             WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();

                    //return RedirectToAction(nameof(Index));
                    return student;
                }
            }
        }

        // Helper method to return a student by their unique identifier
        public static Student GetStudentById(int id)
        {
            using (SqlConnection conn = Connection)
            {

                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName, s.SlackName, s.CohortId
                                    FROM Student s
                                    WHERE s.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Student student = null;

                    if (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackName = reader.GetString(reader.GetOrdinal("SlackName")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))

                        };
                    }

                    reader.Close();
                    return student;
                }
            }
        }

        // Helper method to return a list of all exercises a student can be assigned
        public static List<Exercise> GetAllExercises()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, ExerciseName 
                                        from Exercise;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();

                    while (reader.Read())
                    {
                        exercises.Add(new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ExerciseName = reader.GetString(reader.GetOrdinal("ExerciseName")),
                            ExerciseLanguage = reader.GetString(reader.GetOrdinal("ExerciseLanguage"))
                        });
                    }
                    reader.Close();

                    return exercises;
                }
            }
        }
    }
}
