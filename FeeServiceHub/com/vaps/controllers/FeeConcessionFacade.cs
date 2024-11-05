using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeConcessionFacade : Controller
    {
        public FeeConcessionInterface _org;
        public FeeConcessionFacade(FeeConcessionInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public FeeConcessionDTO Getdet([FromBody] FeeConcessionDTO data)
        {
            return _org.getdata(data);
        }

        [Route("onselectclassorcat")]
        public FeeConcessionDTO selectclassorcat([FromBody] FeeConcessionDTO data)
        {
            return _org.selectcatorclass(data);
        }

        [Route("fillhead")]
        public FeeConcessionDTO fillheaddetails([FromBody] FeeConcessionDTO data)
        {
         
            return _org.fillheaddetailsss(data);
        }


        [Route("fillamount")]
        public FeeConcessionDTO fillamt([FromBody] FeeConcessionDTO data)
        {
           
            return _org.fillamount(data);
        }


        [Route("savedata")]
        public FeeConcessionDTO savedataa([FromBody] FeeConcessionDTO data)
        {
          
            return _org.savedatadelegate(data);
        }

        [Route("deleteconcession")]
        public FeeConcessionDTO deletecon([FromBody] FeeConcessionDTO data)
        {
            return _org.deleteconcess(data);
        }

        [Route("EditconcessionDetails")]
        public FeeConcessionDTO EditconcessionDetails([FromBody] FeeConcessionDTO data)
        {
            return _org.EditconcessionDetails(data);
        }

        [Route("fillstaff")]
        public FeeConcessionDTO fillst([FromBody] FeeConcessionDTO data)
        {
            return _org.filstaff(data);
        }

        [Route("getacademicyear")]
        public FeeConcessionDTO getacademicy([FromBody] FeeConcessionDTO data)
        {
            return _org.getacademir(data);
        }


        [Route("checkpaiddetails")]
        public FeeConcessionDTO checkpaiddetails([FromBody] FeeConcessionDTO data)
        {
            return _org.checkpaiddetails(data);
        }

        [Route("searchfilter")]
        public FeeConcessionDTO searchfilter([FromBody] FeeConcessionDTO data)
        {

            return _org.searchfilter(data);
        }
    }
}
