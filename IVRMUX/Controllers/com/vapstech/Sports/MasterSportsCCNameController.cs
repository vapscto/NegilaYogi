using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Sports;
using PreadmissionDTOs.com.vaps.Sports;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class MasterSportsCCNameController : Controller
    {
        MasterSportsCCNameDelegate delegat = new MasterSportsCCNameDelegate();
        [Route("loadgrid/{id:int}")]
        public MasterSportsCCNameDTO getDetails(int id)
        {
            MasterSportsCCNameDTO dto = new MasterSportsCCNameDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.getDetails(dto);
        }
        [Route("saveRecord")]
        public MasterSportsCCNameDTO saveRecord([FromBody]MasterSportsCCNameDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.save(data);
        }
        [Route("Edit/{id:int}")]
        public MasterSportsCCNameDTO Edit(int id)
        {
            return delegat.EditDetails(id);
        }
        [Route("deactivate")]
        public MasterSportsCCNameDTO deactivate([FromBody] MasterSportsCCNameDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.deactivate(d);
        }
    }
}
