using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Masters;
using PreadmissionDTOs.com.vaps.College.Fee;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees.Masters
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgMasterFeeInstallmentController : Controller
    {
        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        ClgFeeInstallmentDelegate FID = new ClgFeeInstallmentDelegate();
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
        public Clg_Fee_Installment_DTO Get([FromQuery] int id)
        {
            Clg_Fee_Installment_DTO data = new Clg_Fee_Installment_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return FID.getdetails(data);
        }

        //for edit
        [Route("getdetails/{id:int}")]
        public Clg_Fee_Installment_DTO getdetail(int id)
        {
            //  HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // id = 12;
            return FID.getpagedetails(id);
        }
        [Route("getdetailsY/{id:int}")]
        public Clg_Fee_Installment_Due_Date_DTO getdetailY(int id)
        {
            return FID.getpagedetailsY(id);

        }

        [Route("Editdetails/{id:int}")]
        public Clg_Fee_Installment_DTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return FID.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        public Clg_Fee_Installment_DTO savedetail([FromBody] Clg_Fee_Installment_DTO Grouppage)
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
        public Clg_Fee_Installment_DTO Delete(int id)
        {
            return FID.deleterec(id);
        }
        [Route("deletepagesY/{id:int}")]
        public Clg_Fee_Installment_Due_Date_DTO DeleteY(int id)
        {
            return FID.deleterecY(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public Clg_Fee_Installment_DTO deactvate([FromBody] Clg_Fee_Installment_DTO id)
        {
            return FID.deactivateAcademicYear(id);
        }


        [Route("GetWrittenTestMarks/")]
        public Clg_Fee_Installments_Yearly_DTO[] GetWrittenTestMarks([FromBody] Clg_Fee_Installments_Yearly_DTO MMD)
        {
            return FID.GetWrittenTestMarks(MMD);
        }

        [Route("getdpforyear")]
        public Clg_Fee_Installment_DTO getDpData([FromBody] Clg_Fee_Installment_DTO yrs)
        {
            //return sad.getIndependentDropDowns(ctry);
            return FID.getIndependentDropDowns(yrs);
        }

        [Route("Getduedates/")]
        public Clg_Fee_Installments_Yearly_DTO[] Getduedates([FromBody] Clg_Fee_Installments_Yearly_DTO MMD)
        {
            MMD.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FID.Getduedates(MMD);
        }
        // POST api/values
        [HttpPost]
        [Route("savedetailDDD/")]
        public Clg_Fee_Installment_DTO savedetailDDD([FromBody] Clg_Fee_Installment_DTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FID.savedetailDDD(Grouppage);
        }

    }
}
