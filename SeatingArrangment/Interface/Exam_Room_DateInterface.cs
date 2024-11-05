using PreadmissionDTOs.SeatingArrangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatingArrangment.Interface
{
    public interface Exam_Room_DateInterface
    {
        /* Room Transaction Exam Date With Student Allotment */
        Exam_Room_DateDTO GetExamDateloaddata(Exam_Room_DateDTO data);
        Exam_Room_DateDTO GetSearchExamDateData(Exam_Room_DateDTO data);
        Exam_Room_DateDTO SaveExamDateData(Exam_Room_DateDTO data);
        Exam_Room_DateDTO EditExamDateData(Exam_Room_DateDTO data);
        Exam_Room_DateDTO ViewRoomDetails(Exam_Room_DateDTO data);
        Exam_Room_DateDTO ActiveDeactiveExamDate(Exam_Room_DateDTO data);
        Exam_Room_DateDTO ActiveDeactiveRoomDetails(Exam_Room_DateDTO data);
        Exam_Room_DateDTO CheckCount(Exam_Room_DateDTO data);

        /* Room Sitting Style Details */
        Exam_Room_DateDTO GetRoomSittingStyleloaddata(Exam_Room_DateDTO data);
        Exam_Room_DateDTO SaveRoomSittingStyle(Exam_Room_DateDTO data);
        
    }
}
