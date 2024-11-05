using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeSalaryIncreementProcessFacadeController : Controller
    {

        public EmployeeSalaryIncreementProcessInterface _ads;

        public EmployeeSalaryIncreementProcessFacadeController(EmployeeSalaryIncreementProcessInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public EmployeeSalaryIncreementProcessDTO getinitialdata([FromBody]EmployeeSalaryIncreementProcessDTO dto)
        {
            return _ads.getBasicData(dto);
        }


        [Route("getReport")]
        public EmployeeSalaryIncreementProcessDTO getReport([FromBody]EmployeeSalaryIncreementProcessDTO dto)
        {
            return _ads.getReport(dto);
        }
        [Route("Empdetails")]
        public EmployeeSalaryIncreementProcessDTO Empdetails([FromBody]EmployeeSalaryIncreementProcessDTO dto)
        {
            return _ads.Empdetails(dto);
        }

        // POST api/values
        [HttpPost]
        public EmployeeSalaryIncreementProcessDTO Post([FromBody]EmployeeSalaryIncreementProcessDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public EmployeeSalaryIncreementProcessDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public EmployeeSalaryIncreementProcessDTO deactivateRecordById([FromBody]EmployeeSalaryIncreementProcessDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
