using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterEarningsDeductionsController : Controller
    {
    MasterEarningsDeductionsDelegate del = new MasterEarningsDeductionsDelegate();

    // GET: api/values
    [HttpGet]
    [Route("getalldetails/{id:int}")]
    public HR_Master_EarningsDeductionsDTO getalldetails(int id)
    {
      HR_Master_EarningsDeductionsDTO dto = new HR_Master_EarningsDeductionsDTO();
      dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
    }

        [Route("validateordernumber")]
        public HR_Master_EarningsDeductionsDTO validateordernumber([FromBody]HR_Master_EarningsDeductionsDTO dto)
        {
            // HR_Master_GroupTypeDTO dto = new HR_Master_GroupTypeDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.orderchangedata(dto);
        }

        // POST api/values
        [HttpPost]
    public HR_Master_EarningsDeductionsDTO Post([FromBody]HR_Master_EarningsDeductionsDTO dto)
    {
      dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.savedetails(dto);
    }

    [Route("editRecord/{id:int}")]
    public HR_Master_EarningsDeductionsDTO editRecord(int id)
    {
      HR_Master_EarningsDeductionsDTO dto = new HR_Master_EarningsDeductionsDTO();
      dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
      return del.getRecorddetailsById(id);
    }

    [Route("ActiveDeactiveRecord/{id:int}")]
    public HR_Master_EarningsDeductionsDTO ActiveDeactiveRecord(int id)
    {
      HR_Master_EarningsDeductionsDTO dto = new HR_Master_EarningsDeductionsDTO();
      dto.HRMED_Id = id;
      dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.deleterec(dto);
    }



    //type


    [HttpGet]
    [Route("getalldetailstype/{id:int}")]
    public HR_Master_EarningsDeductions_TypeDTO getalldetailstype(int id)
    {
      HR_Master_EarningsDeductions_TypeDTO dto = new HR_Master_EarningsDeductions_TypeDTO();
      dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
      return del.onloadgetdetailstype(dto);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Gets(int id)
    {
      return "value";
    }

    // POST api/values
    [HttpPost]
    [Route("savetype")]
    public HR_Master_EarningsDeductions_TypeDTO Post([FromBody]HR_Master_EarningsDeductions_TypeDTO dto)
    {
      dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
      return del.savedetails(dto);
    }

    [Route("editRecordtype/{id:int}")]
    public HR_Master_EarningsDeductions_TypeDTO editRecordtype(int id)
    {
      HR_Master_EarningsDeductions_TypeDTO dto = new HR_Master_EarningsDeductions_TypeDTO();
      return del.getRecorddetailsByIdType(id);
    }

    [Route("ActiveDeactiveRecordtype/{id:int}")]
    public HR_Master_EarningsDeductions_TypeDTO ActiveDeactiveRecordtype(int id)
    {
      HR_Master_EarningsDeductions_TypeDTO dto = new HR_Master_EarningsDeductions_TypeDTO();
      dto.HRMEDT_Id = id;
      dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
      return del.deleterec(dto);
    }



  }
}
