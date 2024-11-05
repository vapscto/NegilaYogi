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
using PreadmissionDTOs.com.vaps.FrontOffice;
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
    public class LeaveYearlyReportImpl : LeaveYearlyReportInterface
    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ILogger<LeaveReportImpl> _log;
        public LMContext _lmContext;
        public LeaveYearlyReportImpl(LMContext ttcategory)
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

            List<HR_Master_LeaveYear_DMO> academic_year_name = new List<HR_Master_LeaveYear_DMO>();
            academic_year_name = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true).ToList();
            data.academic_year_name = academic_year_name.Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

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
        public async Task<EmployeeYearlyReportDTO> get_report(EmployeeYearlyReportDTO data)
        {
            string strhrme_ids = "";
            for (int i = 0; i < data.multihrme_id.Length; i++)
            {
                if (strhrme_ids == "") { strhrme_ids = (data.multihrme_id[i].HRME_Id).ToString(); }
                else { strhrme_ids = strhrme_ids + "," + (data.multihrme_id[i].HRME_Id).ToString(); }
            }

            try
            {
                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Leave_Emp_yearly_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@year",
                    SqlDbType.VarChar)
                    {
                        Value = data.hrmlY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                        SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                        SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
                   SqlDbType.NVarChar)
                    {
                        Value = strhrme_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid",
                   SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                  SqlDbType.VarChar)
                    {
                        Value = "yearly"
                    });
                    cmd.Parameters.Add(new SqlParameter("@cols",
                 SqlDbType.NVarChar, 2000)
                    {
                        Direction = ParameterDirection.Output
                    });
                    cmd.Parameters.Add(new SqlParameter("@totalpresent",
                 SqlDbType.VarChar, 10)
                    {
                        Direction = ParameterDirection.Output
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader1.ReadAsync())
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
                        data.filldata = retObject1.ToArray();
                        data.columnnames = cmd.Parameters["@cols"].Value.ToString();
                        data.totalworkingdays = cmd.Parameters["@totalpresent"].Value.ToString();

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

            //try
            //{
            //    using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
            //    {
            //        cmd.CommandText = "Leave_Emp_yearly_Report_two";
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        //added
            //        cmd.Parameters.Add(new SqlParameter("@fromdate",
            //            SqlDbType.VarChar)
            //        {
            //            Value = data.fromdate
            //        });
            //        cmd.Parameters.Add(new SqlParameter("@todate",
            //            SqlDbType.VarChar)
            //        {
            //            Value = data.todate
            //        });
            //        //added

            //        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
            //       SqlDbType.NVarChar)
            //        {
            //            //Value = data.multiplehrmeid
            //            Value = strhrme_ids
            //        });
            //        cmd.Parameters.Add(new SqlParameter("@miid",
            //       SqlDbType.BigInt)
            //        {
            //            Value = data.MI_Id
            //        });
            //        cmd.Parameters.Add(new SqlParameter("@type",
            //      SqlDbType.VarChar)
            //        {
            //            Value = "yearly"
            //        });
            //        cmd.Parameters.Add(new SqlParameter("@cols",
            //     SqlDbType.NVarChar, 2000)
            //        {
            //            Direction = ParameterDirection.Output
            //        });
            //        cmd.Parameters.Add(new SqlParameter("@totalpresent",
            //     SqlDbType.VarChar, 10)
            //        {
            //            Direction = ParameterDirection.Output
            //        });
            //        if (cmd.Connection.State != ConnectionState.Open)
            //            cmd.Connection.Open();
            //        try
            //        {
            //            var retObject1 = new List<dynamic>();
            //            using (var dataReader1 = await cmd.ExecuteReaderAsync())
            //            {
            //                while (await dataReader1.ReadAsync())
            //                {

            //                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
            //                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
            //                    {
            //                        dataRow1.Add(
            //                            dataReader1.GetName(iFiled1),
            //                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
            //                        );
            //                    }
            //                    retObject1.Add((ExpandoObject)dataRow1);
            //                }
            //            }
            //            data.filldatatwo = retObject1.ToArray();
            //            data.columnnames = cmd.Parameters["@cols"].Value.ToString();
            //            data.totalworkingdays = cmd.Parameters["@totalpresent"].Value.ToString();

            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //    }

            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}

            return data;
        }
    }
}