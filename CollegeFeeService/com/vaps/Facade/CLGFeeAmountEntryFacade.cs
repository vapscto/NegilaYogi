using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using CollegeFeeService.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CLGFeeAmountEntryFacade : Controller
    {
        public CLGFeeAmountEntryInterfaces _org;

        public CLGFeeAmountEntryFacade(CLGFeeAmountEntryInterfaces orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public CLGFeeAmountEntryDTO Getdet([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.Getinitialformload(data);
        }

        [HttpPost]
        [Route("getcoursedetails")]
        public CLGFeeAmountEntryDTO getcourse([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.getcourdetails(data);
        }

        [HttpPost]
        [Route("getbranchdetails")]
        public CLGFeeAmountEntryDTO getbranch([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.getbranchdetails(data);
        }

        [HttpPost]
        [Route("getsemesterdetails")]
        public CLGFeeAmountEntryDTO getsemester([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.getsemesterdetails(data);
        }

        [HttpPost]
        [Route("getgroupmappedheads")]
        public CLGFeeAmountEntryDTO getgroupmappedhe([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.getgroupmapped(data);
        }

        [HttpPost]
        [Route("fillslabdetails")]
        public CLGFeeAmountEntryDTO fillslabdet([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.fillslabde(data);
        }

        //[HttpPost]
        //[Route("fillslabdetails")]
        //public CLGFeeAmountEntryDTO saveda([FromBody] CLGFeeAmountEntryDTO data)
        //{
        //    return _org.savedata(data);
        //}
        [HttpPost]
        [Route("savedata")]
        public CLGFeeAmountEntryDTO saveda([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.savedata(data);
        }

    }
}
