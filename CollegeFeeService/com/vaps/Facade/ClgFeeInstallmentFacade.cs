using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;
using CollegeFeeService.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{

    [Route("api/[controller]")]
    public class ClgFeeInstallmentFacade : Controller
    {
        //    // GET: api/values
        //    [HttpGet]
        //    public IEnumerable<string> Get()
        //    {
        //        return new string[] { "value1", "value2" };
        //    }

        //    // GET api/values/5
        //    [HttpGet("{id}")]
        //    public string Get(int id)
        //    {
        //        return "value";
        //    }

        //    // POST api/values
        //    [HttpPost]
        //    public void Post([FromBody]string value)
        //    {
        //    }

        //    // PUT api/values/5
        //    [HttpPut("{id}")]
        //    public void Put(int id, [FromBody]string value)
        //    {
        //    }

        //    // DELETE api/values/5
        //    [HttpDelete("{id}")]
        //    public void Delete(int id)
        //    {
        //    }

        public ClgFeeInstallmentInterface _feegrouppage;

        public ClgFeeInstallmentFacade(ClgFeeInstallmentInterface maspag)
        {
            _feegrouppage = maspag;
        }


        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public Clg_Fee_Installment_DTO getpagedetails(int id)
        {
            // id = 12;
            return _feegrouppage.getpageedit(id);
        }
        [Route("getpagedetailsY/{id:int}")]
        //[Route("getenquirycontroller")]
        public Clg_Fee_Installment_Due_Date_DTO getpagedetailsY(int id)
        {
            // id = 12;
            return _feegrouppage.getpageeditY(id);
        }

        // GET: api/values
        [HttpGet]
        public Clg_Fee_Installment_DTO Get(Clg_Fee_Installment_DTO mas)
        {
            return _feegrouppage.GetGroupSearchData(mas);
        }

        [HttpPost]
        // GET api/values/5
        [Route("getdetails")]
        public Clg_Fee_Installment_DTO getorgdet([FromBody] Clg_Fee_Installment_DTO mas)
        {
            return _feegrouppage.getdetails(mas);
        }

        // POST api/values
        [HttpPost]
        public Clg_Fee_Installment_DTO Post([FromBody] Clg_Fee_Installment_DTO org)
        {
            return _feegrouppage.SaveGroupData(org);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] Clg_Fee_Installment_DTO value)
        {
            return "success";
        }


        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public Clg_Fee_Installment_DTO Deleterec(int id)
        {
            return _feegrouppage.deleterec(id);
        }
        [Route("deletedetailsY/{id:int}")]
        public Clg_Fee_Installment_Due_Date_DTO DeleterecY(int id)
        {
            return _feegrouppage.deleterecY(id);
        }


        [HttpPost]
        [Route("deactivate")]
        public Clg_Fee_Installment_DTO deactivateAcdmYear([FromBody] Clg_Fee_Installment_DTO id)
        {
            // id = 12;
            return _feegrouppage.deactivate(id);
        }

        [Route("GetWrittenTestMarks/")]
        public Clg_Fee_Installments_Yearly_DTO[] GetWrittenTestMarks([FromBody] Clg_Fee_Installments_Yearly_DTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _feegrouppage.GetWrittenTestMarks(masterMDT);
        }
        [Route("yearsbind")]
        public Task<Clg_Fee_Installment_DTO> Gets([FromBody] Clg_Fee_Installment_DTO enqo)
        {
            return _feegrouppage.getIndependentDropDowns(enqo);
        }
        [Route("Getduedates/")]
        public Clg_Fee_Installments_Yearly_DTO[] Getduedates([FromBody] Clg_Fee_Installments_Yearly_DTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _feegrouppage.Getduedates(masterMDT);
        }

        // POST api/values
        [HttpPost]
        [Route("savedetailDDD/")]
        public Clg_Fee_Installment_DTO savedetailDDD([FromBody] Clg_Fee_Installment_DTO org)
        {
            return _feegrouppage.savedetailDDD(org);
        }

    }
}

