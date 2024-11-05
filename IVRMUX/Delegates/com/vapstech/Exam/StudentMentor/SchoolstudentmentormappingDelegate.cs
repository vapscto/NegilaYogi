using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam.StudentMentor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam.StudentMentor
{
    public class SchoolstudentmentormappingDelegate
    {
        CommonDelegate<SchoolstudentmentormappingDTO, SchoolstudentmentormappingDTO> _comm = new CommonDelegate<SchoolstudentmentormappingDTO, SchoolstudentmentormappingDTO>();

        public SchoolstudentmentormappingDTO Getdetails(SchoolstudentmentormappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolstudentmentormappingFacade/Getdetails");
        }

        public SchoolstudentmentormappingDTO onchangeyear(SchoolstudentmentormappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolstudentmentormappingFacade/onchangeyear");
        }
        public SchoolstudentmentormappingDTO getsection(SchoolstudentmentormappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolstudentmentormappingFacade/getsection");
        }
        public SchoolstudentmentormappingDTO getemployee(SchoolstudentmentormappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolstudentmentormappingFacade/getemployee");
        }
        public SchoolstudentmentormappingDTO getstudentdata(SchoolstudentmentormappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolstudentmentormappingFacade/getstudentdata");
        }
        public SchoolstudentmentormappingDTO savedata(SchoolstudentmentormappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolstudentmentormappingFacade/savedata");
        }
        public SchoolstudentmentormappingDTO viewrecordspopup(SchoolstudentmentormappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolstudentmentormappingFacade/viewrecordspopup");
        }
        public SchoolstudentmentormappingDTO Deletedata(SchoolstudentmentormappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolstudentmentormappingFacade/Deletedata");
        }
        public SchoolstudentmentormappingDTO onreport(SchoolstudentmentormappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolstudentmentormappingFacade/onreport");
        }

    }
}
