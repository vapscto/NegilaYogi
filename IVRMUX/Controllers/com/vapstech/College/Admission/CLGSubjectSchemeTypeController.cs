using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CLGSubjectSchemeTypeController : Controller
    {
        CLGSubjectSchemeTypeDelegate _scheme = new CLGSubjectSchemeTypeDelegate();
        [HttpGet]
        public AdmCollegeSchemeTypeDTO getschema()
        {
            AdmCollegeSchemeTypeDTO data = new AdmCollegeSchemeTypeDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _scheme.getschema(data);
        }
        [Route("getsubject")]
        public AdmCollegeSchemeTypeDTO getsubject()
        {
            AdmCollegeSchemeTypeDTO data = new AdmCollegeSchemeTypeDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _scheme.getsubject(data);
        }
        [Route("savetype")]
        public AdmCollegeSchemeTypeDTO savetype([FromBody] AdmCollegeSchemeTypeDTO data)
        {
             data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _scheme.savetype(data);
        }

        [Route("savename")]
        public AdmCollegeSchemeTypeDTO savename([FromBody] AdmCollegeSchemeTypeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _scheme.savename(data);
        }
        [Route("activedeactivebatch")]
        public AdmCollegeSchemeTypeDTO activedeactivebatch([FromBody] AdmCollegeSchemeTypeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _scheme.activedeactivebatch(data);
        }
        [Route("activedeactivebatch1")]
        public AdmCollegeSchemeTypeDTO activedeactivebatch1([FromBody] AdmCollegeSchemeTypeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _scheme.activedeactivebatch1(data);
        }
        [Route("getcombination/{id:int}")]
        public Adm_Prv_Sch_CombinationDTO getcombination(int id)
        {
            Adm_Prv_Sch_CombinationDTO data = new Adm_Prv_Sch_CombinationDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _scheme.getcombination(data);
        }
        [Route("savecombination")]
        public Adm_Prv_Sch_CombinationDTO savecombination([FromBody] Adm_Prv_Sch_CombinationDTO data )
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _scheme.savecombination(data);
        }
        [Route("activedeactivecomb")]
        public Adm_Prv_Sch_CombinationDTO activedeactivecomb([FromBody] Adm_Prv_Sch_CombinationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _scheme.activedeactivecomb(data);
        }
        


    }
}
