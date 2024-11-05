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
    public class NAACCriteria3ReportFacade : Controller
    {

        NAACCriteria3ReportInterface _Interface;

        public NAACCriteria3ReportFacade(NAACCriteria3ReportInterface Interfaces)
        {
            _Interface = Interfaces;
        }


        [Route("getdata")]
        public NAACCriteria3ReportDTO getdata([FromBody] NAACCriteria3ReportDTO data)
        {
            return _Interface.getdata(data);
        }

        [Route("get_report")]
        public Task<NAACCriteria3ReportDTO> get_report ([FromBody] NAACCriteria3ReportDTO data)
        {
            return _Interface.get_report(data);
        }
        [Route("get_report364")]
        public Task<NAACCriteria3ReportDTO> get_report364([FromBody] NAACCriteria3ReportDTO data)
        {
            return _Interface.get_report364(data);
        }
        
        [Route("reportIPR")]
        public Task<NAACCriteria3ReportDTO> reportIPR([FromBody] NAACCriteria3ReportDTO data)
        {
            return _Interface.reportIPR(data);
        }

    }
}
