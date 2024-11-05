﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.HRMS;
using IVRMUX.Delegates.HRMS;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class NAACACCommitteememberController : Controller
    {
        NAACACCommitteeMemberDelegates del = new NAACACCommitteeMemberDelegates();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public NAACACCommitteeMembersDTO getalldetails(int id)
        {
            NAACACCommitteeMembersDTO dto = new NAACACCommitteeMembersDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public NAACACCommitteeMembersDTO Post([FromBody]NAACACCommitteeMembersDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public NAACACCommitteeMembersDTO editRecord(int id)
        {
            NAACACCommitteeMembersDTO dto = new NAACACCommitteeMembersDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public NAACACCommitteeMembersDTO ActiveDeactiveRecord(int id)
        {
            NAACACCommitteeMembersDTO dto = new NAACACCommitteeMembersDTO();
            dto.NCACCOMM_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.deleterec(dto);
        }
    }
}
