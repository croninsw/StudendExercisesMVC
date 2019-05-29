using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercises;
using System.Collections.Generic;
using System.Linq;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class InstructorEditViewModel
    {
        public Instructor Instructor { get; set; }
        public List<Cohort> Cohorts { get; set; }
        public List<SelectListItem> CohortOptions
        {
            get
            {
                if (Cohorts == null)
                {
                    return null;
                }

                return Cohorts.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CohortName
                }).ToList();
            }
        }

    }
}