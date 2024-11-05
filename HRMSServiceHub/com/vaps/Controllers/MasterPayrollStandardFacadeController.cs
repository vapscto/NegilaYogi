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
    public class MasterPayrollStandardFacadeController : Controller
    {
        // GET: api/values
        public MasterPayrollStandardInterface _ads;

        public MasterPayrollStandardFacadeController(MasterPayrollStandardInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_ConfigurationDTO getinitialdata([FromBody]HR_ConfigurationDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_ConfigurationDTO Post([FromBody]HR_ConfigurationDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]
        public HR_ConfigurationDTO getRecorddet(int id)
        {
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_ConfigurationDTO deactivateRecordById([FromBody]HR_ConfigurationDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
