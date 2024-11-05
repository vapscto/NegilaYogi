using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class TuitionFeeCertificateController : Controller
    {


        TuitionFeeCertificateDelegate del = new TuitionFeeCertificateDelegate();
        [HttpGet]
        [Route("getdata/{id:int}")]
        public TuitionFeeCertificate_DTO getdata(int id)
        {
            TuitionFeeCertificate_DTO data = new TuitionFeeCertificate_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getdata(data);
        }

        [Route("searchfilter")]
        public TuitionFeeCertificate_DTO searchfilter([FromBody] TuitionFeeCertificate_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.searchfilter(data);
        }
        [Route("onchangeyear")]
        public TuitionFeeCertificate_DTO onchangeyear([FromBody] TuitionFeeCertificate_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public TuitionFeeCertificate_DTO onchangeclass([FromBody] TuitionFeeCertificate_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.onchangeclass(data);
        }

        [Route("onchangesection")]
        public TuitionFeeCertificate_DTO onchangesection([FromBody] TuitionFeeCertificate_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.onchangesection(data);
        }

        [Route("Studdetails")]

        public TuitionFeeCertificate_DTO getStudData([FromBody] TuitionFeeCertificate_DTO stuDTO)
        {
            stuDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getStudData(stuDTO);

        }
    }
}
