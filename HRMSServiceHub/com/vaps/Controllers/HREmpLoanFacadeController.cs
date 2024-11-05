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
    public class HREmpLoanFacadeController : Controller
    {
    // GET: api/values
    public HREmpLoanInterface _ads;

    public HREmpLoanFacadeController(HREmpLoanInterface adstu)
    {
      _ads = adstu;
    }

    // GET: api/values
    [Route("onloadgetdetails")]
    public HR_Emp_LoanDTO getinitialdata([FromBody]HR_Emp_LoanDTO dto)
    {
      return _ads.getBasicData(dto);
    }

    // POST api/values
    [HttpPost]
    public HR_Emp_LoanDTO Post([FromBody]HR_Emp_LoanDTO dto)
    {
      return _ads.SaveUpdate(dto);
    }

    [Route("getRecordById/{id:int}")]

    public HR_Emp_LoanDTO getcatgrydet(int id)
    {
      // id = 12;
      return _ads.editData(id);
    }
    [Route("deactivateRecordById")]
    public HR_Emp_LoanDTO deactivateRecordById([FromBody]HR_Emp_LoanDTO dto)
    {
      return _ads.deactivate(dto);
    }
    [Route("getDetailsByEmployee")]
    public HR_Emp_LoanDTO getDetailsByEmployee([FromBody]HR_Emp_LoanDTO dto)
        {
        return _ads.getDetailsByEmployee(dto);
        }

    }
}
