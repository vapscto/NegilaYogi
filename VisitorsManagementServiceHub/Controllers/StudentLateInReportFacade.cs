using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using VisitorsManagementServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VisitorsManagementServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class StudentLateInReportFacade : Controller
    {

        StudentLateInReportInterface _objInter;

        public StudentLateInReportFacade(StudentLateInReportInterface parameter)
        {
            _objInter = parameter;
        }

        [Route("loaddata")]
        public LateInStudent_DTO loaddata([FromBody] LateInStudent_DTO data)
        {
            return _objInter.loaddata(data);
        }

        [Route("getReport")]
        public Task<LateInStudent_DTO> getReport([FromBody] LateInStudent_DTO data)
        {
            return _objInter.getReport(data);
        }



    }
}
