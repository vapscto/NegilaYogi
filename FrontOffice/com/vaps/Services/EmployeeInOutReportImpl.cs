using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{

    public class EmployeeInOutReportImpl:Interfaces.EmployeeInOutReportInterface
    {
        public FOContext _FOContext;

        public EmployeeInOutReportImpl(FOContext ttcntx)
        {
            _FOContext = ttcntx;

        }
        public EmployeeInOutReportDTO getdata(EmployeeInOutReportDTO data)
        {
            try
            {
                List<HR_Master_GroupType> staf_types = new List<HR_Master_GroupType>();
                staf_types = _FOContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList(); //
                data.filltypes = staf_types.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeInOutReportDTO get_departments(EmployeeInOutReportDTO data)
        {
            var dd = data.multipletype.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }
            //data.filldepartment = (from a in _FOContext.HR_Master_Employee_DMO
            //                       from b in _FOContext.HR_Master_Department_DMO
            //                       from c in _FOContext.HR_Master_GroupType_DMO
            //                       where (a.HRMD_Id == b.HRMD_Id && a.HRMGT_Id == c.HRMGT_Id && c.HRMGT_ActiveFlag == true
            //                           && b.HRMD_ActiveFlag == true && a.HRME_ActiveFlag == true && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && list.Contains(c.HRMGT_Id))
            //                       select new EmployeeInOutReportDTO
            //                       {
            //                           HRMD_Id = b.HRMD_Id,
            //                           HRMD_DepartmentName = b.HRMD_DepartmentName,
            //                       }
            //         ).Distinct().ToArray();

            data.filldepartment = (from a in _FOContext.HRGroupDeptDessgDMO
                                   from b in _FOContext.HR_Master_Department_DMO
                                   where (a.MI_Id == data.MI_Id && a.HRMD_Id == b.HRMD_Id && list.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                   select b).Distinct().ToArray();

            return data;
        }
        public EmployeeInOutReportDTO get_designation(EmployeeInOutReportDTO data)
        {
            var dd = data.multipledep.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }
            //data.filldesignation = (from a in _FOContext.HR_Master_Employee_DMO
            //                        from b in _FOContext.HR_Master_Designation_DMO
            //                        from c in _FOContext.HR_Master_Department_DMO
            //                        where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
            //                        && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
            //                        && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && list.Contains(c.HRMD_Id))
            //                        select new EmployeeInOutReportDTO
            //                        {
            //                            HRMDES_Id = b.HRMDES_Id,
            //                            HRMDES_DesignationName = b.HRMDES_DesignationName,
            //                        }
            //         ).Distinct().ToArray();

            data.filldesignation = (from a in _FOContext.HRGroupDeptDessgDMO
                                    from b in _FOContext.HR_Master_Designation_DMO
                                    where (a.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && list.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                    select b).Distinct().ToArray();

            return data;
        }
        public EmployeeInOutReportDTO get_employee(EmployeeInOutReportDTO data)
        {
            string[] HRMDES_Id = data.multipledes.ToString().Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < HRMDES_Id.Length; i++)
            {
                list.Add(Convert.ToInt64(HRMDES_Id[i]));
            }
            string[] HRMGT_Id = data.multipletype.ToString().Split(',');
            List<long> list1 = new List<long>();
            for (int i = 0; i < HRMGT_Id.Length; i++)
            {
                list1.Add(Convert.ToInt64(HRMGT_Id[i]));
            }
            string[] HRMD_Id = data.multipledep.ToString().Split(','); 
            List<long> list2 = new List<long>();
            for (int i = 0; i < HRMD_Id.Length; i++)
            {
                list2.Add(Convert.ToInt64(HRMD_Id[i]));
            }
            data.fillemployee = (from c in _FOContext.HR_Master_Employee_DMO
                                 where (c.MI_Id == data.MI_Id && list.Contains(c.HRMDES_Id) && list1.Contains(c.HRMGT_Id) && list2.Contains(c.HRMD_Id) && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false)
                                 select new EmployeeInOutReportDTO
                                 {
                                     HRME_Id = c.HRME_Id,
                                    // ename = a.HRME_EmployeeFirstName + ' ' + a.HRME_EmployeeMiddleName + ' ' + a.HRME_EmployeeLastName,
                                     ename = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : " " + c.HRME_EmployeeFirstName)
                                     + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " " + c.HRME_EmployeeLastName)).Trim(),
                                     HRME_EmployeeCode = c.HRME_EmployeeCode,
                                     HRME_EmployeeOrder = c.HRME_EmployeeOrder
                                 }).Distinct().OrderBy(t=>t.HRME_EmployeeOrder).ToArray();
            return data;
        }

        public EmployeeInOutReportDTO getreport(EmployeeInOutReportDTO data)
        {
            try
            {
                data.columnnames = (from a in Enumerable.Range(0, (Convert.ToDateTime(data.todate) - Convert.ToDateTime(data.fromdate)).Days + 1)
                                    let columndate = Convert.ToDateTime(data.fromdate).AddDays(a)
                                    select columndate).Distinct().ToArray();
                string[] hrmeIds = data.multiplehrmeid.ToString().Split(',');
                List<long> list = new List<long>();
                for (int i = 0; i < hrmeIds.Length; i++)
                {
                    list.Add(Convert.ToInt64(hrmeIds[i]));
                }
             

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_EMPLOYEEINOUT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = data.multiplehrmeid.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
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
                        data.filldata = retObject1.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }




                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "Fo_Emp_Log_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.VarChar)
                    {
                        Value = ""
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                       SqlDbType.VarChar)
                    {
                        Value = ""
                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
                   SqlDbType.VarChar)
                    {
                        Value = ""
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                   SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.NVarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
                  SqlDbType.VarChar)
                    {
                        Value = data.multiplehrmeid.ToString()
                    });

                    cmd.Parameters.Add(new SqlParameter("@punchtype",
                  SqlDbType.VarChar)
                    {
                        Value = "LIEO"
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
                                        dataReader1.IsDBNull(iFiled1) ? "-" : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.filldataLIEO = retObject1.ToArray();

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

            return data;
        }

        public async Task<EmployeeInOutReportDTO> lateIn_details(EmployeeInOutReportDTO data)
        {
            try
            {
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SP_MG_PAYCARE_LATEIN_DETAILS";
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                     SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.CommandType = CommandType.StoredProcedure;


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
                    //leaveApprovalStatus(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("late In report error");
                //_log.LogInformation("Late In Report error");
                //_log.LogDebug(ex.Message);
            }
            return data;
        }
    }
}
