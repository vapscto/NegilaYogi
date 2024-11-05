using PreadmissionDTOs.com.vaps.College.Portals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using DomainModel.Model.com.vapstech.Portals.Employee;
using CommonLibrary;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;

namespace CollegePortals.com.Chairman.Services
{
    public class clgChairmanDashboardImpl : Interfaces.clgChairmanDashboardInterface
    {
        private static ConcurrentDictionary<string, clgChairmanDashboardDTO> _login =
           new ConcurrentDictionary<string, clgChairmanDashboardDTO>();
        private CollegeportalContext _ClgPortalContext;
        private PortalContext _PortalContext;
        public clgChairmanDashboardImpl(CollegeportalContext ClgPortalContext, PortalContext _Portal)
        {
            _ClgPortalContext = ClgPortalContext;
            _PortalContext = _Portal;
        }
        public async Task<clgChairmanDashboardDTO> Getdetails(clgChairmanDashboardDTO data)
        {
            try
            {

                #region ACADEMIC YEAR
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _ClgPortalContext.academicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).ToList();
                data.yearlist = list.ToArray();
                #endregion

                #region EMPLOYEE STRENGTH
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_CHAIRMAN_PORTAL_EMPLOYEE_STRENGTH";
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
                        data.fillabsent = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                #endregion

                #region STUDENT STRENGTH
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_CHAIRMANPORTAL_COURSEWISE_STD_STRENGTH";
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
                        data.Fillstudentstrenth = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                #endregion

                #region COURSE WISE FEE DETAILS
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_CHAIRMAN_COURSEWISE_FEEDETALIS";
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
                        data.fillfee = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                #endregion

                #region COE EVENTS and Calendar Details
                //============================= COE Current Month Events
                int month = DateTime.Now.Month;
                data.coereportlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                                      from n in _ClgPortalContext.COE_EventsDMO
                                      from o in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                      where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && o.ASMAY_Id == data.ASMAY_Id  && n.COEE_EStartDate.Value.Month == month
                                      select new clgChairmanDashboardDTO
                                      {
                                          COEME_Id = m.COEME_Id,
                                          COEME_EventName = m.COEME_EventName,
                                          COEME_EventDesc = m.COEME_EventDesc,
                                          COEE_EStartDate = n.COEE_EStartDate,
                                          COEE_EEndDate = n.COEE_EEndDate,
                                          COEE_ReminderDate=n.COEE_ReminderDate,
                                          ASMAY_Id = o.ASMAY_Id,
                                      }).Distinct().OrderBy(c => c.COEME_Id).ToArray();

                //============================ COE All Years Events
                data.calenderlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                                     from n in _ClgPortalContext.COE_EventsDMO
                                     from o in _ClgPortalContext.Adm_Master_College_StudentDMO
                                     where (m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.COEE_EStartDate != null)
                                     select new clgChairmanDashboardDTO
                                     {
                                         COEME_EventName = m.COEME_EventName,
                                         COEME_EventDesc = m.COEME_EventDesc,
                                         COEE_EStartDate = n.COEE_EStartDate,
                                         COEE_EEndDate = n.COEE_EEndDate,
                                         COEE_ReminderDate = n.COEE_ReminderDate
                                     }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();
                #endregion

                #region Payment Notfication
                //if (data.PaymentNootificationCollegeChairman == 0)
                //{
                //    SubscriptionPaymentNotification _sub = new SubscriptionPaymentNotification(_PortalContext);
                //    data.getpaymentnotificationdetails = _sub.getnotificationdetails(data.MI_Id, data.UserId);
                //}
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
