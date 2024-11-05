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
    public class ClgAttendanceSMSDetailsReportFacade : Controller
    {
        public ClgAttendanceSMSDetailsReportInterface inter;
        public ClgAttendanceSMSDetailsReportFacade(ClgAttendanceSMSDetailsReportInterface a)
        {
            inter = a;
        }
        [HttpPost]
        [Route("loaddata")]
        public ClgAttendanceSMSDetailsReport_DTO loaddata([FromBody] ClgAttendanceSMSDetailsReport_DTO data)
        {
            return inter.loaddata(data);
        } [HttpPost]
        [Route("getcourse")]
        public ClgAttendanceSMSDetailsReport_DTO getcourse([FromBody] ClgAttendanceSMSDetailsReport_DTO data)
        {
            return inter.getcourse(data);
        }[Route("getbranch")]
        public ClgAttendanceSMSDetailsReport_DTO getbranch([FromBody] ClgAttendanceSMSDetailsReport_DTO data)
        {
            return inter.getbranch(data);
        }[Route("getsemester")]
        public ClgAttendanceSMSDetailsReport_DTO getsemester([FromBody] ClgAttendanceSMSDetailsReport_DTO data)
        {
            return inter.getsemester(data);
        }[Route("showdetails")]
        public Task<ClgAttendanceSMSDetailsReport_DTO> showdetails([FromBody] ClgAttendanceSMSDetailsReport_DTO data)
        {
            return inter.showdetails(data);
        }
    }
}
