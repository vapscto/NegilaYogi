using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseDonateReportFacade : Controller
    {

        public PurchaseDonateReportInterface _objInter;
        public PurchaseDonateReportFacade(PurchaseDonateReportInterface para)
        {
            _objInter = para;
        }
       
      
        [Route("getdata")]
        public Task<CirculationParameterDTO> getdata([FromBody] CirculationParameterDTO data)
        {
            return _objInter.getdata(data);
        }


    }
}
