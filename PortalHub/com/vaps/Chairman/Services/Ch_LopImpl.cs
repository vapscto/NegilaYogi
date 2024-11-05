

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

namespace PortalHub.com.vaps.Chairman.Services
{
    public class Ch_LopImpl : Interfaces.Ch_LopInterface 
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<Ch_LopImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public Ch_LopImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }

        public Ch_LopDTO getdata(Ch_LopDTO dto)
        {
            try
            {
               
                var year = _ChairmanDashboardContext.HR_MasterLeaveYear.Where(t => t.MI_Id == dto.MI_Id).OrderBy(t => t.HRMLY_LeaveYearOrder).ToList();
                dto.yearlist = year.ToArray();
                List<Ch_LopDTO> result2 = new List<Ch_LopDTO>();
                if (dto.HRMLY_LeaveYear == null|| dto.HRMLY_LeaveYear=="")
                {
                    var HRMLY_Id = year[0].HRMLY_Id;
                    var HRMLY_LeaveYear= year[0].HRMLY_LeaveYear;
                    dto.HRMLY_Id = HRMLY_Id;
                    dto.HRMLY_LeaveYear = HRMLY_LeaveYear;
                }
                //dto.HRMLY_Id = 1;
                //dto.MI_Id = 5;
                //dto.monthName = "October";
                var month = _ChairmanDashboardContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToList();
                dto.fillmonths = month.ToArray();
               
                      if (dto.monthName == null || dto.monthName == "")
                {
                    var IVRM_Month_Id = month[0].IVRM_Month_Id;
                    var IVRM_Month_Name = month[0].IVRM_Month_Name;
                    dto.IVRM_Month_Id = IVRM_Month_Id;
                    dto.monthName = IVRM_Month_Name;
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chairman_Lop_details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.HRMLY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MONTH",
                      SqlDbType.VarChar,50)
                    {
                        Value = dto.monthName.Trim()
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
                                result2.Add(new Ch_LopDTO
                                {
                                    empname = dataReader["name"].ToString(),
                                    departmentname = (dataReader["HRMD_DepartmentName"]).ToString(),
                                    designationname = (dataReader["HRMDES_DesignationName"]).ToString(),
                                    lop = Convert.ToDouble(dataReader["HRELTD_TotDays"]),

                                    frmdate = Convert.ToDateTime(dataReader["HRELTD_FromDate"]),
                                    todate = Convert.ToDateTime(dataReader["HRELTD_ToDate"]),

                                });
                                dto.salarylist = result2.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

       

        public Ch_LopDTO onmonth(Ch_LopDTO dto)
        {
            try
            {
                List<Ch_LopDTO> result2 = new List<Ch_LopDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chairman_Departmentwise_salary";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                      SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
                      SqlDbType.VarChar, 50)
                    {
                        Value = dto.HRMLY_LeaveYear.Trim()
                    });
                    cmd.Parameters.Add(new SqlParameter("@Month",
                     SqlDbType.VarChar, 50)
                    {
                        Value = dto.monthName.Trim()
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
                                result2.Add(new Ch_LopDTO
                                {
                                    departmentname = dataReader["dept"].ToString(),
                                    salary = Convert.ToDecimal(dataReader["netamount"])

                                });
                                dto.deptsalary = result2.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }return dto;
        }
    }
}
