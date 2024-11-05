using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using Microsoft.AspNetCore.Http;

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AttendanceLPFacedeController : Controller
    {
        // create interface object
        public AttendanceLPInterface _IAtt;
        
        // Default Constructor
        //public AttendanceLPFacedeController() { }


        // Parameterized constructor
        public AttendanceLPFacedeController(AttendanceLPInterface IAtt)
        {
            _IAtt = IAtt;
        }

        [Route("getdatabyselectedtype")]
        public AttendanceLPDTO getDataByType([FromBody] AttendanceLPDTO dto)
        {
            return _IAtt.getDataByTypeSelected(dto);
        }

        [Route("savedata")]
        public AttendanceLPDTO SaveData([FromBody]AttendanceLPDTO attdo)
        {
            return _IAtt.SaveData(attdo);
        }
        [Route("getyear")]
        public AttendanceLPDTO getyear([FromBody]AttendanceLPDTO attdo)
        {
            return _IAtt.getyear(attdo);
        }
        
        [Route("getinitialdata/{MIID:long}")]
        public AttendanceLPDTO GetInitialData(long MIID)
        {
            return _IAtt.GetInitialData(MIID);
        }

        [Route("geteditdata")]
        public AttendanceLPDTO GetEditData([FromBody]AttendanceLPDTO attdto)
        {
            return _IAtt.GetEditData(attdto);
        }
        [Route("staffwisegrid")]
        public AttendanceLPDTO staffwisegrid([FromBody]AttendanceLPDTO attdto)
        {            
            return _IAtt.staffwisegrid(attdto);
        }
        [Route("deleteRecord")]
        public AttendanceLPDTO deleteRecord([FromBody]AttendanceLPDTO obj)
        {
            return _IAtt.deleteAttPrivileges(obj);
        }

    }
}
