using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface JSHSPortal_StudentReportsInterface
    {
        JSHSPortal_StudentReportsDTO Getdetails_IT(JSHSPortal_StudentReportsDTO data);
        JSHSPortal_StudentReportsDTO get_Terms_IT(JSHSPortal_StudentReportsDTO data);
        JSHSPortal_StudentReportsDTO get_reportdetails_IT(JSHSPortal_StudentReportsDTO data);
        JSHSPortal_StudentReportsDTO get_Exam_grade_pc(JSHSPortal_StudentReportsDTO data);
         Task<JSHSPortal_ProgressCardReportDTO> saveddata_pc(JSHSPortal_ProgressCardReportDTO data);
       
    }
}
