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
using System.Web;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]

    public class WrittenTestScheduleController : Controller
    {

        // GET: /<controller>/
        WrittenTestScheduleDelegates WrittenTestScheduleDelegates = new WrittenTestScheduleDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


       

        [Route("GetStudentdetails/{id:int}")]
        public StudentDetailsDTO GetStudentdetails(int ID)
        {
            //string SelectedStudentsID = "0";

            //if (HttpContext.Session.GetString("SelectedStudentsID") != null)
            //{
            //    HttpContext.Session.SetString("SelectedStudentsID", Convert.ToString(ID) + "," + HttpContext.Session.GetString("SelectedStudentsID"));
            //    SelectedStudentsID = HttpContext.Session.GetString("SelectedStudentsID");
            //}
            //else
            //{
            //    HttpContext.Session.SetString("SelectedStudentsID", ID.ToString());
            //    SelectedStudentsID = Convert.ToString(ID);
            //}

            return WrittenTestScheduleDelegates.GetSelectedStudentData(ID);

        }

        [Route("GetCurrentStudentdetails/{id:int}")]
        public StudentDetailsDTO GetCurrentStudentdetails(int ID)
        {
            //string SelectedStudentsID = "0";

            //if (HttpContext.Session.GetString("SelectedStudentsID") != null)
            //{
            //    HttpContext.Session.SetString("SelectedStudentsID", Convert.ToString(ID) + "," + HttpContext.Session.GetString("SelectedStudentsID"));
            //    SelectedStudentsID = Convert.ToString(ID) + "," + HttpContext.Session.GetString("SelectedStudentsID");
            //}
            //else
            //{
            //    HttpContext.Session.SetString("SelectedStudentsID", ID.ToString());
           // SelectedStudentsID = Convert.ToString(ID);
            // }


            return WrittenTestScheduleDelegates.GetSelectedStudentData(ID);

        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public WrittenTestScheduleDTO GetSelectedRowDetails(int ID)
        {
            //int drpdata = en.countryid;
            //EnqDTO enq=new EnqDTO();
            HttpContext.Session.SetString("WrittenTestScheduleID", ID.ToString());
            return WrittenTestScheduleDelegates.GetSelectedRowDetails(ID);

        }

        [Route("GetRemainStudentdetails/")]
        public WrittenTestScheduleDTO GetRemainStudentdetails()
        {
            Int32 WrittenTestScheduleID = 0;
            if (HttpContext.Session.GetString("WrittenTestScheduleID") != null)
            {
                WrittenTestScheduleID = Convert.ToInt32(HttpContext.Session.GetString("WrittenTestScheduleID"));
            }
            return WrittenTestScheduleDelegates.GetSelectedRowDetails(WrittenTestScheduleID);

        }

        [HttpPost]
        // public IActionResult Post([FromBody] regis reg)
        public WrittenTestScheduleDTO WrittenTestSchedule([FromBody] WrittenTestScheduleDTO MMD)





        {


            Int32 WrittenTestScheduleID = 0;

            if (HttpContext.Session.GetString("WrittenTestScheduleID") != null)
            {
                WrittenTestScheduleID = Convert.ToInt32(HttpContext.Session.GetString("WrittenTestScheduleID"));
            }
             MMD.IVRMSTAUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           // MMD.IVRMSTAUL_Id = 1;
            MMD.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.PAWTS_Id = WrittenTestScheduleID;
            HttpContext.Session.Remove("WrittenTestScheduleID");
            return WrittenTestScheduleDelegates.WrittenTestScheduleData(MMD);
        }

        [Route("Getdetails")]
        public StudentDetailsDTO Getdetails([FromBody] StudentDetailsDTO StudentDetailsDTO)
        {

            StudentDetailsDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentDetailsDTO.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));


            //HttpContext.Session.Remove("SelectedStudentsID");
            return WrittenTestScheduleDelegates.GetWrittenTestScheduleData(StudentDetailsDTO);

        }

        [HttpDelete]
        [Route("WrittenTestScheduleDeletesData/{id:int}")]
        public WrittenTestScheduleDTO WrittenTestScheduleDeletesData(int ID)
        {

            return WrittenTestScheduleDelegates.WrittenTestScheduleDeletesData(ID);
            //reg.status = "sucess";

            // return reg;
        }

        //[Route("WrittenTestScheduleDeletesStudentData/{id:int}")]
        //public WrittenTestScheduleDTO WrittenTestScheduleDeletesStudentData(int ID)
        //{
        //    Int32 WrittenTestScheduleID = 0;

        //    if (HttpContext.Session.GetString("WrittenTestScheduleID") != null)
        //    {
        //        WrittenTestScheduleID = Convert.ToInt32(HttpContext.Session.GetString("WrittenTestScheduleID"));
        //    }
           
        //    return WrittenTestScheduleDelegates.WrittenTestScheduleDeletesStudentData(ID, WrittenTestScheduleID);
        //    //reg.status = "sucess";

        //    // return reg;
        //}
        [Route("WrittenTestScheduleDeletesStudentData")]
        public WrittenTestScheduleDTO WrittenTestScheduleDeletesStudentData([FromBody] WrittenTestScheduleDTO MMD)
        {

            return WrittenTestScheduleDelegates.WrittenTestScheduleDeletesStudentData(MMD);

        }

    }
}
