using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.HRMS.Interface;
using PreadmissionDTOs.HRMS;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.HRMS.Facade
{
    [Route("api/[controller]")]
    public class HrmsConsolidatedReportFacadeController : Controller
    {

        public HrmsConsolidatedReportInterface _interface;

        public HrmsConsolidatedReportFacadeController(HrmsConsolidatedReportInterface inte)
        {
            _interface = inte;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails")]
        public HRMS_NAAC_DTO getdetails([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getdetails(data);
        }

        [Route("get_depts")]
        public HRMS_NAAC_DTO get_depts([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.get_depts(data);
        }

        [Route("get_desig")]
        public HRMS_NAAC_DTO get_desig([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.get_desig(data);
        }

        [Route("get_Employe_ob")]
        public HRMS_NAAC_DTO get_Employe_ob([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.get_Employe_ob(data);
        }

        [Route("getEmployeReportAsync")]
        public Task<HRMS_NAAC_DTO> getEmployeReportAsync([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getEmployeReportAsync(data);
        }

        [Route("get_EmployeALLDATA")]
        public HRMS_NAAC_DTO get_EmployeALLDATA([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.get_EmployeALLDATA(data);
        }
        
    }
}
