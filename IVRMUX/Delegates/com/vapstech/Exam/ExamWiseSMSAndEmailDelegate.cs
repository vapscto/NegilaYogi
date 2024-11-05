using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;

namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class ExamWiseSMSAndEmailDelegate
    {
        CommonDelegate<ExamWiseSMSAndEmailDTO, ExamWiseSMSAndEmailDTO> _comm = new CommonDelegate<ExamWiseSMSAndEmailDTO, ExamWiseSMSAndEmailDTO>();

        public ExamWiseSMSAndEmailDTO loaddata(ExamWiseSMSAndEmailDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseSMSAndEmailFacade/loaddata");
        }
        public ExamWiseSMSAndEmailDTO getclass(ExamWiseSMSAndEmailDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseSMSAndEmailFacade/getclass");
        }
        public ExamWiseSMSAndEmailDTO getsection(ExamWiseSMSAndEmailDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseSMSAndEmailFacade/getsection");
        }
        public ExamWiseSMSAndEmailDTO getexam(ExamWiseSMSAndEmailDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseSMSAndEmailFacade/getexam");
        }
        public ExamWiseSMSAndEmailDTO searchDetails(ExamWiseSMSAndEmailDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseSMSAndEmailFacade/searchDetails");
        }
        public ExamWiseSMSAndEmailDTO SendSmsEmail(ExamWiseSMSAndEmailDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseSMSAndEmailFacade/SendSmsEmail");
        }
    }
}
