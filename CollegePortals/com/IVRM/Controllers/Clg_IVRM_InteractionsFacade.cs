using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegePortals.com.IVRM.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.IVRM;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegePortals.com.IVRM.Controllers
{
    [Route("api/[controller]")]
    public class Clg_IVRM_InteractionsFacade : Controller
    {
        public Clg_IVRM_InteractionsInterface _ads;

        public Clg_IVRM_InteractionsFacade(Clg_IVRM_InteractionsInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public Task<IVRM_School_InteractionsDTO> getloaddata([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.getloaddata(data);
        }
        [Route("getdetails")]
        public Task<IVRM_School_InteractionsDTO> getdetails([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.getdetails(data);
        }


        [Route("getstudent")]
        public Task<IVRM_School_InteractionsDTO> getstudent([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.getstudent(data);
        }
        [Route("Getbranch")]
        public IVRM_School_InteractionsDTO Getbranch([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.Getbranch(data);
        }
        [Route("Getsection")]
        public IVRM_School_InteractionsDTO Getsection([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.Getsection(data);
        }
        [Route("Getsemester")]
        public IVRM_School_InteractionsDTO Getsemester([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.Getsemester(data);
        }

        [Route("savedetails")]
        public IVRM_School_InteractionsDTO savedetails([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.savedetails(data);
        }

        [Route("savereply")]
        public IVRM_School_InteractionsDTO savereply([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.savereply(data);
        }
        [Route("reply")]
        public Task<IVRM_School_InteractionsDTO> reply([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.reply(data);
        }
        [Route("deletemsg")]
        public IVRM_School_InteractionsDTO deletemsg([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.deletemsg(data);
        }

        [Route("deleteinboxmsg")]
        public IVRM_School_InteractionsDTO deleteinboxmsg([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.deleteinboxmsg(data);
        }
        [Route("seen")]
        public IVRM_School_InteractionsDTO seen([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.seen(data);
        }
    }
}
