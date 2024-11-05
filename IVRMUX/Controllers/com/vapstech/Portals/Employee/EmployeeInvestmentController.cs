using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;
namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{


    [Route("api/[controller]")]
    public class EmployeeInvestmentController : Controller
    {
        EmployeeInvestmentDelegate objdelegate1 = new EmployeeInvestmentDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/values
        [HttpGet]
    [Route("getalldetails/{id:int}")]
    public EmployeeInvestmentDTO getalldetails(int id)
    {
            EmployeeInvestmentDTO dto = new EmployeeInvestmentDTO();
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdelegate1.onloadgetdetails(dto);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/values
    [HttpPost]
    public EmployeeInvestmentDTO Post([FromBody]EmployeeInvestmentDTO dto)
    {
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
         dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdelegate1.savedetails(dto);
    }

    [Route("editRecord/{id:int}")]
    public EmployeeInvestmentDTO editRecord(int id)
    {
            EmployeeInvestmentDTO dto = new EmployeeInvestmentDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdelegate1.getRecorddetailsById(id);
    }

    [Route("ActiveDeactiveRecord/{id:int}")]
    public EmployeeInvestmentDTO ActiveDeactiveRecord(int id)
    {
        EmployeeInvestmentDTO dto = new EmployeeInvestmentDTO();
        dto.HREID_Id= id;
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
        dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        return objdelegate1.deleterec(dto);
    }

    [Route("getDetailsByEmployee/{id:int}")]
    public EmployeeInvestmentDTO getDetailsByEmployee(int id)
    {
            EmployeeInvestmentDTO dto = new EmployeeInvestmentDTO();
        dto.HRME_Id = id;
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdelegate1.getDetailsByEmployee(dto);
    }

}
}
