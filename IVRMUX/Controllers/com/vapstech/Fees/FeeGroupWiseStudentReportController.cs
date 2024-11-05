using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeGroupWiseStudentReportController : Controller
    {
        private static FacadeUrl _config;
        FeeGroupWiseStudentReportDelegate clsdel = new FeeGroupWiseStudentReportDelegate();
        private FacadeUrl fdu = new FacadeUrl();


        // GET: api/ClassCategoryReport
        [Route("getinitialfeedata/{id:int}")]
        public FeeGroupWiseStudentReportDTO getinitialfeedata(int id)
        {

            FeeGroupWiseStudentReportDTO data = new FeeGroupWiseStudentReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clsdel.getInitailData(data);


        }

      
        [Route("Getclass")]
        public FeeGroupWiseStudentReportDTO Getclass([FromBody] FeeGroupWiseStudentReportDTO studentdata)
        {
            studentdata.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            studentdata.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clsdel.Getclass(studentdata);
        }

        [Route("GetSection")]
        public FeeGroupWiseStudentReportDTO GetSection([FromBody] FeeGroupWiseStudentReportDTO studentdata)
        {
            studentdata.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clsdel.GetSection(studentdata);
        }
        [Route("GetStudent")]
        public FeeGroupWiseStudentReportDTO GetStudent([FromBody] FeeGroupWiseStudentReportDTO studentdata)
        {
            studentdata.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clsdel.GetStudent(studentdata);
        }

        
        [HttpPost]
        // POST: api/ClassCategoryReport

        public FeeGroupWiseStudentReportDTO saveData([FromBody] FeeGroupWiseStudentReportDTO studentdata)
        {
            try
            {

                studentdata.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                studentdata.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                return clsdel.SearchData(studentdata);
            }
           
        
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        // PUT: api/ClassCategoryReport/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
