using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using VidyaBharathiServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VidyaBharathiServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class IVRM_User_Login_DistrictFacade : Controller
    {
        //IVRM_User_Login_DistrictFacade
        public IVRM_User_Login_DistrictInterface _cms;

        public IVRM_User_Login_DistrictFacade(IVRM_User_Login_DistrictInterface cmsdept)
        {
            _cms = cmsdept;
        }
    
        [Route("loaddata")]
        public IVRM_User_Login_DistrictDTO loaddata([FromBody]IVRM_User_Login_DistrictDTO data)
        {
            
            return _cms.loaddata(data);
           
        }
        [HttpPost]
        [Route("savedata")]
        public IVRM_User_Login_DistrictDTO savedata([FromBody]IVRM_User_Login_DistrictDTO data)
        {
            return _cms.savedata(data);
        }

        [Route("deactive")]
        public IVRM_User_Login_DistrictDTO deactive([FromBody]IVRM_User_Login_DistrictDTO data)
        {
            return _cms.deactive(data);
        }

    }
}
