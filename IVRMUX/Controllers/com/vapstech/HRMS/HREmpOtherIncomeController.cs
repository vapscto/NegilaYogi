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
  public class HREmpOtherIncomeController : Controller
  {
        // GET: api/values
        HREmpOtherIncomeDelegate del = new HREmpOtherIncomeDelegate();

      // GET: api/values
      [HttpGet]
      [Route("getalldetails/{id:int}")]
      public HR_Emp_otherIncomeDTO getalldetails(int id)
      {
            HR_Emp_otherIncomeDTO dto = new HR_Emp_otherIncomeDTO();
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
      public HR_Emp_otherIncomeDTO Post([FromBody]HR_Emp_otherIncomeDTO dto)
      {
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));


            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.savedetails(dto);
      }

      [Route("editRecord/{id:int}")]
      public HR_Emp_otherIncomeDTO editRecord(int id)
      {
            HR_Emp_otherIncomeDTO dto = new HR_Emp_otherIncomeDTO();
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getRecorddetailsById(id);
      }

      [Route("ActiveDeactiveRecord/{id:int}")]
      public HR_Emp_otherIncomeDTO ActiveDeactiveRecord(int id)
      {
            HR_Emp_otherIncomeDTO dto = new HR_Emp_otherIncomeDTO();
            dto.HREOI_Id
= id;
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
        return del.deleterec(dto);
      }

        [Route("getDetailsByEmployee/{id:int}")]
        public HR_Emp_otherIncomeDTO getDetailsByEmployee(int id)
            {
            HR_Emp_otherIncomeDTO dto = new HR_Emp_otherIncomeDTO();
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getDetailsByEmployee(dto);
            }

        }
}
