using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using ExamServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Promotion_Marks_UpdateFacadeController : Controller
    {
        public Promotion_Marks_UpdateInterface inter_fr;
        public Promotion_Marks_UpdateFacadeController(Promotion_Marks_UpdateInterface _intr)
        {
            inter_fr = _intr;
        }

        [HttpPost]
        [Route("Getdetails")]
        public Promotion_Marks_UpdateDTO Getdetails([FromBody] Promotion_Marks_UpdateDTO data)
        {
            return inter_fr.Getdetails(data);
        }
        [Route("get_categories")]
        public Promotion_Marks_UpdateDTO get_categories([FromBody] Promotion_Marks_UpdateDTO data)
        {
            return inter_fr.get_categories(data);
        }
        [Route("get_classes")]
        public Promotion_Marks_UpdateDTO get_classes([FromBody] Promotion_Marks_UpdateDTO data)
        {
            return inter_fr.get_classes(data);
        }
        [Route("get_sections")]
        public Promotion_Marks_UpdateDTO get_sections([FromBody] Promotion_Marks_UpdateDTO data)
        {
            return inter_fr.get_sections(data);
        }
        [Route("get_subjects")]
        public Promotion_Marks_UpdateDTO get_subjects([FromBody] Promotion_Marks_UpdateDTO data)
        {
            return inter_fr.get_subjects(data);
        }
        [Route("get_prommarks")]
        public Promotion_Marks_UpdateDTO get_prommarks([FromBody] Promotion_Marks_UpdateDTO data)
        {
            return inter_fr.get_prommarks(data);
        }
    }
}
