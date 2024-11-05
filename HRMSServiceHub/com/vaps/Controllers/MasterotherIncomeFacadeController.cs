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
    public class MasterotherIncomeFacadeController : Controller
    {
        public MasterotherIncomeInterface _ads;

        public MasterotherIncomeFacadeController(MasterotherIncomeInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_master_otherIncomeDTO getinitialdata([FromBody]HR_master_otherIncomeDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_master_otherIncomeDTO Post([FromBody]HR_master_otherIncomeDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_master_otherIncomeDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_master_otherIncomeDTO deactivateRecordById([FromBody]HR_master_otherIncomeDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
