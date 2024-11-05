using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vaps.Exam;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Services
{
    public class JSHSProtal_StudentReportsImpl : Interfaces.JSHSPortal_StudentReportsInterface
    {
        public ExamContext _context;
        public PortalContext _contextp;
        public JSHSProtal_StudentReportsImpl(PortalContext _por)
        {

            _contextp = _por;
        }
        public JSHSPortal_StudentReportsDTO Getdetails_IT(JSHSPortal_StudentReportsDTO data)
        {
            try
            {

                data.getyearlist = _contextp.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSPortal_StudentReportsDTO get_Terms_IT(JSHSPortal_StudentReportsDTO data)
        {
            try
            {
                var clssec1 = (from a in _contextp.Adm_M_Student
                               from b in _contextp.School_Adm_Y_StudentDMO
                               from c in _contextp.School_M_Class
                               from s in _contextp.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id && a.AMST_SOL=="S"
                               && a.AMST_ActiveFlag==1 && b.AMAY_ActiveFlag==1)
                               select new JSHSPortal_StudentReportsDTO
                               {
                                   ASMCL_Id = b.ASMCL_Id,
                                   ASMS_Id = b.ASMS_Id,
                               }).Distinct().ToList();

                data.Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                data.Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                var getemcaid = _contextp.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.Class_Id && a.ASMS_Id == data.Section_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();


                data.getgradelist = (from a in _contextp.Exm_Yearly_CategoryDMO
                                     from b in _contextp.Exm_Yearly_Category_ExamsDMO
                                     from c in _contextp.Exm_Master_GradeDMO
                                     where (a.EYC_Id == b.EYC_Id && b.EMGR_Id == c.EMGR_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                                     && a.ASMAY_Id == data.ASMAY_Id)
                                     select c).Distinct().ToArray();

                data.gettermlist = _contextp.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
             && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().OrderBy(a => a.ECT_TermName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // Multiple Term Report
        public JSHSPortal_StudentReportsDTO get_reportdetails_IT(JSHSPortal_StudentReportsDTO data)
        {
            try
            {
                data.getgradedetails = (from a in _contextp.Exm_Master_GradeDMO
                                        from b in _contextp.Exm_Master_Grade_DetailsDMO
                                        where (a.EMGR_Id == b.EMGR_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && b.EMGR_Id == data.EMGR_Id)
                                        select b).Distinct().OrderByDescending(a => a.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();

                var clssec1 = (from a in _contextp.Adm_M_Student
                               from b in _contextp.School_Adm_Y_StudentDMO
                               from c in _contextp.School_M_Class
                               from s in _contextp.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id && a.AMST_SOL == "S"
                               && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                               select new JSHSPortal_StudentReportsDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();

                data.Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                data.Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                List<long> termid = new List<long>();

                foreach (var c in data.termlist)
                {
                    termid.Add(c.ECT_Id);
                }

                var termidnew = "";

                for (int k = 0; k < data.termlist.Length; k++)
                {
                    if (k == 0)
                    {
                        termidnew = data.termlist[k].ECT_Id.ToString();
                    }
                    else
                    {
                        termidnew = termidnew + ',' + data.termlist[k].ECT_Id.ToString();
                    }
                }

                data.gettermdetails = _contextp.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && termid.Contains(a.ECT_Id)).ToArray();


                data.gettermexamdetails = (from a in _contextp.CCE_Exam_M_TermsDMO
                                           from b in _contextp.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _contextp.exammasterDMO
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true && c.EME_ActiveFlag == true && termid.Contains(b.ECT_Id))
                                           select new JSHSPortal_StudentReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue

                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                // STUDENT DETAILS //
                using (var cmd = _contextp.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Report_Details";
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
                        Value = data.Class_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.Section_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
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
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS //
                using (var cmd = _contextp.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Report_Details";
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
                        Value = data.Class_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.Section_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 2
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
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SKILLS LIST
                using (var cmd = _contextp.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Skills_Activites_Report_Details";
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
                        Value = data.Class_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.Section_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = termidnew
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
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _contextp.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Skills_Activites_Report_Details";
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
                        Value = data.Class_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.Section_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 2
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = termidnew
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
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SPORTS LIST
                using (var cmd = _contextp.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Skills_Activites_Report_Details";
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
                        Value = data.Class_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.Section_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 3
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = termidnew
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
                        data.getstudentwisesportsdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE LIST
                using (var cmd = _contextp.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Skills_Activites_Report_Details";
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
                        Value = data.Class_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.Section_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 4
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = termidnew
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
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE TREM WISE REMARKS LIST
                using (var cmd = _contextp.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Skills_Activites_Report_Details";
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
                        Value = data.Class_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.Section_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 5
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = termidnew
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
                        data.getstudentwisetermwisedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // GET STUDENT WISE EXAM MARKS LIST
                using (var cmd = _contextp.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_CCE_Term_Report";
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
                        Value = data.Class_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.Section_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                        SqlDbType.VarChar)
                    {
                        Value = termidnew
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.EMGR_Id
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
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public JSHSPortal_StudentReportsDTO get_Exam_grade_pc(JSHSPortal_StudentReportsDTO data)
        {
            try
            {
                var clssec1 = (from a in _contextp.Adm_M_Student
                               from b in _contextp.School_Adm_Y_StudentDMO
                               from c in _contextp.School_M_Class
                               from s in _contextp.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                               select new JSHSPortal_StudentReportsDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();

                data.Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                data.Section_Id = clssec1.FirstOrDefault().ASMS_Id;



                var getemcaid = _contextp.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.Class_Id
               && a.ASMS_Id == data.Section_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var eycid = _contextp.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                && a.EYC_ActiveFlg == true).ToList();


                List<long> emeidnew = new List<long>();

                var getexamlist = (from a in _contextp.Exm_Category_ClassDMO
                                   from b in _contextp.Exm_Master_CategoryDMO
                                   from c in _contextp.Exm_Yearly_CategoryDMO
                                   from d in _contextp.Exm_Yearly_Category_ExamsDMO
                                   from e in _contextp.ExmStudentMarksProcessDMO
                                   where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && c.EYC_Id == d.EYC_Id 
                                   && d.EME_Id == e.EME_Id && a.ECAC_ActiveFlag == true
                                   && b.EMCA_ActiveFlag == true && c.EYC_ActiveFlg == true 
                                   && d.EYCE_ActiveFlg == true && a.ASMCL_Id == data.Class_Id 
                                   && a.ASMS_Id == data.Section_Id && a.ASMAY_Id == data.ASMAY_Id 
                                   && e.ASMCL_Id == data.Class_Id 
                                   && e.ASMS_Id == data.Section_Id && e.ASMAY_Id == data.ASMAY_Id
                                   && e.AMST_Id == data.AMST_Id &&
                                   ((d.EYCE_MarksPublishDate == null && e.ESTMP_PublishToStudentFlg == true)
                                   || (d.EYCE_MarksPublishDate != null && e.ESTMP_PublishToStudentFlg == false
                                   && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(d.EYCE_MarksPublishDate))
                                   || (d.EYCE_MarksPublishDate != null && e.ESTMP_PublishToStudentFlg == true && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(d.EYCE_MarksPublishDate))))
                                   select new exammasterDMO
                                   {
                                       EME_Id = d.EME_Id
                                   }).Distinct().ToList();

                foreach (var e in getexamlist)
                {
                    emeidnew.Add(e.EME_Id);
                }

                //var getexam = (from a in _contextp.Exm_Yearly_CategoryDMO
                //               from b in _contextp.Exm_Yearly_Category_ExamsDMO
                //               from c in _contextp.exammasterDMO
                //               where (a.EYC_Id == b.EYC_Id && b.EME_Id == c.EME_Id && a.EYC_ActiveFlg == true && b.EYCE_ActiveFlg == true && c.EME_ActiveFlag == true
                //               && c.MI_Id == data.MI_Id && a.EYC_Id == eycid.FirstOrDefault().EYC_Id && b.EYC_Id == eycid.FirstOrDefault().EYC_Id && emeidnew.Contains(b.EME_Id)
                //               && a.ASMAY_Id == data.ASMAY_Id)
                //               select c).Distinct().OrderBy(a => a.EME_ExamOrder).ToList();

                //data.getexam = getexam.ToArray();


                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _contextp.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && emeidnew.Contains(t.EME_Id)).ToList();

                data.getexam = esmp.OrderBy(a => a.EME_ExamOrder).Distinct().ToArray();



                data.getgradelist = (from a in _contextp.Exm_Yearly_CategoryDMO
                                     from b in _contextp.Exm_Yearly_Category_ExamsDMO
                                     from c in _contextp.Exm_Master_GradeDMO
                                     where (a.EYC_Id == b.EYC_Id && b.EMGR_Id == c.EMGR_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                                     && a.ASMAY_Id == data.ASMAY_Id)
                                     select c).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // Individual Exam Wise Progress card report
        public async Task<JSHSPortal_ProgressCardReportDTO> saveddata_pc(JSHSPortal_ProgressCardReportDTO data)
        {
            try
            {
                data.instname = _contextp.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                var clssec1 = (from a in _contextp.Adm_M_Student
                               from b in _contextp.School_Adm_Y_StudentDMO
                               from c in _contextp.School_M_Class
                               from s in _contextp.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                               select new JSHSPortal_ProgressCardReportDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();
                data.Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                data.Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                data.clstchname = (from a in _contextp.ClassTeacherMappingDMO
                                   from b in _contextp.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.Class_Id && a.ASMS_Id == data.Section_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSPortal_ProgressCardReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                List<JSHSPortal_ProgressCardReportDTO> result = new List<JSHSPortal_ProgressCardReportDTO>();

                using (var cmd = _contextp.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });


                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.Class_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.Section_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.EME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                                SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(data.AMST_Id)
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
                                result.Add(new JSHSPortal_ProgressCardReportDTO
                                {
                                    ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                                    ESTMPS_ObtainedGrade = (dataReader["ESTMPS_ObtainedGrade"].ToString() == null || dataReader["ESTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ESTMPS_ObtainedGrade"].ToString()),
                                    ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                                    EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),
                                    ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString() == null || dataReader["ASMCL_ClassName"].ToString() == "" ? "" : dataReader["ASMCL_ClassName"].ToString()),
                                    ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString() == null || dataReader["ASMC_SectionName"].ToString() == "" ? "" : dataReader["ASMC_SectionName"].ToString()),

                                    AMST_FatherName = (dataReader["AMST_FatherName"].ToString() == null || dataReader["AMST_FatherName"].ToString() == "" ? "" : dataReader["AMST_FatherName"].ToString()),
                                    AMST_MotherName = (dataReader["AMST_MotherName"].ToString() == null || dataReader["AMST_MotherName"].ToString() == "" ? "" : dataReader["AMST_MotherName"].ToString()),

                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                    AMST_FirstName = ((dataReader["AMST_FirstName"].ToString() == null ? " " : dataReader["AMST_FirstName"].ToString()) + " " + (dataReader["AMST_MiddleName"].ToString() == null ? " " : dataReader["AMST_MiddleName"].ToString()) + " " + (dataReader["AMST_LastName"].ToString() == null ? " " : dataReader["AMST_LastName"].ToString())).Trim(),
                                    AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"].ToString() == null || dataReader["AMST_DOB"].ToString() == "" ? "" : dataReader["AMST_DOB"].ToString()),
                                    AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                                    AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                    ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),
                                    ESTMPS_MaxMarks = Convert.ToDecimal(dataReader["ESTMPS_MaxMarks"].ToString() == null || dataReader["ESTMPS_MaxMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_MaxMarks"].ToString()),
                                    ESTMPS_ClassAverage = Convert.ToDecimal(dataReader["ESTMPS_ClassAverage"].ToString() == null || dataReader["ESTMPS_ClassAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassAverage"].ToString()),
                                    ESTMPS_SectionAverage = Convert.ToDecimal(dataReader["ESTMPS_SectionAverage"].ToString() == null || dataReader["ESTMPS_SectionAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionAverage"].ToString()),
                                    ESTMPS_ClassHighest = Convert.ToDecimal(dataReader["ESTMPS_ClassHighest"].ToString() == null || dataReader["ESTMPS_ClassHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassHighest"].ToString()),
                                    ESTMPS_SectionHighest = Convert.ToDecimal(dataReader["ESTMPS_SectionHighest"].ToString() == null || dataReader["ESTMPS_SectionHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionHighest"].ToString()),
                                    ISMS_SubjectCode = (dataReader["ISMS_SubjectCode"].ToString() == null || dataReader["ISMS_SubjectCode"].ToString() == "" ? "" : dataReader["ISMS_SubjectCode"].ToString()),
                                    EYCES_AplResultFlg = Convert.ToBoolean(dataReader["EYCES_AplResultFlg"].ToString()),
                                    EYCES_MaxMarks = Convert.ToDecimal(dataReader["EYCES_MaxMarks"].ToString() == null || dataReader["EYCES_MaxMarks"].ToString() == "" ? "0" : dataReader["EYCES_MaxMarks"].ToString()),
                                    EYCES_MinMarks = Convert.ToDecimal(dataReader["EYCES_MinMarks"].ToString() == null || dataReader["EYCES_MinMarks"].ToString() == "" ? "0" : dataReader["EYCES_MinMarks"].ToString()),
                                    EMGR_Id = Convert.ToInt32(dataReader["EMGR_Id"].ToString()),
                                    classheld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),
                                    classatt = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),
                                    graderemark = (dataReader["EMGD_Remarks"].ToString() == null || dataReader["EMGD_Remarks"].ToString() == "" ? "0" : dataReader["EMGD_Remarks"].ToString()),

                                    ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString() == null || dataReader["ESTMP_TotalObtMarks"].ToString() == "" ? "0" : dataReader["ESTMP_TotalObtMarks"].ToString()),
                                    ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString() == null || dataReader["ESTMP_Percentage"].ToString() == "" ? "0" : dataReader["ESTMP_Percentage"].ToString()),
                                    ESTMP_TotalGrade = (dataReader["ESTMP_TotalGrade"].ToString() == null || dataReader["ESTMP_TotalGrade"].ToString() == "" ? "" : dataReader["ESTMP_TotalGrade"].ToString()),
                                    ESTMP_ClassRank = Convert.ToInt16(dataReader["ESTMP_ClassRank"].ToString() == null || dataReader["ESTMP_ClassRank"].ToString() == "" ? "" : dataReader["ESTMP_ClassRank"].ToString()),
                                    ESTMP_SectionRank = Convert.ToInt16(dataReader["ESTMP_SectionRank"].ToString() == null || dataReader["ESTMP_SectionRank"].ToString() == "" ? "" : dataReader["ESTMP_SectionRank"].ToString()),
                                    ESTMP_TotalGradeRemark = (dataReader["ESTMP_TotalGradeRemark"].ToString() == null || dataReader["ESTMP_TotalGradeRemark"].ToString() == "" ? "" : dataReader["ESTMP_TotalGradeRemark"].ToString()),
                                    ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString() == null || dataReader["ESTMP_TotalMaxMarks"].ToString() == "0" ? "" : dataReader["ESTMP_TotalMaxMarks"].ToString()),
                                    EYCES_SubjectOrder = Convert.ToInt16(dataReader["EYCES_SubjectOrder"].ToString() == null || dataReader["EYCES_SubjectOrder"].ToString() == "" ? "" : dataReader["EYCES_SubjectOrder"].ToString()),
                                    EYCES_MarksDisplayFlg = Convert.ToBoolean(dataReader["EYCES_MarksDisplayFlg"].ToString()),
                                    EYCES_GradeDisplayFlg = Convert.ToBoolean(dataReader["EYCES_GradeDisplayFlg"].ToString()),
                                    ESTMP_Result = (dataReader["ESTMP_Result"].ToString() == null || dataReader["ESTMP_Result"].ToString() == "" ? "" : dataReader["ESTMP_Result"].ToString())
                                });

                                data.savelist = result.OrderBy(t => t.EYCES_SubjectOrder).ToList();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

                var from_date = (from a in _contextp.Exm_Category_ClassDMO
                                 from b in _contextp.Exm_Yearly_CategoryDMO
                                 from c in _contextp.Exm_Yearly_Category_ExamsDMO
                                 where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.Class_Id && a.ASMS_Id == data.Section_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id && b.ASMAY_Id == data.ASMAY_Id)
                                 select c.EYCE_AttendanceFromDate).FirstOrDefault();
                var to_date = (from a in _contextp.Exm_Category_ClassDMO
                               from b in _contextp.Exm_Yearly_CategoryDMO
                               from c in _contextp.Exm_Yearly_Category_ExamsDMO
                               where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.Class_Id && a.ASMS_Id == data.Section_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id && b.ASMAY_Id == data.ASMAY_Id)
                               select c.EYCE_AttendanceToDate).Max();

                using (var cmd = _contextp.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "adm_exam_student_attendance_details";
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

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.Class_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.Section_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@from",
                        SqlDbType.Date)
                    {
                        Value = from_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@to",
                        SqlDbType.Date)
                    {
                        Value = to_date
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                            //data.savelist = retObject.ToArray();

                        }
                        data.Present_attendence = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.savelisttot = _contextp.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.Class_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.Section_Id && t.EME_Id == data.EME_Id).Distinct().ToArray();

                data.subjlist = data.savelist.Distinct<JSHSPortal_ProgressCardReportDTO>(new progressEqualityComparerjhs()).OrderBy(t => t.EYCES_SubjectOrder).ToArray();


                List<int> grade = new List<int>();
                foreach (JSHSPortal_ProgressCardReportDTO x in data.subjlist)
                {
                    grade.Add(x.EMGR_Id);
                }

                data.grade_details = (from a in _contextp.Exm_Master_GradeDMO
                                      from b in _contextp.Exm_Master_Grade_DetailsDMO
                                      where (a.MI_Id == data.MI_Id && grade.Contains(a.EMGR_Id) && a.EMGR_Id == b.EMGR_Id)
                                      select b
                                     ).Distinct().ToArray();

                data.examwiseremarks = _contextp.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.Class_Id && a.ASMS_Id == data.Section_Id && a.EME_ID == data.EME_Id && a.EMER_ActiveFlag == true).Distinct().ToArray();

                data.getstudentdetails = (from a in _contextp.School_Adm_Y_StudentDMO
                                          from b in _contextp.Adm_M_Student
                                          from c in _contextp.School_M_Class
                                          from d in _contextp.School_M_Section
                                          from e in _contextp.AcademicYearDMO
                                          where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id
                                          && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id
                                          && a.ASMCL_Id == data.Class_Id && b.MI_Id == data.MI_Id && a.ASMS_Id == data.Section_Id)
                                          select new JSHSProgressCardReportDTO
                                          {
                                              AMST_Id = a.AMST_Id,
                                              photoname = b.AMST_Photoname

                                          }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        class progressEqualityComparerjhs : IEqualityComparer<JSHSPortal_ProgressCardReportDTO>
        {
            public bool Equals(JSHSPortal_ProgressCardReportDTO b1, JSHSPortal_ProgressCardReportDTO b2)
            {
                if (b2 == null && b1 == null)
                    return true;
                else if (b1 == null | b2 == null)
                    return false;
                else if (b1.ISMS_Id == b2.ISMS_Id)
                    return true;
                else
                    return false;
            }
            public int GetHashCode(JSHSPortal_ProgressCardReportDTO bx)
            {
                int hCode = Convert.ToInt32(bx.ISMS_Id);
                return hCode.GetHashCode();
            }
        }
        //======================================

        // Subject With Multiple Exam Report



    }
}
