using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class CollegeExamGeneralReportImpl : Interfaces.CollegeExamGeneralReportInterface
    {
        public ClgExamContext _examcontext;
        public ILogger<CollegeExamGeneralReportImpl> _logger;

        public CollegeExamGeneralReportImpl(ClgExamContext _exam, ILogger<CollegeExamGeneralReportImpl> _log)
        {
            _examcontext = _exam;
            _logger = _log;
        }
        public CollegeExamGeneralReportDTO MasterGradeReportLoadData(CollegeExamGeneralReportDTO data)
        {
            try
            {
                data.MasterGradeList = _examcontext.Exm_Master_GradeDMO.Where(t => t.MI_Id == data.MI_Id && t.EMGR_ActiveFlag == true).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeExamGeneralReportDTO MasterGradeReportDetails(CollegeExamGeneralReportDTO data)
        {
            try
            {
                var grlist = _examcontext.Exm_Master_GradeDMO.Where(t => t.MI_Id == data.MI_Id).ToList();

                var gradedetails = (from a in _examcontext.Exm_Master_GradeDMO
                                    from b in _examcontext.Exm_Master_Grade_DetailsDMO
                                    where (a.MI_Id == data.MI_Id && a.EMGR_Id == b.EMGR_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true)
                                    select b).Distinct().ToList();

                if (data.reporttype == "2")
                {
                    gradedetails = gradedetails.Where(a => a.EMGR_Id == data.EMGR_Id).ToList();
                    grlist = grlist.Where(a => a.EMGR_Id == data.EMGR_Id).ToList();
                }

                data.GradeList = grlist.ToArray();
                data.GradeListDetails = gradedetails.ToArray();

                data.MasterInstitution = _examcontext.Institution_master.Where(a => a.MI_Id == data.MI_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
