using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegeStaffAndOtherTransactionFacade : Controller
    {
        public CollegeStaffAndOtherTransactionInterface _org;

        public CollegeStaffAndOtherTransactionFacade(CollegeStaffAndOtherTransactionInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public CollegeStaffAndOtherTransactionDTO Getdet([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.getdata(data);
        }


        [HttpPost]
        [Route("feereceiptduplicate")]
        public CollegeStaffAndOtherTransactionDTO duplicaterece([FromBody]CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.duplicaterecept(data);
        }


        [Route("get_grp_reptno")]
        public CollegeStaffAndOtherTransactionDTO get_grp_reptno([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.get_grp_reptno(data);
        }

        [Route("edittransactionstaff")]
        public CollegeStaffAndOtherTransactionDTO tranedit([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.edittra(data);
        }
        //for staff_others
        [Route("select_emp")]
        public CollegeStaffAndOtherTransactionDTO select_emp([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.select_emp(data);
        }
        [Route("select_student")]
        public CollegeStaffAndOtherTransactionDTO select_student([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.select_student(data);
        }
        [Route("getgroupmappedheadsnew_st")]
        public CollegeStaffAndOtherTransactionDTO getgroupmappedheadsnew_st([FromBody]CollegeStaffAndOtherTransactionDTO value)
        {
            return _org.getgroupmappedheadsnew_st(value);
        }
        [Route("savedata_st")]
        public CollegeStaffAndOtherTransactionDTO savedata_st([FromBody]CollegeStaffAndOtherTransactionDTO value)
        {
            return _org.savedata_st(value);
        }
        [Route("searching_s")]
        public CollegeStaffAndOtherTransactionDTO searching_s([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.searching_s(data);
        }
        [Route("searching_o")]
        public CollegeStaffAndOtherTransactionDTO searching_o([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.searching_o(data);
        }
        [Route("printreceipt_s")]
        public CollegeStaffAndOtherTransactionDTO printreceipt_s([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.printreceipt_s(data);
        }
        [Route("printreceipt_o")]
        public CollegeStaffAndOtherTransactionDTO printreceipt_o([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.printreceipt_o(data);
        }
        [Route("deletereceipt_s")]
        public CollegeStaffAndOtherTransactionDTO deletereceipt_s([FromBody]CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.deletereceipt_s(data);
        }
        [Route("deletereceipt_o")]
        public CollegeStaffAndOtherTransactionDTO deletereceipt_o([FromBody]CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.deletereceipt_o(data);
        }

        [Route("getacademicyear")]
        public CollegeStaffAndOtherTransactionDTO getacademicyear([FromBody]CollegeStaffAndOtherTransactionDTO data)
        {
            return _org.getacademicyear(data);
        }
    }
}
