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
    public class MasterGenderFacadeController : Controller
    {
        public MasterGenderInterface _ads;

        public MasterGenderFacadeController(MasterGenderInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public IVRM_Master_GenderDTO getinitialdata([FromBody]IVRM_Master_GenderDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public IVRM_Master_GenderDTO Post([FromBody]IVRM_Master_GenderDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public IVRM_Master_GenderDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public IVRM_Master_GenderDTO deactivateRecordById([FromBody]IVRM_Master_GenderDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
