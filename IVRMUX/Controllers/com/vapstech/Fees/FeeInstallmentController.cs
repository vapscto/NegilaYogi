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
    public class FeeInstallmentController : Controller
    {
        FeeInstallmentDelegate FID = new FeeInstallmentDelegate();
        // FID = new FeeInstallmentDelegate();
        // GET: api/values
        [HttpGet]

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeInstallmentDTO Get([FromQuery] int id)
        {
            FeeInstallmentDTO data = new FeeInstallmentDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return FID.getdetails(data);
        }

        //for edit
        [Route("getdetails/{id:int}")]
        public FeeInstallmentDTO getdetail(int id)
        {
          //  HttpContext.Session.SetString("pageid", id.ToString()); //Set
           // id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // id = 12;
            return FID.getpagedetails(id);
        }
        [Route("getdetailsY/{id:int}")]
        public FeeInstalmentDueDateDTO getdetailY(int id)
        {
            return FID.getpagedetailsY(id);

        }

        [Route("Editdetails/{id:int}")]
        public FeeInstallmentDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return FID.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        public FeeInstallmentDTO savedetail([FromBody] FeeInstallmentDTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FID.savedetails(Grouppage);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public FeeInstallmentDTO Delete(int id)
        {
            return FID.deleterec(id);
        }
        //[Route("deletepagesY/{id:int}")]
        //public FeeInstalmentDueDateDTO DeleteY(int id)
        //{
        //    return FID.deleterecY(id);
        //}
        [Route("deletepagesY")]
        public FeeInstalmentDueDateDTO DeleteY([FromBody] FeeInstalmentDueDateDTO data)
        {           
            return FID.deleterecY(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public FeeInstallmentDTO deactvate([FromBody] FeeInstallmentDTO id)
        {
            return FID.deactivateAcademicYear(id);
        }      
        [Route("GetWrittenTestMarks/")]
        public FeeInstallmentyeralyDTO[] GetWrittenTestMarks([FromBody] FeeInstallmentyeralyDTO MMD)
        {
            return FID.GetWrittenTestMarks(MMD);
        }

        [Route("getdpforyear")]
        public FeeInstallmentDTO getDpData([FromBody] FeeInstallmentDTO yrs)
        {
            //return sad.getIndependentDropDowns(ctry);
            return FID.getIndependentDropDowns(yrs);
        }
        
        [Route("Getduedates/")]
        public FeeInstallmentyeralyDTO[] Getduedates([FromBody] FeeInstallmentyeralyDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FID.Getduedates(MMD);
        }
        // POST api/values
        [HttpPost]
        [Route("savedetailDDD/")]
        public FeeInstallmentDTO savedetailDDD([FromBody] FeeInstallmentDTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FID.savedetailDDD(Grouppage);
        }
      

    }
}
