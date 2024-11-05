using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface StudentAttendanceEntryInterface
    {
        StudentAttendanceEntryDTO GetInitialData(StudentAttendanceEntryDTO id);
        Task<StudentAttendanceEntryDTO> GetStudentData(StudentAttendanceEntryDTO attdto);
        Task<StudentAttendanceEntryDTO> SaveStudentAttendance(StudentAttendanceEntryDTO attdto);
        Task<StudentAttendanceEntryDTO> Deleteattendance(StudentAttendanceEntryDTO attdto);        
        Task<StudentAttendanceEntryDTO> getmonthclassheld(StudentAttendanceEntryDTO data);
        Task<StudentAttendanceEntryDTO> getdatewiseatt(StudentAttendanceEntryDTO data);
        StudentAttendanceEntryDTO ViewAttendanceDetailsStaffWise(StudentAttendanceEntryDTO data);
        StudentAttendanceEntryDTO AttendanceDeleteRecordWise(StudentAttendanceEntryDTO data);
        StudentAttendanceEntryDTO year(StudentAttendanceEntryDTO data);
        StudentAttendanceEntryDTO getbatchlist(StudentAttendanceEntryDTO data);
        StudentAttendanceEntryDTO getstdlistperiod(StudentAttendanceEntryDTO data);

        class_section_list getSmartCardData(class_section_list id);
        Task<class_section_list> SaveSmartCardData(class_section_list data);
        StudentAttendanceEntryDTO sendsmsabsent(StudentAttendanceEntryDTO data);
        StudentAttendanceEntryDTO saveattendancesmartcard(StudentAttendanceEntryDTO data);
        class_section_list getstudentdetailssmart(class_section_list data);
        RFIDDATA GETRFIDDATA(RFIDDATA data);

        StudentAttendanceEntryDTO studentattendanceinsert(StudentAttendanceEntryDTO data);
        
    }
}
