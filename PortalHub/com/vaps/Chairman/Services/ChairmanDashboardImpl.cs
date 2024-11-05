

using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace PortalHub.com.vaps.Chairman.Services
{
    public class ChairmanDashboardImpl : Interfaces.ChairmanDashboardInterface
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<ChairmanDashboardImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public ChairmanDashboardImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }

        public ChairmanDashboardDTO Getdetails(ChairmanDashboardDTO data)//int IVRMM_Id
        {
            //ChairmanDashboardDTO getdata = new ChairmanDashboardDTO();
            try
            {


                data.Smscount = _db.IVRM_sms_sentBoxDMO.Where(t => t.MI_Id == data.MI_Id && t.Module_Name == "PRINCIPAL DASHBOARD").ToArray();
                data.Emailcount = _db.IVRM_Email_sentBoxDMO.Where(t => t.MI_Id == data.MI_Id && t.Module_Name == "PRINCIPAL DASHBOARD").ToArray();

                List<ChairmanDashboardDTO> result = new List<ChairmanDashboardDTO>();
                List<ChairmanDashboardDTO> result1 = new List<ChairmanDashboardDTO>();
                List<ChairmanDashboardDTO> result2 = new List<ChairmanDashboardDTO>();



                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).ToList();
                data.yearlist = list.ToArray();

                var emp_Id = _db.Staff_User_Login.Where(c => c.Id == data.Userid && c.MI_Id == data.MI_Id).Distinct().ToList();
                if (emp_Id.Count > 0)
                {
                    data.HRME_Id = emp_Id.FirstOrDefault().Emp_Code;

                    data.employeedetails = (from a in _db.HR_Master_Employee_DMO
                                            from b in _db.HR_Master_Department
                                            from c in _db.HR_Master_Designation

                                            where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id)
                                            select new EmployeeDashboardDTO
                                            {
                                                HRME_Id = a.HRME_Id,
                                                HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),

                                                HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                                HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                                HRME_DOJ = a.HRME_DOJ,
                                                HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                HRMDES_DesignationName = c.HRMDES_DesignationName,
                                                HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                HRME_DOB = a.HRME_DOB,
                                                HRME_PhotoNo = a.HRME_Photo,
                                                DeviceID = (a.HRME_AppDownloadedDeviceId == null ? " " : a.HRME_AppDownloadedDeviceId)
                                            }).Distinct().ToArray();

                    data.mobile = (from a in _db.Multiple_Mobile_DMO
                                  where (a.HRME_Id == data.HRME_Id && a.HRMEMNO_DeFaultFlag == "default")
                                  select new EmployeeDashboardDTO
                                  {
                                      HRME_MobileNo = a.HRMEMNO_MobileNo,
                                  }).Distinct().ToArray();


                    data.email = (from a in _db.Multiple_Email_DMO

                                 where (a.HRME_Id == data.HRME_Id && a.HRMEM_DeFaultFlag == "default")
                                 select new EmployeeDashboardDTO
                                 {
                                     HRME_EmailId = a.HRMEM_EmailId,
                                 }).Distinct().ToArray();
                }


                //data.Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                //                           from b in _db.admissioncls
                //                           from c in _db.Adm_M_Student
                //                           from d in _db.AcademicYear


                //                           where (a.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && b.MI_Id == c.MI_Id && c.AMST_SOL=="S")
                //                           select new
                //                           {
                //                               Class_Name = b.ASMCL_ClassName,
                //                               stud_count = a.AMST_Id
                //                           }).Distinct().GroupBy(id => id.Class_Name).Select(g => new ChairmanDashboardDTO { Class_Name = g.Key, stud_count = g.Count() }).ToArray();


                //data.Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                //                           from b in _db.admissioncls
                //                           from c in _db.Adm_M_Student
                //                           where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1)
                //                           select new
                //                           {
                //                               Class_Name = b.ASMCL_ClassName,
                //                               stud_count = a.AMST_Id
                //                           }).Distinct().GroupBy(id => id.Class_Name).Select(g => new ChairmanDashboardDTO { Class_Name = g.Key, stud_count = g.Count() }).ToArray();


                data.Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                                           from b in _db.admissioncls
                                           from c in _db.Adm_M_Student
                                           where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1)
                                           select new
                                           {
                                               Class_Name = b.ASMCL_ClassName,
                                               ASMCL_Order = b.ASMCL_Order,
                                               stud_count = a.AMST_Id,
                                           }).Distinct().GroupBy(id => new { id.Class_Name, id.ASMCL_Order }).Select(g => new ChairmanDashboardDTO { Class_Name = g.Key.Class_Name, ASMCL_Order = g.Key.ASMCL_Order, stud_count = g.Count() }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();




                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_No_of_Absent";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                                result.Add(new ChairmanDashboardDTO
                                {
                                    NameOfDesig = dataReader["NameOfDesig"].ToString(),
                                    absentee = int.Parse(dataReader["absentee"].ToString())

                                });
                                data.fillabsent = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }



                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_Classwise_fee_collaction";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 400000;

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






                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();


                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result1.Add(new ChairmanDashboardDTO
                                {
                                    paid = Convert.ToDecimal(dataReader["callected"].ToString()),
                                    ballance = Convert.ToDecimal(dataReader["ballance"].ToString()),
                                    recived = Convert.ToDecimal(dataReader["receivable"].ToString()),
                                    feeclass = dataReader["class"].ToString()


                                });
                                data.fillfee = result1.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }



                DateTime now = DateTime.Now;
                int A = now.Month;
                data.coedata = (from m in _ChairmanDashboardContext.COE_Master_EventsDMO
                                from n in _ChairmanDashboardContext.COE_EventsDMO
                                where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == A
                                select new ChairmanDashboardDTO
                                {
                                    eventName = m.COEME_EventName,
                                    eventDesc = m.COEME_EventDesc,
                                    COEE_EStartDate = n.COEE_EStartDate,
                                    COEE_EEndDate = n.COEE_EEndDate
                                }).Distinct().OrderBy(e => e.COEE_EStartDate).ToArray();


                #region Payment Notfication
                //if (data.PaymentNootificationChairman == 0)
                //{
                //    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                //    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                //    var checkdata = _ChairmanDashboardContext.IVRM_Payment_Subscription_RemarksDetilsDMO.Where(a => a.UserId == data.Userid
                //    && a.IVRMPSLR_LoginDatetime.Value.Date == indiantime0.Date && a.MI_Id == data.MI_Id).ToList();

                //    if (checkdata.Count == 0)
                //    {
                //        SubscriptionPaymentNotification _sub = new SubscriptionPaymentNotification(_ChairmanDashboardContext);
                //        data.getpaymentnotificationdetails = _sub.getnotificationdetails(data.MI_Id, data.Userid);
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }











    }
}
