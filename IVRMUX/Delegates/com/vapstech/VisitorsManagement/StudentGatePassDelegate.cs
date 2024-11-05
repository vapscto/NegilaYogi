using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class StudentGatePassDelegate
    {
        CommonDelegate<StudentGatePass_DTO, StudentGatePass_DTO> COMSPRT = new CommonDelegate<StudentGatePass_DTO, StudentGatePass_DTO>();

        public StudentGatePass_DTO getdetails(StudentGatePass_DTO data)
        {
            return COMSPRT.POSTDataVisitors(data, "StudentGatePassFacade/getdetails/");
        }

        public StudentGatePass_DTO get_class(StudentGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StudentGatePassFacade/get_class/");
        }

        public StudentGatePass_DTO get_section(StudentGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StudentGatePassFacade/get_section/");
        }

        public StudentGatePass_DTO get_student(StudentGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StudentGatePassFacade/get_student/");
        }
        public StudentGatePass_DTO saverecord(StudentGatePass_DTO data)
        {
            return COMSPRT.POSTDataVisitors(data, "StudentGatePassFacade/saverecord/");
        }
        public StudentGatePass_DTO editrecord(StudentGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StudentGatePassFacade/editrecord/");
        }
        public StudentGatePass_DTO deactive(StudentGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StudentGatePassFacade/deactive/");
        }
        public StudentGatePass_DTO checkstudentdata(StudentGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StudentGatePassFacade/checkstudentdata/");
        }
        public StudentGatePass_DTO get_otpverification(StudentGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StudentGatePassFacade/get_otpverification/");
        }

        public StudentGatePass_DTO resendotp(StudentGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StudentGatePassFacade/resendotp/");
        }
        public StudentGatePass_DTO get_otpverification22(StudentGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StudentGatePassFacade/get_otpverification22/");
        }
        public StudentGatePass_DTO printbutton(StudentGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StudentGatePassFacade/printbutton/");
        }

         public StudentGatePass_DTO GetStudDetails(StudentGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StudentGatePassFacade/GetStudDetails/");
        }
        public StudentGatePass_DTO getotp(StudentGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StudentGatePassFacade/getotp/");
        }

        
    }
}
