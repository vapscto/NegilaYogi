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
    public class NAACCriteriaFiveReportFacade : Controller
    {

        NAACCriteriaFiveReportInterface _Interface;

        public NAACCriteriaFiveReportFacade(NAACCriteriaFiveReportInterface Interfaces)
        {
            _Interface = Interfaces;
        }


        [Route("getdata")]
        public NAACCriteriaFiveReportDTO getdata([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.getdata(data);
        }

        [Route("get_report")]
        public Task<NAACCriteriaFiveReportDTO> get_report ([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report(data);
        }
        [Route("HSU511")]
        public Task<NAACCriteriaFiveReportDTO> HSU511([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.HSU511(data);
        }
        [Route("get_report513")]
        public Task<NAACCriteriaFiveReportDTO> get_report513([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report513(data);
        }
        [Route("get_report514")]
        public Task<NAACCriteriaFiveReportDTO> get_report514([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report514(data);
        }
        [Route("get_report513med")]
        public Task<NAACCriteriaFiveReportDTO> get_report513med([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report513med(data);
        }
        [Route("get_report516")]
        public Task<NAACCriteriaFiveReportDTO> get_report516([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report516(data);
        }
        [Route("get_report515med")]
        public Task<NAACCriteriaFiveReportDTO> get_report515med([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report515med(data);
        }
         [Route("get_report521")]
        public Task<NAACCriteriaFiveReportDTO> get_report521([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report521(data);
        }
        [Route("get_report522")]
        public Task<NAACCriteriaFiveReportDTO> get_report522([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report522(data);
        }
        [Route("get_report531")]
        public Task<NAACCriteriaFiveReportDTO> get_report531([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report531(data);
        }
        [Route("get_report533")]
        public Task<NAACCriteriaFiveReportDTO> get_report533([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report533(data);
        }

     
        [Route("get_report542")]
        public Task<NAACCriteriaFiveReportDTO> get_report542([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report542(data);
        }
        [Route("get_report542HSU")]
        public Task<NAACCriteriaFiveReportDTO> get_report542HSU([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report542HSU(data);
        }
         [Route("get_report543")]
        public Task<NAACCriteriaFiveReportDTO> get_report543([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report543(data);
        }
           [Route("get_report523")]
        public Task<NAACCriteriaFiveReportDTO> get_report523([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report523(data);
        }
        [Route("get_report515")]
        public Task<NAACCriteriaFiveReportDTO> get_report515([FromBody] NAACCriteriaFiveReportDTO data)
        {
            return _Interface.get_report515(data);
        }

     
        
    }
}
