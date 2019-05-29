using System.Collections.Generic;
using InstructorExercisesMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using StudentExercises;
using StudentExercisesMVC.Models.ViewModels;

namespace InstructorExercisesMVC.Controllers
{
    public class InstructorController : Controller
    {

        // GET: Instructors
        public ActionResult Index()
        {
            List<Instructor> Instructors = InstructorRepository.GetAllInstructors();
            return View(Instructors);
        }

        // GET: Instructors/Details/5
        public ActionResult Details(int id)
        {
            Instructor Instructor = InstructorRepository.InstructorDetail(id);
            return View(Instructor);
        }
        // GET: Instructors/Create
        public ActionResult Create()
        {
            InstructorCreateViewModel viewModel = new InstructorCreateViewModel();
            return View(viewModel);
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstructorCreateViewModel viewModel)
        {
            try
            {
                InstructorRepository.CreateInstructor(viewModel);
                return View(viewModel);
            }
            catch
            {
                return View();
            }
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int id)
        {
            Instructor instructor = GetInstructorById(id);

            if (instructor == null)
            {
                return NotFound();
            }

            InstructorEditViewModel viewModel = new InstructorEditViewModel
            {
                Cohorts = GetAllCohorts(),
                Instructor = instructor
            };

            return View(viewModel);
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InstructorEditViewModel viewModel)
        {
            try
            {
                InstructorRepository.EditInstructor(id, viewModel);
                return View(viewModel);
            }
            catch
            {
                viewModel.Cohorts = GetAllCohorts();
                return View(viewModel);
            }
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(int id)
        {
            Instructor instructor = GetInstructorById(id);
            if (instructor == null)
            {
                return NotFound();
            }
            else
            {
                return View(instructor);
            }
        }

        // POST: Instructors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Instructor instructor)
        {
            InstructorRepository.DeleteInstructor(id, instructor);
            return View(instructor);
        }


        // Helper Method to create a dynamic dropdown list of Cohorts
        private List<Cohort> GetAllCohorts()
        {
            return InstructorRepository.GetAllCohorts();
        }

        // Helper Method to get instructors by their unique identifier
        private Instructor GetInstructorById(int id)
        {
            return InstructorRepository.GetInstructorById(id);
        }

    }
}