using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]

    public class intimationscheduleFacadeController : Controller
    {

        public intimationscheduleInterface _OralTestSchedule;


        public intimationscheduleFacadeController(intimationscheduleInterface OralTestSchedule)
        {
            _OralTestSchedule = OralTestSchedule;
        }
       
        [HttpGet]
        [Route("GetSelectedRowDetails/{id:int}")]
        public OralTestScheduleDTO GetSelectedRowDetails(int ID)
        {
            // return _reg.getregdata(reg);      
            return _OralTestSchedule.GetSelectedRowDetails(ID);
        }

        [Route("GetStudentdetails/{id:int}")]
        public StudentDetailsDTO GetStudentdetails(int ID)
        {
            // return _reg.getregdata(reg);      
            return _OralTestSchedule.GetSelectedStudentData(ID);
        }

        // POST api/values
        [HttpPost]
        public Task< OralTestScheduleDTO> Post([FromBody] OralTestScheduleDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _OralTestSchedule.OralTestScheduleData(masterMDT);
        }

        [Route("Getdetails/")]
        public StudentDetailsDTO Getdetails([FromBody] StudentDetailsDTO StudentDetailsDTO)//int IVRMM_Id
        {
            return _OralTestSchedule.GetOralTestScheduleData(StudentDetailsDTO);
        }

        [Route("OralTestScheduleDeletesStudentData")]
        public OralTestScheduleDTO OralTestScheduleDeletesStudentData([FromBody] OralTestScheduleDTO OralTestScheduleDTO)
        {
            // return _reg.getregdata(reg);
            return _OralTestSchedule.OralTestScheduleDeletesStudentData(OralTestScheduleDTO);
        }

        [HttpDelete]
        [Route("OralTestScheduleDeletesData/{id:int}")]
        public OralTestScheduleDTO OralTestScheduleDeletesData(int ID)
        {
            // return _reg.getregdata(reg);
            return _OralTestSchedule.OralTestScheduleDeletesData(ID);
        }
        [Route("getdataonsearchfilter")]
        public CommonDTO getdataonsearchfilter([FromBody] CommonDTO cdto)
        {
            return _OralTestSchedule.getdataonsearchfilter(cdto);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

    }
}
