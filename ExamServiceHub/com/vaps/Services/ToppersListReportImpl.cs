using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;

namespace ExamServiceHub.com.vaps.Services
{
    public class ToppersListReportImpl : ToppersListReportInterface
    {
        public ExamContext _examctxt;
        private DomainModelMsSqlServerContext _db;
        public ToppersListReportImpl(ExamContext obj, DomainModelMsSqlServerContext _d)
        {
            _examctxt = obj;
            _db = _d;
        }
        public ToppersListReportDTO getdetails(ToppersListReportDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();
                data.Acdlist = list.OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.catlist = (
                                from d in _examctxt.Exm_Master_CategoryDMO
                                where (d.MI_Id == data.MI_Id && d.EMCA_ActiveFlag == true)
                                select d).Distinct().ToArray();

                data.examlist = _examctxt.masterexam.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true).OrderBy(a => a.EME_ExamOrder).ToArray();

                data.sublist = _examctxt.IVRM_School_Master_SubjectsDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_ActiveFlag == 1 && t.ISMS_ExamFlag == 1).OrderBy(a => a.ISMS_OrderFlag).ToArray();

                data.ctlist = _examctxt.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();

                data.seclist = _examctxt.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1).OrderBy(a => a.ASMC_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public ToppersListReportDTO onselectCategory(ToppersListReportDTO data)
        {
            try
            {
                data.ctlist = (from c in _examctxt.AdmissionClass
                               from d in _examctxt.Exm_Category_ClassDMO
                               where (d.ASMCL_Id == c.ASMCL_Id && d.ECAC_ActiveFlag == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id 
                               && d.EMCA_Id == data.EMCA_Id)
                               select c).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ToppersListReportDTO onselectclass(ToppersListReportDTO data)
        {
            try
            {
                data.seclist = (from b in _examctxt.AdmissionClass
                                from c in _examctxt.School_M_Section
                                from d in _examctxt.Exm_Category_ClassDMO
                                where (b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ECAC_ActiveFlag == true && d.ASMCL_Id == data.ASMCL_Id
                                && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.EMCA_Id == data.EMCA_Id)
                                select c).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ToppersListReportDTO onreport1(ToppersListReportDTO data)
        {
            try
            {
                if (data.report_type == "all")
                {
                    if (data.exm_check_type == "1" && data.sub_check_type == "1")
                    {
                        data.datareport = (from a in _examctxt.ExmStudentMarksProcessDMO
                                           from b in _examctxt.Adm_M_Student
                                           from c in _examctxt.School_M_Section
                                           from d in _examctxt.AcademicYear
                                           from e in _examctxt.AdmissionClass
                                           from f in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                           from g in _examctxt.IVRM_School_Master_SubjectsDMO
                                           where (a.AMST_Id == b.AMST_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.EME_Id == f.EME_Id && f.ISMS_Id == g.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id && f.ISMS_Id == data.ISMS_Id)
                                           select new ToppersListReportDTO
                                           {
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = b.AMST_FirstName,
                                               ASMC_SectionName = c.ASMC_SectionName,
                                               ESTMP_TotalMaxMarks = a.ESTMP_TotalMaxMarks,
                                               ESTMP_TotalObtMarks = a.ESTMP_TotalObtMarks,
                                               ESTMP_Percentage = a.ESTMP_Percentage
                                           }
                                           ).Distinct().Take(data.topper).ToArray();
                    }
                    else if (data.exm_check_type == "1")
                    {
                        data.datareport = (from a in _examctxt.ExmStudentMarksProcessDMO
                                           from b in _examctxt.Adm_M_Student
                                           from c in _examctxt.School_M_Section
                                           from d in _examctxt.AcademicYear
                                           from e in _examctxt.AdmissionClass
                                           from f in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                           from g in _examctxt.IVRM_School_Master_SubjectsDMO
                                           where (a.AMST_Id == b.AMST_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.EME_Id == f.EME_Id && f.ISMS_Id == g.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id)
                                           select new ToppersListReportDTO
                                           {
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = b.AMST_FirstName,
                                               ASMC_SectionName = c.ASMC_SectionName,
                                               ESTMP_TotalMaxMarks = a.ESTMP_TotalMaxMarks,
                                               ESTMP_TotalObtMarks = a.ESTMP_TotalObtMarks,
                                               ESTMP_Percentage = a.ESTMP_Percentage
                                           }
                                           ).Distinct().Take(data.topper).ToArray();
                    }
                    else if (data.sub_check_type == "1")
                    {
                        data.datareport = (from a in _examctxt.ExmStudentMarksProcessDMO
                                           from b in _examctxt.Adm_M_Student
                                           from c in _examctxt.School_M_Section
                                           from d in _examctxt.AcademicYear
                                           from e in _examctxt.AdmissionClass
                                           from f in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                           from g in _examctxt.IVRM_School_Master_SubjectsDMO
                                           where (a.AMST_Id == b.AMST_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.EME_Id == f.EME_Id && f.ISMS_Id == g.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && f.ISMS_Id == data.ISMS_Id)
                                           select new ToppersListReportDTO
                                           {
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = b.AMST_FirstName,
                                               ASMC_SectionName = c.ASMC_SectionName,
                                               ESTMP_TotalMaxMarks = a.ESTMP_TotalMaxMarks,
                                               ESTMP_TotalObtMarks = a.ESTMP_TotalObtMarks,
                                               ESTMP_Percentage = a.ESTMP_Percentage
                                           }
                                           ).Distinct().Take(data.topper).ToArray();
                    }
                    else
                    {
                        data.datareport = (from a in _examctxt.ExmStudentMarksProcessDMO
                                           from b in _examctxt.Adm_M_Student
                                           from c in _examctxt.School_M_Section
                                           from d in _examctxt.AcademicYear
                                           from e in _examctxt.AdmissionClass
                                           from f in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                           from g in _examctxt.IVRM_School_Master_SubjectsDMO
                                           where (a.AMST_Id == b.AMST_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.EME_Id == f.EME_Id && f.ISMS_Id == g.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id)
                                           select new ToppersListReportDTO
                                           {
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = b.AMST_FirstName,
                                               ASMC_SectionName = c.ASMC_SectionName,
                                               ESTMP_TotalMaxMarks = a.ESTMP_TotalMaxMarks,
                                               ESTMP_TotalObtMarks = a.ESTMP_TotalObtMarks,
                                               ESTMP_Percentage = a.ESTMP_Percentage
                                           }
                                           ).Distinct().Take(data.topper).ToArray();
                    }
                }

                else if (data.report_type == "individual")
                {
                    if (data.exm_check_type == "1" && data.sub_check_type == "1")
                    {
                        data.datareport = (from a in _examctxt.ExmStudentMarksProcessDMO
                                           from b in _examctxt.Adm_M_Student
                                           from c in _examctxt.School_M_Section
                                           from d in _examctxt.AcademicYear
                                           from e in _examctxt.AdmissionClass
                                           from f in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                           from g in _examctxt.IVRM_School_Master_SubjectsDMO
                                           where (a.AMST_Id == b.AMST_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.EME_Id == f.EME_Id && f.ISMS_Id == g.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id && f.ISMS_Id == data.ISMS_Id && a.ASMS_Id == data.ASMS_Id)
                                           select new ToppersListReportDTO
                                           {
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = b.AMST_FirstName,
                                               ASMC_SectionName = c.ASMC_SectionName,
                                               ESTMP_TotalMaxMarks = a.ESTMP_TotalMaxMarks,
                                               ESTMP_TotalObtMarks = a.ESTMP_TotalObtMarks,
                                               ESTMP_Percentage = a.ESTMP_Percentage
                                           }
                                           ).Distinct().Take(data.topper).ToArray();
                    }
                    else if (data.exm_check_type == "1")
                    {
                        data.datareport = (from a in _examctxt.ExmStudentMarksProcessDMO
                                           from b in _examctxt.Adm_M_Student
                                           from c in _examctxt.School_M_Section
                                           from d in _examctxt.AcademicYear
                                           from e in _examctxt.AdmissionClass
                                           from f in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                           from g in _examctxt.IVRM_School_Master_SubjectsDMO
                                           where (a.AMST_Id == b.AMST_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.EME_Id == f.EME_Id && f.ISMS_Id == g.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id && a.ASMS_Id == data.ASMS_Id)
                                           select new ToppersListReportDTO
                                           {
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = b.AMST_FirstName,
                                               ASMC_SectionName = c.ASMC_SectionName,
                                               ESTMP_TotalMaxMarks = a.ESTMP_TotalMaxMarks,
                                               ESTMP_TotalObtMarks = a.ESTMP_TotalObtMarks,
                                               ESTMP_Percentage = a.ESTMP_Percentage
                                           }
                                           ).Distinct().Take(data.topper).ToArray();
                    }
                    else if (data.sub_check_type == "1")
                    {
                        data.datareport = (from a in _examctxt.ExmStudentMarksProcessDMO
                                           from b in _examctxt.Adm_M_Student
                                           from c in _examctxt.School_M_Section
                                           from d in _examctxt.AcademicYear
                                           from e in _examctxt.AdmissionClass
                                           from f in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                           from g in _examctxt.IVRM_School_Master_SubjectsDMO
                                           where (a.AMST_Id == b.AMST_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.EME_Id == f.EME_Id && f.ISMS_Id == g.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && f.ISMS_Id == data.ISMS_Id && a.ASMS_Id == data.ASMS_Id)
                                           select new ToppersListReportDTO
                                           {
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = b.AMST_FirstName,
                                               ASMC_SectionName = c.ASMC_SectionName,
                                               ESTMP_TotalMaxMarks = a.ESTMP_TotalMaxMarks,
                                               ESTMP_TotalObtMarks = a.ESTMP_TotalObtMarks,
                                               ESTMP_Percentage = a.ESTMP_Percentage
                                           }
                                           ).Distinct().Take(data.topper).ToArray();
                    }
                    else
                    {
                        data.datareport = (from a in _examctxt.ExmStudentMarksProcessDMO
                                           from b in _examctxt.Adm_M_Student
                                           from c in _examctxt.School_M_Section
                                           from d in _examctxt.AcademicYear
                                           from e in _examctxt.AdmissionClass
                                           from f in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                           from g in _examctxt.IVRM_School_Master_SubjectsDMO
                                           where (a.AMST_Id == b.AMST_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.EME_Id == f.EME_Id && f.ISMS_Id == g.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                           select new ToppersListReportDTO
                                           {
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = b.AMST_FirstName,
                                               ASMC_SectionName = c.ASMC_SectionName,
                                               ESTMP_TotalMaxMarks = a.ESTMP_TotalMaxMarks,
                                               ESTMP_TotalObtMarks = a.ESTMP_TotalObtMarks,
                                               ESTMP_Percentage = a.ESTMP_Percentage
                                           }
                                           ).Distinct().Take(data.topper).ToArray();
                    }
                }
                else if (data.report_type == "toppers")
                {
                    if (data.exm_check_type == "1" && data.sub_check_type == "1")
                    {
                        data.datareport1 = (from a in _examctxt.ExmStudentMarksProcessDMO
                                            from b in _examctxt.Adm_M_Student
                                            from c in _examctxt.School_M_Section
                                            from d in _examctxt.AcademicYear
                                            from e in _examctxt.AdmissionClass
                                            from f in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                            from g in _examctxt.IVRM_School_Master_SubjectsDMO
                                            where (a.AMST_Id == b.AMST_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.EME_Id == f.EME_Id && f.ISMS_Id == g.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id && f.ISMS_Id == data.ISMS_Id)
                                            select new ToppersListReportDTO
                                            {
                                                AMST_AdmNo = b.AMST_AdmNo,
                                                AMST_FirstName = b.AMST_FirstName,
                                                ASMCL_ClassName = e.ASMCL_ClassName,
                                                ASMC_SectionName = c.ASMC_SectionName,
                                                ESTMP_ClassRank = a.ESTMP_ClassRank
                                            }
                                           ).Distinct().Take(data.topper).ToArray();
                    }
                    else if (data.exm_check_type == "1")
                    {
                        data.datareport1 = (from a in _examctxt.ExmStudentMarksProcessDMO
                                            from b in _examctxt.Adm_M_Student
                                            from c in _examctxt.School_M_Section
                                            from d in _examctxt.AcademicYear
                                            from e in _examctxt.AdmissionClass
                                            from f in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                            from g in _examctxt.IVRM_School_Master_SubjectsDMO
                                            where (a.AMST_Id == b.AMST_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.EME_Id == f.EME_Id && f.ISMS_Id == g.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id)
                                            select new ToppersListReportDTO
                                            {
                                                AMST_AdmNo = b.AMST_AdmNo,
                                                AMST_FirstName = b.AMST_FirstName,
                                                ASMCL_ClassName = e.ASMCL_ClassName,
                                                ASMC_SectionName = c.ASMC_SectionName,
                                                ESTMP_ClassRank = a.ESTMP_ClassRank
                                            }
                                           ).Distinct().Take(data.topper).ToArray();
                    }
                    else if (data.sub_check_type == "1")
                    {
                        data.datareport1 = (from a in _examctxt.ExmStudentMarksProcessDMO
                                            from b in _examctxt.Adm_M_Student
                                            from c in _examctxt.School_M_Section
                                            from d in _examctxt.AcademicYear
                                            from e in _examctxt.AdmissionClass
                                            from f in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                            from g in _examctxt.IVRM_School_Master_SubjectsDMO
                                            where (a.AMST_Id == b.AMST_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.EME_Id == f.EME_Id && f.ISMS_Id == g.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && f.ISMS_Id == data.ISMS_Id)
                                            select new ToppersListReportDTO
                                            {
                                                AMST_AdmNo = b.AMST_AdmNo,
                                                AMST_FirstName = b.AMST_FirstName,
                                                ASMCL_ClassName = e.ASMCL_ClassName,
                                                ASMC_SectionName = c.ASMC_SectionName,
                                                ESTMP_ClassRank = a.ESTMP_ClassRank
                                            }
                                           ).Distinct().Take(data.topper).ToArray();
                    }
                    else
                    {
                        data.datareport1 = (from a in _examctxt.ExmStudentMarksProcessDMO
                                            from b in _examctxt.Adm_M_Student
                                            from c in _examctxt.School_M_Section
                                            from d in _examctxt.AcademicYear
                                            from e in _examctxt.AdmissionClass
                                            from f in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                            from g in _examctxt.IVRM_School_Master_SubjectsDMO
                                            where (a.AMST_Id == b.AMST_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == e.ASMCL_Id && a.EME_Id == f.EME_Id && f.ISMS_Id == g.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id)
                                            select new ToppersListReportDTO
                                            {
                                                AMST_AdmNo = b.AMST_AdmNo,
                                                AMST_FirstName = b.AMST_FirstName,
                                                ASMCL_ClassName = e.ASMCL_ClassName,
                                                ASMC_SectionName = c.ASMC_SectionName,
                                                ESTMP_ClassRank = a.ESTMP_ClassRank
                                            }
                                           ).Distinct().Take(data.topper).ToArray();
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ToppersListReportDTO onreport(ToppersListReportDTO data)
        {
            try
            {
                using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Topper_List_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EMEFlag",
                       SqlDbType.VarChar)
                    {
                        Value = data.exm_check_type
                    });
                    cmd.Parameters.Add(new SqlParameter("@Subjectflag",
                     SqlDbType.VarChar)
                    {
                        Value = data.sub_check_type
                    });
                    cmd.Parameters.Add(new SqlParameter("@conditionflag",
                    SqlDbType.VarChar)
                    {
                        Value = data.report_type
                    });
                    cmd.Parameters.Add(new SqlParameter("@top",
                   SqlDbType.VarChar)
                    {
                        Value = data.topper
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id",
               SqlDbType.VarChar)
                    {
                        Value = data.ISMS_Id
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;

                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.datareport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ToppersListReportDTO get_sec_exam(ToppersListReportDTO data)
        {
            try
            {
                data.seclist = (from a in _examctxt.Exm_Category_ClassDMO
                                from b in _examctxt.School_M_Section
                                from c in _examctxt.AdmissionClass
                                from d in _examctxt.AcademicYear
                                where (a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id
                                && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ECAC_ActiveFlag == true
                                && b.ASMC_ActiveFlag == 1)
                                select b).Distinct().OrderBy(a => a.ASMC_Order).ToArray();


                data.examlist = (from a in _examctxt.Exm_Master_CategoryDMO
                                 from b in _examctxt.Exm_Category_ClassDMO
                                 from c in _examctxt.Exm_Yearly_CategoryDMO
                                 from d in _examctxt.Exm_Yearly_Category_ExamsDMO
                                 from e in _examctxt.AcademicYear
                                 from f in _examctxt.AdmissionClass
                                 from g in _examctxt.School_M_Section
                                 from h in _examctxt.masterexam
                                 where (a.EMCA_Id == b.EMCA_Id && c.EMCA_Id == a.EMCA_Id && c.EYC_Id == d.EYC_Id && e.ASMAY_Id == c.ASMAY_Id
                                 && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && b.ASMS_Id == g.ASMS_Id && d.EME_Id == h.EME_Id
                                 && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.MI_Id == data.MI_Id
                                 && c.ASMAY_Id == data.ASMAY_Id && b.ECAC_ActiveFlag == true && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true
                                 && h.EME_ActiveFlag == true)
                                 select h).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ToppersListReportDTO onselectexam(ToppersListReportDTO data)
        {
            try
            {
                data.examlist = (from a in _examctxt.Exm_Master_CategoryDMO
                                 from b in _examctxt.Exm_Category_ClassDMO
                                 from c in _examctxt.Exm_Yearly_CategoryDMO
                                 from d in _examctxt.Exm_Yearly_Category_ExamsDMO
                                 from e in _examctxt.AcademicYear
                                 from f in _examctxt.AdmissionClass
                                 from g in _examctxt.School_M_Section
                                 from h in _examctxt.masterexam
                                 where (a.EMCA_Id == b.EMCA_Id && c.EMCA_Id == a.EMCA_Id && c.EYC_Id == d.EYC_Id && e.ASMAY_Id == c.ASMAY_Id
                                 && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && b.ASMS_Id == g.ASMS_Id && d.EME_Id == h.EME_Id
                                 && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.MI_Id == data.MI_Id && b.ASMS_Id == data.ASMS_Id
                                 && c.ASMAY_Id == data.ASMAY_Id && b.ECAC_ActiveFlag == true && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true
                                 && h.EME_ActiveFlag == true)
                                 select h).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ToppersListReportDTO get_subject(ToppersListReportDTO data)
        {
            try
            {
                data.sublist = (from a in _examctxt.Exm_Master_CategoryDMO
                                from b in _examctxt.Exm_Category_ClassDMO
                                from c in _examctxt.Exm_Yearly_CategoryDMO
                                from d in _examctxt.Exm_Yearly_Category_ExamsDMO
                                from i in _examctxt.Exm_Yrly_Cat_Exams_SubwiseDMO
                                from e in _examctxt.AcademicYear
                                from f in _examctxt.AdmissionClass
                                from g in _examctxt.School_M_Section
                                from h in _examctxt.masterexam
                                from j in _examctxt.IVRM_School_Master_SubjectsDMO
                                where (a.EMCA_Id == b.EMCA_Id && c.EMCA_Id == a.EMCA_Id && c.EYC_Id == d.EYC_Id && e.ASMAY_Id == c.ASMAY_Id
                                && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && b.ASMS_Id == g.ASMS_Id && d.EME_Id == h.EME_Id
                                && i.ISMS_Id == j.ISMS_Id && i.EYCE_Id == d.EYCE_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && b.ECAC_ActiveFlag == true && c.EYC_ActiveFlg == true
                                && d.EYCE_ActiveFlg == true && h.EME_ActiveFlag == true && d.EME_Id == data.EME_Id)
                                select j).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ToppersListReportDTO sendsms(ToppersListReportDTO data)
        {
            try
            {

                for (int k = 0; k < data.temp_topper_list_smsdetails.Length; k++)
                {
                    long amst_id = data.temp_topper_list_smsdetails[k].AMST_Id;
                    string classname = data.temp_topper_list_smsdetails[k].asmcL_ClassName;
                    string sectionname = data.temp_topper_list_smsdetails[k].asmC_SectionName;
                    string studentname = data.temp_topper_list_smsdetails[k].amsT_FirstName;
                    string stdadmno = data.temp_topper_list_smsdetails[k].amsT_AdmNo;
                    string rank = data.temp_topper_list_smsdetails[k].estmP_SectionRanknew;
                    string exmaname = data.temp_topper_list_smsdetails[k].exmaname;
                    long MOBILENO = data.temp_topper_list_smsdetails[k].MOBILENO;
                    string EMAILID = data.temp_topper_list_smsdetails[k].EMAILID;
                    string AMST_AppDownloadedDeviceId = data.temp_topper_list_smsdetails[k].AMST_AppDownloadedDeviceId;

                    if (data.smschecked == true)
                    {
                        try
                        {
                            string s = sendsmstopperlist(data.MI_Id, MOBILENO, "TOPPERS_LIST_EXAM", amst_id, data.ASMAY_Id, data.EME_Id, data.ISMS_Id).Result;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    if (data.emailchecked == true)
                    {
                        try
                        {
                            string s1 = sendmailtopperlist(data.MI_Id, EMAILID, "TOPPERS_LIST_EXAM", amst_id, data.ASMAY_Id, data.EME_Id, data.ISMS_Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (data.notificationchecked == true)
                    {

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ToppersListReportDTO.KioskExamTopperDTO kioskExamToppers(ToppersListReportDTO kiosk)
        {
            ToppersListReportDTO.KioskExamTopperDTO obj = new ToppersListReportDTO.KioskExamTopperDTO();
            try
            {
                List<int?> rank = new List<int?>() { 1, 2, 3 };
                var examTopper = (from m in _examctxt.ExmStudentMarksProcessDMO
                                  from n in _examctxt.Adm_M_Student
                                  from o in _examctxt.AdmissionClass
                                  from p in _examctxt.School_M_Section
                                  from q in _examctxt.masterexam
                                  where m.AMST_Id == n.AMST_Id && m.ASMCL_Id == o.ASMCL_Id && p.ASMS_Id == m.ASMS_Id && m.EME_Id == q.EME_Id && m.MI_Id == kiosk.MI_Id
                                  && m.ASMAY_Id == kiosk.ASMAY_Id && n.AMST_SOL.Equals("S") && n.AMST_ActiveFlag == 1 && rank.Contains(m.ESTMP_ClassRank)
                                  group new { m, n, o, p, q } by new { m.AMST_Id, m.EME_Id } into g
                                  select new ToppersListReportDTO.KioskExamTopperDTO
                                  {
                                      studentName = g.FirstOrDefault().n.AMST_FirstName + (string.IsNullOrEmpty(g.FirstOrDefault().n.AMST_MiddleName) ? "" : ' ' + g.FirstOrDefault().n.AMST_MiddleName) + (string.IsNullOrEmpty(g.FirstOrDefault().n.AMST_LastName) ? "" : ' ' + g.FirstOrDefault().n.AMST_LastName),
                                      className = g.FirstOrDefault().o.ASMCL_ClassName,
                                      sectionName = g.FirstOrDefault().p.ASMC_SectionName,
                                      examName = g.FirstOrDefault().q.EME_ExamName,
                                      photo = g.FirstOrDefault().n.AMST_Photoname
                                  }).ToList();
                if (examTopper.Count > 0)
                {
                    obj.kioskExamToppers = examTopper.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }

        //SMS AND EMAIL
        public async Task<string> sendsmstopperlist(long MI_Id, long mobileNo, string Template, long UserID, long ASMAY_Id, long EME_Id, long ISMS_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMSMAILPARAMETER_TOPPERLIST_EXAM";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template", SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                        {
                            Value = ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                        {
                            Value = EME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar)
                        {
                            Value = ISMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                        );
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entity_id", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("template_id", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public string sendmailtopperlist(long MI_Id, string Email, string Template, long UserID, long ASMAY_Id, long EME_Id, long ISMS_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER_TOPPERLIST_EXAM";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template", SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                        {
                            Value = ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                        {
                            Value = EME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar)
                        {
                            Value = ISMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                        );
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }


                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    string mailbcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        mailcc = ccmail[0].ToString();

                        if (ccmail.Length > 1)
                        {
                            if (ccmail[1] != null || ccmail[1] != "")
                            {
                                mailbcc = ccmail[1].ToString();
                            }
                        }

                    }
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);
                    if (mailcc != null && mailcc != "")
                    {
                        message.AddCc(mailcc);
                    }
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }


                    // var client = new Web("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");

                    //if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    //{
                    //    message.AddAttachment(template.FirstOrDefault().ISES_MailHTMLTemplate);

                    //}
                    message.HtmlContent = Mailmsg;

                    var client = new SendGridClient(sengridkey);

                    client.SendEmailAsync(message).Wait();


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

    }
}
