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
    public class MandatorysettingsFacadeController : Controller
    {
        public MandatorysettingsInterface _ads;

        public MandatorysettingsFacadeController(MandatorysettingsInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public IVRM_Mandatory_SettingDTO getinitialdata([FromBody]IVRM_Mandatory_SettingDTO dto)
        {
            return _ads.getBasicData(dto);
        }
        [Route("getPagedetailsBySelection")]
        public IVRM_Mandatory_SettingDTO getPagedetailsBySelection([FromBody]IVRM_Mandatory_SettingDTO dto)
        {
            return _ads.getPagedetailsBySelection(dto);
        }

        // POST api/values
        [HttpPost]
        public IVRM_Mandatory_SettingDTO Post([FromBody]IVRM_Mandatory_SettingDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public IVRM_Mandatory_SettingDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public IVRM_Mandatory_SettingDTO deactivateRecordById([FromBody]IVRM_Mandatory_SettingDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
