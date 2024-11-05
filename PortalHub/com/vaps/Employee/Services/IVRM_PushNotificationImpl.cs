using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Portals.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class IVRM_PushNotificationImpl : Interfaces.IVRM_PushNotificationInterface
    {


        public PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _db;
        ILogger<IVRM_PushNotificationImpl> _logPortal;
        public DomainModelMsSqlServerContext _context;
        public FeeGroupContext _fees;
        public IVRM_PushNotificationImpl(PortalContext portalContext, DomainModelMsSqlServerContext context, FeeGroupContext fee, DomainModelMsSqlServerContext db, ILogger<IVRM_PushNotificationImpl> log)
        {
            _PortalContext = portalContext;
            _db = db;
            _logPortal = log;
            _context = context;
            _fees = fee;
        }
        public async Task<IVRM_PushNotificationDTO> Getdetails(IVRM_PushNotificationDTO data)
        {
            try
            {
                #region flag Details
                if (data.flag != "" && data.flag != null)
                {
                    var roletyp = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                    data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                }

                if (data.roletype.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    data.flag_Type = data.roletype;
                    #region staff Details
                    try
                    {

                        using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "IVRM_Staff_Detailss";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                             SqlDbType.BigInt)
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
                                data.stafflist = retObject.ToArray();
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
                    #endregion

                }
                else if (data.roletype.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.flag_Type = data.roletype;
                 
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                    #region Student Details
                    try
                    {
                        var clstchname = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.School_M_Class
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                          && b.HRME_Id == data.HRME_Id
                                          && c.ASMCL_ActiveFlag == true)
                                          select new IVRM_PushNotificationDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              ASMCL_ClassName = c.ASMCL_ClassName,
                                              ASMS_Id = a.ASMS_Id,
                                          }
                                    ).Distinct().ToArray();


                        data.studentlist = (from a in _fees.AdmissionStudentDMO
                                            from b in _fees.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == clstchname[0].ASMCL_Id && b.ASMS_Id == clstchname[0].ASMS_Id)
                                            select new IVRM_PushNotificationDTO
                                            {
                                                AMST_Id = a.AMST_Id,                                               
                                                AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),                                                
                                            }).OrderBy(t => t.AMST_FirstName).ToArray();



                        //using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                        //{
                        //    cmd.CommandText = "IVRM_Student_Details";
                        //    cmd.CommandType = CommandType.StoredProcedure;

                        //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        //    SqlDbType.BigInt)
                        //    {
                        //        Value = data.MI_Id
                        //    });
                        //    cmd.Parameters.Add(new SqlParameter("@Id",
                        //     SqlDbType.BigInt)
                        //    {
                        //        Value = data.UserId
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
                        //        data.studentlist = retObject.ToArray();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Console.WriteLine(ex.Message);
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        _logPortal.LogInformation("Message get_StudentClsSec :" + ex.Message);
                    }

                    #endregion
                }
                #endregion


                #region All data
                if (data.flag_Type == "Staff")
                {
                    data.studentdata = (from a in _PortalContext.IVRM_PushNotificationDMO
                                        from c in _PortalContext.IVRM_PushNotification_Student_DMO
                                        where (a.IPN_Id == c.IPN_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IVRMUL_Id==data.UserId)
                                        select new IVRM_PushNotificationDTO
                                        {
                                            IPN_Id = a.IPN_Id,
                                            IPN_No = a.IPN_No,
                                            IPN_Date = a.IPN_Date,
                                            IPN_StuStaffFlg = a.IPN_StuStaffFlg,
                                            IPN_PushNotification = a.IPN_PushNotification,
                                            IPN_ActiveFlag = a.IPN_ActiveFlag,

                                        }).Distinct().OrderBy(t => t.IPN_Id).ToArray();
                }

                else if (data.flag_Type == "Student")
                {
                    data.empdata = (from a in _PortalContext.IVRM_PushNotificationDMO
                                    from b in _PortalContext.IVRM_PushNotification_Staff_DMO
                                    from d in _PortalContext.HR_Master_Employee_DMO
                                    where (a.IPN_Id == b.IPN_Id && b.HRME_Id == d.HRME_Id && a.MI_Id==d.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id &&  a.IVRMUL_Id==data.UserId)
                                    select new IVRM_PushNotificationDTO
                                    {
                                        IPN_Id = a.IPN_Id,
                                        IPN_No = a.IPN_No,
                                        IPN_Date = a.IPN_Date,
                                        IPN_StuStaffFlg = a.IPN_StuStaffFlg,
                                        IPN_PushNotification = a.IPN_PushNotification,
                                        HRME_Id = b.HRME_Id,
                                        HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)),
                                        IPN_ActiveFlag = a.IPN_ActiveFlag,
                                        IPNST_ActiveFlag = b.IPNST_ActiveFlag,
                                    }).Distinct().OrderBy(t => t.IPN_Id).ToArray();
                }


                #endregion
            }
            catch (Exception ex)
            {
                _logPortal.LogInformation("Portal  :" + ex.Message);
            }
            return data;
        }


        public IVRM_PushNotificationDTO savedetail(IVRM_PushNotificationDTO data)
        {
            try
            {               
                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                MM = _context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "PushNotification").ToList();
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

                List<long> AMSTID = new List<long>();

                foreach (var std in data.stud_ids)
                {
                    AMSTID.Add(std.AMST_Id);

                }
           
                    //=========================================== Notification
                    var hm = _PortalContext.IVRM_ClassWorkDMO.ToList();
                var devicelist = (from a in _PortalContext.Adm_M_Student
                                  from b in _PortalContext.School_Adm_Y_StudentDMO
                                  where (a.AMST_Id == b.AMST_Id  && AMSTID.Contains(a.AMST_Id)  && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                  select new IVRM_ClassWorkDTO
                                  {
                                      AMST_Id = a.AMST_Id,
                                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                  }).Distinct().ToList();

                var devicelistd = (from a in _PortalContext.Adm_M_Student
                                   from b in _PortalContext.School_Adm_Y_StudentDMO
                                   where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id  && AMSTID.Contains(b.AMST_Id) && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
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
                //            callnotification(deviceid.AMST_AppDownloadedDeviceId, homeid, data.MI_Id, dto, "Notification", data.IPN_PushNotification);
                //        }

                //    }
                //    devicenew = "[" + deviceidsnew + "]";

                //    callnotification(devicenew, homeid, data.MI_Id, dto, "Notification", data.IPN_PushNotification);
                //}


                var deviceidsnew = "";
                var devicenew = "";
                var redirecturl = "";
                long revieveduserid = 0;

                if (devicelist.Count > 0)
                {
                    foreach (var device_id in devicelist)
                    {
                        if (device_id.AMST_AppDownloadedDeviceId.Length > 0)
                        {


                            revieveduserid = (from a in _PortalContext.StudentUserLoginDMO
                                              from b in _PortalContext.ApplicationUser
                                              where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                              select b).Select(t => t.Id).FirstOrDefault();



                            PushNotification push_noti = new PushNotification(_PortalContext);
                            push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, homeid, data.IPN_PushNotification, "Push Notification", "Push Notification");

                        }
                    }
                }


                if (data.IPN_Id > 0)
                {
                    //var Duplicate = _PortalContext.IVRM_PushNotificationDMO.Where(t => t.IPN_Id != data.IPN_Id && t.IPN_No != data.IPN_No).ToList();

                    //if (Duplicate.Count() > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{
                        var result = _PortalContext.IVRM_PushNotificationDMO.Single(t => t.IPN_Id.Equals(data.IPN_Id) && t.MI_Id.Equals(data.MI_Id));


                        result.IPN_StuStaffFlg = data.IPN_StuStaffFlg;
                        result.IPN_Date = data.IPN_Date;
                        result.IPN_PushNotification = data.IPN_PushNotification;
                        result.UpdatedDate = DateTime.Now;

                        _PortalContext.Add(result);

                        if (data.IPN_StuStaffFlg == "Staff")
                        {
                            var remove = _PortalContext.IVRM_PushNotification_Student_DMO.Where(t => t.IPN_Id == data.IPN_Id).ToList();

                            if (remove.Count > 0)
                            {
                                foreach (var item in remove)
                                {
                                    _PortalContext.Remove(item);
                                }
                            }

                            foreach (var std in data.stud_ids)
                            {
                                IVRM_PushNotification_Student_DMO update2 = new IVRM_PushNotification_Student_DMO();

                                update2.IPN_Id = result.IPN_Id;
                                update2.AMST_Id = std.AMST_Id;
                                update2.IPNS_ActiveFlag = true;
                                update2.UpdatedDate = DateTime.Now;
                                update2.CreatedDate = DateTime.Now;
                                _PortalContext.Add(update2);
                            }
                        }
                        else if (data.IPN_StuStaffFlg == "Student")
                        {
                            var update3 = _PortalContext.IVRM_PushNotification_Staff_DMO.Where(t => t.IPN_Id == result.IPN_Id).SingleOrDefault();

                            update3.IPN_Id = result.IPN_Id;
                            update3.HRME_Id = data.HRME_Id;
                            update3.UpdatedDate = DateTime.Now;

                            _PortalContext.Update(update3);
                        }

                        _PortalContext.Update(result);
                        var contactExists = _PortalContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                   // }
                }
                else
                {                   
                     if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                        {
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                            data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                            data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                            data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);

                        }

                        IVRM_PushNotificationDMO obj1 = new IVRM_PushNotificationDMO();


                        obj1.MI_Id = data.MI_Id;
                        obj1.IPN_No = data.trans_id;
                        obj1.IPN_StuStaffFlg = data.IPN_StuStaffFlg;
                        obj1.ASMAY_Id = data.ASMAY_Id;
                        obj1.IPN_Date = data.IPN_Date;
                        obj1.IPN_PushNotification = data.IPN_PushNotification;
                        obj1.IVRMUL_Id = data.UserId;
                        obj1.IPN_ActiveFlag = true;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;

                        _PortalContext.Add(obj1);


                        if (data.IPN_StuStaffFlg == "Staff")
                        {
                            foreach (var std in data.stud_ids)
                            {
                                IVRM_PushNotification_Student_DMO obj2 = new IVRM_PushNotification_Student_DMO();


                                obj2.IPN_Id = obj1.IPN_Id;
                                obj2.AMST_Id = std.AMST_Id;
                                obj2.IPNS_ActiveFlag = true;
                                obj2.CreatedDate = DateTime.Now;
                                obj2.UpdatedDate = DateTime.Now;

                                _PortalContext.Add(obj2);
                            }
                        }
                        else if (data.IPN_StuStaffFlg == "Student")
                        {

                            IVRM_PushNotification_Staff_DMO obj3 = new IVRM_PushNotification_Staff_DMO();

                            obj3.IPN_Id = obj1.IPN_Id;
                            obj3.HRME_Id = data.HRME_Id;
                            obj3.IPNST_ActiveFlag = true;
                            obj3.CreatedDate = DateTime.Now;
                            obj3.UpdatedDate = DateTime.Now;

                            _PortalContext.Add(obj3);
                        }

                        int contactExists = _PortalContext.SaveChanges();
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
            catch (Exception ex)
            {
                _logPortal.LogInformation("Push Notification  :" + ex.Message);
            }
            return data;
        }


        public IVRM_PushNotificationDTO editnoticestf(IVRM_PushNotificationDTO data)
        {
            try
            {
                data.editstaflist = (from a in _PortalContext.IVRM_PushNotificationDMO
                                     from b in _PortalContext.IVRM_PushNotification_Staff_DMO
                                     where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IPN_Id == data.IPN_Id && a.IPN_Id == b.IPN_Id)
                                     select new IVRM_PushNotificationDTO
                                     {
                                         IPN_Id = a.IPN_Id,
                                         IPN_No = a.IPN_No,
                                         IPN_Date = a.IPN_Date,
                                         IPN_PushNotification = a.IPN_PushNotification,
                                         IPN_StuStaffFlg = a.IPN_StuStaffFlg,
                                         HRME_Id = b.HRME_Id

                                     }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public IVRM_PushNotificationDTO editnoticestud(IVRM_PushNotificationDTO data)
        {
            try
            {
                data.editstudlist = (from a in _PortalContext.IVRM_PushNotificationDMO
                                     from b in _PortalContext.IVRM_PushNotification_Student_DMO
                                     where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IPN_Id == data.IPN_Id && a.IPN_Id == b.IPN_Id)
                                     select new IVRM_PushNotificationDTO
                                     {
                                         IPN_Id = a.IPN_Id,
                                         IPN_No = a.IPN_No,
                                         IPN_Date = a.IPN_Date,
                                         IPN_PushNotification = a.IPN_PushNotification,
                                         IPN_StuStaffFlg = a.IPN_StuStaffFlg,
                                         AMST_Id = b.AMST_Id

                                     }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_PushNotificationDTO deactivate(IVRM_PushNotificationDTO data)
        {
            try
            {
                var query = _PortalContext.IVRM_PushNotificationDMO.Single(s => s.MI_Id == data.MI_Id && s.IPN_Id == data.IPN_Id);
                var query2 = _PortalContext.IVRM_PushNotification_Staff_DMO.Single(s => s.IPN_Id == data.IPN_Id);
                if (query.IPN_ActiveFlag == true && query2.IPNST_ActiveFlag == true)
                {
                    query.IPN_ActiveFlag = false;
                    query2.IPNST_ActiveFlag = false;
                }
                else
                {
                    query.IPN_ActiveFlag = true;
                    query2.IPNST_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;
                _PortalContext.Update(query);
                var contactExists = _PortalContext.SaveChanges();
                if (contactExists > 0)
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
                _logPortal.LogInformation("Portal  :" + ex.Message);
            }
            return data;
        }

        public IVRM_PushNotificationDTO Deactivatemain(IVRM_PushNotificationDTO data)
        {
            try
            {
                var query = _PortalContext.IVRM_PushNotificationDMO.Single(s => s.MI_Id == data.MI_Id && s.IPN_Id == data.IPN_Id);
                if (query.IPN_ActiveFlag == true)
                {
                    query.IPN_ActiveFlag = false;
                }
                else
                {
                    query.IPN_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;

                _PortalContext.Update(query);

                var contactExists = _PortalContext.SaveChanges();
                if (contactExists > 0)
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
                _logPortal.LogInformation("Portal  :" + ex.Message);
            }
            return data;
        }

        public IVRM_PushNotificationDTO Deactivatestud(IVRM_PushNotificationDTO data)
        {
            try
            {
                var query = _PortalContext.IVRM_PushNotification_Student_DMO.Single(s => s.IPNS_Id == data.IPNS_Id);

                if (query.IPNS_ActiveFlag == true)
                {
                    query.IPNS_ActiveFlag = false;
                }
                else
                {
                    query.IPNS_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;

                _PortalContext.Update(query);

                var contactExists = _PortalContext.SaveChanges();
                if (contactExists > 0)
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
                _logPortal.LogInformation("Portal  :" + ex.Message);
            }
            return data;
        }

        public IVRM_PushNotificationDTO get_modeldata(IVRM_PushNotificationDTO data)
        {
            try
            {
                data.modalstudlist = (from a in _PortalContext.IVRM_PushNotificationDMO
                                      from c in _PortalContext.IVRM_PushNotification_Student_DMO
                                      from d in _PortalContext.Adm_M_Student
                                      where (a.IPN_Id == c.IPN_Id && c.AMST_Id == d.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IPN_Id == data.IPN_Id)
                                      select new IVRM_PushNotificationDTO
                                      {
                                          IPN_Id = a.IPN_Id,
                                          IPN_No = a.IPN_No,
                                          IPN_Date = a.IPN_Date,
                                          IPN_StuStaffFlg = a.IPN_StuStaffFlg,
                                          IPN_PushNotification = a.IPN_PushNotification,
                                          AMST_Id = c.AMST_Id,
                                          AMST_FirstName = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + " " + (d.AMST_LastName == null ? " " : d.AMST_LastName)),
                                          IPNS_Id = c.IPNS_Id,
                                          IPNS_ActiveFlag = c.IPNS_ActiveFlag,
                                      }).Distinct().OrderBy(t => t.IPN_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string callnotification(string devicenew, long icw_Id, long mi_id, IVRM_ClassWorkDTO dto, string header_flg,string contant)
        {
            try
            {

                var key = _PortalContext.MobileApplAuthenticationDMO.Single(a => a.MI_Id == mi_id).MAAN_AuthenticationKey;

                IVRM_ClassWorkDTO data = new IVRM_ClassWorkDTO();
                var classwork = _PortalContext.IVRM_ClassWorkDMO.Where(h => h.MI_Id == mi_id && h.ICW_ActiveFlag == true && h.ICW_Id == icw_Id).Distinct().ToList();



                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //     "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Topic + '"' + " , " + '"' + "body" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Content + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";


             
                
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


    }
}
