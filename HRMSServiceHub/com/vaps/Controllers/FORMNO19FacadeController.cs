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
    public class FORMNO19FacadeController : Controller
    {
    public FORMNO19Interface _ads;

    public FORMNO19FacadeController(FORMNO19Interface adstu)
    {
      _ads = adstu;
    }

    // GET: api/values
    [Route("onloadgetdetails")]
    public FORMNO19DTO getinitialdata([FromBody]FORMNO19DTO dto)
    {
      return _ads.getBasicData(dto);
    }

    //FilterEmployeeData
    [Route("FilterEmployeeData")]
    public FORMNO19DTO FilterEmployeeData([FromBody]FORMNO19DTO dto)
    {
      return _ads.FilterEmployeeData(dto);
    }

    [Route("getEmployeedetailsBySelection")]
    public FORMNO19DTO getEmployeedetailsBySelection([FromBody]FORMNO19DTO dto)
    {
      return _ads.getEmployeedetailsBySelection(dto);
    }
  }
}
