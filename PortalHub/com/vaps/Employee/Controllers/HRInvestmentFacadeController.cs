using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HRInvestmentFacadeController : Controller
    {
    // GET: api/values
    public HREmpInvestmentInterface _ads;

    public HRInvestmentFacadeController(HREmpInvestmentInterface adstu)
    {
      _ads = adstu;
    }

    // GET: api/values
    [Route("onloadgetdetails")]
    public EmployeeInvestmentDTO getinitialdata([FromBody]EmployeeInvestmentDTO dto)
    {
      return _ads.getBasicData(dto);
    }

    // POST api/values
    [HttpPost]
    public EmployeeInvestmentDTO Post([FromBody]EmployeeInvestmentDTO dto)
    {
      return _ads.SaveUpdate(dto);
    }

    [Route("getRecordById/{id:int}")]

    public EmployeeInvestmentDTO getcatgrydet(int id)
    {
      // id = 12;
      return _ads.editData(id);
    }
    [Route("deactivateRecordById")]
    public EmployeeInvestmentDTO deactivateRecordById([FromBody]EmployeeInvestmentDTO dto)
    {
      return _ads.deactivate(dto);
    }
    [Route("getDetailsByEmployee")]
    public EmployeeInvestmentDTO getDetailsByEmployee([FromBody]EmployeeInvestmentDTO dto)
        {
        return _ads.getDetailsByEmployee(dto);
        }

    }
}
