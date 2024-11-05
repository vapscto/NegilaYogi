using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.PettyCash.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.PettyCash.Facade
{
    [Route("api/[controller]")]
    public class PC_ReportFacadeController : Controller
    {
        public PC_ReportInterface _interface;
        public PC_ReportFacadeController(PC_ReportInterface _inter)
        {
            _interface = _inter;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("onloaddata")]
        public PC_ReportDTO onloaddata([FromBody] PC_ReportDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("getrequisitionreport")]
        public PC_ReportDTO getrequisitionreport([FromBody] PC_ReportDTO data)
        {
            return _interface.getrequisitionreport(data);
        }

        [Route("getindentreport")]
        public PC_ReportDTO getindentreport([FromBody] PC_ReportDTO data)
        {
            return _interface.getindentreport(data);
        }

        [Route("getindentapprovedreport")]
        public PC_ReportDTO getindentapprovedreport([FromBody] PC_ReportDTO data)
        {
            return _interface.getindentapprovedreport(data);
        }

        [Route("getexpenditurereport")]
        public PC_ReportDTO getexpenditurereport([FromBody] PC_ReportDTO data)
        {
            return _interface.getexpenditurereport(data);
        }
    }
}
