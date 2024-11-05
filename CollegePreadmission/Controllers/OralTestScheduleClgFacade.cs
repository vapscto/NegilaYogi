using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegePreadmission.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Preadmission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegePreadmission.Controllers
{
    [Route("api/[controller]")]
    public class OralTestScheduleClgFacade : Controller
    {

        public OralTestScheduleClgInterface _OralTestScheduleClg;


        public OralTestScheduleClgFacade(OralTestScheduleClgInterface OralTestSchedule)
        {
            _OralTestScheduleClg = OralTestSchedule;
        }
        [HttpPost]

        [Route("Getdetails/")]
        public DocumentViewClgDTO Getdetails([FromBody] DocumentViewClgDTO StudentDetailsDTO)//int IVRMM_Id
        {
            return _OralTestScheduleClg.GetOralTestScheduleData(StudentDetailsDTO);
        }
        [Route("coursewisestudent")]
        public DocumentViewClgDTO getdashboardpage([FromBody] DocumentViewClgDTO dt)
        {
            return _OralTestScheduleClg.coursewisestudent(dt);
        }
        [HttpPost]
        public Task<DocumentViewClgDTO> Post([FromBody] DocumentViewClgDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _OralTestScheduleClg.OralTestScheduleData(masterMDT);
        }

        [HttpDelete]
        [Route("OralTestScheduleDeletesData/{id:int}")]
        public DocumentViewClgDTO OralTestScheduleDeletesData(int ID)
        {
            // return _reg.getregdata(reg);
            return _OralTestScheduleClg.OralTestScheduleDeletesData(ID);
        }

        [Route("checkaddparticipants")]
        public DocumentViewClgDTO checkaddparticipants([FromBody] DocumentViewClgDTO dt)
        {
            return _OralTestScheduleClg.checkaddparticipants(dt);
        }


        
        [Route("scheduleGetreportdetails")]
        public Task<DocumentViewClgDTO> scheduleGetreportdetails([FromBody]DocumentViewClgDTO data)
        {
            return _OralTestScheduleClg.scheduleGetreportdetails(data);
        }

        [Route("remarksGetreportdetails")]
        public Task<DocumentViewClgDTO> remarksGetreportdetails([FromBody]DocumentViewClgDTO data)
        {
            return _OralTestScheduleClg.remarksGetreportdetails(data);
        }
    }
}
