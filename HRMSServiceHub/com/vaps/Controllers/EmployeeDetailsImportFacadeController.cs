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
    public class EmployeeDetailsImportFacadeController : Controller
    {
        public EmployeeDetailsImportInterface _ads;

        public EmployeeDetailsImportFacadeController(EmployeeDetailsImportInterface adstu)
        {
            _ads = adstu;
        }

        // POST api/values
        [HttpPost]
        [Route("save_excel_data")]
        public Task<MasterEmployeeImportDTO> saveexceldata([FromBody] MasterEmployeeImportDTO dto)
        {
            return _ads.save_excel_data(dto);
        }


      

    }
}
