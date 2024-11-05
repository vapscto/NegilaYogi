using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Hostel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;
using PreadmissionDTOs.com.vaps.HRMS;

namespace IVRMUX.Controllers.com.vapstech.Hostel
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class Hostel_Student_GatePassController : Controller
    {
        Hostel_Student_GatePassDelegate del = new Hostel_Student_GatePassDelegate();
            // GET: api/values
            [HttpGet]
            [Route("getalldetails/{id:int}")]
           
            public Hostel_Student_GatePassDTO getalldetails(int id)
            {
                Hostel_Student_GatePassDTO dto = new Hostel_Student_GatePassDTO();
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
                return del.onloadgetdetails(dto);
            }

            // POST api/values
            [HttpPost]
        [Route("save")]
        public Hostel_Student_GatePassDTO save([FromBody] Hostel_Student_GatePassDTO dto)
            {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
                return del.savedetails(dto);
            }

        [Route("Edit")]
        public Hostel_Student_GatePassDTO Edit([FromBody] Hostel_Student_GatePassDTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.Edit(dto);
        }


        [Route("ActiveDeactiveRecord")]
        public Hostel_Student_GatePassDTO ActiveDeactiveRecord([FromBody] Hostel_Student_GatePassDTO dto)
        {   
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleterec(dto);
        }
    }
 }


