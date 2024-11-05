﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
  public class HREmpTDSController : Controller
  {
        // GET: api/values
        HREmpTDSDelegate del = new HREmpTDSDelegate();

      // GET: api/values
      [HttpGet]
      [Route("getalldetails/{id:int}")]
      public HR_Emp_TDSDTO getalldetails(int id)
      {
            HR_Emp_TDSDTO dto = new HR_Emp_TDSDTO();
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
      }

      // GET api/values/5
      [HttpGet("{id}")]
      public string Get(int id)
      {
        return "value";
      }

      // POST api/values
      [HttpPost]
      public HR_Emp_TDSDTO Post([FromBody]HR_Emp_TDSDTO dto)
      {
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));


            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.savedetails(dto);
      }

      [Route("editRecord/{id:int}")]
      public HR_Emp_TDSDTO editRecord(int id)
      {
            HR_Emp_TDSDTO dto = new HR_Emp_TDSDTO();
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getRecorddetailsById(id);
      }

      [Route("ActiveDeactiveRecord/{id:int}")]
      public HR_Emp_TDSDTO ActiveDeactiveRecord(int id)
      {
            HR_Emp_TDSDTO dto = new HR_Emp_TDSDTO();
            dto.HRETDS_Id = id;
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
        return del.deleterec(dto);
      }

        [Route("getDetailsByEmployee/{id:int}")]
        public HR_Emp_TDSDTO getDetailsByEmployee(int id)
            {
            HR_Emp_TDSDTO dto = new HR_Emp_TDSDTO();
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getDetailsByEmployee(dto);
            }

        }
}