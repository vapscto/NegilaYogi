using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using System.IO;
using System.Net;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeStaffOthersTransactionFacade : Controller
    {
        public FeeStaffOthersTransactionInterface _org;

        public FeeStaffOthersTransactionFacade(FeeStaffOthersTransactionInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public FeeStaffOthersTransactionDTO Getdet([FromBody] FeeStaffOthersTransactionDTO data)
        {
            return _org.getdata(data);
        }
        

        [HttpPost]
        [Route("feereceiptduplicate")]
        public FeeStaffOthersTransactionDTO duplicaterece([FromBody]FeeStaffOthersTransactionDTO data)
        {
            return _org.duplicaterecept(data);
        }

       
        [Route("get_grp_reptno")]
        public FeeStaffOthersTransactionDTO get_grp_reptno([FromBody] FeeStaffOthersTransactionDTO data)
        {
            return _org.get_grp_reptno(data);
        }
        
        [Route("edittransactionstaff")]
        public FeeStaffOthersTransactionDTO tranedit([FromBody] FeeStaffOthersTransactionDTO data)
        {
            return _org.edittra(data);
        }
        //for staff_others
        [Route("select_emp")]
        public FeeStaffOthersTransactionDTO select_emp([FromBody] FeeStaffOthersTransactionDTO data)
        {
            return _org.select_emp(data);
        }
        [Route("select_student")]
        public FeeStaffOthersTransactionDTO select_student([FromBody] FeeStaffOthersTransactionDTO data)
        {
            return _org.select_student(data);
        }
        [Route("getgroupmappedheadsnew_st")]
        public FeeStaffOthersTransactionDTO getgroupmappedheadsnew_st([FromBody]FeeStaffOthersTransactionDTO value)
        {
            return _org.getgroupmappedheadsnew_st(value);
        }
        [Route("savedata_st")]
        public FeeStaffOthersTransactionDTO savedata_st([FromBody]FeeStaffOthersTransactionDTO value)
        {
            return _org.savedata_st(value);
        }
        [Route("searching_s")]
        public FeeStaffOthersTransactionDTO searching_s([FromBody] FeeStaffOthersTransactionDTO data)
        {
            return _org.searching_s(data);
        }
        [Route("searching_o")]
        public FeeStaffOthersTransactionDTO searching_o([FromBody] FeeStaffOthersTransactionDTO data)
        {
            return _org.searching_o(data);
        }
        [Route("printreceipt_s")]
        public FeeStaffOthersTransactionDTO printreceipt_s([FromBody] FeeStaffOthersTransactionDTO data)
        {
            return _org.printreceipt_s(data);
        }
        [Route("printreceipt_o")]
        public FeeStaffOthersTransactionDTO printreceipt_o([FromBody] FeeStaffOthersTransactionDTO data)
        {
            return _org.printreceipt_o(data);
        }
        [Route("deletereceipt_s")]
        public FeeStaffOthersTransactionDTO deletereceipt_s([FromBody]FeeStaffOthersTransactionDTO data)
        {
            return _org.deletereceipt_s(data);
        }
        [Route("deletereceipt_o")]
        public FeeStaffOthersTransactionDTO deletereceipt_o([FromBody]FeeStaffOthersTransactionDTO data)
        {
            return _org.deletereceipt_o(data);
        }

        [Route("getacademicyear")]
        public FeeStaffOthersTransactionDTO getacademicyear([FromBody]FeeStaffOthersTransactionDTO data)
        {
            return _org.getacademicyear(data);
        }
    }
}
