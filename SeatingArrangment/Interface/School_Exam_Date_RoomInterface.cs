using PreadmissionDTOs.SeatingArrangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatingArrangment.Interface
{
    public interface School_Exam_Date_RoomInterface
    {
        School_Exam_Date_RoomDTO GetExamDateRoomloaddata(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO GetSearchExamDateRoomData(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO SaveExamDateRoomData(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO EditExamDateRoomData(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO ViewExamDateRoomDetails(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO ActiveDeactiveExamRoomDate(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO ActiveDeactiveExamDateRoomDetails(School_Exam_Date_RoomDTO data);

        School_Exam_Date_RoomDTO GetExamDateRoomClassMappingloaddata(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO GetSubjectListRoomClassMapping(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO GetSearchExamDateRoomClassMappingData(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO SaveExamDateRoomClassMappingData(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO EditExamDateRoomClassMappingData(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO ViewExamDateRoomClassMappingDetails(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO ActiveDeactiveExamRoomClassMappingDate(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO ActiveDeactiveExamDateRoomClassMappingDetails(School_Exam_Date_RoomDTO data);

        //Seating Arrangment Allotment
        School_Exam_Date_RoomDTO SchoolSAAllotmentloaddata(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO SchoolGenerateSeatAllotment(School_Exam_Date_RoomDTO data);

        // Seat Allotment Report
        School_Exam_Date_RoomDTO GetSeatAllotedReport(School_Exam_Date_RoomDTO data);
        School_Exam_Date_RoomDTO GetSchoolSeatAllotementReport(School_Exam_Date_RoomDTO data);
    }
}
