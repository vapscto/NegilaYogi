using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
   public interface HallTicketGenerationCollege
    {
        HallTicketGenerationCollegeDTO getdetails(HallTicketGenerationCollegeDTO data);
        HallTicketGenerationCollegeDTO onselectAcdYear(HallTicketGenerationCollegeDTO data);
        HallTicketGenerationCollegeDTO onselectclass(HallTicketGenerationCollegeDTO data);
        HallTicketGenerationCollegeDTO onselectsection(HallTicketGenerationCollegeDTO data);
        HallTicketGenerationCollegeDTO savedetail(HallTicketGenerationCollegeDTO data);
        HallTicketGenerationCollegeDTO ViewStudentDetails(HallTicketGenerationCollegeDTO data);
        HallTicketGenerationCollegeDTO SaveStudentStatus(HallTicketGenerationCollegeDTO data);
        HallTicketGenerationCollegeDTO ExamReport(HallTicketGenerationCollegeDTO data);
        HallTicketGenerationCollegeDTO HalticketSubject(HallTicketGenerationCollegeDTO data);
        HallTicketGenerationCollegeDTO savedetailHalticket(HallTicketGenerationCollegeDTO data);
        HallTicketGenerationCollegeDTO onedit(HallTicketGenerationCollegeDTO data);
    }
}
