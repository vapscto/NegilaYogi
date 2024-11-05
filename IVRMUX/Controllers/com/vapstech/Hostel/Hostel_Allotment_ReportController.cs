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
    public class Hostel_Allotment_ReportController : Controller
    {
        Hostel_Allotment_ReportDelegate del = new Hostel_Allotment_ReportDelegate();

        [Route("getdata/{id:int}")]
        public Hostel_Allotment_ReportDTO getdata(int id)
        {
            Hostel_Allotment_ReportDTO data = new Hostel_Allotment_ReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getdata(data);
        }

        [HttpPost]
        [Route("getreport")]
        public Hostel_Allotment_ReportDTO getreport([FromBody] Hostel_Allotment_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getreport(data);
        }

        //Hostel Allotment Graphical Presentation Report
        [Route("Get_GP_OnLoad_Report/{id:int}")]
        public Hostel_Allotment_ReportDTO Get_GP_OnLoad_Report(int id)
        {
            Hostel_Allotment_ReportDTO data = new Hostel_Allotment_ReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.Get_GP_OnLoad_Report(data);
        }

        [Route("OnChangeHostel")]
        public Hostel_Allotment_ReportDTO OnChangeHostel([FromBody] Hostel_Allotment_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.OnChangeHostel(data);
        }

        [Route("Get_GP_Report")]
        public Hostel_Allotment_ReportDTO Get_GP_Report([FromBody] Hostel_Allotment_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Get_GP_Report(data);
        }

        [Route("Get_GP_RoomWise_StudentAlloted_Details")]
        public Hostel_Allotment_ReportDTO Get_GP_RoomWise_StudentAlloted_Details([FromBody] Hostel_Allotment_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Get_GP_RoomWise_StudentAlloted_Details(data);
        }
    }
}
