using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.mobile;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CommonServiceHub.Interfaces
{
    public interface LoginMinterface
    {
        StudentdetDTO getdetails(StudentdetDTO.input data);
        StudentFeeDetailsDTO stufeedetails(StudentFeeDetailsDTO.input data);
        Task<StudentFeeTermDTO> StudentFeeTerm(StudentFeeTermDTO.input data);
        COEDTO CalenderofEvents(COEDTO.input data);
        Task<StudentAttendanceDTO> getAttend(StudentAttendanceDTO.input data);
        Task<StudentYAttendanceDTO> getYAttend(StudentYAttendanceDTO.input data);
        ExamDTO.examid Examid(ExamDTO.input data);
        ExamDTO Examdetails(ExamDTO.input data);
        PaymentDetails generatehashsequence(OnlinePaymentDTO.input data);
        TTDTO Timetable(TTDTO.input data);
        EmpPortalDTO EmployeePortal_SalaryD(EmpPortalDTO.Input data);
        EmployeePunchDTO EmployeePortal_PunchD(EmployeePunchDTO.Input data);

        EmployeePortal_StudentAttrndenceDTO EmployeePortal_StudentAttrndence(EmployeePortal_StudentAttrndenceDTO.Input data);
        EmployeePortalTimeTableDTO EmployeePortalTimeTableD(EmployeePortalTimeTableDTO.Input data);
        EmployeePortalStudentReportCardDTO EmployeePortalStudentReportCard(EmployeePortalStudentReportCardDTO.Input data);
        EmployeePortalStudentSearchDTO EmployeePortalStudentSearchD(EmployeePortalStudentSearchDTO.Input data);
        EmployeePortalLeaveDTO EmployeePortalLeaveD(EmployeePortalLeaveDTO.Input data);
        EmployeeDTO EmployeePortalDetails(EmployeeDTO.input data);
        EmployeeloginDTO EmployeeDetails(EmployeeloginDTO.input data);

        EmployeeSalaryDTO EmployeesalaryDetails(EmployeeSalaryDTO.input data);
    }
}
