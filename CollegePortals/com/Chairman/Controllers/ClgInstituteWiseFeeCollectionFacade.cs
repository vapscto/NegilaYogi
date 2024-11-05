using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegePortals.com.Chairman.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Portals.Chairman;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegePortals.com.Chairman.Controllers
{
    [Route("api/[controller]")]
    public class ClgInstituteWiseFeeCollectionFacade : Controller
    {
        public ClgInstituteWiseFeeCollectionInterface _ChairmanDashboardReport;

        public ClgInstituteWiseFeeCollectionFacade(ClgInstituteWiseFeeCollectionInterface data)
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
        public ClgInstituteWiseFeeCollectionDTO Getdetails([FromBody] ClgInstituteWiseFeeCollectionDTO data)//int IVRMM_Id
        {
            return _ChairmanDashboardReport.Getdetails(data);
        }


        [HttpPost]
        [Route("Getsectioncount")]
        public ClgInstituteWiseFeeCollectionDTO Getsectioncount([FromBody] ClgInstituteWiseFeeCollectionDTO data)
        {
            return _ChairmanDashboardReport.Getsectioncount(data);
        }

    }
}
