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
    public class MasterMaritalStatusFacadeController : Controller
    {
        public MasterMaritalStatusInterface _ads;

        public MasterMaritalStatusFacadeController(MasterMaritalStatusInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public IVRM_Master_Marital_StatusDTO getinitialdata([FromBody]IVRM_Master_Marital_StatusDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public IVRM_Master_Marital_StatusDTO Post([FromBody]IVRM_Master_Marital_StatusDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public IVRM_Master_Marital_StatusDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public IVRM_Master_Marital_StatusDTO deactivateRecordById([FromBody]IVRM_Master_Marital_StatusDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
