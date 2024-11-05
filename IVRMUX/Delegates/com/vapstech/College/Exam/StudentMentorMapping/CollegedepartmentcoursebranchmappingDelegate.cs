using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam.StudentMentorMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam.StudentMentorMapping
{
    public class CollegedepartmentcoursebranchmappingDelegate
    {
        CommonDelegate<CollegedepartmentcoursebranchmappingDTO, CollegedepartmentcoursebranchmappingDTO> _comm = new CommonDelegate<CollegedepartmentcoursebranchmappingDTO, CollegedepartmentcoursebranchmappingDTO>();

        public CollegedepartmentcoursebranchmappingDTO Getdetails(CollegedepartmentcoursebranchmappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegedepartmentcoursebranchmappingFacade/Getdetails");
        }
        public CollegedepartmentcoursebranchmappingDTO getbranch(CollegedepartmentcoursebranchmappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegedepartmentcoursebranchmappingFacade/getbranch");
        }
        public CollegedepartmentcoursebranchmappingDTO getsemester(CollegedepartmentcoursebranchmappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegedepartmentcoursebranchmappingFacade/getsemester");
        }
        public CollegedepartmentcoursebranchmappingDTO savedetails(CollegedepartmentcoursebranchmappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegedepartmentcoursebranchmappingFacade/savedetails");
        }
        public CollegedepartmentcoursebranchmappingDTO viewrecordspopup(CollegedepartmentcoursebranchmappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegedepartmentcoursebranchmappingFacade/viewrecordspopup");
        }
        public CollegedepartmentcoursebranchmappingDTO semesterdeactive(CollegedepartmentcoursebranchmappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegedepartmentcoursebranchmappingFacade/semesterdeactive");
        }
        public CollegedepartmentcoursebranchmappingDTO deactivate(CollegedepartmentcoursebranchmappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegedepartmentcoursebranchmappingFacade/deactivate");
        }
        public CollegedepartmentcoursebranchmappingDTO Getdetailsreport(CollegedepartmentcoursebranchmappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegedepartmentcoursebranchmappingFacade/Getdetailsreport");
        }
        public CollegedepartmentcoursebranchmappingDTO getreport(CollegedepartmentcoursebranchmappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegedepartmentcoursebranchmappingFacade/getreport");
        }

    }
}
