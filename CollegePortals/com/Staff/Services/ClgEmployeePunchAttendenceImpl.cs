using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePortals.com.Staff.Services
{
    public class ClgEmployeePunchAttendenceImpl : Interfaces.ClgEmployeePunchAttendenceInterface
    {
        public FOContext _FOContext;
        private object multipletype;
        public ExamContext _exm;

        public ClgEmployeePunchAttendenceImpl(FOContext fOContext, ExamContext exm)
        {
            _exm = exm;
            _FOContext = fOContext;
        }
        public ClgStaffDashboardDTO getdata(ClgStaffDashboardDTO data)
        {
            try
            {
                data.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                data.filldepartment = (from a in _FOContext.HR_Master_Employee_DMO
                                       from b in _FOContext.HR_Master_Department_DMO
                                       from c in _FOContext.HR_Master_Designation_DMO
                                       where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true
                                           && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRME_Id==data.HRME_Id)
                                       select new ClgStaffDashboardDTO
                                       {
                                           empFname = a.HRME_EmployeeFirstName,
                                           empMname = a.HRME_EmployeeMiddleName,
                                           empLname = a.HRME_EmployeeLastName,
                                           HRME_DOJ =a.HRME_DOJ ,
                                           HRMD_DepartmentName = b.HRMD_DepartmentName,
                                           HRMDES_DesignationName = c.HRMDES_DesignationName
                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    
        public async Task<ClgStaffDashboardDTO> getreport(ClgStaffDashboardDTO data)
        {
            try
            {
                data.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                data.Emp_punchDetails = (from a in _FOContext.FO_Emp_Punch
                                         from b in _FOContext.FO_Emp_Punch_Details
                                         where (a.FOEP_Id == b.FOEP_Id && b.FOEPD_Flag == "1" && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id && (a.FOEP_PunchDate.Value.Date >= data.fromdate.Value.Date && a.FOEP_PunchDate.Value.Date <= data.todate.Value.Date))
                                       select new ClgStaffDashboardDTO
                                       {
                                           punchdate  = a.FOEP_PunchDate,
                                           punchtime = b.FOEPD_PunchTime,
                                           InOutFlg = b.FOEPD_InOutFlg,
                                       }
                 ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
               return data;
        }
    }

        
}

