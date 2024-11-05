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
    public class VBSC_Events_CategoryFacade : Controller
    {
        //IVRM_User_Login_StateFacade
        public VBSC_Events_CategoryInterface _cms;

        public VBSC_Events_CategoryFacade(VBSC_Events_CategoryInterface cmsdept)
        {
            _cms = cmsdept;
        }
    
        [Route("loaddata")]
        public VBSC_Events_CategoryDTO loaddata([FromBody]VBSC_Events_CategoryDTO data)
        {
            
            return _cms.loaddata(data);
           
        }
        [HttpPost]
        [Route("savedata")]
        public VBSC_Events_CategoryDTO savedata([FromBody]VBSC_Events_CategoryDTO data)
        {
            return _cms.savedata(data);
        }

        [Route("deactive")]
        public VBSC_Events_CategoryDTO deactive([FromBody]VBSC_Events_CategoryDTO data)
        {
            return _cms.deactive(data);
        }

    }
}
