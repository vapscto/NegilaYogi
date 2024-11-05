using System;
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
    public class FORMNO19Controller : Controller
    {

    FORMNO19Delegate del = new FORMNO19Delegate();
    // GET: api/values
    [HttpGet]
    [Route("getalldetails/{id:int}")]
    public FORMNO19DTO getalldetails(int id)
    {
      FORMNO19DTO dto = new FORMNO19DTO();
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
    public FORMNO19DTO getEmployeedetailsBySelection([FromBody]FORMNO19DTO dto)
    {
      dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
      dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
      return del.getEmployeedetailsBySelection(dto);
    }

    //FilterEmployeeData

    [Route("FilterEmployeeData")]
    public FORMNO19DTO FilterEmployeeData([FromBody]FORMNO19DTO dto)
    {
      dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
      dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
      return del.FilterEmployeeData(dto);
    }
  }
}
