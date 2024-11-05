using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;


namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ECRFacadeController : Controller
    {
        // GET: api/values
        public ECRInterface _ads;

        public ECRFacadeController(ECRInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public ECRDTO getinitialdata([FromBody]ECRDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("SaveData")]
        public ECRDTO SaveData([FromBody]ECRDTO dto)
        {
            return _ads.SaveData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public ECRDTO getEmployeedetailsBySelection([FromBody]ECRDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }

        [Route("GetEmpDetails")]
        public ECRDTO GetEmpDetails([FromBody]ECRDTO data)
        {
            return _ads.GetEmpDetails(data);
        }

        [Route("get_depts")]
        public ECRDTO get_depts([FromBody]ECRDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public ECRDTO get_desig([FromBody]ECRDTO dto)
        {
            return _ads.get_desig(dto);
        }
    }
}