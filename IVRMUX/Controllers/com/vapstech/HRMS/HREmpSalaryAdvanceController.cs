using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class HREmpSalaryAdvanceController : Controller
    {
        HREmpSalaryAdvanceDelegate del = new HREmpSalaryAdvanceDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public  HR_Emp_SalaryAdvanceDTO getalldetails(int id)
        {
           HR_Emp_SalaryAdvanceDTO dto = new  HR_Emp_SalaryAdvanceDTO();
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
        public  HR_Emp_SalaryAdvanceDTO Post([FromBody] HR_Emp_SalaryAdvanceDTO dto)
        {
          dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
          return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public  HR_Emp_SalaryAdvanceDTO editRecord(int id)
        {
           HR_Emp_SalaryAdvanceDTO dto = new  HR_Emp_SalaryAdvanceDTO();
          dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public  HR_Emp_SalaryAdvanceDTO ActiveDeactiveRecord(int id)
        {
           HR_Emp_SalaryAdvanceDTO dto = new  HR_Emp_SalaryAdvanceDTO();
          dto.HRESA_Id = id;
          dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
          return del.deleterec(dto);
        }


        [Route("getDetailsByEmployee")]
        public HR_Emp_SalaryAdvanceDTO getDetailsByEmployee([FromBody] HR_Emp_SalaryAdvanceDTO dto)
        {
       // HR_Emp_SalaryAdvanceDTO dto = new HR_Emp_SalaryAdvanceDTO();
      //  dto.HRME_Id = id;
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
        return del.getDetailsByEmployee(dto);
        }

        [Route("searchfilter")]
        public HR_Emp_SalaryAdvanceDTO searchfilter([FromBody] HR_Emp_SalaryAdvanceDTO data)

        {

    
           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.searchfilter(data);
        }
    }
}
