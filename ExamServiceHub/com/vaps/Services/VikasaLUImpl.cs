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

namespace ExamServiceHub.com.vaps.Services
{
    public class VikasaLUImpl : VikasaLUInterface
    {
        public ExamContext _examctxt;

        public VikasaLUImpl(ExamContext obj)
        {
            _examctxt = obj;
        }
        public VikasaLUReportDTO getdetails(VikasaLUReportDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();
                data.Acdlist = list.OrderByDescending(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public VikasaLUReportDTO onselectAcdYear(VikasaLUReportDTO data)
        {
            try
            {
                data.ctlist = (from c in _examctxt.AdmissionClass
                               from d in _examctxt.Exm_Category_ClassDMO
                               where (c.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMCL_ActiveFlag == true
                               && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == c.ASMCL_Id && d.ECAC_ActiveFlag == true)
                               select c).Distinct().OrderBy(c => c.ASMCL_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public VikasaLUReportDTO onselectclass(VikasaLUReportDTO data)
        {
            try
            {
                data.seclist = (from c in _examctxt.School_M_Section
                                from b in _examctxt.Exm_Category_ClassDMO
                                where (b.ASMCL_Id == data.ASMCL_Id && c.MI_Id == data.MI_Id && c.ASMS_Id == b.ASMS_Id && c.ASMC_ActiveFlag == 1 && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == b.ASMCL_Id && b.ECAC_ActiveFlag == true && c.ASMS_Id == b.ASMS_Id)
                                select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public VikasaLUReportDTO onselectSection(VikasaLUReportDTO data)
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

                var EMCA_Id = _examctxt.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;
                var EYC_Id = _examctxt.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id && t.EYC_ActiveFlg == true).EYC_Id;

                var examlist = (from a in _examctxt.masterexam
                                from b in _examctxt.Exm_Yearly_Category_ExamsDMO
                                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYC_Id == EYC_Id && b.EYCE_ActiveFlg == true)
                                select a).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();
                data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                data.studentlist = (from a in _examctxt.Adm_M_Student
                                    from b in _examctxt.School_Adm_Y_StudentDMO
                                    where (a.MI_Id == data.MI_Id && sol.Contains(a.AMST_SOL) && ids.Contains(a.AMST_ActiveFlag) && b.AMST_Id == a.AMST_Id 
                                    && b.ASMAY_Id == data.ASMAY_Id && ids.Contains(b.AMAY_ActiveFlag) && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                    select new VikasaLUReportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                        AMST_AdmNo = a.AMST_AdmNo == null ? "" : a.AMST_AdmNo,
                                        AMAY_RollNo = b.AMAY_RollNo,
                                        AMST_DOB = a.AMST_DOB
                                    }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public VikasaLUReportDTO onreport(VikasaLUReportDTO data)
        {
            try
            {
                data.datareport = (from a in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                   from b in _examctxt.Exm_Student_Marks_Pro_Sub_SubSubjectDMO
                                   from c in _examctxt.IVRM_School_Master_SubjectsDMO
                                   from d in _examctxt.mastersubsubject
                                   from e in _examctxt.exammasterDMO
                                   where (a.MI_Id == data.MI_Id && a.ESTMPS_Id == b.ESTMPS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == c.ISMS_Id && d.EMSS_Id == b.EMSS_Id && e.EME_Id == a.EME_Id)
                                   select new VikasaLUReportDTO
                                   {
                                       AMST_Id = a.AMST_Id,
                                       ISMS_SubjectName = c.ISMS_SubjectName,
                                       EMSS_SubSubjectName = d.EMSS_SubSubjectName,
                                       ISMS_Id = a.ISMS_Id,
                                       EMSS_Id = d.EMSS_Id,
                                       ESTMPSSS_ObtainedGrade = b.ESTMPSSS_ObtainedGrade,
                                       ESTMPSSS_PassFailFlg = b.ESTMPSSS_PassFailFlg,
                                       overallgrade = a.ESTMPS_ObtainedGrade

                                   }).Distinct().OrderBy(t => t.AMST_Id).ThenBy(t => t.ISMS_Id).ToArray();

                data.classteacher = (from a in _examctxt.ClassTeacherMappingDMO
                                     from b in _examctxt.HR_Master_Employee_DMO
                                     where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                     select new BaldwinAllReportDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                     }).Distinct().ToArray();



                List<VikasaSubjectwiseCumulativeReportDTO> result = new List<VikasaSubjectwiseCumulativeReportDTO>();
                using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Exam_get_Attendance_Exam_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMS_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                       SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.EME_Id)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        using (var dataReader = cmd.ExecuteReaderAsync().Result)
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.attendance = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                List<VikasaSubjectwiseCumulativeReportDTO> result1 = new List<VikasaSubjectwiseCumulativeReportDTO>();
                using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Exam_Sports_Remarks_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMS_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.EME_Id)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReaderAsync().Result)
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.sports = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                List<VikasaSubjectwiseCumulativeReportDTO> result2 = new List<VikasaSubjectwiseCumulativeReportDTO>();
                using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Exam_get_Promotion_Remarks_Exam_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMS_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.EME_Id)
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        using (var dataReader = cmd.ExecuteReaderAsync().Result)
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.studentlist = retObject.ToArray();
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
    }
}
