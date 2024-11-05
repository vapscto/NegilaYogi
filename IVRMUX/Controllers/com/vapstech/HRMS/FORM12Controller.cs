﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FORM12Controller : Controller
    {
    FORM12Delegate del = new FORM12Delegate();
    // GET: api/values
    [HttpGet]
    [Route("getalldetails/{id:int}")]
    public FORM12DTO getalldetails(int id)
    {
      FORM12DTO dto = new FORM12DTO();
      dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
      dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
      return del.onloadgetdetails(dto);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }


    [Route("getEmployeedetailsBySelection")]
    public FORM12DTO getEmployeedetailsBySelection([FromBody]FORM12DTO dto)
    {
      dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
      dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
      return del.getEmployeedetailsBySelection(dto);
    }

    //FilterEmployeeData

    [Route("FilterEmployeeData")]
    public FORM12DTO FilterEmployeeData([FromBody]FORM12DTO dto)
    {
      dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
      dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
      return del.FilterEmployeeData(dto);
    }
  }
}