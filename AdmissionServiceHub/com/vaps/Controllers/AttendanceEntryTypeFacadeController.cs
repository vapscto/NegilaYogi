using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vapstech.Controllers
{
    [Route("api/[controller]")]

    public class AttendanceEntryTypeFacadeController : Controller
    {
        public AttendanceEntryTypeInterface _AttendanceEntryType;


        public AttendanceEntryTypeFacadeController(AttendanceEntryTypeInterface AttendanceEntryType)
        {
            _AttendanceEntryType = AttendanceEntryType;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        // [HttpGet]



        [Route("Getdetails/")]
        public AttendanceEntryTypeDTO Getdetails([FromBody] AttendanceEntryTypeDTO AttendanceEntryTypeDTO)//int IVRMM_Id
        {
           
            return _AttendanceEntryType.GetAttendanceEntryTypeData(AttendanceEntryTypeDTO);
          
        }

        //[Route("GetSelectedRowDetails/{id:int}")]
        //public AttendanceEntryTypeDTO GetSelectedRowDetails(int ID)
        //{
        //    // return _reg.getregdata(reg);      
        //    return _AttendanceEntryType.GetSelectedRowDetails(ID);
        //}
        [Route("GetSelectedRowDetails")]
        public AttendanceEntryTypeDTO GetSelectedRowDetails([FromBody] AttendanceEntryTypeDTO ID)
        {
            // return _reg.getregdata(reg);      
            return _AttendanceEntryType.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public AttendanceEntryTypeDTO Post([FromBody] AttendanceEntryTypeDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _AttendanceEntryType.AttendanceEntryTypeData(masterMDT);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("AttendanceDeleteEntryTypeDATA/{id:int}")]
        public AttendanceEntryTypeDTO AttendanceDeleteEntryTypeDATA(int ID)
        {
            // return _reg.getregdata(reg);
            return _AttendanceEntryType.AttendanceDeleteEntryTypeDATA(ID);
        }

    }
}
