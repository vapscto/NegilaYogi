using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterBoardandSchoolTypeController : Controller
    {
        MasterBoardandSchoolTypeDelegate mcd = new MasterBoardandSchoolTypeDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterBoardDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mcd.getAll(id);
        }
        [HttpGet]
        [Route("getallSchoolTypedetails/{id:int}")]
        public MasterSchoolTypeDTO getallSchoolTypedetails([FromQuery] int id)
        {
            return mcd.getAllSchoolType(id);
        }
        [Route("getdetails/{id:int}")]
        public MasterBoardDTO getdetail(int id)
        {
            return mcd.boardDet(id);
        }
        [Route("getSchoolTypedetails/{id:int}")]
        public MasterSchoolTypeDTO getSchoolTypedetails(int id)
        {
            return mcd.schoolTypeDet(id);
        }

        // POST api/values
        [HttpPost]
        public MasterBoardDTO savedetail([FromBody] MasterBoardDTO org)
        {
            org.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mcd.savedetails(org);
        }
        [HttpPost]
        [Route("saveSchoolTypeData")]
        public MasterSchoolTypeDTO saveSchoolTypeData([FromBody] MasterSchoolTypeDTO org)
        {
            return mcd.saveschlTypetails(org);
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        //DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterBoardDTO Delete(int id)
        {
            return mcd.deleterec(id);
        }
        [HttpDelete]
        [Route("deleteSchoolTypedetails/{id:int}")]
        public MasterSchoolTypeDTO deleteSchoolTypedetails(int id)
        {
            return mcd.deleteSchoolTyperec(id);
        }
    }
}
