using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterSubscriptionFacade : Controller
    {

        public MasterSubscriptionInterface _objInter;
        public MasterSubscriptionFacade(MasterSubscriptionInterface data)
        {
            _objInter = data;
        }

        [Route("getdetails")]
        public Task<Master_Subscription_DTO> getdetails([FromBody]Master_Subscription_DTO data)
        {
            return _objInter.getdetails(data);
        }


        [Route("Savedata")]
        public Master_Subscription_DTO Savedata([FromBody]Master_Subscription_DTO data)
        {
            return _objInter.Savedata(data);
        }

        [Route("EditData")]
        public Master_Subscription_DTO EditData([FromBody]Master_Subscription_DTO data)
        {
            return _objInter.EditData(data);
        }

        [Route("deactiveY")]
        public Master_Subscription_DTO deactiveY([FromBody]Master_Subscription_DTO data)
        {
            return _objInter.deactiveY(data);
        }


    }
}
