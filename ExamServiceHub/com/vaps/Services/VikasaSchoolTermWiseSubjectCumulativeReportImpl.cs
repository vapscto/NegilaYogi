using System;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace ExamServiceHub.com.vaps.Services
{
    public class VikasaSchoolTermWiseSubjectCumulativeReportImpl : Interfaces.VikasaSchoolTermWiseSubjectCumulativeReportInterface
    {
        private static ConcurrentDictionary<string, VikasaSubjectwiseCumulativeReportDTO> _login =
       new ConcurrentDictionary<string, VikasaSubjectwiseCumulativeReportDTO>();

        private readonly ExamContext _ReportContext;
        public StudentAttendanceReportContext _db;
        ILogger<VikasaSchoolTermWiseSubjectCumulativeReportImpl> _acdimpl;
        public VikasaSchoolTermWiseSubjectCumulativeReportImpl(ExamContext cpContext, StudentAttendanceReportContext db)
        {
            _ReportContext = cpContext;
            _db = db;
        }
        public VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data)//int IVRMM_Id
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _ReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                data.gradeList = _ReportContext.Exm_Master_GradeDMO.Where(a => a.MI_Id == data.MI_Id && a.EMGR_ActiveFlag == true).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }
        public VikasaSubjectwiseCumulativeReportDTO showdetails1(VikasaSubjectwiseCumulativeReportDTO data)
        {
            try
            {
                List<int?> ids = new List<int?>();
                ids.Add(0);
                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");
                sol.Add("L");
                sol.Add("D");

                data.BasicListYear = (from a in _ReportContext.AcademicYear
                                      where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new VikasaSubjectwiseCumulativeReportDTO
                                      {
                                          yearname = a.ASMAY_Year
                                      }).Distinct().ToArray();


                data.BasiListclass = (from a in _db.admissionyearstudent
                                      from b in _db.admissionClass
                                      where (a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == data.MI_Id 
                                      && b.ASMCL_Id == data.ASMCL_Id)
                                      select new VikasaSubjectwiseCumulativeReportDTO
                                      {
                                          ClassName = b.ASMCL_ClassName
                                      }).Distinct().ToArray();

                data.BasiListsectiont = (from a in _db.admissionyearstudent
                                         from b in _db.masterSection

                                         where (a.ASMAY_Id == data.ASMAY_Id && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == data.MI_Id 
                                         && a.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                         select new VikasaSubjectwiseCumulativeReportDTO
                                         {
                                             sectionname = b.ASMC_SectionName
                                         }).Distinct().ToArray();

                data.BasiListsubject = (from a in _ReportContext.Exm_Category_ClassDMO
                                        from b in _ReportContext.Exm_Yearly_CategoryDMO
                                        from c in _ReportContext.Exm_Yearly_Category_ExamsDMO
                                        from d in _ReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                        from e in _ReportContext.IVRM_School_Master_SubjectsDMO
                                        where (a.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && c.EYCE_Id == d.EYCE_Id && d.ISMS_Id == e.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && d.ISMS_Id == data.ISMS_Id)
                                        select new VikasaSubjectwiseCumulativeReportDTO
                                        {
                                            ISMS_Id = d.ISMS_Id,
                                            SubjectName = e.ISMS_SubjectName

                                        }).Distinct().ToArray();
                //for subject wise grade
                data.gradelist = _ReportContext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == data.ASMCL_Id && t.ISMS_Id == data.ISMS_Id && t.ASMS_Id == data.ASMS_Id && t.ASMAY_Id == data.ASMAY_Id).Distinct().ToArray();

                //for student details
                data.studentList = (from a in _db.admissionyearstudent
                                    from b in _db.admissionStduent
                                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && ids.Contains(a.AMAY_ActiveFlag) && a.ASMS_Id == data.ASMS_Id 
                                    && b.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && ids.Contains(b.AMST_ActiveFlag)
                                    && sol.Contains(b.AMST_SOL))
                                    select b).Distinct().ToArray();


                //for student Exam Group head
                var dt = (from a in _ReportContext.Exm_Yearly_CategoryDMO
                          from b in _ReportContext.Exm_Category_ClassDMO
                          from c in _ReportContext.Exm_Master_CategoryDMO
                          where a.EMCA_Id == b.EMCA_Id && a.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == c.EMCA_Id && b.ECAC_ActiveFlag == true && b.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id
                          select new VikasaSubjectwiseCumulativeReportDTO
                          {
                              EYC_Id = a.EYC_Id
                          }).Distinct().ToList();
                var EQuery = dt.Select(d => d.EYC_Id).ToList();

                data.ExamGroupname = (from a in _ReportContext.Exm_M_PromotionDMO
                                      from b in _ReportContext.Exm_M_Promotion_SubjectsDMO
                                      from c in _ReportContext.Exm_M_Prom_Subj_GroupDMO
                                      where (a.EMP_Id == b.EMP_Id && c.EMPS_Id == b.EMPS_Id && a.MI_Id == data.MI_Id && EQuery.Contains(a.EYC_Id) && c.EMPSG_ActiveFlag == true && b.ISMS_Id == data.ISMS_Id)
                                      select new VikasaSubjectwiseCumulativeReportDTO
                                      {
                                          EMPSG_GroupName = c.EMPSG_GroupName,
                                          EMPSG_PercentValue = c.EMPSG_PercentValue,
                                          EMPSG_MarksValue = c.EMPSG_MarksValue,
                                          EMPSG_Id = c.EMPSG_Id
                                      }).Distinct().ToArray();

                //for student Exam Group Marks

                var dt1 = (from a in _ReportContext.Exm_M_PromotionDMO
                           from b in _ReportContext.Exm_M_Promotion_SubjectsDMO
                           from c in _ReportContext.Exm_M_Prom_Subj_GroupDMO
                           where (a.EMP_Id == b.EMP_Id && c.EMPS_Id == b.EMPS_Id && a.MI_Id == data.MI_Id && EQuery.Contains(a.EYC_Id) && c.EMPSG_ActiveFlag == true)
                           select new VikasaSubjectwiseCumulativeReportDTO
                           {
                               EMPSG_Id = c.EMPSG_Id
                           }).Distinct().ToList();
                var EQuery1 = dt1.Select(d => d.EMPSG_Id).ToList();

                data.examgroupmarks = (from a in _ReportContext.Exm_M_Prom_Subj_Group_ExamsDMO
                                       from b in _ReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                       where a.EME_Id == b.EME_Id && EQuery1.Contains(a.EMPSG_Id) && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.ISMS_Id == data.ISMS_Id
                                       group new { a, b }
                                   by new { b.AMST_Id, a.EMPSG_Id } into g
                                       select new VikasaSubjectwiseCumulativeReportDTO
                                       {
                                           AMST_Id = g.FirstOrDefault().b.AMST_Id,
                                           EMPSG_Id = g.FirstOrDefault().a.EMPSG_Id,
                                           ESTMPS_MaxMarks = g.Sum(d => d.b.ESTMPS_MaxMarks),
                                           ESTMPS_ObtainedMarks = g.Sum(d => d.b.ESTMPS_ObtainedMarks)

                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
        public VikasaSubjectwiseCumulativeReportDTO get_class(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                dto.classlist = (from c in _db.admissionClass
                                 from d in _db.Exm_Category_ClassDMO
                                 where (d.ASMCL_Id == c.ASMCL_Id && d.ECAC_ActiveFlag == true && d.MI_Id == dto.MI_Id && d.ASMAY_Id == dto.ASMAY_Id)
                                 select c).Distinct().OrderBy(t => t.ASMCL_Order).ToArray(); 
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public VikasaSubjectwiseCumulativeReportDTO get_section(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                dto.sectionList = (from b in _db.admissionClass
                                   from c in _db.masterSection
                                   from d in _db.Exm_Category_ClassDMO
                                   where (b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ECAC_ActiveFlag == true && d.ASMCL_Id == dto.ASMCL_Id
                                   && c.MI_Id == dto.MI_Id && d.MI_Id == dto.MI_Id && d.ASMAY_Id == dto.ASMAY_Id)
                                   select c).Distinct().OrderBy(t => t.ASMC_Order).ToArray();                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public VikasaSubjectwiseCumulativeReportDTO get_subject(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                var getcategoryid = _ReportContext.Exm_Category_ClassDMO.Where(a => a.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ASMCL_Id == dto.ASMCL_Id
                && a.ASMS_Id == dto.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                dto.termlist = _ReportContext.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id
                && a.EMCA_Id == getcategoryid).ToArray();

                dto.subjectList = (from a in _ReportContext.Exm_Category_ClassDMO
                                   from b in _ReportContext.Exm_Master_CategoryDMO
                                   from c in _ReportContext.Exm_Yearly_CategoryDMO
                                   from d in _ReportContext.Exm_Yearly_Category_ExamsDMO
                                   from e in _ReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                   from f in _ReportContext.IVRM_School_Master_SubjectsDMO
                                   where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && d.EYC_Id == c.EYC_Id && d.EYCE_Id == e.EYCE_Id && e.ISMS_Id == f.ISMS_Id
                                   && a.ASMAY_Id == dto.ASMAY_Id && a.ASMCL_Id == dto.ASMCL_Id && a.ASMS_Id == dto.ASMS_Id && a.MI_Id == dto.MI_Id && c.ASMAY_Id == dto.ASMAY_Id)
                                   select f).Distinct().OrderBy(f => f.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public async Task<VikasaSubjectwiseCumulativeReportDTO> showdetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Subject_Term_Average_Wise_Report ";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id", SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.EMGR_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ISMS_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.getreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Subject_Term_Average_Wise_Report_headings ";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id", SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar)
                    {
                        Value = data.EMGR_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ISMS_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.getreporthead = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.classteacher = (from a in _ReportContext.Exm_Login_PrivilegeDMO
                                     from b in _ReportContext.Exm_Login_Privilege_SubjectsDMO
                                     from c in _ReportContext.HR_Master_Employee_DMO
                                     from d in _ReportContext.Staff_User_Login
                                     where (a.ELP_Id == b.ELP_Id && a.MI_Id == data.MI_Id && a.ELP_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id
                                     && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.ISMS_Id == data.ISMS_Id && b.ELPs_ActiveFlg == true
                                     && a.Login_Id == d.IVRMSTAUL_Id && d.Emp_Code == c.HRME_Id)
                                     select new VikasaSubjectwiseCumulativeReportDTO
                                     {
                                         HRME_Id = c.HRME_Id,
                                         empname = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null ? " " : "  " + c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null ? " " : "  " + c.HRME_EmployeeLastName)).Trim(),
                                     }).ToArray();

                data.instname = _ReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

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
