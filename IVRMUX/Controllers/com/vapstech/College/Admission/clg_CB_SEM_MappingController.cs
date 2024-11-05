using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
//using PreadmissionDTOs.com.vaps.College.Admission;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class clg_CB_SEM_MappingController : Controller
    {
        clg_CB_SEM_MappingDelegate SectionAllotment = new clg_CB_SEM_MappingDelegate();




        [Route("Getdetails/")]
        public clg_CB_SEM_MappingDTO Get(clg_CB_SEM_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.getInstitutiondata(data);
        }


        //get student details by year

        [HttpGet]
        [Route("GetStudentListByYear/{id:int}")]
        public clg_CB_SEM_MappingDTO GetStudentListByYear(long id)
        {
            clg_CB_SEM_MappingDTO clg_CB_SEM_MappingDTO = new clg_CB_SEM_MappingDTO();
            clg_CB_SEM_MappingDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return SectionAllotment.getStudentdataByYear(id);
        }

        // POST api/values
        [HttpPost]
        //[Route("SaveInstitution")]
        public clg_CB_SEM_MappingDTO SaveSectionAllotment([FromBody] clg_CB_SEM_MappingDTO Ins)
        {
            Ins.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            Ins.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.saveSectionAllotmentdetails(Ins);
        }

        [Route("GetStudentListByYearAndCLass")]
        public clg_CB_SEM_MappingDTO GetStudentListByYearAndCLass_CS([FromBody] clg_CB_SEM_MappingDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.GetstudentdetailsbyYearandclass(yearclass);
        }

        [Route("Getbranch")]
        public clg_CB_SEM_MappingDTO Getbranch([FromBody] clg_CB_SEM_MappingDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.Getbranch(yearclass);
        }

        [Route("savesem")]
        public clg_CB_SEM_MappingDTO savesem([FromBody] clg_CB_SEM_MappingDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.savesem(yearclass);
        }

        [Route("Editrecord/{id:int}")]
        public clg_CB_SEM_MappingDTO Editrecord(int id)
        {
            clg_CB_SEM_MappingDTO yearclass = new clg_CB_SEM_MappingDTO();
            yearclass.AMCOBM_Id = id;
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.Editrecord(yearclass);
        }

        [Route("deactivate")]
        public clg_CB_SEM_MappingDTO deactivate([FromBody] clg_CB_SEM_MappingDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.deactivate(yearclass);
        }


        [Route("sempopup")]
        public clg_CB_SEM_MappingDTO sempopup([FromBody] clg_CB_SEM_MappingDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.sempopup(yearclass);
        }


        [Route("GetStudentListByURN")]
        public clg_CB_SEM_MappingDTO GetStudentListByURN([FromBody] clg_CB_SEM_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.GetStudentListByURN(data);
        }
        
    }
}
