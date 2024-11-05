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
    public class FeeRemittanceCertificateController : Controller
    {
        private static FacadeUrl _config;
        FeeRemittanceCertificateDelegate clsdel = new FeeRemittanceCertificateDelegate();
        private FacadeUrl fdu = new FacadeUrl();


        // GET: api/ClassCategoryReport
        [Route("getinitialfeedata")]
        public FeeRemittanceCertificateDTO getinitialfeedata()
        {


            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));



            return clsdel.getInitailData(mi_id);


        }



        [HttpPost]
        // POST: api/ClassCategoryReport
        [Route("searchdata")]
        public FeeRemittanceCertificateDTO saveData([FromBody] FeeRemittanceCertificateDTO studentdata)
        {
            try
            {

                studentdata.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                return clsdel.SearchData(studentdata);
            }
           
        
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }


        [Route("getadm_no_name")]
        public FeeRemittanceCertificateDTO getadm_no_name([FromBody] FeeRemittanceCertificateDTO studentdata)
        {
            try
            {

                studentdata.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                return clsdel.getAdm_Name(studentdata);
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
