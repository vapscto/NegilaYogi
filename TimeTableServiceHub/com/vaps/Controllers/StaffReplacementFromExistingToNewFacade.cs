using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTableServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StaffReplacementFromExistingToNewFacade : Controller
    {
        public StaffReplacementFromExistingToNewInterface _ttbreaktime;
        public StaffReplacementFromExistingToNewFacade(StaffReplacementFromExistingToNewInterface maspag)
        {
            _ttbreaktime = maspag;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       
        [Route("getdetails/{id:int}")]
        public StaffReplacementFromExistingToNewDTO getorgdet(int id)
        {
            return _ttbreaktime.getdetails(id);
        }
       
        [Route("savedetail")]
        public StaffReplacementFromExistingToNewDTO Post([FromBody] StaffReplacementFromExistingToNewDTO org)
        {
            return _ttbreaktime.savedetail(org);
        }
       
    }
}
