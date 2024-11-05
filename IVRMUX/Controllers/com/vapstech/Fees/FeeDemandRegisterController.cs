using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeDemandRegisterController : Controller
    {
        FeeDemandRegisterDelegate del = new FeeDemandRegisterDelegate();
       [Route("loaddata/{id:int}")]
       public FeeDemandRegisterDTO loaddata(int id)
        {
            FeeDemandRegisterDTO dto = new FeeDemandRegisterDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getInitailData(dto);
        }
        [Route("getStudentByYrClsSec")]
        public FeeDemandRegisterDTO getStudentByYrClsSec([FromBody]FeeDemandRegisterDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getStudentByYrClsSec(dto);
        }
        [Route("getgroupByCG")]
        public FeeDemandRegisterDTO getgroupByCG([FromBody]FeeDemandRegisterDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getgroupByCG(dto);
        }
        [Route("getReport")]
        public FeeDemandRegisterDTO getReport([FromBody]FeeDemandRegisterDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getReport(dto);
        }
        
    }
}
