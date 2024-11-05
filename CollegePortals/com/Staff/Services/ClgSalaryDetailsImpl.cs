using PreadmissionDTOs.com.vaps.College.Portals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;

namespace CollegePortals.com.Staff.Services
{
    public class ClgSalaryDetailsImpl : Interfaces.ClgSalaryDetailsInterface
    {
        private static ConcurrentDictionary<string, ClgPortalHRMSDTO> _login =
           new ConcurrentDictionary<string, ClgPortalHRMSDTO>();
        private CollegeportalContext _ClgPortalContext;
        public ClgSalaryDetailsImpl(CollegeportalContext ClgPortalContext)
        {
            _ClgPortalContext = ClgPortalContext;
        }

        public ClgPortalHRMSDTO getloaddata(ClgPortalHRMSDTO data)
        {
            try
            {
                data.yearlist = _ClgPortalContext.HR_MasterLeaveYear.Where(t => t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true).Distinct().OrderBy(a => a.HRMLY_LeaveYearOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgPortalHRMSDTO getSalary(ClgPortalHRMSDTO data)
        {
            try
            {
                data.HRME_Id = _ClgPortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                data.salarylist = (from m in _ClgPortalContext.HR_Employee_Salary
                                   from n in _ClgPortalContext.HR_Employee_Salary_Details
                                   from o in _ClgPortalContext.HR_Master_EarningsDeductions
                                   where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == data.MI_Id && m.HRME_Id == data.HRME_Id && o.HRMED_EarnDedFlag.Equals("Earning") && m.HRES_Year == data.HRMLY_LeaveYear
                                   group new { m, n, o }
                                     by new { m.HRES_Month, m.HRES_Year } into g
                                   select new ClgPortalHRMSDTO
                                   {

                                       salary = g.Sum(d => d.n.HRESD_Amount),
                                       monthName = g.FirstOrDefault().m.HRES_Month,
                                       hres_id = g.FirstOrDefault().m.HRES_Id,
                                       HRES_Year = g.FirstOrDefault().m.HRES_Year,
                                   }
                                ).OrderBy(t => t.HRES_Month).ToArray();

                data.salaryEarningDlist = (from m in _ClgPortalContext.HR_Employee_Salary
                                           from n in _ClgPortalContext.HR_Employee_Salary_Details
                                           from o in _ClgPortalContext.HR_Master_EarningsDeductions
                                           where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == data.MI_Id && m.HRME_Id == data.HRME_Id && o.HRMED_EarnDedFlag.Equals("Deduction") && m.HRES_Year == data.HRMLY_LeaveYear
                                           group new { m, n, o }
                                             by new { m.HRES_Month, m.HRES_Year } into g
                                           select new ClgPortalHRMSDTO
                                           {
                                               salary = g.Sum(d => d.n.HRESD_Amount),
                                               monthName = g.FirstOrDefault().m.HRES_Month,
                                               hres_id = g.FirstOrDefault().m.HRES_Id,
                                               HRES_Year = g.FirstOrDefault().m.HRES_Year,
                                           }
                                ).OrderBy(t => t.hres_id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public ClgPortalHRMSDTO getsalaryalldetails(ClgPortalHRMSDTO data)
        {
            try
            {
                data.HRME_Id = _ClgPortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                var EQuery = _ClgPortalContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == data.MI_Id && t.HRMED_EarnDedFlag.Equals("Earning")).Select(d => d.HRMED_Id).ToList();
                data.TotalEarning = (from m in _ClgPortalContext.HR_Employee_Salary
                                    from n in _ClgPortalContext.HR_Employee_Salary_Details
                                    where m.HRES_Id == n.HRES_Id && EQuery.Contains(n.HRMED_Id) && m.MI_Id == data.MI_Id && m.HRME_Id == data.HRME_Id && m.HRES_Id == data.HRES_Id
                                    group new { m, n }
                                     by new { m.HRES_Month } into g
                                    select new ClgPortalHRMSDTO
                                    {
                                        hres_id = g.FirstOrDefault().m.HRES_Id,
                                        salary = g.Sum(d => d.n.HRESD_Amount),

                                    }
                                 ).Distinct().ToArray();


                var DQuery = _ClgPortalContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == data.MI_Id && t.HRMED_EarnDedFlag.Equals("Deduction")).Select(d => d.HRMED_Id).ToList();
                data.totalDeduction = (from m in _ClgPortalContext.HR_Employee_Salary
                                      from n in _ClgPortalContext.HR_Employee_Salary_Details
                                      where m.HRES_Id == n.HRES_Id && DQuery.Contains(n.HRMED_Id) && m.MI_Id == data.MI_Id && m.HRME_Id == data.HRME_Id && m.HRES_Id == data.HRES_Id
                                      group new { m, n }
                                       by new { m.HRES_Month } into g
                                      select new ClgPortalHRMSDTO
                                      {
                                          hres_id = g.FirstOrDefault().m.HRES_Id,
                                          salary = g.Sum(d => d.n.HRESD_Amount),
                                      }
                                 ).Distinct().ToArray();


                data.salarylistE = (from m in _ClgPortalContext.HR_Employee_Salary
                                   from n in _ClgPortalContext.HR_Employee_Salary_Details
                                   from o in _ClgPortalContext.HR_Master_EarningsDeductions
                                   where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == data.MI_Id && m.HRME_Id == data.HRME_Id && m.HRES_Id == data.HRES_Id && o.HRMED_EarnDedFlag.Equals("Earning")

                                   select new ClgPortalHRMSDTO
                                   {
                                       HRMED_Id = o.HRMED_Id,
                                       HRMED_Name = o.HRMED_Name,
                                       HRESD_Amount = n.HRESD_Amount,
                                   }
                                 ).Distinct().ToArray();

                data.salarylistD = (from m in _ClgPortalContext.HR_Employee_Salary
                                   from n in _ClgPortalContext.HR_Employee_Salary_Details
                                   from o in _ClgPortalContext.HR_Master_EarningsDeductions
                                   where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == data.MI_Id && m.HRME_Id == data.HRME_Id && m.HRES_Id == data.HRES_Id && o.HRMED_EarnDedFlag.Equals("Deduction")

                                   select new ClgPortalHRMSDTO
                                   {
                                       HRMED_Id = o.HRMED_Id,
                                       HRMED_Name = o.HRMED_Name,
                                       HRESD_Amount = n.HRESD_Amount,
                                   }
                                ).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
