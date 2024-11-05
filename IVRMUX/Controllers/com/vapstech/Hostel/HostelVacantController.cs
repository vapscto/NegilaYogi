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
    public class StudentVacantController : Controller
    {
        StudentVacantDelegate del = new StudentVacantDelegate();

        [Route("loaddata")]
        public StudentVacantDTO loaddata([FromBody] StudentVacantDTO data)

        {
           // StudentVacantDTO data = new StudentVacantDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return del.loaddata(data);

        }
        [Route("getalldetailsOnselectiontype")]
        public StudentVacantDTO getalldetailsOnselectiontype([FromBody] StudentVacantDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.getalldetailsOnselectiontype(data);


        }
        [Route("get_studentDetail")]
        public StudentVacantDTO get_studentDetail([FromBody] StudentVacantDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.get_studentDetail(data);
        }
        [Route("get_staffDetail")]
        public StudentVacantDTO get_staffDetail([FromBody] StudentVacantDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.get_staffDetail(data);
        }
        [Route("get_guestDetail")]
        public StudentVacantDTO get_guestDetail([FromBody] StudentVacantDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.get_guestDetail(data);
        }

        [Route("edittab1")]
        public StudentVacantDTO edittab1([FromBody] StudentVacantDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return del.edittab1(data);
        }
        
    }
}
