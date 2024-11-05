using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class InstituteWiseMandatorysettingsFacadeController : Controller
    {
        public InstituteWiseMandatorysettingsInterface _ads;

        public InstituteWiseMandatorysettingsFacadeController(InstituteWiseMandatorysettingsInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public IVRM_Mandatory_Setting_IWDTO getinitialdata([FromBody]IVRM_Mandatory_Setting_IWDTO dto)
        {
            return _ads.getBasicData(dto);
        }
        [Route("getPagedetailsBySelection")]
        public IVRM_Mandatory_Setting_IWDTO getPagedetailsBySelection([FromBody]IVRM_Mandatory_Setting_IWDTO dto)
        {
            return _ads.getPagedetailsBySelection(dto);
        }
        
        // POST api/values
        [HttpPost]
        public IVRM_Mandatory_Setting_IWDTO Post([FromBody]IVRM_Mandatory_Setting_IWDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }


        [Route("getRecordById")]

        public IVRM_Mandatory_Setting_IWDTO getcatgrydet([FromBody]IVRM_Mandatory_Setting_IWDTO dto)
        {
            // id = 12;
            return _ads.editData(dto);

        }
        [Route("deactivateRecordById")]
        public IVRM_Mandatory_Setting_IWDTO deactivateRecordById([FromBody]IVRM_Mandatory_Setting_IWDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
