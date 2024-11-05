﻿using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AddJobVMSFacadeController : Controller
    {

        public AddJobVMSInterface _ads;

        public AddJobVMSFacadeController(AddJobVMSInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_MRFRequisitionDTO getinitialdata([FromBody]HR_MRFRequisitionDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_MRFRequisitionDTO Post([FromBody]HR_MRFRequisitionDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_MRFRequisitionDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_MRFRequisitionDTO deactivateRecordById([FromBody]HR_MRFRequisitionDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
