using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Student;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Student
{
    public class StudentDashboardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentDashboardDTO, StudentDashboardDTO> COMMM = new CommonDelegate<StudentDashboardDTO, StudentDashboardDTO>();
        public StudentDashboardDTO Getdetails(StudentDashboardDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentDashboardFacade/Getdetails/");
        }
        public StudentDashboardDTO saveakpkfile(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/saveakpkfile/");
        }
        public StudentDashboardDTO saverequest(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/saverequest/");
        }
        public StudentDashboardDTO getImages(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/getImages/");
        }
        public StudentDashboardDTO viewData(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/viewData/");
        }

        public StudentDashboardDTO savecls_doc(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/savecls_doc/");
        }
        public StudentDashboardDTO conformdata(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/conformdata/");
        }

        public StudentDashboardDTO savehome_doc(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/savehome_doc/");
        }

        public StudentDashboardDTO viewData_doc(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/viewData_doc/");
        }
        public StudentDashboardDTO viewnotice(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/viewnotice/");
        }
        public StudentDashboardDTO onclick_notice(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_notice/");
        }
        public StudentDashboardDTO onclick_TT(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_TT/");
        }
        public StudentDashboardDTO onclick_syllabus(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_syllabus/");
        }
        public StudentDashboardDTO onclick_LIB(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_LIB/");
        }
        public StudentDashboardDTO onclick_Homework(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_Homework/");
        }
        public StudentDashboardDTO onclick_Classwork(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_Classwork/");
        }
        public StudentDashboardDTO onclick_Sports(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_Sports/");
        }
        public StudentDashboardDTO onclick_Inventory(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_Inventory/");
        }
        public StudentDashboardDTO onclick_PDA(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_PDA/");
        }
        public StudentDashboardDTO onclick_Gallery(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_Gallery/");
        }
        public StudentDashboardDTO onclick_Homework_load(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_Homework_load/");
        }
        public StudentDashboardDTO onclick_Classwork_load(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_Classwork_load/");
        }
        public StudentDashboardDTO ViewStudentProfile(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/ViewStudentProfile/");
        }
        public StudentDashboardDTO ViewMonthWiseAttendance(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/ViewMonthWiseAttendance/");
        }
        public StudentDashboardDTO ViewYearWiseFee(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/ViewYearWiseFee/");
        }
        public StudentDashboardDTO ViewExamSubjectWiseDetails(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/ViewExamSubjectWiseDetails/");
        }
        public StudentDashboardDTO onclick_Homework_seen(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_Homework_seen/");
        }
        public StudentDashboardDTO onclick_classwork_seen(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_classwork_seen/");
        }

        public StudentDashboardDTO onclick_noticeboard_seen(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_noticeboard_seen/");
        }

        public StudentDashboardDTO onclick_Staff_details(StudentDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentDashboardFacade/onclick_Staff_details/");
        }
    }
}
