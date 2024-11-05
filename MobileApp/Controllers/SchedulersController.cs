using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using MobileApp.Delegates;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.BirthDay;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.College.COE;
using PreadmissionDTOs.com.vaps.College.BirthDay;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MobileApp.Controllers
{
    [Route("api/[controller]")]
    public class SchedulersController : Controller
    {
        Schedulersdelegate pre = new Schedulersdelegate();
       
       
        [Route("getSchdule/{id:int}")]
        public BirthDayDTO getSchdule(int id)
        {
            return pre.getdata(id);
        }

        [Route("getdetail/{id:int}")]
        public MasterCOEDTO getdetail(int id)
        {
            return pre.getdetails(id);
        }

        [Route("Latedetail")]
        public FO_Emp_PunchDTO Latedetail([FromBody] FO_Emp_PunchDTO data)
        {
            return pre.Latedetails(data);
        }

        [Route("LateInAbs_Email/{id:int}")]
        public FO_Emp_PunchDTO LateInAbs_Email(int id)
        {
            FO_Emp_PunchDTO data = new FO_Emp_PunchDTO();
            data.MI_Id = Convert.ToInt32(id);
            return pre.LateInAbs_Email(data);
        }

        [Route("EarlyOut_Email/{id:int}")]
        public FO_Emp_PunchDTO EarlyOut_Email(int id)
        {
            FO_Emp_PunchDTO data = new FO_Emp_PunchDTO();
            data.MI_Id = Convert.ToInt32(id);
            return pre.EarlyOut_Email(data);
        }

        [Route("Earlydetail")]
        public FO_Emp_PunchDTO Earlydetail([FromBody] FO_Emp_PunchDTO data)
        {
            return pre.Earlydetails(data);
        }

        [Route("punchdata")]
        public FO_Emp_PunchDTO punchdata([FromBody] FO_Emp_PunchDTO data)
        {
            return pre.punchdatas(data);
        }
        [Route("clg_getdetail/{id:int}")]
        public ClgMasterCOEDTO clg_getdetail(int id)
        {
            return pre.clg_getdetail(id);
        }

        [Route("clg_getBirthday/{id:int}")]
        public ClgBirthDayDTO clg_getBirthday(int id)
        {
            return pre.clg_getBirthday(id);
        }
    }
}
