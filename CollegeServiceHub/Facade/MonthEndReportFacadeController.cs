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
    public class MonthEndReportFacadeController : Controller
    {
        public MonthEndReportInterface inter;

        public MonthEndReportFacadeController(MonthEndReportInterface maspag)
        {
            inter = maspag;
        }

        [Route("getdata")]
        public MonthEndReportDTO getdata([FromBody] MonthEndReportDTO data)
        {
            return inter.getdata(data);
        }

        [Route("getreport")]
        public Task<MonthEndReportDTO> getreport([FromBody] MonthEndReportDTO data)
        {
            return inter.getreport(data);
        }

        [Route("getyear/{id:int}")]
        public MonthEndReportDTO getyear(int id)
        {
            
            MonthEndReportDTO stud = new MonthEndReportDTO();
            stud.MI_ID = id;
            return inter.getyear(stud);
        }

        [Route("Studdetails")]
        public Task<MonthEndReportDTO> Studdetails([FromBody] MonthEndReportDTO data)
        {
            return inter.Studdetails(data);
        }

        [Route("getbranch")]
        public MonthEndReportDTO getbranch([FromBody] MonthEndReportDTO data)
        {
            return inter.getbranch(data);
        }

        [Route("getsemester")]
        public MonthEndReportDTO getsemester([FromBody] MonthEndReportDTO data)
        {
            return inter.getsemester(data);
        }
    }
}
