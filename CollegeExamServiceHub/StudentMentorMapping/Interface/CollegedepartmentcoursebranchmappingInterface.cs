using PreadmissionDTOs.com.vaps.College.Exam.StudentMentorMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.StudentMentorMapping.Interface
{
    public interface CollegedepartmentcoursebranchmappingInterface
    {
        CollegedepartmentcoursebranchmappingDTO Getdetails(CollegedepartmentcoursebranchmappingDTO data);
        CollegedepartmentcoursebranchmappingDTO getbranch(CollegedepartmentcoursebranchmappingDTO data);
        CollegedepartmentcoursebranchmappingDTO getsemester(CollegedepartmentcoursebranchmappingDTO data);
        CollegedepartmentcoursebranchmappingDTO savedetails(CollegedepartmentcoursebranchmappingDTO data);
        CollegedepartmentcoursebranchmappingDTO viewrecordspopup(CollegedepartmentcoursebranchmappingDTO data);
        CollegedepartmentcoursebranchmappingDTO semesterdeactive(CollegedepartmentcoursebranchmappingDTO data);
        CollegedepartmentcoursebranchmappingDTO deactivate(CollegedepartmentcoursebranchmappingDTO data);
        CollegedepartmentcoursebranchmappingDTO Getdetailsreport(CollegedepartmentcoursebranchmappingDTO data);
        CollegedepartmentcoursebranchmappingDTO getreport(CollegedepartmentcoursebranchmappingDTO data);
        

    }
}
