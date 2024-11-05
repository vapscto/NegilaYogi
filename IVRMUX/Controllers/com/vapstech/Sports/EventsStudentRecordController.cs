using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Sports;
using PreadmissionDTOs.com.vaps.Sports;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class EventsStudentRecordController : Controller
    {
        EventsStudentRecordDelegate delegat = new EventsStudentRecordDelegate();

        [Route("loadgrid/{id:int}")]
        public EventsStudentRecordDTO getDetails(int id)
        {
            EventsStudentRecordDTO dto = new EventsStudentRecordDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //dto.ASMAY_Id=Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.getDetails(dto);
        }
        [Route("getevent")]
        public EventsStudentRecordDTO getevent([FromBody]EventsStudentRecordDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.getevent(data);
        }
        [Route("saveRecord")]
        public EventsStudentRecordDTO saveRecord([FromBody]EventsStudentRecordDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.save(data);
        }
        [Route("Edit")]
        public EventsStudentRecordDTO Edit([FromBody]EventsStudentRecordDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.EditDetails(data);
        }
        //EditDetailsSRKVS
        [Route("EditDetailsSRKVS")]
        public EventsStudentRecordDTO EditDetailsSRKVS([FromBody]EventsStudentRecordDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.EditDetailsSRKVS(data);
        }
        [Route("editdata")]
        public EventsStudentRecordDTO editdata([FromBody]EventsStudentRecordDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.editdata(data);
        }
        [Route("deactivate")]
        public EventsStudentRecordDTO deactivate([FromBody] EventsStudentRecordDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.deactivate(d);
        }
        [Route("getStudents")]
        public EventsStudentRecordDTO getStudents([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.getStudents(dto);
        }

        [Route("onhousechage")]
        public EventsStudentRecordDTO onhousechage([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delegat.onhousechage(dto);
        }

        [Route("onChangeActivities")]
        public EventsStudentRecordDTO onChangeActivities([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.onChangeActivities(dto);
        }

        [Route("get_student_info")]
        public EventsStudentRecordDTO get_student_info([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.get_student_info(dto);
        }

        [Route("get_modeldata")]
        public EventsStudentRecordDTO get_modeldata([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.get_modeldata(dto);
        }

        [Route("get_SportsName")]
        public EventsStudentRecordDTO get_SportsName([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delegat.get_SportsName(dto);
        }


        [Route("get_uom_Name")]
        public EventsStudentRecordDTO get_uom_Name([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.get_uom_Name(dto);
        }

        [Route("Deactivatestud")]
        public EventsStudentRecordDTO Deactivatestud([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.Deactivatestud(dto);
        }

        [Route("classChange")]
        public EventsStudentRecordDTO classChange([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delegat.classChange(dto);
        }
        [Route("get_class_house")]
        public EventsStudentRecordDTO get_class_house([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delegat.get_class_house(dto);
        }

        [Route("get_StudentAgeFilter")]
        public EventsStudentRecordDTO get_StudentAgeFilter([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.get_StudentAgeFilter(dto);
        }

        [Route("houseWiseCompStudentList")]
        public EventsStudentRecordDTO houseWiseCompStudentList([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.houseWiseCompStudentList(dto);
        }

        [Route("classWiseCompStudentList")]
        public EventsStudentRecordDTO classWiseCompStudentList([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.classWiseCompStudentList(dto);
        }


        [Route("sectionWiseCompStudentList")]
        public EventsStudentRecordDTO sectionWiseCompStudentList([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.sectionWiseCompStudentList(dto);
        }

        [Route("get_houseCatAgeFilter")]
        public EventsStudentRecordDTO get_houseCatAgeFilter([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.get_houseCatAgeFilter(dto);
        }

        [Route("comcatwise_classAgefilter")]
        public EventsStudentRecordDTO comcatwise_classAgefilter([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.comcatwise_classAgefilter(dto);
        }

        [Route("houseWiseCompcatClssSectStudentList")]
        public EventsStudentRecordDTO houseWiseCompcatClssSectStudentList([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.houseWiseCompcatClssSectStudentList(dto);
        }

        [Route("get_ComCatgrylistClassWise")]
        public EventsStudentRecordDTO get_ComCatgrylistClassWise([FromBody] EventsStudentRecordDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.get_ComCatgrylistClassWise(dto);
        }
        //saveRecordSRKVS
        [Route("saveRecordSRKVS")]
        public EventsStudentRecordDTO saveRecordSRKVS([FromBody]EventsStudentRecordDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.saveRecordSRKVS(data);
        }
        //UpdateStudentSRKVS
        [Route("UpdateStudentSRKVS")]
        public EventsStudentRecordDTO UpdateStudentSRKVS([FromBody]EventsStudentRecordDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delegat.UpdateStudentSRKVS(data);
        }


        [Route("MasterDeleteEventsStudent")]
        public EventsStudentRecordDTO MasterDeleteEventsStudent([FromBody]EventsStudentRecordDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delegat.MasterDeleteEventsStudent(data);
        }


    }
}
