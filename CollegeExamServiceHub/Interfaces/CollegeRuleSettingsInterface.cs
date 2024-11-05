using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
    public interface CollegeRuleSettingsInterface
    {
        CollegeRuleSettingsDTO getalldetails(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO getbranch(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO get_semesters(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO get_subjectscheme(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO get_schemetype(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO get_subjects(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO saveddata(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO getalldetailsviewrecords(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO viewrecordspopup_subgrps(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO getalldetailsviewrecords_sub_grp_exms(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO editdeatils(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO deactivate(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO deactivatesubject(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO deactivategroup(CollegeRuleSettingsDTO data);
        CollegeRuleSettingsDTO deactivateexam(CollegeRuleSettingsDTO data);


    }
}
