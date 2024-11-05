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
    public class CurricularAspectsFacade : Controller
    {

        CurricularAspectsInterface _Interface;

        public CurricularAspectsFacade(CurricularAspectsInterface Interfaces)
        {
            _Interface = Interfaces;
        }


        [Route("getdata")]
        public CurricularAspects_DTO getdata([FromBody] CurricularAspects_DTO data)
        {
            return _Interface.getdata(data);
        }

        [Route("get_report")]
        public Task<CurricularAspects_DTO> get_report ([FromBody] CurricularAspects_DTO data)
        {
            return _Interface.get_report(data);
        }

        [Route("get_nCourse_report")]
        public Task<CurricularAspects_DTO> get_nCourse_report([FromBody] CurricularAspects_DTO data)
        {
            return _Interface.get_nCourse_report(data);
        }

        [Route("get_report_113")]
        public Task<CurricularAspects_DTO> get_report_113([FromBody] CurricularAspects_DTO data)
        {
            return _Interface.get_report_113(data);
        }

        [Route("get_report_123")]
        public Task<CurricularAspects_DTO> get_report_123([FromBody] CurricularAspects_DTO data)
        {
            return _Interface.get_report_123(data);
        }

        [Route("get_report_133")]
        public Task<CurricularAspects_DTO> get_report_133([FromBody] CurricularAspects_DTO data)
        {
            return _Interface.get_report_133(data);
        }

        [Route("get_report_132")]
        public Task<CurricularAspects_DTO> get_report_132([FromBody] CurricularAspects_DTO data)
        {
            return _Interface.get_report_132(data);
        }

        [Route("get_122CBCSsystemReport")]
        public Task<CurricularAspects_DTO> get_122CBCSsystemReport([FromBody] CurricularAspects_DTO data)
        {
            return _Interface.get_122CBCSsystemReport(data);
        }

       
    }    
}
