using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class AdmissionStatusFacade : Controller
    {
        public AdmissionStatusInterface _acd;
        public AdmissionStatusFacade(AdmissionStatusInterface acdm)
        {
            _acd = acdm;
        }
        [HttpGet]
        public AdmissionStatusDTO Get(AdmissionStatusDTO enqo)
        {
            return _acd.getallDetails(enqo);
        }

        [Route("getdetails/{id:int}")]
        public AdmissionStatusDTO editdetails(int id)
        {
            return _acd.editdetail(id);
        }

        // POST api/values
        [HttpPost]
        public AdmissionStatusDTO savedata([FromBody] AdmissionStatusDTO data)
        {
            return _acd.savedataa(data);
        }

        // DELETE api/values/5
        [HttpPost]
        [Route("deletepages")]
        public AdmissionStatusDTO deletedata([FromBody] AdmissionStatusDTO dta)
        {
            return _acd.deletedata(dta);
        }
    }
}
