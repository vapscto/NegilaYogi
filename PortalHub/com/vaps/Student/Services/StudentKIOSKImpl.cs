using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.Extensions.Logging;
using System.Linq;
using DomainModel.Model.com.vaps.Exam;

namespace PortalHub.com.vaps.Student.Services
{
    public class StudentKIOSKImpl : Interfaces.StudentKIOSKInterface
    {
        private PortalContext _studentDashboardContext;
        ILogger<StudentKIOSKImpl> _acdimpl;

        public StudentKIOSKImpl(PortalContext studentDashboardContext)
        {
            _studentDashboardContext = studentDashboardContext;
        }

        public async Task<StudentKIOSKDTO> GetdetailsKiosk(StudentKIOSKDTO data)
        {
            try
            {
                #region Student Details
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_StudentDashboard";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
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
                        data.studetailslist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                #region Fee Monthly Details
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_STUDENT_MONTHLY_FEE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id",
                     SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.academicyearFeedata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<StudentKIOSKDTO> GetAttendancedetailsKiosk(StudentKIOSKDTO data)
        {
            try
            {
                #region Student Monthly Attendance 
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_STUDENT_MONTHLY_ATTENDANCE";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
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
                        data.academicyearAttendancedata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentKioskExamTopperDTO showExamreport(StudentKioskExamTopperDTO data)
        {
            try
            {
                List<StudentKioskExamTopperDTO> result1 = new List<StudentKioskExamTopperDTO>();
                List<StudentKioskExamTopperDTO> result2 = new List<StudentKioskExamTopperDTO>();
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chairman_Classwise_Exm_Rank";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.EME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();


                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result1.Add(new StudentKioskExamTopperDTO
                                {
                                    ASMAY_Id = Convert.ToInt32(dataReader["ASMAY_Id"].ToString()),
                                    ASMCL_Id = Convert.ToInt32(dataReader["ASMCL_Id"].ToString()),
                                    ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString()),
                                    ASMS_Id = Convert.ToInt32(dataReader["ASMS_Id"].ToString()),
                                    ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString()),
                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                    AMST_FirstName = (dataReader["name"].ToString()),
                                    AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString()),
                                    ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString()),
                                    ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString()),
                                    ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString()),
                                    Class_Rnk = Convert.ToInt32(dataReader["Class_Rnk"].ToString()),
                                    ELP_Flg = dataReader["AMST_Photoname"].ToString()

                                });
                                data.classranklist = result1.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }

        public StudentKioskCOEDTO getcoedata(StudentKioskCOEDTO data)
        {
            try
            {
                var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                               from c in _studentDashboardContext.School_M_Class
                               from s in _studentDashboardContext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                               select new StudentKioskCOEDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();

                //data.coereportlist = (from a in _studentDashboardContext.COE_Master_EventsDMO
                //                      from b in _studentDashboardContext.COE_EventsDMO
                //                      from c in _studentDashboardContext.School_Adm_Y_StudentDMO
                //                      from d in _studentDashboardContext.Adm_M_Student
                //                      from e in _studentDashboardContext.COE_Events_ClassesDMO
                //                      where (a.COEME_Id == b.COEME_Id && b.COEE_Id == e.COEE_Id && b.ASMAY_Id == c.ASMAY_Id && c.AMST_Id == d.AMST_Id && c.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == clssec1.FirstOrDefault().ASMCL_Id && a.MI_Id == data.MI_Id && b.COEE_EStartDate.Value.Month == data.month)
                //                      select new StudentKioskCOEDTO
                //                      {
                //                          COEME_EventName = a.COEME_EventName,
                //                          COEME_EventDesc = a.COEME_EventDesc,
                //                          COEE_EStartDate = b.COEE_EStartDate,
                //                          COEE_EEndDate = b.COEE_EEndDate,
                //                          ASMAY_Id = b.ASMAY_Id
                //                      }).Distinct().OrderBy(t => t.COEE_EStartDate).ToArray();

                data.coereportlist = (from m in _studentDashboardContext.COE_Master_EventsDMO
                                      from n in _studentDashboardContext.COE_EventsDMO
                                      from p in _studentDashboardContext.COE_Events_ClassesDMO
                                      from o in _studentDashboardContext.School_Adm_Y_StudentDMO
                                      where (m.COEME_Id == n.COEME_Id && n.COEE_Id == p.COEE_Id && p.ASMCL_Id == o.ASMCL_Id && n.MI_Id == data.MI_Id && o.ASMAY_Id == data.ASMAY_Id && p.ASMCL_Id == clssec1.FirstOrDefault().ASMCL_Id && n.COEE_EStartDate.Value.Month == data.month && n.COEE_ActiveFlag == true)
                                      select new StudentDashboardDTO
                                      {
                                          COEME_EventName = m.COEME_EventName,
                                          COEME_EventDesc = m.COEME_EventDesc,
                                          COEE_EStartDate = n.COEE_EStartDate,
                                          COEE_EEndDate = n.COEE_EEndDate,
                                          COEE_ReminderDate = n.COEE_ReminderDate
                                      }).OrderBy(c => c.COEE_EStartDate).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentKioskSubjectDTO getloadyear(StudentKioskSubjectDTO data)
        {
            try
            {
                data.stuyearlist = (from d in _studentDashboardContext.AcademicYearDMO
                                    from a in _studentDashboardContext.School_M_Class
                                    from b in _studentDashboardContext.School_M_Section
                                    from c in _studentDashboardContext.School_Adm_Y_StudentDMO
                                    where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                    a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                    select new StudentKioskSubjectDTO
                                    {
                                        ASMCL_Id = c.ASMCL_Id,
                                        ASMCL_ClassName = a.ASMCL_ClassName,
                                        ASMS_Id = c.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName,
                                        ASMAY_Id = c.ASMAY_Id,
                                        ASMAY_Year = d.ASMAY_Year,
                                        ASMAY_Order = d.ASMAY_Order
                                    }
                             ).OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentKioskSubjectDTO getSubjectsdata(StudentKioskSubjectDTO data)
        {
            try
            {
                var classSectionData = (from d in _studentDashboardContext.AcademicYearDMO
                                        from a in _studentDashboardContext.School_M_Class
                                        from b in _studentDashboardContext.School_M_Section
                                        from c in _studentDashboardContext.School_Adm_Y_StudentDMO
                                        where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                        a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id)
                                        select new StudentKioskSubjectDTO
                                        {
                                            ASMCL_Id = c.ASMCL_Id,
                                            ASMCL_ClassName = a.ASMCL_ClassName,
                                            ASMS_Id = c.ASMS_Id,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            ASMAY_Id = c.ASMAY_Id,
                                            ASMAY_Year = d.ASMAY_Year
                                        }
                             ).ToList();
                data.stuyearlist = classSectionData.ToArray();

                data.subjectlist = (from a in _studentDashboardContext.StudentMappingDMO
                                    from b in _studentDashboardContext.IVRM_Master_SubjectsDMO
                                    where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == classSectionData.FirstOrDefault().ASMCL_Id && a.ASMS_Id == classSectionData.FirstOrDefault().ASMS_Id && a.ASMAY_Id == data.ASMAY_Id
                                    && a.AMST_Id == data.AMST_Id && b.MI_Id == data.MI_Id)
                                    select new StudentKioskSubjectDTO
                                    {
                                        ISMS_Id = b.ISMS_Id,
                                        ISMS_SubjectName = b.ISMS_SubjectName
                                    }
                             ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<StudentKioskEXAMDTO> StudentExamDetails(StudentKioskEXAMDTO data)
        {
            try
            {
                long asmclid = 0;
                long asmsid = 0;
                var classSectionData = (from d in _studentDashboardContext.AcademicYearDMO
                                        from a in _studentDashboardContext.School_M_Class
                                        from b in _studentDashboardContext.School_M_Section
                                        from c in _studentDashboardContext.School_Adm_Y_StudentDMO
                                        where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                        select new StudentKioskEXAMDTO
                                        {
                                            ASMCL_Id = c.ASMCL_Id,
                                            ASMCL_ClassName = a.ASMCL_ClassName,
                                            ASMS_Id = c.ASMS_Id,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            ASMAY_Id = c.ASMAY_Id,
                                            ASMAY_Year = d.ASMAY_Year
                                        }).ToList();

                if (classSectionData.Count > 0)
                {
                    asmclid = classSectionData.FirstOrDefault().ASMCL_Id;
                    asmsid = classSectionData.FirstOrDefault().ASMS_Id;
                }
                else
                {
                    asmclid = 0;
                    asmsid = 0;
                }

                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_EXAMREPORT_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt)
                    {
                        Value = asmclid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt)
                    {
                        Value = asmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.BigInt)
                    {
                        Value = data.EME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt)
                    {
                        Value = data.ISMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = data.Type
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
                        data.examReportList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<StudentKioskEXAMDTO> getexamdata(StudentKioskEXAMDTO data)
        {
            try
            {
                long asmclid = 0;
                long asmsid = 0;
                var classSectionData = (from d in _studentDashboardContext.AcademicYearDMO
                                        from a in _studentDashboardContext.School_M_Class
                                        from b in _studentDashboardContext.School_M_Section
                                        from c in _studentDashboardContext.School_Adm_Y_StudentDMO
                                        where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                        select new StudentKioskEXAMDTO
                                        {
                                            ASMCL_Id = c.ASMCL_Id,
                                            ASMCL_ClassName = a.ASMCL_ClassName,
                                            ASMS_Id = c.ASMS_Id,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            ASMAY_Id = c.ASMAY_Id,
                                            ASMAY_Year = d.ASMAY_Year
                                        }
                         ).ToList();
                if (classSectionData.Count > 0)
                {
                    asmclid = classSectionData.FirstOrDefault().ASMCL_Id;
                    asmsid = classSectionData.FirstOrDefault().ASMS_Id;
                }
                else
                {
                    asmclid = 0;
                    asmsid = 0;
                }

                if (data.Type == "Overall")
                {
                    await StudentExamDetails(data);
                }
                else if (data.Type == "SWAE")
                {
                    data.subjectlist = (from ECC in _studentDashboardContext.Exm_Category_ClassDMO
                                        from EYC in _studentDashboardContext.Exm_Yearly_CategoryDMO
                                        from EYCE in _studentDashboardContext.Exm_Yearly_Category_ExamsDMO
                                        from EYCES in _studentDashboardContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                        from n in _studentDashboardContext.StudentMappingDMO
                                        from i in _studentDashboardContext.IVRM_Master_SubjectsDMO
                                        from ef in _studentDashboardContext.ExmStudentMarksProcessDMO
                                        where (EYC.MI_Id == data.MI_Id && EYC.ASMAY_Id == data.ASMAY_Id && ECC.EMCA_Id == EYC.EMCA_Id && EYCE.EYC_Id == EYC.EYC_Id
                                        && EYCES.EYCE_Id == EYCE.EYCE_Id && n.ISMS_Id == EYCES.ISMS_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id
                                        && n.ASMCL_Id == asmclid && n.ASMS_Id == asmsid && EYC.MI_Id == data.MI_Id && EYC.ASMAY_Id == data.ASMAY_Id
                                        && ECC.ASMCL_Id == asmclid && ECC.ASMS_Id == asmsid && EYCE.EME_Id == ef.EME_Id && ef.AMST_Id == data.AMST_Id && ef.ESTMP_PublishToStudentFlg == true
                                        && i.ISMS_Id == EYCES.ISMS_Id && i.MI_Id == data.MI_Id && n.AMST_Id == data.AMST_Id && n.ESTSU_ActiveFlg == true)
                                        select new StudentKioskEXAMDTO
                                        {
                                            ISMS_Id = EYCES.ISMS_Id.ToString(),
                                            ISMS_SubjectName = i.ISMS_SubjectName,
                                            ISMS_SubjectCode = i.ISMS_SubjectCode,
                                        }).Distinct().OrderBy(a => a.ISMS_SubjectName).ToArray();
                }
                else
                {
                    var EQuery = _studentDashboardContext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == asmclid && t.ASMS_Id == asmsid && t.AMST_Id == data.AMST_Id && t.ESTMP_PublishToStudentFlg == true).Select(d => d.EME_Id).Distinct().ToList();

                    List<long> emeidnew = new List<long>();

                    var getexamlist = (from a in _studentDashboardContext.Exm_Category_ClassDMO
                                       from b in _studentDashboardContext.Exm_Master_CategoryDMO
                                       from c in _studentDashboardContext.Exm_Yearly_CategoryDMO
                                       from d in _studentDashboardContext.Exm_Yearly_Category_ExamsDMO
                                       from e in _studentDashboardContext.ExmStudentMarksProcessDMO
                                       where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && c.EYC_Id == d.EYC_Id && d.EME_Id == e.EME_Id && a.ECAC_ActiveFlag == true
                                       && b.EMCA_ActiveFlag == true && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true && a.ASMCL_Id == asmclid && a.ASMS_Id == asmsid
                                       && a.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == asmclid && e.ASMS_Id == asmsid && e.ASMAY_Id == data.ASMAY_Id
                                       && e.AMST_Id == data.AMST_Id &&
                                       ((d.EYCE_MarksPublishDate == null && e.ESTMP_PublishToStudentFlg == true)
                                       || (d.EYCE_MarksPublishDate != null && e.ESTMP_PublishToStudentFlg == false
                                       && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(d.EYCE_MarksPublishDate))
                                       || (d.EYCE_MarksPublishDate != null && e.ESTMP_PublishToStudentFlg == true && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(d.EYCE_MarksPublishDate))))
                                       select new StudentKioskEXAMDTO
                                       {
                                           EME_Id = d.EME_Id.ToString()
                                       }).Distinct().ToList();

                    foreach (var e in getexamlist)
                    {
                        emeidnew.Add(Convert.ToInt64(e.EME_Id));
                    }

                    List<exammasterDMO> esmp = new List<exammasterDMO>();
                    esmp = _studentDashboardContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && emeidnew.Contains(t.EME_Id)).ToList();
                    data.examlist = esmp.OrderBy(a => a.EME_ExamOrder).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<StudentKioskFEEDTO> getloaddataFEE(StudentKioskFEEDTO data)
        {
            try
            {
                var ASMAY_Id = _studentDashboardContext.AcademicYearDMO.Where(y => y.Is_Active == true && y.MI_Id == data.MI_Id).OrderByDescending(y => y.ASMAY_Order).FirstOrDefault().ASMAY_Id;

                var studentfeedetails = (from c in _studentDashboardContext.FeeStudentTransactionDMO
                                         from a in _studentDashboardContext.feeMTH
                                         from b in _studentDashboardContext.feeTr
                                         from d in _studentDashboardContext.FeeHeadDMO
                                         where (a.FMT_Id == b.FMT_Id && c.FMH_Id == d.FMH_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == c.FTI_Id && c.MI_Id == data.MI_Id && c.ASMAY_Id == ASMAY_Id && c.AMST_Id == data.AMST_Id && c.FSS_NetAmount > 0)
                                         select new StudentKioskFEEDTO
                                         {
                                             FSS_CurrentYrCharges = c.FSS_CurrentYrCharges,
                                             FSS_ToBePaid = c.FSS_ToBePaid,
                                             FSS_PaidAmount = c.FSS_PaidAmount,
                                             FSS_ConcessionAmount = c.FSS_ConcessionAmount,
                                             FTI_Name = b.FMT_Name,
                                             FMH_FeeName = d.FMH_FeeName
                                         }).ToList();

                data.studentfeedetails = (from i in studentfeedetails
                                          group i by new { i.FTI_Name, i.FMH_FeeName } into g
                                          select new StudentKioskFEEDTO
                                          {
                                              FSS_CurrentYrCharges = g.Sum(t => t.FSS_CurrentYrCharges),
                                              FSS_ToBePaid = g.Sum(t => t.FSS_ToBePaid),
                                              FSS_PaidAmount = g.Sum(t => t.FSS_PaidAmount),
                                              FSS_ConcessionAmount = g.Sum(t => t.FSS_ConcessionAmount),
                                              FTI_Name = g.Key.FTI_Name,
                                              FMH_FeeName = g.Key.FMH_FeeName
                                          }).Distinct().ToArray();

                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_Cumulative_Fee_Analysis";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id",SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.feeAnalysisList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentKioskSPORTSDTO kioskSportsWinners(StudentKioskSPORTSDTO kiosk)
        {
            StudentKioskSPORTSDTO obj = new StudentKioskSPORTSDTO();
            try
            {
                List<StudentKioskSPORTSDTO> result = new List<StudentKioskSPORTSDTO>();
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sports_Winners_Kiosk";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = kiosk.MI_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new StudentKioskSPORTSDTO
                                {
                                    eventName = dataReader["SPCCME_EventName"].ToString(),
                                    studentName = dataReader["AMST_FirstName"].ToString(),
                                    sportsName = dataReader["SPCCMSCC_SportsCCName"].ToString(),
                                    SPCCESTR_Place = Convert.ToInt32(dataReader["SPCCESTR_Place"].ToString()),
                                    studentPhotoPath = dataReader["AMST_Photoname"].ToString()

                                });
                                obj.winnerList = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }

        public async Task<StudentKioskBIRTHDAYDTO> getstaffdetails(StudentKioskBIRTHDAYDTO data)
        {
            DateTime fromdatecon = DateTime.Now;
            string confromdate = "";
            fromdatecon = Convert.ToDateTime(data.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
            confromdate = fromdatecon.ToString("yyyy-MM-dd");

            DateTime todatecon = DateTime.Now;
            string contodate = "";
            todatecon = Convert.ToDateTime(data.Todate.Value.Date.ToString("yyyy-MM-dd"));
            contodate = todatecon.ToString("yyyy-MM-dd");

            using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Staff_Birthday_Report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@month",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToString(data.months)
                });
                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                {
                    Value = confromdate
                });
                cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                {
                    Value = contodate
                });
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.MI_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@all1", SqlDbType.Text)
                {
                    Value = Convert.ToString(data.all1)
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
                                    dataReader.IsDBNull(iFiled) ? "N/A" : dataReader[iFiled]
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.staffDetails = retObject.ToArray();
                    if (data.staffDetails.Length > 0)
                    {
                        data.count = data.staffDetails.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return data;
        }

        public async Task<StudentKioskHomeWorkDTO> GetHomeWorkdetailsKiosk(StudentKioskHomeWorkDTO data)
        {
            try
            {
                #region  Class/Section
                var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                               from c in _studentDashboardContext.School_M_Class
                               from s in _studentDashboardContext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                               select new StudentKioskHomeWorkDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();
                #endregion

                if (clssec1.Count == 0)
                {
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;


                    #region Student Home Work
                    data.homeworklist = (from a in _studentDashboardContext.IVRM_Homework_DMO
                                         from b in _studentDashboardContext.IVRM_Master_SubjectsDMO
                                         from c in _studentDashboardContext.School_Adm_Y_StudentDMO
                                         from d in _studentDashboardContext.AcademicYearDMO
                                         where (a.ISMS_Id == b.ISMS_Id && a.ASMCL_Id == c.ASMCL_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                         && a.ASMCL_Id == Class_Id && a.ASMS_Id == Section_Id
                                         && a.MI_Id == data.MI_Id && c.AMAY_ActiveFlag == 1
                                         && a.IHW_ActiveFlag == true)
                                         select new StudentKioskHomeWorkDTO
                                         {
                                             IHW_AssignmentNo = a.IHW_AssignmentNo,
                                             IHW_Date = a.IHW_Date,
                                             IHW_Topic = a.IHW_Topic,
                                             IHW_Assignment = a.IHW_Assignment,
                                             IHW_Attachment = a.IHW_Attachment,
                                             ASMS_Id = a.ASMS_Id,
                                             IHW_FilePath = a.IHW_FilePath,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = b.ISMS_SubjectName,
                                             ASMCL_Id = a.ASMCL_Id
                                         }).Distinct().OrderByDescending(d => d.IHW_Date).ToArray();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<StudentKioskNoticeDTO> GetNoticedetailsKiosk(StudentKioskNoticeDTO data)
        {
            try
            {
                var clssec1 = _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                    && a.AMAY_ActiveFlag == 1).ToList();

                if (clssec1.Count == 0)
                {
                    data.message = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    var date = DateTime.Now;
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_NoticeBoardYearWise";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar)
                        {
                            Value = Class_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",SqlDbType.VarChar)
                        {
                            Value = "O"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",SqlDbType.VarChar)
                        {
                            Value = "Student"
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
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
                            data.noticelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                //#region Student Details
                //using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "PORTAL_StudentDashboard";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                //    {
                //        Value = data.ASMAY_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                //    {
                //        Value = data.AMST_Id
                //    });


                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = await cmd.ExecuteReaderAsync())
                //        {
                //            while (await dataReader.ReadAsync())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }

                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.studetailslist = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
                //#endregion

                //#region  Class/Section
                //var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                //               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                //               from c in _studentDashboardContext.School_M_Class
                //               from s in _studentDashboardContext.School_M_Section
                //               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                //               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                //               select new StudentKioskNoticeDTO
                //               {
                //                   ASMCL_Id = c.ASMCL_Id,
                //                   ASMCL_ClassName = c.ASMCL_ClassName,
                //                   ASMS_Id = s.ASMS_Id,
                //                   ASMC_SectionName = s.ASMC_SectionName
                //               }).Distinct().ToList();
                //#endregion

                //if (clssec1.Count == 0)
                //{
                //}
                //else
                //{
                //    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                //    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                //    #region  Notice Board 
                //    try
                //    {
                //        var date = DateTime.Now;
                //        using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                //        {
                //            cmd.CommandText = "Portal_NoticeBoardYearWise";
                //            cmd.CommandType = CommandType.StoredProcedure;

                //            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //     SqlDbType.VarChar)
                //            {
                //                Value = data.MI_Id
                //            });
                //            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                //            SqlDbType.VarChar)
                //            {
                //                Value = Class_Id
                //            });
                //            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //        SqlDbType.VarChar)
                //            {
                //                Value = data.ASMAY_Id
                //            });

                //            if (cmd.Connection.State != ConnectionState.Open)
                //                cmd.Connection.Open();

                //            var retObject = new List<dynamic>();
                //            try
                //            {
                //                using (var dataReader = await cmd.ExecuteReaderAsync())
                //                {
                //                    while (await dataReader.ReadAsync())
                //                    {
                //                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                        {
                //                            dataRow.Add(
                //                                dataReader.GetName(iFiled),
                //                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                            );
                //                        }

                //                        retObject.Add((ExpandoObject)dataRow);
                //                    }
                //                }
                //                data.noticelist = retObject.ToArray();
                //            }
                //            catch (Exception ex)
                //            {
                //                Console.WriteLine(ex.Message);
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //    #endregion
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentKioskBIRTHDAYDTO getstudentBD(StudentKioskBIRTHDAYDTO stu1)
        {
            try
            {
                var acd_Id = _studentDashboardContext.AcademicYearDMO.Where(t => t.MI_Id.Equals(stu1.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                if (stu1.rdbbutton.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    stu1.studentlist = (from a in _studentDashboardContext.Adm_M_Student
                                        from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                                        from c in _studentDashboardContext.School_M_Class
                                        from d in _studentDashboardContext.School_M_Section
                                        where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMAY_Id == acd_Id && d.ASMS_Id == b.ASMS_Id && a.MI_Id == stu1.MI_Id &&
                                        a.AMST_ActiveFlag == 1 && a.AMST_SOL.Equals("S") && b.AMAY_ActiveFlag == 1 && a.AMST_DOB.Date.Day == DateTime.Now.Day && a.AMST_DOB.Date.Month == DateTime.Now.Month)
                                        select new StudentKioskBIRTHDAYDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                            AMST_emailId = a.AMST_emailId == null || a.AMST_emailId == "" ? "" : a.AMST_emailId,
                                            AMST_MobileNo = a.AMST_MobileNo == 0 || a.AMST_MobileNo == null ? 0 : a.AMST_MobileNo,
                                            amst_dob = a.AMST_DOB,
                                            ASMCL_ClassName = c.ASMCL_ClassName,
                                            ASMC_SectionName = d.ASMC_SectionName,
                                            PhotoPath = (string.IsNullOrEmpty(a.AMST_Photoname) ? "" : a.AMST_Photoname)
                                        }).Distinct().ToArray();
                    if (stu1.studentlist.Length > 0)
                    {
                        stu1.count = stu1.studentlist.Length;
                    }
                    else
                    {
                        stu1.count = 0;
                    }
                }
                else if (stu1.rdbbutton.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    stu1.staffList = (from a in _studentDashboardContext.HR_Master_Employee_DMO
                                      from b in _studentDashboardContext.Multiple_Email_DMO
                                      from c in _studentDashboardContext.Multiple_Mobile_DMO
                                      where (a.HRME_Id == b.HRME_Id && b.HRME_Id == c.HRME_Id && b.HRMEM_DeFaultFlag.Equals("default") && c.HRMEMNO_DeFaultFlag.Equals("default") && a.MI_Id == stu1.MI_Id && a.HRME_ActiveFlag == true && a.HRME_DOB.Value.Date.Day == DateTime.Now.Day && a.HRME_DOB.Value.Date.Month == DateTime.Now.Month && a.HRME_LeftFlag == false)
                                      select new StudentKioskBIRTHDAYDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          employeeName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                          HRME_EmailId = b.HRMEM_EmailId == null || b.HRMEM_EmailId == "" ? "" : b.HRMEM_EmailId,
                                          HRME_MobileNo = c.HRMEMNO_MobileNo == 0 || c.HRMEMNO_MobileNo == null ? 0 : c.HRMEMNO_MobileNo,
                                          HRME_DOB = a.HRME_DOB,
                                          PhotoPath = (string.IsNullOrEmpty(a.HRME_Photo) ? "" : a.HRME_Photo)
                                      }).Distinct().ToArray();
                    if (stu1.staffList.Length > 0)
                    {
                        stu1.count = stu1.staffList.Length;
                    }
                    else
                    {
                        stu1.count = 0;
                    }
                }
                if (stu1.rdbbutton.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                {
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ALUMNI_BirthdayList_Day";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = stu1.MI_Id });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
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
                            stu1.studentlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                    if (stu1.studentlist.Length > 0)
                    {
                        stu1.count = stu1.studentlist.Length;
                    }
                    else
                    {
                        stu1.count = 0;
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu1;
        }

        public async Task<StudentKioskTimeTableDTO> getStudentTT(StudentKioskTimeTableDTO data)
        {
            try
            {
                var clssec = (from a in _studentDashboardContext.Adm_M_Student
                              from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                              from c in _studentDashboardContext.School_M_Class
                              from s in _studentDashboardContext.School_M_Section
                              where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                              select new StudentKioskTimeTableDTO
                              {
                                  ASMCL_Id = c.ASMCL_Id,
                                  ASMCL_ClassName = c.ASMCL_ClassName,
                                  ASMS_Id = s.ASMS_Id,
                                  ASMC_SectionName = s.ASMC_SectionName

                              }).Distinct().ToArray();

                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_StudentTT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Asmay_id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                    SqlDbType.BigInt)
                    {
                        Value = clssec.FirstOrDefault().ASMCL_Id,
                    });

                    cmd.Parameters.Add(new SqlParameter("@asms_id",
                    SqlDbType.VarChar)
                    {
                        Value = clssec.FirstOrDefault().ASMS_Id,
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
                        data.getStudentTT = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
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
