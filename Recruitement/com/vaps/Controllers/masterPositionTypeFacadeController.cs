﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class masterPositionTypeFacadeController : Controller
    {
        public masterPositionTypeInterface _ads;

        public masterPositionTypeFacadeController(masterPositionTypeInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Master_PostionTypeDTO getinitialdata([FromBody]HR_Master_PostionTypeDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Master_PostionTypeDTO Post([FromBody]HR_Master_PostionTypeDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }
        [Route("getdata")]
        public HR_Master_PostionTypeDTO getdata([FromBody]HR_Master_PostionTypeDTO dto)
        {
            return _ads.getdata(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_Master_PostionTypeDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_Master_PostionTypeDTO deactivateRecordById([FromBody]HR_Master_PostionTypeDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}