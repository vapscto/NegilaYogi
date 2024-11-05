using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Reports.Interface;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Reports.Controllers
{
    [Route("api/[controller]")]
    public class HSU_CR1_ReportFacade : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public HSU_CR1_ReportInterface _Interface;

        public HSU_CR1_ReportFacade(HSU_CR1_ReportInterface para)
        {
            _Interface = para;
        }

        [Route("getdata")]
        public HSU_CR1_Report_DTO getdata([FromBody] HSU_CR1_Report_DTO data)
        {
            return _Interface.getdata(data);
        }

        [Route("HSU_112_Report")]
        public Task<HSU_CR1_Report_DTO> HSU_112_Report([FromBody] HSU_CR1_Report_DTO data)
        {
            return _Interface.HSU_112_Report(data);
        }
        [Route("HSU_132_133_Report")]
        public Task<HSU_CR1_Report_DTO> HSU_132_133_Report([FromBody] HSU_CR1_Report_DTO data)
        {
            return _Interface.HSU_132_133_Report(data);
        }
        [Route("HSU_141_Report")]
        public Task<HSU_CR1_Report_DTO> HSU_141_Report([FromBody] HSU_CR1_Report_DTO data)
        {
            return _Interface.HSU_141_Report(data);
        }
        [Route("HSU_142_Report")]
        public Task<HSU_CR1_Report_DTO> HSU_142_Report([FromBody] HSU_CR1_Report_DTO data)
        {
            return _Interface.HSU_142_Report(data);
        }
        [Route("HSU_121_Report")]
        public Task<HSU_CR1_Report_DTO> HSU_121_Report([FromBody] HSU_CR1_Report_DTO data)
        {
            return _Interface.HSU_121_Report(data);
        }
        [Route("HSU_122_Report")]
        public Task<HSU_CR1_Report_DTO> HSU_122_Report([FromBody] HSU_CR1_Report_DTO data)
        {
            return _Interface.HSU_122_Report(data);
        }
        [Route("HSU_123_Report")]
        public Task<HSU_CR1_Report_DTO> HSU_123_Report([FromBody] HSU_CR1_Report_DTO data)
        {
            return _Interface.HSU_123_Report(data);
        }
    }
}
