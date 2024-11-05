using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EventsStudentRecordFacade : Controller
    {
        EventsStudentRecordInterface _interface;
        public EventsStudentRecordFacade(EventsStudentRecordInterface interfaces)
        {
            _interface = interfaces;
        }
        [Route("getDetails")]
        public Task<EventsStudentRecordDTO> getDetails([FromBody]EventsStudentRecordDTO data)
        {
            return _interface.getDetails(data);
        }
        [Route("getevent")]
        public EventsStudentRecordDTO getevent([FromBody]EventsStudentRecordDTO data)
        {
            return _interface.getevent(data);
        }

        [Route("save")]
        public Task<EventsStudentRecordDTO> save([FromBody]EventsStudentRecordDTO data)
        {
            return _interface.saveRecord(data);
        }
        //UpdateStudentSRKVS
        [Route("UpdateStudentSRKVS")]
        public Task<EventsStudentRecordDTO> UpdateStudentSRKVS([FromBody]EventsStudentRecordDTO data)
        {
            return _interface.UpdateStudentSRKVS(data);
        }

        [Route("MasterDeleteEventsStudent")]
        public Task<EventsStudentRecordDTO> MasterDeleteEventsStudent([FromBody]EventsStudentRecordDTO data)
        {
            return _interface.MasterDeleteEventsStudent(data);
        }

        //saveRecordSRKVS
        [Route("saveRecordSRKVS")]
        public Task<EventsStudentRecordDTO> saveRecordSRKVS([FromBody]EventsStudentRecordDTO data)
        {
            return _interface.saveRecordSRKVS(data);
        }
        //EditDetailsSRKVS
        [Route("EditDetailsSRKVS")]
        public Task<EventsStudentRecordDTO> EditDetailsSRKVS([FromBody]EventsStudentRecordDTO data)
        {
            return _interface.EditDetailsSRKVS(data);
        }
        [Route("EditDetails")]
        public Task<EventsStudentRecordDTO> EditDetails([FromBody]EventsStudentRecordDTO data)
        {
            return _interface.EditDetails(data);
        }
        [Route("editdata")]
        public Task<EventsStudentRecordDTO> editdata([FromBody]EventsStudentRecordDTO data)
        {
            return _interface.editdata(data);
        }


        [Route("deactivate")]
        public EventsStudentRecordDTO deactivate([FromBody]EventsStudentRecordDTO data)
        {
            return _interface.deactivate(data);
        }

        [Route("getStudents")]
        public Task<EventsStudentRecordDTO> getStudents([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.getStudents(dto);
        }

        [Route("onChangeActivities")]
        public EventsStudentRecordDTO onChangeActivities([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.onChangeActivities(dto);
        }

        [Route("onhousechage")]
        public EventsStudentRecordDTO onhousechage([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.onhousechage(dto);
        }

        [Route("get_student_info")]
        public EventsStudentRecordDTO get_student_info([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.get_student_info(dto);
        }

        [Route("get_modeldata")]
        public EventsStudentRecordDTO get_modeldata([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.get_modeldata(dto);
        }

        [Route("get_SportsName")]
        public EventsStudentRecordDTO get_SportsName([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.get_SportsName(dto);
        }

        [Route("get_uom_Name")]
        public EventsStudentRecordDTO get_uom_Name([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.get_uom_Name(dto);
        }

        [Route("Deactivatestud")]
        public EventsStudentRecordDTO Deactivatestud([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.Deactivatestud(dto);
        }

        [Route("classChange")]
        public EventsStudentRecordDTO classChange([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.classChange(dto);
        }

        [Route("get_class_house")]
        public EventsStudentRecordDTO get_class_house([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.get_class_house(dto);
        }


        [Route("get_StudentAgeFilter")]
        public Task<EventsStudentRecordDTO> get_StudentAgeFilter([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.get_StudentAgeFilter(dto);
        }

        [Route("houseWiseCompStudentList")]
        public Task<EventsStudentRecordDTO> houseWiseCompStudentList([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.houseWiseCompStudentList(dto);
        }

        [Route("classWiseCompStudentList")]
        public Task<EventsStudentRecordDTO> classWiseCompStudentList([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.classWiseCompStudentList(dto);
        }

        [Route("sectionWiseCompStudentList")]
        public Task<EventsStudentRecordDTO> sectionWiseCompStudentList([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.sectionWiseCompStudentList(dto);
        }

        [Route("get_houseCatAgeFilter")]
        public Task<EventsStudentRecordDTO> get_houseCatAgeFilter([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.get_houseCatAgeFilter(dto);
        }

        [Route("comcatwise_classAgefilter")]
        public Task<EventsStudentRecordDTO> comcatwise_classAgefilter([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.comcatwise_classAgefilter(dto);
        }

        [Route("houseWiseCompcatClssSectStudentList")]
        public Task<EventsStudentRecordDTO> houseWiseCompcatClssSectStudentList([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.houseWiseCompcatClssSectStudentList(dto);
        }


        [Route("KioskSportsWinners")]
        public EventsStudentRecordDTO.SportsWinnersDTO KioskSportsWinners([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.kioskSportsWinners(dto);
        }

        [Route("get_ComCatgrylistClassWise")]
        public Task<EventsStudentRecordDTO> get_ComCatgrylistClassWise([FromBody] EventsStudentRecordDTO dto)
        {
            return _interface.get_ComCatgrylistClassWise(dto);
        }

    }
}
