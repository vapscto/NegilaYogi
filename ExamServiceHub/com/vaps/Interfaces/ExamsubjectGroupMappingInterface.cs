using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamsubjectGroupMappingInterface
    {
        ExamsubjectGroupMappingDTo Getdetails(ExamsubjectGroupMappingDTo data);
        ExamsubjectGroupMappingDTo getcategory(ExamsubjectGroupMappingDTo data);
        ExamsubjectGroupMappingDTo getexam(ExamsubjectGroupMappingDTo data);
        ExamsubjectGroupMappingDTo getsubject(ExamsubjectGroupMappingDTo data);
        ExamsubjectGroupMappingDTo savedetails(ExamsubjectGroupMappingDTo data);
        ExamsubjectGroupMappingDTo getlist(ExamsubjectGroupMappingDTo data);
        ExamsubjectGroupMappingDTo Editexammasterdata1(ExamsubjectGroupMappingDTo data);
        ExamsubjectGroupMappingDTo viewrecordspopup(ExamsubjectGroupMappingDTo data);
        ExamsubjectGroupMappingDTo deactivate(ExamsubjectGroupMappingDTo data);
        
    }
}
