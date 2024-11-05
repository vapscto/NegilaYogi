using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;
using System.IO;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ProspectusController : Controller
    {
        ProspectusDelegate od = new ProspectusDelegate();
        PaymentDetails od1 = new PaymentDetails();
        //public IEnumerable<string> Get()
        //{


        //    return new string[] { "value1", "value2" };
        //}


        [HttpPost]
        [Route("getalldetails")]
        public ProspectusDTO Get([FromBody] ProspectusDTO data)
        {
            //ProspectusDTO data = new ProspectusDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.roleId = roleidd;

            return od.getcountrydata(data);
        }
        [HttpGet]
        [Route("getorganisationcontroller/{id:int}")]
        public StateDTO Getstates(int id)
        {
            return od.enqdatacountrydrp(id);
        }

        [Route("getorganisationstatecontroller/{id:int}")]
        public CityDTO getcity(int id)
        {
            return od.cityfill(id);
        }

        [Route("getdetails/{id:int}")]
        public ProspectusDTO getdetail(int id)
        {
            return od.prospdet(id);
        }
        [HttpPost]
        [Route("getEnquiry")]
        public Enq getEnquiry([FromBody] searchEnquiryDTO  srch)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            srch.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            srch.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            srch.ASMAY_Id = ASMAY_Id;

            return od.enqDetails(srch);
        }
      
        
        // POST api/values
        [HttpPost]
        public ProspectusDTO savedetail([FromBody] ProspectusDTO org)
        {
            
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            org.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            org.id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            org.ASMAY_Id = ASMAY_Id;

            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            org.roleId = roleidd;

            return od.savedetails(org);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        //DELETE api/values/5
        [HttpPost]
        [Route("deletedetails")]
        public ProspectusDTO Delete([FromBody] ProspectusDTO dta)
        {
            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dta.roleId = roleidd;

            return od.deleterec(dta);
        }
        [Route("SearchByColumn")]
        public ProspectusDTO searchByColumn([FromBody] ProspectusDTO data)
        {
            return od.searchByColumn(data);
        }


        [Route("download/{id:int}")]
        public IActionResult DownloadFile(int id)
        {
            ProspectusDTO dto = new ProspectusDTO();
            Stream stream = null;
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto = od.downloadProspectus(id);
            if (dto.prospectusfilePath != null)
            {
                stream = new FileStream(dto.prospectusfilePath, FileMode.Open);
            }
            // var filepath = Path.Combine(_hostingEnvironment.WebRootPath, "EmailTemplate", "Prospectors.pdf");
            //Response.ContentType = "application/pdf";
            //Response.Headers.Add("content-disposition","inline; filename=myPDF.pdf");
            return File(stream, "application/pdf", "Prospectors.pdf");
        }

        [Route("paymentresponse/")]
        public ActionResult paymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            string querystring = "";
            try
            {
                dto = od.getpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/prospectus/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/prospectus/13?status=Networkfailure";
                }
            }
            catch(Exception e)
            {
              //  dto.returnvalue = "";
            }

            return Redirect(querystring);
        }


    }
}
