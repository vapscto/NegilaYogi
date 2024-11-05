using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using HRMSServicesHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeServiceRecordReportFacadeController : Controller
    {
        // GET: api/values
        public EmployeeServiceRecordReportInterface _ads;

        public EmployeeServiceRecordReportFacadeController(EmployeeServiceRecordReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public EmployeeServiceRecordReportDTO getinitialdata([FromBody]EmployeeServiceRecordReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }
        //FilterEmployeeData
        [Route("FilterEmployeeData")]
        public EmployeeServiceRecordReportDTO FilterEmployeeData([FromBody]EmployeeServiceRecordReportDTO dto)
        {
            return _ads.FilterEmployeeData(dto);
        }


        [Route("getEmployeedetailsBySelection")]
        public Task<EmployeeServiceRecordReportDTO> getEmployeedetailsBySelection([FromBody]EmployeeServiceRecordReportDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }
        [Route("get_depts")]
        public EmployeeServiceRecordReportDTO get_depts([FromBody]EmployeeServiceRecordReportDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public EmployeeServiceRecordReportDTO get_desig([FromBody]EmployeeServiceRecordReportDTO dto)
        {
            return _ads.get_desig(dto);
        }



    }
}
