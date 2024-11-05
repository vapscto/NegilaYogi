using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.University.Interface;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.University.FacadeController
{
    [Route("api/[controller]")]
    public class MC_342_HRIncentivesFacade : Controller
    {
        public MC_342_HRIncentivesInterface _inter;
        public MC_342_HRIncentivesFacade(MC_342_HRIncentivesInterface i)
        {
            _inter = i;
        }
        [HttpPost]
        [Route("getdata")]
        public MC_342_HRIncentivesDTO getdata([FromBody] MC_342_HRIncentivesDTO data)
        {
            return _inter.loaddata(data);
        }        
        [Route("savedata")]
        public MC_342_HRIncentivesDTO savedata([FromBody] MC_342_HRIncentivesDTO data)
        {
            return _inter.savedata(data);
        }
        [Route("deactive")]
        public MC_342_HRIncentivesDTO deactive([FromBody] MC_342_HRIncentivesDTO data)
        {
            return _inter.deactive(data);
        }
        [Route("editdata")]
        public MC_342_HRIncentivesDTO editdata([FromBody] MC_342_HRIncentivesDTO data)
        {
            return _inter.editdata(data);
        }        
    }
}
