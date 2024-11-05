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

    public class WrittenTestScheduleFacadeController : Controller
    {
        public WrittenTestScheduleInterface _MasterModule;


        public WrittenTestScheduleFacadeController(WrittenTestScheduleInterface MasterModule)
        {
            _MasterModule = MasterModule;
        }

        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5

      
        [Route("Getdetails")]
       
        public StudentDetailsDTO Getdetails([FromBody]StudentDetailsDTO StudentDetailsDTO)//int IVRMM_Id
        {
            return _MasterModule.GetWrittenTestScheduleData(StudentDetailsDTO);
        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public WrittenTestScheduleDTO GetSelectedRowDetails(int ID)
        {
            // return _reg.getregdata(reg);      
            return _MasterModule.GetSelectedRowDetails(ID);
        }

        [Route("GetStudentdetails/{id:int}")]
        public StudentDetailsDTO GetStudentdetails(int ID)
        {
            // return _reg.getregdata(reg);      
            return _MasterModule.GetSelectedStudentData(ID);
        }

        // POST api/values
        [HttpPost]
        public Task<WrittenTestScheduleDTO> Post([FromBody] WrittenTestScheduleDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _MasterModule.WrittenTestScheduleData(masterMDT);            
        }

        
        [Route("WrittenTestScheduleDeletesData/{id:int}")]
        public WrittenTestScheduleDTO WrittenTestScheduleDeletesData(int ID)
        {
            // return _reg.getregdata(reg);
            return _MasterModule.WrittenTestScheduleDeletesData(ID);
        }

        //[Route("WrittenTestScheduleDeletesStudentData/{id:int}/{MID:int}")]
        //public WrittenTestScheduleDTO WrittenTestScheduleDeletesStudentData(int ID,int MID)
        //{
        //    // return _reg.getregdata(reg);
        //    return _MasterModule.WrittenTestScheduleDeletesStudentData(ID,MID);
        //}



        [Route("WrittenTestScheduleDeletesStudentData")]
        public WrittenTestScheduleDTO WrittenTestScheduleDeletesStudentData([FromBody] WrittenTestScheduleDTO WrittenTestScheduleDTO)
        {
            // return _reg.getregdata(reg);
            return _MasterModule.WrittenTestScheduleDeletesStudentData(WrittenTestScheduleDTO);
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
