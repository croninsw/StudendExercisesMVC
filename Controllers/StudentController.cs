using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StudentExercises;
using StudentExercisesAPI.Models;
using StudentExercisesMVC.Models.ViewModels;
using StudentExercisesMVC.Repositories;

namespace StudentExercisesMVC.Controllers
{
    public class StudentController : Controller
    {

        // GET: Students
        public ActionResult Index()
        {
            List<Student> Students = StudentRepository.GetAllStudents();
            return View(Students);
        }

        // GET: Students/Details/5
        public ActionResult Details(int id)
        {
            Student Student = StudentRepository.StudentDetail(id);
            return View(Student);
        }
        // GET: Students/Create
        public ActionResult Create()
        {
            StudentCreateViewModel viewModel = new StudentCreateViewModel();
            return View(viewModel);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentCreateViewModel viewModel)
        {
            try
            {
                StudentRepository.CreateStudent(viewModel);
                return View(viewModel);
            }
            catch
            {
                return View();
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
                Exercises = GetAllExercises(),
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
                StudentRepository.EditStudent(id, viewModel);
                return View(viewModel);
            }
            catch
            {
                viewModel.Cohorts = GetAllCohorts();
                viewModel.Exercises = GetAllExercises();
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
            StudentRepository.DeleteStudent(id, student);
            return View(student);
        }
        

        // Helper Method to create a dynamic dropdown list of Cohorts
        private List<Cohort> GetAllCohorts() 
        {
            return StudentRepository.GetAllCohorts();
        }

        // Helper Method to get students by their unique identifier
        private Student GetStudentById(int id)
        {
            return StudentRepository.GetStudentById(id);
        }

        // Helper Method to create a dynamic dropdown list of Exercises
        private List<Exercise> GetAllExercises()
        {
            return StudentRepository.GetAllExercises();
        }
    }
}