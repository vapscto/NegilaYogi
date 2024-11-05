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
    public class AdmissionReportFacadeController : Controller
    {
            public AdmissionReportInterface _Inv;
            public AdmissionReportFacadeController(AdmissionReportInterface Inv)
            {
                _Inv = Inv;
            }
            [Route("getloaddata")]
            public AdmissionReportDTO getloaddata([FromBody] AdmissionReportDTO data)
            {
                return _Inv.getloaddata(data);
            }
            [Route("onreport")]
            public Task<AdmissionReportDTO> onreport([FromBody] AdmissionReportDTO data)
            {
                return _Inv.onreport(data);
            }
    }
}
