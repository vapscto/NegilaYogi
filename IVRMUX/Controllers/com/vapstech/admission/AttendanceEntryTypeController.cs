using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
   // [EnableCors("AllowSpecificOrigin")]

    public class AttendanceEntryTypeController : Controller
    {
        // GET: /<controller>/
        AttendanceEntryTypeDelegates AttendanceEntryTypeStr = new AttendanceEntryTypeDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/")]
        //public AttendanceEntryTypeDTO Getdetails(AttendanceEntryTypeDTO AttendanceEntryTypeDTO)
        //{
        //    AttendanceEntryTypeDTO.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return AttendanceEntryTypeStr.GetAttendanceEnetryTypeData(AttendanceEntryTypeDTO);
        //}


        public AttendanceEntryTypeDTO Getdetails(int id)
        {            

            AttendanceEntryTypeDTO AttendanceEntryTypeDTO = new AttendanceEntryTypeDTO();
            AttendanceEntryTypeDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            AttendanceEntryTypeDTO.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return AttendanceEntryTypeStr.GetAttendanceEnetryTypeData(AttendanceEntryTypeDTO);
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public AttendanceEntryTypeDTO GetSelectedRowDetails(int ID)
        {
            AttendanceEntryTypeDTO AttendanceEntryTypeDTO = new AttendanceEntryTypeDTO();
            AttendanceEntryTypeDTO.ASAET_Id = ID;
            AttendanceEntryTypeDTO.yearid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return AttendanceEntryTypeStr.GetSelectedRowDetails(AttendanceEntryTypeDTO);

        }

        [HttpPost]
        public AttendanceEntryTypeDTO AttendanceTypeData([FromBody] AttendanceEntryTypeDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AttendanceEntryTypeStr.AttendanceTypeEntryData(MMD);
        }

        [HttpDelete]
        [Route("AttendanceDeleteEntryTypeDTO/{id:int}")]
        public AttendanceEntryTypeDTO AttendanceDeleteEntryTypeDTO(int ID)
        {
            return AttendanceEntryTypeStr.AttendanceDeleteEntryTypeDTO(ID);
           
        }

    }

}
