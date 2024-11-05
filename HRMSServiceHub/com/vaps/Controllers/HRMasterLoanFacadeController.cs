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
    public class HRMasterLoanFacadeController : Controller
    {
    // GET: api/values
    public HRMasterLoanInterface _ads;

    public HRMasterLoanFacadeController(HRMasterLoanInterface adstu)
    {
      _ads = adstu;
    }

    // GET: api/values
    [Route("onloadgetdetails")]
    public HRMasterLoanDTO getinitialdata([FromBody]HRMasterLoanDTO dto)
    {
      return _ads.getBasicData(dto);
    }

    // POST api/values
    [HttpPost]
    public HRMasterLoanDTO Post([FromBody]HRMasterLoanDTO dto)
    {
      return _ads.SaveUpdate(dto);
    }

    [Route("getRecordById/{id:int}")]

    public HRMasterLoanDTO getcatgrydet(int id)
    {
      // id = 12;
      return _ads.editData(id);
    }
    [Route("deactivateRecordById")]
    public HRMasterLoanDTO deactivateRecordById([FromBody]HRMasterLoanDTO dto)
    {
      return _ads.deactivate(dto);
    }
  }
}
