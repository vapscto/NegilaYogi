using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission
{
    [Route("api/[controller]")]
    public class MasterCycleYearMappingController : Controller
    {
        public MasterCycleYearMappingDelegate _delg = new MasterCycleYearMappingDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }  
        
        // Master Cycle
        
        [Route("getalldetails/{id:int}")]
        public MasterCycleYearMappingDTO getalldetails(int id)
        {
            MasterCycleYearMappingDTO data = new MasterCycleYearMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NCMACY_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getalldetails(data);
        }
        [Route("savedetails")]
        public MasterCycleYearMappingDTO savedetails([FromBody] MasterCycleYearMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NCMACY_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedetails(data);
        }
        [Route("activedeactivedetails")]
        public MasterCycleYearMappingDTO activedeactivedetails([FromBody] MasterCycleYearMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NCMACY_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.activedeactivedetails(data);
        }
        [Route("editdetails")]
        public MasterCycleYearMappingDTO editdetails([FromBody] MasterCycleYearMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NCMACY_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.editdetails(data);
        }

        [Route("getOrder")]
        public MasterCycleYearMappingDTO getOrder([FromBody] MasterCycleYearMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NCMACY_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getOrder(data);
        }

        // Master Cycle Year Mapping 
        [Route("onchangecycle")]
        public MasterCycleYearMappingDTO onchangecycle([FromBody] MasterCycleYearMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NCMACY_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangecycle(data);
        }

        [Route("savedetails1")]
        public MasterCycleYearMappingDTO savedetails1([FromBody] MasterCycleYearMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NCMACY_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedetails1(data);
        }

        [Route("viewdetails")]
        public MasterCycleYearMappingDTO viewdetails([FromBody] MasterCycleYearMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NCMACY_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.viewdetails(data);
        }
        [Route("deactivesem")]
        public MasterCycleYearMappingDTO deactivesem([FromBody] MasterCycleYearMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NCMACY_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactivesem(data);
        }

        [Route("delete")]
        public MasterCycleYearMappingDTO delete([FromBody] MasterCycleYearMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NCMACY_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.delete(data);
        }
    }
}
