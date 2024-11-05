using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.SeatingArrangment;

namespace IVRMUX.Delegates.SeatingArrangment
{
    public class School_Exam_Date_RoomDelegate
    {
        CommonDelegate<School_Exam_Date_RoomDTO, School_Exam_Date_RoomDTO> _comm = new CommonDelegate<School_Exam_Date_RoomDTO, School_Exam_Date_RoomDTO>();

        public School_Exam_Date_RoomDTO GetExamDateRoomloaddata(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/GetExamDateRoomloaddata");
        }
        public School_Exam_Date_RoomDTO GetSearchExamDateRoomData(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/GetSearchExamDateRoomData");
        }
        public School_Exam_Date_RoomDTO SaveExamDateRoomData(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/SaveExamDateRoomData");
        }
        public School_Exam_Date_RoomDTO EditExamDateRoomData(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/EditExamDateRoomData");
        }
        public School_Exam_Date_RoomDTO ViewExamDateRoomDetails(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/ViewExamDateRoomDetails");
        }
        public School_Exam_Date_RoomDTO ActiveDeactiveExamRoomDate(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/ActiveDeactiveExamRoomDate");
        }
        public School_Exam_Date_RoomDTO ActiveDeactiveExamDateRoomDetails(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/ActiveDeactiveExamDateRoomDetails");
        }

        //Room Date Class Subject Mapping
        public School_Exam_Date_RoomDTO GetExamDateRoomClassMappingloaddata(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/GetExamDateRoomClassMappingloaddata");
        }
        public School_Exam_Date_RoomDTO GetSubjectListRoomClassMapping(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/GetSubjectListRoomClassMapping");
        }
        public School_Exam_Date_RoomDTO GetSearchExamDateRoomClassMappingData(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/GetSearchExamDateRoomClassMappingData");
        }
        public School_Exam_Date_RoomDTO SaveExamDateRoomClassMappingData(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/SaveExamDateRoomClassMappingData");
        }
        public School_Exam_Date_RoomDTO EditExamDateRoomClassMappingData(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/EditExamDateRoomClassMappingData");
        }
        public School_Exam_Date_RoomDTO ViewExamDateRoomClassMappingDetails(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/ViewExamDateRoomClassMappingDetails");
        }
        public School_Exam_Date_RoomDTO ActiveDeactiveExamRoomClassMappingDate(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/ActiveDeactiveExamRoomClassMappingDate");
        }
        public School_Exam_Date_RoomDTO ActiveDeactiveExamDateRoomClassMappingDetails(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/ActiveDeactiveExamDateRoomClassMappingDetails");
        }

        // Seating Arrangment Allotment
        public School_Exam_Date_RoomDTO SchoolSAAllotmentloaddata(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/SchoolSAAllotmentloaddata");
        }
        public School_Exam_Date_RoomDTO SchoolGenerateSeatAllotment(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/SchoolGenerateSeatAllotment");
        }

        // Seat Allotment Report
        public School_Exam_Date_RoomDTO GetSeatAllotedReport(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/GetSeatAllotedReport");
        }
        public School_Exam_Date_RoomDTO GetSchoolSeatAllotementReport(School_Exam_Date_RoomDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "School_Exam_Date_RoomFacade/GetSchoolSeatAllotementReport");
        }
    }
}
