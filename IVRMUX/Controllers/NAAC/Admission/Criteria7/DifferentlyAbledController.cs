﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission.Criteria7
{
    [Route("api/[controller]")]
    public class DifferentlyAbledController : Controller
    {
        public DifferentlyAbledDelegate _objdel = new DifferentlyAbledDelegate();

        [Route("loaddata/{id:int}")]
        public NAAC_AC_719_DifferentlyAbled_DTO loaddata(int id)
        {
            NAAC_AC_719_DifferentlyAbled_DTO data = new NAAC_AC_719_DifferentlyAbled_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loaddata(data);
        }

        [Route("save")]
        public NAAC_AC_719_DifferentlyAbled_DTO savedatatab1([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedatatab1(data);
        }

        [Route("EditData")]
        public NAAC_AC_719_DifferentlyAbled_DTO editTab1([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return _objdel.editTab1(data);
        }

        [Route("deactivate")]
        public NAAC_AC_719_DifferentlyAbled_DTO deactivYTab1([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivYTab1(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_AC_719_DifferentlyAbled_DTO deleteuploadfile([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deleteuploadfile(data);
        }

        [Route("getData/{id:int}")]
        public NAAC_AC_719_DifferentlyAbled_DTO getData(int id)
        {
            NAAC_AC_719_DifferentlyAbled_DTO data = new NAAC_AC_719_DifferentlyAbled_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getData(data);
        }

        
        [Route("getDataMC/{id:int}")]
        public NAAC_AC_719_DifferentlyAbled_DTO getDataMC(int id)
        {
            NAAC_AC_719_DifferentlyAbled_DTO data = new NAAC_AC_719_DifferentlyAbled_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getDataMC(data);
        }
        [Route("saveMC")]
        public NAAC_AC_719_DifferentlyAbled_DTO saveMC([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveMC(data);
        }

        [Route("EditDataMC")]
        public NAAC_AC_719_DifferentlyAbled_DTO EditDataMC([FromBody] NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return _objdel.EditDataMC(data);
        }
    }
}
