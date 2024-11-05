using PreadmissionDTOs.com.vaps.Exam.StudentMentor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.StudentMentor.Interface
{
    public interface SchoolstudentmentormappingInterface
    {
        SchoolstudentmentormappingDTO Getdetails(SchoolstudentmentormappingDTO data);
        SchoolstudentmentormappingDTO onchangeyear(SchoolstudentmentormappingDTO data);
        SchoolstudentmentormappingDTO getsection(SchoolstudentmentormappingDTO data);
        SchoolstudentmentormappingDTO getemployee(SchoolstudentmentormappingDTO data);
        SchoolstudentmentormappingDTO getstudentdata(SchoolstudentmentormappingDTO data);
        SchoolstudentmentormappingDTO savedata(SchoolstudentmentormappingDTO data);
        SchoolstudentmentormappingDTO viewrecordspopup(SchoolstudentmentormappingDTO data);
        SchoolstudentmentormappingDTO Deletedata(SchoolstudentmentormappingDTO data);
        SchoolstudentmentormappingDTO onreport(SchoolstudentmentormappingDTO data);
    }
}
