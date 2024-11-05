using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MarksEntryHHSFacadeController : Controller
    {
        public MarksEntryHHSInterface int_ssse;
        public MarksEntryHHSFacadeController(MarksEntryHHSInterface inter)
        {
            int_ssse = inter;
        }

        [HttpPost]
        [Route("Getdetails")]
        public MarksEntryHHSDTO Getdetails([FromBody] MarksEntryHHSDTO id)
        {
            return int_ssse.getdetails(id);
        }

        [Route("get_classes")]
        public MarksEntryHHSDTO get_classes([FromBody] MarksEntryHHSDTO id)
        {
            return int_ssse.get_classes(id);
        }

        [Route("get_sections")]
        public MarksEntryHHSDTO get_sections([FromBody] MarksEntryHHSDTO id)
        {
            return int_ssse.get_sections(id);
        }

        [Route("get_exams")]
        public MarksEntryHHSDTO get_exams([FromBody] MarksEntryHHSDTO id)
        {
            return int_ssse.get_exams(id);
        }

        [Route("get_subjects")]
        public MarksEntryHHSDTO get_subjects([FromBody] MarksEntryHHSDTO id)
        {
            return int_ssse.get_subjects(id);
        }

        [Route("onsearch")]
        public MarksEntryHHSDTO onsearch([FromBody] MarksEntryHHSDTO id)
        {
            return int_ssse.onsearch(id);
        }

        [Route("SaveMarks")]
        public MarksEntryHHSDTO SaveMarks([FromBody] MarksEntryHHSDTO id)
        {
            return int_ssse.SaveMarks(id);
        }
    }
}