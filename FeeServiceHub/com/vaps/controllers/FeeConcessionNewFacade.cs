using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeConcessionNewFacade : Controller
    {
        public FeeConcessionNewInterface _org;
        public FeeConcessionNewFacade(FeeConcessionNewInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public FeeConcessionDTO Getdet([FromBody] FeeConcessionDTO data)
        {
            return _org.getdata(data);
        }

        //[Route("onselectclassorcat")]
        //public FeeConcessionDTO selectclassorcat([FromBody] FeeConcessionDTO data)
        //{
        //    return _org.selectcatorclass(data);
        //}

        //[Route("fillhead")]
        //public FeeConcessionDTO fillheaddetails([FromBody] FeeConcessionDTO data)
        //{

        //    return _org.fillheaddetailsss(data);
        //}


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

        //[Route("fillstaff")]
        //public FeeConcessionDTO fillst([FromBody] FeeConcessionDTO data)
        //{
        //    return _org.filstaff(data);
        //}

        //[Route("getacademicyear")]
        //public FeeConcessionDTO getacademicy([FromBody] FeeConcessionDTO data)
        //{
        //    return _org.getacademir(data);
        //}
        [Route("getfeegroup")]
        public FeeConcessionDTO getfeegroup([FromBody] FeeConcessionDTO data)
        {
            return _org.getfeegroup(data);
        }
        [Route("getfeehead")]
        public FeeConcessionDTO getfeeterm([FromBody] FeeConcessionDTO data)
        {
            return _org.getfeehead(data);
        }
        [Route("getterm")]
        public FeeConcessionDTO getterm([FromBody] FeeConcessionDTO data)
        {
            return _org.getterm(data);
        }
        [Route("getinstallment")]
        public FeeConcessionDTO getinstallment([FromBody] FeeConcessionDTO data)
        {
            return _org.getinstallment(data);
        }
        [Route("studentdata")]
        public FeeConcessionDTO studentdata([FromBody] FeeConcessionDTO data)
        {
            return _org.studentdata(data);
        }
        [Route("studentdata1")]
        public FeeConcessionDTO studentdata1([FromBody] FeeConcessionDTO data)
        {
            return _org.studentdata1(data);
        }
        [Route("Save")]
        public FeeConcessionDTO Save([FromBody] FeeConcessionDTO data)
        {
            return _org.Save(data);
        }
    }
}
