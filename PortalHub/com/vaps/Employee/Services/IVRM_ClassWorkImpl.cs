using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.Portals.Student;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Portals.Employee;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DomainModel.Model.com.vaps.admission;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonLibrary;
using Newtonsoft.Json;

namespace PortalHub.com.vaps.Employee.Services
{
    public class IVRM_ClassWorkImpl : Interfaces.IVRM_ClassWorkInterface
    {
        public PortalContext _PrincipalDashboardContext;
        public ExamContext _examcontext;
        public DomainModelMsSqlServerContext _db;
        //public DomainModelMsSqlServerContext _db;
        public IVRM_ClassWorkImpl(PortalContext cpContext, DomainModelMsSqlServerContext db, ExamContext exm)
        {
            _PrincipalDashboardContext = cpContext;
            _db = db;
            _examcontext = exm;
        }


        public IVRM_ClassWorkDTO Getdetails(IVRM_ClassWorkDTO data)
        {
            try
            {
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "IVRM_homeworkclasswork_MarksUpdate_list_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.Login_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Parameter",
                SqlDbType.VarChar)
                    {
                        Value = "Classwork"
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
                        data.marksupdate_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.academicyear = _db.AcademicYear.Where(a => a.ASMAY_Id == data.ASMAY_Id).ToArray();
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = _examcontext.Staff_User_Login.Single(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).Emp_Code;

                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_StaffwiseClassStdata";

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
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
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
                            data.classlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    data.HRME_Id = 0;

                    data.classlist = (from a in _PrincipalDashboardContext.School_M_Class
                                      from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                      from c in _PrincipalDashboardContext.AcademicYearDMO
                                      where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_ActiveFlag == true)
                                      select new IVRM_ClassWorkDTO
                                      {
                                          ASMCL_Id = b.ASMCL_Id,
                                          ASMCL_ClassName = a.ASMCL_ClassName,

                                          ASMCL_Order = a.ASMCL_Order
                                      }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                }




                var chekemployee = _examcontext.Staff_User_Login.Where(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).ToList();
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_ClassHomeworkGrid_List";
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

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.Login_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Type",
            SqlDbType.VarChar)
                    {
                        Value = "Classwork"
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.classwork = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //if (chekemployee.Count() > 0)
                //{
                //    data.HRME_Id = _examcontext.Staff_User_Login.Where(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).FirstOrDefault().Emp_Code;

                //    data.classwork = (from m in _PrincipalDashboardContext.IVRM_ClassWorkDMO
                //                      from n in _PrincipalDashboardContext.School_M_Class
                //                      from o in _PrincipalDashboardContext.School_M_Section
                //                      from p in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                //                      from z in _PrincipalDashboardContext.Staff_User_Login
                //                      where (m.ASMCL_Id == n.ASMCL_Id && m.ASMS_Id == o.ASMS_Id && m.ISMS_Id == p.ISMS_Id && m.Login_Id == z.Id && m.MI_Id == z.MI_Id && m.MI_Id == n.MI_Id && m.MI_Id == data.MI_Id && m.Login_Id == data.Login_Id && m.ASMAY_Id == data.ASMAY_Id)
                //                      select new IVRM_ClassWorkDTO
                //                      {
                //                          ASMCL_ClassName = n.ASMCL_ClassName,
                //                          ASMCL_Id = n.ASMCL_Id,
                //                          ASMC_SectionName = o.ASMC_SectionName,
                //                          ASMS_Id = o.ASMS_Id,
                //                          ISMS_SubjectName = p.ISMS_SubjectName,
                //                          ISMS_Id = p.ISMS_Id,
                //                          ICW_Id = m.ICW_Id,
                //                          ICW_Content = m.ICW_Content,
                //                          ICW_Topic = m.ICW_Topic,
                //                          ICW_SubTopic = m.ICW_SubTopic,
                //                          ICW_FromDate = m.ICW_FromDate,
                //                          ICW_ToDate = m.ICW_ToDate,
                //                          ICW_ActiveFlag = m.ICW_ActiveFlag,
                //                          ICW_Assignment = m.ICW_Assignment,
                //                          ICW_TeachingAid = m.ICW_TeachingAid,
                //                          ICW_Evaluation = m.ICW_Evaluation,
                //                          ICW_Attachment = m.ICW_Attachment,
                //                          ICW_FilePath = m.ICW_FilePath,
                //                          MI_Id = m.MI_Id,
                //                          ASMAY_Id = m.ASMAY_Id

                //                      }).Distinct().OrderByDescending(t => t.ICW_Id).ToArray();
                //}
                //else
                //{
                //    data.classwork = (from m in _PrincipalDashboardContext.IVRM_ClassWorkDMO
                //                      from n in _PrincipalDashboardContext.School_M_Class
                //                      from o in _PrincipalDashboardContext.School_M_Section
                //                      from p in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                //                      where (m.ASMCL_Id == n.ASMCL_Id && m.ASMS_Id == o.ASMS_Id && m.ISMS_Id == p.ISMS_Id && m.MI_Id == n.MI_Id
                //                      && m.MI_Id == data.MI_Id && m.Login_Id == data.Login_Id && m.ASMAY_Id == data.ASMAY_Id)
                //                      select new IVRM_ClassWorkDTO
                //                      {
                //                          ASMCL_ClassName = n.ASMCL_ClassName,
                //                          ASMCL_Id = n.ASMCL_Id,
                //                          ASMC_SectionName = o.ASMC_SectionName,
                //                          ASMS_Id = o.ASMS_Id,
                //                          ISMS_SubjectName = p.ISMS_SubjectName,
                //                          ISMS_Id = p.ISMS_Id,
                //                          ICW_Id = m.ICW_Id,
                //                          ICW_Content = m.ICW_Content,
                //                          ICW_Topic = m.ICW_Topic,
                //                          ICW_SubTopic = m.ICW_SubTopic,
                //                          ICW_FromDate = m.ICW_FromDate,
                //                          ICW_ToDate = m.ICW_ToDate,
                //                          ICW_ActiveFlag = m.ICW_ActiveFlag,
                //                          ICW_Assignment = m.ICW_Assignment,
                //                          ICW_TeachingAid = m.ICW_TeachingAid,
                //                          ICW_Evaluation = m.ICW_Evaluation,
                //                          ICW_Attachment = m.ICW_Attachment,
                //                          ICW_FilePath = m.ICW_FilePath,
                //                          MI_Id = m.MI_Id,
                //                          ASMAY_Id = m.ASMAY_Id

                //                      }).Distinct().OrderByDescending(t => t.ICW_Id).ToArray();
                //}

                data.yearlist = _PrincipalDashboardContext.AcademicYearDMO.Where(s => s.MI_Id == data.MI_Id && s.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();






            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<IVRM_ClassWorkDTO> get_classes(IVRM_ClassWorkDTO data)
        {
            try
            {

                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "IVRM_homeworkclasswork_list";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Login_Id",
                     SqlDbType.BigInt)
                        {
                            Value = data.Login_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Parameter",
                    SqlDbType.VarChar)
                        {
                            Value = "Classwork"
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
                            data.class_gridlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_StaffwiseClassStdata";

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
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
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
                            data.classlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else
                {
                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "IVRM_homeworkclasswork_list";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Login_Id",
                     SqlDbType.BigInt)
                        {
                            Value = data.Login_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Parameter",
                    SqlDbType.VarChar)
                        {
                            Value = "Classwork"
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
                            data.class_gridlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    data.classlist = (from a in _PrincipalDashboardContext.School_M_Class
                                      from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                      from c in _PrincipalDashboardContext.AcademicYearDMO
                                      where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_ActiveFlag == true)
                                      select new IVRM_ClassWorkDTO
                                      {
                                          ASMCL_Id = b.ASMCL_Id,
                                          ASMCL_ClassName = a.ASMCL_ClassName,

                                          ASMCL_Order = a.ASMCL_Order
                                      }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //public async Task<IVRM_ClassWorkDTO> get_classes(IVRM_ClassWorkDTO data)
        //{
        //    try
        //    {
        //        var chekemployee = _examcontext.Staff_User_Login.Where(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).ToList();
        //        if (chekemployee.Count() > 0)
        //        {
        //            data.HRME_Id = _examcontext.Staff_User_Login.Where(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).FirstOrDefault().Emp_Code;
        //        }
        //        else
        //        {
        //            data.HRME_Id = 0;
        //        }

        //        using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
        //        {
        //            //cmd.CommandText = "IVRM_StaffwiseClassStdata";
        //            cmd.CommandText = "IVRM_homeworkclasswork_list";
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //        SqlDbType.BigInt)
        //            {
        //                Value = data.MI_Id
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@Login_Id",
        //             SqlDbType.BigInt)
        //            {
        //                Value = data.Login_Id
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
        //            SqlDbType.BigInt)
        //            {
        //                Value = data.ASMAY_Id
        //            });
        //             cmd.Parameters.Add(new SqlParameter("@Parameter",
        //            SqlDbType.VarChar)
        //            {
        //                Value = "classwork"
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
        //                data.classlist = retObject.ToArray();
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
        //    return data;
        //}
        public IVRM_ClassWorkDTO getsectiondata(IVRM_ClassWorkDTO data)

        {
            try
            {
                List<long> sectionexamidd = new List<long>();

                var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id).Select(t => t.ASMCC_Id).ToArray();

                var secid = _examcontext.AdmSchoolMasterClassCatSec.Where(t => classid.Contains(t.ASMCC_Id)).Select(t => t.ASMS_Id).ToArray();

                var chekemployee = _examcontext.Staff_User_Login.Where(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).ToList();
                if (chekemployee.Count() > 0)
                {
                    data.HRME_Id = _examcontext.Staff_User_Login.Where(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).FirstOrDefault().Emp_Code;

                    var sectionexamid = (from e in _examcontext.Staff_User_Login
                                         from f in _examcontext.Exm_Login_PrivilegeDMO
                                         from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                         where (e.Id == data.Login_Id && f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id
                                         && f.MI_Id == data.MI_Id && i.ASMCL_Id == data.ASMCL_Id && secid.Contains(i.ASMS_Id) && f.ELP_Id == i.ELP_Id
                                         && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true && i.ASMCL_Id == data.ASMCL_Id && i.ASMCL_Id == data.ASMCL_Id)
                                         select new IVRM_ClassWorkDTO
                                         {
                                             ASMS_Id = i.ASMS_Id
                                         }).Distinct().Select(t => t.ASMS_Id).ToArray();
                    foreach (var c in sectionexamid)
                    {
                        sectionexamidd.Add(c);
                    }

                }
                else
                {
                    var sectionexamiddd = secid;
                    foreach (var c in sectionexamiddd)
                    {
                        sectionexamidd.Add(c);
                    }
                }

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _examcontext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1 && sectionexamidd.Contains(t.ASMS_Id)).ToList();
                data.sectionlist = seclist.Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_ClassWorkDTO getsubject(IVRM_ClassWorkDTO data)
        {
            try
            {
                //data.subjectlist = (from d in _PrincipalDashboardContext.Exm_Login_Privilege_SubjectsDMO
                //                     from f in _PrincipalDashboardContext.School_M_Section
                //                     from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                //                     where (d.ASMS_Id==f.ASMS_Id && d.ISMS_Id==g.ISMS_Id && g.MI_Id==data.MI_Id && d.ASMS_Id==data.ASMS_Id)
                //                     select new IVRM_ClassWorkDTO
                //                     {
                //                         ISMS_Id = g.ISMS_Id,
                //                         ISMS_SubjectName = g.ISMS_SubjectName,
                //                         ISMS_SubjectCode = g.ISMS_SubjectCode
                //                     }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                //data.subjectlist = (from d in _PrincipalDashboardContext.Exm_Login_Privilege_SubjectsDMO
                //                    from f in _PrincipalDashboardContext.School_M_Section
                //                    from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                //                    from h in _PrincipalDashboardContext.Staff_User_Login
                //                    from i in _PrincipalDashboardContext.HR_Master_Employee_DMO
                //                    from j in _PrincipalDashboardContext.Exm_Login_PrivilegeDMO
                //                    from k in _PrincipalDashboardContext.School_M_Class
                //                    where (d.ELP_Id == j.ELP_Id && h.IVRMSTAUL_Id == j.Login_Id && i.HRME_Id == h.Emp_Code && k.ASMCL_Id == d.ASMCL_Id
                //                    && d.ASMS_Id == f.ASMS_Id && d.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && d.ASMS_Id == data.ASMS_Id
                //                    && j.ASMAY_Id == data.ASMAY_Id
                //                    && d.ELPs_ActiveFlg == true && j.ELP_ActiveFlg == true && i.HRME_ActiveFlag == true && i.HRME_LeftFlag == false
                //                    && h.Emp_Code == data.HRME_Id)
                //                    select new IVRM_ClassWorkDTO
                //                    {
                //                        ISMS_Id = g.ISMS_Id,
                //                        ISMS_SubjectName = g.ISMS_SubjectName,
                //                        ISMS_SubjectCode = g.ISMS_SubjectCode,
                //                        ASMCL_Order = k.ASMCL_Order,
                //                        ISMS_order = g.ISMS_OrderFlag

                //                    }).Distinct().OrderBy(t => t.ASMCL_Order).ThenBy(a => a.ISMS_order).ToArray();
                List<long> list1 = new List<long>();
                foreach (var item in data.hm_section_list)
                {
                    list1.Add(item.ASMS_Id);
                }

                var chekemployee = _examcontext.Staff_User_Login.Where(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).ToList();
                if (chekemployee.Count() > 0)
                {
                    data.HRME_Id = _examcontext.Staff_User_Login.Where(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).FirstOrDefault().Emp_Code;


                    data.subjectlist = (from d in _PrincipalDashboardContext.Exm_Login_Privilege_SubjectsDMO
                                        from f in _PrincipalDashboardContext.School_M_Section
                                        from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                        from h in _PrincipalDashboardContext.Staff_User_Login
                                        from i in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                        from j in _PrincipalDashboardContext.Exm_Login_PrivilegeDMO
                                        from k in _PrincipalDashboardContext.School_M_Class
                                        where (d.ELP_Id == j.ELP_Id && h.IVRMSTAUL_Id == j.Login_Id && i.HRME_Id == h.Emp_Code && k.ASMCL_Id == d.ASMCL_Id
                                        && d.ASMS_Id == f.ASMS_Id && d.ISMS_Id == g.ISMS_Id && j.ASMAY_Id == data.ASMAY_Id && g.MI_Id == data.MI_Id && list1.Contains(d.ASMS_Id) && d.ELPs_ActiveFlg == true && j.ELP_ActiveFlg == true && i.HRME_ActiveFlag == true && i.HRME_LeftFlag == false && h.Emp_Code == data.HRME_Id && d.ASMCL_Id == data.ASMCL_Id)
                                        select new IVRM_ClassWorkDTO
                                        {
                                            ISMS_Id = g.ISMS_Id,
                                            ISMS_SubjectName = g.ISMS_SubjectName,
                                            ISMS_SubjectCode = g.ISMS_SubjectCode,
                                            //ASMC_SectionName = f.ASMC_SectionName,
                                            ISMS_order = g.ISMS_OrderFlag
                                        }).Distinct().OrderBy(t => t.ISMS_order).ToArray();
                }
                else
                {
                    var getemcaid = _PrincipalDashboardContext.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                     && list1.Contains(a.ASMS_Id)).Select(a => a.EMCA_Id).Distinct().ToList();

                    data.subjectlist = (from a in _PrincipalDashboardContext.Exm_Yearly_CategoryDMO
                                        from b in _PrincipalDashboardContext.Exm_Yearly_Category_GroupDMO
                                        from c in _PrincipalDashboardContext.Exm_Yearly_Category_Group_SubjectsDMO
                                        from d in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO

                                        where a.EYC_Id == b.EYC_Id && b.EYCG_Id == c.EYCG_Id && c.ISMS_Id == d.ISMS_Id && a.EYC_ActiveFlg == true
                                        && b.EYCG_ActiveFlg == true && c.EYCGS_ActiveFlg == true && d.ISMS_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id && getemcaid.Contains(a.EMCA_Id)
                                        select new IVRM_ClassWorkDTO
                                        {
                                            ISMS_Id = d.ISMS_Id,
                                            ISMS_SubjectName = d.ISMS_SubjectName,
                                            ISMS_SubjectCode = d.ISMS_SubjectCode,

                                            ISMS_order = d.ISMS_OrderFlag
                                        }).Distinct().OrderBy(t => t.ISMS_order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_ClassWorkDTO savedetail(IVRM_ClassWorkDTO data)
        {
            try
            {
                var header_flg = "ClassWork";
                var chekemployee = _examcontext.Staff_User_Login.Where(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).ToList();
                if (chekemployee.Count() > 0)
                {
                    var hrmeid = _PrincipalDashboardContext.Staff_User_Login.Single(a => a.Id == data.Login_Id);

                    if (data.ICW_Id > 0)
                    {
                        //var Duplicate = _PrincipalDashboardContext.IVRM_ClassWorkDMO.Where(t => t.ICW_Id != data.ICW_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ISMS_Id == data.ISMS_Id && t.Login_Id == data.Login_Id && t.ICW_Topic == data.ICW_Topic && t.ICW_ToDate == data.ICW_ToDate).ToList();
                        //if (Duplicate.Count() > 0)
                        //{
                        //    data.dulicate = true;
                        //}
                        //else
                        //{
                            var update = _PrincipalDashboardContext.IVRM_ClassWorkDMO.Where(t => t.ICW_Id == data.ICW_Id).SingleOrDefault();

                            foreach (var item in data.hm_section_list)
                            {
                                var sub = (from d in _PrincipalDashboardContext.Exm_Login_Privilege_SubjectsDMO
                                           from f in _PrincipalDashboardContext.School_M_Section
                                           from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                           from h in _PrincipalDashboardContext.Staff_User_Login
                                           from i in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                           from j in _PrincipalDashboardContext.Exm_Login_PrivilegeDMO
                                           from k in _PrincipalDashboardContext.School_M_Class
                                           where (d.ELP_Id == j.ELP_Id && h.IVRMSTAUL_Id == j.Login_Id && i.HRME_Id == h.Emp_Code && k.ASMCL_Id == d.ASMCL_Id
                                           && d.ASMS_Id == f.ASMS_Id && d.ISMS_Id == g.ISMS_Id && j.ASMAY_Id == data.ASMAY_Id && g.MI_Id == data.MI_Id
                                           && d.ASMS_Id == item.ASMS_Id && d.ELPs_ActiveFlg == true && j.ELP_ActiveFlg == true && i.HRME_ActiveFlag == true
                                           && i.HRME_LeftFlag == false && h.Emp_Code == hrmeid.Emp_Code && d.ASMCL_Id == data.ASMCL_Id)
                                           select new IVRM_ClassWorkDTO
                                           {
                                               ISMS_Id = g.ISMS_Id,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ASMC_SectionName = f.ASMC_SectionName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               ISMS_order = g.ISMS_OrderFlag

                                           }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
                                if (sub.Count() > 0)
                                {
                                    foreach (var item1 in sub)
                                    {
                                        if (item1.ISMS_Id == data.ISMS_Id)
                                        {
                                            update.ISMS_Id = data.ISMS_Id;
                                            update.ASMCL_Id = data.ASMCL_Id;
                                            update.ASMS_Id = item.ASMS_Id;
                                            update.ICW_FromDate = data.ICW_FromDate;
                                            update.ICW_ToDate = data.ICW_ToDate;
                                            update.ICW_Topic = data.ICW_Topic;
                                            update.ICW_SubTopic = data.ICW_SubTopic;
                                            update.ICW_Content = data.ICW_Content;
                                            update.ICW_TeachingAid = data.ICW_TeachingAid;
                                            update.ICW_Assignment = data.ICW_Assignment;
                                            update.ICW_Evaluation = data.ICW_Evaluation;
                                            update.ICW_Attachment = data.ICW_Attachment;
                                            update.UpdatedDate = DateTime.Now;
                                            _PrincipalDashboardContext.Update(update);
                                        }
                                    }
                                    if (data.ICW_FilePath_Array != null && data.ICW_FilePath_Array.Length>0)
                                    {
                                        var result = _PrincipalDashboardContext.IVRM_ClassWork_Attatchment_DMO_con.Where(a => a.ICW_Id == data.ICW_Id).ToList();
                                        if (result.Count > 0)
                                        {
                                            foreach (var res in result)
                                            {
                                                var result2 = _PrincipalDashboardContext.IVRM_ClassWork_Attatchment_DMO_con.Single(a => a.ICW_Id == data.ICW_Id && a.ICWATT_Id == res.ICWATT_Id);
                                                _PrincipalDashboardContext.Remove(result2);

                                            }
                                        }
                                        foreach (var itm in data.ICW_FilePath_Array)
                                        {
                                            IVRM_ClassWork_Attatchment_DMO cad = new IVRM_ClassWork_Attatchment_DMO();
                                            cad.ICW_Id = data.ICW_Id;
                                            cad.ICWATT_Attachment = itm.ICW_FilePath;
                                            cad.ICWATT_FileName = itm.FileName;
                                            cad.ICWATT_ActiveFlag = true;
                                            cad.UpdatedDate = DateTime.Now;
                                            _PrincipalDashboardContext.Add(cad);
                                        }

                                    }

                                }
                            }




                            List<long> amst = new List<long>();
                            foreach (var item1 in data.hm_section_list)
                            {
                                amst.Add(item1.ASMS_Id);
                            }
                            var contactExists = _PrincipalDashboardContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnval = true;

                                //=========================================== Notification
                                var hm = _PrincipalDashboardContext.IVRM_ClassWorkDMO.ToList();
                                var devicelist = (from a in _PrincipalDashboardContext.Adm_M_Student
                                                  from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                                  where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                                  select new IVRM_ClassWorkDTO
                                                  {
                                                      AMST_Id = b.AMST_Id,
                                                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                  }).Distinct().ToList();

                                var devicelistd = (from a in _PrincipalDashboardContext.Adm_M_Student
                                                   from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                                   select new IVRM_ClassWorkDTO
                                                   {
                                                       AMST_MobileNo = a.AMST_MobileNo,
                                                       AMST_Id = a.AMST_Id,
                                                       AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                   }).Distinct().ToList();

                                IVRM_ClassWorkDTO dto = new IVRM_ClassWorkDTO();
                                dto.devicelist12 = devicelistd;

                                IVRM_ClassWorkDTO dd = new IVRM_ClassWorkDTO();
                                dd.ICW_Id = hm.LastOrDefault().ICW_Id;
                                var homeid = dd.ICW_Id;

                                data.deviceArray = devicelist.ToArray();


                                var deviceidsnew = "";
                                var devicenew = "";
                                var redirecturl = "";
                                long revieveduserid = 0;

                                if (devicelist.Count > 0)
                                {
                                    foreach (var device_id in devicelist)
                                    {
                                        if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                        {
                                            
                                                
                                                revieveduserid = (from a in _PrincipalDashboardContext.StudentUserLoginDMO
                                                                  from b in _PrincipalDashboardContext.ApplicationUser
                                                                  where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id==device_id.AMST_Id)
                                                                  select b).Select(t => t.Id).FirstOrDefault();

                                            

                                            PushNotification push_noti = new PushNotification(_PrincipalDashboardContext);
                                            push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, homeid, data.ICW_Topic, "Classwork", "Classwork");

                                        }
                                    }
                                }

                                //var deviceidsnew = "";
                                //var devicenew = "";
                                //if (devicelist.Count > 0)
                                //{
                                //    int k = 0;

                                //    foreach (var deviceid in devicelist)
                                //    {
                                //        if (k == 0)
                                //        {
                                //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //            k = 1;
                                //        }
                                //        else
                                //        {
                                //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //        }
                                //        if (deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null)
                                //        {
                                //            callnotification(deviceid.AMST_AppDownloadedDeviceId, homeid, data.MI_Id, dto, header_flg);
                                //        }

                                //    }
                                //    devicenew = "[" + deviceidsnew + "]";

                                //    callnotification(devicenew, homeid, data.MI_Id, dto, header_flg);
                                //}






                            }
                            else
                            {
                                data.returnval = false;
                            }
                        //}
                    }


                    else
                    {
                        //var Duplicate = _PrincipalDashboardContext.IVRM_ClassWorkDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ISMS_Id == data.ISMS_Id && t.Login_Id == data.Login_Id && t.ICW_Topic == data.ICW_Topic && t.ICW_ToDate == data.ICW_ToDate).ToList();
                        //if (Duplicate.Count() > 0)
                        //{
                        //    data.dulicate = true;
                        //}
                        //else
                        //{
                            foreach (var item in data.hm_section_list)
                            {
                                var sub = (from d in _PrincipalDashboardContext.Exm_Login_Privilege_SubjectsDMO
                                           from f in _PrincipalDashboardContext.School_M_Section
                                           from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                           from h in _PrincipalDashboardContext.Staff_User_Login
                                           from i in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                           from j in _PrincipalDashboardContext.Exm_Login_PrivilegeDMO
                                           from k in _PrincipalDashboardContext.School_M_Class
                                           where (d.ELP_Id == j.ELP_Id && h.IVRMSTAUL_Id == j.Login_Id && i.HRME_Id == h.Emp_Code && k.ASMCL_Id == d.ASMCL_Id
                                           && d.ASMS_Id == f.ASMS_Id && d.ISMS_Id == g.ISMS_Id && j.ASMAY_Id == data.ASMAY_Id && g.MI_Id == data.MI_Id && d.ASMS_Id == item.ASMS_Id && d.ELPs_ActiveFlg == true && j.ELP_ActiveFlg == true && i.HRME_ActiveFlag == true && i.HRME_LeftFlag == false && h.Emp_Code == hrmeid.Emp_Code && d.ASMCL_Id == data.ASMCL_Id)
                                           select new IVRM_ClassWorkDTO
                                           {
                                               ISMS_Id = g.ISMS_Id,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ASMC_SectionName = f.ASMC_SectionName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               ISMS_order = g.ISMS_OrderFlag

                                           }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
                                if (sub.Count() > 0)
                                {
                                    foreach (var item1 in sub)
                                    {
                                        if (item1.ISMS_Id == data.ISMS_Id)
                                        {
                                            IVRM_ClassWorkDMO obj = new IVRM_ClassWorkDMO();

                                            obj.MI_Id = data.MI_Id;
                                            obj.ASMAY_Id = data.ASMAY_Id;
                                            obj.ISMS_Id = data.ISMS_Id;
                                            obj.Login_Id = data.Login_Id;
                                            obj.ASMCL_Id = data.ASMCL_Id;
                                            obj.ASMS_Id = item.ASMS_Id;
                                            obj.ICW_FromDate = data.ICW_FromDate;
                                            obj.ICW_ToDate = data.ICW_ToDate;
                                            obj.ICW_Topic = data.ICW_Topic;
                                            obj.ICW_SubTopic = data.ICW_SubTopic;
                                            obj.ICW_Content = data.ICW_Content;
                                            obj.ICW_TeachingAid = data.ICW_TeachingAid;
                                            obj.ICW_Assignment = data.ICW_Assignment;
                                            obj.ICW_Evaluation = data.ICW_Evaluation;
                                            obj.ICW_Attachment = data.ICW_Attachment;

                                            obj.ICW_ActiveFlag = true;
                                            obj.CreatedDate = DateTime.Now;
                                            obj.UpdatedDate = DateTime.Now;
                                            _PrincipalDashboardContext.Add(obj);

                                            if (data.ICW_FilePath_Array != null && data.ICW_FilePath_Array.Length>0)
                                            {
                                                foreach (var itm in data.ICW_FilePath_Array)
                                                {
                                                    IVRM_ClassWork_Attatchment_DMO cad = new IVRM_ClassWork_Attatchment_DMO();
                                                    cad.ICW_Id = obj.ICW_Id;
                                                    cad.ICWATT_Attachment = itm.ICW_FilePath;
                                                    cad.ICWATT_FileName = itm.FileName;
                                                    cad.ICWATT_ActiveFlag = true;
                                                    cad.UpdatedDate = DateTime.Now;
                                                    _PrincipalDashboardContext.Add(cad);
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            List<long> amst = new List<long>();
                            foreach (var item1 in data.hm_section_list)
                            {
                                amst.Add(item1.ASMS_Id);
                            }
                            var contactExists = _PrincipalDashboardContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnval = true;

                                //=========================================== Notification
                                var hm = _PrincipalDashboardContext.IVRM_ClassWorkDMO.ToList();
                                var devicelist = (from a in _PrincipalDashboardContext.Adm_M_Student
                                                  from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                                  where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                                  select new IVRM_ClassWorkDTO
                                                  {
                                                      AMST_Id = b.AMST_Id,
                                                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                  }).Distinct().ToList();

                                var devicelistd = (from a in _PrincipalDashboardContext.Adm_M_Student
                                                   from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                                   select new IVRM_ClassWorkDTO
                                                   {
                                                       AMST_MobileNo = a.AMST_MobileNo,
                                                       AMST_Id = a.AMST_Id,
                                                       AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                   }).Distinct().ToList();

                                IVRM_ClassWorkDTO dto = new IVRM_ClassWorkDTO();
                                dto.devicelist12 = devicelistd;

                                IVRM_ClassWorkDTO dd = new IVRM_ClassWorkDTO();
                                dd.ICW_Id = hm.LastOrDefault().ICW_Id;
                                var homeid = dd.ICW_Id;

                                data.deviceArray = devicelist.ToArray();


                                var deviceidsnew = "";
                                var devicenew = "";
                                var redirecturl = "";
                                long revieveduserid = 0;

                                if (devicelist.Count > 0)
                                {
                                    foreach (var device_id in devicelist)
                                    {
                                    if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                    {


                                            revieveduserid = (from a in _PrincipalDashboardContext.StudentUserLoginDMO
                                                              from b in _PrincipalDashboardContext.ApplicationUser
                                                              where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                              select b).Select(t => t.Id).FirstOrDefault();



                                            PushNotification push_noti = new PushNotification(_PrincipalDashboardContext);
                                            push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.Login_Id, revieveduserid, homeid, data.ICW_Topic, "Classwork", "Classwork");

                                        }
                                    }
                                }

                                //var deviceidsnew = "";
                                //var devicenew = "";
                                //if (devicelist.Count > 0)
                                //{
                                //    int k = 0;

                                //    foreach (var deviceid in devicelist)
                                //    {
                                //        if (k == 0)
                                //        {
                                //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //            k = 1;
                                //        }
                                //        else
                                //        {
                                //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //        }

                                //        if (deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null)
                                //        {
                                //            callnotification(deviceid.AMST_AppDownloadedDeviceId, homeid, data.MI_Id, dto, header_flg);
                                //        }
                                //    }
                                //    devicenew = "[" + deviceidsnew + "]";

                                //    callnotification(devicenew, homeid, data.MI_Id, dto, header_flg);
                                //}
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        //}
                    }
                }

                else
                {
                    //var hrmeid = _PrincipalDashboardContext.Staff_User_Login.Single(a => a.Id == data.Login_Id);

                    if (data.ICW_Id > 0)
                    {
                        //var Duplicate = _PrincipalDashboardContext.IVRM_ClassWorkDMO.Where(t => t.ICW_Id != data.ICW_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ISMS_Id == data.ISMS_Id && t.Login_Id == data.Login_Id && t.ICW_Topic == data.ICW_Topic && t.ICW_ToDate == data.ICW_ToDate).ToList();
                        //if (Duplicate.Count() > 0)
                        //{
                        //    data.dulicate = true;
                        //}
                        //else
                        //{
                            var update = _PrincipalDashboardContext.IVRM_ClassWorkDMO.Where(t => t.ICW_Id == data.ICW_Id).SingleOrDefault();

                            foreach (var item in data.hm_section_list)
                            {
                                var sub = (from d in _PrincipalDashboardContext.Exm_Yearly_CategoryDMO
                                           from e in _PrincipalDashboardContext.Exm_Yearly_Category_GroupDMO
                                           from h in _PrincipalDashboardContext.Exm_Yearly_Category_Group_SubjectsDMO
                                           from i in _PrincipalDashboardContext.Exm_Category_ClassDMO
                                           from f in _PrincipalDashboardContext.School_M_Section
                                           from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                           from k in _PrincipalDashboardContext.School_M_Class
                                           where (d.EYC_Id == e.EYC_Id && e.EYCG_Id == h.EYCG_Id && d.EMCA_Id == i.EMCA_Id && i.ASMCL_Id == k.ASMCL_Id
                                           && i.ASMS_Id == f.ASMS_Id && h.ISMS_Id == g.ISMS_Id && d.EYC_ActiveFlg == true && e.EYCG_ActiveFlg == true
                                           && h.EYCGS_ActiveFlg == true && i.ECAC_ActiveFlag == true && d.ASMAY_Id == data.ASMAY_Id && i.ASMAY_Id == data.ASMAY_Id
                                           && i.ASMCL_Id == data.ASMCL_Id && i.ASMS_Id == item.ASMS_Id && g.MI_Id == data.MI_Id)
                                           select new IVRM_ClassWorkDTO
                                           {
                                               ISMS_Id = g.ISMS_Id,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ASMC_SectionName = f.ASMC_SectionName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               ISMS_order = g.ISMS_OrderFlag
                                           }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

                                if (sub.Count() > 0)
                                {
                                    foreach (var item1 in sub)
                                    {
                                        if (item1.ISMS_Id == data.ISMS_Id)
                                        {
                                            update.ISMS_Id = data.ISMS_Id;
                                            update.ASMCL_Id = data.ASMCL_Id;
                                            update.ASMS_Id = item.ASMS_Id;
                                            update.ICW_FromDate = data.ICW_FromDate;
                                            update.ICW_ToDate = data.ICW_ToDate;
                                            update.ICW_Topic = data.ICW_Topic;
                                            update.ICW_SubTopic = data.ICW_SubTopic;
                                            update.ICW_Content = data.ICW_Content;
                                            update.ICW_TeachingAid = data.ICW_TeachingAid;
                                            update.ICW_Assignment = data.ICW_Assignment;
                                            update.ICW_Evaluation = data.ICW_Evaluation;
                                            update.ICW_Attachment = data.ICW_Attachment;
                                            update.UpdatedDate = DateTime.Now;
                                            _PrincipalDashboardContext.Update(update);
                                        }
                                    }
                                    if (data.ICW_FilePath_Array != null && data.ICW_FilePath_Array.Length>0)
                                    {
                                        var result = _PrincipalDashboardContext.IVRM_ClassWork_Attatchment_DMO_con.Where(a => a.ICW_Id == data.ICW_Id).ToList();
                                        if (result.Count > 0)
                                        {
                                            foreach (var res in result)
                                            {
                                                var result2 = _PrincipalDashboardContext.IVRM_ClassWork_Attatchment_DMO_con.Single(a => a.ICW_Id == data.ICW_Id && a.ICWATT_Id == res.ICWATT_Id);
                                                _PrincipalDashboardContext.Remove(result2);

                                            }
                                        }
                                        foreach (var itm in data.ICW_FilePath_Array)
                                        {
                                            IVRM_ClassWork_Attatchment_DMO cad = new IVRM_ClassWork_Attatchment_DMO();
                                            cad.ICW_Id = data.ICW_Id;
                                            cad.ICWATT_Attachment = itm.ICW_FilePath;
                                            cad.ICWATT_FileName = itm.FileName;
                                            cad.ICWATT_ActiveFlag = true;
                                            cad.UpdatedDate = DateTime.Now;
                                            _PrincipalDashboardContext.Add(cad);
                                        }

                                    }
                                }
                            }


                            List<long> amst = new List<long>();
                            foreach (var item1 in data.hm_section_list)
                            {
                                amst.Add(item1.ASMS_Id);
                            }
                            var contactExists = _PrincipalDashboardContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnval = true;

                                //=========================================== Notification
                                var hm = _PrincipalDashboardContext.IVRM_ClassWorkDMO.ToList();
                                var devicelist = (from a in _PrincipalDashboardContext.Adm_M_Student
                                                  from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                                  where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                                  select new IVRM_ClassWorkDTO
                                                  {
                                                      AMST_Id = b.AMST_Id,
                                                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                  }).Distinct().ToList();

                                var devicelistd = (from a in _PrincipalDashboardContext.Adm_M_Student
                                                   from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                                   select new IVRM_ClassWorkDTO
                                                   {
                                                       AMST_MobileNo = a.AMST_MobileNo,
                                                       AMST_Id = a.AMST_Id,
                                                       AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                   }).Distinct().ToList();

                                IVRM_ClassWorkDTO dto = new IVRM_ClassWorkDTO();
                                dto.devicelist12 = devicelistd;

                                IVRM_ClassWorkDTO dd = new IVRM_ClassWorkDTO();
                                dd.ICW_Id = hm.LastOrDefault().ICW_Id;
                                var homeid = dd.ICW_Id;

                                data.deviceArray = devicelist.ToArray();


                                var deviceidsnew = "";
                                var devicenew = "";
                                var redirecturl = "";
                                long revieveduserid = 0;

                                if (devicelist.Count > 0)
                                {
                                    foreach (var device_id in devicelist)
                                    {
                                    if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                    {


                                            revieveduserid = (from a in _PrincipalDashboardContext.StudentUserLoginDMO
                                                              from b in _PrincipalDashboardContext.ApplicationUser
                                                              where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                              select b).Select(t => t.Id).FirstOrDefault();



                                            PushNotification push_noti = new PushNotification(_PrincipalDashboardContext);
                                            push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, homeid, data.ICW_Topic, "Classwork", "Classwork");

                                        }
                                    }
                                }

                                //var deviceidsnew = "";
                                //var devicenew = "";
                                //if (devicelist.Count > 0)
                                //{
                                //    int k = 0;

                                //    foreach (var deviceid in devicelist)
                                //    {
                                //        if (k == 0)
                                //        {
                                //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //            k = 1;
                                //        }
                                //        else
                                //        {
                                //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //        }

                                //        if (deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null)
                                //        {
                                //            callnotification(deviceid.AMST_AppDownloadedDeviceId, homeid, data.MI_Id, dto, header_flg);
                                //        }
                                //    }
                                //    devicenew = "[" + deviceidsnew + "]";

                                //    callnotification(devicenew, homeid, data.MI_Id, dto, header_flg);
                                //}
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        //}
                    }
                    else
                    {
                        //var Duplicate = _PrincipalDashboardContext.IVRM_ClassWorkDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ISMS_Id == data.ISMS_Id && t.Login_Id == data.Login_Id && t.ICW_Topic == data.ICW_Topic && t.ICW_ToDate == data.ICW_ToDate).ToList();
                        //if (Duplicate.Count() > 0)
                        //{
                        //    data.dulicate = true;
                        //}
                        //else
                        //{

                            foreach (var item in data.hm_section_list)
                            {
                                var sub = (from d in _PrincipalDashboardContext.Exm_Yearly_CategoryDMO
                                           from e in _PrincipalDashboardContext.Exm_Yearly_Category_GroupDMO
                                           from h in _PrincipalDashboardContext.Exm_Yearly_Category_Group_SubjectsDMO
                                           from i in _PrincipalDashboardContext.Exm_Category_ClassDMO
                                           from f in _PrincipalDashboardContext.School_M_Section
                                           from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                           from k in _PrincipalDashboardContext.School_M_Class
                                           where (d.EYC_Id == e.EYC_Id && e.EYCG_Id == h.EYCG_Id && d.EMCA_Id == i.EMCA_Id && i.ASMCL_Id == k.ASMCL_Id
                                           && i.ASMS_Id == f.ASMS_Id && h.ISMS_Id == g.ISMS_Id && d.EYC_ActiveFlg == true && e.EYCG_ActiveFlg == true
                                           && h.EYCGS_ActiveFlg == true && i.ECAC_ActiveFlag == true && d.ASMAY_Id == data.ASMAY_Id && i.ASMAY_Id == data.ASMAY_Id
                                           && i.ASMCL_Id == data.ASMCL_Id && i.ASMS_Id == item.ASMS_Id && g.MI_Id == data.MI_Id)
                                           select new IVRM_ClassWorkDTO
                                           {
                                               ISMS_Id = g.ISMS_Id,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ASMC_SectionName = f.ASMC_SectionName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               ISMS_order = g.ISMS_OrderFlag
                                           }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

                                if (sub.Count() > 0)
                                {
                                    foreach (var item1 in sub)
                                    {
                                        if (item1.ISMS_Id == data.ISMS_Id)
                                        {
                                            IVRM_ClassWorkDMO obj = new IVRM_ClassWorkDMO();

                                            obj.MI_Id = data.MI_Id;
                                            obj.ASMAY_Id = data.ASMAY_Id;
                                            obj.ISMS_Id = data.ISMS_Id;
                                            obj.Login_Id = data.Login_Id;
                                            obj.ASMCL_Id = data.ASMCL_Id;
                                            obj.ASMS_Id = item.ASMS_Id;
                                            obj.ICW_FromDate = data.ICW_FromDate;
                                            obj.ICW_ToDate = data.ICW_ToDate;
                                            obj.ICW_Topic = data.ICW_Topic;
                                            obj.ICW_SubTopic = data.ICW_SubTopic;
                                            obj.ICW_Content = data.ICW_Content;
                                            obj.ICW_TeachingAid = data.ICW_TeachingAid;
                                            obj.ICW_Assignment = data.ICW_Assignment;
                                            obj.ICW_Evaluation = data.ICW_Evaluation;
                                            obj.ICW_Attachment = data.ICW_Attachment;
                                            obj.ICW_FilePath = null;
                                            obj.ICW_ActiveFlag = true;
                                            obj.CreatedDate = DateTime.Now;
                                            obj.UpdatedDate = DateTime.Now;
                                            _PrincipalDashboardContext.Add(obj);

                                            if (data.ICW_FilePath_Array != null && data.ICW_FilePath_Array.Length>0)
                                            {
                                                foreach (var itm in data.ICW_FilePath_Array)
                                                {
                                                    IVRM_ClassWork_Attatchment_DMO cad = new IVRM_ClassWork_Attatchment_DMO();
                                                    cad.ICW_Id = obj.ICW_Id;
                                                    cad.ICWATT_Attachment = itm.ICW_FilePath;
                                                    cad.ICWATT_FileName = itm.FileName;
                                                    cad.ICWATT_ActiveFlag = true;
                                                    cad.UpdatedDate = DateTime.Now;
                                                    _PrincipalDashboardContext.Add(cad);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            List<long> amst = new List<long>();
                            foreach (var item1 in data.hm_section_list)
                            {
                                amst.Add(item1.ASMS_Id);
                            }
                            var contactExists = _PrincipalDashboardContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnval = true;


                                //=========================================== Notification
                                var hm = _PrincipalDashboardContext.IVRM_ClassWorkDMO.ToList();
                                var devicelist = (from a in _PrincipalDashboardContext.Adm_M_Student
                                                  from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                                  where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                                  select new IVRM_ClassWorkDTO
                                                  {
                                                      AMST_Id = b.AMST_Id,
                                                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                  }).Distinct().ToList();

                                var devicelistd = (from a in _PrincipalDashboardContext.Adm_M_Student
                                                   from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && amst.Contains(b.ASMS_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                                   select new IVRM_ClassWorkDTO
                                                   {
                                                       AMST_MobileNo = a.AMST_MobileNo,
                                                       AMST_Id = a.AMST_Id,
                                                       AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                   }).Distinct().ToList();

                                IVRM_ClassWorkDTO dto = new IVRM_ClassWorkDTO();
                                dto.devicelist12 = devicelistd;

                                IVRM_ClassWorkDTO dd = new IVRM_ClassWorkDTO();
                                dd.ICW_Id = hm.LastOrDefault().ICW_Id;
                                var homeid = dd.ICW_Id;

                                data.deviceArray = devicelist.ToArray();

                                var deviceidsnew = "";
                                var devicenew = "";
                                var redirecturl = "";
                                long revieveduserid = 0;

                                if (devicelist.Count > 0)
                                {
                                    foreach (var device_id in devicelist)
                                    {
                                        if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                        {


                                            revieveduserid = (from a in _PrincipalDashboardContext.StudentUserLoginDMO
                                                              from b in _PrincipalDashboardContext.ApplicationUser
                                                              where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                              select b).Select(t => t.Id).FirstOrDefault();



                                            PushNotification push_noti = new PushNotification(_PrincipalDashboardContext);
                                            push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, homeid, data.ICW_Topic, "Classwork", "Classwork");

                                        }
                                    }
                                }

                                //var deviceidsnew = "";
                                //var devicenew = "";
                                //if (devicelist.Count > 0)
                                //{
                                //    int k = 0;

                                //    foreach (var deviceid in devicelist)
                                //    {
                                //        if (k == 0)
                                //        {
                                //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //            k = 1;
                                //        }
                                //        else
                                //        {
                                //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //        }

                                //        if (deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null)
                                //        {
                                //            callnotification(deviceid.AMST_AppDownloadedDeviceId, homeid, data.MI_Id, dto, header_flg);
                                //        }
                                //    }
                                //    devicenew = "[" + deviceidsnew + "]";

                                //    //callnotification(devicenew, homeid, data.MI_Id, dto);
                                //}
                            }
                            else
                            {
                                data.returnval = false;
                            }
                      //  }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public string callnotification(string devicenew, long icw_Id, long mi_id, IVRM_ClassWorkDTO dto, string header_flg)
        {
            try
            {

                var key = _PrincipalDashboardContext.MobileApplAuthenticationDMO.Single(a => a.MI_Id == mi_id).MAAN_AuthenticationKey;

                IVRM_ClassWorkDTO data = new IVRM_ClassWorkDTO();
                var classwork = _PrincipalDashboardContext.IVRM_ClassWorkDMO.Where(h => h.MI_Id == mi_id && h.ICW_ActiveFlag == true && h.ICW_Id == icw_Id).Distinct().ToList();



                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //     "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Topic + '"' + " , " + '"' + "body" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Content + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";


                string sound = "default";
                string notId = "4";
                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                // "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Topic + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' +
                // +'"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , "
                // + '"' + "body" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Content + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                // daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //"" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Classwork" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Topic + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                // notificationparams.Add(daata.ToString()); 

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                //   daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //"" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Classwork" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Topic + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                //   notificationparams.Add(daata.ToString());
               // daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
               //"" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Classwork" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Topic + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } " + "," + '"' + "data" + '"' + ":" + "{" + '"' + "page" + '"' + ":" + '"' + "classwork" + '"' + "}" + "}";



                Dictionary<string, object> input = new Dictionary<string, object>();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                transfersnotes.Add("body", classwork.FirstOrDefault().ICW_Topic);
                transfersnotes.Add("title", "Classwork");

                input.Add("to", devicenew);
                input.Add("notification", transfersnotes);

                var myContent = JsonConvert.SerializeObject(input);
                String postdata = myContent;


                //var mycontent = notificationparams[0];
                //string postdata = mycontent.ToString();


                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=" + key;

                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }



                PushNotification push_noti = new PushNotification(_db);

                push_noti.Insert_PushNotification_classwork(icw_Id, mi_id, devicenew, dto, header_flg);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";

        }
        public IVRM_ClassWorkDTO editData(IVRM_ClassWorkDTO data)
        {
            try
            {
                data.editlist = (from m in _PrincipalDashboardContext.IVRM_ClassWorkDMO
                                 from a in _PrincipalDashboardContext.AcademicYearDMO
                                 from n in _PrincipalDashboardContext.School_M_Class
                                 from o in _PrincipalDashboardContext.School_M_Section
                                 from p in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                 where (m.MI_Id == n.MI_Id && m.ASMAY_Id == a.ASMAY_Id && m.ASMCL_Id == n.ASMCL_Id && m.ASMS_Id == o.ASMS_Id && m.ISMS_Id == p.ISMS_Id && m.ICW_Id == data.ICW_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.ASMAY_Id)
                                 select new IVRM_ClassWorkDTO
                                 {
                                     ASMCL_Id = n.ASMCL_Id,
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMS_Id = o.ASMS_Id,
                                     ISMS_Id = p.ISMS_Id,
                                     ICW_Topic = m.ICW_Topic,
                                     ICW_SubTopic = m.ICW_SubTopic,
                                     ICW_Assignment = m.ICW_Assignment,
                                     ICW_Attachment = m.ICW_Attachment,
                                     ICW_FilePath = m.ICW_FilePath,
                                     ICW_FromDate = m.ICW_FromDate,
                                     ICW_ToDate = m.ICW_ToDate,
                                     ICW_Id = m.ICW_Id,
                                     ICW_Content = m.ICW_Content,
                                     ICW_TeachingAid = m.ICW_TeachingAid,
                                     ICW_Evaluation = m.ICW_Evaluation,

                                 }).Distinct().ToArray();
                data.editlist_section = (from m in _PrincipalDashboardContext.IVRM_ClassWorkDMO
                                         from a in _PrincipalDashboardContext.AcademicYearDMO
                                         from n in _PrincipalDashboardContext.School_M_Class
                                         from o in _PrincipalDashboardContext.School_M_Section
                                         from p in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                         where (m.MI_Id == n.MI_Id && m.ASMAY_Id == a.ASMAY_Id && m.ASMCL_Id == n.ASMCL_Id && m.ASMS_Id == o.ASMS_Id && m.ISMS_Id == p.ISMS_Id && m.ICW_Id == data.ICW_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.ASMAY_Id)
                                         select new IVRM_ClassWorkDTO
                                         {

                                             ASMS_Id = o.ASMS_Id,


                                         }).Distinct().ToArray();
                var img = (from a in _PrincipalDashboardContext.IVRM_ClassWork_Attatchment_DMO_con
                           from b in _PrincipalDashboardContext.IVRM_ClassWorkDMO
                           where a.ICW_Id == data.ICW_Id && a.ICW_Id == b.ICW_Id
                           select new IVRM_ClassWorkDTO
                           {
                               ICW_Id = b.ICW_Id,
                               ICW_Attachment = b.ICW_Attachment,
                               ICWATT_Attachment = a.ICWATT_Attachment,
                               ICWATT_FileName = a.ICWATT_FileName,

                           }).ToArray();
                if (img.Length > 0)
                {
                    data.attachementlist = img;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_ClassWorkDTO deactivate(IVRM_ClassWorkDTO data)
        {
            try
            {
                var query = _PrincipalDashboardContext.IVRM_ClassWorkDMO.Single(s => s.MI_Id == data.MI_Id && s.ICW_Id == data.ICW_Id);

                if (query.ICW_ActiveFlag == true)
                {
                    query.ICW_ActiveFlag = false;
                }
                else
                {
                    query.ICW_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;
                _PrincipalDashboardContext.Update(query);
                var contactExists = _PrincipalDashboardContext.SaveChanges();
                if (contactExists == 1)
                {
                    data.returnval = true;
                    //data.returnsavestatus = "saved";
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_ClassWorkDTO viewData(IVRM_ClassWorkDTO dto)
        {
            try
            {
                dto.attachementlist = (from a in _PrincipalDashboardContext.IVRM_ClassWork_Attatchment_DMO_con
                                       from b in _PrincipalDashboardContext.IVRM_ClassWorkDMO
                                       where a.ICW_Id == dto.ICW_Id && a.ICW_Id == b.ICW_Id
                                       select new IVRM_ClassWorkDTO
                                       {
                                           ICW_Id = b.ICW_Id,
                                           ICW_Attachment = b.ICW_Attachment,
                                           ICWATT_Attachment = a.ICWATT_Attachment,
                                           ICWATT_FileName = a.ICWATT_FileName
                                       }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dto;
        }


        //============ class work marks enter===========

        public IVRM_ClassWorkDTO getclasswork_student(IVRM_ClassWorkDTO dto)
        {
            try
            {
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_HomeWork_Marks_Student_List";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
               SqlDbType.BigInt)
                    {
                        Value = dto.ASMS_Id
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
                        dto.studentlist1 = retObject.ToArray();
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
            return dto;
        }
        public IVRM_ClassWorkDTO getsubjectlist(IVRM_ClassWorkDTO dto)
        {
            try
            {
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeclasswork_SubjectList_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = dto.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = dto.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.BigInt){Value = dto.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.BigInt){Value = dto.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@Type",SqlDbType.VarChar){Value = "ClassWork"});
                    cmd.Parameters.Add(new SqlParameter("@UserId",SqlDbType.VarChar){Value = dto.UserId});
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
                                    dataRow.Add(dataReader.GetName(iFiled),dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.getsubject_list = retObject.ToArray();
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
            return dto;
        }
        public IVRM_ClassWorkDTO getclasswork_list(IVRM_ClassWorkDTO dto)
        {
            try
            {
                //var amst_id = "";

                //foreach (var x in dto.studentarray)
                //{
                //    amst_id += x.AMST_Id1 + ",";
                //}
                //amst_id = amst_id.Substring(0, (amst_id.Length - 1));
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeclasswork_List_Modify";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = dto.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = dto.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = 0 });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id", SqlDbType.BigInt) { Value = dto.Login_Id });
                    cmd.Parameters.Add(new SqlParameter("@Parameter", SqlDbType.VarChar) { Value = "Classwork" });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt) { Value = dto.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = dto.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = dto.todate });
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
                        dto.getclasswork_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                    //doclist

                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeclasswork_List_Modify_Doclist";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = dto.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = dto.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = 0 });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id", SqlDbType.BigInt) { Value = dto.Login_Id });
                    cmd.Parameters.Add(new SqlParameter("@Parameter", SqlDbType.VarChar) { Value = "Classwork" });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt) { Value = dto.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = dto.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = dto.todate });

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
                        dto.viewhomework = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //


                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "IVRM_homeworkclasswork_MarksUpdate_list";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id",
                 SqlDbType.BigInt)
                    {
                        Value = dto.Login_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Parameter",
                SqlDbType.VarChar)
                    {
                        Value = "Classwork"
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
                        dto.marksupdate_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "IVRM_classwork_Topiclist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id",
                 SqlDbType.BigInt)
                    {
                        Value = dto.Login_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ISMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                  SqlDbType.BigInt)
                    {
                        Value = dto.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Parameter",
                SqlDbType.VarChar)
                    {
                        Value = "Classwork"
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
                        dto.TopicList = retObject.ToArray();
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
            return dto;
        }
        public IVRM_ClassWorkDTO classwork_marks_update(IVRM_ClassWorkDTO dto)
        {
            try
            {
                foreach (var item1 in dto.getclasswork_list_array)
                {
                    var result = _PrincipalDashboardContext.IVRM_ClassWork_Upload_DMO_con.Where(a => a.ICW_Id == item1.ICW_Id && a.AMST_Id == item1.AMST_Id
                    && a.ICWUPL_Id == item1.ICWUPL_Id).ToList();

                    if (result.Count > 0)
                    {
                        var update_result = _PrincipalDashboardContext.IVRM_ClassWork_Upload_DMO_con.Single(a => a.ICW_Id == item1.ICW_Id 
                        && a.AMST_Id == item1.AMST_Id && a.ICWUPL_Id == item1.ICWUPL_Id);

                        update_result.ICWUPL_Marks = item1.Marks;
                        update_result.ICWUPL_FileName = item1.ICWUPL_FileName;
                        update_result.ICWUPL_StaffUplaod = item1.ICWUPL_StaffUplaod;
                        update_result.ICWUPL_StaffRemarks = item1.ICWUPL_StaffRemarks;
                        update_result.UpdatedDate = DateTime.Now;
                        _PrincipalDashboardContext.Update(update_result);

                        if(item1.doclist_temp !=null && item1.doclist_temp.Length > 0)
                        {
                            foreach (var d in item1.doclist_temp)
                            {
                                var dd = _PrincipalDashboardContext.IVRM_ClassWork_Upload_Attatchment_DMO_con.Where(a => a.ICWUPL_Id == item1.ICWUPL_Id
                                 && a.ICWUPLATT_Id == d.ICWUPLATT_Id).Count();

                                if (dd > 0)
                                {
                                    var result_update = _PrincipalDashboardContext.IVRM_ClassWork_Upload_Attatchment_DMO_con.Single(a => a.ICWUPL_Id == item1.ICWUPL_Id
                                 && a.ICWUPLATT_Id == d.ICWUPLATT_Id);

                                    result_update.ICWUPLATT_StaffUpload = d.FilePath1;
                                    result_update.ICWUPLATT_StaffRemarks = d.Remarks;
                                    result_update.ICWUPLATT_UpdatedDate = DateTime.Now;
                                    _PrincipalDashboardContext.Update(result_update);
                                }
                            }
                        }
                        
                    }
                }
                var update = _PrincipalDashboardContext.SaveChanges();
                if (update > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public IVRM_ClassWorkDTO edit_classwork_mark(IVRM_ClassWorkDTO dto)
        {
            try
            {
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeclasswork_edit_modfiy";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IHW_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.ICW_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Parameter",
               SqlDbType.VarChar)
                    {
                        Value = "Classwork"
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
                        dto.edit_mark_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            


                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeworkstudent";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IHW_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.ICW_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Parameter",
               SqlDbType.VarChar)
                    {
                        Value = "Homework"
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
                        dto.editstudent = retObject.ToArray();
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
            return dto;
        }
        public IVRM_ClassWorkDTO viewclasswork(IVRM_ClassWorkDTO dto)
        {
            try
            {
                dto.viewhomework = (from a in _PrincipalDashboardContext.IVRM_ClassWorkDMO
                                    from b in _PrincipalDashboardContext.IVRM_ClassWork_Attatchment_DMO_con
                                    where a.ICW_Id == b.ICW_Id && a.MI_Id == dto.MI_Id && a.ICW_Id == dto.ICW_Id
                                    select new IVRM_ClassWorkDTO
                                    {
                                        ICW_Id = a.ICW_Id,
                                        ICW_Topic = a.ICW_Topic,
                                        ICW_SubTopic = a.ICW_SubTopic,
                                        ICW_Assignment = a.ICW_Assignment,
                                        ICWATT_Attachment = b.ICWATT_Attachment,
                                        ICW_Attachment = a.ICW_Attachment,
                                        ICWATT_FileName = b.ICWATT_FileName
                                    }).Distinct().ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public IVRM_ClassWorkDTO viewstudentupload(IVRM_ClassWorkDTO dto)
        {
            try
            {
                dto.viewstudentupload = (from a in _PrincipalDashboardContext.IVRM_ClassWorkDMO
                                         from b in _PrincipalDashboardContext.IVRM_ClassWork_Upload_DMO_con
                                         from c in _PrincipalDashboardContext.IVRM_ClassWork_Upload_Attatchment_DMO_con
                                         where a.ICW_Id == b.ICW_Id && b.ICWUPL_Id == c.ICWUPL_Id && a.MI_Id == dto.MI_Id && a.ICW_Id == dto.ICW_Id
                                         && b.AMST_Id == dto.AMST_Id && b.ICWUPL_Id==dto.ICWUPL_Id
                                         select new IVRM_ClassWorkDTO
                                         {
                                             ICWUPL_Id = b.ICWUPL_Id,
                                             ICWUPL_Attachment = c.ICWUPLATT_Attachment,
                                             ICWUPL_FileName = c.ICWUPLATT_FileName,
                                             ICWUPLATT_StaffUpload = c.ICWUPLATT_StaffUpload,
                                             ICWUPLATT_StaffRemarks = c.ICWUPLATT_StaffRemarks,
                                             ICWUPLATT_Id = c.ICWUPLATT_Id
                                         }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public IVRM_ClassWorkDTO stfupload(IVRM_ClassWorkDTO dto)
        {
            try
            {
                foreach (var item in dto.doclist)
                {
                    IVRM_ClassWork_Upload_Attatchment_DMO hp = new IVRM_ClassWork_Upload_Attatchment_DMO();
                    hp.ICWUPL_Id = item.ICWUPL_Id;
                    hp.ICWUPLATT_Attachment = item.FilePath1;
                    hp.ICWUPLATT_FileName = item.FileName1;
                    hp.ICWUPLATT_StaffRemarks = item.Remarks;
                    hp.ICWUPLATT_StaffUpload = "Staff";
                    hp.ICWUPLATT_ActiveFlag = true;
                    hp.ICWUPLATT_CreatedDate = DateTime.Today;
                    hp.ICWUPLATT_CreatedDate = DateTime.Today;
                    _PrincipalDashboardContext.Add(hp);
                }

                var update = _PrincipalDashboardContext.SaveChanges();


                if (update > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public IVRM_ClassWorkDTO Getdata_class(IVRM_ClassWorkDTO dto)
        {
            try
            {
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    dto.HRME_Id = _PrincipalDashboardContext.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }
                else
                {
                    dto.HRME_Id = 0;
                }

                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_ClassList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Role",
                SqlDbType.VarChar)
                    {
                        Value = rolet[0].IVRMRT_Role
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
                        dto.classlist = retObject.ToArray();
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
            return dto;
        }

        public IVRM_ClassWorkDTO getreportnotice(IVRM_ClassWorkDTO dto)
        {
            try
            {
              
                string asmclid = "0";
                if (dto.classarray.Length > 0)
                {
                    foreach (var item in dto.classarray)
                    {
                        asmclid = asmclid + "," + item.ASMCL_Id;
                    }
                }
                //var rolet = _Context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                //dto.HRME_Id = _Context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;


                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_NoticeboardConsolidated";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
               SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                 SqlDbType.VarChar)
                    {
                        Value = asmclid
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.todate
                    });


                    cmd.Parameters.Add(new SqlParameter("@flag",
                SqlDbType.VarChar)
                    {
                        Value = dto.flag
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
                        dto.reportlist = retObject.ToArray();

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
            return dto;
        }

        public IVRM_ClassWorkDTO Getdataview(IVRM_ClassWorkDTO dto)
        {
            try

            {
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();
                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_NoticeboardConsolidated";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
               SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                 SqlDbType.VarChar)
                    {
                        Value = dto.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.todate
                    });


                    cmd.Parameters.Add(new SqlParameter("@flag",
                SqlDbType.VarChar)
                    {
                        Value = dto.flag
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
                        dto.view_array = retObject.ToArray();

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
            return dto;
        }
        public IVRM_ClassWorkDTO getclasswork_Topiclist(IVRM_ClassWorkDTO dto)
        {
            try
            {

                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeclasswork_TopicList_Modify";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = dto.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = dto.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = 0 });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id", SqlDbType.BigInt) { Value = dto.Login_Id });
                    cmd.Parameters.Add(new SqlParameter("@Parameter", SqlDbType.VarChar) { Value = "Classwork" });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt) { Value = dto.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = dto.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = dto.todate });
                    cmd.Parameters.Add(new SqlParameter("@Topic_Id", SqlDbType.BigInt) { Value = dto.ICW_Id });
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
                        dto.getclasswork_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_homeclasswork_List_Modify_Doclist";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = dto.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = dto.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = 0 });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id", SqlDbType.BigInt) { Value = dto.Login_Id });
                    cmd.Parameters.Add(new SqlParameter("@Parameter", SqlDbType.VarChar) { Value = "Classwork" });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt) { Value = dto.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = dto.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = dto.todate });

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
                        dto.viewhomework = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //


                using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "IVRM_homeworkclasswork_MarksUpdate_list";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Login_Id",
                 SqlDbType.BigInt)
                    {
                        Value = dto.Login_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Parameter",
                SqlDbType.VarChar)
                    {
                        Value = "Classwork"
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
                        dto.marksupdate_list = retObject.ToArray();
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
            return dto;
        }
    }
}
