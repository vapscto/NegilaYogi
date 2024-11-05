using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
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
    public class EmployeePunchAttendenceImpl:Interfaces.EmployeePunchAttendenceInterface
    {
        public FOContext _FOContext;
        private object multipletype;
        public ExamContext _exm;

        public EmployeePunchAttendenceImpl(FOContext fOContext, ExamContext exm)
        {
            _exm = exm;
            _FOContext = fOContext;
        }
        public EmployeeDashboardDTO getdata(EmployeeDashboardDTO data)
        {
            try
            {
                data.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;


                data.filldepartment = (from a in _FOContext.HR_Master_Employee_DMO
                                       from b in _FOContext.HR_Master_Department_DMO
                                       from c in _FOContext.HR_Master_Designation_DMO
                                       where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true
                                           && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRME_Id==data.HRME_Id)
                                       select new EmployeeDashboardDTO
                                       {
                                           empFname = a.HRME_EmployeeFirstName,
                                           empMname = a.HRME_EmployeeMiddleName,
                                           empLname = a.HRME_EmployeeLastName,
                                           HRME_DOJ =a.HRME_DOJ ,
                                           HRMD_DepartmentName = b.HRMD_DepartmentName,
                                           HRMDES_DesignationName = c.HRMDES_DesignationName,

                                       }
                   ).Distinct().ToArray();

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    
    
      
        public async Task<EmployeeDashboardDTO> getreport(EmployeeDashboardDTO data)
        {
            try
            {
                data.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                //data.Emp_punchDetails = (from a in _FOContext.FO_Emp_Punch
                //                         from b in _FOContext.FO_Emp_Punch_Details
                //                         where (a.FOEP_Id == b.FOEP_Id && b.FOEPD_Flag == "1" && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id && (a.FOEP_PunchDate.Value.Date >= data.fromdate.Value.Date && a.FOEP_PunchDate.Value.Date <= data.todate.Value.Date))
                //                         select new EmployeeDashboardDTO
                //                         {
                //                             punchdate = a.FOEP_PunchDate,
                //                             punchtime = b.FOEPD_PunchTime,
                //                             InOutFlg = b.FOEPD_InOutFlg,
                //                         }).Distinct().ToArray();


                //using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Fo_Emp_Log_Report";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@date",
                //        SqlDbType.VarChar)
                //    {
                //        Value = ""
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@month",
                //       SqlDbType.VarChar)
                //    {
                //        Value = ""
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@year",
                //   SqlDbType.VarChar)
                //    {
                //        Value = ""
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@fromdate",
                //   SqlDbType.VarChar)
                //    {
                //        Value = data.fromdate.Value.Date.ToString("yyyy/MM/dd")
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@todate",
                //  SqlDbType.NVarChar)
                //    {
                //        Value = data.todate.Value.Date.ToString("yyyy/MM/dd")
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@miid",
                //  SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
                //  SqlDbType.VarChar)
                //    {
                //        Value = data.HRME_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@punchtype",
                //  SqlDbType.VarChar)
                //    {
                //        Value = "LIEO"
                //    });
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();


                //    try
                //    {
                //        var retObject1 = new List<dynamic>();
                //        using (var dataReader1 = await cmd.ExecuteReaderAsync())
                //        {
                //            while (await dataReader1.ReadAsync())
                //            {

                //                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                //                {
                //                    dataRow1.Add(
                //                        dataReader1.GetName(iFiled1),
                //                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                //                    );
                //                }
                //                retObject1.Add((ExpandoObject)dataRow1);
                //            }
                //        }
                //        data.datalst = retObject1.ToArray();

                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}



                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fo_Employee_Punch_Detail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });                  
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                   SqlDbType.VarChar)
                    {
                        Value = data.fromdate.Value.Date.ToString("yyyy/MM/dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.NVarChar)
                    {
                        Value = data.todate.Value.Date.ToString("yyyy/MM/dd")
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
                        data.Emp_punchDetails = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
               return data;
        }
    }

        
}

