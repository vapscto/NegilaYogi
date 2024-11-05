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
    public class CTCReportFacadeController : Controller
    {
    public CTCReportInterface _ads;

    public CTCReportFacadeController(CTCReportInterface adstu)
    {
      _ads = adstu;
    }

    // GET: api/values
    [Route("onloadgetdetails")]
    public CTCReportDTO getinitialdata([FromBody]CTCReportDTO dto)
    {
      return _ads.getBasicData(dto);
    }

    ////FilterEmployeeData
    //[Route("FilterEmployeeData")]
    //public CTCReportDTO FilterEmployeeData([FromBody]CTCReportDTO dto)
    //{
    //  return _ads.FilterEmployeeData(dto);
    //}

    [Route("getEmployeedetailsBySelection")]
    public CTCReportDTO getEmployeedetailsBySelection([FromBody]CTCReportDTO dto)
    {
      return _ads.getEmployeedetailsBySelection(dto);
    }
  }
}
