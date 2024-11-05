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
    public class LeaveReportImpl : LeaveReportInterface
    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ILogger<LeaveReportImpl> _log;
        public LMContext _lmContext;
        public LeaveReportImpl(LMContext ttcategory)
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

            data.Department_types = (from a in _lmContext.HRGroupDeptDessgDMO
                                     from b in _lmContext.HR_Master_Department_DMO
                                     where (a.HRMD_Id == b.HRMD_Id
                                          && b.HRMD_ActiveFlag == true && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id
                                          && selected_emp_types.Contains(a.HRMGT_Id))
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

            data.Designation_types = (from a in _lmContext.HRGroupDeptDessgDMO
                                      from b in _lmContext.HR_Master_Designation_DMO
                                      where (a.HRMDES_Id == b.HRMDES_Id
                                      && b.HRMDES_ActiveFlag == true
                                       && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && selected_desg_types.Contains(a.HRMD_Id))
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
            List<long> selected_emp = new List<long>();
            List<long> selected_dep = new List<long>();
            foreach (var itm in data.emptypes)
            {
                selected_emp.Add(itm.HRMDES_Id);
            }
            foreach (var itm11 in data.empdept)
            {
                selected_dep.Add(itm11.HRMD_Id);
            }

            //data.get_emp = (from a in _lmContext.HR_Master_Employee_DMO
            //                from b in _lmContext.HR_Master_Designation_DMO
            //                from c in _lmContext.HR_Master_Department_DMO
            //                where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
            //                && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
            //                 && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && selected_emp.Contains(b.HRMDES_Id)  && selected_dep.Contains(c.HRMD_Id))
            //                select new LeaveTransactionManualDTO
            //                {
            //                    HRME_Id = a.HRME_Id,
            //                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
            //                }).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToArray();


            List<long> selected_department = new List<long>();

            foreach (var itm in data.empdept)
            {
                selected_department.Add(itm.HRMD_Id);
            }

            //data.get_emp = (from a in _lmContext.HR_Master_Employee_DMO
            //                select new LeaveTransactionManualDTO
            //                {
            //                    HRME_Id = a.HRME_Id,
            //                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
            //                }).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToArray();

            data.get_emp = (from a in _lmContext.HR_Master_Employee_DMO
                            from b in _lmContext.HR_Master_Designation_DMO
                            from c in _lmContext.HR_Master_Department_DMO
                            where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                            && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                             && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && selected_emp.Contains(b.HRMDES_Id) && selected_dep.Contains(c.HRMD_Id))
                            select new LeaveTransactionManualDTO
                            {
                                HRME_Id = a.HRME_Id,
                                HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                            }).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToArray();

            return data;
        }
        public async Task<LeaveCreditDTO> get_report(LeaveCreditDTO data)
        {
            try
            {
                string employeelist = "0";
                employeelist = string.Join(",", data.selectedreport);
                if (data.Edit_flag == false)
                {
                    data.HRELTD_LWPFlag = false;
                    if (employeelist != "")
                    {
                        if (data.selectedLeave.IndexOf(',') < 0)
                        {
                            var leavetype = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRML_Id.ToString() == data.selectedLeave && t.HRML_LeaveType == "LWP").ToArray();
                            if (leavetype.Length > 0)
                            {
                                data.HRELTD_LWPFlag = true;
                                var lopresult = (from a in _lmContext.HR_Emp_Leave_Trans_DMO
                                                 from b in _lmContext.HR_Master_Leave_DMO
                                                 from c in _lmContext.HR_Master_Employee_DMO
                                                 where (c.MI_Id == data.MI_Id && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false && a.HRME_Id == c.HRME_Id && a.HRELT_FromDate >= data.HRELTD_FromDate && a.HRELT_ToDate <= data.HRELT_ToDate && a.HRELT_LeaveId == b.HRML_Id && data.selectedEmployee.Contains(c.HRME_Id.ToString()) && b.HRML_Id.ToString() == data.selectedLeave)
                                                 select new LeaveCreditDTO
                                                 {
                                                     HRME_EmployeeCode = c.HRME_EmployeeCode,
                                                     HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == " " ? " " : c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == " " ? " " : c.HRME_EmployeeLastName),
                                                     HRML_LeaveName = b.HRML_LeaveName,
                                                     HRELT_FromDate = a.HRELT_FromDate,
                                                     HRELT_ToDate = a.HRELT_ToDate,
                                                     HRELT_TotDays = a.HRELT_TotDays
                                                 }).ToArray();
                                data.result = lopresult;
                                return data;
                            }
                        }

                        List<LeaveCreditDTO> result = new List<LeaveCreditDTO>();

                        DateTime fromdate = new DateTime();
                        string confromdate = "";

                        fromdate = Convert.ToDateTime(data.HRELTD_FromDate.Date.ToString("yyyy-MM-dd"));
                        confromdate = fromdate.ToString("yyyy-MM-dd");

                        DateTime todate = new DateTime();
                        string contodate = "";

                        todate = Convert.ToDateTime(data.HRELT_ToDate.Date.ToString("yyyy-MM-dd"));
                        contodate = todate.ToString("yyyy-MM-dd");

                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Employees_Bal_Leaves";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.Date)
                            {
                                Value = data.HRELTD_FromDate.ToString()
                            });
                            cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date)
                            {
                                Value = data.HRELT_ToDate.ToString()
                            });

                            cmd.Parameters.Add(new SqlParameter("@Leaveid", SqlDbType.VarChar)
                            {
                                Value = data.selectedLeave
                            });
                            cmd.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.VarChar)
                            {
                                Value = employeelist
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
                                                dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
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

                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Employees_Bal_Leaves_kiosk";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                            {
                                Value = data.HRELTD_FromDate.ToString("dd-MM-yyyy")
                            });
                            cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                            {
                                Value = data.HRELT_ToDate.ToString("dd-MM-yyyy")
                            });
                            //cmd.Parameters.Add(new SqlParameter("@Leaveid", SqlDbType.VarChar)
                            //{
                            //    Value = data.selectedLeave
                            //});
                            cmd.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.VarChar)
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
                                                dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.get_leaveDetails = retObject.ToArray();
                            }

                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }
                }

                else
                {
                    using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FO_AllEmpsLeaveTransaction_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Ids", SqlDbType.VarChar)
                        {
                            Value = employeelist
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
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.result = retObject.ToArray();

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
                _log.LogInformation("Student Activities error");
                _log.LogDebug(ex.Message);
            }

            ///////////////////////////////////////////////////////////////////////////


            return data;
        }

        ////PeriodWiseleaveReport//////////////////////////////////////////////////////////
        public LeaveCreditDTO periodgetleavereport(LeaveCreditDTO data)
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
        public LeaveCreditDTO periodget_departments(LeaveCreditDTO data)
        {
            List<long> selected_emp_types = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_emp_types.Add(itm.HRMGT_Id);
            }

            data.Department_types = (from a in _lmContext.HRGroupDeptDessgDMO
                                     from b in _lmContext.HR_Master_Department_DMO
                                     where (a.HRMD_Id == b.HRMD_Id
                                          && b.HRMD_ActiveFlag == true && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id
                                          && selected_emp_types.Contains(a.HRMGT_Id))
                                     select new LeaveCreditDTO
                                     {
                                         HRMD_Id = b.HRMD_Id,
                                         HRMD_DepartmentName = b.HRMD_DepartmentName,
                                     }
                     ).Distinct().ToArray();

            return data;
        }
        public LeaveCreditDTO periodget_designation(LeaveCreditDTO data)
        {
            List<long> selected_desg_types = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_desg_types.Add(itm.HRMD_Id);
            }

            data.Designation_types = (from a in _lmContext.HRGroupDeptDessgDMO
                                      from b in _lmContext.HR_Master_Designation_DMO
                                      where (a.HRMDES_Id == b.HRMDES_Id
                                      && b.HRMDES_ActiveFlag == true
                                       && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && selected_desg_types.Contains(a.HRMD_Id))
                                      select new LeaveCreditDTO
                                      {
                                          HRMDES_Id = b.HRMDES_Id,
                                          HRMDES_DesignationName = b.HRMDES_DesignationName,
                                      }
                     ).Distinct().ToArray();

            return data;
        }
        public LeaveCreditDTO periodget_Employees(LeaveCreditDTO data)
        {
            List<long> selected_emp = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_emp.Add(itm.HRMDES_Id);
            }

            data.get_emp = (from a in _lmContext.HR_Master_Employee_DMO
                            from b in _lmContext.HR_Master_Designation_DMO
                            from c in _lmContext.HR_Master_Department_DMO
                            where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                            && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                             && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && selected_emp.Contains(b.HRMDES_Id))
                            select new LeaveTransactionManualDTO
                            {
                                HRME_Id = a.HRME_Id,
                                HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                            }).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToArray();

            return data;
        }
        public async Task<LeaveCreditDTO> periodget_report(LeaveCreditDTO data)
        {
            try
            {
                data.HRELTD_LWPFlag = false;
                if (data.selectedEmployee != "")
                {
                    if (data.selectedLeave.IndexOf(',') < 0)
                    {
                        var leavetype = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRML_Id.ToString() == data.selectedLeave && t.HRML_LeaveType == "LWP").ToArray();
                        if (leavetype.Length > 0)
                        {
                            data.HRELTD_LWPFlag = true;
                            var lopresult = (from a in _lmContext.HR_Emp_Leave_Trans_DMO
                                             from b in _lmContext.HR_Master_Leave_DMO
                                             from c in _lmContext.HR_Master_Employee_DMO
                                             where (c.MI_Id == data.MI_Id && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false && a.HRME_Id == c.HRME_Id && a.HRELT_FromDate >= data.HRELTD_FromDate && a.HRELT_ToDate <= data.HRELT_ToDate && a.HRELT_LeaveId == b.HRML_Id && data.selectedEmployee.Contains(c.HRME_Id.ToString()) && b.HRML_Id.ToString() == data.selectedLeave)
                                             select new LeaveCreditDTO
                                             {
                                                 HRME_EmployeeCode = c.HRME_EmployeeCode,
                                                 HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == " " ? " " : c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == " " ? " " : c.HRME_EmployeeLastName),
                                                 HRML_LeaveName = b.HRML_LeaveName,
                                                 HRELT_FromDate = a.HRELT_FromDate,
                                                 HRELT_ToDate = a.HRELT_ToDate,
                                                 HRELT_TotDays = a.HRELT_TotDays,
                                                 HRELT_Id = a.HRELT_Id,
                                                 HRELT_LeaveId = a.HRELT_LeaveId
                                             }).ToArray();
                            data.result = lopresult;

                            //data.periodreport = (from a in _lmContext.HR_Emp_Leave_Application_DeputationDMO
                            //                     from b in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                            //                     where (a.HRELAPD_Id==b.HRELAPD_Id)
                            //                     select new LeaveCreditDTO
                            //                     {
                            //                         HRELAPDD_Period= a.HRELAPDD_Period,
                            //                         HRELAPDD_Date=a.HRELAPDD_Date,
                            //                         HRME_Id=a.HRME_Id
                            //                     }).ToArray();

                            using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "IVRM_PeriodWiseLeavereport";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

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
                                                    dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow);
                                        }
                                    }
                                    data.periodreport = retObject.ToArray();
                                }

                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            return data;
                        }
                    }

                    List<LeaveCreditDTO> result = new List<LeaveCreditDTO>();

                    DateTime fromdate = new DateTime();
                    string confromdate = "";

                    fromdate = Convert.ToDateTime(data.HRELTD_FromDate.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdate.ToString("yyyy-MM-dd");

                    DateTime todate = new DateTime();
                    string contodate = "";

                    todate = Convert.ToDateTime(data.HRELT_ToDate.Date.ToString("yyyy-MM-dd"));
                    contodate = todate.ToString("yyyy-MM-dd");

                    using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Employees_Bal_Leaves";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                        {
                            Value = data.HRELTD_FromDate.ToString("dd-MM-yyyy")
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                        {
                            Value = data.HRELT_ToDate.ToString("dd-MM-yyyy")
                        });
                        cmd.Parameters.Add(new SqlParameter("@Leaveid", SqlDbType.VarChar)
                        {
                            Value = data.selectedLeave
                        });
                        cmd.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.VarChar)
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
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
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
                    using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_PeriodWiseLeavereport";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

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
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.periodreport = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Employees_Bal_Leaves_kiosk";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                        {
                            Value = data.HRELTD_FromDate.ToString("dd-MM-yyyy")
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                        {
                            Value = data.HRELT_ToDate.ToString("dd-MM-yyyy")
                        });
                        //cmd.Parameters.Add(new SqlParameter("@Leaveid", SqlDbType.VarChar)
                        //{
                        //    Value = data.selectedLeave
                        //});
                        cmd.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.VarChar)
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
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.get_leaveDetails = retObject.ToArray();
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
                _log.LogInformation("Student Activities error");
                _log.LogDebug(ex.Message);
            }

            ///////////////////////////////////////////////////////////////////////////


            return data;
        }
    }
}