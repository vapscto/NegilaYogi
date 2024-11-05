using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class mastercasteController : Controller
    {

        mastercasteDelegates mastercastedelStr = new mastercasteDelegates();

      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public mastercasteDTO Getdetails(mastercasteDTO mastercasteDTO)
        {
            mastercasteDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return mastercastedelStr.GetmastercasteData(mastercasteDTO);            
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public mastercasteDTO GetSelectedRowDetails(int ID)
        {
            return mastercastedelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]      
        public mastercasteDTO mastercasteDTO([FromBody] mastercasteDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.mastercasteData(MMD);            
        }
        [HttpGet]
        [Route("MasterDeleteModulesDTO/{id:int}")]
        public mastercasteDTO mastercasteDTO(int ID)
        {
            return mastercastedelStr.MasterDeleteModulesData(ID);         
        }
    }
}