using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class HallTicketGenerationCollegeDelgate
    {

        CommonDelegate<HallTicketGenerationCollegeDTO, HallTicketGenerationCollegeDTO> _comm = new CommonDelegate<HallTicketGenerationCollegeDTO, HallTicketGenerationCollegeDTO>();

        public HallTicketGenerationCollegeDTO getdetails(HallTicketGenerationCollegeDTO data)
        {
            return _comm.POSTcolExam(data, "HallTicketGenerationCollegeFacade/getdetails");
        }
        public HallTicketGenerationCollegeDTO onselectAcdYear(HallTicketGenerationCollegeDTO data)
        {
            return _comm.POSTcolExam(data, "HallTicketGenerationCollegeFacade/onselectAcdYear");
        }
        public HallTicketGenerationCollegeDTO onselectclass(HallTicketGenerationCollegeDTO data)
        {
            return _comm.POSTcolExam(data, "HallTicketGenerationCollegeFacade/onselectclass");
        }
        public HallTicketGenerationCollegeDTO onselectsection(HallTicketGenerationCollegeDTO data)
        {
            return _comm.POSTcolExam(data, "HallTicketGenerationCollegeFacade/onselectsection");
        }
        public HallTicketGenerationCollegeDTO savedetail(HallTicketGenerationCollegeDTO data)
        {
            return _comm.POSTcolExam(data, "HallTicketGenerationCollegeFacade/savedetail");
        }
        public HallTicketGenerationCollegeDTO ViewStudentDetails(HallTicketGenerationCollegeDTO data)
        {
            return _comm.POSTcolExam(data, "HallTicketGenerationCollegeFacade/ViewStudentDetails");
        }
        public HallTicketGenerationCollegeDTO SaveStudentStatus(HallTicketGenerationCollegeDTO data)
        {
            return _comm.POSTcolExam(data, "HallTicketGenerationCollegeFacade/SaveStudentStatus");
        }
        //ExamReport
        public HallTicketGenerationCollegeDTO ExamReport(HallTicketGenerationCollegeDTO data)
        {
            return _comm.POSTcolExam(data, "HallTicketGenerationCollegeFacade/ExamReport");
        }
        //HalticketSubject
        public HallTicketGenerationCollegeDTO HalticketSubject(HallTicketGenerationCollegeDTO data)
        {
            return _comm.POSTcolExam(data, "HallTicketGenerationCollegeFacade/HalticketSubject");
        }
        //savedetailHalticket
        public HallTicketGenerationCollegeDTO savedetailHalticket(HallTicketGenerationCollegeDTO data)
        {
            return _comm.POSTcolExam(data, "HallTicketGenerationCollegeFacade/savedetailHalticket");
        }
        //onedit
        public HallTicketGenerationCollegeDTO onedit(HallTicketGenerationCollegeDTO data)
        {
            return _comm.POSTcolExam(data, "HallTicketGenerationCollegeFacade/onedit");
        }
    }
}
