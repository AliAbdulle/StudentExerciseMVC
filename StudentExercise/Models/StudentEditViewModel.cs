using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercise.Models.ViewModel
{
    public class StudentEditViewModel
    {
        public Students student { get; set;}
        public List<CohortOne> AvailableCohorts { get; set; }
        public List<SelectListItem> AvailableCohortSelectList
        {
            get
            {
                if (AvailableCohorts == null)
                {
                    return null;
                    
                }
                return AvailableCohorts
                                    .Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();
            }
        }
    }
}
