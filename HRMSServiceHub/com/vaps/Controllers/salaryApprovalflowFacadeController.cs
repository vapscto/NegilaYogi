using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class salaryApprovalflowFacadeController : Controller
    {
        // GET: api/values
        public salaryApprovalflowInterface _ads;

        public salaryApprovalflowFacadeController(salaryApprovalflowInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public salaryApprovalFlowDTO getinitialdata([FromBody]salaryApprovalFlowDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        //FilterEmployeeData
        [Route("FilterEmployeeData")]
        public salaryApprovalFlowDTO FilterEmployeeData([FromBody]salaryApprovalFlowDTO dto)
        {
            return _ads.FilterEmployeeData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public salaryApprovalFlowDTO getEmployeedetailsBySelection([FromBody]salaryApprovalFlowDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }

        [Route("get_depts")]
        public salaryApprovalFlowDTO get_depts([FromBody]salaryApprovalFlowDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public salaryApprovalFlowDTO get_desig([FromBody]salaryApprovalFlowDTO dto)
        {
            return _ads.get_desig(dto);
        }

    }
}
