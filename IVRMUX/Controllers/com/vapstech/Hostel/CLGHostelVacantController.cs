using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Hostel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Hostel
{
    [Route("api/[controller]")]
    public class CLGHostelVacantController : Controller
    {
        CLGHostelVacantDelegate del = new CLGHostelVacantDelegate();

        [Route("loaddata")]
        public CLGHostelVacantDTO loaddata([FromBody] CLGHostelVacantDTO data)

        {
            // StudentVacantDTO data = new StudentVacantDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return del.loaddata(data);

        }
        [Route("getalldetailsOnselectiontype")]
        public CLGHostelVacantDTO getalldetailsOnselectiontype([FromBody] CLGHostelVacantDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.getalldetailsOnselectiontype(data);


        }
        [Route("get_studentDetail")]
        public CLGHostelVacantDTO get_studentDetail([FromBody] CLGHostelVacantDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.get_studentDetail(data);
        }
        [Route("get_staffDetail")]
        public CLGHostelVacantDTO get_staffDetail([FromBody] CLGHostelVacantDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.get_staffDetail(data);
        }
        [Route("get_guestDetail")]
        public CLGHostelVacantDTO get_guestDetail([FromBody] CLGHostelVacantDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.get_guestDetail(data);
        }

        [Route("edittab1")]
        public CLGHostelVacantDTO edittab1([FromBody] CLGHostelVacantDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return del.edittab1(data);
        }

    }
}
