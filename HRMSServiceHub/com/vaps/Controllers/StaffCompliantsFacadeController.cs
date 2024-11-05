using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StaffCompliantsFacadeController : Controller
    {
        public StaffCompliantsInterface _interface;

        public StaffCompliantsFacadeController(StaffCompliantsInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("loaddata")]
        public StaffCompliantsDTO loaddata([FromBody] StaffCompliantsDTO data)
        {
            return _interface.loaddata(data);
        }

        [Route("OnChangeEmployee")]
        public StaffCompliantsDTO OnChangeEmployee([FromBody] StaffCompliantsDTO data)
        { 
            return _interface.OnChangeEmployee(data);
        }

        [Route("SaveDetails")]
        public StaffCompliantsDTO SaveDetails([FromBody] StaffCompliantsDTO data)
        { 
            return _interface.SaveDetails(data);
        }

        [Route("EditDetails")]
        public StaffCompliantsDTO EditDetails([FromBody] StaffCompliantsDTO data)
        {
            return _interface.EditDetails(data);
        }

        [Route("ActiveDeativeEmployeeCompliantsDetails")]
        public StaffCompliantsDTO ActiveDeativeEmployeeCompliantsDetails([FromBody] StaffCompliantsDTO data)
        {
            return _interface.ActiveDeativeEmployeeCompliantsDetails(data);
        }

        [Route("GetReport")]
        public StaffCompliantsDTO GetReport([FromBody] StaffCompliantsDTO data)
        {
            return _interface.GetReport(data);
        }

        [Route("GetViewStaffLoaddata")]
        public StaffCompliantsDTO GetViewStaffLoaddata([FromBody] StaffCompliantsDTO data)
        {
            return _interface.GetViewStaffLoaddata(data);
        }
    }
}
