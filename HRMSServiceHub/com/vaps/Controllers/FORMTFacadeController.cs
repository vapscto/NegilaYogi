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
    public class FORMTFacadeController : Controller
    {
    public FORMTInterface _ads;

    public FORMTFacadeController(FORMTInterface adstu)
    {
      _ads = adstu;
    }

    // GET: api/values
    [Route("onloadgetdetails")]
    public FORMTDTO getinitialdata([FromBody]FORMTDTO dto)
    {
      return _ads.getBasicData(dto);
    }

    //FilterEmployeeData
    [Route("FilterEmployeeData")]
    public FORMTDTO FilterEmployeeData([FromBody]FORMTDTO dto)
    {
      return _ads.FilterEmployeeData(dto);
    }

    [Route("getEmployeedetailsBySelection")]
    public FORMTDTO getEmployeedetailsBySelection([FromBody]FORMTDTO dto)
    {
      return _ads.getEmployeedetailsBySelection(dto);
    }
  }
}
