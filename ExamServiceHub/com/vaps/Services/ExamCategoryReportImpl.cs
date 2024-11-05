using System.Collections.Generic;
using System.Linq;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.Extensions.Logging;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamCategoryReportImpl : Interfaces.ExamCategoryReportInterface
    {
        private static ConcurrentDictionary<string, ExamCategoryReportDTO> _login =
         new ConcurrentDictionary<string, ExamCategoryReportDTO>();

        private readonly ExamContext _examcateReportContext;
        ILogger<ExamCategoryReportImpl> _acdimpl;        
        public ExamCategoryReportImpl(ExamContext cpContext,  ILogger<ExamCategoryReportImpl> _acdim)
        {
            _examcateReportContext = cpContext;             
            _acdimpl = _acdim;
        }

        public ExamCategoryReportDTO getdetails(ExamCategoryReportDTO data)//int IVRMM_Id
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examcateReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                List<Exm_Master_CategoryDMO> grlist = new List<Exm_Master_CategoryDMO>();
                grlist = _examcateReportContext.Exm_Master_CategoryDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.grlist = grlist.ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public ExamCategoryReportDTO getAttendetails(ExamCategoryReportDTO data)//int IVRMM_Id
        {
            try
            {
                List<Exm_Master_CategoryDMO> grlist = new List<Exm_Master_CategoryDMO>();
                grlist = _examcateReportContext.Exm_Master_CategoryDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.grlist = grlist.ToArray();
                if (data.type == "2")
                {
                    data.studentAttendanceList = (from b in _examcateReportContext.Exm_Yearly_CategoryDMO
                                                  from a in _examcateReportContext.Exm_Master_CategoryDMO
                                                  from c in _examcateReportContext.Exm_Yearly_Category_GroupDMO
                                                  from d in _examcateReportContext.Exm_Yearly_Category_Group_SubjectsDMO
                                                  from e in _examcateReportContext.Exm_Master_GroupDMO
                                                  from f in _examcateReportContext.Exm_Master_Group_SubjectsDMO
                                                  from g in _examcateReportContext.IVRM_School_Master_SubjectsDMO
                                                  where (a.MI_Id == data.MI_Id && a.EMCA_ActiveFlag == true && a.EMCA_Id == data.EMCA_Id && b.MI_Id == a.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == a.EMCA_Id && b.EYC_ActiveFlg == true && c.EYC_Id == b.EYC_Id && c.EYCG_ActiveFlg == true && d.EYCG_Id == c.EYCG_Id && d.EYCGS_ActiveFlg == true && e.MI_Id == a.MI_Id && e.EMG_ActiveFlag == true && e.EMG_Id == c.EMG_Id && f.EMGS_ActiveFlag == true && f.EMG_Id == e.EMG_Id && f.ISMS_Id == d.ISMS_Id && g.MI_Id == a.MI_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == f.ISMS_Id)
                                                  select new ExamCategoryReportDTO
                                                  {
                                                      EMCA_Id = a.EMCA_Id,
                                                      EMCA_CategoryName = a.EMCA_CategoryName,
                                                      EMG_Id = e.EMG_Id,
                                                      EMG_GroupName = e.EMG_GroupName,
                                                      ISMS_Id = g.ISMS_Id,
                                                      ISMS_SubjectName = g.ISMS_SubjectName,
                                                      ISMS_OrderFlag = g.ISMS_OrderFlag
                                                  }).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }
                else if (data.type == "1")
                {
                    data.studentAttendanceList = (from b in _examcateReportContext.Exm_Yearly_CategoryDMO
                                                  from a in _examcateReportContext.Exm_Master_CategoryDMO
                                                  from c in _examcateReportContext.Exm_Yearly_Category_GroupDMO
                                                  from d in _examcateReportContext.Exm_Yearly_Category_Group_SubjectsDMO
                                                  from e in _examcateReportContext.Exm_Master_GroupDMO
                                                  from f in _examcateReportContext.Exm_Master_Group_SubjectsDMO
                                                  from g in _examcateReportContext.IVRM_School_Master_SubjectsDMO
                                                  where (a.MI_Id == data.MI_Id && a.EMCA_ActiveFlag == true && b.MI_Id == a.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == a.EMCA_Id && b.EYC_ActiveFlg == true && c.EYC_Id == b.EYC_Id && c.EYCG_ActiveFlg == true && d.EYCG_Id == c.EYCG_Id && d.EYCGS_ActiveFlg == true && e.MI_Id == a.MI_Id && e.EMG_ActiveFlag == true && e.EMG_Id == c.EMG_Id && f.EMGS_ActiveFlag == true && f.EMG_Id == e.EMG_Id && f.ISMS_Id == d.ISMS_Id && g.MI_Id == a.MI_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == f.ISMS_Id)
                                                  select new ExamCategoryReportDTO
                                                  {
                                                      EMCA_Id = a.EMCA_Id,
                                                      EMCA_CategoryName = a.EMCA_CategoryName,
                                                      EMG_Id = e.EMG_Id,
                                                      EMG_GroupName = e.EMG_GroupName,
                                                      ISMS_Id = g.ISMS_Id,
                                                      ISMS_SubjectName = g.ISMS_SubjectName,
                                                      ISMS_OrderFlag = g.ISMS_OrderFlag
                                                  }).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }

                data.masterinstitution = _examcateReportContext.Institution_master.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
    }
}
