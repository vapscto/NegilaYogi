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
    public class Medical_Criteria2ReportsFacade : Controller
    {
        Medical_Criteria2ReportsInterface _Interface;

        public Medical_Criteria2ReportsFacade(Medical_Criteria2ReportsInterface Interfaces)
        {
            _Interface = Interfaces;
        }


        [Route("getdata")]
        public Medical_Criteria2Reports_DTO getdata([FromBody] Medical_Criteria2Reports_DTO data)
        {
            return _Interface.getdata(data);
        }
        [Route("MC_221_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_221_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_221_Report(data);
        }
        [Route("MC_254_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_254_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_254_Report(data);
        }
        [Route("MC_232_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_232_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_232_Report(data);
        }
        [Route("MC_212_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_212_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_212_Report(data);
        }
        [Route("MC_213_report")]
        public Task<Medical_Criteria2Reports_DTO> MC_213_report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_213_report(data);
        }
        [Route("MC_222_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_222_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_222_Report(data);
        }
        [Route("MC_234_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_234_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_234_Report(data);
        }
        [Route("MC_241_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_241_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_241_Report(data);
        }
        [Route("MC_242_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_242_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_242_Report(data);
        }
        [Route("MC_243_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_243_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_243_Report(data);
        }
        [Route("MC_244_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_244_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_244_Report(data);
        }
        [Route("MC_245_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_245_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_245_Report(data);
        }
        [Route("MC_262_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_262_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_262_Report(data);
        }
        [Route("MC_271_Report")]
        public Task<Medical_Criteria2Reports_DTO> MC_271_Report([FromBody]Medical_Criteria2Reports_DTO data)
        {
            return _Interface.MC_271_Report(data);
        }
    }
}
