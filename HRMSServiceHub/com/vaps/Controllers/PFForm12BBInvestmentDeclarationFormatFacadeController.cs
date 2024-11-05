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
    public class PFForm12BBInvestmentDeclarationFormatFacadeController : Controller
    {
    // GET: api/values
    public PFForm12BBInvestmentDeclarationFormatInterface _ads;

    public PFForm12BBInvestmentDeclarationFormatFacadeController(PFForm12BBInvestmentDeclarationFormatInterface adstu)
    {
      _ads = adstu;
    }

    // GET: api/values
    [Route("onloadgetdetails")]
    public PFReportsDTO getinitialdata([FromBody]PFReportsDTO dto)
    {
      return _ads.getBasicData(dto);
    }

    //FilterEmployeeData
    [Route("FilterEmployeeData")]
    public PFReportsDTO FilterEmployeeData([FromBody]PFReportsDTO dto)
    {
      return _ads.FilterEmployeeData(dto);
    }

    [Route("getEmployeedetailsBySelection")]
    public PFReportsDTO getEmployeedetailsBySelection([FromBody]PFReportsDTO dto)
    {
      return _ads.getEmployeedetailsBySelection(dto);
    }
  }
}
