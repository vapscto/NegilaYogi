using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeMasterConfigController : Controller
    {
        FeeMasterConfigDelegate FMD = new FeeMasterConfigDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeMasterConfigurationDTO Get([FromQuery] int id)
        {
            FeeMasterConfigurationDTO data = new FeeMasterConfigurationDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.rolenme = Convert.ToString(HttpContext.Session.GetString("RoleNme"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FMD.getdetailsY(data);
        }
      
     

        [HttpPost]
        public FeeMasterConfigurationDTO savedetail([FromBody] FeeMasterConfigurationDTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            Grouppage.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            Grouppage.rolenme = Convert.ToString(HttpContext.Session.GetString("RoleNme"));
            return FMD.savedetails(Grouppage);
        }

        [Route("editdetails")]
        public FeeMasterConfigurationDTO editdetails([FromBody] FeeMasterConfigurationDTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            Grouppage.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            Grouppage.rolenme = Convert.ToString(HttpContext.Session.GetString("RoleNme"));
            return FMD.editdetails(Grouppage);
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
