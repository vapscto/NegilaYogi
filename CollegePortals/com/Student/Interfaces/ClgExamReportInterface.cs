using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace CollegePortals.com.Student.Interfaces
{
    public interface ClgExamReportInterface
    {
        ClgExamDTO getloaddata(ClgExamDTO data);
        Task<ClgExamDTO> getexamdata(ClgExamDTO data);
        ClgExamDTO getSubjects(ClgExamDTO sddto);
        Task<ClgExamDTO> StudentExamDetails(ClgExamDTO dto);
    }
}
