using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.SeatingArrangment;

namespace IVRMUX.Delegates.SeatingArrangment
{
    public class Exam_Room_DateDelegate
    {
        CommonDelegate<Exam_Room_DateDTO, Exam_Room_DateDTO> _comm = new CommonDelegate<Exam_Room_DateDTO, Exam_Room_DateDTO>();

        /* Room Transaction Exam Date With Student Allotment */
        public Exam_Room_DateDTO GetExamDateloaddata(Exam_Room_DateDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "Exam_Room_DateFacade/GetExamDateloaddata");
        }
        public Exam_Room_DateDTO GetSearchExamDateData(Exam_Room_DateDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "Exam_Room_DateFacade/GetSearchExamDateData");
        }
        public Exam_Room_DateDTO SaveExamDateData(Exam_Room_DateDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "Exam_Room_DateFacade/SaveExamDateData");
        }
        public Exam_Room_DateDTO EditExamDateData(Exam_Room_DateDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "Exam_Room_DateFacade/EditExamDateData");
        }
        public Exam_Room_DateDTO ViewRoomDetails(Exam_Room_DateDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "Exam_Room_DateFacade/ViewRoomDetails");
        }
        public Exam_Room_DateDTO ActiveDeactiveExamDate(Exam_Room_DateDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "Exam_Room_DateFacade/ActiveDeactiveExamDate");
        }
        public Exam_Room_DateDTO ActiveDeactiveRoomDetails(Exam_Room_DateDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "Exam_Room_DateFacade/ActiveDeactiveRoomDetails");
        }
        public Exam_Room_DateDTO CheckCount(Exam_Room_DateDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "Exam_Room_DateFacade/CheckCount");
        }

        /* Room Sitting Style Details */
        public Exam_Room_DateDTO GetRoomSittingStyleloaddata(Exam_Room_DateDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "Exam_Room_DateFacade/GetRoomSittingStyleloaddata");
        }
        public Exam_Room_DateDTO SaveRoomSittingStyle(Exam_Room_DateDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "Exam_Room_DateFacade/SaveRoomSittingStyle");
        }

    }
}
