using PreadmissionDTOs.SeatingArrangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatingArrangment.Interface
{
    public interface School_Absent_Student_EntryInterface
    {
        School_Absent_Student_EntryDTO GetAbsentStudentLoadData(School_Absent_Student_EntryDTO data);
        School_Absent_Student_EntryDTO OnChangeYear(School_Absent_Student_EntryDTO data);
        School_Absent_Student_EntryDTO OnChangeClass(School_Absent_Student_EntryDTO data);
        School_Absent_Student_EntryDTO OnChangeSection(School_Absent_Student_EntryDTO data);
        School_Absent_Student_EntryDTO SearchData(School_Absent_Student_EntryDTO data);
        School_Absent_Student_EntryDTO SaveData(School_Absent_Student_EntryDTO data);

        // Absent Report
        School_Absent_Student_EntryDTO GetAbsentStudentReportLoadData(School_Absent_Student_EntryDTO data);
        School_Absent_Student_EntryDTO OnChangeYearAbsentReport(School_Absent_Student_EntryDTO data);
        School_Absent_Student_EntryDTO OnChangeClassAbsentReport(School_Absent_Student_EntryDTO data);
        School_Absent_Student_EntryDTO OnChangeSectionAbsentReport(School_Absent_Student_EntryDTO data);
        School_Absent_Student_EntryDTO GetSchoolAbsentStudentReport(School_Absent_Student_EntryDTO data);

    }
}
