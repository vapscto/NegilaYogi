using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.University;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.University
{
    [Route("api/[controller]")]
    public class MC_342_HRIncentivesController : Controller
    {
        MC_342_HRIncentivesDelegate del = new MC_342_HRIncentivesDelegate();
        [Route("loaddata/{id:int}")]
        public MC_342_HRIncentivesDTO loaddata(int id)
        {
            MC_342_HRIncentivesDTO data = new MC_342_HRIncentivesDTO();

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("savedata")]
        public MC_342_HRIncentivesDTO savedata([FromBody] MC_342_HRIncentivesDTO data)
        {           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }
        [Route("deactive")]
        public MC_342_HRIncentivesDTO deactive([FromBody] MC_342_HRIncentivesDTO data)
        {           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive(data);
        }
        [Route("editdata")]
        public MC_342_HRIncentivesDTO editdata([FromBody] MC_342_HRIncentivesDTO data)
        {            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.editdata(data);
        }        
    }
}
