using System;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Birthday;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Dynamic;
using CommonLibrary;
using QRCoder;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net.Mail;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using Newtonsoft.Json.Linq;
using DomainModel.Model.com.vapstech.IssueManager;
using System.Globalization;

namespace VisitorsManagementServiceHub.Services
{
    public class V_AppointmentApprovalStatusImpl : Interfaces.V_AppointmentApprovalStatusInterface
    {
        public VisitorsManagementContext _VisitorContext;
        public DomainModelMsSqlServerContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _accessor;
        public V_AppointmentApprovalStatusImpl(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, VisitorsManagementContext context1, DomainModelMsSqlServerContext context2)
        {
            _hostingEnvironment = hostingEnvironment;
            _accessor = httpContextAccessor;
            _VisitorContext = context1;
            _db = context2;
        }

        public AppointmentApprovalStatus_DTO getDetails(AppointmentApprovalStatus_DTO data)
        {
            try
            {
                long roletypeid = 0;
                string roletype = "";

                if (data.UserId > 0)
                {
                    roletypeid = _VisitorContext.appUserRole.Where(t => t.UserId == data.UserId).Distinct().FirstOrDefault().RoleTypeId;
                    if (roletypeid > 0)
                    {
                        roletype = _VisitorContext.applicationRole.Where(t => t.Id == roletypeid).Distinct().FirstOrDefault().roleType;
                    }
                    if (roletype==null)
                    {
                        roletype = "";
                    }
                  
                }

                data.emplist = (from t in _VisitorContext.MasterEmployee
                                where (t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.MI_Id==data.MI_Id)
                                select new Visitor_Management_Appointment_DTO
                                {
                                    HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                    HRME_Id = t.HRME_Id,
                                    MI_Id = t.MI_Id,
                                }).Distinct().OrderBy(T => T.HRME_EmployeeFirstName).ToArray();


                


                using (var cmd = _VisitorContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_VISITORS_FOR_APPROVAL_NEW_IVRM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USER_ID",
                      SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });

                    cmd.Parameters.Add(new SqlParameter("@ROLE",
                      SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.visitorlist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }





                using (var cmd = _VisitorContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_APPROVED_VISITOR_IVRM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USER_ID",
                      SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });

                    cmd.Parameters.Add(new SqlParameter("@ROLE",
                      SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.griddata = retObject.ToArray();
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
            return data;
        }
        public AppointmentApprovalStatus_DTO EditDetails(AppointmentApprovalStatus_DTO id)
        {
            AppointmentApprovalStatus_DTO resp = new AppointmentApprovalStatus_DTO();
            try
            {

                resp.editDetails = (from a in _VisitorContext.Visitor_Management_Appointment_DMO
                                    where (a.VMAP_Id == id.VMAP_Id && a.MI_Id == id.MI_Id)
                                    select new AppointmentApprovalStatus_DTO
                                    {
                                        VMAP_Id = a.VMAP_Id,
                                        VMAP_VisitorName = a.VMAP_VisitorName,
                                        VMAP_EntryDateTime = a.VMAP_EntryDateTime,
                                        VMAP_Remarks = a.VMAP_Remarks,
                                        VMAP_MeetingDateTime = a.VMAP_MeetingDateTime,
                                        MI_Id = a.MI_Id,
                                    }).Distinct().ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }
        public AppointmentApprovalStatus_DTO Editnew(AppointmentApprovalStatus_DTO data)
        {
            try
            {

                data.editDetails = (from a in _VisitorContext.Visitor_Management_Appointment_DMO
                                    where (a.VMAP_Id == data.VMAP_Id)
                                    select new AppointmentApprovalStatus_DTO
                                    {
                                        VMAP_Id = a.VMAP_Id,
                                        VMAP_VisitorContactNo = a.VMAP_VisitorContactNo,
                                        VMAP_VisitorEmailid = a.VMAP_VisitorEmailid,
                                        VMAP_FromAddress = a.VMAP_FromAddress,
                                        VMAP_VisitorName = a.VMAP_VisitorName,
                                        VMAP_EntryDateTime = a.VMAP_EntryDateTime,
                                        VMAP_Remarks = a.VMAP_Remarks,
                                        VMAP_MeetingDateTime = a.VMAP_MeetingDateTime,
                                        MI_Id = a.MI_Id,
                                    }).Distinct().ToArray();


                data.editfiles = (from a in _VisitorContext.Visitor_Management_Appointment_FilesDMO

                                  where (a.VMAP_Id == data.VMAP_Id && a.VMAPVF_ActiveFlg==true)
                                  select new vmsappdocDTO
                                  {
                                      gridid = a.VMAP_Id,
                                      cfileid = a.VMAPVF_Id,
                                      cfilename = a.VMAPVF_FileName,
                                      cfilepath = a.VMAPVF_FilePath,
                                      cfiledesc = a.VMAPVF_Filedesc
                                  }).Distinct().ToArray();

                data.visitorlist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(f => f.VMAP_Id == data.VMAP_Id ).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<AppointmentApprovalStatus_DTO> saveDataAsync(AppointmentApprovalStatus_DTO data)
        {
            try
            {
                if (data.VMAP_Id > 0)
                {
                    //if (data.VMAP_Status== "Re-Schedule")
                    //{
                    //    data.VMAP_Status = "Approved";
                    //}
                    
                    long message = 0;
                    if (data.VMAP_Status == "Approved" || data.VMAP_Status == "RE-SCHEDULE")
                    {
                        using (var cmd = _VisitorContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "VMP_DUPLICATE_CHECKING_IVRM";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();


                            cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                              SqlDbType.BigInt)
                            {
                                Value = data.VMAP_HRME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@VMAP_Id",
                              SqlDbType.BigInt)
                            {
                                Value = data.VMAP_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@MeetingDate",
                              SqlDbType.Date)
                            {
                                Value = Convert.ToDateTime(data.VMAP_MeetingDateTime).Date
                            });
                            cmd.Parameters.Add(new SqlParameter("@StartTime",
                              SqlDbType.VarChar)
                            {
                                Value = data.VMAP_MeetingTiming
                            });
                            cmd.Parameters.Add(new SqlParameter("@EndTime",
                              SqlDbType.VarChar)
                            {
                                Value = data.VMAP_MeetingToTime
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.BigInt) { Direction = ParameterDirection.Output });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data3 = cmd.ExecuteNonQuery();

                            message = Convert.ToInt64(cmd.Parameters["@Result"].Value.ToString());
                        }
                    }
                    else
                    {
                        message = 0;
                    }

                    if (message > 0)
                    {
                        data.returnVal = "ex";
                       
                    }
                    else
                    {
                        var update = _VisitorContext.Visitor_Management_Appointment_DMO.Single(d => d.VMAP_Id == data.VMAP_Id && d.MI_Id == data.MI_Id);
                        DateTime date = new DateTime();
                        date = Convert.ToDateTime(data.VMAP_MeetingDateTime);

                        update.UpdatedDate = DateTime.Now;
                        update.VMAP_MeetingDateTime = date.AddHours(data.fhrors).AddMinutes(data.fminutes).AddSeconds(0);
                        update.VMAP_HRME_Id = data.VMAP_HRME_Id;
                        update.VMAP_Status = data.VMAP_Status;
                        update.VMAP_MeetingTiming = data.VMAP_MeetingTiming;
                        update.VMAP_MeetingFromTime = data.VMAP_MeetingTiming;
                        update.VMAP_MeetingToTime = data.VMAP_MeetingToTime;
                        update.VMAP_ReminderDate = data.VMAP_ReminderDate;
                        update.VMAP_UpdatedBy = data.UserId;
                        update.VMAP_Remarks = data.VMAP_Remarks;
                        update.VMAP_RescheduleReason = data.VMAP_RescheduleReason;

                        if (data.type != "R")
                        {
                            update.VMAP_RepeatFlag = data.VMAP_RepeatFlag;
                            update.VMAP_ReminderSchedule = data.VMAP_ReminderSchedule;
                            update.VMAP_ReminderFlag = data.VMAP_ReminderFlag;
                        }



                        _VisitorContext.Update(update);
                      

                         int s = _VisitorContext.SaveChanges();
                        if (s > 0)
                        {
                            data.returnVal = "updated";

                            if (data.VMAP_Status == "Approved")
                            {
                                var base64string = "";

                                long primary_id = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_Id;

                                List<string> toemail = new List<string>();
                                List<string> tomobile = new List<string>();

                                string emailid = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_VisitorEmailid;
                                if (emailid != "")
                                {
                                    toemail.Add(emailid);

                                }

                                //var allvisitorlist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(d => d.VMAP_Id == data.VMAP_Id).ToList();
                                //if (allvisitorlist.Count > 0)
                                //{
                                //    foreach (var item in allvisitorlist)
                                //    {

                                //        if (item.VMAPVI_VisitorEmailId != null && item.VMAPVI_VisitorEmailId != "" && item.VMAPVI_VisitorEmailId != "0")
                                //        {

                                //            toemail.Add(item.VMAPVI_VisitorEmailId);

                                //        }
                                //        if (item.VMAPVI_VisitorContactNo != null && item.VMAPVI_VisitorContactNo != "" && item.VMAPVI_VisitorContactNo != "0")
                                //        {

                                //            tomobile.Add(item.VMAPVI_VisitorContactNo);

                                //        }



                                //    }
                                //}
                                long? staffid = _VisitorContext.Visitor_Management_Appointment_DMO.OrderByDescending(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_HRME_Id;
                                string staffemail = _VisitorContext.Emp_Email_Id.Where(d => d.HRME_Id == staffid && d.HRMEM_DeFaultFlag == "default").FirstOrDefault().HRMEM_EmailId;


                                sendmailVisitor(data.MI_Id, primary_id, staffemail, "NewVisitorAppointment", base64string, toemail);

                                long mobileno = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_VisitorContactNo;

                                if (mobileno > 0)
                                {
                                    SMS sss = new SMS(_db);
                                    string x = sss.sendSmsVisitor(data.MI_Id, primary_id, mobileno, "NewVisitorAppointment").Result;
                                }
                                if (tomobile.Count > 0)
                                {
                                    foreach (var item in tomobile)
                                    {
                                        long mobileno1 = Convert.ToInt64(item);
                                        SMS sss = new SMS(_db);
                                        string x = sss.sendSmsVisitor(data.MI_Id, primary_id, mobileno1, "NewVisitorAppointment").Result;
                                    }
                                }


                                long staffmobileno = _VisitorContext.Emp_MobileNo.Where(d => d.HRME_Id == staffid && d.HRMEMNO_DeFaultFlag == "default").FirstOrDefault().HRMEMNO_MobileNo;

                                if (staffmobileno > 0)
                                {
                                    SMS sss = new SMS(_db);
                                    string x = sss.sendSmsStaff(data.MI_Id, primary_id, staffmobileno, "VisitorStaffApproval").Result;
                                }



                            }

                            if (data.VMAP_Status == "RE-SCHEDULE")
                            {
                                var base64string = "";

                                var update1 = _VisitorContext.Visitor_Management_Appointment_DMO.Single(d => d.VMAP_Id == data.VMAP_Id && d.MI_Id == data.MI_Id);
                                update1.VMAP_Status = "Approved";
                                _VisitorContext.Update(update1);
                                int s1 = _VisitorContext.SaveChanges();
                                long primary_id = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_Id;

                                List<string> toemail = new List<string>();
                                List<string> tomobile = new List<string>();

                                string emailid = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_VisitorEmailid;
                                if (emailid != "")
                                {
                                    toemail.Add(emailid);

                                }

                                //var allvisitorlist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(d => d.VMAP_Id == data.VMAP_Id).ToList();
                                //if (allvisitorlist.Count > 0)
                                //{
                                //    foreach (var item in allvisitorlist)
                                //    {

                                //        if (item.VMAPVI_VisitorEmailId != null && item.VMAPVI_VisitorEmailId != "" && item.VMAPVI_VisitorEmailId != "0")
                                //        {

                                //            toemail.Add(item.VMAPVI_VisitorEmailId);

                                //        }
                                //        if (item.VMAPVI_VisitorContactNo != null && item.VMAPVI_VisitorContactNo != "" && item.VMAPVI_VisitorContactNo != "0")
                                //        {

                                //            tomobile.Add(item.VMAPVI_VisitorContactNo);

                                //        }



                                //    }
                                //}
                                long? staffid = _VisitorContext.Visitor_Management_Appointment_DMO.OrderByDescending(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_HRME_Id;
                                string staffemail = _VisitorContext.Emp_Email_Id.Where(d => d.HRME_Id == staffid && d.HRMEM_DeFaultFlag == "default").FirstOrDefault().HRMEM_EmailId;


                                sendmailVisitor(data.MI_Id, primary_id, staffemail, "Appointment_Reschedule", base64string, toemail);

                                long mobileno = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_VisitorContactNo;

                                if (mobileno > 0)
                                {
                                    SMS sss = new SMS(_db);
                                    string x = sss.sendSmsVisitor(data.MI_Id, primary_id, mobileno, "Appointment_Reschedule").Result;
                                }
                                if (tomobile.Count > 0)
                                {
                                    foreach (var item in tomobile)
                                    {
                                        long mobileno1 = Convert.ToInt64(item);
                                        SMS sss = new SMS(_db);
                                        string x = sss.sendSmsVisitor(data.MI_Id, primary_id, mobileno1, "Appointment_Reschedule").Result;
                                    }
                                }


                                long staffmobileno = _VisitorContext.Emp_MobileNo.Where(d => d.HRME_Id == staffid && d.HRMEMNO_DeFaultFlag == "default").FirstOrDefault().HRMEMNO_MobileNo;

                                if (staffmobileno > 0)
                                {
                                    SMS sss = new SMS(_db);
                                    string x = sss.sendSmsStaff(data.MI_Id, primary_id, staffmobileno, "VisitorStaffApproval").Result;
                                }



                            }

                            else if (data.VMAP_Status == "Canceled")
                            {
                                var base64string = "";



                                long primary_id = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_Id;

                                List<string> toemail = new List<string>();
                                List<string> tomobile = new List<string>();

                                string emailid = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_VisitorEmailid;
                                if (emailid != "")
                                {
                                    toemail.Add(emailid);

                                }

                                var allvisitorlist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(d => d.VMAP_Id == data.VMAP_Id).ToList();
                                //if (allvisitorlist.Count > 0)
                                //{
                                //    foreach (var item in allvisitorlist)
                                //    {

                                //        if (item.VMAPVI_VisitorEmailId != null && item.VMAPVI_VisitorEmailId != "" && item.VMAPVI_VisitorEmailId != "0")
                                //        {

                                //            toemail.Add(item.VMAPVI_VisitorEmailId);

                                //        }
                                //        if (item.VMAPVI_VisitorContactNo != null && item.VMAPVI_VisitorContactNo != "" && item.VMAPVI_VisitorContactNo != "0")
                                //        {

                                //            tomobile.Add(item.VMAPVI_VisitorContactNo);

                                //        }



                                //    }
                                //}
                                long? staffid = _VisitorContext.Visitor_Management_Appointment_DMO.OrderByDescending(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_HRME_Id;
                                string staffemail = _VisitorContext.Emp_Email_Id.Where(d => d.HRME_Id == staffid && d.HRMEM_DeFaultFlag == "default").FirstOrDefault().HRMEM_EmailId;


                                sendmailVisitor(data.MI_Id, primary_id, staffemail, "VisitorAppointmentCanceled", base64string, toemail);

                                long mobileno = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_VisitorContactNo;

                                if (mobileno > 0)
                                {
                                    SMS sss = new SMS(_db);
                                    string x = sss.sendSmsVisitor(data.MI_Id, primary_id, mobileno, "VisitorAppointmentCanceled").Result;
                                }
                                if (tomobile.Count > 0)
                                {
                                    foreach (var item in tomobile)
                                    {
                                        long mobileno1 = Convert.ToInt64(item);
                                        SMS sss = new SMS(_db);
                                        string x = sss.sendSmsVisitor(data.MI_Id, primary_id, mobileno1, "VisitorAppointmentCanceled").Result;
                                    }
                                }


                                long staffmobileno = _VisitorContext.Emp_MobileNo.Where(d => d.HRME_Id == staffid && d.HRMEMNO_DeFaultFlag == "default").FirstOrDefault().HRMEMNO_MobileNo;

                                if (staffmobileno > 0)
                                {
                                    SMS sss = new SMS(_db);
                                    string x = sss.sendSmsStaff(data.MI_Id, primary_id, staffmobileno, "VisitorAppointmentCanceled").Result;
                                }


                            }
                            else if (data.VMAP_Status == "Rejected")
                            {

                                var base64string = "";

                                long primary_id = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_Id;

                                List<string> toemail = new List<string>();
                                List<string> tomobile = new List<string>();

                                string emailid = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_VisitorEmailid;
                                if (emailid != "")
                                {
                                    toemail.Add(emailid);

                                }

                                var allvisitorlist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(d => d.VMAP_Id == data.VMAP_Id).ToList();
                                if (allvisitorlist.Count > 0)
                                {
                                    foreach (var item in allvisitorlist)
                                    {

                                        if (item.VMAPVI_VisitorEmailId != null && item.VMAPVI_VisitorEmailId != "" && item.VMAPVI_VisitorEmailId != "0")
                                        {

                                            toemail.Add(item.VMAPVI_VisitorEmailId);

                                        }
                                        if (item.VMAPVI_VisitorContactNo != null && item.VMAPVI_VisitorContactNo != "" && item.VMAPVI_VisitorContactNo != "0")
                                        {

                                            tomobile.Add(item.VMAPVI_VisitorContactNo);

                                        }



                                    }
                                }
                                long? staffid = _VisitorContext.Visitor_Management_Appointment_DMO.OrderByDescending(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_HRME_Id;
                                string staffemail = _VisitorContext.Emp_Email_Id.Where(d => d.HRME_Id == staffid && d.HRMEM_DeFaultFlag == "default").FirstOrDefault().HRMEM_EmailId;


                                sendmailVisitor(data.MI_Id, primary_id, staffemail, "VisitorAppointmentRejected", base64string, toemail);

                                long mobileno = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_VisitorContactNo;

                                if (mobileno > 0)
                                {
                                    SMS sss = new SMS(_db);
                                    string x = sss.sendSmsVisitor(data.MI_Id, primary_id, mobileno, "VisitorAppointmentRejected").Result;
                                }
                                if (tomobile.Count > 0)
                                {
                                    foreach (var item in tomobile)
                                    {
                                        long mobileno1 = Convert.ToInt64(item);
                                        SMS sss = new SMS(_db);
                                        string x = sss.sendSmsVisitor(data.MI_Id, primary_id, mobileno1, "VisitorAppointmentRejected").Result;
                                    }
                                }


                                long staffmobileno = _VisitorContext.Emp_MobileNo.Where(d => d.HRME_Id == staffid && d.HRMEMNO_DeFaultFlag == "default").FirstOrDefault().HRMEMNO_MobileNo;

                                if (staffmobileno > 0)
                                {
                                    SMS sss = new SMS(_db);
                                    string x = sss.sendSmsStaff(data.MI_Id, primary_id, staffmobileno, "VisitorAppointmentRejected").Result;
                                }
                                


                            }

                            notify(data.VMAP_Id,data.UserId);

                        }
                        else
                        {
                            data.returnVal = "updateFailed";
                        }
                    }

                   
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public AppointmentApprovalStatus_DTO check_remainder(AppointmentApprovalStatus_DTO data)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AppointmentApprovalStatus_DTO viewuploadflies(AppointmentApprovalStatus_DTO data)
        {

            try
            {
                data.editfiles = (from a in _VisitorContext.Visitor_Management_Appointment_FilesDMO

                                  where (a.VMAP_Id == data.VMAP_Id && a.VMAPVF_ActiveFlg == true)
                                  select new vmsappdocDTO
                                  {
                                      gridid = a.VMAP_Id,
                                      cfileid = a.VMAPVF_Id,
                                      cfilename = a.VMAPVF_FileName,
                                      cfilepath = a.VMAPVF_FilePath,
                                      cfiledesc = a.VMAPVF_Filedesc
                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public AppointmentApprovalStatus_DTO sendMOM(AppointmentApprovalStatus_DTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                if (data.VMAP_Id>0)
                {
                    if (data.filelist!=null)
                    {
                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    Visitor_Management_Appointment_FilesDMO obb = new Visitor_Management_Appointment_FilesDMO();

                                    obb.VMAP_Id = data.VMAP_Id;
                                    obb.VMAPVF_FileName = item.cfilename;
                                    obb.VMAPVF_FilePath = item.cfilepath;
                                    obb.VMAPVF_Filedesc = item.cfiledesc;
                                    obb.VMAPVF_ActiveFlg = true;
                                    obb.VMAPVF_CreatedBy = data.UserId;
                                    obb.VMAPVF_UpdatedBy = data.UserId;
                                    obb.VMAPVF_CreatedDate = indianTime;
                                    obb.VMAPVF_Updateddate = indianTime;
                                    _VisitorContext.Add(obb);
                                }
                                
                            }
                            int cxt = _VisitorContext.SaveChanges();
                            if (cxt>0)
                            {
                                data.returnVal = "updated";

                                List<string> toemail = new List<string>();

                                string emailid = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_VisitorEmailid;
                                if (emailid != "")
                                {
                                    toemail.Add(emailid);

                                }
                                var allvisitorlist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(d => d.VMAP_Id == data.VMAP_Id).ToList();
                                if (allvisitorlist.Count > 0)
                                {
                                    foreach (var item in allvisitorlist)
                                    {

                                        if (item.VMAPVI_VisitorEmailId != null && item.VMAPVI_VisitorEmailId != "" && item.VMAPVI_VisitorEmailId != "0")
                                        {

                                            toemail.Add(item.VMAPVI_VisitorEmailId);

                                        }
                                       
                                    }
                                }

                                long? staffid = _VisitorContext.Visitor_Management_Appointment_DMO.OrderByDescending(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_HRME_Id;
                                string staffemail = _VisitorContext.Emp_Email_Id.Where(d => d.HRME_Id == staffid && d.HRMEM_DeFaultFlag == "default").FirstOrDefault().HRMEM_EmailId;


                                sendmailVisitorMOM(data.MI_Id, data.VMAP_Id, staffemail, "VSMAPMOM", "", toemail, data.filelist);


                            }
                            else
                            {
                                data.returnVal = "failed";
                            }

                        }
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public AppointmentApprovalStatus_DTO getfeedback(AppointmentApprovalStatus_DTO data)
        {
            try
            {
                data.editDetails = (from a in _VisitorContext.Visitor_Management_Appointment_DMO
                                    where (a.VMAP_Id == data.VMAP_Id)
                                    select new AppointmentApprovalStatus_DTO
                                    {
                                        VMAP_Id = a.VMAP_Id,
                                        VMAP_Feedback = a.VMAP_Feedback,
                                        MI_Id = a.MI_Id,
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public AppointmentApprovalStatus_DTO savefeedback(AppointmentApprovalStatus_DTO data)
        {
            try
            {
                var update = _VisitorContext.Visitor_Management_Appointment_DMO.Single(e => e.VMAP_Id == data.VMAP_Id);
                update.VMAP_Feedback = data.VMAP_Feedback;
                update.UpdatedDate = DateTime.Now;
                update.VMAP_UpdatedBy = data.UserId;
                _VisitorContext.Update(update);


                int s = _VisitorContext.SaveChanges();
                if (s>0)
                {
                    data.returnVal = "up";                }
                else
                {
                    data.returnVal = "er";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }



        public async Task<string> qrcodegenerate(String str, long mid)
        {
            string s = "Vistior Id :" + str;

            string message = qrcode(s);

            var myfilename = string.Format(@"{0}", Guid.NewGuid());

            var webRootPath = _hostingEnvironment.ContentRootPath;
            string filepath = Path.Combine(_hostingEnvironment.ContentRootPath, "QRCode") + $@"\{str}.jpeg";

            var bytess = Convert.FromBase64String(message);
            using (var imageFile = new FileStream(filepath, FileMode.Create))
            {
                imageFile.Write(bytess, 0, bytess.Length);
                imageFile.Flush();
            }
            string accountname = "";
            string accesskey = "";
            string newImageNamePath = "";
            string newImageName = "";
            var data = _VisitorContext.IVRM_Storage_path_Details.ToList();
            if (data.Count() > 0)
            {
                accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
            }

            string containername = "files";
            string folder = "QRCODE";
            StorageCredentials cre = new StorageCredentials(accountname, accesskey);
            CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
            CloudBlobClient blobClient = acc.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containername);

            var ext = ".jpeg";
            var guid = Guid.NewGuid().ToString();
            newImageName = guid + ext;
            string blobName = newImageName;
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            var blockBlob = container.GetBlockBlobReference(mid + "/" + folder + "/" + blobName);
            await blockBlob.UploadFromFileAsync(filepath);
            newImageNamePath = blockBlob.Uri.ToString();

            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }

            return newImageNamePath;
        }
        public string qrcode(String str)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            var bitmapBytes = BitmapToBytes(qrCodeImage);
            string imageData = Convert.ToBase64String(bitmapBytes);
            //string imageData1 = "data:image/png;base64," + Convert.ToBase64String(bitmapBytes);
            return imageData;
        }
        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }


        public AppointmentApprovalStatus_DTO ApprovalReminder(AppointmentApprovalStatus_DTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                var remiderlist = _VisitorContext.Visitor_Management_Appointment_DMO.Where(a => a.VMAP_ActiveFlag == true && a.VMAP_Status == "Approved" && Convert.ToDateTime(a.VMAP_MeetingDateTime).Date == indiantime0.Date).Distinct().ToList();

                if (remiderlist.Count>0)
                {
                    foreach (var item in remiderlist)
                    {
                        if (data.flag=="H")
                        {
                            if (item.VMAP_ReminderFlag == "Hour")
                            {
                                if (item.VMAP_ReminderSchedule != null && item.VMAP_ReminderSchedule != "")
                                {

                                    string c = Convert.ToDateTime(item.VMAP_MeetingDateTime).Date.ToString("dd/MM/yyyy") + " " + item.VMAP_MeetingTiming;
                                    DateTime exfromdate = Convert.ToDateTime(c);
                                    int hr = Convert.ToInt32(item.VMAP_ReminderSchedule);
                                    DateTime newdate = exfromdate.AddHours(-hr);
                                    if (indiantime0 >= newdate && indiantime0 <= exfromdate)
                                    {
                                        var base64string = "";
                                        
                                        long primary_id = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == item.VMAP_Id).FirstOrDefault().VMAP_Id;

                                        long? staffid = _VisitorContext.Visitor_Management_Appointment_DMO.OrderByDescending(d => d.VMAP_Id == item.VMAP_Id).FirstOrDefault().VMAP_HRME_Id;

                                        string staffemail = _VisitorContext.Emp_Email_Id.Where(d => d.HRME_Id == staffid && d.HRMEM_DeFaultFlag == "default").FirstOrDefault().HRMEM_EmailId;

                                        if (staffemail != "")
                                        {
                                            Email email = new Email(_db);
                                            email.sendmailStaffReminder(data.MI_Id, primary_id, staffemail, "VisitorStaffApprovalReminder");
                                        }

                                        var allvisitorlist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(d => d.VMAP_Id == item.VMAP_Id).ToList();
                                        if (allvisitorlist.Count > 0)
                                        {
                                            foreach (var item1 in allvisitorlist)
                                            {
                                                if (item1.VMAPVI_VisitorContactNo != null && item1.VMAPVI_VisitorContactNo != "" && item1.VMAPVI_VisitorContactNo != "0")
                                                {
                                                    long mob1 = Convert.ToInt64(item1.VMAPVI_VisitorContactNo);
                                                    SMS sss = new SMS(_db);
                                                    string x = sss.sendSmsVisitorReminder(data.MI_Id, item.VMAP_Id, mob1, "VisitorAppointmentReminder").Result;
                                                }


                                                if (item1.VMAPVI_VisitorEmailId != null && item1.VMAPVI_VisitorEmailId != "" && item1.VMAPVI_VisitorEmailId != "0")
                                                {
                                                    Email email = new Email(_db);
                                                    email.sendmailVisitorReminder(data.MI_Id, item.VMAP_Id, item1.VMAPVI_VisitorEmailId, "VisitorAppointmentReminder", base64string);
                                                }
                                            }
                                        }

                                        string emailid = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == item.VMAP_Id).FirstOrDefault().VMAP_VisitorEmailid;
                                        if (emailid != "")
                                        {
                                            Email email = new Email(_db);
                                            email.sendmailVisitorReminder(data.MI_Id, primary_id, emailid, "VisitorAppointmentReminder", base64string);
                                        }


                                        long staffmobileno = _VisitorContext.Emp_MobileNo.Where(d => d.HRME_Id == staffid && d.HRMEMNO_DeFaultFlag == "default").FirstOrDefault().HRMEMNO_MobileNo;

                                        if (staffmobileno > 0)
                                        {
                                            SMS sss = new SMS(_db);
                                            string x = sss.sendSmsStaffReminder(data.MI_Id, primary_id, staffmobileno, "VisitorStaffApprovalReminder").Result;
                                        }
                                        long mobileno = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == item.VMAP_Id).FirstOrDefault().VMAP_VisitorContactNo;

                                        if (mobileno > 0)
                                        {
                                            SMS sss = new SMS(_db);
                                            string x = sss.sendSmsVisitorReminder(data.MI_Id, primary_id, mobileno, "VisitorAppointmentReminder").Result;
                                        }
                                       


                                    }


                                }
                            }
                        }

                        if (data.flag=="D")
                        {
                            if (item.VMAP_ReminderFlag == "Day")
                            {
                                if (item.VMAP_ReminderSchedule != null && item.VMAP_ReminderSchedule != "")
                                {

                                    string c = Convert.ToDateTime(item.VMAP_MeetingDateTime).Date.ToString("dd/MM/yyyy") + " " + item.VMAP_MeetingTiming;
                                    DateTime exfromdate = Convert.ToDateTime(c);
                                    int hr = Convert.ToInt32(item.VMAP_ReminderSchedule);
                                    DateTime newdate = exfromdate.AddDays(-hr);
                                    if (indiantime0.Date >= newdate.Date && indiantime0.Date <= exfromdate.Date)
                                    {


                                        var base64string = "";



                                        long primary_id = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == item.VMAP_Id).FirstOrDefault().VMAP_Id;

                                        long? staffid = _VisitorContext.Visitor_Management_Appointment_DMO.OrderByDescending(d => d.VMAP_Id == item.VMAP_Id).FirstOrDefault().VMAP_HRME_Id;

                                        var allvisitorlist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(d => d.VMAP_Id == item.VMAP_Id).ToList();
                                        if (allvisitorlist.Count > 0)
                                        {
                                            foreach (var item1 in allvisitorlist)
                                            {
                                                if (item1.VMAPVI_VisitorContactNo != null && item1.VMAPVI_VisitorContactNo != "" && item1.VMAPVI_VisitorContactNo != "0")
                                                {
                                                    long mob1 = Convert.ToInt64(item1.VMAPVI_VisitorContactNo);
                                                    SMS sss = new SMS(_db);
                                                    string x = sss.sendSmsVisitorReminder(data.MI_Id, item.VMAP_Id, mob1, "VisitorAppointmentReminder").Result;
                                                }


                                                if (item1.VMAPVI_VisitorEmailId != null && item1.VMAPVI_VisitorEmailId != "" && item1.VMAPVI_VisitorEmailId != "0")
                                                {
                                                    Email email = new Email(_db);
                                                    email.sendmailVisitorReminder(data.MI_Id, item.VMAP_Id, item1.VMAPVI_VisitorEmailId, "VisitorAppointmentReminder", base64string);
                                                }
                                            }
                                        }

                                        string emailid = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == item.VMAP_Id).FirstOrDefault().VMAP_VisitorEmailid;
                                        if (emailid != "")
                                        {
                                            Email email = new Email(_db);
                                            email.sendmailVisitorReminder(data.MI_Id, primary_id, emailid, "VisitorAppointmentReminder", base64string);
                                        }

                                        string staffemail = _VisitorContext.Emp_Email_Id.Where(d => d.HRME_Id == staffid && d.HRMEM_DeFaultFlag == "default").FirstOrDefault().HRMEM_EmailId;

                                        if (staffemail != "")
                                        {
                                            Email email = new Email(_db);
                                            email.sendmailStaffReminder(data.MI_Id, primary_id, staffemail, "VisitorStaffApprovalReminder");
                                        }




                                        long mobileno = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == item.VMAP_Id).FirstOrDefault().VMAP_VisitorContactNo;

                                        if (mobileno > 0)
                                        {
                                            SMS sss = new SMS(_db);
                                            string x = sss.sendSmsVisitorReminder(data.MI_Id, primary_id, mobileno, "VisitorAppointmentReminder").Result;
                                        }


                                    

                                        long staffmobileno = _VisitorContext.Emp_MobileNo.Where(d => d.HRME_Id == staffid && d.HRMEMNO_DeFaultFlag == "default").FirstOrDefault().HRMEMNO_MobileNo;

                                        if (staffmobileno > 0)
                                        {
                                            SMS sss = new SMS(_db);
                                            string x = sss.sendSmsStaffReminder(data.MI_Id, primary_id, staffmobileno, "VisitorStaffApprovalReminder").Result;
                                        }

                                       


                                    }


                                }
                            }
                        }

                       
                    }
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }


        #region  visitor sms and email
        public string sendmailVisitorMOM(long MI_Id, long UserID, string Email, string Template, string base64string, List<string> ToEmail, vmsappdocDTOq[]  files)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "no template";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VISITOR_SMS_Email_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                }

                if (base64string != null && base64string != "")
                {
                    Mailmsg = Mailmsg.Replace("[image]", base64string);
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    string Attechement = "";
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        var message = new SendGridMessage();


                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;

                        if (ToEmail.Count > 0)
                        {
                            foreach (var item in ToEmail)
                            {
                                message.AddTo(item);
                            }

                        }
                        //var fileattachment = _VisitorContext.Visitor_Management_Appointment_FilesDMO.Where(i => i.VMAP_Id == UserID).ToList();
                        //if (fileattachment.Count > 0)
                        //{
                        //    for (int i = 0; i < fileattachment.Count; i++)
                        //    {
                        //        if (fileattachment[i].VMAPVF_FilePath != null && fileattachment[i].VMAPVF_FilePath != "")
                        //        {
                        //            var webClient = new WebClient();
                        //            byte[] imageBytes = webClient.DownloadData(fileattachment[i].VMAPVF_FilePath);
                        //            string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                        //            message.AddAttachment(fileattachment[i].VMAPVF_FileName, fileContentsAsBase64, null, null, null);
                        //        }
                        //    }
                        //} 

                        if (files!=null)
                        {

                            if (files.Length > 0)
                            {
                                for (int i = 0; i < files.Length; i++)
                                {
                                    if (files[i].cfilepath != null && files[i].cfilepath != "")
                                    {
                                        var webClient = new WebClient();
                                        byte[] imageBytes = webClient.DownloadData(files[i].cfilepath);
                                        string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                        message.AddAttachment(files[i].cfilename, fileContentsAsBase64, null, null, null);
                                    }
                                }
                            }
                        }
                        


                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }


                        if (Email != null && Email != "")
                        {
                            message.AddCc(Email);
                        }

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                            eevv.Reverse();

                            if (eevv.Count > 0)
                            {
                                foreach (var item in eevv)
                                {
                                    message.AddCc(item);
                                }
                            }
                        }

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                            eevv1.Reverse();

                            if (eevv1.Count > 0)
                            {
                                foreach (var item in eevv1)
                                {
                                    message.AddBcc(item);
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {
                                if (ToEmail.Count > 0)
                                {
                                    foreach (var item in ToEmail)
                                    {
                                        emailMessage.To.Add(new MailAddress(item));
                                    }
                                }


                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }
                                if (Email != null && Email != "")
                                {

                                    emailMessage.CC.Add(Email);
                                }

                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                                    eevv.Reverse();

                                    if (eevv.Count > 0)
                                    {
                                        foreach (var item in eevv)
                                        {
                                            emailMessage.CC.Add(item);
                                        }
                                    }
                                }

                                if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                {
                                    List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                                    eevv1.Reverse();

                                    if (eevv1.Count > 0)
                                    {
                                        foreach (var item in eevv1)
                                        {
                                            emailMessage.Bcc.Add(item);
                                        }
                                    }
                                }


                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        //cmd.Parameters.Add(new SqlParameter("@To_FLag",
                        //SqlDbType.VarChar)
                        //{
                        //    Value = "Visitor Appointment"
                        //});

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                //Mails Sending end

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return "success";
        }
        #endregion





        #region  visitor sms and email
        public string sendmailVisitor(long MI_Id, long UserID, string Email, string Template, string base64string,List<string> ToEmail)
        {
            try
            {

                ToEmail = ToEmail.Distinct().ToList();
         
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "no template";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VISITOR_SMS_Email_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                }

                if (base64string != null && base64string != "")
                {
                    Mailmsg = Mailmsg.Replace("[image]", base64string);
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    string Attechement = "";
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                    
                        var message = new SendGridMessage();
                  

                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;

                        if (ToEmail.Count>0)
                        {
                            foreach (var item in ToEmail)
                            {
                                    message.AddTo(item);
                            }
                            
                        }
                        var fileattachment = _VisitorContext.Visitor_Management_Appointment_FilesDMO.Where(i => i.VMAP_Id == UserID).ToList();
                        if (fileattachment.Count > 0)
                        {
                            for (int i = 0; i < fileattachment.Count; i++)
                            {
                                if (fileattachment[i].VMAPVF_FilePath != null && fileattachment[i].VMAPVF_FilePath != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(fileattachment[i].VMAPVF_FilePath);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(fileattachment[i].VMAPVF_FileName, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }


                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }
                         

                        if (Email!=null && Email!="")
                        {
                            if (!ToEmail.Contains(Email))
                            {
                                message.AddCc(Email);
                            }
                           
                        }

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                            eevv.Reverse();

                            if (eevv.Count>0)
                            {
                                foreach (var item in eevv)
                                {
                                    message.AddCc(item);
                                }
                            }
                        }

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                            eevv1.Reverse();

                            if (eevv1.Count > 0)
                            {
                                foreach (var item in eevv1)
                                {
                                    message.AddBcc(item);
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;
                       
                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";
                    
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {
                                if (ToEmail.Count>0)
                                {
                                    foreach (var item in ToEmail)
                                    {
                                        
                                            emailMessage.To.Add(new MailAddress(item));
                                      
                                       
                                    }
                                }

                            
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }
                               

                                if (Email != null && Email != "")
                                {
                                    if (!ToEmail.Contains(Email))
                                    {
                                        emailMessage.CC.Add(Email);
                                    }
                                }

                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                                    eevv.Reverse();

                                    if (eevv.Count > 0)
                                    {
                                        foreach (var item in eevv)
                                        {
                                            emailMessage.CC.Add(item);
                                        }
                                    }
                                }

                                if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                {
                                    List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                                    eevv1.Reverse();

                                    if (eevv1.Count > 0)
                                    {
                                        foreach (var item in eevv1)
                                        {
                                            emailMessage.Bcc.Add(item);
                                        }
                                    }
                                }

                              
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        //cmd.Parameters.Add(new SqlParameter("@To_FLag",
                        //SqlDbType.VarChar)
                        //{
                        //    Value = "Visitor Appointment"
                        //});

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                //Mails Sending end

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return "success";
        }
        #endregion







        public string notify(long AppointmentId,long UserId)
        {
            try
            {
                string deviceidsnew = "";
                string devicenew = "";
                string sound = "";
                var devicelist = (from a in _VisitorContext.Visitor_Management_Appointment_DMO
                                  from e in _VisitorContext.MasterEmployee
                                  where (a.VMAP_HRME_Id == e.HRME_Id  && e.HRME_AppDownloadedDeviceId !="" &&  e.HRME_AppDownloadedDeviceId != null && a.VMAP_Id== AppointmentId)
                                  select new AppointmentApprovalStatus_DTO
                                  {
                                      HRME_Id = e.HRME_Id,
                                      HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                  }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                if (devicelist.Count > 0)
                {
                    string titletext = "Appointment Status";
                    string Body = "";
                    int k = 0;
                    foreach (var deviceid in devicelist)
                    {
                        if (k == 0)
                        {
                            deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                            k = 1;
                        }
                        else
                        {
                            deviceidsnew = deviceidsnew + "," + '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                        }
                    }
                    devicenew = "[" + deviceidsnew + "]";


                    var apointmentdetails = _VisitorContext.Visitor_Management_Appointment_DMO.Where(w=>w.VMAP_Id== AppointmentId).ToList();
                    if (apointmentdetails.Count>0)
                    {
                        
                        if (apointmentdetails[0].VMAP_Status== "Approved" || apointmentdetails[0].VMAP_Status == "RE-SCHEDULE")
                        {
                            var date1 = Convert.ToDateTime(apointmentdetails[0].VMAP_MeetingDateTime).Date.ToString("dd/MM/yyyy");

                            DateTime dateTime = DateTime.ParseExact(apointmentdetails[0].VMAP_MeetingFromTime, "HH:mm", CultureInfo.InvariantCulture);
                            string entrydatetime = dateTime.ToString("hh:mm tt");

                            Body = "Appointment is confirmed at " + " " + entrydatetime + " " + "on" + " " + date1;
                        }
                        else if (apointmentdetails[0].VMAP_Status == "Canceled")
                        {
                            var date1 = Convert.ToDateTime(apointmentdetails[0].VMAP_MeetingDateTime).Date.ToString();

                            DateTime dateTime = DateTime.ParseExact(apointmentdetails[0].VMAP_MeetingFromTime, "HH:mm", CultureInfo.InvariantCulture);
                            string entrydatetime = dateTime.ToString("hh:mm tt");

                            Body = "Appointment is Cancelled";
                        }
                        else if (apointmentdetails[0].VMAP_Status == "Rejected")
                        {
                            var date1 = Convert.ToDateTime(apointmentdetails[0].VMAP_MeetingDateTime).Date.ToString();

                            DateTime dateTime = DateTime.ParseExact(apointmentdetails[0].VMAP_MeetingFromTime, "HH:mm", CultureInfo.InvariantCulture);
                            string entrydatetime = dateTime.ToString("hh:mm tt");

                            Body = "Appointment is Rejected";
                        }


                        if (sound == "")
                        {
                            sound = "default";
                        }
                        long hrid = Convert.ToInt64(apointmentdetails[0].VMAP_HRME_Id);
                        callnotification(devicenew, hrid, apointmentdetails[0].VMAP_Id, titletext, apointmentdetails[0].MI_Id,sound,Body);
                       //Insertnotification(apointmentdetails[0].MI_Id, hrid, apointmentdetails[0].VMAP_Id, Body, UserId, "APPOINTMENT");

                    }

                  


                }




            }
            catch (Exception ex)
            {

                throw ex;
            }

            return "";
        }

        public string callnotification(string devicenew, long empid, long task_id, string titletext, long mi_id, string sound,string body)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + titletext + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + body + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";

                notificationparams.Add(daata.ToString());
                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=AAAADrksgbk:APA91bGjurLMMB23AWc8SklzSksUUMoFt6zA_XY2TMkk0BxzDYFIkYuKNpNlhtYdVIWiQ8zjsQxXIlGdWI-Zrqb9UHhNpJf9DMM7qtAFxxgZPWbhenI4KWsnpZaaeWtM6O2qR_vIHXqS";
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        //public string Insertnotification(long MI_Id, long empid, long task_id, string titletext, long userid, string type)
        //{
        //    try
        //    {
        //        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

        //        //ISM_PlannerCreationDTO data = new ISM_PlannerCreationDTO();
        //        ISM_NotificationsDMO push = new ISM_NotificationsDMO();
        //        push.MI_Id = MI_Id;
        //        push.HRME_Id = empid;
        //        push.ISMNO_Notification = titletext;
        //        push.ISMNO_NoticationDate = indianTime;
        //        push.ISMNO_NotificationType = type;
        //        push.ISMNO_ReadFlg = false;
        //        push.ISMNO_MakeUnReadFlg = false;
        //        push.ISMNO_ActiveFlag = true;
        //        push.ISMNO_CreatedBy = userid;
        //        push.ISMNO_UpdatedBy = userid;
        //        push.CreatedDate = indianTime;
        //        push.UpdatedDate = indianTime;
        //        _VisitorContext.Add(push);
        //        var contactExists = _VisitorContext.SaveChanges();
        //        if (contactExists > 0)
        //        {
        //          //  data.returnval = true;
        //        }
        //        else
        //        {
        //           // data.returnval = false;
        //        }
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
