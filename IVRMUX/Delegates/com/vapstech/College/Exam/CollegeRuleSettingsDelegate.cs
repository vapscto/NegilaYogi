using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class CollegeRuleSettingsDelegate
    {
        CommonDelegate<CollegeRuleSettingsDTO, CollegeRuleSettingsDTO> _comm = new CommonDelegate<CollegeRuleSettingsDTO, CollegeRuleSettingsDTO>();

        public CollegeRuleSettingsDTO getalldetails(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/getalldetails");
        }
        public CollegeRuleSettingsDTO getbranch(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/getbranch");
        }
        public CollegeRuleSettingsDTO get_semesters(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/get_semesters");
        }
        public CollegeRuleSettingsDTO get_subjectscheme(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/get_subjectscheme");
        }
        public CollegeRuleSettingsDTO get_schemetype(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/get_schemetype");
        }
        public CollegeRuleSettingsDTO get_subjects(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/get_subjects");
        }
        public CollegeRuleSettingsDTO saveddata(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/saveddata");
        }
        public CollegeRuleSettingsDTO getalldetailsviewrecords(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/getalldetailsviewrecords");
        }
        public CollegeRuleSettingsDTO viewrecordspopup_subgrps(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/viewrecordspopup_subgrps");
        }
        public CollegeRuleSettingsDTO getalldetailsviewrecords_sub_grp_exms(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/getalldetailsviewrecords_sub_grp_exms");
        }
        public CollegeRuleSettingsDTO editdeatils(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/editdeatils");
        }
        public CollegeRuleSettingsDTO deactivate(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/deactivate");
        }
        public CollegeRuleSettingsDTO deactivatesubject(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/deactivatesubject");
        }
        public CollegeRuleSettingsDTO deactivategroup(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/deactivategroup");
        }
        public CollegeRuleSettingsDTO deactivateexam(CollegeRuleSettingsDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeRuleSettingsFacade/deactivateexam");
        }



    }
    
}
