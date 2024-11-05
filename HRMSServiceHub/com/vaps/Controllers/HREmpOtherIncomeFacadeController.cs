using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HREmpOtherIncomeFacadeController : Controller
    {
    // GET: api/values
    public HREmpOtherIncomeInterface _ads;

    public HREmpOtherIncomeFacadeController(HREmpOtherIncomeInterface adstu)
    {
      _ads = adstu;
    }

    // GET: api/values
    [Route("onloadgetdetails")]
    public HR_Emp_otherIncomeDTO getinitialdata([FromBody]HR_Emp_otherIncomeDTO dto)
    {
      return _ads.getBasicData(dto);
    }

    // POST api/values
    [HttpPost]
    public HR_Emp_otherIncomeDTO Post([FromBody]HR_Emp_otherIncomeDTO dto)
    {
      return _ads.SaveUpdate(dto);
    }

    [Route("getRecordById/{id:int}")]

    public HR_Emp_otherIncomeDTO getcatgrydet(int id)
    {
      // id = 12;
      return _ads.editData(id);
    }
    [Route("deactivateRecordById")]
    public HR_Emp_otherIncomeDTO deactivateRecordById([FromBody]HR_Emp_otherIncomeDTO dto)
    {
      return _ads.deactivate(dto);
    }
    [Route("getDetailsByEmployee")]
    public HR_Emp_otherIncomeDTO getDetailsByEmployee([FromBody]HR_Emp_otherIncomeDTO dto)
        {
        return _ads.getDetailsByEmployee(dto);
        }

    }
}
