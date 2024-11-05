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
    public class massUpdationFacadeController : Controller
    {
        // GET: api/values
        public massUpdationInterface _ads;

        public massUpdationFacadeController(massUpdationInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public massUpdationDTO getinitialdata([FromBody]massUpdationDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        //FilterEmployeeData
        [Route("FilterEmployeeData")]
        public massUpdationDTO FilterEmployeeData([FromBody]massUpdationDTO dto)
        {
            return _ads.FilterEmployeeData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public massUpdationDTO getEmployeedetailsBySelection([FromBody]massUpdationDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }


        [Route("get_depts")]
        public massUpdationDTO get_depts([FromBody]massUpdationDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public massUpdationDTO get_desig([FromBody]massUpdationDTO dto)
        {
            return _ads.get_desig(dto);
        }
    }

}
