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
    public class SectionAllotmentController : Controller
    {
        SectionAllotmentDelegate SectionAllotment = new SectionAllotmentDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public SchoolYearWiseStudentDTO Get(int id)
        {
            SchoolYearWiseStudentDTO SchoolYearWiseStudentDTO = new SchoolYearWiseStudentDTO();
            SchoolYearWiseStudentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            SchoolYearWiseStudentDTO.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return SectionAllotment.getInstitutiondata(SchoolYearWiseStudentDTO);
        }       

        [HttpGet]
        [Route("GetStudentListByYear/{id:int}")]
        public SchoolYearWiseStudentDTO GetStudentListByYear(long id)
        {
            SchoolYearWiseStudentDTO SchoolYearWiseStudentDTO = new SchoolYearWiseStudentDTO();
            SchoolYearWiseStudentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            SchoolYearWiseStudentDTO.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return SectionAllotment.getStudentdataByYear(id);
        }
      
        [HttpPost]       
        public SchoolYearWiseStudentDTO SaveSectionAllotment([FromBody] SchoolYearWiseStudentDTO Ins)
        {
            Ins.LoginId=Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            Ins.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.saveSectionAllotmentdetails(Ins);
        }

        [Route("GetStudentListByYearAndCLass")]
        public SchoolYearWiseStudentDTO GetStudentListByYearAndCLass_CS([FromBody] SchoolYearWiseStudentDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            yearclass.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return SectionAllotment.GetstudentdetailsbyYearandclass(yearclass);
        }

        [Route("GetStudentListByURN")]
        public SchoolYearWiseStudentDTO GetStudentListByURN([FromBody] SchoolYearWiseStudentDTO data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return SectionAllotment.GetStudentListByURN(data);
        }

        [Route("GetStudentListByURNsave")]
        public Student_Update_RollNumber GetStudentListByURNsave([FromBody]Student_Update_RollNumber data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return SectionAllotment.GetStudentListByURNsave(data);
        }

        //Change Clas 
        [Route("GetChangeClassDetails/{id:int}")]
        public SchoolYearWiseStudentDTO GetChangeClassDetails(long id)
        {
            SchoolYearWiseStudentDTO SchoolYearWiseStudentDTO = new SchoolYearWiseStudentDTO();
            SchoolYearWiseStudentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            SchoolYearWiseStudentDTO.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            SchoolYearWiseStudentDTO.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return SectionAllotment.GetChangeClassDetails(SchoolYearWiseStudentDTO);
        }

        [Route("GetStudentListByYearCLS")]
        public SchoolYearWiseStudentDTO GetStudentListByYearCLS([FromBody]SchoolYearWiseStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return SectionAllotment.GetStudentListByYearCLS(data);
        }

        [Route("onstudentnamechange")]
        public SchoolYearWiseStudentDTO onstudentnamechange([FromBody]SchoolYearWiseStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return SectionAllotment.onstudentnamechange(data);
        }

        [Route("DeleteFeeMapping")]
        public SchoolYearWiseStudentDTO DeleteFeeMapping([FromBody]SchoolYearWiseStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return SectionAllotment.DeleteFeeMapping(data);
        }

        [Route("SaveClassChange")]
        public SchoolYearWiseStudentDTO SaveClassChange([FromBody]SchoolYearWiseStudentDTO data)
        {
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.SaveClassChange(data);
        }
        //aded By Kavita 
        [Route("SaveClassFeeChange")]
        public SchoolYearWiseStudentDTO SaveClassFeeChange([FromBody]SchoolYearWiseStudentDTO data)
        {
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.SaveClassFeeChange(data);
        }
    }
}
