using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class salaryUpdationFacadeController : Controller
    {
        // GET: api/values
        public salaryUpdationInterface _ads;

        public salaryUpdationFacadeController(salaryUpdationInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public salaryUpdationDTO getinitialdata([FromBody]salaryUpdationDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        //FilterEmployeeData
        [Route("FilterEmployeeData")]
        public salaryUpdationDTO FilterEmployeeData([FromBody]salaryUpdationDTO dto)
        {
            return _ads.FilterEmployeeData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public salaryUpdationDTO getEmployeedetailsBySelection([FromBody]salaryUpdationDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }

        [Route("get_depts")]
        public salaryUpdationDTO get_depts([FromBody]salaryUpdationDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public salaryUpdationDTO get_desig([FromBody]salaryUpdationDTO dto)
        {
            return _ads.get_desig(dto);
        }

    }
}
