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
    public class BloodGroupWiseStudentDetailsReportFacade : Controller
    {
        public BloodGroupWiseStudentDetailsReportInterface _inter;
        public BloodGroupWiseStudentDetailsReportFacade(BloodGroupWiseStudentDetailsReportInterface p)
        {
            _inter = p;
        }
        
        [Route("loaddata")]
        public BloodGroupWiseStudentDetailsReportDTO loaddata([FromBody] BloodGroupWiseStudentDetailsReportDTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("getcourse")]
        public BloodGroupWiseStudentDetailsReportDTO getcourse([FromBody] BloodGroupWiseStudentDetailsReportDTO data)
        {
            return _inter.getcourse(data);
        }
        [Route("getbranch")]
        public BloodGroupWiseStudentDetailsReportDTO getbranch([FromBody] BloodGroupWiseStudentDetailsReportDTO data)
        {
            return _inter.getbranch(data);
        }
        [Route("getsemester")]
        public BloodGroupWiseStudentDetailsReportDTO getsemester([FromBody] BloodGroupWiseStudentDetailsReportDTO data)
        {
            return _inter.getsemester(data);
        }
        [Route("Report")]
        public BloodGroupWiseStudentDetailsReportDTO Report([FromBody] BloodGroupWiseStudentDetailsReportDTO data)
        {
            return _inter.Report(data);
        }


    }
}
