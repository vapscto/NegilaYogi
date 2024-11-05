﻿using System;
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
    public class MasterIncomeTaxDetailsFacadeController : Controller
    {

        public MasterIncomeTaxDetailsInterface _ads;

        public MasterIncomeTaxDetailsFacadeController(MasterIncomeTaxDetailsInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Master_IncomeTax_DetailsDTO getinitialdata([FromBody]HR_Master_IncomeTax_DetailsDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Master_IncomeTax_DetailsDTO Post([FromBody]HR_Master_IncomeTax_DetailsDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_Master_IncomeTax_DetailsDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_Master_IncomeTax_DetailsDTO deactivateRecordById([FromBody]HR_Master_IncomeTax_DetailsDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
