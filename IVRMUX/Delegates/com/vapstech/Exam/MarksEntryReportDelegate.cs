using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;

namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class MarksEntryReportDelegate
    {
        CommonDelegate<MarksEntryReportDTO, MarksEntryReportDTO> _comm = new CommonDelegate<MarksEntryReportDTO, MarksEntryReportDTO>();

        public MarksEntryReportDTO Getdetails(MarksEntryReportDTO data)
        {
            return _comm.POSTDataExam(data, "MarksEntryReportFacade/Getdetails");
        }
        public MarksEntryReportDTO get_class(MarksEntryReportDTO data)
        {
            return _comm.POSTDataExam(data, "MarksEntryReportFacade/get_class");
        }
        public MarksEntryReportDTO get_section(MarksEntryReportDTO data)
        {
            return _comm.POSTDataExam(data, "MarksEntryReportFacade/get_section");
        }
        public MarksEntryReportDTO get_exam(MarksEntryReportDTO data)
        {
            return _comm.POSTDataExam(data, "MarksEntryReportFacade/get_exam");
        }
        public MarksEntryReportDTO get_report(MarksEntryReportDTO data)
        {
            return _comm.POSTDataExam(data, "MarksEntryReportFacade/get_report");
        }
        public MarksEntryReportDTO get_markspublishreport(MarksEntryReportDTO data)
        {
            return _comm.POSTDataExam(data, "MarksEntryReportFacade/get_markspublishreport");
        }


    }
}
