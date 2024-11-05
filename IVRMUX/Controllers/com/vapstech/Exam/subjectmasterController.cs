
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class subjectmasterController : Controller
    {

        subjectmasterDelegates subjectmasterdelStr = new subjectmasterDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public subjectmasterDTO Getdetails(subjectmasterDTO subjectmasterDTO)
        {
            subjectmasterDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return subjectmasterdelStr.GetsubjectmasterData(subjectmasterDTO);            
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public subjectmasterDTO GetSelectedRowDetails(int ID)
        {
            return subjectmasterdelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]      
        public subjectmasterDTO subjectmasterDTO([FromBody] subjectmasterDTO MMD)
        {
            MMD.MI_Id  = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return subjectmasterdelStr.subjectmasterData(MMD);
        }

        [HttpDelete]
        [Route("MasterDeleteModulesDTO/{id:int}")]
        public subjectmasterDTO MasterDeleteModulesDTO(int ID)
        {
            return subjectmasterdelStr.MasterDeleteModulesData(ID);         
        }

    }

}
