using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;


namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class masterpointsFacadeController : Controller
    {
        public masterpointInterface _ads;

        public masterpointsFacadeController(masterpointInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Employee_AssesmentpointsDTO getinitialdata([FromBody]HR_Employee_AssesmentpointsDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Employee_AssesmentpointsDTO Post([FromBody]HR_Employee_AssesmentpointsDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_Employee_AssesmentpointsDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_Employee_AssesmentpointsDTO deactivateRecordById([FromBody]HR_Employee_AssesmentpointsDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}