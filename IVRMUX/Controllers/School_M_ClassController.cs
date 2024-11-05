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
    public class School_M_ClassController : Controller
    {
        School_M_ClassDelegate M_Class = new School_M_ClassDelegate();

        // POST api/values
        [HttpPost]
        public School_M_ClassDTO SaveSchool_M_Class([FromBody] School_M_ClassDTO Ins)
        {
            Ins.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return M_Class.saveSchool_M_Classdetails(Ins);
        }


        [Route("SearchByColumn")]
        public School_M_ClassDTO searchByColumn([FromBody] School_M_ClassDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return M_Class.searchByColumn(data);
        }
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public School_M_ClassDTO Get([FromQuery] int id)
        {
            School_M_ClassDTO classlist = new School_M_ClassDTO();
            classlist.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return M_Class.getSchool_M_Classdata(classlist);
        }


        [Route("getSchool_M_ClassById/{id:int}")]
        public School_M_ClassDTO GetSchool_M_ClassDetailsById(int id)
        {
            return M_Class.getSchool_M_ClassDetailsbySchool_M_ClassId(id);
        }

        // DELETE api/values/5
        [HttpGet]
        [Route("deletedetails/{id:int}")]
        public School_M_ClassDTO Delete(int id)
        {
            School_M_ClassDTO data = new School_M_ClassDTO();
            data.ASMCL_Id = id;
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return M_Class.deleterec(data);
        }

        
    }
}
