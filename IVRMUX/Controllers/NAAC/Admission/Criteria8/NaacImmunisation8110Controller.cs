﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission.Criteria8;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission.Criteria8;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission.Criteria8
{
    [Route("api/[controller]")]
    public class NaacImmunisation8110Controller : Controller
    {
        NaacImmunisation8110Delegate del = new NaacImmunisation8110Delegate();
        [Route("loaddata/{id:int}")]
        public NAAC_MC_8110_Immunisation_DTO loaddata(int id)
        {
            NAAC_MC_8110_Immunisation_DTO data = new NAAC_MC_8110_Immunisation_DTO();
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("save")]
        public NAAC_MC_8110_Immunisation_DTO save([FromBody] NAAC_MC_8110_Immunisation_DTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }
        [Route("deactive")]
        public NAAC_MC_8110_Immunisation_DTO deactive([FromBody] NAAC_MC_8110_Immunisation_DTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive(data);
        }
        [Route("EditData")]
        public NAAC_MC_8110_Immunisation_DTO EditData([FromBody] NAAC_MC_8110_Immunisation_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }


        [Route("viewuploadflies")]
        public NAAC_MC_8110_Immunisation_DTO viewuploadflies([FromBody] NAAC_MC_8110_Immunisation_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }


        [Route("deleteuploadfile")]
        public NAAC_MC_8110_Immunisation_DTO deleteuploadfile([FromBody] NAAC_MC_8110_Immunisation_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }
        [Route("getcomment")]
        public NAAC_MC_8110_Immunisation_DTO getcomment([FromBody] NAAC_MC_8110_Immunisation_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_MC_8110_Immunisation_DTO getfilecomment([FromBody] NAAC_MC_8110_Immunisation_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getfilecomment(data);
        }
        [Route("savecomments")]
        public NAAC_MC_8110_Immunisation_DTO savecomments([FromBody] NAAC_MC_8110_Immunisation_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_MC_8110_Immunisation_DTO savefilewisecomments([FromBody] NAAC_MC_8110_Immunisation_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savefilewisecomments(data);
        }
    }
}