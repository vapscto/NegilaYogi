using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class StaffAndOtherConcessionFacade : Controller
    {
        public StaffAndOtherConcessionInterface _org;
        public StaffAndOtherConcessionFacade(StaffAndOtherConcessionInterface orga)
        {
            _org = orga;
        }

    
        [Route("getalldetails")]
        public CollegeConcessionDTO Getdet([FromBody] CollegeConcessionDTO data)
        {
            return _org.getdata(data);
        }

        [Route("onselectclassorcat")]
        public CollegeConcessionDTO selectclassorcat([FromBody] CollegeConcessionDTO data)
        {
            return _org.selectcatorclass(data);
        }

        [Route("fillhead")]
        public CollegeConcessionDTO fillheaddetails([FromBody] CollegeConcessionDTO data)
        {

            return _org.fillheaddetailsss(data);
        }


        [Route("fillamount")]
        public CollegeConcessionDTO fillamt([FromBody] CollegeConcessionDTO data)
        {

            return _org.fillamount(data);
        }


        [Route("savedata")]
        public CollegeConcessionDTO savedataa([FromBody] CollegeConcessionDTO data)
        {

            return _org.savedatadelegate(data);
        }

        [Route("deleteconcession")]
        public CollegeConcessionDTO deletecon([FromBody] CollegeConcessionDTO data)
        {
            return _org.deleteconcess(data);
        }

        [Route("EditconcessionDetails")]
        public CollegeConcessionDTO EditconcessionDetails([FromBody] CollegeConcessionDTO data)
        {
            return _org.EditconcessionDetails(data);
        }

        [Route("fillstaff")]
        public CollegeConcessionDTO fillst([FromBody] CollegeConcessionDTO data)
        {
            return _org.filstaff(data);
        }

        [Route("getacademicyear")]
        public CollegeConcessionDTO getacademicy([FromBody] CollegeConcessionDTO data)
        {
            return _org.getacademir(data);
        }


        [Route("checkpaiddetails")]
        public CollegeConcessionDTO checkpaiddetails([FromBody] CollegeConcessionDTO data)
        {
            return _org.checkpaiddetails(data);
        }
    }
}
