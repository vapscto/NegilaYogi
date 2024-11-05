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
    public class FORMNO15GFacadeController : Controller
    {
    public FORMNO15GInterface _ads;

    public FORMNO15GFacadeController(FORMNO15GInterface adstu)
    {
      _ads = adstu;
    }

    // GET: api/values
    [Route("onloadgetdetails")]
    public FORMNO15GDTO getinitialdata([FromBody]FORMNO15GDTO dto)
    {
      return _ads.getBasicData(dto);
    }

    //FilterEmployeeData
    [Route("FilterEmployeeData")]
    public FORMNO15GDTO FilterEmployeeData([FromBody]FORMNO15GDTO dto)
    {
      return _ads.FilterEmployeeData(dto);
    }

    [Route("getEmployeedetailsBySelection")]
    public FORMNO15GDTO getEmployeedetailsBySelection([FromBody]FORMNO15GDTO dto)
    {
      return _ads.getEmployeedetailsBySelection(dto);
    }
  }
}
