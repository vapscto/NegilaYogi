using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Portals.IVRM;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CollegePortals.com.IVRM.Services
{
    public class Clg_IVRM_InteractionsImpl:Interfaces.Clg_IVRM_InteractionsInterface
    {

        private static ConcurrentDictionary<string, IVRM_School_InteractionsDTO> _login =
          new ConcurrentDictionary<string, IVRM_School_InteractionsDTO>();
        private CollegeportalContext _PortalContext;
        public DomainModelMsSqlServerContext _context;

        public Clg_IVRM_InteractionsImpl(CollegeportalContext PortalContext, DomainModelMsSqlServerContext context)
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
                    data.userhrmE_Id = data.AMCST_Id;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMCST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    data.userhrmE_Id = data.HRME_Id;
                }
                else
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Clg_IVRM_Interaction_Inbox";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",SqlDbType.BigInt){Value = data.AMCST_Id});
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",SqlDbType.BigInt){Value = data.HRME_Id});
                    cmd.Parameters.Add(new SqlParameter("@roleflg",SqlDbType.VarChar){Value = rolet.FirstOrDefault().IVRMRT_Role});

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
                    cmd.CommandText = "Clg_IVRM_Interaction_ReadCount";
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
                        Value = data.AMCST_Id
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

                data.configflag = _PortalContext.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id).Distinct().ToArray();
                if (data.roleflg.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    var clsid = (from a in _PortalContext.Adm_Master_College_StudentDMO
                                 from b in _PortalContext.MasterCourseDMO
                                 from c in _PortalContext.ClgMasterBranchDMO
                                 from f in _PortalContext.CLG_Adm_Master_SemesterDMO
                                 from g in _PortalContext.Adm_College_Master_SectionDMO
                                 from d in _PortalContext.Adm_College_Yearly_StudentDMO
                                 from e in _PortalContext.AcademicYear
                                 where (a.AMCST_Id == d.AMCST_Id && b.AMCO_Id == d.AMCO_Id && c.AMB_Id == d.AMB_Id && f.AMSE_Id == d.AMSE_Id && g.ACMS_Id==d.ACMS_Id && d.ASMAY_Id == e.ASMAY_Id && a.MI_Id == b.MI_Id && c.MI_Id == e.MI_Id && a.AMCST_Id == data.AMCST_Id && d.ASMAY_Id == data.ASMAY_Id)
                                 select new IVRM_School_InteractionsDTO
                                 {
                                     AMCO_Id = b.AMCO_Id,
                                     AMB_Id = c.AMB_Id,
                                     AMSE_Id = f.AMSE_Id,
                                     ACMS_Id = g.ACMS_Id,
                                     AMB_BranchName = c.AMB_BranchName,
                                     AMCO_CourseName = b.AMCO_CourseName,
                                     AMSE_SEMName = f.AMSE_SEMName,
                                     ACMS_SectionName = g.ACMS_SectionName,
                                 }).Distinct().ToArray();
                    data.amcoid = clsid.FirstOrDefault().AMCO_Id;
                    data.ambid = clsid.FirstOrDefault().AMB_Id;
                    data.amseid = clsid.FirstOrDefault().AMSE_Id;
                    data.acmsid = clsid.FirstOrDefault().ACMS_Id;
                }
                else if (data.roleflg.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_filter_CLG";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",SqlDbType.BigInt){Value = data.HRME_Id});
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",SqlDbType.BigInt){Value = data.amcoid});
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",SqlDbType.BigInt){Value = data.ambid});
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",SqlDbType.BigInt){Value = data.amseid});
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",SqlDbType.BigInt){Value = data.acmsid});
                    cmd.Parameters.Add(new SqlParameter("@IINTS_Flag",SqlDbType.VarChar){Value = data.userflag});
                    cmd.Parameters.Add(new SqlParameter("@roletype",SqlDbType.VarChar){Value = data.roleflg});
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
                                    dataRow.Add(dataReader.GetName(iFiled),dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
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
                    cmd.CommandText = "Clg_IVRM_Interaction_StudentList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",SqlDbType.VarChar){Value = data.AMCO_Id});
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",SqlDbType.VarChar){Value = data.AMB_Id});
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",SqlDbType.VarChar){Value = data.AMSE_Id});
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",SqlDbType.VarChar){Value = data.ACMS_Id});
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
                                    dataRow.Add(dataReader.GetName(iFiled),dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
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
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_School_InteractionsDTO Getbranch(IVRM_School_InteractionsDTO data)
        {
            try
            {
                data.branchList = (from a in _PortalContext.ClgMasterBranchDMO
                                   from b in _PortalContext.CLG_Adm_College_AY_CourseDMO
                                   from c in _PortalContext.CLG_Adm_College_AY_Course_BranchDMO
                                   where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                   select new IVRM_School_InteractionsDTO
                                   {
                                       AMB_Id = a.AMB_Id,
                                       AMB_BranchName = a.AMB_BranchName,
                                       AMB_BranchCode = a.AMB_BranchCode,
                                       AMB_Order = a.AMB_Order,
                                   }).Distinct().OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public IVRM_School_InteractionsDTO Getsemester(IVRM_School_InteractionsDTO data)
        {
            try
            {
                data.semesterList = (from a in _PortalContext.CLG_Adm_Master_SemesterDMO
                                     from b in _PortalContext.CLG_Adm_College_AY_CourseDMO
                                     from c in _PortalContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _PortalContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                     select new IVRM_School_InteractionsDTO
                                     {
                                         AMSE_Id = a.AMSE_Id,
                                         AMSE_SEMName = a.AMSE_SEMName,
                                         AMSE_SEMCode = a.AMSE_SEMCode,
                                         AMSE_SEMOrder = a.AMSE_SEMOrder,
                                     }).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public IVRM_School_InteractionsDTO Getsection(IVRM_School_InteractionsDTO data)
        {
            try
            {
                data.sectionList = (from a in _PortalContext.Adm_College_Yearly_StudentDMO
                                    from b in _PortalContext.Adm_College_Master_SectionDMO
                                    where a.ASMAY_Id == data.ASMAY_Id && b.ACMS_Id == a.ACMS_Id && b.MI_Id == data.MI_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == data.AMCO_Id && a.AMSE_Id == data.AMSE_Id
                                    select new IVRM_School_InteractionsDTO
                                    {
                                        ACMS_Id = b.ACMS_Id,
                                        ACMS_SectionName = b.ACMS_SectionName,
                                        ACMS_Order = b.ACMS_Order
                                    }).Distinct().OrderBy(t => t.ACMS_Order).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public IVRM_School_InteractionsDTO savedetails(IVRM_School_InteractionsDTO data)
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
                }

                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

                int level_no = 1;
                //var level = _PortalContext.IVRM_School_Master_InteractionsDMO.ToList();
                //var max_level = level.Count();
                //if (max_level <= 0)
                //{
                //    level_no = 1;
                //}
                //else
                //{
                //    level_no = max_level + 1;
                //}

                if (data.roleflg.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                    inter.MI_Id = data.MI_Id;
                    inter.ISMINT_InteractionId = data.trans_id;
                    inter.ASMAY_Id = data.ASMAY_Id;
                    inter.ISMINT_ComposedByFlg = data.roleflg;
                    inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                    inter.ISMINT_ComposedById = data.AMCST_Id;
                    inter.ISMINT_Subject = data.ISMINT_Subject;
                    inter.ISMINT_DateTime = indianTime;
                    inter.ISMINT_Interaction = data.ISMINT_Interaction;
                    inter.ISMINT_ActiveFlag = true;
                    inter.ISMINT_CreatedBy = data.UserId;
                    inter.ISMINT_UpdatedBy = data.UserId;
                    inter.CreatedDate = indianTime;
                    inter.UpdatedDate = indianTime;
                    inter.ISMINT_Attachment = image;
                    _PortalContext.Add(inter);

                    //IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                    data.deviceids = (from a in _PortalContext.MasterEmployee
                                      where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                      select new IVRM_School_InteractionsDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          HRME_MobileNo = a.HRME_MobileNo,
                                          HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                      }).Distinct().ToArray();

                    var devlist1 = (from a in _PortalContext.MasterEmployee
                                    where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                    select new IVRM_School_InteractionsDTO
                                    {
                                        HRME_Id = a.HRME_Id,
                                        HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                        employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                    }).Distinct().ToList();

                    IVRM_School_InteractionsDTO dto = new IVRM_School_InteractionsDTO();
                    data.devicelist12 = devlist1;

                    devicelist = (from a in _PortalContext.MasterEmployee
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
                    intrans.ISTINT_ComposedById = data.AMCST_Id;
                    intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                    intrans.ISTINT_DateTime = indianTime;
                    intrans.ISTINT_ComposedByFlg = data.roleflg;
                    intrans.ISTINT_InteractionOrder = level_no;
                    intrans.ISTINT_ActiveFlag = true;
                    intrans.ISTINT_CreatedBy = data.UserId;
                    intrans.ISTINT_UpdatedBy = data.UserId;
                    intrans.CreatedDate = DateTime.Now;
                    intrans.UpdatedDate = DateTime.Now;
                    intrans.ISTINT_ReadFlg = false;
                    intrans.ISTINT_Attachment = image;
                    _PortalContext.Add(intrans);
                    ismint_Id = inter.ISMINT_Id;
                    istint_Id = intrans.ISTINT_Id;
                    var contactExists = _PortalContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        //============================== Notification
                        var deviceidsnew = "";
                        var devicenew = "";
                        //ismint_Id = intrans.ISMINT_Id;
                        //istint_Id = intrans.ISTINT_Id;
                        if (devicelist.Count > 0)
                        {
                            int k = 0;
                            foreach (var deviceid in devicelist)
                            {
                                if (k == 0)
                                {
                                    deviceidsnew = '"' + deviceid.AppDownloadedDeviceId + '"';
                                    k = 1;
                                }
                                else
                                {
                                    deviceidsnew = deviceidsnew + "," + '"' + deviceid.AppDownloadedDeviceId + '"';
                                }
                            }
                            devicenew = "[" + deviceidsnew + "]";

                            callnotificationNew(devicenew, data.ISMINT_Subject, istint_Id, data.MI_Id, dto);
                        }
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
                        _PortalContext.Add(inter);

                        if (data.userflag == "Student")
                        {
                            List<long> device_ids = new List<long>();
                            foreach (var s in data.arrayStudent1)
                            {
                                device_ids.Add(s.AMCST_Id);

                                IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();

                                intrans.ISMINT_Id = inter.ISMINT_Id;
                                intrans.ISTINT_ToId = s.AMCST_Id;
                                intrans.ISTINT_ToFlg = "Student";
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
                                intrans.ISTINT_ReadFlg = false;
                                intrans.ISTINT_Attachment = image;
                                _PortalContext.Add(intrans);
                                istint_Id = intrans.ISTINT_Id;
                            }
                            ismint_Id = inter.ISMINT_Id;
                            data.deviceids = (from a in _PortalContext.Adm_Master_College_StudentDMO
                                              where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMCST_Id))
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  AMCST_MobileNo = a.AMCST_MobileNo,
                                                  AMCST_Id = a.AMCST_Id,
                                                  AMCST_AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId,
                                                  studentName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + (a.AMCST_MiddleName == null ? "  " : "  " + a.AMCST_MiddleName) + (a.AMCST_LastName == null ? "  " : "  " + a.AMCST_LastName)).Trim(),
                                              }).Distinct().ToArray();
                            var devi = (from a in _PortalContext.Adm_Master_College_StudentDMO
                                        where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMCST_Id))
                                        select new IVRM_School_InteractionsDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId,
                                            studentName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + (a.AMCST_MiddleName == null ? "  " : "  " + a.AMCST_MiddleName) + (a.AMCST_LastName == null ? "  " : "  " + a.AMCST_LastName)).Trim(),
                                        }).Distinct().ToList();
                            data.devicelist12 = devi;

                            devicelist = (from a in _PortalContext.Adm_Master_College_StudentDMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMCST_Id))
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              AMCST_Id = a.AMCST_Id,
                                              AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId,
                                              studentName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + (a.AMCST_MiddleName == null ? "  " : "  " + a.AMCST_MiddleName) + (a.AMCST_LastName == null ? "  " : "  " + a.AMCST_LastName)).Trim(),
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
                                intrans.ISTINT_ReadFlg = false;
                                _PortalContext.Add(intrans);
                                istint_Id = intrans.ISTINT_Id;
                            }
                            ismint_Id = inter.ISMINT_Id;
                            data.deviceids = (from a in _PortalContext.MasterEmployee
                                              where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              }).Distinct().ToArray();

                            var devlist = (from a in _PortalContext.MasterEmployee
                                           where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                           select new IVRM_School_InteractionsDTO
                                           {
                                               HRME_MobileNo = a.HRME_MobileNo,
                                               HRME_Id = a.HRME_Id,
                                               HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                               employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                           }).Distinct().ToList();
                            data.devicelist12 = devlist;

                            devicelist = (from a in _PortalContext.MasterEmployee
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
                            data.deviceids = (from a in _PortalContext.MasterEmployee
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              }).Distinct().ToArray();

                            var devlist1 = (from a in _PortalContext.MasterEmployee
                                            where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                            select new IVRM_School_InteractionsDTO
                                            {
                                                HRME_Id = a.HRME_Id,
                                                HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                            }).Distinct().ToList();
                            data.devicelist12 = devlist1;


                            devicelist = (from a in _PortalContext.MasterEmployee
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
                            intrans.ISTINT_ReadFlg = false;
                            _PortalContext.Add(intrans);
                            ismint_Id = inter.ISMINT_Id;
                            istint_Id = intrans.ISTINT_Id;
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
                            _PortalContext.Add(inter);

                            data.deviceids = (from a in _PortalContext.Adm_Master_College_StudentDMO
                                              where (a.MI_Id == data.MI_Id && a.AMCST_Id == data.student_Id)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  AMCST_Id = a.AMCST_Id,
                                                  AMCST_AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId,
                                                  studentName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + (a.AMCST_MiddleName == null ? "  " : "  " + a.AMCST_MiddleName) + (a.AMCST_LastName == null ? "  " : "  " + a.AMCST_LastName)).Trim(),
                                              }).Distinct().ToArray();

                            var slist = (from a in _PortalContext.Adm_Master_College_StudentDMO
                                         where (a.MI_Id == data.MI_Id && a.AMCST_Id == data.student_Id)
                                         select new IVRM_School_InteractionsDTO
                                         {
                                             AMCST_MobileNo = a.AMCST_MobileNo,
                                             AMCST_Id = a.AMCST_Id,
                                             AMCST_AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId,
                                             studentName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + (a.AMCST_MiddleName == null ? "  " : "  " + a.AMCST_MiddleName) + (a.AMCST_LastName == null ? "  " : "  " + a.AMCST_LastName)).Trim(),
                                         }).Distinct().ToList();
                            data.devicelist12 = slist;

                            devicelist = (from a in _PortalContext.Adm_Master_College_StudentDMO
                                          where (a.MI_Id == data.MI_Id && a.AMCST_Id == data.student_Id)
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              AMCST_Id = a.AMCST_Id,
                                              AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId,
                                              studentName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + (a.AMCST_MiddleName == null ? "  " : "  " + a.AMCST_MiddleName) + (a.AMCST_LastName == null ? "  " : "  " + a.AMCST_LastName)).Trim(),
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
                            intrans.ISTINT_ReadFlg = false;
                            _PortalContext.Add(intrans);
                            ismint_Id = inter.ISMINT_Id;
                            istint_Id = intrans.ISTINT_Id;
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
                            inter.ISMINT_Attachment = image;
                            _PortalContext.Add(inter);

                            data.deviceids = (from a in _PortalContext.MasterEmployee
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.employee_Id)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              }).Distinct().ToArray();
                            var sss = (from a in _PortalContext.MasterEmployee
                                       where (a.MI_Id == data.MI_Id && a.HRME_Id == data.employee_Id)
                                       select new IVRM_School_InteractionsDTO
                                       {
                                           HRME_MobileNo = a.HRME_MobileNo,
                                           HRME_Id = a.HRME_Id,
                                           HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                           employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                       }).Distinct().ToList();
                            data.devicelist12 = sss;
                            devicelist = (from a in _PortalContext.MasterEmployee
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
                            intrans.ISTINT_ReadFlg = false;
                            _PortalContext.Add(intrans);
                            ismint_Id = inter.ISMINT_Id;
                            istint_Id = intrans.ISTINT_Id;
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
                            _PortalContext.Add(inter);
                            data.deviceids = (from a in _PortalContext.MasterEmployee
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              }).Distinct().ToArray();

                            var devlist1 = (from a in _PortalContext.MasterEmployee
                                            where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                            select new IVRM_School_InteractionsDTO
                                            {
                                                HRME_MobileNo = a.HRME_MobileNo,
                                                HRME_Id = a.HRME_Id,
                                                HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                            }).Distinct().ToList();
                            data.devicelist12 = devlist1;

                            devicelist = (from a in _PortalContext.MasterEmployee
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
                            intrans.ISTINT_ReadFlg = false;
                            _PortalContext.Add(intrans);
                            ismint_Id = inter.ISMINT_Id;
                            istint_Id = intrans.ISTINT_Id;
                        }
                    }
                    var contactExists = _PortalContext.SaveChanges();



                    long istint_Id3 = 0;
                    var istint_Id1 = _PortalContext.IVRM_School_Master_InteractionsDMO.OrderByDescending(a => a.ISMINT_Id).ToList();
                    var istint_Id2 = istint_Id1.FirstOrDefault().ISMINT_Id;
                    istint_Id3 = istint_Id2;
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        //============================== Notification
                        var deviceidsnew = "";
                        var devicenew = "";
                        // ismint_Id = intrans.ISMINT_Id;
                        //istint_Id = intrans.ISTINT_Id;                        
                        if (devicelist.Count > 0)
                        {
                            int k = 0;
                            foreach (var deviceid in devicelist)
                            {
                                if (k == 0)
                                {
                                    deviceidsnew = '"' + deviceid.AppDownloadedDeviceId + '"';
                                    k = 1;
                                }
                                else
                                {
                                    deviceidsnew = deviceidsnew + "," + '"' + deviceid.AppDownloadedDeviceId + '"';
                                }
                            }
                            devicenew = "[" + deviceidsnew + "]";
                            callnotificationNew(devicenew, data.ISMINT_Subject, istint_Id3, data.MI_Id, data);
                        }
                    }
                    else
                    {
                        data.returnval = false;
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
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Clg_IVRM_Interaction_View_Reply";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ISMINT_Id",SqlDbType.BigInt){Value = data.ISMINT_Id});
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


                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
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
                    loginuserid = data.AMCST_Id;
                }
                else if (data.AMCST_Id == 0)
                {
                    loginuserid = data.HRME_Id;
                }
                var rmv = 0;
                var result = _context.IVRM_School_Transaction_InteractionsDMO.Single(a => a.ISMINT_Id == data.ISMINT_Id);
                if (result != null)
                {
                    result.ISTINT_ReadFlg = true;
                    result.UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    rmv = _context.SaveChanges();
                }
                if (rmv > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Clg_IVRM_Interaction_CreatedBy";
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
                    composeby = data.AMCST_Id;
                    composeflag = "Student";
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
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

                var getuserid = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                                 from b in _PortalContext.IVRM_School_Master_InteractionsDMO
                                 where (a.ISMINT_Id == b.ISMINT_Id && a.ISMINT_Id == data.ISMINT_Id && b.MI_Id == data.MI_Id && a.ISTINT_InteractionOrder == 1)
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
                            var deviceidsgg = (from a in _PortalContext.Adm_Master_College_StudentDMO
                                               where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMCST_Id))
                                               select new IVRM_School_InteractionsDTO
                                               {
                                                   AMST_Id = a.AMCST_Id,
                                                   studentName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + (a.AMCST_MiddleName == null ? "  " : "  " + a.AMCST_MiddleName) + (a.AMCST_LastName == null ? "  " : "  " + a.AMCST_LastName)).Trim(),
                                                   AMST_AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId,
                                                   AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId
                                               }).Distinct().ToList();

                            var deviceidGrpddd = (from a in _PortalContext.MasterEmployee
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
                            data.deviceids = (from a in _PortalContext.MasterEmployee
                                              where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                              select new IVRM_School_InteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              }).Distinct().ToArray();
                            devicelist = (from a in _PortalContext.MasterEmployee
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                          }).Distinct().ToList();

                            var d1 = (from a in _PortalContext.MasterEmployee
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
                        data.deviceids = (from a in _PortalContext.Adm_Master_College_StudentDMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMCST_Id))
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              AMST_Id = a.AMCST_Id,
                                              studentName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + (a.AMCST_MiddleName == null ? "  " : "  " + a.AMCST_MiddleName) + (a.AMCST_LastName == null ? "  " : "  " + a.AMCST_LastName)).Trim(),
                                              AMST_AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId
                                          }).Distinct().ToArray();
                        devicelist = (from a in _PortalContext.Adm_Master_College_StudentDMO
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMCST_Id))
                                      select new IVRM_School_InteractionsDTO
                                      {
                                          AMST_Id = a.AMCST_Id,
                                          studentName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + (a.AMCST_MiddleName == null ? "  " : "  " + a.AMCST_MiddleName) + (a.AMCST_LastName == null ? "  " : "  " + a.AMCST_LastName)).Trim(),
                                          AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId
                                      }).Distinct().ToList();

                        var d2 = (from a in _PortalContext.Adm_Master_College_StudentDMO
                                  where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMCST_Id))
                                  select new IVRM_School_InteractionsDTO
                                  {
                                      HRME_MobileNo = a.AMCST_MobileNo,
                                      AMCST_Id = a.AMCST_Id,
                                      studentName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + (a.AMCST_MiddleName == null ? "  " : "  " + a.AMCST_MiddleName) + (a.AMCST_LastName == null ? "  " : "  " + a.AMCST_LastName)).Trim(),
                                      AppDownloadedDeviceId = a.AMCST_AppDownloadedDeviceId
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
                        data.deviceids = (from a in _PortalContext.MasterEmployee
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                          }).Distinct().ToArray();

                        devicelist = (from a in _PortalContext.MasterEmployee
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                      select new IVRM_School_InteractionsDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                      }).Distinct().ToList();
                        var d3 = (from a in _PortalContext.MasterEmployee
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

                        var deviceidGrpddd = (from a in _PortalContext.MasterEmployee
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
                        data.deviceids = (from a in _PortalContext.MasterEmployee
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new IVRM_School_InteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                          }).Distinct().ToArray();

                        devicelist = (from a in _PortalContext.MasterEmployee
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                      select new IVRM_School_InteractionsDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                      }).Distinct().ToList();
                        var d4 = (from a in _PortalContext.MasterEmployee
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
                intrans.CreatedDate = indianTime;
                intrans.UpdatedDate = indianTime;
                intrans.ISTINT_ReadFlg = false;
                intrans.ISTINT_Attachment = image;
                _PortalContext.Add(intrans);

                var contactExists = _PortalContext.SaveChanges();

                long ISTINT_Id3 = 0;
                var ISTINT_Id1 = _PortalContext.IVRM_School_Transaction_InteractionsDMO.OrderByDescending(a => a.ISTINT_Id).ToList();
                var ISTINT_Id2 = ISTINT_Id1.FirstOrDefault().ISTINT_Id;
                ISTINT_Id3 = ISTINT_Id2;

                if (contactExists > 0)
                {
                    data.returnval = true;
                    if (groupOrIndFlg == "Individual" && byflg == "Staff")
                    {
                        var employeedata = _PortalContext.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == intrans.ISTINT_ComposedById).Distinct().ToList();
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

                    if (devicelist.Count > 0)
                    {
                        int k = 0;
                        foreach (var device_id in devicelist)
                        {
                            if (k == 0)
                            {
                                deviceidsnew = '"' + device_id.AppDownloadedDeviceId + '"';
                                k = 1;
                            }
                            else
                            {
                                deviceidsnew = deviceidsnew + "," + '"' + device_id.AppDownloadedDeviceId + '"';
                            }
                        }
                        devicenew = "[" + deviceidsnew + "]";

                        callnotification(devicenew, notiSubject, intrans.ISTINT_Id, data.MI_Id, ISTINT_Id3, data);
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
        public string callnotificationNew(string devicenew, string subject, long istint_Id, long mi_id, IVRM_School_InteractionsDTO dto)
        {
            try
            {
              

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


                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
              "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "New Message" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + subject + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=AAAAvDDD0Rs:APA91bEFpdVjbc7oDFoFR2LIagSffKZmmu17NowfggiE752rEo45Hgl1kNX2_AWVxHqBcAUJOTvo5CApdlHNwNFHKBjIFqhVEwiQC9PVYdba_SRCAHC2tMVTVzV2WBaWcKIGGwAOGo4I";

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

                push_noti.Insert_PushNotification_interaction1(subject, istint_Id, mi_id, dto);
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


                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
             "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Reply" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + notiSubject + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=AAAAvDDD0Rs:APA91bEFpdVjbc7oDFoFR2LIagSffKZmmu17NowfggiE752rEo45Hgl1kNX2_AWVxHqBcAUJOTvo5CApdlHNwNFHKBjIFqhVEwiQC9PVYdba_SRCAHC2tMVTVzV2WBaWcKIGGwAOGo4I";

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

                push_noti.Insert_PushNotification_interaction_replay1(notiSubject, ISTINT_Id3, mi_id, dto);
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
                    var updt = _context.IVRM_School_Transaction_InteractionsDMO.Single(a => a.ISMINT_Id == item.ISMINT_Id && a.ISTINT_Id==item.ISTINT_Id);
                    updt.ISTINT_ActiveFlag = false;
                    _context.Update(updt);
                }
                var resultnew = _context.IVRM_School_Master_InteractionsDMO.Single(a => a.ISMINT_Id == dto.ISMINT_Id);
                resultnew.ISMINT_ActiveFlag = false;
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

        public IVRM_School_InteractionsDTO seen(IVRM_School_InteractionsDTO dto)
        {
            try
            {
                var rmv = 0;
                var result = _context.IVRM_School_Transaction_InteractionsDMO.Single(a => a.ISMINT_Id == dto.ISMINT_Id);
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