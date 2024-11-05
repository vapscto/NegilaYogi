using PreadmissionDTOs.HealthManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthManagement.Interfaces
{
    public interface HM_Illness_StudentEntryInterface
    {
        HM_Illness_StudentEntryDTO LoadStudentIllnessData(HM_Illness_StudentEntryDTO data);
        HM_Illness_StudentEntryDTO SaveStudentIllnessData(HM_Illness_StudentEntryDTO data);
        HM_Illness_StudentEntryDTO EditStudentIllnessData(HM_Illness_StudentEntryDTO data);
        HM_Illness_StudentEntryDTO ActiveDeactiveStudentIllnessData(HM_Illness_StudentEntryDTO data);
        HM_Illness_StudentEntryDTO GetStudentDetailsBySearch(HM_Illness_StudentEntryDTO data);
        HM_Illness_StudentEntryDTO OnStudentNameChange(HM_Illness_StudentEntryDTO data);

        // Student Illness Report
        HM_Illness_StudentEntryDTO LoadStudentIllnessReportData(HM_Illness_StudentEntryDTO data);
        HM_Illness_StudentEntryDTO ReportStudentIllnessData(HM_Illness_StudentEntryDTO data);
        HM_Illness_StudentEntryDTO ReportOnChangeYearData(HM_Illness_StudentEntryDTO data);
    }
}
