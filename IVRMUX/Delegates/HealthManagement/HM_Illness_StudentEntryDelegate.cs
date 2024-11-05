using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.HealthManagement;

namespace IVRMUX.Delegates.HealthManagement
{
    public class HM_Illness_StudentEntryDelegate
    {
        CommonDelegate<HM_Illness_StudentEntryDTO, HM_Illness_StudentEntryDTO> _delg = new CommonDelegate<HM_Illness_StudentEntryDTO, HM_Illness_StudentEntryDTO>();

        public HM_Illness_StudentEntryDTO LoadStudentIllnessData(HM_Illness_StudentEntryDTO data)
        {
            return _delg.HealthManagementPOST(data, "HM_Illness_StudentEntryFacade/LoadStudentIllnessData");
        }
        public HM_Illness_StudentEntryDTO SaveStudentIllnessData(HM_Illness_StudentEntryDTO data)
        {
            return _delg.HealthManagementPOST(data, "HM_Illness_StudentEntryFacade/SaveStudentIllnessData");
        }
        public HM_Illness_StudentEntryDTO EditStudentIllnessData(HM_Illness_StudentEntryDTO data)
        {
            return _delg.HealthManagementPOST(data, "HM_Illness_StudentEntryFacade/EditStudentIllnessData");
        }
        public HM_Illness_StudentEntryDTO ActiveDeactiveStudentIllnessData(HM_Illness_StudentEntryDTO data)
        {
            return _delg.HealthManagementPOST(data, "HM_Illness_StudentEntryFacade/ActiveDeactiveStudentIllnessData");
        }
        public HM_Illness_StudentEntryDTO GetStudentDetailsBySearch(HM_Illness_StudentEntryDTO data)
        {
            return _delg.HealthManagementPOST(data, "HM_Illness_StudentEntryFacade/GetStudentDetailsBySearch");
        }
        public HM_Illness_StudentEntryDTO OnStudentNameChange(HM_Illness_StudentEntryDTO data)
        {
            return _delg.HealthManagementPOST(data, "HM_Illness_StudentEntryFacade/OnStudentNameChange");
        }

        //Student Illness Report
        public HM_Illness_StudentEntryDTO LoadStudentIllnessReportData(HM_Illness_StudentEntryDTO data)
        {
            return _delg.HealthManagementPOST(data, "HM_Illness_StudentEntryFacade/LoadStudentIllnessReportData");
        }
        public HM_Illness_StudentEntryDTO ReportStudentIllnessData(HM_Illness_StudentEntryDTO data)
        {
            return _delg.HealthManagementPOST(data, "HM_Illness_StudentEntryFacade/ReportStudentIllnessData");
        }
        public HM_Illness_StudentEntryDTO ReportOnChangeYearData(HM_Illness_StudentEntryDTO data)
        {
            return _delg.HealthManagementPOST(data, "HM_Illness_StudentEntryFacade/ReportOnChangeYearData");
        }
    }
}
