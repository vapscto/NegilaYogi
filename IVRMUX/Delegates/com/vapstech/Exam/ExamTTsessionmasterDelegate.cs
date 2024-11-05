using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Exam
{
    public class ExamTTsessionmasterDelegate
    {
        CommonDelegate<ExamTTsessionmasterDTO, ExamTTsessionmasterDTO> commtt = new CommonDelegate<ExamTTsessionmasterDTO, ExamTTsessionmasterDTO>();
        public ExamTTsessionmasterDTO Getdetails(ExamTTsessionmasterDTO data)
        {
            return commtt.POSTDataExam(data, "ExamTTsessionmasterFacade/Getdetails/");
        }
        public ExamTTsessionmasterDTO savedetails(ExamTTsessionmasterDTO data)
        {
            return commtt.POSTDataExam(data, "ExamTTsessionmasterFacade/savedetails/");
        }
        public ExamTTsessionmasterDTO editdetails(ExamTTsessionmasterDTO data)
        {
            return commtt.POSTDataExam(data, "ExamTTsessionmasterFacade/editdetails/");
        }
        public ExamTTsessionmasterDTO deactivate(ExamTTsessionmasterDTO data)
        {
            return commtt.POSTDataExam(data, "ExamTTsessionmasterFacade/deactivate/");
        }
        

    }
}
