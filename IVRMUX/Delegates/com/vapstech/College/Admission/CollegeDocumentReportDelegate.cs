using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeDocumentReportDelegate
    {
        CommonDelegate<CollegeDocumentReportDTO, CollegeDocumentReportDTO> comm = new CommonDelegate<CollegeDocumentReportDTO, CollegeDocumentReportDTO>();

        public CollegeDocumentReportDTO getdetails(CollegeDocumentReportDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegeDocumentReportFacade/getdetails");
        }
        public CollegeDocumentReportDTO onchangeyear(CollegeDocumentReportDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegeDocumentReportFacade/onchangeyear");
        }
        public CollegeDocumentReportDTO onchangecourse(CollegeDocumentReportDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegeDocumentReportFacade/onchangecourse");
        }
        public CollegeDocumentReportDTO onchangebranch(CollegeDocumentReportDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegeDocumentReportFacade/onchangebranch");
        }
        public CollegeDocumentReportDTO onchangesemester(CollegeDocumentReportDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegeDocumentReportFacade/onchangesemester");
        }
        public CollegeDocumentReportDTO onchangesection(CollegeDocumentReportDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegeDocumentReportFacade/onchangesection");
        }
        public CollegeDocumentReportDTO getreportdetails(CollegeDocumentReportDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegeDocumentReportFacade/getreportdetails");
        }

        public CollegeDocumentReportDTO getdetails_view(CollegeDocumentReportDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegeDocumentReportFacade/getdetails_view/");
        }

        public CollegeDocumentReportDTO getclgstudata_view(CollegeDocumentReportDTO CollegePreadmissionstudnetDto)
        {
            return comm.clgadmissionbypost(CollegePreadmissionstudnetDto, "CollegeDocumentReportFacade/getclgstudata_view/");
        }
    }
}
