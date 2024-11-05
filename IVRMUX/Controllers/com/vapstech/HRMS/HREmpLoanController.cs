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
  public class HREmpLoanController : Controller
  {
      // GET: api/values
      HREmpLoanDelegate del = new HREmpLoanDelegate();

      // GET: api/values
      [HttpGet]
      [Route("getalldetails/{id:int}")]
      public HR_Emp_LoanDTO getalldetails(int id)
      {
        HR_Emp_LoanDTO dto = new HR_Emp_LoanDTO();
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

      // POST api/values
      [HttpPost]
      public HR_Emp_LoanDTO Post([FromBody]HR_Emp_LoanDTO dto)
      {
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

        int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
        dto.ASMAY_Id = ASMAY_Id;

            return del.savedetails(dto);
      }

      [Route("editRecord/{id:int}")]
      public HR_Emp_LoanDTO editRecord(int id)
      {
        HR_Emp_LoanDTO dto = new HR_Emp_LoanDTO();
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        return del.getRecorddetailsById(id);
      }

      [Route("ActiveDeactiveRecord/{id:int}")]
      public HR_Emp_LoanDTO ActiveDeactiveRecord(int id)
      {
        HR_Emp_LoanDTO dto = new HR_Emp_LoanDTO();
        dto.HREL_Id = id;
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
        return del.deleterec(dto);
      }

        [Route("getDetailsByEmployee/{id:int}")]
        public HR_Emp_LoanDTO getDetailsByEmployee(int id)
            {
            HR_Emp_LoanDTO dto = new HR_Emp_LoanDTO();
            dto.HRME_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getDetailsByEmployee(dto);
            }

        }
}
