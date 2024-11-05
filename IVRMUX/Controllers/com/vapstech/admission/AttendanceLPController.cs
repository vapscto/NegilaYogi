using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vaps.admission;
using Microsoft.AspNetCore.Http;

namespace corewebapi18072016.Controllers.com.vaps.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AttendanceLPController : Controller
    {
        // create DTO object
        private AttendanceLPDelegate _AdmDTO = new AttendanceLPDelegate();

        // get data by selection type - teacher, class, section, subject
        [Route("getdatabyselectedtype/{id:int}")]
        public AttendanceLPDTO getDataBySelectedType(int id)
        {
            AttendanceLPDTO attdo = new AttendanceLPDTO();
            attdo.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            attdo.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            attdo.ASALU_EntryTypeFlag = id;
            return _AdmDTO.getDataBySelectedType(attdo);
        }

        [HttpPost]
        [Route("save")]
        public AttendanceLPDTO SaveData([FromBody] AttendanceLPDTO attdo)
        {
            attdo.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //attdo.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _AdmDTO.SaveData(attdo);
        }

        [Route("getyear")]
        public AttendanceLPDTO getyear([FromBody] AttendanceLPDTO attdo)
        {
            attdo.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //attdo.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _AdmDTO.getyear(attdo);
        }

        [Route("getinitialdata/{id:int}")]
        public AttendanceLPDTO LoadInitialData(int id)
        {
            long MIId = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _AdmDTO.LoadInitialData(MIId);
        }

        [Route("geteditdata")]
        public AttendanceLPDTO GetEditData([FromBody] AttendanceLPDTO attdto)
        {
            return _AdmDTO.GetEditData(attdto);
        }
        [Route("staffwisegrid")]
        public AttendanceLPDTO staffwisegrid([FromBody] AttendanceLPDTO attdto)
        {
            attdto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            // attdto.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _AdmDTO.staffwisegrid(attdto);
        }
        [Route("DeleteRecord")]
        public AttendanceLPDTO delete([FromBody] AttendanceLPDTO obj)
        {
            obj.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _AdmDTO.deleteRecord(obj);
        }
    }
}