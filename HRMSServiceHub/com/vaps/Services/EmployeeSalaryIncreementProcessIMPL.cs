using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class EmployeeSalaryIncreementProcessIMPL : Interfaces.EmployeeSalaryIncreementProcessInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeSalaryIncreementProcessIMPL(HRMSContext HRMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _HRMSContext = HRMSContext;
            _Context = OrganisationContext;
        }

        public EmployeeSalaryIncreementProcessDTO getBasicData(EmployeeSalaryIncreementProcessDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public EmployeeSalaryIncreementProcessDTO getReport(EmployeeSalaryIncreementProcessDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                string fromDate = ""; string ToDate = "";
                if (dto.Option == "MONTHWISE")
                {
                    fromDate = dto.Fromdate.ToString("yyyy-MM-dd");
                    ToDate = dto.Todate.ToString("yyyy-MM-dd");
                }
                else
                {
                    fromDate = dto.Fromdate.ToString("yyyy-MM-dd");
                    ToDate = dto.Todate.ToString("yyyy-MM-dd");
                }
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Increment_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("HRME_Id", SqlDbType.VarChar)
                    {
                        Value = dto.selected_hrmeID
                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                    {
                        Value = fromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                    {
                        Value = ToDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar)
                    {
                        Value = dto.Month
                    });
                    cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar)
                    {
                        Value = dto.Year
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = dto.Type
                    });
                    cmd.Parameters.Add(new SqlParameter("@option", SqlDbType.VarChar)
                    {
                        Value = dto.Option
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader1.GetName(iFiled),
                                       dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.Reportdata = retObject.ToArray();
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
                dto.retrunMsg = "Error occured"; ;
            }
            return dto;
        }

        public EmployeeSalaryIncreementProcessDTO SaveUpdate(EmployeeSalaryIncreementProcessDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                // @MI_Id bigint, @HRME_Id bigint,@HRMED_Id bigint, @HREICED_Amount decimal, @HREICED_Percentage varchar(15),@Incrementdate date, @userid bigint
                for (int i = 0; i < dto.employee.Length; i++)
                {

                    var contactExistsP = _HRMSContext.Database.ExecuteSqlCommand("HR_Increement_Save @p0,@p1,@p2,@p3,@p4,@p5,@p6", dto.MI_Id, dto.employee[i].HRME_Id, dto.HRMED_Id, dto.HREICED_Amount, dto.HREICED_Percentage, DateTime.Now.ToString("yyyy-MM-dd"), dto.UserId);
                    if (contactExistsP > 0)
                    {
                        dto.retrunMsg = "Add";
                    }

                    else
                    {
                        dto.retrunMsg = "notUpdated";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public EmployeeSalaryIncreementProcessDTO editData(int id)
        {

            EmployeeSalaryIncreementProcessDTO dto = new EmployeeSalaryIncreementProcessDTO();
            dto.retrunMsg = "";
            try
            {
                var contactExistsP = _HRMSContext.Database.ExecuteSqlCommand("HR_Increement_Edit @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", dto.MI_Id, dto.HRME_Id, dto.HREIC_IncrementDate, dto.HREIC_ArrearApplicableFlg, dto.HREIC_ArrearGivenFlg, dto.HREIC_ArrearMonths, dto.HRMED_Id, dto.HREICED_Amount, dto.HREICED_Percentage, dto.Status);
                if (contactExistsP > 0)
                {
                    dto.retrunMsg = "updated";
                }
                else
                {
                    dto.retrunMsg = "notUpdated";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public EmployeeSalaryIncreementProcessDTO deactivate(EmployeeSalaryIncreementProcessDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                var contactExistsP = _HRMSContext.Database.ExecuteSqlCommand("HR_Increement_ActivDeactivate @p0", dto.HREIC_Id);
                if (contactExistsP > 0)
                {
                    dto.retrunMsg = "updated";
                }
                else
                {
                    dto.retrunMsg = "notUpdated";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public EmployeeSalaryIncreementProcessDTO GetAllDropdownAndDatatableDetails(EmployeeSalaryIncreementProcessDTO dto)
        {
            try
            {

                dto.employeelist = _HRMSContext.MasterEmployee.Where(m => m.MI_Id == dto.MI_Id).OrderBy(m => m.HRME_Id).ToArray();
                dto.earningdeductiontype = _HRMSContext.HR_Master_EarningsDeductions.Where(m => m.MI_Id == dto.MI_Id).OrderBy(m => m.HRMED_Id).ToArray();
                dto.monthdropdown = _HRMSContext.Month.ToArray();
                //  dto.griddata = _HRMSContext.HR_MasterLeaveYear.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();
                dto.leaveyeardropdown = _HRMSContext.HR_MasterLeaveYear.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "INCREMENTDETAILS";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });


                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)                                {                                    dataRow1.Add(                                        dataReader.GetName(iFiled1),                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow1);                            }                        }                        dto.griddata = retObject.ToArray();                    }                    catch (Exception ee)                    {                        Console.WriteLine(ee.Message);                    }                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public EmployeeSalaryIncreementProcessDTO Empdetails(EmployeeSalaryIncreementProcessDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "HR_EmployeeDetials";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.CommandTimeout = 0;
                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar) { Value = dto.HRME_Id });


                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)                                {                                    dataRow1.Add(                                        dataReader.GetName(iFiled1),                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow1);                            }                        }                        dto.getempsdetails = retObject.ToArray();                    }                    catch (Exception ee)                    {                        Console.WriteLine(ee.Message);                    }                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }



    }
}
