using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamWiseRemarksReportImpl : Interfaces.ExamWiseRemarksReportInterface
    {
        public ExamContext _examContext;

        public ExamWiseRemarksReportImpl(ExamContext _exam)
        {
            _examContext = _exam;
        }

        public ExamWiseRemarksReportDTO LoadData(ExamWiseRemarksReportDTO data)
        {
            try
            {
                data.yearlist = _examContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamWiseRemarksReportDTO OnChangeYear(ExamWiseRemarksReportDTO data)
        {
            try
            {
                var getuserid = _examContext.Staff_User_Login.Where(a => a.Id == data.User_Id).ToList();

                if (getuserid.Count > 0)
                {
                    data.classname = (from a in _examContext.ClassTeacherMappingDMO
                                      from b in _examContext.AdmissionClass
                                      from c in _examContext.AcademicYear
                                      where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                      && a.HRME_Id == getuserid.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                      select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
                else
                {
                    data.classname = (from a in _examContext.Exm_Category_ClassDMO
                                      from b in _examContext.AdmissionClass
                                      where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true)
                                      select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamWiseRemarksReportDTO OnChangeClass(ExamWiseRemarksReportDTO data)
        {
            try
            {

                var getuserid = _examContext.Staff_User_Login.Where(a => a.Id == data.User_Id).ToList();

                if (getuserid.Count > 0)
                {
                    data.secname = (from a in _examContext.ClassTeacherMappingDMO
                                    from b in _examContext.AdmissionClass
                                    from c in _examContext.AcademicYear
                                    from d in _examContext.School_M_Section
                                    where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id
                                    && a.HRME_Id == getuserid.FirstOrDefault().Emp_Code && a.ASMCL_Id == data.ASMCL_Id && a.IMCT_ActiveFlag == true
                                    && b.ASMCL_ActiveFlag == true)
                                    select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
                else
                {
                    data.secname = (from a in _examContext.Exm_Category_ClassDMO
                                    from b in _examContext.AdmissionClass
                                    from c in _examContext.School_M_Section
                                    where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                    && a.ECAC_ActiveFlag == true && a.ASMCL_Id == data.ASMCL_Id)
                                    select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamWiseRemarksReportDTO OnChangeSection(ExamWiseRemarksReportDTO data)
        {
            try
            {
                var getemcaid = _examContext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
               && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToArray();

                var geteycid = _examContext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EYC_ActiveFlg == true
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                if (data.Report_Type == "Exam")
                {
                    data.examname = (from a in _examContext.Exm_Yearly_Category_ExamsDMO
                                     from b in _examContext.exammasterDMO
                                     where (a.EME_Id == b.EME_Id && a.EYCE_ActiveFlg == true && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id)
                                     select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
                }
                else
                {
                    data.termlist = _examContext.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true
                     && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().OrderBy(a => a.ECT_TermName).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamWiseRemarksReportDTO OnChangeExam(ExamWiseRemarksReportDTO data)
        {
            try
            {
                var getemcaid = _examContext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
               && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToArray();

                var geteycid = _examContext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EYC_ActiveFlg == true
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                data.getsubjectlist = (from a in _examContext.Exm_Yearly_Category_ExamsDMO
                                       from b in _examContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                       from c in _examContext.IVRM_School_Master_SubjectsDMO
                                       where (a.EYCE_Id == b.EYCE_Id && b.ISMS_Id == c.ISMS_Id && b.EYCES_ActiveFlg == true
                                       && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id && a.EME_Id == data.EME_Id)
                                       select c).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamWiseRemarksReportDTO GetExamWiseRemarksReport(ExamWiseRemarksReportDTO data)
        {
            try
            {
                List<long> emeids = new List<long>();

                foreach (var c in data.Selected_EME_List)
                {
                    emeids.Add(c.EME_Id);
                }

                string order = "";
                var get_configuration = _examContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                data.instituelist = _examContext.Institution_master.Where(a => a.MI_Id == data.MI_Id).ToArray();

                if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                {
                    order = "studentname";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                {
                    order = "AMST_AdmNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                {
                    order = "AMAY_RollNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                {
                    order = "AMST_RegistrationNo";
                }
                else
                {
                    order = "studentname";
                }

                List<ExamWiseRemarksReportDTO> savedstudentList = new List<ExamWiseRemarksReportDTO>();

                savedstudentList = (from a in _examContext.School_Adm_Y_StudentDMO
                                    from b in _examContext.Adm_M_Student
                                    from c in _examContext.AcademicYear
                                    from d in _examContext.AdmissionClass
                                    from e in _examContext.School_M_Section
                                    from f in _examContext.Exm_ProgressCard_RemarksDMO
                                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                    && f.AMST_Id == a.AMST_Id && f.ASMAY_Id == c.ASMAY_Id && f.ASMCL_Id == d.ASMCL_Id
                                    && f.ASMS_Id == e.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                    && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id
                                    && emeids.Contains(f.EME_ID) && f.EMER_ActiveFlag == true)
                                    select new ExamWiseRemarksReportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                        AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                        AMST_RegistrationNo = b.AMST_RegistrationNo,
                                        AMAY_RollNo = a.AMAY_RollNo,
                                        EMER_Remarks = f.EMER_Remarks,
                                        EME_Id = f.EME_ID
                                    }).Distinct().ToList();

                var propertyInfo1 = typeof(ExamWiseRemarksReportDTO).GetProperty(order);
                savedstudentList = savedstudentList.OrderBy(x => propertyInfo1.GetValue(x, null)).ToList();
                data.savedata = savedstudentList.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamWiseRemarksReportDTO GetExamSubjectWiseRemarks_PTReport(ExamWiseRemarksReportDTO data)
        {
            try
            {
                List<long> ismsids = new List<long>();

                if (data.Selected_Subject_List != null && data.Selected_Subject_List.Length > 0)
                {
                    foreach (var c in data.Selected_Subject_List)
                    {
                        ismsids.Add(c.ISMS_Id);
                    }

                    string order = "studentname";
                    var get_configuration = _examContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                    data.configuration = get_configuration.ToArray();

                    data.instituelist = _examContext.Institution_master.Where(a => a.MI_Id == data.MI_Id).ToArray();

                    if (get_configuration != null && get_configuration.Count > 0)
                    {
                        if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                        {
                            order = "studentname";
                        }
                        else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                        {
                            order = "AMST_AdmNo";
                        }
                        else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                        {
                            order = "AMAY_RollNo";
                        }
                        else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                        {
                            order = "AMST_RegistrationNo";
                        }
                    }

                    List<ExamWiseRemarksReportDTO> savedstudentList = new List<ExamWiseRemarksReportDTO>();

                    if (data.Subject_PT_TypeReport == "StudetSubjectWiseRemarks")
                    {
                        savedstudentList = (from a in _examContext.School_Adm_Y_StudentDMO
                                            from b in _examContext.Adm_M_Student
                                            from c in _examContext.AcademicYear
                                            from d in _examContext.AdmissionClass
                                            from e in _examContext.School_M_Section
                                            from f in _examContext.Exm_Student_ProgressCard_SubjectWise_RemarksDMO
                                            from g in _examContext.masterexam
                                            from h in _examContext.IVRM_School_Master_SubjectsDMO
                                            where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                            && f.AMST_Id == a.AMST_Id && f.ASMAY_Id == c.ASMAY_Id && f.ASMCL_Id == d.ASMCL_Id && f.EME_ID == g.EME_Id
                                            && f.ISMS_Id == h.ISMS_Id && f.ASMS_Id == e.ASMS_Id
                                            && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                            && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id
                                            && ismsids.Contains(f.ISMS_Id) && f.EME_ID == data.EME_Id && f.ESSEPCR_ActiveFlag == true)
                                            select new ExamWiseRemarksReportDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                studentname = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                (b.AMST_MiddleName == null || b.AMST_MiddleName == "" ? "" : " " + b.AMST_MiddleName) +
                                                (b.AMST_LastName == null || b.AMST_LastName == "" ? "" : " " + b.AMST_LastName)).Trim(),
                                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                                AMST_RegistrationNo = b.AMST_RegistrationNo,
                                                AMAY_RollNo = a.AMAY_RollNo,
                                                EMER_Remarks = f.ESSEPCR_Remarks,
                                                EME_Id = f.EME_ID,
                                                ISMS_Id = f.ISMS_Id,
                                            }).Distinct().ToList();
                    }
                    else if (data.Subject_PT_TypeReport == "StudetSubjectWisePaperType")
                    {
                        savedstudentList = (from a in _examContext.School_Adm_Y_StudentDMO
                                            from b in _examContext.Adm_M_Student
                                            from c in _examContext.AcademicYear
                                            from d in _examContext.AdmissionClass
                                            from e in _examContext.School_M_Section
                                            from f in _examContext.Exm_Student_Examwise_PTDMO
                                            from g in _examContext.masterexam
                                            from h in _examContext.Exm_Master_PaperTypeDMO
                                            from i in _examContext.IVRM_School_Master_SubjectsDMO
                                            where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                            && f.AMST_Id == a.AMST_Id && f.ASMAY_Id == c.ASMAY_Id && f.ASMCL_Id == d.ASMCL_Id && f.EME_Id == g.EME_Id
                                            && f.ISMS_Id == i.ISMS_Id && f.EMPATY_Id == h.EMPATY_Id
                                            && f.ASMS_Id == e.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                            && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id
                                            && ismsids.Contains(f.ISMS_Id) && f.EME_Id == data.EME_Id && f.ESEWPT_ActiveFlg == true)
                                            select new ExamWiseRemarksReportDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                studentname = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                (b.AMST_MiddleName == null || b.AMST_MiddleName == "" ? "" : " " + b.AMST_MiddleName) +
                                                (b.AMST_LastName == null || b.AMST_LastName == "" ? "" : " " + b.AMST_LastName)).Trim(),
                                                AMST_AdmNo = b.AMST_AdmNo ?? "",
                                                AMST_RegistrationNo = b.AMST_RegistrationNo,
                                                AMAY_RollNo = a.AMAY_RollNo,
                                                EMER_Remarks = h.EMPATY_PaperTypeName,
                                                EMPATY_Color = h.EMPATY_Color == null || h.EMPATY_Color == "" ? "black" : h.EMPATY_Color,
                                                EME_Id = f.EME_Id,
                                                ISMS_Id = f.ISMS_Id,
                                            }).Distinct().ToList();
                    }

                    var propertyInfo1 = typeof(ExamWiseRemarksReportDTO).GetProperty(order);
                    savedstudentList = savedstudentList.OrderBy(x => propertyInfo1.GetValue(x, null)).ToList();
                    data.savedata = savedstudentList.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamWiseRemarksReportDTO GetTermWiseRemarksReport(ExamWiseRemarksReportDTO data)
        {
            try
            {
                string order = "";
                var get_configuration = _examContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                data.instituelist = _examContext.Institution_master.Where(a => a.MI_Id == data.MI_Id).ToArray();

                if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                {
                    order = "studentname";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                {
                    order = "AMST_AdmNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                {
                    order = "AMAY_RollNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                {
                    order = "AMST_RegistrationNo";
                }
                else
                {
                    order = "studentname";
                }

                List<ExamWiseRemarksReportDTO> savedstudentList = new List<ExamWiseRemarksReportDTO>();

                if (data.Term_Type == "IE")
                {
                    List<long> termids = new List<long>();

                    foreach (var c in data.Selected_Term_List)
                    {
                        termids.Add(c.ECT_Id);
                    }

                    savedstudentList = (from a in _examContext.School_Adm_Y_StudentDMO
                                        from b in _examContext.Adm_M_Student
                                        from c in _examContext.AcademicYear
                                        from d in _examContext.AdmissionClass
                                        from e in _examContext.School_M_Section
                                        from f in _examContext.ExamTermWiseRemarksDMO
                                        where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                        && f.AMST_Id == a.AMST_Id && f.ASMAY_Id == c.ASMAY_Id && f.ASMCL_Id == d.ASMCL_Id
                                        && f.ASMS_Id == e.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                        && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id
                                        && termids.Contains(f.ECT_Id) && f.ECTERE_ActiveFlag == true && f.ECTERE_Indi_OverAllFlag == "IE")
                                        select new ExamWiseRemarksReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                            AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                            AMST_RegistrationNo = b.AMST_RegistrationNo,
                                            AMAY_RollNo = a.AMAY_RollNo,
                                            ECTERE_Remarks = f.ECTERE_Remarks,
                                            ECT_Id = f.ECT_Id
                                        }).Distinct().ToList();

                    var propertyInfo1 = typeof(ExamWiseRemarksReportDTO).GetProperty(order);
                    savedstudentList = savedstudentList.OrderBy(x => propertyInfo1.GetValue(x, null)).ToList();
                    data.savedata = savedstudentList.ToArray();
                }
                else
                {
                    savedstudentList = (from a in _examContext.School_Adm_Y_StudentDMO
                                        from b in _examContext.Adm_M_Student
                                        from c in _examContext.AcademicYear
                                        from d in _examContext.AdmissionClass
                                        from e in _examContext.School_M_Section
                                        from f in _examContext.ExamTermWiseRemarksDMO
                                        where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                        && f.AMST_Id == a.AMST_Id && f.ASMAY_Id == c.ASMAY_Id && f.ASMCL_Id == d.ASMCL_Id
                                        && f.ASMS_Id == e.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                        && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id
                                        && f.ECTERE_ActiveFlag == true && f.ECTERE_Indi_OverAllFlag == "PE")
                                        select new ExamWiseRemarksReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                            AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                            AMST_RegistrationNo = b.AMST_RegistrationNo,
                                            AMAY_RollNo = a.AMAY_RollNo,
                                            ECTERE_Remarks = f.ECTERE_Remarks
                                        }).Distinct().ToList();

                    var propertyInfo1 = typeof(ExamWiseRemarksReportDTO).GetProperty(order);
                    savedstudentList = savedstudentList.OrderBy(x => propertyInfo1.GetValue(x, null)).ToList();
                    data.savedata = savedstudentList.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
