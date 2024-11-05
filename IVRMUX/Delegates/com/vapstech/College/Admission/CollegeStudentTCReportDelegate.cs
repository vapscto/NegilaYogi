using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeStudentTCReportDelegate
    {
        CommonDelegate<CollegeStudentTCReportDTO, CollegeStudentTCReportDTO> _comm = new CommonDelegate<CollegeStudentTCReportDTO, CollegeStudentTCReportDTO>();

        public CollegeStudentTCReportDTO getalldetails(CollegeStudentTCReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudentTCReportFacade/getalldetails");
        }
        public CollegeStudentTCReportDTO onchangeyear(CollegeStudentTCReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudentTCReportFacade/onchangeyear");
        }
        public CollegeStudentTCReportDTO onchangecourse(CollegeStudentTCReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudentTCReportFacade/onchangecourse");
        }
        public CollegeStudentTCReportDTO onchangebranch(CollegeStudentTCReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudentTCReportFacade/onchangebranch");
        }
        public CollegeStudentTCReportDTO onchangesemester(CollegeStudentTCReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudentTCReportFacade/onchangesemester");
        }
        public CollegeStudentTCReportDTO Getreportdetails(CollegeStudentTCReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudentTCReportFacade/Getreportdetails");
        }
        // TC Custom Report
        public CollegeStudentTCReportDTO onchangeyeartc(CollegeStudentTCReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudentTCReportFacade/onchangeyeartc");
        }
        public CollegeStudentTCReportDTO stdnamechange(CollegeStudentTCReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudentTCReportFacade/stdnamechange");
        }
        public CollegeStudentTCReportDTO getTcdetails(CollegeStudentTCReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudentTCReportFacade/getTcdetails");
        }
        
    }
}
