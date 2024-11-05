using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostelServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HostelServiceHub.Facade
{
    [Route("api/[controller]")]
    public class Hostel_Student_InOutReportFacadeController : Controller
    {
        // GET: /<controller>/
        public Hostel_Student_InOutReportInterface _interface;

        public Hostel_Student_InOutReportFacadeController(Hostel_Student_InOutReportInterface _inter)
        {
            _interface = _inter;
        }
        [HttpPost]
        [Route("loaddata")]
        public Hostel_Student_InOutDTO loaddata([FromBody] Hostel_Student_InOutDTO data)
        {
            return _interface.loaddata(data);
        }
        [Route("empname")]
        public Hostel_Student_InOutDTO empname([FromBody] Hostel_Student_InOutDTO data)
        {
            return _interface.empname(data);
        }

        [Route("savedetail")]
        public Hostel_Student_InOutDTO savedetail([FromBody] Hostel_Student_InOutDTO data)
        {
            return _interface.savedetail(data);
        }
        [Route("deletedetails")]
        public Hostel_Student_InOutDTO deletedetails([FromBody] Hostel_Student_InOutDTO data)
        {
            return _interface.deleterec(data);
        }


        //report
        [HttpPost]
        [Route("getloaddata")]
        public Hostel_Student_InOutDTO getloaddata([FromBody] Hostel_Student_InOutDTO data)
        {
            return _interface.getloaddata(data);
        }
        [Route("report")]
        public Hostel_Student_InOutDTO report([FromBody] Hostel_Student_InOutDTO data)
        {
            return _interface.report(data);
        }
    }
}
