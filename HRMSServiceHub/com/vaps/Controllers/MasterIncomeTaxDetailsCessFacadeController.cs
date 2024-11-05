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
    public class MasterIncomeTaxDetailsCessFacadeController : Controller
    {

        public MasterIncomeTaxDetailsCessInterface _ads;

        public MasterIncomeTaxDetailsCessFacadeController(MasterIncomeTaxDetailsCessInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Master_IncomeTax_Details_CessDTO getinitialdata([FromBody]HR_Master_IncomeTax_Details_CessDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Master_IncomeTax_Details_CessDTO Post([FromBody]HR_Master_IncomeTax_Details_CessDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_Master_IncomeTax_Details_CessDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_Master_IncomeTax_Details_CessDTO deactivateRecordById([FromBody]HR_Master_IncomeTax_Details_CessDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
