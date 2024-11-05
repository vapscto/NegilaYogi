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
    public class MasterAllowanceFacadeController : Controller
    {
        public MasterAllowanceInterface _ads;

        public MasterAllowanceFacadeController(MasterAllowanceInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public MasterAllowanceDTO getinitialdata([FromBody]MasterAllowanceDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public MasterAllowanceDTO Post([FromBody]MasterAllowanceDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public MasterAllowanceDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public MasterAllowanceDTO deactivateRecordById([FromBody]MasterAllowanceDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
