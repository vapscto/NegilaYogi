using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class CollegeBMCPUProgresscardReportDelegate
    {
        CommonDelegate<CollegeBMCPUProgresscardReportDTO, CollegeBMCPUProgresscardReportDTO> _comm = new CommonDelegate<CollegeBMCPUProgresscardReportDTO, CollegeBMCPUProgresscardReportDTO>();

        public CollegeBMCPUProgresscardReportDTO Getdetails(CollegeBMCPUProgresscardReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeBMCPUProgresscardReportFacade/Getdetails");
        }
        public CollegeBMCPUProgresscardReportDTO OnAcdyear(CollegeBMCPUProgresscardReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeBMCPUProgresscardReportFacade/OnAcdyear");
        }
        public CollegeBMCPUProgresscardReportDTO onchangecourse(CollegeBMCPUProgresscardReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeBMCPUProgresscardReportFacade/onchangecourse");
        }
        public CollegeBMCPUProgresscardReportDTO onchangebranch(CollegeBMCPUProgresscardReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeBMCPUProgresscardReportFacade/onchangebranch");
        }
        public CollegeBMCPUProgresscardReportDTO onchangesemester(CollegeBMCPUProgresscardReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeBMCPUProgresscardReportFacade/onchangesemester");
        }
        public CollegeBMCPUProgresscardReportDTO onchangesection(CollegeBMCPUProgresscardReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeBMCPUProgresscardReportFacade/onchangesection");
        }

        public CollegeBMCPUProgresscardReportDTO onchangesubjectscheme(CollegeBMCPUProgresscardReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeBMCPUProgresscardReportFacade/onchangesubjectscheme");
        }
        public CollegeBMCPUProgresscardReportDTO onchangeschemetype(CollegeBMCPUProgresscardReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeBMCPUProgresscardReportFacade/onchangeschemetype");
        }
        public CollegeBMCPUProgresscardReportDTO getreport(CollegeBMCPUProgresscardReportDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeBMCPUProgresscardReportFacade/getreport");
        }
    }
}
