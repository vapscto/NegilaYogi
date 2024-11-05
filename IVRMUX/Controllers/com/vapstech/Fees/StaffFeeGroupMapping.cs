using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Fees;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StaffFeeGroupMapping : Controller
    {
        StaffFeeGroupMappingDelegate od = new StaffFeeGroupMappingDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeStudentGroupMappingDTO Get(int id)
        {
            FeeStudentGroupMappingDTO pgmodu = new FeeStudentGroupMappingDTO();
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getdata(pgmodu);
        }

        [Route("Editdetails/{id:int}")]
        public FeeStudentGroupMappingDTO EditDetails(int id)
        {
            return od.EditDetails(id);
        }

        [Route("getgroupmappedheads")]
        public FeeStudentGroupMappingDTO getstudentclass([FromBody] FeeStudentGroupMappingDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.ASMCL_Id = pgmodu.ASMCL_Id;
            return od.getstucls(pgmodu);
        }

        [HttpPost]
        public FeeStudentGroupMappingDTO savedata([FromBody] FeeStudentGroupMappingDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //pgmodu.MI_Id = 2;
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedetails(pgmodu);
        }
        [Route("saveeditdata")]
        public FeeStudentGroupMappingDTO saveeditdata([FromBody] FeeStudentGroupMappingDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //pgmodu.MI_Id = 2;
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.saveeditdata(pgmodu);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public FeeStudentGroupMappingDTO getradiodata([FromBody]FeeStudentGroupMappingDTO value)
        {
            value.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getradiofiltereddata(value);
        }


        [Route("studentsavedgroup")]
        public FeeStudentGroupMappingDTO studentsavedgroupfun([FromBody]FeeStudentGroupMappingDTO value)
        {
            value.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.studentsavedgroupcon(value);
        }


        [HttpPost]
        [Route("getclassoncatselect")]
        public FeeStudentGroupMappingDTO getdataaspercat([FromBody]FeeStudentGroupMappingDTO value)
        {

            value.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getdataaspercategory(value);
        }

        [HttpPost("{id}")]
        public FeeStudentGroupMappingDTO Put(int id, [FromBody]FeeStudentGroupMappingDTO value)
        {
            return od.getsearchdata(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }


        [Route("Deletedetails")]
        public FeeStudentGroupMappingDTO Delete([FromBody] FeeStudentGroupMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.deleterec(data);
        }

        [HttpPost]
        [Route("searching")]
        public FeeStudentGroupMappingDTO searching([FromBody] FeeStudentGroupMappingDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.user_id = UserId;
            return od.searching(categorypage);
        }

        [Route("searchingstudent")]
        public FeeStudentGroupMappingDTO searchingstu([FromBody] FeeStudentGroupMappingDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.user_id = UserId;
            return od.searchingstu(categorypage);
        }


        [Route("geteditdata")]
        public FeeStudentGroupMappingDTO studentdataedt([FromBody] FeeStudentGroupMappingDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.studentdataedit(pgmodu);
        }
    }
}
