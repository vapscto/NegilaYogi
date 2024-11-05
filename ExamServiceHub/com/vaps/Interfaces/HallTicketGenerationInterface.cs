using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface HallTicketGenerationInterface
    {
        HallTicketGenerationDTO getdetails(HallTicketGenerationDTO data);
        HallTicketGenerationDTO onselectAcdYear(HallTicketGenerationDTO data);
        HallTicketGenerationDTO onselectclass(HallTicketGenerationDTO data);
        HallTicketGenerationDTO onselectsection(HallTicketGenerationDTO data);
        HallTicketGenerationDTO savedetail(HallTicketGenerationDTO data);
        HallTicketGenerationDTO ViewStudentDetails(HallTicketGenerationDTO data);
        HallTicketGenerationDTO SaveStudentStatus(HallTicketGenerationDTO data);

    }
}
