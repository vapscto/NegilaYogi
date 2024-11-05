using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface StudentDashboardInterface
    {
        Task<StudentDashboardDTO> Getdetails(StudentDashboardDTO sddto);
        StudentDashboardDTO saveakpkfile(StudentDashboardDTO data);
        StudentDashboardDTO saverequest(StudentDashboardDTO data);
        Task<StudentDashboardDTO> getImages(StudentDashboardDTO data);
        StudentDashboardDTO viewData(StudentDashboardDTO data);
        StudentDashboardDTO conformdata(StudentDashboardDTO data);
        StudentDashboardDTO savecls_doc(StudentDashboardDTO data);
        StudentDashboardDTO savehome_doc(StudentDashboardDTO data);
        StudentDashboardDTO viewData_doc(StudentDashboardDTO data);
        StudentDashboardDTO viewnotice(StudentDashboardDTO data);
        StudentDashboardDTO onclick_notice(StudentDashboardDTO data);
        StudentDashboardDTO onclick_TT(StudentDashboardDTO data);
        StudentDashboardDTO onclick_syllabus(StudentDashboardDTO data);
        StudentDashboardDTO onclick_LIB(StudentDashboardDTO data);
        StudentDashboardDTO onclick_Homework(StudentDashboardDTO data);
        StudentDashboardDTO onclick_Classwork(StudentDashboardDTO data);
        StudentDashboardDTO onclick_Sports(StudentDashboardDTO data);
        StudentDashboardDTO onclick_Inventory(StudentDashboardDTO data);
        StudentDashboardDTO onclick_PDA(StudentDashboardDTO data);
        StudentDashboardDTO onclick_Gallery(StudentDashboardDTO data);
        StudentDashboardDTO onclick_Homework_load(StudentDashboardDTO data);
        StudentDashboardDTO onclick_Classwork_load(StudentDashboardDTO data);
        StudentDashboardDTO ViewStudentProfile(StudentDashboardDTO data);
        StudentDashboardDTO ViewMonthWiseAttendance(StudentDashboardDTO data);
        StudentDashboardDTO ViewYearWiseFee(StudentDashboardDTO data);
        StudentDashboardDTO ViewExamSubjectWiseDetails(StudentDashboardDTO data);

        StudentDashboardDTO onclick_Homework_seen(StudentDashboardDTO data);
        StudentDashboardDTO onclick_classwork_seen(StudentDashboardDTO data);

        StudentDashboardDTO onclick_noticeboard_seen(StudentDashboardDTO data);

        StudentDashboardDTO onclick_Homework_datewise(StudentDashboardDTO data);
        StudentDashboardDTO onclick_classwork_datewise(StudentDashboardDTO data);

        StudentDashboardDTO onclick_noticeboard_datewise(StudentDashboardDTO data);
        StudentDashboardDTO onclick_notice_datewise(StudentDashboardDTO data);

        StudentDashboardDTO onclick_Staff_details(StudentDashboardDTO data);
    }
}