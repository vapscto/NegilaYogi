using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Masters;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class Collegemasterstudentconcessioncontroller : Controller
    {

      
        CollegemasterstudentconcessionDelegate od = new CollegemasterstudentconcessionDelegate();

        [Route("getdata")]
        public CollegeConcessionDTO getdata([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.getdata(data);
        }

        [Route("get_courses")]
        public CollegeConcessionDTO get_courses([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.get_courses(data);
        }

        [Route("get_branches")]
        public CollegeConcessionDTO get_branches([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.get_branches(data);
        }

        [Route("get_semisters")]
        public CollegeConcessionDTO get_semisters([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.get_semisters(data);
        }

        [Route("get_student")]
        public CollegeConcessionDTO get_student([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.get_student(data);
        }


        [Route("fillamount")]
        public CollegeConcessionDTO fillamount([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.fillamount(data);
        }

        [Route("fillheaddetailsss")]
        public CollegeConcessionDTO fillheaddetailsss([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.fillheaddetailsss(data);
        }





        [Route("savedata")]
        public CollegeConcessionDTO savedata([FromBody] CollegeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.savedata(data);
        }

      
        [Route("DeletRecord/{id:int}")]
        public CollegeConcessionDTO DeletRecord(int id)
        {
            CollegeConcessionDTO data = new CollegeConcessionDTO();
            data.FSCI_ID = id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            int user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = user_id;

            return od.DeletRecord(data);
        }

    }
}
