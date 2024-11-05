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
    public class HRMasterPANFacadeController : Controller
    {
    // GET: api/values
    public HRMasterPANInterface _ads;

    public HRMasterPANFacadeController(HRMasterPANInterface adstu)
    {
      _ads = adstu;
    }

    // GET: api/values
    [Route("onloadgetdetails")]
    public HRMasterPANDTO getinitialdata([FromBody]HRMasterPANDTO dto)
    {
      return _ads.getBasicData(dto);
    }

    // POST api/values
    [Route("savedetails")]
    public HRMasterPANDTO savedetails([FromBody]HRMasterPANDTO dto)
    {
      return _ads.SaveUpdate(dto);
    }

    [Route("getRecordById/{id:int}")]

    public HRMasterPANDTO getcatgrydet(int id)
    {
      // id = 12;
      return _ads.editData(id);
    }
    [Route("deactivateRecordById")]
    public HRMasterPANDTO deactivateRecordById([FromBody]HRMasterPANDTO dto)
    {
      return _ads.deactivate(dto);
    }
  }
}
