using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class StudentActiveInactiveReportDelegate
    {
        CommonDelegate<StudentActiveInactiveReportDTO, StudentActiveInactiveReportDTO> _comm = new CommonDelegate<StudentActiveInactiveReportDTO, StudentActiveInactiveReportDTO>();

        public StudentActiveInactiveReportDTO getdata(StudentActiveInactiveReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "StudentActiveInactiveReportFacade/getdata/");
        }
        public StudentActiveInactiveReportDTO onacademicyearchange(StudentActiveInactiveReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "StudentActiveInactiveReportFacade/onacademicyearchange/");
        }
        public StudentActiveInactiveReportDTO oncoursechange(StudentActiveInactiveReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "StudentActiveInactiveReportFacade/oncoursechange/");
        }
        public StudentActiveInactiveReportDTO onbranchchange(StudentActiveInactiveReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "StudentActiveInactiveReportFacade/onbranchchange/");
        }
        public StudentActiveInactiveReportDTO onchangesemester(StudentActiveInactiveReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "StudentActiveInactiveReportFacade/onchangesemester/");
        }
        public StudentActiveInactiveReportDTO search(StudentActiveInactiveReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "StudentActiveInactiveReportFacade/search/");
        }
        public StudentActiveInactiveReportDTO savedata(StudentActiveInactiveReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "StudentActiveInactiveReportFacade/savedata/");
        }
        public StudentActiveInactiveReportDTO getreport(StudentActiveInactiveReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "StudentActiveInactiveReportFacade/getreport/");
        }
    }
}
