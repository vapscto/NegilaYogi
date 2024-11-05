using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface EventsStudentRecordInterface
    {
        Task<EventsStudentRecordDTO> getDetails(EventsStudentRecordDTO data);
        Task<EventsStudentRecordDTO> saveRecord(EventsStudentRecordDTO obj);
        Task<EventsStudentRecordDTO> UpdateStudentSRKVS(EventsStudentRecordDTO obj);
        Task<EventsStudentRecordDTO> MasterDeleteEventsStudent(EventsStudentRecordDTO obj);
        Task<EventsStudentRecordDTO> saveRecordSRKVS(EventsStudentRecordDTO obj);
        Task<EventsStudentRecordDTO> EditDetails(EventsStudentRecordDTO data);
        Task<EventsStudentRecordDTO> EditDetailsSRKVS(EventsStudentRecordDTO data);
        Task<EventsStudentRecordDTO> editdata(EventsStudentRecordDTO data);
        EventsStudentRecordDTO getevent(EventsStudentRecordDTO obj);
        EventsStudentRecordDTO deactivate(EventsStudentRecordDTO obj);
        Task<EventsStudentRecordDTO> getStudents(EventsStudentRecordDTO obj);
        EventsStudentRecordDTO onChangeActivities(EventsStudentRecordDTO obj);
        EventsStudentRecordDTO onhousechage(EventsStudentRecordDTO obj);
        EventsStudentRecordDTO get_student_info(EventsStudentRecordDTO obj);
        EventsStudentRecordDTO get_modeldata(EventsStudentRecordDTO obj);
        EventsStudentRecordDTO get_SportsName(EventsStudentRecordDTO obj);
        EventsStudentRecordDTO get_uom_Name(EventsStudentRecordDTO obj);
        EventsStudentRecordDTO classChange(EventsStudentRecordDTO obj);
        EventsStudentRecordDTO Deactivatestud(EventsStudentRecordDTO obj);
        EventsStudentRecordDTO get_class_house(EventsStudentRecordDTO obj);
        Task<EventsStudentRecordDTO> get_StudentAgeFilter(EventsStudentRecordDTO data);
        Task<EventsStudentRecordDTO> houseWiseCompStudentList(EventsStudentRecordDTO data);

        Task<EventsStudentRecordDTO> classWiseCompStudentList(EventsStudentRecordDTO data);
        Task<EventsStudentRecordDTO> sectionWiseCompStudentList(EventsStudentRecordDTO data);
        Task<EventsStudentRecordDTO> get_houseCatAgeFilter(EventsStudentRecordDTO data);
        Task<EventsStudentRecordDTO> comcatwise_classAgefilter(EventsStudentRecordDTO data);
        Task<EventsStudentRecordDTO> houseWiseCompcatClssSectStudentList(EventsStudentRecordDTO data);
        Task<EventsStudentRecordDTO> get_ComCatgrylistClassWise(EventsStudentRecordDTO data);
        EventsStudentRecordDTO.SportsWinnersDTO kioskSportsWinners(EventsStudentRecordDTO kiosk);


    }
}
