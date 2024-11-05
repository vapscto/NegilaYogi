using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.FrontOffice;
using corewebapi18072016.Delegates.com.vapstech.FrontOffice;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]

    public class EmployeeShiftMappingController : Controller
    {
        EmployeeShiftMappingDelegate ad = new EmployeeShiftMappingDelegate();

        // GET: api/Academic/5

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public EmployeeShiftMappingDTO Get(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getdetails(id);
        }

        [Route("Shiftname/{id:int}")]
        public EmployeeShiftMappingDTO Shiftname(int id)
        {
            EmployeeShiftMappingDTO data = new EmployeeShiftMappingDTO();
            data.FOMS_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.Shiftname(data);
        }

        [HttpPost]
        [Route("savedetail")]
        public EmployeeShiftMappingDTO getdetail([FromBody]EmployeeShiftMappingDTO ac)
        {
            // HttpContext.Session.SetString("ClassCategoryID", id.ToString());
            ac.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            ac.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savedetail(ac);
        }
        [Route("editdetails/{id:int}")]
        public EmployeeShiftMappingDTO editdetails(int id)
        {
            return ad.editdetails(id);
        }

        [Route("deletedetails")]
        public EmployeeShiftMappingDTO Delete([FromBody]EmployeeShiftMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deleterec(data);
        }

        [Route("get_departments")]
        public EmployeeShiftMappingDTO get_departments([FromBody]EmployeeShiftMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_departments(data);
        }
        
        [Route("get_designation")]
        public EmployeeShiftMappingDTO get_designation([FromBody]EmployeeShiftMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_designation(data);
        }

        [Route("get_employee")]
        public EmployeeShiftMappingDTO get_employee([FromBody]EmployeeShiftMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.get_employee(data);
        }
    }
}
