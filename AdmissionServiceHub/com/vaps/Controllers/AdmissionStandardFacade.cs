using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vapstech.Controllers
{
    [Route("api/[controller]")]
    public class AdmissionStandardFacade : Controller
    {
        // GET: api/values
        public AdmissionStandardInterface _ads;

        public AdmissionStandardFacade(AdmissionStandardInterface adstu)
        {
            _ads = adstu;
        }
       
        // POST api/values
        [HttpPost]
        [Route("savedata")]
        public AdmissionStandardDTO getclassstudentlist([FromBody]AdmissionStandardDTO student)
        {
            return _ads.getlisttwo(student);
        }

        [HttpGet]
        [Route("loaddata/{id:int}")]        
        public AdmissionStandardDTO getdata(int id)
        {
            return _ads.getlistdata(id);
        }

        // Admission Cancel Configuration

        [Route("CancelConfigLoad")]
        public AdmissionStandardDTO CancelConfigLoad([FromBody]AdmissionStandardDTO data)
        {
            return _ads.CancelConfigLoad(data);
        }

        [Route("SaveCancelConfigData")]
        public AdmissionStandardDTO SaveCancelConfigData([FromBody]AdmissionStandardDTO data)
        {
            return _ads.SaveCancelConfigData(data);
        }

        [Route("EditCancelConfig")]
        public AdmissionStandardDTO EditCancelConfig([FromBody]AdmissionStandardDTO data)
        {
            return _ads.EditCancelConfig(data);
        }

        [Route("ActiveDeactiveCancelConfig")]
        public AdmissionStandardDTO ActiveDeactiveCancelConfig([FromBody]AdmissionStandardDTO data)
        {
            return _ads.ActiveDeactiveCancelConfig(data);
        }
    }
}
