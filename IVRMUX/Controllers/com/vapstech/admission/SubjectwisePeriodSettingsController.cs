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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
   // [EnableCors("AllowSpecificOrigin")]

    public class SubjectwisePeriodSettingsController : Controller
    {
        // GET: /<controller>/
        SubjectwisePeriodSettingsDelegates SubjectwisePeriodSettingsStr = new SubjectwisePeriodSettingsDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/")]
        public SubjectwisePeriodSettingsDTO Getdetails(SubjectwisePeriodSettingsDTO SubjectwisePeriodSettingsDTO)
        {
            //int drpdata = en.countryid;
            //EnqDTO enq=new EnqDTO();
            // AttendanceEntryTypeDTO tempdto = null;
            //  tempdto =  AttendanceEntryTypeStr.GetAttendanceEnetryTypeData(AttendanceEntryTypeDTO);
            // return tempdto;
            SubjectwisePeriodSettingsDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            SubjectwisePeriodSettingsDTO.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return SubjectwisePeriodSettingsStr.GetData(SubjectwisePeriodSettingsDTO);
        }



        [HttpPost]
        public SubjectwisePeriodSettingsDTO SaveData([FromBody] SubjectwisePeriodSettingsDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SubjectwisePeriodSettingsStr.SaveData(MMD);
        }
        [Route("subjectMaxPeriod")]
        public SubjectwisePeriodSettingsDTO subjectMaxPeriod([FromBody] SubjectwisePeriodSettingsDTO MMD)
        {
            return SubjectwisePeriodSettingsStr.getsubjectMaxPeriod(MMD);
        }



    }

}
