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
    public class MasterTemplateFacade : Controller
    {
        public masterTemplateInterface _templt;

        public MasterTemplateFacade(masterTemplateInterface templt)
        {
            _templt = templt;
        }
        [HttpGet]
        public MasterTemplateDTO Get(MasterTemplateDTO teml)
        {
            return _templt.getAllDetails(teml);
        }

        [Route("getdetails/{id:int}")]
        public MasterTemplateDTO gettmpltdet(int id)
        {
            return _templt.getdetails(id);
        }

        [Route("getSaletypes/{id:int}")]
        public MasterTemplateDTO getSaletypes(int id)
        {
            return _templt.getSaletypes(id);
        }


        [HttpPost]
        public MasterTemplateDTO Post([FromBody]MasterTemplateDTO templtdata)
        {
            return _templt.saveTempldet(templtdata);
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterTemplateDTO Deleterec(int id)
        {
            return _templt.deleterec(id);
        }

        [Route("satregistration")]
        public SatRegistrationDTO satregistration([FromBody] SatRegistrationDTO dta)
        {
            return _templt.satregistration(dta);
        }
    }
}
