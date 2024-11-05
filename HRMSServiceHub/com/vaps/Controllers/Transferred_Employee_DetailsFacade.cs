using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Transferred_Employee_DetailsFacade : Controller
    {
        public Transferred_Employee_DetailsInterface _ads;
        public Transferred_Employee_DetailsFacade(Transferred_Employee_DetailsInterface adstu)
        {
            _ads = adstu;
        }

        [Route("getvalue")]
        public EmployeeReportsDTO getvalue([FromBody]EmployeeReportsDTO data)
        {
            return _ads.getvalue(data);
        }
        
    }
}
