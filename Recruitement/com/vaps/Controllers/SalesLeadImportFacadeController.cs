using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SalesLeadImportFacadeController : Controller
    {
        public SalesLeadImportInterface _sali;


        public SalesLeadImportFacadeController(SalesLeadImportInterface lm)
        {
            _sali = lm;
        }
    
        [Route("saveadvance")]
        public SalesLeadImportDTO saveadvance([FromBody] SalesLeadImportDTO dTO)
        {
            return _sali.saveadvance(dTO);
        }

    }
}