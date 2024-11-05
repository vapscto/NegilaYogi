using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ReligionCasteCategoryReportFacade : Controller
    {
        public ReligionCasteCategoryReportInterface _inter;
      public ReligionCasteCategoryReportFacade(ReligionCasteCategoryReportInterface inter)
        {
            _inter = inter;
        }
        [Route("loaddata")]
        public ReligionCasteCategoryReport_DTO loaddata([FromBody] ReligionCasteCategoryReport_DTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("showdetails")]
        public Task<ReligionCasteCategoryReport_DTO> showdetails([FromBody] ReligionCasteCategoryReport_DTO data)
        {
            return _inter.showdetails(data);
        }

    }
}
