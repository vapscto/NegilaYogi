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
    public class Medical_Criteria3ReportsFacade : Controller
    {
        Medical_Criteria3ReportsInterface _Interface;

        public Medical_Criteria3ReportsFacade(Medical_Criteria3ReportsInterface Interfaces)
        {
            _Interface = Interfaces;
        }


        [Route("getdata")]
        public Medical_Criteria3Reports_DTO getdata([FromBody] Medical_Criteria3Reports_DTO data)
        {
            return _Interface.getdata(data);
        }
        [Route("MC_311_Report")]
        public Task<Medical_Criteria3Reports_DTO> MC_311_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            return _Interface.MC_311_Report(data);
        }
        [Route("MC_312_Report")]
        public Task<Medical_Criteria3Reports_DTO> MC_312_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            return _Interface.MC_312_Report(data);
        }
        [Route("MC_313_Report")]
        public Task<Medical_Criteria3Reports_DTO> MC_313_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            return _Interface.MC_313_Report(data);
        }
        [Route("MC_322_Report")]
        public Task<Medical_Criteria3Reports_DTO> MC_322_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            return _Interface.MC_322_Report(data);
        }
        [Route("MC_331_report")]
        public Task<Medical_Criteria3Reports_DTO> MC_331_report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            return _Interface.MC_331_report(data);
        }
        [Route("MC_332_Report")]
        public Task<Medical_Criteria3Reports_DTO> MC_332_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            return _Interface.MC_332_Report(data);
        }
        [Route("MC_333_Report")]
        public Task<Medical_Criteria3Reports_DTO> MC_333_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            return _Interface.MC_333_Report(data);
        }
        [Route("MC_334_Report")]
        public Task<Medical_Criteria3Reports_DTO> MC_334_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            return _Interface.MC_334_Report(data);
        }
        [Route("MC_341_Report")]
        public Task<Medical_Criteria3Reports_DTO> MC_341_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            return _Interface.MC_341_Report(data);
        }
        [Route("MC_342_Report")]
        public Task<Medical_Criteria3Reports_DTO> MC_342_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            return _Interface.MC_342_Report(data);
        }
        [Route("MC_351_Report")]
        public Task<Medical_Criteria3Reports_DTO> MC_351_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            return _Interface.MC_351_Report(data);
        }
        [Route("MC_352_Report")]
        public Task<Medical_Criteria3Reports_DTO> MC_352_Report([FromBody]Medical_Criteria3Reports_DTO data)
        {
            return _Interface.MC_352_Report(data);
        }
        
    }
}
