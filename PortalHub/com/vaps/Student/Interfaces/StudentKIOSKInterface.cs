using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface StudentKIOSKInterface
    {
        Task<StudentKIOSKDTO> GetdetailsKiosk(StudentKIOSKDTO sddto);
        Task<StudentKIOSKDTO> GetAttendancedetailsKiosk(StudentKIOSKDTO data);
        StudentKioskExamTopperDTO showExamreport(StudentKioskExamTopperDTO data);
        StudentKioskCOEDTO getcoedata(StudentKioskCOEDTO data);
        StudentKioskSubjectDTO getSubjectsdata(StudentKioskSubjectDTO data);
        Task<StudentKioskEXAMDTO> StudentExamDetails(StudentKioskEXAMDTO data);
        Task<StudentKioskEXAMDTO> getexamdata(StudentKioskEXAMDTO data);
        Task<StudentKioskFEEDTO> getloaddataFEE(StudentKioskFEEDTO data);
        StudentKioskSPORTSDTO kioskSportsWinners(StudentKioskSPORTSDTO kiosk);
        Task<StudentKioskBIRTHDAYDTO> getstaffdetails(StudentKioskBIRTHDAYDTO data);
        Task<StudentKioskHomeWorkDTO> GetHomeWorkdetailsKiosk(StudentKioskHomeWorkDTO data);
        Task<StudentKioskNoticeDTO> GetNoticedetailsKiosk(StudentKioskNoticeDTO data);
        StudentKioskBIRTHDAYDTO getstudentBD(StudentKioskBIRTHDAYDTO stu1);
        Task<StudentKioskTimeTableDTO> getStudentTT(StudentKioskTimeTableDTO data);
        StudentKioskSubjectDTO getloadyear(StudentKioskSubjectDTO data);
    }

}
