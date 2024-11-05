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
    public class Hostel_Request_ReportFacade : Controller
    {

        public Hostel_Request_ReportInterface _Interface;
        public Hostel_Request_ReportFacade(Hostel_Request_ReportInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("getdata")]
        public Hostel_Request_ReportDTO getdata([FromBody] Hostel_Request_ReportDTO data)
        {
            return _Interface.getdata(data);
        }
        [Route("getreport")]
        public Task<Hostel_Request_ReportDTO> getreport([FromBody] Hostel_Request_ReportDTO data)
        {
            return _Interface.getreport(data);
        }
        [Route("getconfirmreport")]
        public Task<Hostel_Request_ReportDTO> getconfirmreport([FromBody] Hostel_Request_ReportDTO data)
        {
            return _Interface.getconfirmreport(data);
        }
    }
}
