using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Exam
{
    public class ExamsubjectGroupMappingDelegate 
    {
        CommonDelegate<ExamsubjectGroupMappingDTo, ExamsubjectGroupMappingDTo> _comm = new CommonDelegate<ExamsubjectGroupMappingDTo, ExamsubjectGroupMappingDTo>();
         
        public ExamsubjectGroupMappingDTo Getdetails(ExamsubjectGroupMappingDTo id)
        {
            return _comm.POSTDataExam(id, "ExamsubjectGroupMappingFacade/Getdetails/");
        }
        public ExamsubjectGroupMappingDTo getcategory(ExamsubjectGroupMappingDTo id)
        {
            return _comm.POSTDataExam(id, "ExamsubjectGroupMappingFacade/getcategory/");
        }
        public ExamsubjectGroupMappingDTo getexam(ExamsubjectGroupMappingDTo id)
        {
            return _comm.POSTDataExam(id, "ExamsubjectGroupMappingFacade/getexam/");
        }
        public ExamsubjectGroupMappingDTo getsubject(ExamsubjectGroupMappingDTo id)
        {
            return _comm.POSTDataExam(id, "ExamsubjectGroupMappingFacade/getsubject/");
        }
        public ExamsubjectGroupMappingDTo savedetails(ExamsubjectGroupMappingDTo id)
        {
            return _comm.POSTDataExam(id, "ExamsubjectGroupMappingFacade/savedetails/");
        }
        public ExamsubjectGroupMappingDTo getlist(ExamsubjectGroupMappingDTo id)
        {
            return _comm.POSTDataExam(id, "ExamsubjectGroupMappingFacade/getlist/");
        }
        public ExamsubjectGroupMappingDTo Editexammasterdata1(ExamsubjectGroupMappingDTo id)
        {
            return _comm.POSTDataExam(id, "ExamsubjectGroupMappingFacade/Editexammasterdata1/");
        }
        public ExamsubjectGroupMappingDTo viewrecordspopup(ExamsubjectGroupMappingDTo id)
        {
            return _comm.POSTDataExam(id, "ExamsubjectGroupMappingFacade/viewrecordspopup/");
        }
        public ExamsubjectGroupMappingDTo deactivate(ExamsubjectGroupMappingDTo id)
        {
            return _comm.POSTDataExam(id, "ExamsubjectGroupMappingFacade/deactivate/");
        }
        

    }
}

