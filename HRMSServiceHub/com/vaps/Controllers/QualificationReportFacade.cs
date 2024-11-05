using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class QualificationReportFacade : Controller
    {
        public QualificationReportInterface _ads;
        public QualificationReportFacade(QualificationReportInterface adstu)
        {
            _ads = adstu;
        }
        [Route("getalldetails")]
        public MasterEmployeeDTO getalldetails([FromBody]MasterEmployeeDTO data)
        {
            return _ads.getalldetails(data);
        }
        [Route("getQualificationReport")]
        public Task<MasterEmployeeDTO> getQualificationReport([FromBody]MasterEmployeeDTO data)
        {
            return _ads.getQualificationReport(data);
        }
    }
}
