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
    public class MasterDesignationFacadeController : Controller
    {
        // GET: api/values

        public MasterDesignationInterface _ads;

        public MasterDesignationFacadeController(MasterDesignationInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Master_DesignationDTO getinitialdata([FromBody]HR_Master_DesignationDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("Onchangedetails")]
        public HR_Master_DesignationDTO orderchangedata([FromBody]HR_Master_DesignationDTO dto)
        {
            return _ads.changeorderData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Master_DesignationDTO Post([FromBody]HR_Master_DesignationDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_Master_DesignationDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_Master_DesignationDTO deactivateRecordById([FromBody]HR_Master_DesignationDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
