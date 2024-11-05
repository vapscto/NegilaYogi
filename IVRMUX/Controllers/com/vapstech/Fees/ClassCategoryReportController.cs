using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using Microsoft.Extensions.Options;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{

    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class ClassCategoryReportController : Controller
    {

        private static FacadeUrl _config;
        ClassCategoryReportDelegate clsdel = new ClassCategoryReportDelegate();
        private FacadeUrl fdu = new FacadeUrl();


        // GET: api/ClassCategoryReport
        [HttpGet]
        [Route("getinitialdata/{id:int}")]
        public ClassCategoryReportDTO getInitialData()
        {
          
            
                int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            

            return clsdel.getInitailData(mi_id);
          
           
        }


      
        [HttpPost]
        // POST: api/ClassCategoryReport
        
        public ClassCategoryReportDTO saveData([FromBody] ClassCategoryReportDTO studentdata)
        {
           
            studentdata.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return clsdel.SearchData(studentdata);
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
