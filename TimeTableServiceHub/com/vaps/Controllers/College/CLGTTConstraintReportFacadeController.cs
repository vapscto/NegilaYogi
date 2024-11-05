using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using TimeTableServiceHub.com.vaps.Interfaces.College;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers.College
{
    [Route("api/[controller]")]
    public class CLGTTConstraintReportFacadeController : Controller
    {

        public CLGTTConstraintReportInterface inter;
     public CLGTTConstraintReportFacadeController(CLGTTConstraintReportInterface k)
        {
            inter = k;
        }

        [Route("getalldetails/{id:int}")]
        public CLGTTConstraintReportDTO getalldetails(int id)
        {
            return inter.getalldetails(id);
        }
        [Route("getpagedetails")]
        public CLGTTConstraintReportDTO getpagedetails([FromBody] CLGTTConstraintReportDTO data)
        {
            return inter.getpagedetails(data);
        }

    }
}
