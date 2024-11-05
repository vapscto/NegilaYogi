using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.MobileApp;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.Portals.Student;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeePtalKIOSKImpl : Interfaces.EmployeePtalKIOSKInterface
    {
        public HRMSContext _hrms;
        public TTContext _tt;
        public ExamContext _exm;
        public FOContext _FOContext;
        public PortalContext _PortalContext;
        public EmployeePtalKIOSKImpl(HRMSContext hrms, TTContext tt, FOContext fOContext, ExamContext exm, PortalContext portalContext)
        {
            _hrms = hrms;
            _tt = tt;
            _FOContext = fOContext;
            _exm = exm;
            _PortalContext = portalContext;
        }

        public async Task<EmployeeKioskLEAVEDTO> getleave_report(EmployeeKioskLEAVEDTO data)
        {
            try
            {
                if (data.selectedEmployee != "")
                {
                    List<EmployeeKioskLEAVEDTO> result = new List<EmployeeKioskLEAVEDTO>();
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Employees_Bal_Leaves";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Fromdate",SqlDbType.VarChar)
                        {
                            Value = data.HRELTD_FromDate.ToString("dd-MM-yyyy")
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",SqlDbType.VarChar)
                        {
                            Value = data.HRELT_ToDate.ToString("dd-MM-yyyy")
                        });
                        cmd.Parameters.Add(new SqlParameter("@Leaveid",SqlDbType.VarChar)
                        {
                            Value = data.selectedLeave
                        });
                        cmd.Parameters.Add(new SqlParameter("@EmployeeId",SqlDbType.VarChar)
                        {
                            Value = data.selectedEmployee
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
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.activityIds = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public EmployeeKIOSKPortalDTO getEmployeedata(EmployeeKIOSKPortalDTO dto)
        {
            try
            {
                var emp_Id = _PortalContext.Staff_User_Login.Where(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Distinct().ToList();
                if (emp_Id.Count > 0)
                {
                    dto.HRME_Id = emp_Id.FirstOrDefault().Emp_Code;
                }
                var empdetails = _PortalContext.HR_Master_Employee_DMO.Where(e => e.MI_Id == dto.MI_Id && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && e.HRME_Id == dto.HRME_Id).Distinct().ToList();
                if (empdetails.Count > 0)
                {
                    dto.HRME_TechNonTeachingFlg = empdetails.FirstOrDefault().HRME_TechNonTeachingFlg;
                    dto.HRMD_Id = emp_Id.FirstOrDefault().Emp_Code;
                }

                dto.filldepartment = (from a in _PortalContext.HR_Master_Employee_DMO
                                      from b in _PortalContext.HR_Master_Department
                                      from c in _PortalContext.HR_Master_Designation
                                      where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id)
                                      select new EmployeeKIOSKPortalDTO
                                      {
                                          HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                          HRME_DOJ = a.HRME_DOJ,
                                          HRME_EmployeeCode = a.HRME_EmployeeCode,
                                          HRME_PhotoNo = a.HRME_Photo,
                                          HRME_DOB = a.HRME_DOB,
                                          HRMD_DepartmentName = b.HRMD_DepartmentName,
                                          HRMDES_DesignationName = c.HRMDES_DesignationName,
                                      }).Distinct().ToArray();

                dto.mobile = (from a in _hrms.Emp_MobileNo
                              where (a.HRME_Id == dto.HRME_Id && a.HRMEMNO_DeFaultFlag == "default")
                              select new EmployeeKIOSKPortalDTO
                              {
                                  HRME_MobileNo = a.HRMEMNO_MobileNo,
                              }).Distinct().ToArray();

                dto.email = (from a in _hrms.Emp_Email_Id

                             where (a.HRME_Id == dto.HRME_Id && a.HRMEM_DeFaultFlag == "default")
                             select new EmployeeKIOSKPortalDTO
                             {
                                 HRME_EmailId = a.HRMEM_EmailId,
                             }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public async Task<EmployeeKioskPunchDTO> getPunchreport(EmployeeKioskPunchDTO data)
        {
            try
            {
                data.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                data.Emp_punchDetails = (from a in _FOContext.FO_Emp_Punch
                                         from b in _FOContext.FO_Emp_Punch_Details
                                         where (a.FOEP_Id == b.FOEP_Id && b.FOEPD_Flag == "1" && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id && (a.FOEP_PunchDate.Value.Date >= data.fromdate.Value.Date && a.FOEP_PunchDate.Value.Date <= data.todate.Value.Date))
                                         select new EmployeeKioskPunchDTO
                                         {
                                             punchdate = a.FOEP_PunchDate,
                                             punchtime = b.FOEPD_PunchTime,
                                             InOutFlg = b.FOEPD_InOutFlg
                                         }
                 ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public EmployeeKIOSKPortalDTO getEmployeeFullDetails(EmployeeKIOSKPortalDTO dto)
        {
            try
            {
                var emp_Id = _exm.Staff_User_Login.Where(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Distinct().ToList();
                if (emp_Id.Count > 0)
                {
                    dto.HRME_Id = emp_Id.FirstOrDefault().Emp_Code;
                }
                var empdetails = _exm.HR_Master_Employee_DMO.Where(e => e.MI_Id == dto.MI_Id && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && e.HRME_Id == dto.HRME_Id).Distinct().ToList();
                if (empdetails.Count > 0)
                {
                    dto.HRME_TechNonTeachingFlg = empdetails.FirstOrDefault().HRME_TechNonTeachingFlg;
                    dto.HRMD_Id = emp_Id.FirstOrDefault().Emp_Code;
                }

                dto.filldepartment = (from a in _FOContext.HR_Master_Employee_DMO
                                      from b in _FOContext.HR_Master_Department_DMO
                                      from c in _FOContext.HR_Master_Designation_DMO
                                      where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id)
                                      select new EmployeeKIOSKPortalDTO
                                      {
                                          HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                          HRME_DOJ = a.HRME_DOJ,
                                          HRMD_DepartmentName = b.HRMD_DepartmentName,
                                          HRMDES_DesignationName = c.HRMDES_DesignationName,
                                          HRME_EmployeeCode = a.HRME_EmployeeCode,
                                          HRME_DOB = a.HRME_DOB,
                                          HRME_PhotoNo = a.HRME_Photo,
                                          DeviceID = a.HRME_AppDownloadedDeviceId
                                      }).Distinct().ToArray();

                dto.mobile = (from a in _hrms.Emp_MobileNo
                              where (a.HRME_Id == dto.HRME_Id && a.HRMEMNO_DeFaultFlag == "default")
                              select new EmployeeKIOSKPortalDTO
                              {
                                  HRME_MobileNo = a.HRMEMNO_MobileNo,
                              }).Distinct().ToArray();

                dto.email = (from a in _hrms.Emp_Email_Id
                             where (a.HRME_Id == dto.HRME_Id && a.HRMEM_DeFaultFlag == "default")
                             select new EmployeeKIOSKPortalDTO
                             {
                                 HRME_EmailId = a.HRMEM_EmailId,
                             }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public EmployeeKioskSalaryDTO getyeardata(EmployeeKioskSalaryDTO dto)
        {
            try
            {
                dto.yearlist = _hrms.HR_MasterLeaveYear.Where(t => t.MI_Id == dto.MI_Id && t.HRMLY_ActiveFlag == true).OrderBy(t=>t.HRMLY_LeaveYearOrder).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public EmployeeKioskSalaryDTO getsalarydetailsdata(EmployeeKioskSalaryDTO dto)
        {
            try
            {
                dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;

                dto.salarylist = (from m in _hrms.HR_Employee_Salary
                                  from n in _hrms.HR_Employee_Salary_Details
                                  from o in _hrms.HR_Master_EarningsDeductions
                                  where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == dto.MI_Id && m.HRME_Id == dto.HRME_Id && o.HRMED_EarnDedFlag.Equals("Earning") && m.HRES_Year == dto.HRMLY_LeaveYear
                                  group new { m, n, o }
                                    by new { m.HRES_Month, m.HRES_Year } into g
                                  select new EmployeeKioskSalaryDTO
                                  {
                                      salary = g.Sum(d => d.n.HRESD_Amount),
                                      monthName = g.FirstOrDefault().m.HRES_Month,
                                      hres_id = g.FirstOrDefault().m.HRES_Id,
                                      HRES_Year = g.FirstOrDefault().m.HRES_Year,
                                  }).OrderBy(t => t.hres_id).ToArray();

                dto.salaryEarningDlist = (from m in _hrms.HR_Employee_Salary
                                          from n in _hrms.HR_Employee_Salary_Details
                                          from o in _hrms.HR_Master_EarningsDeductions
                                          where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == dto.MI_Id && m.HRME_Id == dto.HRME_Id && o.HRMED_EarnDedFlag.Equals("Deduction") && m.HRES_Year == dto.HRMLY_LeaveYear
                                          group new { m, n, o }
                                            by new { m.HRES_Month, m.HRES_Year } into g
                                          select new EmployeeKioskSalaryDTO
                                          {
                                              salary = g.Sum(d => d.n.HRESD_Amount),
                                              monthName = g.FirstOrDefault().m.HRES_Month,
                                              hres_id = g.FirstOrDefault().m.HRES_Id,
                                              HRES_Year = g.FirstOrDefault().m.HRES_Year,
                                          }).OrderBy(t => t.hres_id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public EmployeeKioskTimeTableDTO getTTdata(EmployeeKioskTimeTableDTO dto)
        {
            try
            {
                dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;

                dto.TT_final_generation = (from a in _tt.TT_Final_GenerationDMO
                                           from b in _tt.TT_Final_Generation_DetailedDMO
                                           from c in _tt.School_M_Class
                                           from d in _tt.School_M_Section
                                           from e in _tt.TT_Master_PeriodDMO
                                           from f in _tt.TT_Master_DayDMO
                                           where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id && a.MI_Id == dto.MI_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id)
                                           select new
                                           {
                                               DayName = f.TTMD_DayName,
                                               PeriodCount = e.TTMP_PeriodName.Count()
                                           }).Distinct().GroupBy(f => f.DayName).Select(g => new EmployeeDashboardDTO { DayName = g.Key, PeriodCount = g.Count() }).ToArray();

                dto.allperiods = _tt.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag == true && c.MI_Id == dto.MI_Id).ToArray();
                dto.periods = _tt.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(dto.MI_Id)).ToArray();

                dto.class_sectons = (from a in _tt.TT_Final_GenerationDMO
                                     from b in _tt.TT_Final_Generation_DetailedDMO
                                     from c in _tt.School_M_Class
                                     from d in _tt.School_M_Section
                                     from e in _tt.TT_Master_PeriodDMO
                                     from f in _tt.TT_Master_DayDMO
                                     where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id && a.MI_Id == dto.MI_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id)
                                     select new EmployeeKioskTimeTableDTO
                                     {
                                         P_Days = f.TTMD_DayName,
                                         Period = e.TTMP_PeriodName,
                                         ASMCL_ClassName = c.ASMCL_ClassName,
                                         ASMC_SectionName = d.ASMC_SectionName
                                     }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
