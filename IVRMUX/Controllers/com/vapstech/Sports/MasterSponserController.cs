using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using corewebapi18072016.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class MasterSponserController : Controller
    {
        MasterSponserDelegate delegat = new MasterSponserDelegate();
        [Route("loadgrid/{id:int}")]
        public MasterSponserDTO getDetails(int id)
        {
            MasterSponserDTO dto = new MasterSponserDTO();
            dto.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.getDetails(dto);
        }
        [Route("saveRecord")]
        public MasterSponserDTO saveRecord([FromBody]MasterSponserDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.save(data);
        }
        [Route("EditSponser/{id:int}")]
        public MasterSponserDTO Edit(int id)
        {
            return delegat.EditDetails(id);
        }
        [Route("deactivateSponser")]
        public MasterSponserDTO deactivateSponser([FromBody] MasterSponserDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.deactivateSponser(d);
        }
    }
}
