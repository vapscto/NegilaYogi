using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    public class EventsStudentRecordDelegate
    {
        CommonDelegate<EventsStudentRecordDTO, EventsStudentRecordDTO> COMSPRT = new CommonDelegate<EventsStudentRecordDTO, EventsStudentRecordDTO>();

        public EventsStudentRecordDTO getDetails(EventsStudentRecordDTO data)
        {
            return COMSPRT.POSTDataSports(data, "EventsStudentRecordFacade/getDetails/");
        }
        public EventsStudentRecordDTO getevent(EventsStudentRecordDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventsStudentRecordFacade/getevent/");
        }
        public EventsStudentRecordDTO save(EventsStudentRecordDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventsStudentRecordFacade/save/");
        }
        //saveRecordSRKVS
        public EventsStudentRecordDTO saveRecordSRKVS(EventsStudentRecordDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventsStudentRecordFacade/saveRecordSRKVS/");
        }
        //UpdateStudentSRKVS
        public EventsStudentRecordDTO UpdateStudentSRKVS(EventsStudentRecordDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventsStudentRecordFacade/UpdateStudentSRKVS/");
        }

        public EventsStudentRecordDTO MasterDeleteEventsStudent(EventsStudentRecordDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventsStudentRecordFacade/MasterDeleteEventsStudent/");
        }
        public EventsStudentRecordDTO EditDetails(EventsStudentRecordDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventsStudentRecordFacade/EditDetails/");
        }
        //EditDetailsSRKVS
        public EventsStudentRecordDTO EditDetailsSRKVS(EventsStudentRecordDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventsStudentRecordFacade/EditDetailsSRKVS/");
        }
        public EventsStudentRecordDTO editdata(EventsStudentRecordDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventsStudentRecordFacade/editdata/");
        }
        public EventsStudentRecordDTO deactivate(EventsStudentRecordDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventsStudentRecordFacade/deactivate/");
        }
        public EventsStudentRecordDTO getStudents(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/getStudents/");
        }

        public EventsStudentRecordDTO onChangeActivities(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/onChangeActivities/");
        }

        public EventsStudentRecordDTO onhousechage(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/onhousechage/");
        }

        public EventsStudentRecordDTO get_student_info(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/get_student_info/");
        }

        public EventsStudentRecordDTO get_modeldata(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/get_modeldata/");
        }

        public EventsStudentRecordDTO get_SportsName(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/get_SportsName/");
        }

        public EventsStudentRecordDTO get_uom_Name(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/get_uom_Name/");
        }

        public EventsStudentRecordDTO Deactivatestud(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/Deactivatestud/");
        }

        public EventsStudentRecordDTO classChange(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/classChange/");
        }

        public EventsStudentRecordDTO get_class_house(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/get_class_house/");
        }

        public EventsStudentRecordDTO get_StudentAgeFilter(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/get_StudentAgeFilter/");
        }

        public EventsStudentRecordDTO houseWiseCompStudentList(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/houseWiseCompStudentList/");
        }

        public EventsStudentRecordDTO classWiseCompStudentList(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/classWiseCompStudentList/");
        }

        public EventsStudentRecordDTO sectionWiseCompStudentList(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/sectionWiseCompStudentList/");
        }

        public EventsStudentRecordDTO get_houseCatAgeFilter(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/get_houseCatAgeFilter/");
        }

        public EventsStudentRecordDTO comcatwise_classAgefilter(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/comcatwise_classAgefilter/");
        }
        public EventsStudentRecordDTO houseWiseCompcatClssSectStudentList(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/houseWiseCompcatClssSectStudentList/");
        }

        public EventsStudentRecordDTO get_ComCatgrylistClassWise(EventsStudentRecordDTO dto)
        {
            return COMSPRT.POSTDataSports(dto, "EventsStudentRecordFacade/get_ComCatgrylistClassWise/");
        }


    }
}
