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
    public class HSU_CR2_ReportFacade : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public HSU_CR2_ReportInterface _Interface;

        public HSU_CR2_ReportFacade(HSU_CR2_ReportInterface para)
        {
            _Interface = para;
        }

        [Route("getdata")]
        public HSU_CR2_Report_DTO getdata([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.getdata(data);
        }

        [Route("HSU_211_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_211_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_211_Report(data);
        }
        [Route("HSU_212_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_212_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_212_Report(data);
        }
        [Route("HSU_213_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_213_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_213_Report(data);
        }
        [Route("HSU_221_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_221_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_221_Report(data);
        }
        [Route("HSU_222_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_222_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_222_Report(data);
        }
        [Route("HSU_232_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_232_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_232_Report(data);
        }
        [Route("HSU_234_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_234_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_234_Report(data);
        }
        [Route("HSU_241_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_241_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_241_Report(data);
        }
        [Route("HSU_242_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_242_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_242_Report(data);
        }
        [Route("HSU_243_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_243_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_243_Report(data);
        }
        [Route("HSU_244_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_244_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_244_Report(data);
        }
        [Route("HSU_245_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_245_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_245_Report(data);
        }
        [Route("HSU_251_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_251_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_251_Report(data);
        }
        [Route("HSU_252_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_252_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_252_Report(data);
        }
        [Route("HSU_253_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_253_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_253_Report(data);
        }
        [Route("HSU_255_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_255_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_255_Report(data);
        }
        [Route("HSU_262_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_262_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_262_Report(data);
        }
        [Route("HSU_271_Report")]
        public Task<HSU_CR2_Report_DTO> HSU_271_Report([FromBody] HSU_CR2_Report_DTO data)
        {
            return _Interface.HSU_271_Report(data);
        }
        
    }
}
