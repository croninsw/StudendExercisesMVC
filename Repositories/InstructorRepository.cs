using Microsoft.Extensions.Configuration;
using StudentExercises;
using StudentExercisesAPI.Models;
using StudentExercisesMVC.Models.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace InstructorExercisesMVC.Repositories
{
    public class InstructorRepository
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
        // provides a list of instructors
        public static List<Instructor> GetAllInstructors()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
            SELECT i.Id,
                i.FirstName,
                i.LastName,
                i.SlackName,
                i.Specialty,
                i.CohortId
            FROM Instructor i
             ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Instructor> instructors = new List<Instructor>();
                    while (reader.Read())
                    {
                        Instructor instructor = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackName = reader.GetString(reader.GetOrdinal("SlackName")),
                            Specialty = reader.GetString(reader.GetOrdinal("Specialty")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                        };

                        instructors.Add(instructor);
                    }

                    reader.Close();

                    return instructors;
                }
            }
        }

       

        // provides the details of a single instructor by Instructor.Id
        public static Instructor InstructorDetail(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT i.Id, i.FirstName, i.LastName, i.SlackName, i.Specialty, i.CohortId
                                        FROM Instructor i
                                        WHERE i.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Instructor instructor = null;

                    if (reader.Read())
                    {
                        instructor = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackName = reader.GetString(reader.GetOrdinal("SlackName")),
                            Specialty = reader.GetString(reader.GetOrdinal("Specialty")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                        };
                    }

                    reader.Close();

                    return instructor;
                }
            }
        }

        // create a new instance of an instructor using a view model to join separate tables of information
        public static InstructorCreateViewModel CreateInstructor(InstructorCreateViewModel viewModel)
        {
            using (SqlConnection conn = Connection)
            {

                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Instructor (FirstName, LastName, SlackName, Specialty, CohortId)
                                                OUTPUT INSERTED.Id
                                                     VALUES (@FirstName, @LastName, @SlackName, @Specialty, @CohortId)";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", viewModel.Instructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", viewModel.Instructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@SlackName", viewModel.Instructor.SlackName));
                    cmd.Parameters.Add(new SqlParameter("@Specialty", viewModel.Instructor.Specialty));
                    cmd.Parameters.Add(new SqlParameter("@CohortId", viewModel.Instructor.CohortId));

                    int newId = (int)cmd.ExecuteScalar();
                    viewModel.Instructor.Id = newId;


                    return viewModel;
                }
            }
        }

        // edit a currently instantiated student using a unique identifier
        public static InstructorEditViewModel EditInstructor(int id, InstructorEditViewModel viewModel)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Instructor
                                            SET FirstName = @FirstName,
                                            LastName = @LastName,
                                            SlackName = @SlackName,
                                            Specialty = @Specialty,
                                            CohortId = @CohortId
                                            WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@FirstName", viewModel.Instructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", viewModel.Instructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@SlackName", viewModel.Instructor.SlackName));
                    cmd.Parameters.Add(new SqlParameter("@Specialty", viewModel.Instructor.Specialty));
                    cmd.Parameters.Add(new SqlParameter("@CohortId", viewModel.Instructor.CohortId));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                    return viewModel;
                }
            }
        }

        // delete a student using a unique identifier
        public static Instructor DeleteInstructor(int id, Instructor instructor)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM StudentExercise 
                                             WHERE InstructorId = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();

                }
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Instructor 
                                             WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();

                    return instructor;
                }
            }
        }

        // Helper method to return an instructor by their unique identifier
        public static Instructor GetInstructorById(int id)
        {
            using (SqlConnection conn = Connection)
            {

                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
            SELECT i.Id,
                i.FirstName,
                i.LastName,
                i.SlackName,
                i.Specialty,
                i.CohortId
            FROM Instructor i
             ";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Instructor instructor = null;

                    if (reader.Read())
                    {
                        instructor = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackName = reader.GetString(reader.GetOrdinal("SlackName")),
                            Specialty = reader.GetString(reader.GetOrdinal("Specialty")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))

                        };
                    }

                    reader.Close();
                    return instructor;
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
    }
}
