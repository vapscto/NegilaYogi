
using PreadmissionDTOs.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.HRMS.Interface
{
    public interface HrmsNAACReportInterface
    {

        HRMS_NAAC_DTO getdetails(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO get_depts(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO get_desig(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO get_Employe_ob(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO SaveData(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getOrientdata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getStudentActivitydata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getProfessionalActivitydata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getResearchProjectdata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getResearchGuidedata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getBOSBOEdata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getJournaldata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getConferencedata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getBookdata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getBookChapterdata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getCommetteedata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getOtherDetaildata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO get_EmployeALLDATA(HRMS_NAAC_DTO data);
    }
}
