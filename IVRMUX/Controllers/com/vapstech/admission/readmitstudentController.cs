using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    // [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    public class readmitstudentController : Controller
    {
        readmitstudentDelegate readmitstudent = new readmitstudentDelegate();


        [Route("getalldetails/{id:int}")]
        public SchoolYearWiseStudentDTO Get(int id)
        {
            SchoolYearWiseStudentDTO SchoolYearWiseStudentDTO = new SchoolYearWiseStudentDTO();
            SchoolYearWiseStudentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return readmitstudent.getInstitutiondata(SchoolYearWiseStudentDTO);
        }
        //get student details by year

        [HttpGet]
        [Route("GetStudentListByYear/{id:int}")]
        public SchoolYearWiseStudentDTO GetStudentListByYear(long id)
        {
            SchoolYearWiseStudentDTO SchoolYearWiseStudentDTO = new SchoolYearWiseStudentDTO();
            SchoolYearWiseStudentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return readmitstudent.getStudentdataByYear(id);
        }

        // POST api/values
        [HttpPost]
        //[Route("SaveInstitution")]
        public readmitstudentDTO savereadmit_student([FromBody] readmitstudentDTO Ins)
        {
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            Ins.userid = UserId;
            Ins.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return readmitstudent.savereadmit_student(Ins);
        }

        [Route("GetStudentListByYearAndCLass")]
        public SchoolYearWiseStudentDTO GetStudentListByYearAndCLass_CS([FromBody] SchoolYearWiseStudentDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return readmitstudent.GetstudentdetailsbyYearandclass(yearclass);
        }
        [Route("getnewjoinlist")]
        public SchoolYearWiseStudentDTO getnewjoinlist([FromBody]SchoolYearWiseStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return readmitstudent.getnewjoinlist(data);
        }
    }

}
