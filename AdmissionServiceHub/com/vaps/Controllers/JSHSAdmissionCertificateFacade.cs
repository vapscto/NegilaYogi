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
    public class JSHSAdmissionCertificateFacade : Controller
    {
        public JSHSAdmissionCertificateInterface inter;

        public JSHSAdmissionCertificateFacade(JSHSAdmissionCertificateInterface t)
        {
            inter = t;
        }
        [HttpPost]
        [Route("getdata")]
        public JSHSAdmissionCertificate_DTO getdata([FromBody] JSHSAdmissionCertificate_DTO data)
        {
            return inter.getdata(data);
        }
        [Route("searchfilter")]
        public JSHSAdmissionCertificate_DTO searchfilter([FromBody] JSHSAdmissionCertificate_DTO data)
        {
            return inter.searchfilter(data);
        }
        [Route("onchangeyear")]
        public JSHSAdmissionCertificate_DTO onchangeyear([FromBody] JSHSAdmissionCertificate_DTO data)
        {
            return inter.onchangeyear(data);
        }
        [Route("onchangeclass")]
        public JSHSAdmissionCertificate_DTO onchangeclass([FromBody] JSHSAdmissionCertificate_DTO data)
        {
            return inter.onchangeclass(data);
        }
        [Route("onchangesection")]
        public JSHSAdmissionCertificate_DTO onchangesection([FromBody] JSHSAdmissionCertificate_DTO data)
        {
            return inter.onchangesection(data);
        }

        [Route("getStudData")]
        public Task<JSHSAdmissionCertificate_DTO> getStudData([FromBody] JSHSAdmissionCertificate_DTO stuDTO)
        {
            return inter.getStudData(stuDTO);
        }
    }
}
