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
    public class StudentActiveInactiveReportFacade : Controller
    {
        public StudentActiveInactiveReportInterface _intf;
        public StudentActiveInactiveReportFacade(StudentActiveInactiveReportInterface intf)
        {
            _intf = intf;
        }
        [Route("getdata")]
        public StudentActiveInactiveReportDTO getdata([FromBody] StudentActiveInactiveReportDTO data)
        {
            return _intf.getdata(data);
        }
        [Route("onacademicyearchange")]
        public StudentActiveInactiveReportDTO onacademicyearchange([FromBody] StudentActiveInactiveReportDTO data)
        {
            return _intf.onacademicyearchange(data);
        }
        [Route("oncoursechange")]
        public StudentActiveInactiveReportDTO oncoursechange([FromBody] StudentActiveInactiveReportDTO data)
        {
            return _intf.oncoursechange(data);
        }
        [Route("onbranchchange")]
        public StudentActiveInactiveReportDTO onbranchchange([FromBody] StudentActiveInactiveReportDTO data)
        {
            return _intf.onbranchchange(data);
        }
        [Route("onchangesemester")]
        public StudentActiveInactiveReportDTO onchangesemester([FromBody] StudentActiveInactiveReportDTO data)
        {
            return _intf.onchangesemester(data);
        }        
        [Route("getreport")]
        public StudentActiveInactiveReportDTO getreport([FromBody] StudentActiveInactiveReportDTO data)
        {
            return _intf.getreport(data);
        }
    }
}
