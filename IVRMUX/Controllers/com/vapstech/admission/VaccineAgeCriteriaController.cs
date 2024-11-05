using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class VaccineAgeCriteriaController : Controller
    {
        VaccineAgeCriteriaDelegate ad = new VaccineAgeCriteriaDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("OnLoadVaccineAgeCriteriaDetails/{id:int}")]
        public VaccineAgeCriteriaDTO OnLoadVaccineAgeCriteriaDetails(int id)
        {
            VaccineAgeCriteriaDTO data = new VaccineAgeCriteriaDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId= Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return ad.OnLoadVaccineAgeCriteriaDetails(data);
        }

        [Route("SaveVaccineAgeDetails")]
        public VaccineAgeCriteriaDTO SaveVaccineAgeDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return ad.SaveVaccineAgeDetails(data);
        }

        [Route("EditVaccineAgeDetails")]
        public VaccineAgeCriteriaDTO EditVaccineAgeDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return ad.EditVaccineAgeDetails(data);
        }

        [Route("ActiveDeactiveVaccineAgeDetails")]
        public VaccineAgeCriteriaDTO ActiveDeactiveVaccineAgeDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return ad.ActiveDeactiveVaccineAgeDetails(data);
        }

        [Route("OnClickViewDetails")]
        public VaccineAgeCriteriaDTO OnClickViewDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return ad.OnClickViewDetails(data);
        }

        [Route("ActiveDeactiveVaccineDetails")]
        public VaccineAgeCriteriaDTO ActiveDeactiveVaccineDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return ad.ActiveDeactiveVaccineDetails(data);
        }

        // Vaccine Student Details
        [Route("OnLoadVaccineStudentDetails/{id:int}")]
        public VaccineAgeCriteriaDTO OnLoadVaccineStudentDetails(int id)
        {
            VaccineAgeCriteriaDTO data = new VaccineAgeCriteriaDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return ad.OnLoadVaccineStudentDetails(data);
        }

        [Route("GetStudentDetailsBySearch")]
        public VaccineAgeCriteriaDTO GetStudentDetailsBySearch([FromBody] VaccineAgeCriteriaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return ad.GetStudentDetailsBySearch(data);
        }

        [Route("SearchVaccineStudentDetails")]
        public VaccineAgeCriteriaDTO SearchVaccineStudentDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return ad.SearchVaccineStudentDetails(data);
        }

        [Route("SaveStudentVaccineDetails")]
        public VaccineAgeCriteriaDTO SaveStudentVaccineDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return ad.SaveStudentVaccineDetails(data);
        }

        [Route("OnClickViewStudentVaccineDetails")]
        public VaccineAgeCriteriaDTO OnClickViewStudentVaccineDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return ad.OnClickViewStudentVaccineDetails(data);
        }

        // Illness Student Details 
        [Route("OnLoadIllnessStudentDetails/{id:int}")]
        public VaccineAgeCriteriaDTO OnLoadIllnessStudentDetails(int id)
        {
            VaccineAgeCriteriaDTO data = new VaccineAgeCriteriaDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return ad.OnLoadIllnessStudentDetails(data);
        }

    }
}
