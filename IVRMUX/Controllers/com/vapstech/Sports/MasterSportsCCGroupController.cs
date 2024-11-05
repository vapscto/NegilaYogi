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
    public class MasterSportsCCGroupController : Controller
    {
        MasterSportsCCGroupDelegate delegat = new MasterSportsCCGroupDelegate();
        [Route("loadgrid/{id:int}")]
        public MasterSportsCCGroupDTO getDetails(int id)
        {
            MasterSportsCCGroupDTO dto = new MasterSportsCCGroupDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.getDetails(dto);
        }
        [Route("saveRecord")]
        public MasterSportsCCGroupDTO saveRecord([FromBody]MasterSportsCCGroupDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return delegat.save(data);
        }
        [Route("Edit/{id:int}")]
        public MasterSportsCCGroupDTO Edit(int id)
        {
            return delegat.EditDetails(id);
        }
        [Route("deactivate")]
        public MasterSportsCCGroupDTO deactivate([FromBody] MasterSportsCCGroupDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.deactivate(d);
        }
    }
}
