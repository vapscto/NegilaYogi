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
    public class VBadminFacade : Controller
    {
        //VBadminFacade
        public VBadminInterface _cms;

        public VBadminFacade(VBadminInterface cmsdept)
        {
            _cms = cmsdept;
        }
    
        [Route("LoadData")]
        public VBadminDTO LoadData([FromBody]VBadminDTO data)
        {
            
            return _cms.LoadData(data);
           
        }
        [Route("ViewCOEDetails")]
        public VBadminDTO ViewCOEDetails([FromBody]VBadminDTO data)
        {

            return _cms.ViewCOEDetails(data);

        }
    }
}
