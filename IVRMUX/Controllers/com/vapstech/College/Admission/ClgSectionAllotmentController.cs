using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
//using PreadmissionDTOs.com.vaps.College.Admission;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class ClgSectionAllotmentController : Controller
    {
        ClgSectionAllotmentDelegate SectionAllotment = new ClgSectionAllotmentDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public ClgYearWiseStudentDTO Get(int id)
        {
            ClgYearWiseStudentDTO ClgYearWiseStudentDTO = new ClgYearWiseStudentDTO();
            ClgYearWiseStudentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.getInstitutiondata(ClgYearWiseStudentDTO);
        }
        //get student details by year

        [HttpGet]
        [Route("GetStudentListByYear/{id:int}")]
        public ClgYearWiseStudentDTO GetStudentListByYear(long id)
        {
            ClgYearWiseStudentDTO ClgYearWiseStudentDTO = new ClgYearWiseStudentDTO();
            ClgYearWiseStudentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return SectionAllotment.getStudentdataByYear(id);
        }

        // POST api/values
        [HttpPost]
        //[Route("SaveInstitution")]
        public ClgYearWiseStudentDTO SaveSectionAllotment([FromBody] ClgYearWiseStudentDTO Ins)
        {
            Ins.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            Ins.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.saveSectionAllotmentdetails(Ins);
        }

        [Route("GetStudentListByYearAndCLass")]
        public ClgYearWiseStudentDTO GetStudentListByYearAndCLass_CS([FromBody] ClgYearWiseStudentDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.GetstudentdetailsbyYearandclass(yearclass);
        }

        [Route("Getbranch")]
        public ClgYearWiseStudentDTO Getbranch([FromBody] ClgYearWiseStudentDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.Getbranch(yearclass);
        }
        [Route("Get_academiccourse")]
        public ClgYearWiseStudentDTO Get_academiccourse([FromBody] ClgYearWiseStudentDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.Get_academiccourse(yearclass);
        }

        [Route("Get_semister")]
        public ClgYearWiseStudentDTO Get_semister([FromBody] ClgYearWiseStudentDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.Get_semister(yearclass);
        }

        

        [Route("GetPromocourse")]
        public ClgYearWiseStudentDTO GetPromocourse([FromBody] ClgYearWiseStudentDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.GetPromocourse(yearclass);
        }

        [Route("GetPromobranch")]
        public ClgYearWiseStudentDTO GetPromobranch([FromBody] ClgYearWiseStudentDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.GetPromobranch(yearclass);
        }

        [Route("GetPromosem")]
        public ClgYearWiseStudentDTO GetPromosem([FromBody] ClgYearWiseStudentDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.GetPromosem(yearclass);
        }


        [Route("promsemonchange")]
        public ClgYearWiseStudentDTO promsemonchange([FromBody] ClgYearWiseStudentDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.promsemonchange(yearclass);
        }
        

        [Route("GetStudentListByURN")]
        public ClgYearWiseStudentDTO GetStudentListByURN([FromBody] ClgYearWiseStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.GetStudentListByURN(data);
        }
        [Route("GetStudentListByURNsave")]
        public ClgYearWiseStudentDTO GetStudentListByURNsave([FromBody]ClgYearWiseStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.GetStudentListByURNsave(data);
        }

        // Year Loss Section
        [Route("OnChangeyearlossAcademic")]
        public ClgYearWiseStudentDTO OnChangeyearlossAcademic([FromBody] ClgYearWiseStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.OnChangeyearlossAcademic(data);
        }
        [Route("Getyearlossbranch")]
        public ClgYearWiseStudentDTO Getyearlossbranch([FromBody] ClgYearWiseStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.Getyearlossbranch(data);
        }
        [Route("Getyearlosssem")]
        public ClgYearWiseStudentDTO Getyearlosssem([FromBody] ClgYearWiseStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.Getyearlosssem(data);
        }
        [Route("GetStudentListByYear_yearloss1")]
        public ClgYearWiseStudentDTO GetStudentListByYear_yearloss1([FromBody] ClgYearWiseStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.GetStudentListByYear_yearloss1(data);
        }
        
    }
}
