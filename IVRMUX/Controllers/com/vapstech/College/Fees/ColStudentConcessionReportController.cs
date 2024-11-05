using IVRMUX.Delegates.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.College.Fees
{
    [Route("api/[Controller]")]
    [ValidateAntiForgeryToken]
    public class ColStudentConcessionReportController:Controller
    {
        ColStudentConcessionReportDelegate dt = new ColStudentConcessionReportDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]

        public CollegeConcessionDTO getalldetails(int id)
        {
            CollegeConcessionDTO data = new CollegeConcessionDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            
            return dt.getalldetails(data);

        }

    }
}
