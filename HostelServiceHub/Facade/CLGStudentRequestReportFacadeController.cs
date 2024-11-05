using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostelServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HostelServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CLGStudentRequestReportFacadeController : Controller
    {

        public CLGStudentRequestReportInterface _Interface;
        public CLGStudentRequestReportFacadeController(CLGStudentRequestReportInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("getdata")]
        public CLGStudentReportDTO getdata([FromBody] CLGStudentReportDTO data)
        {
            return _Interface.getdata(data);
        }
        [Route("getreport")]
        public Task<CLGStudentReportDTO> getreport([FromBody] CLGStudentReportDTO data)
        {
            return _Interface.getreport(data);
        }
        [Route("getconfirmreport")]
        public Task<CLGStudentReportDTO> getconfirmreport([FromBody] CLGStudentReportDTO data)
        {
            return _Interface.getconfirmreport(data);
        }
    }
}
