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
    public class Ch_EmployeeSalaryDetailsImpl : Interfaces.Ch_EmployeeSalaryDetailsInterface 
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<Ch_EmployeeSalaryDetailsImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public Ch_EmployeeSalaryDetailsImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }

        public Emp_salaryDTO getdata(Emp_salaryDTO dto)
        {
            try
            {
                //dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.userid && c.MI_Id == dto.MI_Id).Emp_Code;

                //dto.yearlist = _ChairmanDashboardContext.HR_MasterLeaveYear.Where(t => t.MI_Id == dto.MI_Id && t.HRMLY_ActiveFlag==true).ToArray();
                var year = _ChairmanDashboardContext.HR_MasterLeaveYear.Where(t => t.MI_Id == dto.MI_Id && t.HRMLY_ActiveFlag==true).ToList();
                dto.yearlist = year.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();
                List<Emp_salaryDTO> result2 = new List<Emp_salaryDTO>();
                if (dto.HRMLY_LeaveYear == null|| dto.HRMLY_LeaveYear=="")
                {
                    var HRMLY_Id = year[0].HRMLY_Id;
                    var HRMLY_LeaveYear= year[0].HRMLY_LeaveYear;
                    dto.HRMLY_Id = HRMLY_Id;
                    dto.HRMLY_LeaveYear = HRMLY_LeaveYear;
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chairman_yearwise_salary";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                      SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
                      SqlDbType.VarChar,50)
                    {
                        Value = dto.HRMLY_LeaveYear.Trim()
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
                                result2.Add(new Emp_salaryDTO
                                {
                                    monthName = dataReader["month1"].ToString(),
                                salary =Convert.ToDecimal(dataReader["netamount"])

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

       

        public Emp_salaryDTO onmonth(Emp_salaryDTO dto)
        {
            try
            {
                List<Emp_salaryDTO> result2 = new List<Emp_salaryDTO>();
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
                                result2.Add(new Emp_salaryDTO
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
