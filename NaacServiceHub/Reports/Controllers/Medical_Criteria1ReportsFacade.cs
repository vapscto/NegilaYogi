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
    public class Medical_Criteria1ReportsFacade : Controller
    {
        Medical_Criteria1ReportsInterface _Interface;

        public Medical_Criteria1ReportsFacade(Medical_Criteria1ReportsInterface Interfaces)
        {
            _Interface = Interfaces;
        }


        [Route("getdata")]
        public Medical_Criteria1Reports_DTO getdata([FromBody] Medical_Criteria1Reports_DTO data)
        {
            return _Interface.getdata(data);
        }

        [Route("get_report_MC_112")]
        public Task<Medical_Criteria1Reports_DTO> get_report_MC_112([FromBody]Medical_Criteria1Reports_DTO data)
        {
            return _Interface.get_report_MC_112Async(data);
        }

        [Route("report_MC_141")]
        public Medical_Criteria1Reports_DTO report_MC_141([FromBody]Medical_Criteria1Reports_DTO data)
        {
            return _Interface.report_MC_141(data);
        }

        [Route("report_MC_142")]
        public Medical_Criteria1Reports_DTO report_MC_142([FromBody]Medical_Criteria1Reports_DTO data)
        {
            return _Interface.report_MC_142(data);
        }

        [Route("M_IDC121_Report")]
        public Task<Medical_Criteria1Reports_DTO> M_IDC121_Report([FromBody]Medical_Criteria1Reports_DTO data)
        {
            return _Interface.M_IDC121_Report(data);
        }

        [Route("M_SRC122_Report")]
        public Task<Medical_Criteria1Reports_DTO> M_SRC122_Report([FromBody]Medical_Criteria1Reports_DTO data)
        {
            return _Interface.M_SRC122_Report(data);
        }

        [Route("MC_VAC_report_132")]
        public Task<Medical_Criteria1Reports_DTO> MC_VAC_report_132([FromBody]Medical_Criteria1Reports_DTO data)
        {
            return _Interface.MC_VAC_report_132(data);
        }

        [Route("StudentsEnrolledInVAC133_report")]
        public Task<Medical_Criteria1Reports_DTO> StudentsEnrolledInVAC133_report([FromBody]Medical_Criteria1Reports_DTO data)
        {
            return _Interface.StudentsEnrolledInVAC133_report(data);
        }

        [Route("MC_StudentUTFV_134_Report")]
        public Task<Medical_Criteria1Reports_DTO> MC_StudentUTFV_134_Report([FromBody]Medical_Criteria1Reports_DTO data)
        {
            return _Interface.MC_StudentUTFV_134_Report(data);
        }
        
    }
}
