using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.Portals.Student;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using DomainModel.Model.com.vapstech.Portals.IVRM;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;

namespace PortalHub.com.vaps.Student.Services
{
    public class IVRM_InteractionsImpl : Interfaces.IVRM_InteractionsInterface
    {
        private static ConcurrentDictionary<string, IVRM_School_InteractionsDTO> _login =
           new ConcurrentDictionary<string, IVRM_School_InteractionsDTO>();
        private PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _context;

        public IVRM_InteractionsImpl(PortalContext PortalContext, DomainModelMsSqlServerContext context)
        {
            _PortalContext = PortalContext;
            _context = context;
        }
        public async Task<IVRM_School_InteractionsDTO> getloaddata(IVRM_School_InteractionsDTO data)

        {
            try
            {

                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                data.configflag = _PortalContext.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id).Distinct().ToArray();
                data.notificationflag = "Interaction";

                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                    data.userhrmE_Id = data.AMST_Id;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    data.userhrmE_Id = data.HRME_Id;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("HOD", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    data.userhrmE_Id = data.HRME_Id;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    data.userhrmE_Id = data.HRME_Id;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Chairman", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    data.userhrmE_Id = data.HRME_Id;
                }

                //else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Fee End User", StringComparison.OrdinalIgnoreCase))
                //{
                //    data.AMST_Id = 0;
                //    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                //}
                //else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase))
                //{
                //    data.AMST_Id = 0;
                //    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                //}
                //else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("HOD", StringComparison.OrdinalIgnoreCase))
                //{
                //    data.AMST_Id = 0;
                //    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                //}
                //else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Chairman", StringComparison.OrdinalIgnoreCase))
                //{
                //    data.AMST_Id = 0;
                //    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                //}
                else
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }


                data.rolename = rolet.FirstOrDefault().IVRMRT_Role.ToLower();
                if (rolet.FirstOrDefault().IVRMRT_Role.ToLower() != "student")
                {
                    data.classteacherlist = (from a in _PortalContext.ClassTeacherMappingDMO
                                             where a.MI_Id == data.MI_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == data.HRME_Id && a.ASMAY_Id == data.ASMAY_Id

                                             select a
                                                ).Distinct().ToArray();

                    data.subteacherlist = (from a in _PortalContext.Exm_Login_PrivilegeDMO
                                           from b in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                           from c in _PortalContext.Staff_User_Login
                                           where a.ELP_Id == b.ELP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ELP_ActiveFlg == true && a.Login_Id == c.IVRMSTAUL_Id && c.Emp_Code == data.HRME_Id && a.MI_Id == c.MI_Id && a.ELP_Flg.ToLower() == "st"
                                           select a).Distinct().Take(1).ToArray();

                    data.typelist = (from a in _PortalContext.HOD_DMO

                                     where a.IHOD_ActiveFlag == true && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id
                                     select a).Distinct().ToArray();

                }



                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_Inbox";
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
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@roleflg",
                    SqlDbType.VarChar)
                    {
                        Value = rolet.FirstOrDefault().IVRMRT_Role
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
                        data.getinboxmsg = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_ReadCount";
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
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@roleflg",
                    SqlDbType.VarChar)
                    {
                        Value = rolet.FirstOrDefault().IVRMRT_Role
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
                        data.getinboxmsg_readflg = retObject.ToArray();
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

        public async Task<IVRM_School_InteractionsDTO> getdetails(IVRM_School_InteractionsDTO data)
        {
            try
            {
                var type = _PortalContext.ApplicationUserRole.Where(a => a.UserId == data.UserId).ToList();
                var role = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == type[0].RoleTypeId).ToArray();

                data.configflag = _PortalContext.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id).Distinct().ToArray();
                if (data.roleflg.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    var clsid = (from a in _PortalContext.Adm_M_Student
                                 from b in _PortalContext.School_M_Class
                                 from c in _PortalContext.School_M_Section
                                 from d in _PortalContext.School_Adm_Y_StudentDMO
                                 from e in _PortalContext.AcademicYearDMO
                                 where (a.AMST_Id == d.AMST_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ASMAY_Id == e.ASMAY_Id && a.MI_Id == b.MI_Id && c.MI_Id == e.MI_Id && a.AMST_Id == data.AMST_Id && d.ASMAY_Id == data.ASMAY_Id)
                                 select new IVRM_School_InteractionsDTO
                                 {
                                     ASMCL_Id = b.ASMCL_Id,
                                     ASMS_Id = c.ASMS_Id,
                                     ASMC_SectionName = c.ASMC_SectionName,
                                     ASMCL_ClassName = b.ASMCL_ClassName
                                 }
                         ).Distinct().ToArray();


                    if (clsid.Length > 0)
                    {
                        data.asmclid = clsid.FirstOrDefault().ASMCL_Id;
                        data.asmsid = clsid.FirstOrDefault().ASMS_Id;
                    }

                }
                else if (data.roleflg != "student")
                {
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_filter";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.asmclid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.asmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@IINTS_Flag",
                    SqlDbType.VarChar)
                    {
                        Value = data.userflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@roletype",
                   SqlDbType.VarChar)
                    {
                        Value = role[0].IVRMRT_Role
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
                        data.getdetails = retObject.ToArray();
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
        public async Task<IVRM_School_InteractionsDTO> getstudent(IVRM_School_InteractionsDTO data)
        {
            try
            {
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_StudentList";
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
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
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
                        data.get_student = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //data.get_student = (from a in _PortalContext.Adm_M_Student
                //                    from b in _PortalContext.School_Adm_Y_StudentDMO
                //                    from d in _PortalContext.School_M_Class
                //                    from e in _PortalContext.School_M_Section
                //                    where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.AMAY_ActiveFlag == 1 && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                //                    select new IVRM_School_InteractionsDTO
                //                    {
                //                        AMST_Id = a.AMST_Id,
                //                        ASMCL_Id = b.ASMCL_Id,
                //                        AMST_AdmNo = a.AMST_AdmNo,
                //                        studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                //                    }).Distinct().OrderBy(t => t.studentName).ToArray();
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_School_InteractionsDTO savedetails(IVRM_School_InteractionsDTO data)
        {
            try
            {

                string image = "";
                string sentoflg = "";
                long istint_Id3 = 0;
                if (data.images_paths != null)
                {
                    foreach (var ig in data.images_paths)
                    {
                        image = ig;
                    }
                }
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                long ismint_Id = 0;
                long istint_Id = 0;
                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                List<IVRM_School_InteractionsDTO> devicelist = new List<IVRM_School_InteractionsDTO>();
                //IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();

                MM = _context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "InteractionStudent").ToList();
                if (MM.Count() > 0)
                {

                    data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                    data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                    data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                    data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                    data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                    data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                    data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                    data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                    data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                }

                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

                int level_no = 1;

                if (data.roleflg.Equals("student", StringComparison.OrdinalIgnoreCase))
                {


                    IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                    inter.MI_Id = data.MI_Id;
                    inter.ISMINT_InteractionId = data.trans_id;
                    inter.ASMAY_Id = data.ASMAY_Id;
                    inter.ISMINT_ComposedByFlg = data.roleflg;
                    inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                    inter.ISMINT_ComposedById = data.AMST_Id;
                    inter.ISMINT_Subject = data.ISMINT_Subject;
                    inter.ISMINT_DateTime = indianTime;
                    inter.ISMINT_Interaction = data.ISMINT_Interaction;
                    inter.ISMINT_ActiveFlag = true;
                    inter.ISMINT_CreatedBy = data.UserId;
                    inter.ISMINT_UpdatedBy = data.UserId;
                    inter.CreatedDate = indianTime;
                    inter.UpdatedDate = indianTime;
                    inter.ISMINT_Attachment = image;

                    inter.ISMINT_MACAddress = data.ISMINT_MACAddress;
                    inter.ISMINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                    _PortalContext.Add(inter);

                    //IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                    data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                      where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                      select new IVRM_School_InteractionsDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          HRME_MobileNo = a.HRME_MobileNo,
                                          HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                      }).Distinct().ToArray();

                    var devlist1 = (from a in _PortalContext.HR_Master_Employee_DMO
                                    where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                    select new IVRM_School_InteractionsDTO
                                    {
                                        HRME_Id = a.HRME_Id,
                                        HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                        employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                    }).Distinct().ToList();

                    IVRM_School_InteractionsDTO dto = new IVRM_School_InteractionsDTO();
                    data.devicelist12 = devlist1;

                    devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                  where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                  select new IVRM_School_InteractionsDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                      employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                  }).Distinct().ToList();
                    IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                    intrans.ISMINT_Id = inter.ISMINT_Id;
                    intrans.ISTINT_ToId = data.ISTINT_ToId;
                    intrans.ISTINT_ToFlg = "Staff";                                      
                    intrans.ISTINT_ComposedById = data.AMST_Id;
                    intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                    intrans.ISTINT_DateTime = indianTime;
                    intrans.ISTINT_ComposedByFlg = data.roleflg;
                    intrans.ISTINT_InteractionOrder = level_no;
                    intrans.ISTINT_ActiveFlag = true;
                    intrans.ISTINT_CreatedBy = data.UserId;
                    intrans.ISTINT_UpdatedBy = data.UserId;
                    intrans.ISTINT_Attachment = image;
                    intrans.CreatedDate = DateTime.Now;
                    intrans.UpdatedDate = DateTime.Now;
                    intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                    intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                    _PortalContext.Add(intrans);
                    ismint_Id = inter.ISMINT_Id;
                    istint_Id = intrans.ISTINT_Id;
                    istint_Id3 = intrans.ISMINT_Id;
                    intrans.ISTINT_ReadFlg = true;
                    var contactExists = _PortalContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        sentoflg = "Staff";
                        //============================== Notification
                        //var deviceidsnew = "";
                        //var devicenew = "";
                        //ismint_Id = intrans.ISMINT_Id;
                        //istint_Id = intrans.ISTINT_Id;
                        //if (devicelist.Count > 0)
                        //{
                        //    int k = 0;
                        //    foreach (var deviceid in devicelist)
                        //    {
                        //        if (k == 0)
                        //        {
                        //            deviceidsnew = '"' + deviceid.AppDownloadedDeviceId + '"';
                        //            k = 1;
                        //        }
                        //        else
                        //        {
                        //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AppDownloadedDeviceId + '"';
                        //        }

                        //        if (deviceid.AppDownloadedDeviceId != "" && deviceid.AppDownloadedDeviceId != null)
                        //        {
                        //            callnotificationNew(deviceid.AppDownloadedDeviceId, data.ISMINT_Subject, istint_Id, data.MI_Id, dto);
                        //        }

                        //    }
                        //    devicenew = "[" + deviceidsnew + "]";

                        //    //  callnotificationNew(devicenew, data.ISMINT_Subject, istint_Id, data.MI_Id, dto);
                        //}
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    if (data.ISMINT_GroupOrIndFlg == "Group")
                    {
                        IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                        inter.MI_Id = data.MI_Id;
                        inter.ISMINT_InteractionId = data.trans_id;
                        inter.ASMAY_Id = data.ASMAY_Id;
                        inter.ISMINT_ComposedByFlg = data.roleflg;
                        inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                        inter.ISMINT_ComposedById = data.HRME_Id;
                        inter.ISMINT_Subject = data.ISMINT_Subject;
                        inter.ISMINT_DateTime = indianTime;
                        inter.ISMINT_Interaction = data.ISMINT_Interaction;
                        inter.ISMINT_ActiveFlag = true;
                        inter.ISMINT_CreatedBy = data.HRME_Id;
                        inter.ISMINT_UpdatedBy = data.HRME_Id;
                        inter.CreatedDate = indianTime;
                        inter.UpdatedDate = indianTime;
                        inter.ISMINT_Attachment = image;
                        inter.ISMINT_MACAddress = data.ISMINT_MACAddress;
                        inter.ISMINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                        _PortalContext.Add(inter);

                        if (data.userflag == "Student")
                        {
                            List<long> device_ids = new List<long>();
                            foreach (var s in data.arrayStudent)
                            {
                                device_ids.Add(s.AMST_Id);

                                IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();

                                intrans.ISMINT_Id = inter.ISMINT_Id;
                                intrans.ISTINT_ToId = s.AMST_Id;
                                intrans.ISTINT_ToFlg = "Student";
                                intrans.ISTINT_ComposedById = data.HRME_Id;
                                intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                                intrans.ISTINT_DateTime = indianTime;
                                intrans.ISTINT_ComposedByFlg = data.roleflg;
                                intrans.ISTINT_InteractionOrder = level_no;
                                intrans.ISTINT_ActiveFlag = true;
                                intrans.ISTINT_CreatedBy = data.UserId;
                                intrans.ISTINT_UpdatedBy = data.UserId;
                                intrans.ISTINT_Attachment = image;
                                intrans.CreatedDate = DateTime.Now;
                                intrans.UpdatedDate = DateTime.Now;
                                intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                                intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                                intrans.ISTINT_ReadFlg = true;
                                _PortalContext.Add(intrans);
                                istint_Id = intrans.ISTINT_Id;
                                sentoflg = "Student";                               
                            }
                            ismint_Id = inter.ISMINT_Id;
                            data.deviceids = (from a in _PortalContext.Adm_M_Student
                                              where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_MobileNo = a.AMST_MobileNo,
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                                  studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                              }).Distinct().ToArray();
                            var devi = (from a in _PortalContext.Adm_M_Student
                                        where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                        select new IVRM_School_InteractionsDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                            studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                        }).Distinct().ToList();
                            data.devicelist12 = devi;

                            devicelist = (from a in _PortalContext.Adm_M_Student
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              AMST_Id = a.AMST_Id,
                                              AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                              studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                          }).Distinct().ToList();

                        }
                        else if (data.userflag == "Teachers")
                        {

                            List<long> device_ids = new List<long>();
                            foreach (var t in data.arrayTeachers)
                            {
                                device_ids.Add(t.HRME_Id);
                                IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                                intrans.ISMINT_Id = inter.ISMINT_Id;
                                intrans.ISTINT_ToId = t.HRME_Id;
                                intrans.ISTINT_ToFlg = "Staff";
                                intrans.ISTINT_ComposedById = data.HRME_Id;
                                intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                                intrans.ISTINT_DateTime = indianTime;
                                intrans.ISTINT_ComposedByFlg = data.roleflg;
                                intrans.ISTINT_InteractionOrder = level_no;
                                intrans.ISTINT_ActiveFlag = true;
                                intrans.ISTINT_CreatedBy = data.UserId;
                                intrans.ISTINT_UpdatedBy = data.UserId;
                                intrans.CreatedDate = DateTime.Now;
                                intrans.UpdatedDate = DateTime.Now;
                                intrans.ISTINT_Attachment = image;
                                intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                                intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                                intrans.ISTINT_ReadFlg = true;
                                _PortalContext.Add(intrans);
                                istint_Id = intrans.ISTINT_Id;
                                sentoflg = "Staff";
                            }
                            ismint_Id = inter.ISMINT_Id;
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              }).Distinct().ToArray();

                            var devlist = (from a in _PortalContext.HR_Master_Employee_DMO
                                           where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                           select new IVRM_School_InteractionsDTO
                                           {
                                               HRME_MobileNo = a.HRME_MobileNo,
                                               HRME_Id = a.HRME_Id,
                                               HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                               employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                           }).Distinct().ToList();
                            data.devicelist12 = devlist;

                            devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          }).Distinct().ToList();
                        }
                        else
                        {
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              }).Distinct().ToArray();

                            var devlist1 = (from a in _PortalContext.HR_Master_Employee_DMO
                                            where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                            select new IVRM_School_InteractionsDTO
                                            {
                                                HRME_Id = a.HRME_Id,
                                                HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                            }).Distinct().ToList();
                            data.devicelist12 = devlist1;


                            devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          }).Distinct().ToList();
                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.ISTINT_ToId;
                            intrans.ISTINT_ToFlg = "Staff";
                            intrans.ISTINT_ComposedById = data.HRME_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_no;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            intrans.ISTINT_Attachment = image;
                            intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                            intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            intrans.ISTINT_ReadFlg = true;
                            _PortalContext.Add(intrans);
                            ismint_Id = inter.ISMINT_Id;
                            istint_Id = intrans.ISTINT_Id;
                            sentoflg = "Staff";
                        }
                    }
                    else if (data.ISMINT_GroupOrIndFlg == "Individual")
                    {
                        var level_order = 1;
                        if (data.userflag == "Student")
                        {
                            data.trans_id = "";
                            if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                                data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                                data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                                data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                            }

                            IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                            inter.MI_Id = data.MI_Id;
                            inter.ISMINT_InteractionId = data.trans_id;
                            inter.ASMAY_Id = data.ASMAY_Id;
                            inter.ISMINT_ComposedByFlg = data.roleflg;
                            inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                            inter.ISMINT_ComposedById = data.HRME_Id;
                            inter.ISMINT_Subject = data.ISMINT_Subject;
                            inter.ISMINT_DateTime = indianTime;
                            inter.ISMINT_Interaction = data.ISMINT_Interaction;
                            inter.ISMINT_ActiveFlag = true;
                            inter.ISMINT_CreatedBy = data.UserId;
                            inter.ISMINT_UpdatedBy = data.UserId;
                            inter.CreatedDate = indianTime;
                            inter.UpdatedDate = indianTime;
                            inter.ISMINT_Attachment = image;
                            inter.ISMINT_MACAddress = data.ISMINT_MACAddress;
                            inter.ISMINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            _PortalContext.Add(inter);

                            data.deviceids = (from a in _PortalContext.Adm_M_Student
                                              where (a.MI_Id == data.MI_Id && a.AMST_Id == data.student_Id)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                                  studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                              }).Distinct().ToArray();

                            var slist = (from a in _PortalContext.Adm_M_Student
                                         where (a.MI_Id == data.MI_Id && a.AMST_Id == data.student_Id)
                                         select new IVRM_School_InteractionsDTO
                                         {
                                             AMST_MobileNo = a.AMST_MobileNo,
                                             AMST_Id = a.AMST_Id,
                                             AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                             studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                         }).Distinct().ToList();
                            data.devicelist12 = slist;

                            devicelist = (from a in _PortalContext.Adm_M_Student
                                          where (a.MI_Id == data.MI_Id && a.AMST_Id == data.student_Id)
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              AMST_Id = a.AMST_Id,
                                              AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                              studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                          }).Distinct().ToList();
                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.student_Id;
                            intrans.ISTINT_ToFlg = "Student";
                            intrans.ISTINT_ComposedById = data.HRME_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_order;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            intrans.ISTINT_Attachment = image;
                            intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                            intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            intrans.ISTINT_ReadFlg = true;
                            _PortalContext.Add(intrans);
                            ismint_Id = inter.ISMINT_Id;
                            istint_Id = intrans.ISTINT_Id;
                            sentoflg = "Student";
                           
                        }
                        else if (data.userflag == "Teachers")
                        {
                            data.trans_id = "";
                            if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                                data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                                data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                                data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                            }
                            //if (orderno == 0)
                            //{
                            //    orderno = orderno + 1;
                            //    level_order = level_no;
                            //}
                            //else
                            //{
                            //    level_order = level_order + 1;
                            //}
                            IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                            inter.MI_Id = data.MI_Id;
                            inter.ISMINT_InteractionId = data.trans_id;
                            inter.ASMAY_Id = data.ASMAY_Id;
                            inter.ISMINT_ComposedByFlg = data.roleflg;
                            inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                            inter.ISMINT_ComposedById = data.HRME_Id;
                            inter.ISMINT_Subject = data.ISMINT_Subject;
                            inter.ISMINT_DateTime = indianTime;
                            inter.ISMINT_Interaction = data.ISMINT_Interaction;
                            inter.ISMINT_ActiveFlag = true;
                            inter.ISMINT_CreatedBy = data.UserId;
                            inter.ISMINT_UpdatedBy = data.UserId;
                            inter.CreatedDate = indianTime;
                            inter.UpdatedDate = indianTime;
                            inter.ISMINT_MACAddress = data.ISMINT_MACAddress;
                            inter.ISMINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            inter.ISMINT_Attachment = image;

                            _PortalContext.Add(inter);

                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.employee_Id)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              }).Distinct().ToArray();
                            var sss = (from a in _PortalContext.HR_Master_Employee_DMO
                                       where (a.MI_Id == data.MI_Id && a.HRME_Id == data.employee_Id)
                                       select new IVRM_School_InteractionsDTO
                                       {
                                           HRME_MobileNo = a.HRME_MobileNo,
                                           HRME_Id = a.HRME_Id,
                                           HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                           employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                       }).Distinct().ToList();
                            data.devicelist12 = sss;
                            devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && a.HRME_Id == data.employee_Id)
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          }).Distinct().ToList();
                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.employee_Id;
                            intrans.ISTINT_ToFlg = "Staff";
                            intrans.ISTINT_ComposedById = data.HRME_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_order;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            intrans.ISTINT_Attachment = image;
                            intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                            intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            intrans.ISTINT_ReadFlg = true;
                            _PortalContext.Add(intrans);
                            ismint_Id = inter.ISMINT_Id;
                            istint_Id = intrans.ISTINT_Id;
                            sentoflg = "Staff";
                        }
                        else
                        {
                            IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                            inter.MI_Id = data.MI_Id;
                            inter.ISMINT_InteractionId = data.trans_id;
                            inter.ASMAY_Id = data.ASMAY_Id;
                            inter.ISMINT_ComposedByFlg = data.roleflg;
                            inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                            inter.ISMINT_ComposedById = data.HRME_Id;
                            inter.ISMINT_Subject = data.ISMINT_Subject;
                            inter.ISMINT_DateTime = indianTime;
                            inter.ISMINT_Interaction = data.ISMINT_Interaction;
                            inter.ISMINT_ActiveFlag = true;
                            inter.ISMINT_CreatedBy = data.UserId;
                            inter.ISMINT_UpdatedBy = data.UserId;
                            inter.CreatedDate = indianTime;
                            inter.UpdatedDate = indianTime;
                            inter.ISMINT_Attachment = image;
                            inter.ISMINT_MACAddress = data.ISMINT_MACAddress;
                            inter.ISMINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            _PortalContext.Add(inter);
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              }).Distinct().ToArray();

                            var devlist1 = (from a in _PortalContext.HR_Master_Employee_DMO
                                            where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                            select new IVRM_School_InteractionsDTO
                                            {
                                                HRME_MobileNo = a.HRME_MobileNo,
                                                HRME_Id = a.HRME_Id,
                                                HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                            }).Distinct().ToList();
                            data.devicelist12 = devlist1;

                            devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          }).Distinct().ToList();
                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.ISTINT_ToId;
                            intrans.ISTINT_ToFlg = "Staff";
                            intrans.ISTINT_ComposedById = data.HRME_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_no;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            intrans.ISTINT_Attachment = image;
                            intrans.ISTINT_ReadFlg = true;
                            _PortalContext.Add(intrans);
                            ismint_Id = inter.ISMINT_Id;
                            istint_Id = intrans.ISTINT_Id;
                            sentoflg = "Staff";
                        }
                    }
                    var contactExists = _PortalContext.SaveChanges();



                    //long istint_Id3 = 0;
                    var istint_Id1 = _PortalContext.IVRM_School_Master_InteractionsDMO.OrderByDescending(a => a.ISMINT_Id).ToList();
                    var istint_Id2 = istint_Id1.FirstOrDefault().ISMINT_Id;
                    istint_Id3 = istint_Id2;
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        //============================== Notification
                        //var deviceidsnew = "";
                        //var devicenew = "";
                        //var redirecturl = "";
                        //long revieveduserid = 0;

                        //if (devicelist.Count > 0)
                        //{
                        //    int k = 0;
                        //    foreach (var deviceid in devicelist)
                        //    {
                        //        if (k == 0)
                        //        {
                        //            deviceidsnew = '"' + deviceid.AppDownloadedDeviceId + '"';
                        //            k = 1;
                        //        }
                        //        else
                        //        {
                        //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AppDownloadedDeviceId + '"';
                        //        }

                        //        if (deviceid.AppDownloadedDeviceId != "" && deviceid.AppDownloadedDeviceId != null)
                        //        {
                        //            callnotificationNew(deviceid.AppDownloadedDeviceId, data.ISMINT_Subject, istint_Id3, data.MI_Id, data);
                        //        }
                        //    }
                        //    devicenew = "[" + deviceidsnew + "]";
                        //    // callnotificationNew(devicenew, data.ISMINT_Subject, istint_Id3, data.MI_Id, data);
                        //}
                    }
                    else
                    {
                        data.returnval = false;
                    }

                    if (data.returnval == true)
                    {
                        var deviceidsnew = "";
                        var devicenew = "";
                        var redirecturl = "";
                        long revieveduserid = 0;

                        if (devicelist.Count > 0)
                        {
                            foreach (var device_id in devicelist)
                            {
                                if (device_id.AppDownloadedDeviceId.Length > 0)
                                {
                                    if (sentoflg == "Staff")
                                    {
                                        revieveduserid = _PortalContext.IVRM_Staff_User_Login.Where(t => t.Emp_Code == device_id.HRME_Id).Select(t => t.Id).FirstOrDefault();
                                    }
                                    else
                                    {
                                        revieveduserid = (from a in _PortalContext.StudentUserLoginDMO
                                                          from b in _PortalContext.ApplicationUser
                                                          where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                          select b).Select(t => t.Id).FirstOrDefault();

                                    }

                                    PushNotification push_noti = new PushNotification(_PortalContext);
                                    push_noti.Call_PushNotificationGeneral(device_id.AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, istint_Id3, data.ISMINT_Interaction, "Interaction", "InteractionReply");

                                }
                            }
                        }
                    }
                  

                }
            }
            catch (Exception ex)
            {
                data.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<IVRM_School_InteractionsDTO> reply(IVRM_School_InteractionsDTO data)
        {
            try
            {



                var composeedby = _context.IVRM_School_Master_InteractionsDMO.Single(q => q.ISMINT_Id == data.ISMINT_Id);

                var composeedto = _context.IVRM_School_Transaction_InteractionsDMO.Where(q => q.ISMINT_Id == data.ISMINT_Id).ToList();

                data.composeedto = composeedto.FirstOrDefault().ISTINT_ToFlg.ToLower();


                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();

                data.rolename = rolet.FirstOrDefault().IVRMRT_Role.ToLower();
                long cmpid = 0;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    data.ISMINT_ComposedByFlg = composeedby.ISMINT_ComposedByFlg.ToLower();

                    if (data.ISMINT_ComposedByFlg == "student")
                    {
                        var trans = _PortalContext.IVRM_School_Transaction_InteractionsDMO.Where(q => q.ISMINT_Id == data.ISMINT_Id && q.ISTINT_ToFlg.ToLower() == "staff" && q.ISTINT_ActiveFlag == true).ToList();
                        if (trans.Count > 0)
                        {
                            cmpid = trans.FirstOrDefault().ISTINT_ToId;
                        }

                    }
                    else
                    {

                        cmpid = composeedby.ISMINT_ComposedById;

                    }

                    data.typelistrole = (from a in _PortalContext.IVRM_Role_Type
                                         from b in _PortalContext.Staff_User_Login
                                         from d in _PortalContext.ApplicationUserRole
                                         where b.MI_Id == data.MI_Id && b.Emp_Code == cmpid
                                         && d.UserId == b.Id && d.RoleTypeId == a.IVRMRT_Id
                                         select a).Distinct().ToArray();



                    data.typelist = (from a in _PortalContext.HOD_DMO

                                     where a.IHOD_ActiveFlag == true && a.HRME_Id == cmpid && a.MI_Id == data.MI_Id
                                     select a).Distinct().ToArray();



                    if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                    {
                        var clsid = (from a in _PortalContext.Adm_M_Student
                                     from b in _PortalContext.School_M_Class
                                     from c in _PortalContext.School_M_Section
                                     from d in _PortalContext.School_Adm_Y_StudentDMO
                                     from e in _PortalContext.AcademicYearDMO
                                     where (a.AMST_Id == d.AMST_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ASMAY_Id == e.ASMAY_Id && a.MI_Id == b.MI_Id && c.MI_Id == e.MI_Id && a.AMST_Id == data.AMST_Id && d.ASMAY_Id == data.ASMAY_Id)
                                     select new IVRM_School_InteractionsDTO
                                     {
                                         ASMCL_Id = b.ASMCL_Id,
                                         ASMS_Id = c.ASMS_Id,
                                         ASMC_SectionName = c.ASMC_SectionName,
                                         ASMCL_ClassName = b.ASMCL_ClassName
                                     }
                             ).Distinct().ToArray();


                        if (clsid.Length > 0)
                        {
                            data.asmclid = clsid.FirstOrDefault().ASMCL_Id;
                            data.asmsid = clsid.FirstOrDefault().ASMS_Id;
                        }


                        data.classteacherlist = (from a in _PortalContext.ClassTeacherMappingDMO
                                                 where a.MI_Id == data.MI_Id && a.IMCT_ActiveFlag == true && a.ASMCL_Id == data.asmclid && a.ASMS_Id == data.asmsid && a.HRME_Id == cmpid && a.ASMAY_Id == data.ASMAY_Id

                                                 select a
                                              ).Distinct().ToArray();


                        data.subteacherlist = (from a in _PortalContext.Exm_Login_PrivilegeDMO
                                               from b in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                               from c in _PortalContext.Staff_User_Login
                                               where a.ELP_Id == b.ELP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ELP_ActiveFlg == true && b.ASMCL_Id == data.asmclid && b.ASMS_Id == data.asmsid && a.Login_Id == c.IVRMSTAUL_Id && c.Emp_Code == cmpid && a.MI_Id == c.MI_Id && a.ELP_Flg.ToLower() == "st"
                                               select a).Distinct().Take(1).ToArray();

                    }





                }
                else
                {


                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                    data.ISMINT_ComposedByFlg = composeedby.ISMINT_ComposedByFlg.ToLower();


                    data.typelistrole = (from a in _PortalContext.IVRM_Role_Type
                                         from b in _PortalContext.Staff_User_Login
                                         from d in _PortalContext.ApplicationUserRole
                                         where b.MI_Id == data.MI_Id && b.Emp_Code == data.HRME_Id
                                         && d.UserId == b.Id && d.RoleTypeId == a.IVRMRT_Id
                                         select a).Distinct().ToArray();



                    data.typelist = (from a in _PortalContext.HOD_DMO

                                     where a.IHOD_ActiveFlag == true && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id
                                     select a).Distinct().ToArray();



                    data.classteacherlist = (from a in _PortalContext.ClassTeacherMappingDMO
                                             where a.MI_Id == data.MI_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == data.HRME_Id && a.ASMAY_Id == data.ASMAY_Id

                                             select a
                                          ).Distinct().ToArray();


                    data.subteacherlist = (from a in _PortalContext.Exm_Login_PrivilegeDMO
                                           from b in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                           from c in _PortalContext.Staff_User_Login
                                           where a.ELP_Id == b.ELP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ELP_ActiveFlg == true && a.Login_Id == c.IVRMSTAUL_Id && c.Emp_Code == data.HRME_Id && a.MI_Id == c.MI_Id && a.ELP_Flg.ToLower() == "st"
                                           select a).Distinct().Take(1).ToArray();



                }


                var configflaglist = _PortalContext.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id && a.IVRMGC_GMRDTOALLFlg == true).Distinct().ToList();


                int cnt = 0;
                if (cnt == 0)
                {
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_Interaction_View_Reply";
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
                        cmd.Parameters.Add(new SqlParameter("@ISMINT_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.ISMINT_Id
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
                            data.viewmessage = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    if (composeedby.ISMINT_GroupOrIndFlg.ToLower() == "group")
                    {

                        if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                        {
                            data.HRME_Id1 = data.AMST_Id;
                        }
                        else
                        {
                            data.HRME_Id1 = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                        }


                        using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "IVRM_Interaction_View_Reply_Group";
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
                            cmd.Parameters.Add(new SqlParameter("@ISMINT_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.ISMINT_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@STORHRMEID",
                          SqlDbType.BigInt)
                            {
                                Value = data.HRME_Id1
                            });
                            cmd.Parameters.Add(new SqlParameter("@SRole",
                          SqlDbType.Char)
                            {
                                Value = rolet.FirstOrDefault().IVRMRT_Role
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
                                data.viewmessage = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "IVRM_Interaction_View_Reply";
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
                            cmd.Parameters.Add(new SqlParameter("@ISMINT_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.ISMINT_Id
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
                                data.viewmessage = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }




                //var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                else
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                long loginuserid = 0;
                if (data.HRME_Id == 0)
                {
                    loginuserid = data.AMST_Id;
                }
                else if (data.AMST_Id == 0)
                {
                    loginuserid = data.HRME_Id;
                }
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_CreatedBy";
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
                    cmd.Parameters.Add(new SqlParameter("@loginuserid",
                    SqlDbType.BigInt)
                    {
                        Value = loginuserid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMINT_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.ISMINT_Id
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
                        data.get_msgcreator = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                var rmv = 0;

                long composedid = 0;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    composedid = (from a in _context.ApplicationUser
                                  from b in _context.StudentUserLoginDMO
                                  where (a.UserName == b.IVRMSTUUL_UserName && a.Id == data.UserId && b.MI_Id == data.MI_Id)
                                  select b.AMST_Id).Distinct().FirstOrDefault();
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    composedid = _context.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                else
                {
                    composedid = (from a in _context.ApplicationUser
                                  from b in _context.Staff_User_Login
                                  where (a.UserName == b.IVRMSTAUL_UserName && a.Id == data.UserId && b.MI_Id == data.MI_Id)
                                  select b.Emp_Code).Distinct().FirstOrDefault();
                }


                var result = _context.IVRM_School_Transaction_InteractionsDMO.Where(a => a.ISMINT_Id == data.ISMINT_Id && a.ISTINT_ToId == composedid).ToArray();

                if (result.Length > 0)
                {
                    foreach (var item in result)
                    {
                        if (item.ISTINT_ReadFlg != true)
                        {
                            item.ISTINT_ReadFlg = true;
                            item.UpdatedDate = DateTime.Today;
                            _context.Update(item);
                        }


                    }
                    rmv = _context.SaveChanges();
                }


                //if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                //{
                //    var result = _context.IVRM_School_Transaction_InteractionsDMO.Where(a => a.ISMINT_Id == data.ISMINT_Id && a.ISTINT_ComposedById == data.AMST_Id).ToArray();

                //    if (result.Length > 0)
                //    {
                //        foreach (var item in result)
                //        {
                //            if (item.ISTINT_ReadFlg != true)
                //            {
                //                item.ISTINT_ReadFlg = true;
                //                item.UpdatedDate = DateTime.Today;
                //                _context.Update(item);
                //            }


                //        }
                //        rmv = _context.SaveChanges();
                //    }
                //}
                //else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                //{
                //    var result = _context.IVRM_School_Transaction_InteractionsDMO.Where(a => a.ISMINT_Id == data.ISMINT_Id && a.ISTINT_ComposedById == data.HRME_Id).ToArray();

                //    if (result.Length > 0)
                //    {
                //        foreach (var item in result)
                //        {
                //            if (item.ISTINT_ReadFlg != true)
                //            {
                //                item.ISTINT_ReadFlg = true;
                //                item.UpdatedDate = DateTime.Today;
                //                _context.Update(item);
                //            }


                //        }
                //        rmv = _context.SaveChanges();
                //    }
                //}
                //else
                //{
                //    var result = _context.IVRM_School_Transaction_InteractionsDMO.Where(a => a.ISMINT_Id == data.ISMINT_Id && a.ISTINT_ComposedById == data.HRME_Id).ToArray();

                //    if (result.Length > 0)
                //    {
                //        foreach (var item in result)
                //        {
                //            if (item.ISTINT_ReadFlg != true)
                //            {
                //                item.ISTINT_ReadFlg = true;
                //                item.UpdatedDate = DateTime.Today;
                //                _context.Update(item);
                //            }


                //        }
                //        rmv = _context.SaveChanges();
                //    }
                //}




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_School_InteractionsDTO savereply(IVRM_School_InteractionsDTO data)
        {
            try
            {
                string image = "";
                if (data.images_paths != null)
                {
                    foreach (var ig in data.images_paths)
                    {
                        image = ig;
                    }
                }
                long toId = 0;
                long sentoId = 0;
                var sentoflg = "";
                long byId = 0;
                var toflg = "";
                var byflg = "";
                var groupOrIndFlg = "";
                int level_no = 0;
                var level_order = 0;
                long composeby = 0;
                string composeflag = "";
                string notiSubject = "";

                List<IVRM_School_InteractionsDTO> deviceid = new List<IVRM_School_InteractionsDTO>();
                List<IVRM_School_InteractionsDTO> deviceiddddd = new List<IVRM_School_InteractionsDTO>();
                data.deviceids = deviceid.ToArray();
                List<IVRM_School_InteractionsDTO> devicelist = new List<IVRM_School_InteractionsDTO>();
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                List<long> device_ids = new List<long>();
                List<long> device_grp = new List<long>();

                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                    composeby = data.AMST_Id;
                    composeflag = "Student";
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    var empid = _PortalContext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Distinct().ToList();
                    composeby = empid.FirstOrDefault().Emp_Code;
                    composeflag = "Staff";
                }

                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("HOD", StringComparison.OrdinalIgnoreCase))
                {
                    var empid = _PortalContext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Distinct().ToList();
                    composeby = empid.FirstOrDefault().Emp_Code;
                    composeflag = "Staff";
                }

                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase))
                {
                    var empid = _PortalContext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Distinct().ToList();
                    composeby = empid.FirstOrDefault().Emp_Code;
                    composeflag = "Staff";
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Chairman", StringComparison.OrdinalIgnoreCase))
                {
                    var empid = _PortalContext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Distinct().ToList();
                    composeby = empid.FirstOrDefault().Emp_Code;
                    composeflag = "Staff";
                }

                var comp_id = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                               from b in _PortalContext.IVRM_School_Master_InteractionsDMO
                               where (a.ISMINT_Id == b.ISMINT_Id && a.ISMINT_Id == data.ISMINT_Id && b.MI_Id == data.MI_Id)
                               select new IVRM_School_InteractionsDTO
                               {
                                   ISMINT_Id = b.ISMINT_Id,
                                   ISTINT_Id = a.ISTINT_Id,
                                   ISTINT_ComposedByFlg = a.ISTINT_ComposedByFlg,
                                   ISTINT_ComposedById = a.ISTINT_ComposedById,
                                   ISTINT_ToFlg = a.ISTINT_ToFlg,
                                   ISTINT_ToId = a.ISTINT_ToId,
                                   ISMINT_GroupOrIndFlg = b.ISMINT_GroupOrIndFlg,
                               }).Distinct().OrderBy(o => o.ISTINT_Id).ToList();

                toId = comp_id.FirstOrDefault().ISTINT_ToId;
                toflg = comp_id.FirstOrDefault().ISTINT_ToFlg;
                byId = comp_id.FirstOrDefault().ISTINT_ComposedById;
                byflg = comp_id.FirstOrDefault().ISTINT_ComposedByFlg;
                groupOrIndFlg = comp_id.FirstOrDefault().ISMINT_GroupOrIndFlg;

                if (composeby == byId)
                {
                    byId = toId;
                }

                var getuserid = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                                 from b in _PortalContext.IVRM_School_Master_InteractionsDMO
                                 where (a.ISMINT_Id == b.ISMINT_Id && a.ISMINT_Id == data.ISMINT_Id && b.MI_Id == data.MI_Id )
                                 select new IVRM_School_InteractionsDTO
                                 {
                                     ISMINT_Id = b.ISMINT_Id,
                                     ISTINT_Id = a.ISTINT_Id,
                                     ISTINT_ComposedByFlg = a.ISTINT_ComposedByFlg,
                                     ISTINT_ComposedById = a.ISTINT_ComposedById,
                                     ISTINT_ToFlg = a.ISTINT_ToFlg,
                                     ISTINT_ToId = a.ISTINT_ToId,
                                     ISMINT_GroupOrIndFlg = b.ISMINT_GroupOrIndFlg,
                                 }).Distinct().OrderByDescending(o => o.ISTINT_Id).ToList().Take(1);

                if (composeflag == "Staff")
                {
                    if (composeby == byId && composeflag == byflg && toflg == "Staff")
                    {
                        if (groupOrIndFlg == "Group")
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                            device_ids.Add(byId);
                        }
                        else
                        {
                            sentoId = toId;
                            sentoflg = "Staff";
                        }
                        foreach (var r in getuserid)
                        {
                            device_ids.Add(r.ISTINT_ToId);
                        }
                    }
                    else if (composeby == byId && composeflag == byflg && toflg == "Student")
                    {
                        if (groupOrIndFlg == "Group")
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                        }
                        else
                        {
                            sentoId = toId;
                            sentoflg = "Student";
                        }
                        foreach (var r in getuserid)
                        {
                            device_ids.Add(r.ISTINT_ToId);
                        }
                    }
                    else if (composeby != byId && composeflag == byflg && toflg == "Staff")
                    {
                        if (groupOrIndFlg == "Group")
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                            device_ids.Add(byId);
                        }
                        else
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                        }
                        foreach (var r in getuserid)
                        {
                            device_ids.Add(r.ISTINT_ToId);
                        }
                    }
                    else
                    {
                        if (groupOrIndFlg == "Group")
                        {
                            sentoId = toId;
                            sentoflg = "Student";
                            foreach (var r in getuserid)
                            {
                                device_grp.Add(r.ISTINT_ToId);
                            }
                        }
                        else
                        {
                            sentoId = byId;
                            sentoflg = "Student";
                        }
                        foreach (var r in getuserid)
                        {
                            device_ids.Add(r.ISTINT_ComposedById);
                        }
                    }
                    if (byflg == "Staff")
                    {
                        if (toflg == "Student")
                        {
                            //var deviceidsgg = (from a in _PortalContext.Adm_M_Student
                            //                   where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                            //                   select new IVRM_School_InteractionsDTO
                            //                   {
                            //                       AMST_Id = a.AMST_Id,
                            //                       studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                            //                       AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                            //                       AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                            //                   }).Distinct().ToList();
                            var deviceidsgg = (from a in _PortalContext.Adm_M_Student
                                               where (a.MI_Id == data.MI_Id && a.AMST_Id == toId)
                                               select new IVRM_School_InteractionsDTO
                                               {
                                                   AMST_Id = a.AMST_Id,
                                                   studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                                   AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                                   AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                               }).Distinct().ToList();

                            var deviceidGrpddd = (from a in _PortalContext.HR_Master_Employee_DMO
                                                  where (a.MI_Id == data.MI_Id && device_grp.Contains(a.HRME_Id))
                                                  select new IVRM_School_InteractionsDTO
                                                  {
                                                      HRME_Id = a.HRME_Id,
                                                      employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                                      HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                      AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  }).Distinct().ToList();

                            data.deviceids = deviceidsgg.ToArray();
                            data.deviceidGrp = deviceidGrpddd.ToArray();

                            for (int i = 0; i < deviceidsgg.Count; i++)
                            {
                                devicelist.Add(deviceidsgg[i]);
                            }

                            for (int j = 0; j < deviceidGrpddd.Count; j++)
                            {
                                devicelist.Add(deviceidGrpddd[j]);
                            }

                        }
                        else
                        {
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              }).Distinct().ToArray();
                            devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                          }).Distinct().ToList();

                            var d1 = (from a in _PortalContext.HR_Master_Employee_DMO
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                      select new IVRM_School_InteractionsDTO
                                      {
                                          HRME_MobileNo = a.HRME_MobileNo,
                                          HRME_Id = a.HRME_Id,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                      }).Distinct().ToList();
                            data.devicelist12 = d1;
                        }
                    }
                    else if (byflg == "Student")
                    {
                        data.deviceids = (from a in _PortalContext.Adm_M_Student
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              AMST_Id = a.AMST_Id,
                                              studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                              AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                          }).Distinct().ToArray();
                        devicelist = (from a in _PortalContext.Adm_M_Student
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                      select new IVRM_School_InteractionsDTO
                                      {
                                          AMST_Id = a.AMST_Id,
                                          studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                          AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                      }).Distinct().ToList();

                        var d2 = (from a in _PortalContext.Adm_M_Student
                                  where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                  select new IVRM_School_InteractionsDTO
                                  {
                                      HRME_MobileNo = a.AMST_MobileNo,
                                      AMST_Id = a.AMST_Id,
                                      studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                      AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                  }).Distinct().ToList();
                        data.devicelist12 = d2;
                    }
                }
                else if (composeflag == "Student")
                {
                    if (composeby == byId && composeflag == byflg && toflg == "Staff")
                    {
                        if (groupOrIndFlg == "Group" && byflg == "Student")
                        {
                            sentoId = byId;
                            sentoflg = "Student";
                            foreach (var r in getuserid)
                            {
                                device_grp.Add(r.ISTINT_ToId);
                            }
                        }
                        else if (groupOrIndFlg == "Group" && byflg == "Staff")
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                        }
                        else
                        {
                            sentoId = toId;
                            sentoflg = "Staff";
                        }
                        foreach (var r in getuserid)
                        {
                            device_ids.Add(r.ISTINT_ToId);
                        }
                    }
                    else
                    {
                        if (groupOrIndFlg == "Group" && byflg == "Student")
                        {
                            sentoId = byId;
                            sentoflg = "Student";

                        }
                        else if (groupOrIndFlg == "Group" && byflg == "Staff")
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                            foreach (var r in getuserid)
                            {
                                device_grp.Add(r.ISTINT_ComposedById);
                            }
                        }
                        else
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                        }
                        if (groupOrIndFlg == "Group")
                        {
                            foreach (var r in getuserid)
                            {
                                device_ids.Add(r.ISTINT_ToId);
                            }
                        }
                        else
                        {
                            foreach (var r in getuserid)
                            {
                                device_ids.Add(r.ISTINT_ComposedById);
                            }
                        }
                    }

                    if (groupOrIndFlg == "Group" && byflg == "Student")
                    {
                        data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                          }).Distinct().ToArray();

                        devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                      select new IVRM_School_InteractionsDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                      }).Distinct().ToList();
                        var d3 = (from a in _PortalContext.HR_Master_Employee_DMO
                                  where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                  select new IVRM_School_InteractionsDTO
                                  {
                                      HRME_MobileNo = a.HRME_MobileNo,
                                      HRME_Id = a.HRME_Id,
                                      employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                      AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                  }).Distinct().ToList();
                        data.devicelist12 = d3;
                    }
                    else if (groupOrIndFlg == "Group" && byflg == "Staff")
                    {
                        var deviceidsgg = (from a in _PortalContext.Adm_M_Student
                                           where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                           select new IVRM_School_InteractionsDTO
                                           {
                                               AMST_Id = a.AMST_Id,
                                               studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                               AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                               AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                           }).Distinct().ToList();

                        var deviceidGrpddd = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && device_grp.Contains(a.HRME_Id))
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_MobileNo = a.HRME_MobileNo,
                                                  HRME_Id = a.HRME_Id,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              }).Distinct().ToList();
                        data.devicelist12 = deviceidGrpddd;

                        data.deviceids = deviceidsgg.ToArray();
                        data.deviceidGrp = deviceidGrpddd.ToArray();

                        for (int i = 0; i < deviceidsgg.Count; i++)
                        {
                            devicelist.Add(deviceidsgg[i]);
                        }

                        for (int j = 0; j < deviceidGrpddd.Count; j++)
                        {
                            devicelist.Add(deviceidGrpddd[j]);
                        }

                    }
                    else
                    {
                        data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                          }).Distinct().ToArray();

                        devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                      select new IVRM_School_InteractionsDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                      }).Distinct().ToList();
                        var d4 = (from a in _PortalContext.HR_Master_Employee_DMO
                                  where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                  select new IVRM_School_InteractionsDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                      AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                  }).Distinct().ToList();
                        data.devicelist12 = d4;
                    }

                }


                var orderno = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                               from b in _PortalContext.IVRM_School_Master_InteractionsDMO
                               where (a.ISMINT_Id == b.ISMINT_Id && b.MI_Id == data.MI_Id && a.ISMINT_Id == data.ISMINT_Id)
                               select new IVRM_School_InteractionsDTO
                               {
                                   ISTINT_InteractionOrder = a.ISTINT_InteractionOrder
                               }).Distinct().ToList();

                level_no = orderno.LastOrDefault().ISTINT_InteractionOrder;
                if (level_no <= 0)
                {

                    level_order = 1;
                }
                else
                {
                    level_order = level_no + 1;
                }

                IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                intrans.ISMINT_Id = data.ISMINT_Id;
                intrans.ISTINT_ToId = sentoId;
                intrans.ISTINT_ToFlg = sentoflg;
                intrans.ISTINT_ComposedById = composeby;
                intrans.ISTINT_Interaction = data.ISTINT_Interaction;
                intrans.ISTINT_DateTime = indianTime;
                intrans.ISTINT_ComposedByFlg = composeflag;
                intrans.ISTINT_InteractionOrder = level_order;
                intrans.ISTINT_ActiveFlag = true;
                intrans.ISTINT_CreatedBy = data.UserId;
                intrans.ISTINT_UpdatedBy = data.UserId;
                intrans.ISTINT_ISPIPAddress = data.ISMINT_MACAddress;
                intrans.ISTINT_MACAddress = data.ISMINT_ISPIPAddress;
                intrans.CreatedDate = indianTime;
                intrans.UpdatedDate = indianTime;
                intrans.ISTINT_Attachment = image;
                intrans.ISTINT_ReadFlg = false;
                _PortalContext.Add(intrans);

                var contactExists = _PortalContext.SaveChanges();

                long ISTINT_Id3 = 0;
                var ISTINT_Id1 = _PortalContext.IVRM_School_Transaction_InteractionsDMO.OrderByDescending(a => a.ISTINT_Id).ToList();
                var ISTINT_Id2 = ISTINT_Id1.FirstOrDefault().ISTINT_Id;
                var ISMINT_Id2 = ISTINT_Id1.FirstOrDefault().ISMINT_Id;
                ISTINT_Id3 = ISTINT_Id2;

                if (contactExists > 0)
                {
                    data.returnval = true;
                    if (groupOrIndFlg == "Individual" && byflg == "Staff")
                    {
                        var employeedata = _PortalContext.HR_Master_Employee_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && device_ids.Contains(a.HRME_Id)).Distinct().ToList();

                        // var employeedata = _PortalContext.HR_Master_Employee_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == intrans.ISTINT_ComposedById).Distinct().ToList();
                        notiSubject = employeedata.FirstOrDefault().HRME_EmployeeFirstName + ' ' + employeedata.FirstOrDefault().HRME_EmployeeMiddleName + ' ' + employeedata.FirstOrDefault().HRME_EmployeeLastName;

                    }
                    else if (groupOrIndFlg == "Individual" && byflg == "Student")
                    {
                        var studentdata = _PortalContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && a.AMST_Id == intrans.ISTINT_ComposedById).Distinct().ToList();
                        notiSubject = studentdata.FirstOrDefault().AMST_FirstName + ' ' + studentdata.FirstOrDefault().AMST_MiddleName + ' ' + studentdata.FirstOrDefault().AMST_LastName;
                    }
                    else if (groupOrIndFlg == "Group")
                    {
                        notiSubject = "Group Message";
                    }

                    //============================== Notification

                    var deviceidsnew = "";
                    var devicenew = "";
                    var redirecturl = "";
                    long revieveduserid = 0;

                    if (devicelist.Count > 0)                    {                        foreach (var device_id in devicelist)                        {                            if (device_id.AppDownloadedDeviceId.Length > 0)                            {                                if (sentoflg == "Staff")
                                {
                                    revieveduserid = _PortalContext.IVRM_Staff_User_Login.Where(t => t.Emp_Code == device_id.HRME_Id).Select(t => t.Id).FirstOrDefault();
                                }                                else
                                {
                                    revieveduserid = (from a in _PortalContext.StudentUserLoginDMO
                                                      from b in _PortalContext.ApplicationUser
                                                      where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id== device_id.AMST_Id)
                                                      select b).Select(t => t.Id).FirstOrDefault();

                                }

                                PushNotification push_noti = new PushNotification(_PortalContext);                                push_noti.Call_PushNotificationGeneral(device_id.AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, ISMINT_Id2, data.ISTINT_Interaction, "Interaction", "InteractionReply");                            }                        }                    }


                    //if (devicelist.Count > 0)
                    //{
                    //    int k = 0;
                    //    foreach (var device_id in devicelist)
                    //    {
                    //        if (k == 0)
                    //        {
                    //            deviceidsnew = '"' + device_id.AppDownloadedDeviceId + '"';
                    //            k = 1;
                    //        }
                    //        else
                    //        {
                    //            deviceidsnew = deviceidsnew + "," + '"' + device_id.AppDownloadedDeviceId + '"';
                    //        }
                    //    }
                    //    devicenew = "[" + deviceidsnew + "]";

                    //    callnotification(devicenew, notiSubject, intrans.ISTINT_Id, data.MI_Id, ISTINT_Id3, data);
                    //}






                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string callnotificationNew(string devicenew, string subject, long istint_Id, long mi_id, IVRM_School_InteractionsDTO dto)
        {
            try
            {
                var key = _PortalContext.MobileApplAuthenticationDMO.Single(a => a.MI_Id == mi_id).MAAN_AuthenticationKey;
                // IVRM_InteractionsDTO data = new IVRM_InteractionsDTO();
                //var interaction = (from a in _PortalContext.IVRM_School_Master_InteractionsDMO
                //                   from b in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                //                   where (a.ISMINT_Id == b.ISMINT_Id && a.ISMINT_ActiveFlag == true && a.MI_Id == mi_id && b.ISMINT_Id == ismint_Id && b.ISTINT_Id == istint_Id)
                //                   select new IVRM_School_InteractionsDTO
                //                   {
                //                       ISMINT_Subject = a.ISMINT_Subject,
                //                       ISTINT_Interaction = b.ISTINT_Interaction
                //                   }).Distinct().ToList();

                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //     "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + interaction.FirstOrDefault().ISMINT_Subject + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + interaction.FirstOrDefault().ISTINT_Interaction + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                string sound = "default";
                string notId = "1";
                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                // "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + interaction.FirstOrDefault().ISMINT_Subject + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' +
                // +'"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , "
                // + '"' + "body" + '"' + ":" + '"' + interaction.FirstOrDefault().ISTINT_Interaction + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";


                //  daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //"" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "New Message" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + subject + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                //  notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                //var mycontent = notificationparams[0];
                //string postdata = mycontent.ToString();




                Dictionary<string, object> input = new Dictionary<string, object>();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                transfersnotes.Add("body", subject);
                transfersnotes.Add("title", "New Message");

                input.Add("to", devicenew);
                input.Add("notification", transfersnotes);


                var myContent = JsonConvert.SerializeObject(input);
                String postdata = myContent;


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
                PushNotification push_noti = new PushNotification(_PortalContext);

                push_noti.Insert_PushNotification_interaction(subject, istint_Id, mi_id, dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";

        }

        public string callnotification(string devicenew, string notiSubject, long istint_Id, long mi_id, long ISTINT_Id3, IVRM_School_InteractionsDTO dto)
        {
            try
            {
                var key = _PortalContext.MobileApplAuthenticationDMO.Single(a => a.MI_Id == mi_id).MAAN_AuthenticationKey;
                //IVRM_InteractionsDTO data = new IVRM_InteractionsDTO();
                //var interaction = (from a in _PortalContext.IVRM_School_Master_InteractionsDMO
                //                   from b in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                //                   where (a.ISMINT_Id == b.ISMINT_Id && a.ISMINT_ActiveFlag == true && a.MI_Id == mi_id && b.ISMINT_Id == ismint_Id && b.ISTINT_Id == istint_Id)
                //                   select new IVRM_School_InteractionsDTO
                //                   {
                //                       ISMINT_Subject = a.ISMINT_Subject,
                //                       ISTINT_Interaction = b.ISTINT_Interaction
                //                   }).Distinct().ToList();

                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //     "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + notiSubject + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + interaction.FirstOrDefault().ISTINT_Interaction + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";


                string sound = "default";
                string notId = "1";
                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                // "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + notiSubject + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' +
                // +'"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , "
                // + '"' + "body" + '"' + ":" + '"' + interaction.FirstOrDefault().ISTINT_Interaction + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";


                //   daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //"" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Reply" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + notiSubject + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";
                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
             "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Interaction" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + notiSubject + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " }" + "," + '"' + "data" + '"' + ":" + "{" + '"' + "page" + '"' + ":" + '"' + "interaction" + '"' + "}" + "}";

                notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
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
                PushNotification push_noti = new PushNotification(_PortalContext);

                push_noti.Insert_PushNotification_interaction_replay(notiSubject, ISTINT_Id3, mi_id, dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";

        }

        public IVRM_School_InteractionsDTO deletemsg(IVRM_School_InteractionsDTO dto)
        {
            try
            {
                var result = _context.IVRM_School_Transaction_InteractionsDMO.Single(a => a.ISTINT_Id == dto.ISTINT_Id);
                result.ISTINT_ActiveFlag = false;
                result.UpdatedDate = DateTime.Today;
                _context.Update(result);
                var rmv = _context.SaveChanges();
                if (rmv > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = false;
                }
            }

            catch (Exception ex)
            {
                dto.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public IVRM_School_InteractionsDTO deleteinboxmsg(IVRM_School_InteractionsDTO dto)
        {
            try
            {


                var result = _context.IVRM_School_Transaction_InteractionsDMO.Where(a => a.ISMINT_Id == dto.ISMINT_Id).ToList();
                foreach (var item in result)
                {
                    var updt = _context.IVRM_School_Transaction_InteractionsDMO.Single(a => a.ISMINT_Id == item.ISMINT_Id && a.ISTINT_Id == item.ISTINT_Id);
                    updt.ISTINT_ActiveFlag = false;
                    updt.UpdatedDate = DateTime.Today;
                    _context.Update(updt);
                }
                var resultnew = _context.IVRM_School_Master_InteractionsDMO.Single(a => a.ISMINT_Id == dto.ISMINT_Id);
                resultnew.ISMINT_ActiveFlag = false;
                resultnew.UpdatedDate = DateTime.Today;
                _context.Update(resultnew);


                var rmv = _context.SaveChanges();
                if (rmv > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = false;
                }
            }

            catch (Exception ex)
            {
                dto.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        //public IVRM_School_InteractionsDTO savereply(IVRM_School_InteractionsDTO data)
        //{
        //    try
        //    {
        //        long toId = 0;
        //        var toflg = "";
        //        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

        //        var comp_id = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
        //                       from b in _PortalContext.IVRM_School_Master_InteractionsDMO
        //                       where (a.ISMINT_Id == b.ISMINT_Id && b.MI_Id == data.MI_Id && a.ISMINT_Id == data.ISMINT_Id)
        //                       select new IVRM_School_InteractionsDTO
        //                       {
        //                           ISTINT_Id=a.ISTINT_Id,
        //                           ISTINT_ComposedById = a.ISTINT_ComposedById,
        //                           ISTINT_ComposedByFlg = a.ISTINT_ComposedByFlg
        //                       }
        //     ).Distinct().ToList();
        //        toId = comp_id.FirstOrDefault().ISTINT_ComposedById;
        //        toflg = comp_id.FirstOrDefault().ISTINT_ComposedByFlg;

        //        int level_no = 0;
        //        var level_order = 0;
        //        var orderno = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
        //                       where (a.ISMINT_Id == data.ISMINT_Id)
        //                       select new IVRM_School_InteractionsDTO
        //                       {
        //                           ISTINT_InteractionOrder = a.ISTINT_InteractionOrder
        //                       }
        //        ).Distinct().ToList();

        //        level_no = orderno.LastOrDefault().ISTINT_InteractionOrder;



        //        IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
        //        if (data.ISTINT_ComposedByFlg == "Student")
        //        {
        //            if (toflg == "Student")
        //            {
        //                comp_id = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
        //                           where (a.ISMINT_Id == data.ISMINT_Id)
        //                           select new IVRM_School_InteractionsDTO
        //                           {
        //                               ISTINT_ToId = a.ISTINT_ToId,
        //                               ISTINT_ToFlg = a.ISTINT_ToFlg
        //                           }
        //                             ).Distinct().ToList();
        //                toId = comp_id.FirstOrDefault().ISTINT_ToId;
        //                toflg = comp_id.FirstOrDefault().ISTINT_ToFlg;
        //            }
        //            else
        //            {
        //                comp_id = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
        //                           where (a.ISMINT_Id == data.ISMINT_Id)
        //                           select new IVRM_School_InteractionsDTO
        //                           {
        //                               ISTINT_ComposedById = a.ISTINT_ComposedById,
        //                               ISTINT_ComposedByFlg = a.ISTINT_ComposedByFlg
        //                           }
        //         ).Distinct().ToList();
        //                toId = comp_id.FirstOrDefault().ISTINT_ComposedById;
        //                toflg = comp_id.FirstOrDefault().ISTINT_ComposedByFlg;
        //            }
        //            if (level_no <= 0)
        //            {
        //                level_order = 1;
        //            }
        //            else
        //            {
        //                level_order = level_no + 1;
        //            }
        //            intrans.ISMINT_Id = data.ISMINT_Id;
        //            intrans.ISTINT_ToId = toId;
        //            intrans.ISTINT_ToFlg = toflg;
        //            intrans.ISTINT_ComposedById = data.AMST_Id;
        //            intrans.ISTINT_Interaction = data.ISTINT_Interaction;
        //            intrans.ISTINT_DateTime = indianTime;
        //            intrans.ISTINT_ComposedByFlg = data.ISTINT_ComposedByFlg;
        //            intrans.ISTINT_InteractionOrder = level_order;
        //            intrans.ISTINT_ActiveFlag = true;
        //            intrans.ISTINT_CreatedBy = data.AMST_Id;
        //            intrans.ISTINT_UpdatedBy = data.AMST_Id;
        //            intrans.CreatedDate = indianTime;
        //            intrans.UpdatedDate = indianTime;
        //            _PortalContext.Add(intrans);
        //        }
        //        else
        //        {
        //            if (toflg == "Staff")
        //            {
        //                comp_id = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
        //                           where (a.ISMINT_Id == data.ISMINT_Id)
        //                           select new IVRM_School_InteractionsDTO
        //                           {
        //                               ISTINT_ToId = a.ISTINT_ToId,
        //                               ISTINT_ToFlg = a.ISTINT_ToFlg
        //                           }
        //                             ).Distinct().ToList();
        //                toId = comp_id.FirstOrDefault().ISTINT_ToId;
        //                toflg = comp_id.FirstOrDefault().ISTINT_ToFlg;
        //            }
        //            else
        //            {
        //                comp_id = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
        //                           where (a.ISMINT_Id == data.ISMINT_Id)
        //                           select new IVRM_School_InteractionsDTO
        //                           {
        //                               ISTINT_ComposedById = a.ISTINT_ComposedById,
        //                               ISTINT_ComposedByFlg = a.ISTINT_ComposedByFlg
        //                           }
        //         ).Distinct().ToList();
        //                toId = comp_id.FirstOrDefault().ISTINT_ComposedById;
        //                toflg = comp_id.FirstOrDefault().ISTINT_ComposedByFlg;
        //            }
        //            if (level_no <= 0)
        //            {
        //                level_order = 1;
        //            }
        //            else
        //            {
        //                level_order = level_no + 1;
        //            }
        //            data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
        //            intrans.ISMINT_Id = data.ISMINT_Id;
        //            intrans.ISTINT_ToId = toId;
        //            intrans.ISTINT_ToFlg = toflg;
        //            intrans.ISTINT_ComposedById = data.HRME_Id;
        //            intrans.ISTINT_Interaction = data.ISTINT_Interaction;
        //            intrans.ISTINT_DateTime = indianTime;
        //            intrans.ISTINT_ComposedByFlg = data.ISTINT_ComposedByFlg;
        //            intrans.ISTINT_InteractionOrder = level_order;
        //            intrans.ISTINT_ActiveFlag = true;
        //            intrans.ISTINT_CreatedBy = data.HRME_Id;
        //            intrans.ISTINT_UpdatedBy = data.HRME_Id;
        //            intrans.CreatedDate = indianTime;
        //            intrans.UpdatedDate = indianTime;
        //            _PortalContext.Add(intrans);
        //        }
        //        var contactExists = _PortalContext.SaveChanges();
        //        if (contactExists > 0)
        //        {
        //            data.returnval = true;
        //        }
        //        else
        //        {
        //            data.returnval = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        data.message = "Error";
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}

        public IVRM_School_InteractionsDTO seen(IVRM_School_InteractionsDTO dto)
        {
            try
            {
                var rmv = 0;
                var result = _context.IVRM_School_Transaction_InteractionsDMO.Single(a => a.ISTINT_Id == dto.ISTINT_Id);
                if (result != null)
                {
                    result.ISTINT_ReadFlg = true;
                    result.UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    rmv = _context.SaveChanges();
                }
               


                if (rmv > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = false;
                }
            }

            catch (Exception ex)
            {
                dto.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

    }
}
