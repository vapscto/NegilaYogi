using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeCancellationConfigurationFacadeController : Controller
    {
        public CollegeCancellationConfigurationInterface _intf;

        public CollegeCancellationConfigurationFacadeController(CollegeCancellationConfigurationInterface intf)
        {
            _intf = intf;
        }
        [Route("getdata")]
        public CollegeCancellationConfigurationDTO getdata([FromBody] CollegeCancellationConfigurationDTO data)
        {
            return _intf.getdata(data);
        }

        [Route("saveconfig")]
        public CollegeCancellationConfigurationDTO saveconfig([FromBody]CollegeCancellationConfigurationDTO data)
        {
            return _intf.saveconfig(data);
        }
        [Route("editconfig")]
        public CollegeCancellationConfigurationDTO editconfig([FromBody]CollegeCancellationConfigurationDTO data)
        {
            return _intf.editconfig(data);
        }
        [Route("activedeactive")]
        public CollegeCancellationConfigurationDTO activedeactive([FromBody]CollegeCancellationConfigurationDTO data)
        {          
            return _intf.activedeactive(data);
        }       
       
    }
}
