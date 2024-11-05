using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class ClgSMSEmailCountFacade : Controller
    {
        public ClgSMSEmailCountInterface inter;

        public ClgSMSEmailCountFacade(ClgSMSEmailCountInterface maspag)
        {
            inter = maspag;
        }

        [Route("getdata")]
        public ClgSMSEmailCountDTO getdata([FromBody] ClgSMSEmailCountDTO data)
        {
            return inter.getdata(data);
        }

        [Route("getreport")]
        public ClgSMSEmailCountDTO getreport([FromBody] ClgSMSEmailCountDTO data)
        {
            return inter.getreport(data);
        }
        [Route("SearchByColumn")]
        public ClgSMSEmailCountDTO SearchByColumn([FromBody] ClgSMSEmailCountDTO data)
        {
            return inter.SearchByColumn(data);
        }
    }
}
