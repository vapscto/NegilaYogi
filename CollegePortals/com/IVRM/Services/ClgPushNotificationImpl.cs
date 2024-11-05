using PreadmissionDTOs.com.vaps.College.Portals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;
using DomainModel.Model.com.vapstech.College.Portals.IVRM;
using DomainModel.Model.com.vapstech.Portals.Employee;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs;

namespace CollegePortals.com.Student.Services
{
    public class ClgPushNotificationImpl : Interfaces.ClgPushNotificationInterface
    {
        private static ConcurrentDictionary<string, ClgPushNotificationDTO> _login =
           new ConcurrentDictionary<string, ClgPushNotificationDTO>();
        public CollegeportalContext _ClgPortalContext;
        public DomainModelMsSqlServerContext _db;

        public ClgPushNotificationImpl(CollegeportalContext ClgPortalContext, DomainModelMsSqlServerContext db)
        {
            _ClgPortalContext = ClgPortalContext;
            _db = db;
        }

        public async Task<ClgPushNotificationDTO> getloaddata(ClgPushNotificationDTO data)
        {
            try
            {
                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _ClgPortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _ClgPortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var role = rolet.FirstOrDefault().IVRMRT_Role;

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = _ClgPortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    data.AMCST_Id = 0;
                }

                using (var cmd1 = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "CLG_PORTAL_PN_STUDENT_STAFFLIST";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
               SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@AMCST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@HRME_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@roleflag",
              SqlDbType.VarChar)
                    {
                        Value = role
                    });


                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd1.ExecuteReaderAsync())
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
                        data.namelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                data.notificationlist = _ClgPortalContext.IVRM_PushNotificationDMO.Where(n => n.MI_Id == data.MI_Id).Distinct().OrderBy(i => i.IPN_Id).ToArray();
                //data.namelist = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                //                        from b in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                //                        from c in _ClgPortalContext.academicYearDMO

                //                        where (a.MI_Id == c.MI_Id && a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id &&
                //                         a.AMCST_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                //                        select new ClgPushNotificationDTO
                //                        {
                //                            AMCST_Id = a.AMCST_Id,
                //                            AMCST_FirstName = ((a.AMCST_FirstName == null ? "" : a.AMCST_FirstName.ToUpper()) + " " + (a.AMCST_MiddleName == null ? "" : a.AMCST_MiddleName.ToUpper()) + " " + (a.AMCST_LastName == null ? "" : a.AMCST_LastName.ToUpper()) + ':' + a.AMCST_AdmNo).Trim(),
                //                        }
                //        ).Distinct().OrderBy(c => c.AMCST_FirstName).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClgPushNotificationDTO savedata(ClgPushNotificationDTO data)
        {
            try
            {
                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _ClgPortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _ClgPortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();

                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                MM = _db.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "PushNotification").ToList();
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
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }


                IVRM_PushNotificationDMO obj = new IVRM_PushNotificationDMO();
                obj.MI_Id = data.MI_Id;
                obj.IPN_No = data.trans_id;
                obj.IPN_StuStaffFlg = rolet.FirstOrDefault().IVRMRT_Role;
                obj.ASMAY_Id = data.ASMAY_Id;
                obj.IPN_Date = data.IPN_Date;
                obj.IPN_PushNotification = data.IPN_PushNotification;
                obj.IVRMUL_Id = data.UserId;
                obj.IPN_ActiveFlag = true;
                obj.UpdatedDate = DateTime.Now;
                obj.CreatedDate = DateTime.Now;
                _ClgPortalContext.Add(obj);
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var s in data.studentarray)
                    {
                        IVRM_College_PN_StudentDMO clg = new IVRM_College_PN_StudentDMO();
                        clg.IPN_Id = obj.IPN_Id;
                        clg.AMCST_Id = s.AMCST_Id;
                        clg.ICPNS_ActiveFlag = true;
                        clg.CreatedDate = DateTime.Now;
                        clg.UpdatedDate = DateTime.Now;
                        _ClgPortalContext.Add(clg);
                    }
                }

                int returnval = _ClgPortalContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
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

        public ClgPushNotificationDTO getNotificationdetails(ClgPushNotificationDTO data)
        {
            try
            {
                data.notificationdetails = (from a in _ClgPortalContext.IVRM_PushNotificationDMO
                                            from b in _ClgPortalContext.IVRM_College_PN_StudentDMO
                                            from c in _ClgPortalContext.Adm_Master_College_StudentDMO
                                            where (a.IPN_Id == b.IPN_Id && b.AMCST_Id == c.AMCST_Id && a.MI_Id == data.MI_Id && a.IPN_Id == data.IPN_Id)
                                            select new ClgPushNotificationDTO
                                            {
                                                IPN_Id = a.IPN_Id,
                                                IPN_No=a.IPN_No,
                                                AMCST_Id = c.AMCST_Id,
                                                AMCST_FirstName = ((c.AMCST_FirstName == null ? "" : c.AMCST_FirstName.ToUpper()) + " " + (c.AMCST_MiddleName == null ? "" : c.AMCST_MiddleName.ToUpper()) + " " + (c.AMCST_LastName == null ? "" : c.AMCST_LastName.ToUpper()) + ':' + c.AMCST_AdmNo).Trim(),
                                                ICPNS_ActiveFlag = b.ICPNS_ActiveFlag
                                            }
                            ).Distinct().OrderBy(c => c.AMCST_FirstName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }




    }
}
