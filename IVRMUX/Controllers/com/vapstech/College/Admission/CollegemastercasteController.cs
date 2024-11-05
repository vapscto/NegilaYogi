using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegemastercasteController : Controller
    {        
        CollegemastercasteDelegate mastercastedelStr = new CollegemastercasteDelegate();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public CollegemastercasteDTO Getdetails(CollegemastercasteDTO CollegemastercasteDTO)
        {
            CollegemastercasteDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return mastercastedelStr.GetmastercasteData(CollegemastercasteDTO);
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public CollegemastercasteDTO GetSelectedRowDetails(int ID)
        {
            return mastercastedelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]
        public CollegemastercasteDTO CollegemastercasteDTO([FromBody] CollegemastercasteDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.mastercasteData(MMD);
        }
        
        [Route("MasterDeleteModulesDTO/{id:int}")]
        public CollegemastercasteDTO CollegemastercasteDTO(int ID)
        {
            return mastercastedelStr.MasterDeleteModulesData(ID);
        }
    }
}
