using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterRolePreviledgeController : Controller
    {
        MasterRolePreviledgeDelegate pgmod = new MasterRolePreviledgeDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterRolePreviledgeDTO Get(int id)
        {
            return pgmod.getmoduledet(id);
        }

        [Route("mobilegetalldetails/{id:int}")]
        public MasterRolePreviledgeDTO mobilegetalldetails(int id)
        {
            return pgmod.mobilegetalldetails(id);
        }


        [Route("getdetails/{id:int}")]
        public MasterRolePreviledgeDTO Getpagedata(int id)
        {
            return pgmod.getselectedpg(id);
        }

      

        [HttpPost]
        [Route("getmodulepages")]
        public MasterRolePreviledgeDTO getpages([FromBody] MasterRolePreviledgeDTO id)
        {
            return pgmod.getmodulepagedata(id);
        }

        [Route("mobilegetmodulepages")]
        public MasterRolePreviledgeDTO mobilegetmodulepages([FromBody] MasterRolePreviledgeDTO id)
        {
            return pgmod.mobilegetmodulepages(id);
        }
        // POST api/values
        [HttpPost]
        public MasterRolePreviledgeDTO savedata([FromBody] MasterRolePreviledgeDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");
            return pgmod.savedetails(pgmodu);
        }
        [Route("mobilesaveorgdet")]
        public MasterRolePreviledgeDTO mobilesaveorgdet([FromBody] MasterRolePreviledgeDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");
            return pgmod.mobilesaveorgdet(pgmodu);
        }

        [HttpPost("{id}")]
        public MasterRolePreviledgeDTO Put(int id, [FromBody]MasterRolePreviledgeDTO value)
        {
            return pgmod.getsearchdata(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletemodpages/{id:int}")]
        public MasterRolePreviledgeDTO Delete(int id)
        {
            return pgmod.deleterec(id);
        }

        [Route("mobiledeletemodpages")]
        public MasterRolePreviledgeDTO mobiledeletemodpages([FromBody] MasterRolePreviledgeDTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return pgmod.mobiledeletemodpages(dto);
        }
    }
}
