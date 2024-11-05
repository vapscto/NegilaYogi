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
    public class FORM12FacadeController : Controller
    {
    public FORM12Interface _ads;

    public FORM12FacadeController(FORM12Interface adstu)
    {
      _ads = adstu;
    }

    // GET: api/values
    [Route("onloadgetdetails")]
    public FORM12DTO getinitialdata([FromBody]FORM12DTO dto)
    {
      return _ads.getBasicData(dto);
    }

    //FilterEmployeeData
    [Route("FilterEmployeeData")]
    public FORM12DTO FilterEmployeeData([FromBody]FORM12DTO dto)
    {
      return _ads.FilterEmployeeData(dto);
    }

    [Route("getEmployeedetailsBySelection")]
    public FORM12DTO getEmployeedetailsBySelection([FromBody]FORM12DTO dto)
    {
      return _ads.getEmployeedetailsBySelection(dto);
    }
  }
}
