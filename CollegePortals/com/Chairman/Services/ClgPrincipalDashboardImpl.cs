using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals.Chairman;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePortals.com.Chairman.Services
{
    public class ClgPrincipalDashboardImpl:Interfaces.ClgPrincipalDashboardInterface
    {
        private static ConcurrentDictionary<string, clgChairmanDashboardDTO> _login =
          new ConcurrentDictionary<string, clgChairmanDashboardDTO>();
        private CollegeportalContext _PrincipalDashboardContext;      
        public PortalContext _PortalContext;      

        public ClgPrincipalDashboardImpl(CollegeportalContext ClgPortalContext, PortalContext _Portal)
        {
            _PrincipalDashboardContext = ClgPortalContext;
            _PortalContext = _Portal;
        }
        public async Task<clgChairmanDashboardDTO> Getdetails(clgChairmanDashboardDTO data)
        {
            try
            {

                data.Smscount = _PrincipalDashboardContext.IVRM_sms_sentBoxDMO.Where(t => t.MI_Id == data.MI_Id && t.Module_Name == "PRINCIPAL DASHBOARD").ToArray();
                data.Emailcount = _PrincipalDashboardContext.IVRM_Email_sentBoxDMO.Where(t => t.MI_Id == data.MI_Id && t.Module_Name == "PRINCIPAL DASHBOARD").ToArray();


                data.notification = (from a in _PrincipalDashboardContext.Adm_Students_Certificate_Apply_DMO
                                     from b in _PrincipalDashboardContext.Adm_Master_College_StudentDMO
                                     from c in _PrincipalDashboardContext.Adm_College_Yearly_StudentDMO
                                     where (a.AMST_Id == b.AMCST_Id && b.AMCST_Id == c.AMCST_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ASCA_Status == "Pending" && a.ASCA_ActiveFlg == true)
                                     select new TransferCertificate_DTO
                                     {
                                         ASCA_Id = a.ASCA_Id,
                                         AMST_Id = a.AMST_Id,
                                         AMST_FirstName = ((b.AMCST_FirstName == null ? "" : b.AMCST_FirstName.ToUpper()) + " " + (b.AMCST_MiddleName == null ? "" : b.AMCST_MiddleName.ToUpper()) + " " + (b.AMCST_LastName == null ? "" : b.AMCST_LastName.ToUpper())).Trim(),
                                         ASCA_CertificateType = a.ASCA_CertificateType,
                                         ASCA_Reason = a.ASCA_Reason,
                                         ASCA_ApplyDate = a.ASCA_ApplyDate,
                                         ASCA_Status = a.ASCA_Status,
                                         ASCA_ActiveFlg = a.ASCA_ActiveFlg
                                     }).Distinct().OrderByDescending(m => m.ASCA_ApplyDate).ToArray();

                data.leavenotification = (from a in _PrincipalDashboardContext.Adm_Students_Leave_Apply_DMO
                                          from b in _PrincipalDashboardContext.Adm_Master_College_StudentDMO
                                          from c in _PrincipalDashboardContext.Adm_College_Yearly_StudentDMO
                                          where (a.AMST_Id == b.AMCST_Id && b.AMCST_Id == c.AMCST_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ASLA_Status == "Pending" && a.ASLA_ActiveFlag == true)
                                          select new OnlineLeaveApp_DTO
                                          {
                                              ASLA_Id = a.ASLA_Id,
                                              AMST_Id = a.AMST_Id,
                                              AMST_FirstName = ((b.AMCST_FirstName == null ? "" : b.AMCST_FirstName.ToUpper()) + " " + (b.AMCST_MiddleName == null ? "" : b.AMCST_MiddleName.ToUpper()) + " " + (b.AMCST_LastName == null ? "" : b.AMCST_LastName.ToUpper())).Trim(),
                                              ASLA_Flag = a.ASLA_Flag,
                                              ASLA_Reason = a.ASLA_Reason,
                                              ASLA_ApplyDate = a.ASLA_ApplyDate,
                                              ASLA_FromDate = a.ASLA_FromDate,
                                              ASLA_ToDate = a.ASLA_ToDate,
                                              ASLA_ApprovedFromDate = a.ASLA_ApprovedFromDate,
                                              ASLA_ApprovedToDate = a.ASLA_ApprovedToDate,
                                              ASLA_Status = a.ASLA_Status,
                                              ASAPCS_ActiveFlg = a.ASLA_ActiveFlag
                                          }).Distinct().OrderByDescending(m => m.ASLA_ApplyDate).ToArray();

                List<clgChairmanDashboardDTO> result = new List<clgChairmanDashboardDTO>();
                List<clgChairmanDashboardDTO> result1 = new List<clgChairmanDashboardDTO>();
                List<MasterAcademic> list = new List<MasterAcademic>();

                list = _PrincipalDashboardContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.yearlist = list.ToArray();

                data.CurrentAcademicYear = _PrincipalDashboardContext.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).ToArray();

                if (data.ASMAY_Id > 0)
                {
                    data.Fillstudentstrenth = (from a in _PrincipalDashboardContext.Adm_College_Yearly_StudentDMO
                                               from b in _PrincipalDashboardContext.ClgMasterBranchDMO
                                               from c in _PrincipalDashboardContext.Adm_Master_College_StudentDMO
                                               where (a.AMCST_Id == c.AMCST_Id && a.AMB_Id == b.AMB_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMCST_SOL.Equals("S") && c.AMCST_ActiveFlag == true && a.ACYST_ActiveFlag == 1)
                                               select new
                                               {
                                                   Class_Name = b.AMB_BranchName,
                                                   ASMCL_Order = b.AMB_Order,
                                                   stud_count = a.AMCST_Id,
                                               }).Distinct().GroupBy(id => new { id.Class_Name, id.ASMCL_Order }).Select(g => new clgChairmanDashboardDTO { Class_Name = g.Key.Class_Name, ASMCL_Order = g.Key.ASMCL_Order, stud_count = g.Count() }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();


                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_PRINCIPAL_STAFF_BIRTHDAYLIST";
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
                            data.staffbrthlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_PRINCIPAL_STUDENT_TODAY_ABSENT_College";
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
                            data.stdabsentlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }


                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
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
                                    result.Add(new clgChairmanDashboardDTO
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

                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Get_Classwise_fee_collection_college";
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.CommandTimeout = 800000000;
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
                                    result1.Add(new clgChairmanDashboardDTO
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
                    data.coedata = (from m in _PrincipalDashboardContext.COE_Master_EventsDMO
                                    from n in _PrincipalDashboardContext.COE_EventsDMO
                                    where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == A
                                    select new clgChairmanDashboardDTO
                                    {
                                        eventName = m.COEME_EventName,
                                        eventDesc = m.COEME_EventDesc,
                                        COEE_EStartDate = n.COEE_EStartDate,
                                        COEE_EEndDate = n.COEE_EEndDate,
                                    }).ToArray();
                }

                #region Payment Notfication
                if (data.PaymentNootificationCollegePrinicipal == 0)
                {
                    SubscriptionPaymentNotification _sub = new SubscriptionPaymentNotification(_PortalContext);
                    data.getpaymentnotificationdetails = _sub.getnotificationdetails(data.MI_Id, data.UserId);
                }
                #endregion

                //STAFF BIRTH DAY 

                //data.staffbrthlist = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO
                //                  where (a.MI_Id == data.MI_Id  && a.HRME_DOB.Value.Date.Day == DateTime.Now.Day && a.HRME_DOB.Value.Date.Month == DateTime.Now.Month && a.HRME_LeftFlag == false && a.HRME_ActiveFlag == true)
                //                  //group a by a.HRME_Id into g
                //                  select new clgChairmanDashboardDTO
                //                  {
                //                      HRME_Id = a.HRME_Id,
                //                      employeeName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),

                //                  }
                //                ).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
