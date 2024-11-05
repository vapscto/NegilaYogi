
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Chairman.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
//using AdmissionServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Chairman.Controllers
{
    [Route("api/[controller]")]
    public class InstituteWiseFeeCollectionFacade : Controller
    {
        public InstituteWiseFeeCollectionInterface _ChairmanDashboardReport;

        public InstituteWiseFeeCollectionFacade(InstituteWiseFeeCollectionInterface data)
        {
            _ChairmanDashboardReport = data;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("Getdetails")]
        public InstituteWiseFeeCollectionDTO Getdetails([FromBody] InstituteWiseFeeCollectionDTO data)//int IVRMM_Id
        {
            return _ChairmanDashboardReport.Getdetails(data);
        }


        [HttpPost]
        [Route("Getsectioncount")]
        public InstituteWiseFeeCollectionDTO Getsectioncount([FromBody] InstituteWiseFeeCollectionDTO data)
        {
            return _ChairmanDashboardReport.Getsectioncount(data);
        }



    }
}
