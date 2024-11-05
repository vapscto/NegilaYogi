using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Employee;
using IVRMUX.Delegates.com.vapstech.VMS.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.VMS.Training;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace IVRMUX.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class CovidVaccinationController : Controller
    {
        CovidVaccinationDelegate _del = new CovidVaccinationDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onloaddata/{id:int}")]
        public CovidVaccineDTO onloaddata(int id)
        {
            CovidVaccineDTO data = new CovidVaccineDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.onloaddata(data);
        }
        [HttpPost]
        [Route("saverecord")]
        public CovidVaccineDTO saverecord([FromBody] CovidVaccineDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.saverecord(data);
        }

        [Route("deactiveY")]
        public CovidVaccineDTO deactiveY([FromBody] CovidVaccineDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.deactiveY(data);
        }
    }
}
