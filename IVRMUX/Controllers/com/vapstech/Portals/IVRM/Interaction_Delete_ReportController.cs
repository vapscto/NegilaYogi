using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.IVRM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using PreadmissionDTOs.com.vaps.Portals.IVRS;

namespace IVRMUX.Controllers.com.vapstech.Portals.IVRM
{
    [Route("api/[controller]")]
    public class Interaction_Delete_ReportController : Controller
    {
        Interaction_Delete_Report_Delegate idrd = new Interaction_Delete_Report_Delegate();

        [Route("loadreportdata/{id:int}")]
        public Interaction_Delete_Report_DTO loadreportdata(int id)
        {
            Interaction_Delete_Report_DTO dto = new Interaction_Delete_Report_DTO();
           dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
           
            return idrd.loadreportdata(dto);
        }

        [Route("getreport")]
        public Interaction_Delete_Report_DTO getreport([FromBody] Interaction_Delete_Report_DTO dto)
        {
           dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
           
            return idrd.getreport(dto);
        }
        [Route("getintreport")]
        public Interaction_Delete_Report_DTO getintreport([FromBody] Interaction_Delete_Report_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
         
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return idrd.getintreport(data);
        }


        //===================mobile app download
        [Route("mobload/{id:int}")]
        public Interaction_Delete_Report_DTO loadmobdwn(int id)
        {
            Interaction_Delete_Report_DTO dto = new Interaction_Delete_Report_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return idrd.mobload(dto);
        }

        [Route("mobreport")]
        public Interaction_Delete_Report_DTO mobreport([FromBody] Interaction_Delete_Report_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return idrd.mobreport(dto);
        }
    }
}