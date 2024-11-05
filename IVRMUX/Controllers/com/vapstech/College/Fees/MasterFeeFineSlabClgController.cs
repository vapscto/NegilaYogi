using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.Fees;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class MasterFeeFineSlabClgController : Controller
    {
        public MasterFeeFineSlabClgDelegate _feeFS = new MasterFeeFineSlabClgDelegate();
        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterFeeFineSlabClg_DTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _feeFS.getdetails(id);
        }
        //for edit
        [Route("getdetails/{id:int}")]
        public MasterFeeFineSlabClg_DTO getdetail(int id)
        {
           // id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return _feeFS.getpagedetails(id);
        }
        [Route("Editdetails/{id:int}")]
        public MasterFeeFineSlabClg_DTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return _feeFS.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        [Route("savedetail")]
        public MasterFeeFineSlabClg_DTO savedetail([FromBody] MasterFeeFineSlabClg_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _feeFS.savedetails(data);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public MasterFeeFineSlabClg_DTO Delete(int id)
        {
            return _feeFS.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public MasterFeeFineSlabClg_DTO deactvate([FromBody] MasterFeeFineSlabClg_DTO id)
        {
            return _feeFS.deactivateAcademicYear(id);
        }

    }
}
