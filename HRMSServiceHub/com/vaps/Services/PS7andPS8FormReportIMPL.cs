using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class PS7andPS8FormReportIMPL : Interfaces.PS7andPS8FormReportInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public PS7andPS8FormReportIMPL(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public PFReportsDTO getBasicData(PFReportsDTO dto)
        {
            CultureInfo us = new CultureInfo("en-US");
            var startDate = DateTime.ParseExact("04/01/2017", "MM/dd/yyyy", us);
            var endDate = DateTime.ParseExact("03/31/2018", "MM/dd/yyyy", us);



            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public static IEnumerable<Tuple<string, int>> MonthsBetween(
           DateTime startDate,
           DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return Tuple.Create(
                    dateTimeFormat.GetMonthName(iterator.Month),
                    iterator.Year);
                iterator = iterator.AddMonths(1);
            }
        }

        public PFReportsDTO GetAllDropdownAndDatatableDetails(PFReportsDTO dto)
        {
            
            try
            {



                dto.employeedropdown = (from a in _HRMSContext.MasterEmployee
                                        from b in _HRMSContext.HR_Employee_Salary
                                        where (a.HRME_Id == b.HRME_Id && a.MI_Id.Equals(dto.MI_Id) && a.HRME_ActiveFlag == true)
                                        select new PFReportsDTO
                                        {
                                            HRME_Id=a.HRME_Id,
                                            HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                                  + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                        }).Distinct().ToArray();


              dto.leaveyeardropdown = _HRMSContext.HR_MasterLeaveYear.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

                dto.monthdropdown = _Context.month.Where(t => t.Is_Active == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

      

        public PFReportsDTO getEmployeedetailsBySelection(PFReportsDTO dto)
        {

            
            try
            {

                List<long> HRMEIDList = new List<long>();                string HRME_Id = "0";                if (dto.employee != null && dto.employee.Length > 0)                {                    foreach (var d in dto.employee)                    {                        HRME_Id = HRME_Id + ',' + d.HRME_Id;                        HRMEIDList.Add(d.HRME_Id);                    }                }

                dto.employeeDetails = (from a in _HRMSContext.MasterEmployee
                                       from b in _HRMSContext.Institution
                                       where (a.MI_Id == b.MI_Id && HRMEIDList.Contains(a.HRME_Id))
                                       select new PFReportsDTO
                                       {
                                           HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                                  + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                           HRME_FatherName=a.HRME_FatherName,
                                           MI_Name = b.MI_Name,
                                           MI_Address1 = b.MI_Address1,
                                           HRME_PFAccNo = a.HRME_PFAccNo,
                                           HRME_DOL = a.HRME_DOL,
                                           HRME_PensionStoppedDate = a.HRME_PensionStoppedDate,
                                           HRME_Id=a.HRME_Id
                                       }).ToArray();

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_7ps_Report";
                    cmd.CommandTimeout = 900000000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.BigInt)
                    {
                        Value = dto.HRES_Year
                    });

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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.pfreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                dto.retrunMsg = "Error";
            }
            return dto;
        }
        public PFReportsDTO getdataps8(PFReportsDTO dto)
        {
            try
            {

                dto.employeeDetails = (from a in _HRMSContext.MasterEmployee
                                       from b in _HRMSContext.Institution
                                       where (a.MI_Id == b.MI_Id && a.HRME_Id == dto.HRME_Id)
                                       select new PFReportsDTO
                                       {
                                           HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                                  + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                           HRME_FatherName=a.HRME_FatherName,
                                           MI_Name = b.MI_Name,
                                           MI_Address1 = b.MI_Address1,
                                       }).ToArray();
                var typeflag = "";

                if (dto.Flag=="MonthWise")
                {
                    typeflag = "MonthWise";
                }
                else
                {
                    typeflag = "yearly";
                }
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_8ps_Report";
                    cmd.CommandTimeout = 900000000;
                    cmd.CommandType = CommandType.StoredProcedure;                   
                    cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.BigInt)
                    {
                        Value = dto.HRES_Year
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = typeflag
                    });

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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.pfreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                dto.retrunMsg = "Error";
            }
            return dto;
        }

    }
}
