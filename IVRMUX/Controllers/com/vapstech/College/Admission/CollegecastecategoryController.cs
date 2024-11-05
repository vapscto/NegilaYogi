using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegecastecategoryController : Controller
    {
        CollegecastecategoryDelegate castecategorydelStr = new CollegecastecategoryDelegate();
        
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails/")]
        public CollegecastecategoryDTO Getdetails(CollegecastecategoryDTO CollegecastecategoryDTO)
        {
            return castecategorydelStr.GetcastecategoryData(CollegecastecategoryDTO);
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public CollegecastecategoryDTO GetSelectedRowDetails(int ID)
        {
            return castecategorydelStr.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public CollegecastecategoryDTO CollegecastecategoryDTO([FromBody] CollegecastecategoryDTO MMD)
        {
            return castecategorydelStr.castecategoryData(MMD);
        }
       
        [Route("MasterDeleteModulesDTO/{id:int}")]
        public CollegecastecategoryDTO CollegecastecategoryDTO(int ID)
        {
            return castecategorydelStr.MasterDeleteModulesData(ID);
        }
    }
}
