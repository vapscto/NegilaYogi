using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class Lib_stu_punch_reportDelegate
    {
                CommonDelegate<Lib_stu_punch_reportDTO, Lib_stu_punch_reportDTO> _commnbranch = new CommonDelegate<Lib_stu_punch_reportDTO, Lib_stu_punch_reportDTO>();
        public Lib_stu_punch_reportDTO Getdetails(Lib_stu_punch_reportDTO data)
        {
            return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/Getdetails");
        }

        public Lib_stu_punch_reportDTO get_classes(Lib_stu_punch_reportDTO data)
        {
            return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/get_classes");
        }
        public Lib_stu_punch_reportDTO get_sections(Lib_stu_punch_reportDTO data)
        {
            return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/get_sections");
        }
        public Lib_stu_punch_reportDTO get_students_category_grade(Lib_stu_punch_reportDTO data)
        {
            return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/get_students_category_grade");
        }
        public Lib_stu_punch_reportDTO get_report(Lib_stu_punch_reportDTO data)
        {
            return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/get_report");
        }
        //public Lib_stu_punch_reportDTO get_biometric_device(Lib_stu_punch_reportDTO data)
        //{
        //    return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/get_biometric_device");
        //}

        //for college
        public Lib_stu_punch_reportDTO onloadpage(Lib_stu_punch_reportDTO data)
        {
            return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/onloadpage");
        }
        public Lib_stu_punch_reportDTO loadcourse(Lib_stu_punch_reportDTO data)
        {
            return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/loadcourse");
        }
        public Lib_stu_punch_reportDTO loadbranch(Lib_stu_punch_reportDTO data)
        {
            return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/loadbranch");
        }
        public Lib_stu_punch_reportDTO loadsemester(Lib_stu_punch_reportDTO data)
        {
            return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/loadsemester");
        }
        public Lib_stu_punch_reportDTO loaadsection(Lib_stu_punch_reportDTO data)
        {
            return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/loaadsection");
        }
        public Lib_stu_punch_reportDTO loadstudents(Lib_stu_punch_reportDTO data)
        {
            return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/loadstudents");
        }
        public Lib_stu_punch_reportDTO clgpunchreport(Lib_stu_punch_reportDTO data)
        {
            return _commnbranch.PostLibrary(data, "Lib_stu_punch_reportFacade/clgpunchreport");
        }
    }
}
