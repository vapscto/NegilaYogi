using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HREmpSalaryAdvanceFacadeController : Controller
    {
        // GET: api/values
        
    public HREmpSalaryAdvanceInterface _ads;

    public HREmpSalaryAdvanceFacadeController(HREmpSalaryAdvanceInterface adstu)
    {
      _ads = adstu;
    }

    // GET: api/values
    [Route("onloadgetdetails")]
    public HR_Emp_SalaryAdvanceDTO getinitialdata([FromBody]HR_Emp_SalaryAdvanceDTO dto)
    {
      return _ads.getBasicData(dto);
    }

    // POST api/values
    [HttpPost]
    public HR_Emp_SalaryAdvanceDTO Post([FromBody]HR_Emp_SalaryAdvanceDTO dto)
    {
      return _ads.SaveUpdate(dto);
    }

    [Route("getRecordById/{id:int}")]

    public HR_Emp_SalaryAdvanceDTO getcatgrydet(int id)
    {
      // id = 12;
      return _ads.editData(id);
    }
    [Route("deactivateRecordById")]
    public HR_Emp_SalaryAdvanceDTO deactivateRecordById([FromBody]HR_Emp_SalaryAdvanceDTO dto)
    {
      return _ads.deactivate(dto);
    }
        [Route("getDetailsByEmployee")]
        public HR_Emp_SalaryAdvanceDTO getDetailsByEmployee([FromBody]HR_Emp_SalaryAdvanceDTO dto)
            {
            return _ads.getDetailsByEmployee(dto);
            }


        [Route("searchfilter")]
        public HR_Emp_SalaryAdvanceDTO searchfilter([FromBody]HR_Emp_SalaryAdvanceDTO data)
        {
            return _ads.searchfilter(data);
        }

    }
}
