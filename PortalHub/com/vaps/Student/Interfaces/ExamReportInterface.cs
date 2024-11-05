using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface ExamReportInterface
    {
        ExamDTO getloaddata(ExamDTO data);
        Task<ExamDTO> getexamdata(ExamDTO data);
        ExamDTO getSubjects(ExamDTO sddto);
        Task<ExamDTO> StudentExamDetails(ExamDTO dto);
    }
}
