using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.TT;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;


namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveStatusReportImpl : LeaveStatusReportInterface
    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ILogger<LeaveStatusReportImpl> _log;
        public LMContext _lmContext;
        public LeaveStatusReportImpl(LMContext ttcategory)
        {
            _lmContext = ttcategory;
        }

        public LeaveCreditDTO getleavereport(LeaveCreditDTO data)
        {
            List<HR_Master_GroupType_DMO> staf_types = new List<HR_Master_GroupType_DMO>();
            staf_types = _lmContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList();
            data.stf_types = staf_types.Distinct().ToArray();

            List<HR_Master_Department_DMO> Department_types = new List<HR_Master_Department_DMO>();
            Department_types = _lmContext.HR_Master_Department_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList();
            data.Department_types = Department_types.Distinct().ToArray();


            List<HR_Master_Designation_DMO> Designation_types = new List<HR_Master_Designation_DMO>();
            Designation_types = _lmContext.HR_Master_Designation_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMDES_ActiveFlag == true).ToList();
            data.Designation_types = Designation_types.Distinct().ToArray();

            //List<HR_Master_Employee_DMO> get_emp = new List<HR_Master_Employee_DMO>();
            //get_emp = _lmContext.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true).ToList();
            //data.get_emp = get_emp.Distinct().ToArray();

            List<HR_Master_Leave_DMO> leave_name = new List<HR_Master_Leave_DMO>();
            leave_name = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
            data.leave_name = leave_name.Distinct().ToArray();

            List<IVRM_Month_DMO> credit_month = new List<IVRM_Month_DMO>();
            credit_month = _lmContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToList();
            data.credit_month = credit_month.Distinct().ToArray();

            List<HR_Master_LeaveYear_DMO> get_year = new List<HR_Master_LeaveYear_DMO>();
            get_year = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true).ToList();
            data.get_year = get_year.Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

            return data;
        }
        public LeaveCreditDTO get_departments(LeaveCreditDTO data)
        {
            List<long> selected_emp_types = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_emp_types.Add(itm.HRMGT_Id);
            }

            data.Department_types = (from a in _lmContext.HR_Master_Employee_DMO
                                     from b in _lmContext.HR_Master_Department_DMO
                                     from c in _lmContext.HR_Master_GroupType_DMO
                                     where (a.HRMD_Id == b.HRMD_Id && a.HRMGT_Id == c.HRMGT_Id && c.HRMGT_ActiveFlag == true
                                          && b.HRMD_ActiveFlag == true && a.HRME_ActiveFlag == true && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id
                                          && selected_emp_types.Contains(c.HRMGT_Id))
                                     select new LeaveCreditDTO
                                     {
                                         HRMD_Id = b.HRMD_Id,
                                         HRMD_DepartmentName = b.HRMD_DepartmentName,
                                     }
                     ).Distinct().ToArray();

            return data;
        }
        public LeaveCreditDTO get_designation(LeaveCreditDTO data)
        {
            List<long> selected_desg_types = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_desg_types.Add(itm.HRMD_Id);
            }

            data.Designation_types = (from a in _lmContext.HR_Master_Employee_DMO
                                      from b in _lmContext.HR_Master_Designation_DMO
                                      from c in _lmContext.HR_Master_Department_DMO
                                      where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                      && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                       && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && selected_desg_types.Contains(c.HRMD_Id))
                                      select new LeaveCreditDTO
                                      {
                                          HRMDES_Id = b.HRMDES_Id,
                                          HRMDES_DesignationName = b.HRMDES_DesignationName,
                                      }
                     ).Distinct().ToArray();

            return data;
        }
        public LeaveCreditDTO get_Employees(LeaveCreditDTO data)
        {

            //List<HR_Master_Employee_DMO> get_emp = new List<HR_Master_Employee_DMO>();
            //get_emp = _lmContext.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true).ToList();
            //data.get_emp = get_emp.Distinct().ToArray();


            List<long> selected_emp = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_emp.Add(itm.HRMDES_Id);
            }

            data.get_emp = (from a in _lmContext.HR_Master_Employee_DMO
                            from b in _lmContext.HR_Master_Designation_DMO
                            from c in _lmContext.HR_Master_Department_DMO
                            where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                            && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                             && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && selected_emp.Contains(b.HRMDES_Id))
                            select new LeaveTransactionManualDTO
                            {
                                //HRME_Id = a.HRME_Id,
                                //HRMDES_Id = a.HRMDES_Id,
                                //HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,

                                HRME_Id = a.HRME_Id,
                                HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                            }
                     ).Distinct().ToArray();

            return data;
        }
        public async Task<LeaveCreditDTO> get_report(LeaveCreditDTO data)
        {
            data.activityIds = (from c in _lmContext.HR_Master_Employee_DMO
                                from b in _lmContext.HR_Master_Leave_DMO
                                from e in _lmContext.HR_Emp_Leave_StatusDMO
                                where (e.HRME_Id == c.HRME_Id && e.MI_Id==c.MI_Id && b.MI_Id == data.MI_Id && b.HRML_Id == e.HRML_Id && c.MI_Id == data.MI_Id && data.selectedreport.Contains(c.HRME_Id) && e.HRML_Id== data.HRML_Id && e.HRMLY_Id == data.HRMLY_Id)
                                select new LeaveCreditDTO
                                {
                                    HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                                    HRELS_TotalLeaves = e.HRELS_TotalLeaves,
                                    HRELS_TransLeaves = e.HRELS_TransLeaves,
                                    HRELS_CBLeaves = e.HRELS_CBLeaves,
                                    HRML_LeaveName = b.HRML_LeaveName,
                                    HRME_EmployeeCode = c.HRME_EmployeeCode,
                                }
                       ).Distinct().ToArray();
            return data;
        }
    }
}