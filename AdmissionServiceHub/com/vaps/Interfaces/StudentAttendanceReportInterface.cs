using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface StudentAtttendanceReportInterface
    {
        StudentAttendanceReportDTO getInitailData(StudentAttendanceReportDTO id);
        Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO getDataByTypeSelected(int id);
        Task<StudentAttendanceReportDTO> getdatatype(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO getreportdiv(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO savetmpldatanew(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO onchangeyear(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO onclasschange(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO getclass(StudentAttendanceReportDTO data);
        
        StudentAttendanceReportDTO onsectionchange(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO getreport(StudentAttendanceReportDTO data);

        // Subject wise attendance report
        StudentAttendanceReportDTO LoadData(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO OnChangeAcademicYear(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO OnChangeClass(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO OnChangeSection(StudentAttendanceReportDTO data);        
        StudentAttendanceReportDTO OnReport(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO PeriodWiseReportOverAll(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO OnAttendanceLoadData(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO OnAttendanceChangeYear(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO OnAttendanceChangeClass(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO OnAttendanceChangeSection(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO GetAttendanceDeletedReport(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO getstudetails(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO OnsendSMS(StudentAttendanceReportDTO data);

        StudentAttendanceReportDTO OnChangeSectionAbsent(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO OnChangeClassAbsent(StudentAttendanceReportDTO data);
       

    }
}
