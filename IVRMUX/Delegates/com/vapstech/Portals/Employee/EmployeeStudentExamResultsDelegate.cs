using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class EmployeeStudentExamResultsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO> COMMM = new CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO>();

        public EmployeeDashboardDTO getalldetails(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentExamResultsFacade/getdata/");
        }
        public EmployeeDashboardDTO getstudentdetails(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentExamResultsFacade/getstudentdetails/");
        }
        public EmployeeDashboardDTO get_class(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentExamResultsFacade/get_class/");
        }
        public EmployeeDashboardDTO get_section(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentExamResultsFacade/get_section/");
        }
        public EmployeeDashboardDTO get_student(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentExamResultsFacade/get_student/");
        }
        public EmployeeDashboardDTO get_exam(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentExamResultsFacade/get_exam/");
        }
        public EmployeeDashboardDTO saveRemark(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentExamResultsFacade/saveRemark/");
        }
        public EmployeeDashboardDTO getremarkdetails(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentExamResultsFacade/getremarkdetails/");
        }
    }
}
