using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Alumni;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Alumni;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AlumniHub.Com.Service
{
    public class Alumni_Interactions_Impl : Interface.Alumni_Interactions_Interface
    {
        public AlumniContext _AlumniContext;
        public DomainModelMsSqlServerContext _context;
       // private PortalContext _PortalContext;
        
        public Alumni_Interactions_Impl(AlumniContext AlumniContext, DomainModelMsSqlServerContext context)
        {
            _AlumniContext = AlumniContext;
            _context = context;
           // _PortalContext = portalContext;
        }
        public Alumni_School_Interactions_DTO getloaddata(Alumni_School_Interactions_DTO data)
        {
            try
            {
                data.HRME_Id = 0;

                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    var role = _AlumniContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                    data.roletype = role.ToArray();
                }

                if (data.ALMST_Id > 0)
                {
                    var role1 = _AlumniContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();


                    if (role1.FirstOrDefault().IVRMRT_Role.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                    {

                        data.userhrmE_Id = data.ALMST_Id;
                    }

                    using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_ALUMNI_Interaction_Inbox";
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
                        cmd.Parameters.Add(new SqlParameter("@ALMST_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.ALMST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@roleflg",
                        SqlDbType.VarChar)
                        {
                            Value = role1[0].IVRMRT_Role
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
                            data.getinboxmsg = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public Alumni_School_Interactions_DTO getdetails(Alumni_School_Interactions_DTO data)
        {
            try
            {

                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _AlumniContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }

                var rolet = _AlumniContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                {

                    data.useralmst = data.ALMST_Id;
                    var al = _AlumniContext.Alumni_M_StudentDMO.Where(a => a.ALMST_Id == data.ALMST_Id && a.MI_Id == data.MI_Id).ToArray();

                    using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Alumni_Interaction_details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                        {
                            Value = al[0].ASMAY_Id_Left
                        });
                        cmd.Parameters.Add(new SqlParameter("@userflag",
                       SqlDbType.VarChar)
                        {
                            Value = data.userflag
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
                            data.getdetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public Alumni_School_Interactions_DTO savedetails(Alumni_School_Interactions_DTO data)
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
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                long alstinT_Id = 0;
                long alsminT_Id = 0;
                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                List<Alumni_School_Interactions_DTO> devicelist = new List<Alumni_School_Interactions_DTO>();
                //IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();




                MM = _AlumniContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "InteractionAlumni").ToList();
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
                }

                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

                int level_no = 1;

                var rolet = _AlumniContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                {
                    Alumni_School_Master_Interactions_DMO inter = new Alumni_School_Master_Interactions_DMO();
                    inter.MI_Id = data.MI_Id;
                    inter.ALSMINT_InteractionId = data.trans_id;
                    inter.ASMAY_Id = data.ASMAY_Id;
                    inter.ALSMINT_ComposedByFlg = rolet[0].IVRMRT_Role;
                    inter.ALSMINT_GroupOrIndFlg = data.ALSMINT_GroupOrIndFlg;
                    inter.ALSMINT_ComposedById = data.ALMST_Id;
                    inter.ALSMINT_Subject = data.ALSMINT_Subject;
                    inter.ALSMINT_DateTime = indianTime;
                    inter.ALSMINT_Interaction = data.ALSMINT_Interaction;
                    inter.ALSMINT_ActiveFlag = true;
                    inter.ALSMINT_CreatedBy = data.UserId;
                    inter.ALSMINT_UpdatedBy = data.UserId;
                    inter.ALSMINT_CreatedDate = indianTime;
                    inter.ALSMINT_UpdatedDate = indianTime;
                    inter.ALSMINT_Attachment = image;
                    _AlumniContext.Add(inter);

                    if (data.ALSMINT_GroupOrIndFlg == "Individual")
                    {
                        if (data.userflag == "SameBatch" || data.userflag == "AllBatch")
                        {
                            Alumni_School_Transaction_Interactions_DMO intrans = new Alumni_School_Transaction_Interactions_DMO();
                            intrans.ALSMINT_Id = inter.ALSMINT_Id;
                            intrans.ALSTINT_ToId = data.ALSTINT_ToId;
                            intrans.ALSTINT_ToFlg = "Alumni";
                            intrans.ALSTINT_ComposedById = data.ALMST_Id;
                            intrans.ALSTINT_Interaction = data.ALSMINT_Interaction;
                            intrans.ALSTINT_DateTime = indianTime;
                            intrans.ALSTINT_ComposedByFlg = rolet[0].IVRMRT_Role;
                            intrans.ALSTINT_InteractionOrder = level_no;
                            intrans.ALSTINT_ActiveFlag = true;
                            intrans.ALSTINT_CreatedBy = data.UserId;
                            intrans.ALSTINT_UpdatedBy = data.UserId;
                            intrans.ALSTINT_Attachment = image;
                            intrans.ALSTINT_CreatedDate = DateTime.Now;
                            intrans.ALSTINT_UpdatedDate = DateTime.Now;
                            _AlumniContext.Add(intrans);
                            alsminT_Id = inter.ALSMINT_Id;
                            alstinT_Id = intrans.ALSTINT_Id;
                            var contactExists = _AlumniContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }

                    }

                    else if (data.ALSMINT_GroupOrIndFlg == "Group")
                    {


                        if (data.userflag == "SameBatch" || data.userflag == "AllBatch")
                        {
                            if (data.arrayalumni.Length > 0)
                            {
                                foreach (var item in data.arrayalumni)
                                {
                                    Alumni_School_Transaction_Interactions_DMO intrans = new Alumni_School_Transaction_Interactions_DMO();
                                    intrans.ALSMINT_Id = inter.ALSMINT_Id;
                                    intrans.ALSTINT_ToId = item.ALMST_Id;
                                    intrans.ALSTINT_ToFlg = "Alumni";
                                    intrans.ALSTINT_ComposedById = data.ALMST_Id;
                                    intrans.ALSTINT_Interaction = data.ALSTINT_Interaction;
                                    intrans.ALSTINT_DateTime = indianTime;
                                    intrans.ALSTINT_ComposedByFlg = rolet[0].IVRMRT_Role;
                                    intrans.ALSTINT_InteractionOrder = level_no;
                                    intrans.ALSTINT_ActiveFlag = true;
                                    intrans.ALSTINT_CreatedBy = data.UserId;
                                    intrans.ALSTINT_UpdatedBy = data.UserId;
                                    intrans.ALSTINT_Attachment = image;
                                    intrans.ALSTINT_CreatedDate = DateTime.Now;
                                    intrans.ALSTINT_UpdatedDate = DateTime.Now;
                                    _AlumniContext.Add(intrans);
                                    alsminT_Id = inter.ALSMINT_Id;
                                    alstinT_Id = intrans.ALSTINT_Id;
                                }

                            }

                            var contactExists = _AlumniContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
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

        public Alumni_School_Interactions_DTO reply(Alumni_School_Interactions_DTO data)
        {
            try
            {



                var composeedby = _AlumniContext.Alumni_School_Master_Interactions_DMO_con.Single(q => q.ALSMINT_Id == data.ALSMINT_Id);

                var composeedto = _AlumniContext.Alumni_School_Transaction_Interactions_DMO_con.Where(q => q.ALSMINT_Id == data.ALSMINT_Id).ToList();

                data.composeedto = composeedto.FirstOrDefault().ALSTINT_ToFlg.ToLower();


                var rolet = _AlumniContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();

                data.rolename = rolet.FirstOrDefault().IVRMRT_Role.ToLower();
                long cmpid = 0;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                {
                    data.ALSMINT_ComposedByFlg = composeedby.ALSMINT_ComposedByFlg.ToLower();

                    if (data.ALSMINT_ComposedByFlg == "Alumni")
                    {
                        var trans = _AlumniContext.Alumni_School_Transaction_Interactions_DMO_con.Where(q => q.ALSMINT_Id == data.ALSMINT_Id && q.ALSTINT_ToFlg.ToLower() == "staff" && q.ALSTINT_ActiveFlag == true).ToList();
                        if (trans.Count > 0)
                        {
                            cmpid = trans.FirstOrDefault().ALSTINT_ToId;
                        }

                    }
                    else
                    {

                        cmpid = composeedby.ALSMINT_ComposedById;

                    }

                    data.typelistrole = (from a in _AlumniContext.IVRM_Role_Type
                                         from b in _AlumniContext.Staff_User_Login
                                         from d in _AlumniContext.ApplicationUserRole
                                         where b.MI_Id == data.MI_Id && b.Emp_Code == cmpid
                                         && d.UserId == b.Id && d.RoleTypeId == a.IVRMRT_Id
                                         select a).Distinct().ToArray();





                    int cnt = 0;
                    if (cnt == 0)
                    {
                        using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Alumni_Interaction_View_Reply";
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
                            cmd.Parameters.Add(new SqlParameter("@ALSMINT_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.ALSMINT_Id
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
                                data.viewmessage = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    //else
                    //{
                    //    if (composeedby.ISMINT_GroupOrIndFlg.ToLower() == "group")
                    //    {

                    //        if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                    //        {
                    //            data.HRME_Id1 = data.AMST_Id;
                    //        }
                    //        else
                    //        {
                    //            data.HRME_Id1 = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    //        }


                    //        using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    //        {
                    //            cmd.CommandText = "IVRM_Interaction_View_Reply_Group";
                    //            cmd.CommandType = CommandType.StoredProcedure;

                    //            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    //     SqlDbType.BigInt)
                    //            {
                    //                Value = data.MI_Id
                    //            });
                    //            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    //    SqlDbType.BigInt)
                    //            {
                    //                Value = data.ASMAY_Id
                    //            });
                    //            cmd.Parameters.Add(new SqlParameter("@ISMINT_Id",
                    //           SqlDbType.BigInt)
                    //            {
                    //                Value = data.ISMINT_Id
                    //            });
                    //            cmd.Parameters.Add(new SqlParameter("@STORHRMEID",
                    //          SqlDbType.BigInt)
                    //            {
                    //                Value = data.HRME_Id1
                    //            });
                    //            cmd.Parameters.Add(new SqlParameter("@SRole",
                    //          SqlDbType.Char)
                    //            {
                    //                Value = rolet.FirstOrDefault().IVRMRT_Role
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
                    //                data.viewmessage = retObject.ToArray();
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                Console.WriteLine(ex.Message);
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    //        {
                    //            cmd.CommandText = "IVRM_Interaction_View_Reply";
                    //            cmd.CommandType = CommandType.StoredProcedure;

                    //            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    //     SqlDbType.BigInt)
                    //            {
                    //                Value = data.MI_Id
                    //            });
                    //            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    //    SqlDbType.BigInt)
                    //            {
                    //                Value = data.ASMAY_Id
                    //            });
                    //            cmd.Parameters.Add(new SqlParameter("@ISMINT_Id",
                    //           SqlDbType.BigInt)
                    //            {
                    //                Value = data.ISMINT_Id
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
                    //                data.viewmessage = retObject.ToArray();
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                Console.WriteLine(ex.Message);
                    //            }
                    //        }
                    //    }
                    //}




                    //var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                    if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                    {
                        data.HRME_Id = 0;
                    }
                    else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                    {
                        data.ALMST_Id = 0;
                        data.HRME_Id = _AlumniContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    }
                    else
                    {
                        data.ALMST_Id = 0;
                        data.HRME_Id = _AlumniContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    }
                    long loginuserid = 0;
                    if (data.HRME_Id == 0)
                    {
                        loginuserid = data.ALMST_Id;
                    }
                    else if (data.ALMST_Id == 0)
                    {
                        loginuserid = data.HRME_Id;
                    }
                    using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
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
                        cmd.Parameters.Add(new SqlParameter("@ALSMINT_Id",
                      SqlDbType.BigInt)
                        {
                            Value = data.ALSMINT_Id
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
                            data.get_msgcreator = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Alumni_School_Interactions_DTO savereply(Alumni_School_Interactions_DTO data)
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

                List<Alumni_School_Interactions_DTO> deviceid = new List<Alumni_School_Interactions_DTO>();
                List<Alumni_School_Interactions_DTO> deviceiddddd = new List<Alumni_School_Interactions_DTO>();
                data.deviceids = deviceid.ToArray();
                List<Alumni_School_Interactions_DTO> devicelist = new List<Alumni_School_Interactions_DTO>();
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                List<long> device_ids = new List<long>();
                List<long> device_grp = new List<long>();

                var rolet = _AlumniContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                    composeby = data.ALMST_Id;
                    composeflag = "Alumni";
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    var empid = _AlumniContext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Distinct().ToList();
                    composeby = empid.FirstOrDefault().Emp_Code;
                    composeflag = "Staff";
                }

                var comp_id = (from a in _AlumniContext.Alumni_School_Transaction_Interactions_DMO_con
                               from b in _AlumniContext.Alumni_School_Master_Interactions_DMO_con
                               where (a.ALSMINT_Id == b.ALSMINT_Id && a.ALSMINT_Id == data.ALSMINT_Id && b.MI_Id == data.MI_Id)
                               select new Alumni_School_Interactions_DTO
                               {
                                   ALSMINT_Id = b.ALSMINT_Id,
                                   ALSTINT_Id = a.ALSTINT_Id,
                                   ALSTINT_ComposedByFlg = a.ALSTINT_ComposedByFlg,
                                   ALSTINT_ComposedById = a.ALSTINT_ComposedById,
                                   ALSTINT_ToFlg = a.ALSTINT_ToFlg,
                                   ALSTINT_ToId = a.ALSTINT_ToId,
                                   ALSMINT_GroupOrIndFlg = b.ALSMINT_GroupOrIndFlg,
                               }).Distinct().OrderBy(o => o.ALSTINT_Id).ToList();

                toId = comp_id.FirstOrDefault().ALSTINT_ToId;
                toflg = comp_id.FirstOrDefault().ALSTINT_ToFlg;
                byId = comp_id.FirstOrDefault().ALSTINT_ComposedById;
                byflg = comp_id.FirstOrDefault().ALSTINT_ComposedByFlg;
                groupOrIndFlg = comp_id.FirstOrDefault().ALSMINT_GroupOrIndFlg;

                var getuserid = (from a in _AlumniContext.Alumni_School_Transaction_Interactions_DMO_con
                                 from b in _AlumniContext.Alumni_School_Master_Interactions_DMO_con
                                 where (a.ALSMINT_Id == b.ALSMINT_Id && a.ALSMINT_Id == data.ALSMINT_Id && b.MI_Id == data.MI_Id && a.ALSTINT_InteractionOrder == 1)
                                 select new Alumni_School_Interactions_DTO
                                 {
                                     ALSMINT_Id = b.ALSMINT_Id,
                                     ALSTINT_Id = a.ALSTINT_Id,
                                     ALSTINT_ComposedByFlg = a.ALSTINT_ComposedByFlg,
                                     ALSTINT_ComposedById = a.ALSTINT_ComposedById,
                                     ALSTINT_ToFlg = a.ALSTINT_ToFlg,
                                     ALSTINT_ToId = a.ALSTINT_ToId,
                                     ALSMINT_GroupOrIndFlg = b.ALSMINT_GroupOrIndFlg,
                                 }).Distinct().OrderBy(o => o.ALSTINT_Id).ToList();

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
                            device_ids.Add(r.ALSTINT_ToId);
                        }
                    }
                    else if (composeby == byId && composeflag == byflg && toflg == "Alumni")
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
                            device_ids.Add(r.ALSTINT_ToId);
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
                            device_ids.Add(r.ALSTINT_ToId);
                        }
                    }
                    else
                    {
                        if (groupOrIndFlg == "Group")
                        {
                            sentoId = toId;
                            sentoflg = "Alumni";
                            foreach (var r in getuserid)
                            {
                                device_grp.Add(r.ALSTINT_ToId);
                            }
                        }
                        else
                        {
                            sentoId = byId;
                            sentoflg = "Alumni";
                        }
                        foreach (var r in getuserid)
                        {
                            device_ids.Add(r.ALSTINT_ComposedById);
                        }
                    }
                    if (byflg == "Staff")
                    {
                        if (toflg == "Alumni")
                        {
                            var deviceidsgg = (from a in _AlumniContext.Alumni_M_StudentDMO
                                               where (a.MI_Id == data.MI_Id && device_ids.Contains(a.ALMST_Id))
                                               select new Alumni_School_Interactions_DTO
                                               {
                                                   ALMST_Id = a.ALMST_Id,
                                                   studentName = ((a.ALMST_FirstName == null ? " " : " " + a.ALMST_FirstName) + (a.ALMST_MiddleName == null ? "  " : "  " + a.ALMST_MiddleName) + (a.ALMST_LastName == null ? "  " : "  " + a.ALMST_LastName)).Trim(),
                                                   ALMST_AppDownloadedDeviceId = a.ALMST_AppDownloadedDeviceId,
                                                   AppDownloadedDeviceId = a.ALMST_AppDownloadedDeviceId
                                               }).Distinct().ToList();

                            var deviceidGrpddd = (from a in _AlumniContext.HR_Master_Employee_DMO
                                                  where (a.MI_Id == data.MI_Id && device_grp.Contains(a.HRME_Id))
                                                  select new Alumni_School_Interactions_DTO
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

                    }
                    else if (byflg == "Alumni")
                    {
                        data.deviceids = (from a in _AlumniContext.Alumni_M_StudentDMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.ALMST_Id))
                                          select new Alumni_School_Interactions_DTO
                                          {
                                              ALMST_Id = a.ALMST_Id,
                                              studentName = ((a.ALMST_FirstName == null ? " " : a.ALMST_FirstName) + (a.ALMST_MiddleName == null ? "  " : "  " + a.ALMST_MiddleName) + (a.ALMST_LastName == null ? "  " : "  " + a.ALMST_LastName)).Trim(),
                                              ALMST_AppDownloadedDeviceId = a.ALMST_AppDownloadedDeviceId
                                          }).Distinct().ToArray();
                        devicelist = (from a in _AlumniContext.Alumni_M_StudentDMO
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.ALMST_Id))
                                      select new Alumni_School_Interactions_DTO
                                      {
                                          ALMST_Id = a.ALMST_Id,
                                          studentName = ((a.ALMST_FirstName == null ? " " : a.ALMST_FirstName) + (a.ALMST_MiddleName == null ? "  " : "  " + a.ALMST_MiddleName) + (a.ALMST_LastName == null ? "  " : "  " + a.ALMST_LastName)).Trim(),
                                          AppDownloadedDeviceId = a.ALMST_AppDownloadedDeviceId
                                      }).Distinct().ToList();

                        var d2 = (from a in _AlumniContext.Alumni_M_StudentDMO
                                  where (a.MI_Id == data.MI_Id && device_ids.Contains(a.ALMST_Id))
                                  select new Alumni_School_Interactions_DTO
                                  {
                                      ALMST_MobileNo = a.ALMST_MobileNo,
                                      ALMST_Id = a.ALMST_Id,
                                      studentName = ((a.ALMST_FirstName == null ? " " : a.ALMST_FirstName) + (a.ALMST_MiddleName == null ? "  " : "  " + a.ALMST_MiddleName) + (a.ALMST_LastName == null ? "  " : "  " + a.ALMST_LastName)).Trim(),
                                      AppDownloadedDeviceId = a.ALMST_AppDownloadedDeviceId
                                  }).Distinct().ToList();
                        data.devicelist12 = d2;
                    }
                }
                else if (composeflag == "Alumni")
                {
                    if (composeby == byId && composeflag == byflg && toflg == "Staff")
                    {
                        if (groupOrIndFlg == "Group" && byflg == "Alumni")
                        {
                            sentoId = byId;
                            sentoflg = "Alumni";
                            foreach (var r in getuserid)
                            {
                                device_grp.Add(r.ALSTINT_ToId);
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
                            device_ids.Add(r.ALSTINT_ToId);
                        }
                    }
                    else
                    {
                        if (groupOrIndFlg == "Group" && byflg == "Alumni")
                        {
                            sentoId = byId;
                            sentoflg = "Alumni";

                        }
                        else if (groupOrIndFlg == "Group" && byflg == "Staff")
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                            foreach (var r in getuserid)
                            {
                                device_grp.Add(r.ALSTINT_ComposedById);
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
                                device_ids.Add(r.ALSTINT_ToId);
                            }
                        }
                        else
                        {
                            foreach (var r in getuserid)
                            {
                                device_ids.Add(r.ALSTINT_ComposedById);
                            }
                        }
                    }

                    if (groupOrIndFlg == "Group" && byflg == "Alumni")
                    {
                        data.deviceids = (from a in _AlumniContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new Alumni_School_Interactions_DTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                          }).Distinct().ToArray();

                        devicelist = (from a in _AlumniContext.HR_Master_Employee_DMO
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                      select new Alumni_School_Interactions_DTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                      }).Distinct().ToList();
                        var d3 = (from a in _AlumniContext.HR_Master_Employee_DMO
                                  where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                  select new Alumni_School_Interactions_DTO
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
                        var deviceidsgg = (from a in _AlumniContext.Alumni_M_StudentDMO
                                           where (a.MI_Id == data.MI_Id && device_ids.Contains(a.ALMST_Id))
                                           select new Alumni_School_Interactions_DTO
                                           {
                                               ALMST_Id = a.ALMST_Id,
                                               studentName = ((a.ALMST_FirstName == null ? " " : a.ALMST_FirstName) + (a.ALMST_MiddleName == null ? "  " : "  " + a.ALMST_MiddleName) + (a.ALMST_LastName == null ? "  " : "  " + a.ALMST_LastName)).Trim(),
                                               ALMST_AppDownloadedDeviceId = a.ALMST_AppDownloadedDeviceId,
                                               AppDownloadedDeviceId = a.ALMST_AppDownloadedDeviceId
                                           }).Distinct().ToList();

                        var deviceidGrpddd = (from a in _AlumniContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && device_grp.Contains(a.HRME_Id))
                                              select new Alumni_School_Interactions_DTO
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
                        data.deviceids = (from a in _AlumniContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new Alumni_School_Interactions_DTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                          }).Distinct().ToArray();

                        devicelist = (from a in _AlumniContext.HR_Master_Employee_DMO
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                      select new Alumni_School_Interactions_DTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                      }).Distinct().ToList();
                        var d4 = (from a in _AlumniContext.HR_Master_Employee_DMO
                                  where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                  select new Alumni_School_Interactions_DTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                      AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                  }).Distinct().ToList();
                        data.devicelist12 = d4;
                    }

                }


                var orderno = (from a in _AlumniContext.Alumni_School_Transaction_Interactions_DMO_con
                               from b in _AlumniContext.Alumni_School_Master_Interactions_DMO_con
                               where (a.ALSMINT_Id == b.ALSMINT_Id && b.MI_Id == data.MI_Id && a.ALSMINT_Id == data.ALSMINT_Id)
                               select new Alumni_School_Interactions_DTO
                               {
                                   ALSTINT_InteractionOrder = a.ALSTINT_InteractionOrder
                               }).Distinct().ToList();

                level_no = orderno.LastOrDefault().ALSTINT_InteractionOrder;
                if (level_no <= 0)
                {

                    level_order = 1;
                }
                else
                {
                    level_order = level_no + 1;
                }

                Alumni_School_Transaction_Interactions_DMO intrans = new Alumni_School_Transaction_Interactions_DMO();
                intrans.ALSMINT_Id = data.ALSMINT_Id;
                intrans.ALSTINT_ToId = sentoId;
                intrans.ALSTINT_ToFlg = sentoflg;
                intrans.ALSTINT_ComposedById = composeby;
                intrans.ALSTINT_Interaction = data.ALSTINT_Interaction;
                intrans.ALSTINT_DateTime = indianTime;
                intrans.ALSTINT_ComposedByFlg = composeflag;
                intrans.ALSTINT_InteractionOrder = level_order;
                intrans.ALSTINT_ActiveFlag = true;
                intrans.ALSTINT_CreatedBy = data.UserId;
                intrans.ALSTINT_UpdatedBy = data.UserId;
                intrans.ALSTINT_CreatedDate = indianTime;
                intrans.ALSTINT_UpdatedDate = indianTime;
                intrans.ALSTINT_Attachment = image;
                _AlumniContext.Add(intrans);

                var contactExists = _AlumniContext.SaveChanges();

                long ISTINT_Id3 = 0;
                var ISTINT_Id1 = _AlumniContext.Alumni_School_Transaction_Interactions_DMO_con.OrderByDescending(a => a.ALSTINT_Id).ToList();
                var ISTINT_Id2 = ISTINT_Id1.FirstOrDefault().ALSTINT_Id;
                ISTINT_Id3 = ISTINT_Id2;

                if (contactExists > 0)
                {
                    data.returnval = true;
                    //if (groupOrIndFlg == "Individual" && byflg == "Staff")
                    //{
                    //    var employeedata = _AlumniContext.HR_Master_Employee_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == intrans.ISTINT_ComposedById).Distinct().ToList();
                    //    notiSubject = employeedata.FirstOrDefault().HRME_EmployeeFirstName + ' ' + employeedata.FirstOrDefault().HRME_EmployeeMiddleName + ' ' + employeedata.FirstOrDefault().HRME_EmployeeLastName;

                    //}

                    if (groupOrIndFlg == "Individual" && byflg == "Alumni")
                    {
                        var studentdata = _AlumniContext.Alumni_M_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.ALMST_Id == intrans.ALSTINT_ComposedById).Distinct().ToList();
                        notiSubject = studentdata.FirstOrDefault().ALMST_FirstName + ' ' + studentdata.FirstOrDefault().ALMST_MiddleName + ' ' + studentdata.FirstOrDefault().ALMST_LastName;
                    }
                    else if (groupOrIndFlg == "Group")
                    {
                        notiSubject = "Group Message";
                    }

                    //============================== Notification
                    var deviceidsnew = "";
                    var devicenew = "";

                    if (devicelist.Count > 0)
                    {
                        int k = 0;
                        foreach (var device_id in devicelist)
                        {
                            if (k == 0)
                            {
                                deviceidsnew = '"' + device_id.ALMST_AppDownloadedDeviceId + '"';
                                k = 1;
                            }
                            else
                            {
                                deviceidsnew = deviceidsnew + "," + '"' + device_id.AppDownloadedDeviceId + '"';
                            }
                        }
                        devicenew = "[" + deviceidsnew + "]";

                       // callnotification(devicenew, notiSubject, intrans.ALSTINT_Id, data.MI_Id, ISTINT_Id3, data);
                    }
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

        //public string callnotification(string devicenew, string notiSubject, long istint_Id, long mi_id, long ISTINT_Id3, Alumni_School_Interactions_DTO dto)
        //{
        //    try
        //    {
        //        var key = _AlumniContext.MobileApplAuthenticationDMO.Single(a => a.MI_Id == mi_id).MAAN_AuthenticationKey;
        //        string url = "";
        //        string utrrno = "";
        //        url = "https://fcm.googleapis.com/fcm/send";

        //        List<string> notificationparams = new List<string>();
        //        string daata = "";



        //        string sound = "default";
        //        string notId = "1";



        //        daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
        //     "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Reply" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + notiSubject + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

        //        notificationparams.Add(daata.ToString());

        //        // var mycontent = JsonConvert.SerializeObject(notificationparams);
        //        var mycontent = notificationparams[0];
        //        string postdata = mycontent.ToString();
        //        HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
        //        connection.ContentType = "application/json";
        //        connection.MediaType = "application/json";
        //        connection.Accept = "application/json";

        //        connection.Method = "post";
        //        connection.Headers["authorization"] = "key=" + key;

        //        using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
        //        {
        //            requestwriter.Write(postdata);
        //        }
        //        string responsedata = string.Empty;

        //        using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
        //        {
        //            responsedata = responsereader.ReadToEnd();
        //            JObject joresponse1 = JObject.Parse(responsedata);
        //        }
        //        PushNotification push_noti = new PushNotification(_AlumniContext);

        //        push_noti.Insert_PushNotification_alumni_interaction_replay(notiSubject, ISTINT_Id3, mi_id, dto);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return ex.Message;
        //    }
        //    return "success";

        //}

    }
}
