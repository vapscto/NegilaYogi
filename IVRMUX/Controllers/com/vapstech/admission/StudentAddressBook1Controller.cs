using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using corewebapi18072016.Delegates.com.vapstech.admission;
using CommonLibrary;


namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentAddressBook1Controller:Controller
    {

        StudentAddressBook1Delegate st = new StudentAddressBook1Delegate();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("getinitialdata/{id:int}")]
        public StudentAddressBook1DTO getInitialData(int id)
        {
             id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));         
            return st.getData(id);
        }



        [Route("classchange")]
        public StudentAddressBook1DTO classchange()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //return sad.getInitailData(mi_id);
            //CommonDelegate<StudentAddressBook1DTO> sad1 = new CommonDelegate<StudentAddressBook1DTO>();
            //var aa = sad1.GetDataById(id, "StudentAddressBook1Facade/getinitialdata/");
            //StudentAddressBook1DTO cdto = (StudentAddressBook1DTO)aa;
            return st.getData1(id);
        }




        [Route("yearchange")]
        public StudentAddressBook1DTO yearchange([FromBody] StudentAddressBook1DTO data )
        {
            data.MI_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return st.yearchange(data);
        }



        [Route("sectionchange")]
        public StudentAddressBook1DTO sectionchange([FromBody] StudentAddressBook1DTO MMD)
        {
            MMD.MI_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return st.sectionchange(MMD);
        }



        [HttpPost]
        [Route("getdetails")]
        public StudentAddressBook1DTO getdetails([FromBody]StudentAddressBook1DTO MMD)
        {
            MMD.MI_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return st.getdetails(MMD);
        }




        [Route("ExportToExcle/")]
        public string ExportToExcle([FromBody] StudentAddressBook1DTO MMD)
        {
            return st.ExportToExcle(MMD);
        }





        //[Route("Getdetails/")]
        //public StudentAddressBook1DTO Getdetails(int id)
        //{
        //    //int drpdata = en.countryid;
        //    //EnqDTO enq=new EnqDTO();
        //    // AttendanceEntryTypeDTO tempdto = null;
        //    //  tempdto =  AttendanceEntryTypeStr.GetAttendanceEnetryTypeData(AttendanceEntryTypeDTO);
        //    // return tempdto;
        //    return st.getData(id);
        //    //return st.getData(StudentAddressBook1DTO);





        //}



    }
}
