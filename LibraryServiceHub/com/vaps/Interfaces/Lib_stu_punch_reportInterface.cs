using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
    public interface Lib_stu_punch_reportInterface
    {
        Lib_stu_punch_reportDTO Getdetails(Lib_stu_punch_reportDTO data);
        Lib_stu_punch_reportDTO get_classes(Lib_stu_punch_reportDTO data);
        Lib_stu_punch_reportDTO get_sections(Lib_stu_punch_reportDTO data);
        Lib_stu_punch_reportDTO get_students_category_grade(Lib_stu_punch_reportDTO data);
        Lib_stu_punch_reportDTO biometric_install(Lib_stu_punch_reportDTO data);
        //get_biometric_deviceclg
        Lib_stu_punch_reportDTO get_biometric_deviceclg(Lib_stu_punch_reportDTO data);
        Lib_stu_punch_reportDTO get_report(Lib_stu_punch_reportDTO data);
        Lib_stu_punch_reportDTO onloadpage(Lib_stu_punch_reportDTO data);
        Lib_stu_punch_reportDTO loadcourse(Lib_stu_punch_reportDTO data);
        Lib_stu_punch_reportDTO loadbranch(Lib_stu_punch_reportDTO data);
        Lib_stu_punch_reportDTO loadsemester(Lib_stu_punch_reportDTO data);
        Lib_stu_punch_reportDTO loaadsection(Lib_stu_punch_reportDTO data);
        Lib_stu_punch_reportDTO loadstudents(Lib_stu_punch_reportDTO data);
        Lib_stu_punch_reportDTO clgpunchreport(Lib_stu_punch_reportDTO data);
    }
}
