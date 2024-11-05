using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeSalaryDetailsImpl : Interfaces.EmployeeSalaryDetailsInterface
    {
        public FeeGroupContext _fees;
        public HRMSContext _hrms;
        public ExamContext _exm;
        public EmployeeSalaryDetailsImpl(HRMSContext hrms, FeeGroupContext fees, ExamContext exm)
        {
            _hrms = hrms;
            _fees = fees;
            _exm = exm;

        }

        public EmployeeDashboardDTO getdata(EmployeeDashboardDTO dto)
        {
            try
            {
                //dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.userid && c.MI_Id == dto.MI_Id).Emp_Code;

                dto.yearlist = _hrms.HR_MasterLeaveYear.Where(t => t.MI_Id == dto.MI_Id && t.HRMLY_ActiveFlag == true).OrderBy(t=>t.HRMLY_LeaveYearOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public EmployeeDashboardDTO getdaily_data(EmployeeDashboardDTO dto)
        {
            try
            {

                if (dto.HRME_Id>0)
                {
                    dto.HRME_Id = dto.HRME_Id;
                }
                else
                {
                    dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }

                

                //dto.salarylist = (from m in _hrms.HR_Employee_Salary
                //                  from n in _hrms.HR_Employee_Salary_Details
                //                  from o in _hrms.HR_Master_EarningsDeductions
                //                  from p in _hrms.Month
                //                  where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == dto.MI_Id && m.HRME_Id == dto.HRME_Id && o.HRMED_EarnDedFlag.Equals("Earning") && m.HRES_Year == dto.HRMLY_LeaveYear && m.HRES_Month == p.IVRM_Month_Name
                //                  group new { m, n, o, p }
                //                    by new { p.IVRM_Month_Name, m.HRES_Year } into g
                //                  select new EmployeeDashboardDTO
                //                  {

                //                      salary = g.Sum(d => d.n.HRESD_Amount),
                //                      IVRM_Month_Id = g.FirstOrDefault().p.IVRM_Month_Id,
                //                      monthName = g.FirstOrDefault().p.IVRM_Month_Name,
                //                      hres_id = g.FirstOrDefault().m.HRES_Id,
                //                      HRES_Year = g.FirstOrDefault().m.HRES_Year,
                //                  }
                //                ).OrderBy(t => t.IVRM_Month_Id).ToArray();


                using (var cmd = _hrms.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_SalaryList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRES_Year",
                   SqlDbType.VarChar)
                    {
                        Value = dto.HRMLY_LeaveYear
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();


                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.salarylist = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                dto.salaryEarningDlist = (from m in _hrms.HR_Employee_Salary
                                          from n in _hrms.HR_Employee_Salary_Details
                                          from o in _hrms.HR_Master_EarningsDeductions
                                          from p in _hrms.Month
                                          where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == dto.MI_Id && m.HRME_Id == dto.HRME_Id && o.HRMED_EarnDedFlag.Equals("Deduction") && m.HRES_Year == dto.HRMLY_LeaveYear && m.HRES_Month == p.IVRM_Month_Name
                                          group new { m, n, o, p }
                                            by new { p.IVRM_Month_Name, m.HRES_Year } into g
                                          select new EmployeeDashboardDTO
                                          {
                                              salary = g.Sum(d => d.n.HRESD_Amount),
                                              IVRM_Month_Id = g.FirstOrDefault().p.IVRM_Month_Id,
                                              monthName = g.FirstOrDefault().p.IVRM_Month_Name,
                                              hres_id = g.FirstOrDefault().m.HRES_Id,
                                              HRES_Year = g.FirstOrDefault().m.HRES_Year,
                                          }
                                ).OrderBy(t => t.IVRM_Month_Id).ToArray();


                using (var cmd = _hrms.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_SalarySlip_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                       SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRELY_Year",
                   SqlDbType.VarChar)
                    {
                        Value = dto.HRMLY_LeaveYear
                    });                   
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();


                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.salaryDetailslist = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public EmployeeDashboardDTO getsalaryalldetails(EmployeeDashboardDTO dto)
        {
            try
            {

                if (dto.HRME_Id > 0)
                {
                    dto.HRME_Id = dto.HRME_Id;
                }
                else
                {
                    dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }

                if (dto.HRES_Id == 0)
                {
                    dto.HRES_Id = _hrms.HR_Employee_Salary.Where(t => t.HRME_Id == dto.HRME_Id && t.HRES_Year == dto.HRES_Year.ToString() && t.HRES_Month == dto.HRES_Month).Select(t => t.HRES_Id).FirstOrDefault();

                }


                var EQuery = _hrms.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_EarnDedFlag.Equals("Earning")).Select(d => d.HRMED_Id).ToList();
                dto.TotalEarning = (from m in _hrms.HR_Employee_Salary
                                    from n in _hrms.HR_Employee_Salary_Details

                                    where m.HRES_Id == n.HRES_Id && EQuery.Contains(n.HRMED_Id) && m.MI_Id == dto.MI_Id && m.HRME_Id == dto.HRME_Id && m.HRES_Id == dto.HRES_Id
                                    group new { m, n }
                                     by new { m.HRES_Month } into g
                                    select new EmployeeDashboardDTO
                                    {
                                        hres_id = g.FirstOrDefault().m.HRES_Id,
                                        salary = g.Sum(d => d.n.HRESD_Amount),

                                    }
                                 ).OrderBy(m => m.hres_id).ToArray();
                var DQuery = _hrms.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_EarnDedFlag.Equals("Deduction")).Select(d => d.HRMED_Id).ToList();
                dto.totalDeduction = (from m in _hrms.HR_Employee_Salary
                                      from n in _hrms.HR_Employee_Salary_Details
                                      where m.HRES_Id == n.HRES_Id && DQuery.Contains(n.HRMED_Id) && m.MI_Id == dto.MI_Id && m.HRME_Id == dto.HRME_Id && m.HRES_Id == dto.HRES_Id
                                      group new { m, n }
                                       by new { m.HRES_Month } into g
                                      select new EmployeeDashboardDTO
                                      {
                                          hres_id = g.FirstOrDefault().m.HRES_Id,
                                          salary = g.Sum(d => d.n.HRESD_Amount),
                                      }
                                 ).OrderBy(m => m.hres_id).ToArray();
                dto.salarylistE = (from m in _hrms.HR_Employee_Salary
                                   from n in _hrms.HR_Employee_Salary_Details
                                   from o in _hrms.HR_Master_EarningsDeductions
                                   where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == dto.MI_Id && m.HRME_Id == dto.HRME_Id && m.HRES_Id == dto.HRES_Id && o.HRMED_EarnDedFlag.Equals("Earning")
                                   select new EmployeeDashboardDTO
                                   {
                                       hrmed_Name = o.HRMED_Name,
                                       hrmed_Amount = n.HRESD_Amount,
                                   }
                                 ).ToArray();
                dto.salarylistD = (from m in _hrms.HR_Employee_Salary
                                   from n in _hrms.HR_Employee_Salary_Details
                                   from o in _hrms.HR_Master_EarningsDeductions
                                   where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == dto.MI_Id && m.HRME_Id == dto.HRME_Id && m.HRES_Id == dto.HRES_Id && o.HRMED_EarnDedFlag.Equals("Deduction")

                                   select new EmployeeDashboardDTO
                                   {
                                       hrmed_Name = o.HRMED_Name,
                                       hrmed_Amount = n.HRESD_Amount,
                                   }
                                ).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }



    }
}
