using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
    public interface CLGTTCommonInterface
    {
        CLGTTCommonDTO getBranch(CLGTTCommonDTO data);
        CLGTTCommonDTO getcourse_catg(CLGTTCommonDTO data);
        CLGTTCommonDTO getbranch_catg(CLGTTCommonDTO data);
        CLGTTCommonDTO multplegetbranch_catg(CLGTTCommonDTO data);
        CLGTTCommonDTO get_semister(CLGTTCommonDTO data);
        CLGTTCommonDTO multget_semister(CLGTTCommonDTO data);
        CLGTTCommonDTO get_section(CLGTTCommonDTO data);
        CLGTTCommonDTO get_staff(CLGTTCommonDTO data);
        CLGTTCommonDTO get_subject(CLGTTCommonDTO data);
        CLGTTCommonDTO get_subject_onsec(CLGTTCommonDTO data);
        CLGTTCommonDTO get_semday(CLGTTCommonDTO data);
        CLGTTCommonDTO get_staffaca(CLGTTCommonDTO data);
        CLGTTCommonDTO get_course_onstaff(CLGTTCommonDTO data);
        CLGTTCommonDTO get_branch_onstaff(CLGTTCommonDTO data);
        CLGTTCommonDTO get_sem_onstaff(CLGTTCommonDTO data);
        CLGTTCommonDTO get_sec_onstaff(CLGTTCommonDTO data);
        CLGTTCommonDTO get_subject_onstaff(CLGTTCommonDTO data);
        CLGTTCommonDTO get_subjecttab3(CLGTTCommonDTO data);
        CLGTTCommonDTO get_course_onsubject(CLGTTCommonDTO data);
        CLGTTCommonDTO get_branch_onsubject(CLGTTCommonDTO data);
        CLGTTCommonDTO get_sem_onsubject(CLGTTCommonDTO data);
        CLGTTCommonDTO get_sec_onsubject(CLGTTCommonDTO data);
        CLGTTCommonDTO get_staff_onsubject(CLGTTCommonDTO data);
    }
}
