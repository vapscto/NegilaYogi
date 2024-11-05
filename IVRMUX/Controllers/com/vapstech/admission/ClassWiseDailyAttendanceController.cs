using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using DomainModel.Model;
using PreadmissionDTOs;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClassWiseDailyAttendanceController : Controller
    {

        ClassWiseDailyAttendanceDelegates ClassWiseDailyAttendancedelStr = new ClassWiseDailyAttendanceDelegates();


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public SchoolYearWiseStudentDTO Getdetails(int MI_Id)
        {
            SchoolYearWiseStudentDTO data = new SchoolYearWiseStudentDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return ClassWiseDailyAttendancedelStr.GetClassWiseDailyAttendanceData(data);
        }
        [HttpPost]
        [Route("Getdetailsreport/")]
        public SchoolYearWiseStudentDTO Getdetailsreport([FromBody] SchoolYearWiseStudentDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            //   MMD.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            MMD.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return ClassWiseDailyAttendancedelStr.Getdetailsreport(MMD);
        }


        [Route("GetSelectedRowdetails/{id:int}")]
        public castecategoryDTO GetSelectedRowDetails(int ID)
        {
            HttpContext.Session.SetString("castecategoryID", ID.ToString());
            return ClassWiseDailyAttendancedelStr.GetSelectedRowDetails(ID);
        }
        [Route("getsection")]
        public SchoolYearWiseStudentDTO getsection([FromBody] SchoolYearWiseStudentDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            // MMD.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            MMD.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return ClassWiseDailyAttendancedelStr.getsection(MMD);
        }
        [Route("setfromdate")]
        public SchoolYearWiseStudentDTO setfromdate([FromBody] SchoolYearWiseStudentDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            //   MMD.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            MMD.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return ClassWiseDailyAttendancedelStr.setfromdate(MMD);
        }
        [Route("absentsendsms")]
        public SchoolYearWiseStudentDTO absentsendsms([FromBody]SchoolYearWiseStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ClassWiseDailyAttendancedelStr.absentsendsms(data);
        }
        [HttpPost]
        public castecategoryDTO castecategoryDTO([FromBody] castecategoryDTO MMD)
        {
            Int32 castecategoryID = 0;
            if (HttpContext.Session.GetString("castecategoryID") != null)
            {
                castecategoryID = Convert.ToInt32(HttpContext.Session.GetString("castecategoryID"));
            }
            MMD.IMCC_Id = castecategoryID;
            HttpContext.Session.Remove("castecategoryID");
            ClassWiseDailyAttendancedelStr.castecategoryData(MMD);
            return MMD;
        }

        [HttpDelete]
        [Route("MasterDeleteModulesDTO/{id:int}")]
        public castecategoryDTO castecategoryDTO(int ID)
        {
            return ClassWiseDailyAttendancedelStr.MasterDeleteModulesData(ID);
        }


    }

}
