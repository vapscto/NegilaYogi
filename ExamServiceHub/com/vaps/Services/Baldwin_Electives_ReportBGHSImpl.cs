using ExamServiceHub.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.Exam;

namespace ExamServiceHub.com.vaps.Services
{
    public class Baldwin_Electives_ReportImpl : Baldwin_Electives_ReportInterface
    {
        private readonly ExamContext _examcontext;
        ILogger<Baldwin_Final_P_ReportImpl> _acdimpl;
        public Baldwin_Electives_ReportImpl(ExamContext exm, ILogger<Baldwin_Final_P_ReportImpl> _acdim)
        {
            _examcontext = exm;
            _acdimpl = _acdim;
        }
        public Baldwin_Electives_ReportDTO Getdetails(Baldwin_Electives_ReportDTO data)
        {
            try
            {
                data.yearlist = _examcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id
                && t.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.categorylist = _examcontext.Exm_Master_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.EMCA_ActiveFlag == true).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }

        public Baldwin_Electives_ReportDTO get_categories(Baldwin_Electives_ReportDTO data)
        {
            try
            {
                data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == data.MI_Id && a.EMCA_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                     && b.EMCA_Id == a.EMCA_Id && b.EYC_ActiveFlg == true)
                                     select a).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public Baldwin_Electives_ReportDTO get_groups(Baldwin_Electives_ReportDTO data)
        {
            try
            {
                data.grouplist = (from a in _examcontext.Exm_Master_GroupDMO
                                  from b in _examcontext.Exm_Yearly_CategoryDMO
                                  from c in _examcontext.Exm_Yearly_Category_GroupDMO
                                  where (a.MI_Id == data.MI_Id && a.EMG_ActiveFlag == true && a.EMG_ElectiveFlg == true && b.MI_Id == data.MI_Id
                                  && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.EYC_ActiveFlg == true && c.EYCG_ActiveFlg == true
                                  && c.EYC_Id == b.EYC_Id && c.EMG_Id == a.EMG_Id)
                                  select new Exm_Master_GroupDMO
                                  {
                                      EMG_Id = a.EMG_Id,
                                      EMG_GroupName = a.EMG_GroupName,
                                      EMG_TotSubjects = a.EMG_TotSubjects,
                                      EMG_MaxAplSubjects = a.EMG_MaxAplSubjects,
                                      EMG_MinAplSubjects = a.EMG_MinAplSubjects,
                                      EMG_BestOff = a.EMG_BestOff,
                                      EMG_ElectiveFlg = a.EMG_ElectiveFlg,
                                      EMG_ActiveFlag = a.EMG_ActiveFlag,
                                  }).Distinct().ToArray();

                data.classlist = (from a in _examcontext.AdmissionClass
                                  from b in _examcontext.Exm_Category_ClassDMO
                                  where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.ECAC_ActiveFlag == true && a.MI_Id == b.MI_Id && a.ASMCL_ActiveFlag == true && a.ASMCL_Id == b.ASMCL_Id)
                                  select a).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public Baldwin_Electives_ReportDTO get_subjects(Baldwin_Electives_ReportDTO data)
        {
            try
            {
                data.subjectlist = (from a in _examcontext.IVRM_School_Master_SubjectsDMO
                                    from b in _examcontext.Exm_Yearly_CategoryDMO
                                    from c in _examcontext.Exm_Yearly_Category_GroupDMO
                                    from d in _examcontext.Exm_Yearly_Category_Group_SubjectsDMO
                                    where (a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1 && a.ISMS_ExamFlag == 1 && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.EYC_ActiveFlg == true && c.EYCG_ActiveFlg == true && c.EYC_Id == b.EYC_Id && c.EMG_Id == data.EMG_Id && d.EYCG_Id == c.EYCG_Id && d.EYCGS_ActiveFlg == true && d.ISMS_Id == a.ISMS_Id)
                                    select new IVRM_School_Master_SubjectsDMO
                                    {
                                        ISMS_Id = a.ISMS_Id,
                                        ISMS_SubjectName = a.ISMS_SubjectName,
                                        ISMS_SubjectCode = a.ISMS_SubjectCode
                                    }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public Baldwin_Electives_ReportDTO get_sections(Baldwin_Electives_ReportDTO data)
        {
            try
            {
                data.sectionlist = (from a in _examcontext.School_M_Section
                                    from b in _examcontext.Exm_Category_ClassDMO
                                    where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.ECAC_ActiveFlag == true && a.MI_Id == b.MI_Id && a.ASMC_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && b.ASMCL_Id == data.ASMCL_Id)
                                    select a).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public Baldwin_Electives_ReportDTO get_report(Baldwin_Electives_ReportDTO data)
        {
            try
            {
                data.instituelist = _examcontext.Institution_master.Where(a => a.MI_Id == data.MI_Id).ToArray();

                List<int?> ids = new List<int?>();
                if (data.Left_Flag == true || data.Deactive_Flag == true)
                {
                    ids.Add(0);
                }

                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");

                if (data.Left_Flag == true)
                {
                    sol.Add("L");
                }
                if (data.Deactive_Flag == true)
                {
                    sol.Add("D");
                }

                if (data.Type == "All")
                {
                    data.EMCA_Id = 0;
                    data.grouplist = _examcontext.Exm_Master_GroupDMO.Where(t => t.MI_Id == data.MI_Id && t.EMG_ActiveFlag == true && t.EMG_ElectiveFlg == true).Distinct().ToArray();

                    var AMST_Ids = _examcontext.StudentMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ESTSU_ActiveFlg == true && t.ESTSU_ElecetiveFlag == true).Select(t => t.AMST_Id).Distinct().ToList();

                    data.studentlist = (from a in _examcontext.Adm_M_Student
                                        from b in _examcontext.School_Adm_Y_StudentDMO
                                        from d in _examcontext.AdmissionClass
                                        from e in _examcontext.School_M_Section
                                        where (a.MI_Id == data.MI_Id && sol.Contains(a.AMST_SOL) && ids.Contains(a.AMST_ActiveFlag) && b.AMST_Id == a.AMST_Id
                                        && b.ASMAY_Id == data.ASMAY_Id && ids.Contains(b.AMAY_ActiveFlag) && d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true
                                        && d.ASMCL_Id == b.ASMCL_Id && e.MI_Id == data.MI_Id && e.ASMC_ActiveFlag == 1 && e.ASMS_Id == b.ASMS_Id
                                        && AMST_Ids.Contains(a.AMST_Id))
                                        select new Baldwin_Electives_ReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)).Trim(),
                                            AMST_AdmNo = a.AMST_AdmNo == null ? "" : a.AMST_AdmNo,
                                            AMAY_RollNo = b.AMAY_RollNo,
                                            AMST_DOB = a.AMST_DOB,
                                            AMST_MobileNo = a.AMST_MobileNo,
                                            AMST_emailId = a.AMST_emailId,
                                            ASMCL_Id = b.ASMCL_Id,
                                            ASMCL_ClassName = d.ASMCL_ClassName,
                                            ASMCL_Order = d.ASMCL_Order,
                                            ASMS_Id = b.ASMS_Id,
                                            ASMC_SectionName = e.ASMC_SectionName,
                                            ASMC_Order = e.ASMC_Order
                                        }).Distinct().OrderBy(t => t.ASMCL_Order).ThenBy(t => t.ASMC_Order).ThenBy(t => t.AMAY_RollNo).ToArray();

                    data.studentsubj_list = (from a in _examcontext.StudentMappingDMO
                                             from b in _examcontext.Exm_Master_GroupDMO
                                             from d in _examcontext.IVRM_School_Master_SubjectsDMO
                                             from e in _examcontext.School_Adm_Y_StudentDMO
                                             from f in _examcontext.Adm_M_Student
                                             where (a.AMST_Id == e.AMST_Id && e.AMST_Id == f.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id
                                             && a.ASMS_Id == e.ASMS_Id && e.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                             && a.ESTSU_ElecetiveFlag == true && a.ESTSU_ActiveFlg == true && b.MI_Id == a.MI_Id && b.EMG_ActiveFlag == true
                                             && b.EMG_ElectiveFlg == true && b.EMG_Id == a.EMG_Id && d.MI_Id == data.MI_Id && d.ISMS_ActiveFlag == 1
                                             && d.ISMS_ExamFlag == 1 && d.ISMS_Id == a.ISMS_Id && AMST_Ids.Contains(a.AMST_Id))
                                             select new Baldwin_Electives_ReportDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 EMG_Id = a.EMG_Id,
                                                 ISMS_Id = a.ISMS_Id,
                                                 EMG_GroupName = b.EMG_GroupName,
                                                 ISMS_SubjectName = d.ISMS_SubjectName,
                                                 EME_ExamName = a.EME_Id > 0 ? _examcontext.masterexam.Where(z => z.EME_Id == a.EME_Id 
                                                 && z.MI_Id == data.MI_Id).FirstOrDefault().EME_ExamName : "",
                                             }).Distinct().ToArray();

                }

                else if (data.Type == "Indi")
                {

                    data.grouplist = (from a in _examcontext.Exm_Master_GroupDMO
                                      from b in _examcontext.Exm_Yearly_CategoryDMO
                                      from c in _examcontext.Exm_Yearly_Category_GroupDMO
                                      where (a.MI_Id == data.MI_Id && a.EMG_ActiveFlag == true && a.EMG_ElectiveFlg == true && b.MI_Id == a.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EYC_ActiveFlg == true && b.EMCA_Id == data.EMCA_Id && c.EYCG_ActiveFlg == true && c.EYC_Id == b.EYC_Id && c.EMG_Id == a.EMG_Id)
                                      select new Exm_Master_GroupDMO
                                      {
                                          EMG_Id = a.EMG_Id,
                                          EMG_GroupName = a.EMG_GroupName,
                                          EMG_TotSubjects = a.EMG_TotSubjects,
                                          EMG_MaxAplSubjects = a.EMG_MaxAplSubjects,
                                          EMG_MinAplSubjects = a.EMG_MinAplSubjects,
                                          EMG_BestOff = a.EMG_BestOff,
                                          EMG_ElectiveFlg = a.EMG_ElectiveFlg,
                                          EMG_ActiveFlag = a.EMG_ActiveFlag,
                                      }).Distinct().ToArray();

                    var AMST_Ids = (from a in _examcontext.StudentMappingDMO
                                    from b in _examcontext.Exm_Category_ClassDMO
                                    from e in _examcontext.School_Adm_Y_StudentDMO
                                    from f in _examcontext.Adm_M_Student
                                    where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ESTSU_ActiveFlg == true
                                    && a.ESTSU_ElecetiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id
                                    && b.ECAC_ActiveFlag == true && a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id)
                                    select a.AMST_Id).Distinct().ToList();

                    data.studentlist = (from a in _examcontext.Adm_M_Student
                                        from b in _examcontext.School_Adm_Y_StudentDMO
                                        from c in _examcontext.Exm_Category_ClassDMO
                                        from d in _examcontext.AdmissionClass
                                        from e in _examcontext.School_M_Section
                                        where (a.MI_Id == data.MI_Id && sol.Contains(a.AMST_SOL) && ids.Contains(a.AMST_ActiveFlag) && b.AMST_Id == a.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && ids.Contains(b.AMAY_ActiveFlag) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.ECAC_ActiveFlag == true && c.EMCA_Id == data.EMCA_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true && d.ASMCL_Id == b.ASMCL_Id && e.MI_Id == data.MI_Id && e.ASMC_ActiveFlag == 1 && e.ASMS_Id == b.ASMS_Id && AMST_Ids.Contains(a.AMST_Id))
                                        select new Baldwin_Electives_ReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                            AMST_AdmNo = a.AMST_AdmNo == null ? "" : a.AMST_AdmNo,
                                            AMAY_RollNo = b.AMAY_RollNo,
                                            AMST_DOB = a.AMST_DOB,
                                            AMST_MobileNo = a.AMST_MobileNo,
                                            AMST_emailId = a.AMST_emailId,
                                            ASMCL_Id = b.ASMCL_Id,
                                            ASMCL_ClassName = d.ASMCL_ClassName,
                                            ASMCL_Order = d.ASMCL_Order,
                                            ASMS_Id = b.ASMS_Id,
                                            ASMC_SectionName = e.ASMC_SectionName,
                                            ASMC_Order = e.ASMC_Order
                                        }).Distinct().OrderBy(t => t.ASMCL_Order).ThenBy(t => t.ASMC_Order).ThenBy(t => t.AMAY_RollNo).ToArray();

                    data.studentsubj_list = (from a in _examcontext.StudentMappingDMO
                                             from b in _examcontext.Exm_Master_GroupDMO
                                             from d in _examcontext.IVRM_School_Master_SubjectsDMO
                                             from e in _examcontext.School_Adm_Y_StudentDMO
                                             from f in _examcontext.Adm_M_Student
                                             where (a.AMST_Id == e.AMST_Id && e.AMST_Id == f.AMST_Id && e.ASMAY_Id == data.ASMAY_Id
                                             && a.ASMAY_Id == e.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                             && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ESTSU_ElecetiveFlag == true && a.ESTSU_ActiveFlg == true
                                             && b.MI_Id == a.MI_Id && b.EMG_ActiveFlag == true && b.EMG_ElectiveFlg == true
                                             && b.EMG_Id == a.EMG_Id && d.MI_Id == data.MI_Id && d.ISMS_ActiveFlag == 1 && d.ISMS_ExamFlag == 1
                                             && d.ISMS_Id == a.ISMS_Id && AMST_Ids.Contains(a.AMST_Id))
                                             select new Baldwin_Electives_ReportDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 EMG_Id = a.EMG_Id,
                                                 ISMS_Id = a.ISMS_Id,
                                                 EMG_GroupName = b.EMG_GroupName,
                                                 ISMS_SubjectName = d.ISMS_SubjectName,
                                                 EME_ExamName = a.EME_Id > 0 ? _examcontext.masterexam.Where(z => z.EME_Id == a.EME_Id
                                                && z.MI_Id == data.MI_Id).FirstOrDefault().EME_ExamName : "",
                                             }).Distinct().ToArray();
                }

                else if (data.Type == "subj")
                {
                    var AMST_Ids = (from a in _examcontext.StudentMappingDMO
                                    from b in _examcontext.Exm_Category_ClassDMO
                                    where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ESTSU_ActiveFlg == true && a.ESTSU_ElecetiveFlag == true
                                    && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.ECAC_ActiveFlag == true
                                    && a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.EMG_Id == data.EMG_Id && a.ISMS_Id == data.ISMS_Id)
                                    select a.AMST_Id).Distinct().ToList();

                    data.studentlist = (from a in _examcontext.Adm_M_Student
                                        from b in _examcontext.School_Adm_Y_StudentDMO
                                        from c in _examcontext.Exm_Category_ClassDMO
                                        from d in _examcontext.AdmissionClass
                                        from e in _examcontext.School_M_Section
                                        where (a.MI_Id == data.MI_Id && sol.Contains(a.AMST_SOL) && ids.Contains(a.AMST_ActiveFlag) && b.AMST_Id == a.AMST_Id
                                        && b.ASMAY_Id == data.ASMAY_Id && ids.Contains(b.AMAY_ActiveFlag) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id
                                        && c.ECAC_ActiveFlag == true && c.EMCA_Id == data.EMCA_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id
                                        && d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true && d.ASMCL_Id == b.ASMCL_Id && e.MI_Id == data.MI_Id
                                        && e.ASMC_ActiveFlag == 1 && e.ASMS_Id == b.ASMS_Id && AMST_Ids.Contains(a.AMST_Id))
                                        select new Baldwin_Electives_ReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                            AMST_AdmNo = a.AMST_AdmNo == null ? "" : a.AMST_AdmNo,
                                            AMAY_RollNo = b.AMAY_RollNo,
                                            AMST_DOB = a.AMST_DOB,
                                            AMST_MobileNo = a.AMST_MobileNo,
                                            AMST_emailId = a.AMST_emailId,
                                            ASMCL_Id = b.ASMCL_Id,
                                            ASMCL_ClassName = d.ASMCL_ClassName,
                                            ASMCL_Order = d.ASMCL_Order,
                                            ASMS_Id = b.ASMS_Id,
                                            ASMC_SectionName = e.ASMC_SectionName,
                                            ASMC_Order = e.ASMC_Order
                                        }).Distinct().OrderBy(t => t.ASMCL_Order).ThenBy(t => t.ASMC_Order).ThenBy(t => t.AMAY_RollNo).ToArray();

                    data.studentsubj_list = (from a in _examcontext.StudentMappingDMO
                                             from b in _examcontext.Exm_Master_GroupDMO
                                             from d in _examcontext.IVRM_School_Master_SubjectsDMO
                                             from e in _examcontext.School_Adm_Y_StudentDMO
                                             from f in _examcontext.Adm_M_Student
                                             where (a.AMST_Id == e.AMST_Id && e.AMST_Id == f.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id
                                             && a.ASMS_Id == e.ASMS_Id && e.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                             && a.ESTSU_ElecetiveFlag == true && a.ESTSU_ActiveFlg == true && b.MI_Id == a.MI_Id && b.EMG_ActiveFlag == true
                                             && b.EMG_ElectiveFlg == true && b.EMG_Id == a.EMG_Id && d.MI_Id == data.MI_Id && d.ISMS_ActiveFlag == 1
                                             && d.ISMS_ExamFlag == 1 && d.ISMS_Id == a.ISMS_Id && AMST_Ids.Contains(a.AMST_Id) && a.EMG_Id == data.EMG_Id
                                             && a.ISMS_Id == data.ISMS_Id)
                                             select new Baldwin_Electives_ReportDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 EMG_Id = a.EMG_Id,
                                                 ISMS_Id = a.ISMS_Id,
                                                 EMG_GroupName = b.EMG_GroupName,
                                                 ISMS_SubjectName = d.ISMS_SubjectName,
                                                 EME_ExamName = a.EME_Id > 0 ? _examcontext.masterexam.Where(z => z.EME_Id == a.EME_Id
                                                && z.MI_Id == data.MI_Id).FirstOrDefault().EME_ExamName : "",
                                             }).Distinct().ToArray();
                }

                else if (data.Type == "csw")
                {
                    data.grouplist = (from a in _examcontext.Exm_Master_GroupDMO
                                      from b in _examcontext.Exm_Yearly_CategoryDMO
                                      from c in _examcontext.Exm_Yearly_Category_GroupDMO
                                      from d in _examcontext.Exm_Category_ClassDMO
                                      where (a.MI_Id == data.MI_Id && a.EMG_ActiveFlag == true && a.EMG_ElectiveFlg == true && b.MI_Id == a.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EYC_ActiveFlg == true && b.EMCA_Id == data.EMCA_Id && c.EYCG_ActiveFlg == true && c.EYC_Id == b.EYC_Id && c.EMG_Id == a.EMG_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.EMCA_Id == data.EMCA_Id && d.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == data.ASMS_Id && d.ECAC_ActiveFlag == true && d.EMCA_Id == b.EMCA_Id)
                                      select new Exm_Master_GroupDMO
                                      {
                                          EMG_Id = a.EMG_Id,
                                          EMG_GroupName = a.EMG_GroupName,
                                          EMG_TotSubjects = a.EMG_TotSubjects,
                                          EMG_MaxAplSubjects = a.EMG_MaxAplSubjects,
                                          EMG_MinAplSubjects = a.EMG_MinAplSubjects,
                                          EMG_BestOff = a.EMG_BestOff,
                                          EMG_ElectiveFlg = a.EMG_ElectiveFlg,
                                          EMG_ActiveFlag = a.EMG_ActiveFlag,
                                      }).Distinct().ToArray();

                    var AMST_Ids = (from a in _examcontext.StudentMappingDMO
                                    from b in _examcontext.Exm_Category_ClassDMO
                                    where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ESTSU_ActiveFlg == true && a.ESTSU_ElecetiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.ECAC_ActiveFlag == true && a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                    select a.AMST_Id).Distinct().ToList();

                    data.studentlist = (from a in _examcontext.Adm_M_Student
                                        from b in _examcontext.School_Adm_Y_StudentDMO
                                        from c in _examcontext.Exm_Category_ClassDMO
                                        from d in _examcontext.AdmissionClass
                                        from e in _examcontext.School_M_Section
                                        where (a.MI_Id == data.MI_Id && sol.Contains(a.AMST_SOL) && ids.Contains(a.AMST_ActiveFlag) && b.AMST_Id == a.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && ids.Contains(b.AMAY_ActiveFlag) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.ECAC_ActiveFlag == true && c.EMCA_Id == data.EMCA_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true && d.ASMCL_Id == b.ASMCL_Id && e.MI_Id == data.MI_Id && e.ASMC_ActiveFlag == 1 && e.ASMS_Id == b.ASMS_Id && AMST_Ids.Contains(a.AMST_Id) && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                        select new Baldwin_Electives_ReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                            AMST_AdmNo = a.AMST_AdmNo == null ? "" : a.AMST_AdmNo,
                                            AMAY_RollNo = b.AMAY_RollNo,
                                            AMST_DOB = a.AMST_DOB,
                                            AMST_MobileNo = a.AMST_MobileNo,
                                            AMST_emailId = a.AMST_emailId,
                                            ASMCL_Id = b.ASMCL_Id,
                                            ASMCL_ClassName = d.ASMCL_ClassName,
                                            ASMCL_Order = d.ASMCL_Order,
                                            ASMS_Id = b.ASMS_Id,
                                            ASMC_SectionName = e.ASMC_SectionName,
                                            ASMC_Order = e.ASMC_Order
                                        }).Distinct().OrderBy(t => t.ASMCL_Order).ThenBy(t => t.ASMC_Order).ThenBy(t => t.AMAY_RollNo).ToArray();

                    data.studentsubj_list = (from a in _examcontext.StudentMappingDMO
                                             from b in _examcontext.Exm_Master_GroupDMO
                                             from d in _examcontext.IVRM_School_Master_SubjectsDMO
                                             from e in _examcontext.School_Adm_Y_StudentDMO
                                             from f in _examcontext.Adm_M_Student
                                             where (a.AMST_Id == e.AMST_Id && e.AMST_Id == f.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id
                                             && a.ASMS_Id == e.ASMS_Id && e.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                             && a.ESTSU_ElecetiveFlag == true && a.ESTSU_ActiveFlg == true && b.MI_Id == a.MI_Id && b.EMG_ActiveFlag == true
                                             && b.EMG_ElectiveFlg == true && b.EMG_Id == a.EMG_Id && d.MI_Id == data.MI_Id && d.ISMS_ActiveFlag == 1
                                             && d.ISMS_ExamFlag == 1 && d.ISMS_Id == a.ISMS_Id && AMST_Ids.Contains(a.AMST_Id) && a.ASMCL_Id == data.ASMCL_Id
                                             && a.ASMS_Id == data.ASMS_Id)
                                             select new Baldwin_Electives_ReportDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 EMG_Id = a.EMG_Id,
                                                 ISMS_Id = a.ISMS_Id,
                                                 EMG_GroupName = b.EMG_GroupName,
                                                 ISMS_SubjectName = d.ISMS_SubjectName,
                                                 EME_ExamName = a.EME_Id > 0 ? _examcontext.masterexam.Where(z => z.EME_Id == a.EME_Id
                                                && z.MI_Id == data.MI_Id).FirstOrDefault().EME_ExamName : "",
                                             }).Distinct().ToArray();
                }
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