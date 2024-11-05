using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeInstallmentFacade : Controller
    {
        public FeeInstallmentInterface _feegrouppage;

        public FeeInstallmentFacade(FeeInstallmentInterface maspag)
        {
            _feegrouppage = maspag;
        }


        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeInstallmentDTO getpagedetails(int id)
        {
            // id = 12;
            return _feegrouppage.getpageedit(id);
        }
        [Route("getpagedetailsY/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeInstalmentDueDateDTO getpagedetailsY(int id)
        {
            // id = 12;
            return _feegrouppage.getpageeditY(id);
        }

        // GET: api/values
        [HttpGet]
        public FeeInstallmentDTO Get(FeeInstallmentDTO mas)
        {
            return _feegrouppage.GetGroupSearchData(mas);
        }

        [HttpPost]
        // GET api/values/5
        [Route("getdetails")]
        public FeeInstallmentDTO getorgdet([FromBody] FeeInstallmentDTO mas)
        {
            return _feegrouppage.getdetails(mas);
        }

        // POST api/values
        [HttpPost]
        public FeeInstallmentDTO Post([FromBody] FeeInstallmentDTO org)
        {
            return _feegrouppage.SaveGroupData(org);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] FeeInstallmentDTO value)
        {
            return "success";
        }


        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public FeeInstallmentDTO Deleterec(int id)
        {
            return _feegrouppage.deleterec(id);
        }
        //[Route("deletedetailsY/{id:int}")]
        //public FeeInstalmentDueDateDTO DeleterecY(int id)
        //{
        //    return _feegrouppage.deleterecY(id);
        //}
        [Route("deletedetailsY")]
        public FeeInstalmentDueDateDTO DeleterecY([FromBody] FeeInstalmentDueDateDTO data)
        {
            return _feegrouppage.deleterecY(data);
        }

        [HttpPost]
        [Route("deactivate")]
        public FeeInstallmentDTO deactivateAcdmYear([FromBody] FeeInstallmentDTO id)
        {
            // id = 12;
            return _feegrouppage.deactivate(id);
        }

        [Route("GetWrittenTestMarks/")]
        public FeeInstallmentyeralyDTO[] GetWrittenTestMarks([FromBody] FeeInstallmentyeralyDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _feegrouppage.GetWrittenTestMarks(masterMDT);
        }
        [Route("yearsbind")]
        public Task<FeeInstallmentDTO> Gets([FromBody] FeeInstallmentDTO enqo)
        {
            return _feegrouppage.getIndependentDropDowns(enqo);
        }
        [Route("Getduedates/")]
        public FeeInstallmentyeralyDTO[] Getduedates([FromBody] FeeInstallmentyeralyDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _feegrouppage.Getduedates(masterMDT);
        }

        // POST api/values
        [HttpPost]
        [Route("savedetailDDD/")]
        public FeeInstallmentDTO savedetailDDD([FromBody] FeeInstallmentDTO org)
        {
            return _feegrouppage.savedetailDDD(org);
        }
      
    }
}
