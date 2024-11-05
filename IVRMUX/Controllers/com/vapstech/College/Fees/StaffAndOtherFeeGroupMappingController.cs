using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using corewebapi18072016.Delegates;
using IVRMUX.Delegates.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;

namespace IVRMUX.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class StaffAndOtherFeeGroupMappingController : Controller
    {
        StaffAndOtherFeeGroupMappingDelegate od = new StaffAndOtherFeeGroupMappingDelegate();
       
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public Clg_StudentFeeGroupMapping_DTO Get(int id)
        {
            Clg_StudentFeeGroupMapping_DTO pgmodu = new Clg_StudentFeeGroupMapping_DTO();
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getdata(pgmodu);
        }

        [Route("Editdetails/{id:int}")]
        public Clg_StudentFeeGroupMapping_DTO EditDetails(int id)
        {
            return od.EditDetails(id);
        }

        [Route("getgroupmappedheads")]
        public Clg_StudentFeeGroupMapping_DTO getstudentclass([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
      
           // pgmodu.ASMCL_Id = pgmodu.ASMCL_Id;
            return od.getstucls(pgmodu);
        }

        [Route("fillstudentsroute")]
        public Clg_StudentFeeGroupMapping_DTO fillstudentsroute([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.fillstudentsroute(pgmodu);
        }


        [HttpPost]
        public Clg_StudentFeeGroupMapping_DTO savedata([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {
            
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedetails(pgmodu);
        }
        [Route("savedata_s")]
        public Clg_StudentFeeGroupMapping_DTO savedata_s([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {

            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
         
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedetails_s(pgmodu);
        }
        [Route("savedata_o")]
        public Clg_StudentFeeGroupMapping_DTO savedata_o([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {

            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedetails_o(pgmodu);
        }
        [Route("saveeditdata")]
        public Clg_StudentFeeGroupMapping_DTO saveeditdata([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {

            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           

            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.saveeditdata(pgmodu);
        }
        [HttpPost]
        [Route("radiobtndata")]
        public Clg_StudentFeeGroupMapping_DTO getradiodata([FromBody]Clg_StudentFeeGroupMapping_DTO value)
        {
            value.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
      
            value.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getradiofiltereddata(value);
        }


        [Route("studentsavedgroup")]
        public Clg_StudentFeeGroupMapping_DTO studentsavedgroupfun([FromBody]Clg_StudentFeeGroupMapping_DTO value)
        {
            value.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return od.studentsavedgroupcon(value);
        }


        [HttpPost]
        [Route("getclassoncatselect")]
        public Clg_StudentFeeGroupMapping_DTO getdataaspercat([FromBody]Clg_StudentFeeGroupMapping_DTO value)
        {

            value.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return od.getdataaspercategory(value);
        }

        [HttpPost("{id}")]
        public Clg_StudentFeeGroupMapping_DTO Put(int id, [FromBody]Clg_StudentFeeGroupMapping_DTO value)
        {
            return od.getsearchdata(id, value);
        }
        
        
        [Route("Deletedetails")]
        public Clg_StudentFeeGroupMapping_DTO Delete([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return od.deleterec(data);
        }
        [Route("Deletedetails_s")]
        public Clg_StudentFeeGroupMapping_DTO Delete_s([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return od.deleterec_s(data);
        }
        [Route("Deletedetails_o")]
        public Clg_StudentFeeGroupMapping_DTO Delete_o([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return od.deleterec_o(data);
        }

        [HttpPost]
        [Route("searching")]
        public Clg_StudentFeeGroupMapping_DTO searching([FromBody] Clg_StudentFeeGroupMapping_DTO categorypage)
        {
          
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.user_id = UserId;
            return od.searching(categorypage);
        }
        [Route("searching_s")]
        public Clg_StudentFeeGroupMapping_DTO searching_s([FromBody] Clg_StudentFeeGroupMapping_DTO categorypage)
        {
           
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.user_id = UserId;
            return od.searching_s(categorypage);
        }
        [Route("searching_o")]
        public Clg_StudentFeeGroupMapping_DTO searching_o([FromBody] Clg_StudentFeeGroupMapping_DTO categorypage)
        {
           
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.user_id = UserId;
            return od.searching_o(categorypage);
        }

        [Route("searchingstudent")]
        public Clg_StudentFeeGroupMapping_DTO searchingstu([FromBody] Clg_StudentFeeGroupMapping_DTO categorypage)
        {
           
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.user_id = UserId;
            return od.searchingstu(categorypage);
        }
        [Route("searchingstaff")]
        public Clg_StudentFeeGroupMapping_DTO searchingstaff([FromBody] Clg_StudentFeeGroupMapping_DTO categorypage)
        {
           
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.user_id = UserId;
            return od.searchingstaff(categorypage);
        }
        [Route("searchingothers")]
        public Clg_StudentFeeGroupMapping_DTO searchingothers([FromBody] Clg_StudentFeeGroupMapping_DTO categorypage)
        {
       
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.user_id = UserId;
            return od.searchingothers(categorypage);
        }

        [Route("geteditdata")]
        public Clg_StudentFeeGroupMapping_DTO studentdataedt([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return od.studentdataedit(pgmodu);
        }

        [Route("getacademicyear")]
        public Clg_StudentFeeGroupMapping_DTO getacademicye([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return od.getacademicye(pgmodu);
        }

        [Route("geteditdatastaffothers")]
        public Clg_StudentFeeGroupMapping_DTO geteditdatastaffothers([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return od.geteditdatastaffothers(pgmodu);
        }



        [Route("saveeditdatastaff")]
        public Clg_StudentFeeGroupMapping_DTO saveeditdatastaff([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.saveeditdatastaff(pgmodu);
        }


        [Route("saveeditdataothers")]
        public Clg_StudentFeeGroupMapping_DTO saveeditdataothers([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.saveeditdataothers(pgmodu);
        }

    }
}
