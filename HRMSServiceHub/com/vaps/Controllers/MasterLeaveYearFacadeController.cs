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
    public class MasterLeaveYearFacadeController : Controller
    {

        public MasterLeaveYearInterface _ads;

        public MasterLeaveYearFacadeController(MasterLeaveYearInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Master_LeaveYearDTO getinitialdata([FromBody]HR_Master_LeaveYearDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Master_LeaveYearDTO Post([FromBody]HR_Master_LeaveYearDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_Master_LeaveYearDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_Master_LeaveYearDTO deactivateRecordById([FromBody]HR_Master_LeaveYearDTO dto)
        {
            return _ads.deactivate(dto);
        }
        [Route("validateordernumber")]
        public HR_Master_LeaveYearDTO validateordernumber([FromBody]HR_Master_LeaveYearDTO dto)
        {
            return _ads.validateordernumber(dto);
        }
    }
}
