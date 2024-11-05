using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class TuitionFeeCertificateFacade : Controller
    {


        public TuitionFeeCertificateInterface inter;

        public TuitionFeeCertificateFacade(TuitionFeeCertificateInterface t)
        {
            inter = t;
        }
        [HttpPost]
        [Route("getdata")]
        public TuitionFeeCertificate_DTO getdata([FromBody] TuitionFeeCertificate_DTO data)
        {
            return inter.getdata(data);
        }
        [Route("searchfilter")]
        public TuitionFeeCertificate_DTO searchfilter([FromBody] TuitionFeeCertificate_DTO data)
        {
            return inter.searchfilter(data);
        }
        [Route("onchangeyear")]
        public TuitionFeeCertificate_DTO onchangeyear([FromBody] TuitionFeeCertificate_DTO data)
        {
            return inter.onchangeyear(data);
        }
        [Route("onchangeclass")]
        public TuitionFeeCertificate_DTO onchangeclass([FromBody] TuitionFeeCertificate_DTO data)
        {
            return inter.onchangeclass(data);
        }
        [Route("onchangesection")]
        public TuitionFeeCertificate_DTO onchangesection([FromBody] TuitionFeeCertificate_DTO data)
        {
            return inter.onchangesection(data);
        }

        [Route("getStudData")]
        public Task<TuitionFeeCertificate_DTO> getStudData([FromBody] TuitionFeeCertificate_DTO stuDTO)
        {
            return inter.getStudData(stuDTO);
        }

    }
}
