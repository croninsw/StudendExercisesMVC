using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercises;
using StudentExercisesAPI.Models;
using StudentExercisesMVC.Models;
using StudentExercisesMVC.Models.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StudentExercisesMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly IConfiguration _config;

        public StudentController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: Students
        public ActionResult Index()
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

                    return View(students);
                }
            }
        }

        // GET: Students/Details/5
        public ActionResult Details(int id)
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

                    return View(student);
                }
            }
        }
        // GET: Students/Create
        public ActionResult Create()

        {
            StudentCreateViewModel viewModel = new StudentCreateViewModel(_config.GetConnectionString("DefaultConnection"));

            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentCreateViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {

                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Student (FirstName, LastName, SlackName, CohortId)
                                                     VALUES (@FirstName, @LastName, @SlackName, @CohortId)";
                        cmd.Parameters.Add(new SqlParameter("@FirstName", viewModel.Student.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", viewModel.Student.LastName));
                        cmd.Parameters.Add(new SqlParameter("@SlackName", viewModel.Student.SlackName));
                        cmd.Parameters.Add(new SqlParameter("@CohortId", viewModel.Student.CohortId));


                        cmd.ExecuteNonQuery();


                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                return View(viewModel);
            }
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int id)
        {
            Student student = GetStudentById(id);

            if (student == null)
            {
                return NotFound();
            }

            StudentEditViewModel viewModel = new StudentEditViewModel
            {
                Cohorts = GetAllCohorts(),
                Student = student
            };

            return View(viewModel);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, StudentEditViewModel viewModel)
        {
            try
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
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                viewModel.Cohorts = GetAllCohorts();
                return View(viewModel);
            }
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int id)
        {
            Student student = GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            else
            {
                return View(student);
            }

        }

        // POST: Students/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Student student)
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

                    return RedirectToAction(nameof(Index));
                }
            }

        }

        // Helper method to find individual student by their Id in the DB
        private Student GetStudentById(int id) 
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

        // Helper Method to create a dynamic dropdown list of Cohorts
        private List<Cohort> GetAllCohorts() 
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