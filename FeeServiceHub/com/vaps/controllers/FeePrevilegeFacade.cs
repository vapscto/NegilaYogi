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
    public class FeePrevilegeFacade : Controller
    {
        public FeePrevilegeInterface _feegrouppage;

        public FeePrevilegeFacade(FeePrevilegeInterface maspag)
        {
            _feegrouppage = maspag;
        }


        [Route("getpagedetails")]

        public FeePrevilegeDTO getpagedetails([FromBody] FeePrevilegeDTO org)
        {
            // id = 12;
            return _feegrouppage.getdetails(org);

        }

        [Route("getusername")]
        public FeePrevilegeDTO getusername([FromBody] FeePrevilegeDTO data)
        {
            return _feegrouppage.getusername(data);
        }

        [Route("delete/{id:int}")]
        public FeePrevilegeDTO delete(int id)
        {
            return _feegrouppage.delete(id);
        }

        [Route("savedetail")]
        public FeePrevilegeDTO Post([FromBody] FeePrevilegeDTO org)
        {
            return _feegrouppage.savedetail(org);
        }

        [Route("edit/{id:int}")]
        public FeePrevilegeDTO edit(int id)
        {
            return _feegrouppage.edit(id);
        }


        [Route("fillhead")]
        public FeePrevilegeDTO fillheadss([FromBody] FeePrevilegeDTO org)
        {
            // id = 12;
            return _feegrouppage.fillheadsinterface(org);

        }

        //[HttpPost]
        //// GET api/values/5
        //[Route("getdetails")]
        //public FeeInstallmentDTO getorgdet([FromBody] FeeInstallmentDTO mas)
        //{
        //    return _feegrouppage.getdetails(mas);
        //}

        //// POST api/values
        //[HttpPost]
        //public FeeInstallmentDTO Post([FromBody] FeeInstallmentDTO org)
        //{
        //    return _feegrouppage.SaveGroupData(org);
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public string Put(int id, [FromBody] FeeInstallmentDTO value)
        //{
        //    return "success";
        //}


        //[HttpDelete]
        //[Route("deletedetails/{id:int}")]
        //public FeeInstallmentDTO Deleterec(int id)
        //{
        //    return _feegrouppage.deleterec(id);
        //}
        //[Route("deletedetailsY/{id:int}")]
        //public FeeInstalmentDueDateDTO DeleterecY(int id)
        //{
        //    return _feegrouppage.deleterecY(id);
        //}


        //[HttpPost]
        //[Route("deactivate")]
        //public FeeInstallmentDTO deactivateAcdmYear([FromBody] FeeInstallmentDTO id)
        //{
        //    // id = 12;
        //    return _feegrouppage.deactivate(id);
        //}

        //[Route("GetWrittenTestMarks/")]
        //public FeeInstallmentyeralyDTO[] GetWrittenTestMarks([FromBody] FeeInstallmentyeralyDTO masterMDT)
        //{
        //    // return _reg.getregdata(reg);
        //    return _feegrouppage.GetWrittenTestMarks(masterMDT);
        //}
        //[Route("yearsbind")]
        //public Task<FeeInstallmentDTO> Gets([FromBody] FeeInstallmentDTO enqo)
        //{
        //    return _feegrouppage.getIndependentDropDowns(enqo);
        //}
        //[Route("Getduedates/")]
        //public FeeInstallmentyeralyDTO[] Getduedates([FromBody] FeeInstallmentyeralyDTO masterMDT)
        //{
        //    // return _reg.getregdata(reg);
        //    return _feegrouppage.Getduedates(masterMDT);
        //}

        //// POST api/values
        //[HttpPost]
        //[Route("savedetailDDD/")]
        //public FeeInstallmentDTO savedetailDDD([FromBody] FeeInstallmentDTO org)
        //{
        //    return _feegrouppage.savedetailDDD(org);
        //}

    }
}
