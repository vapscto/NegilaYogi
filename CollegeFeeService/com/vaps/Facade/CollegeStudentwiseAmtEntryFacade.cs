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
    public class CollegeStudentwiseAmtEntryFacade : Controller
    {
        public CollegeStudentwiseAmtEntryInterfaces _org;

        public CollegeStudentwiseAmtEntryFacade(CollegeStudentwiseAmtEntryInterfaces orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public CLGFeeAmountEntryDTO Getdet([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.Getinitialformload(data);
        }

       
        [Route("selectacademicyear")]
        public CLGFeeAmountEntryDTO selectacademicyear([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.selectacademicyear(data);
        }

        [Route("deleterec")]
        public CLGFeeAmountEntryDTO deleterec([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.deleterec(data);
        }

        [Route("getbranchdetails")]
        public CLGFeeAmountEntryDTO getbranch([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.getbranchdetails(data);
        }

        
        [Route("getsemesterdetails")]
        public CLGFeeAmountEntryDTO getsemester([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.getsemesterdetails(data);
        }


        [Route("selectsem")]
        public CLGFeeAmountEntryDTO selectsem([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.selectsem(data);
        }
        [Route("getgroupmappedheads")]
        public CLGFeeAmountEntryDTO getgroupmappedhe([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.getgroupmapped(data);
        }

       
        [Route("fillslabdetails")]
        public CLGFeeAmountEntryDTO fillslabdet([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.fillslabde(data);
        }

       
        //[Route("fillslabdetails")]
        //public CLGFeeAmountEntryDTO saveda([FromBody] CLGFeeAmountEntryDTO data)
        //{
        //    return _org.savedata(data);
        //}
      
        [Route("savedata")]
        public CLGFeeAmountEntryDTO saveda([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.savedata(data);
        }

        [Route("getalldetailsOnselectiontype")]
        public CLGFeeAmountEntryDTO getalldetailsOnselectiontype([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.getalldetailsOnselectiontype(data);
        }

        // Holy-Cross 14-03-2024

        [Route("getmappedconcessionheads")]
        public CLGFeeAmountEntryDTO getmappedconcessionheads([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.getmappedconcessionheads(data);
        }

        [Route("savescholershpheaddata")]
        public CLGFeeAmountEntryDTO savescholershpheaddata([FromBody] CLGFeeAmountEntryDTO data)
        {
            return _org.savescholershpheaddata(data);
        }

    }
}
