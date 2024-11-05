using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeDataImportFacadeController : Controller
    {
        public EmployeeDataImportInterface _objInter;
        public EmployeeDataImportFacadeController(EmployeeDataImportInterface data)
        {
            _objInter = data;
        }

        [Route("Savedata")]
        public EmployeeDataImportDTO Savedata([FromBody] EmployeeDataImportDTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public EmployeeDataImportDTO getdetails(int id)
        {
            return _objInter.getdetails(id);
        }

        [Route("deactiveY")]
        public EmployeeDataImportDTO deactiveY([FromBody] EmployeeDataImportDTO data)
        {
            return _objInter.deactiveY(data);
        }
    }
}
