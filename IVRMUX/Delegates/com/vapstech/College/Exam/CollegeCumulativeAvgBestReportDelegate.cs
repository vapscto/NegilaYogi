using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class CollegeCumulativeAvgBestReportDelegate
    {
        CommonDelegate<CollegeCumulativeAvgBestReportDTO, CollegeCumulativeAvgBestReportDTO> _comm = new CommonDelegate<CollegeCumulativeAvgBestReportDTO, CollegeCumulativeAvgBestReportDTO>();


        public CollegeCumulativeAvgBestReportDTO Getdetails(CollegeCumulativeAvgBestReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeCumulativeAvgBestReportFacade/Getdetails");
        }
        public CollegeCumulativeAvgBestReportDTO onchangeyear(CollegeCumulativeAvgBestReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeCumulativeAvgBestReportFacade/onchangeyear");
        }
        public CollegeCumulativeAvgBestReportDTO onchangecourse(CollegeCumulativeAvgBestReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeCumulativeAvgBestReportFacade/onchangecourse");
        }
        public CollegeCumulativeAvgBestReportDTO onchangebranch(CollegeCumulativeAvgBestReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeCumulativeAvgBestReportFacade/onchangebranch");
        }
        public CollegeCumulativeAvgBestReportDTO onchangesemester(CollegeCumulativeAvgBestReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeCumulativeAvgBestReportFacade/onchangesemester");
        }
        public CollegeCumulativeAvgBestReportDTO onchangesubjectscheme(CollegeCumulativeAvgBestReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeCumulativeAvgBestReportFacade/onchangesubjectscheme");
        }
        public CollegeCumulativeAvgBestReportDTO onchangeschemetype(CollegeCumulativeAvgBestReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeCumulativeAvgBestReportFacade/onchangeschemetype");
        }
        public CollegeCumulativeAvgBestReportDTO Getcmreport(CollegeCumulativeAvgBestReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeCumulativeAvgBestReportFacade/Getcmreport");
        }
        public CollegeCumulativeAvgBestReportDTO getindreport(CollegeCumulativeAvgBestReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeCumulativeAvgBestReportFacade/getindreport");
        }
    }
}
