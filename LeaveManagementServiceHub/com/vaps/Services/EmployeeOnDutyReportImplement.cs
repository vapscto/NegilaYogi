using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class EmployeeOnDutyReportImplement : EmployeeOnDutyReportInterface
    {
        public LMContext _lmContext;
        public EmployeeOnDutyReportImplement(LMContext ttcategory)
        {
            _lmContext = ttcategory;
        }

        public EmployeeOnDutyReportDTO getalldetails(EmployeeOnDutyReportDTO dto)
        {
           
           dto.employeedropdown = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToArray();

            


            //dto.employeedropdown = (from a in _lmContext.HR_Master_Employee_DMO
            //                        where (a.HRME_ActiveFlag == true)
            //                        select new EmployeeOnDutyReportDTO
            //                        {
            //                            HRME_Id = a.HRME_Id,
            //                            HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
            //                        }).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToArray();
            return dto;

        }

        
        public EmployeeOnDutyReportDTO getEmployeedetailsBySelection(EmployeeOnDutyReportDTO dto)
        {


            try
            {
                string HRME_Id = "0";
                if (dto.employeelist != null && dto.employeelist.Length > 0)
                {
                    foreach (var d in dto.employeelist)
                    {

                        HRME_Id = HRME_Id + ',' + d.HRME_Id;
                    }
                  
                       
                }



                //dto.employeeDetails = (from a in _lmContext.HR_Emp_Leave_ApplicationDMO
                //                       from b in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                //                       from c in _lmContext.HR_Master_Leave_DMO
                //                       from d in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO
                //                       from e in _lmContext.HR_Master_Employee_DMO
                //                       where (a.HRELAP_Id == b.HRELAP_Id)

                //                       select new EmployeeOnDutyReportDTO
                //                       {
                //                           HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : " " + e.HRME_EmployeeFirstName)
                //                                  + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" || e.HRME_EmployeeMiddleName == "0" ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" || e.HRME_EmployeeLastName == "0" ? "" : " " + e.HRME_EmployeeLastName)).Trim(),
                //                           HRELAP_FromDate = a.HRELAP_FromDate,
                //                           HRELAP_ToDate = a.HRELAP_ToDate,
                //                           HRELAP_ApplicationStatus = a.HRELAP_ApplicationStatus,
                //                           HRELAPD_InTime = b.HRELAPD_InTime,
                //                           HRELAPD_OutTime = b.HRELAPD_OutTime,
                //                           HRML_LeaveName = c.HRML_LeaveName,
                //                           HRML_LeaveCode = c.HRML_LeaveCode,
                //                           HRELAPA_SanctioningLevel = d.HRELAPA_SanctioningLevel,

                //                       }).ToArray();

                var fromdate = dto.HRELAP_FromDate.Value.Date.ToString("yyyy-MM-dd");
                var todate = dto.HRELAP_ToDate.Value.Date.ToString("yyyy-MM-dd");

                //var d = data.FromDate.Value.Date.ToString("dd-MM-yyyy");





                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_MASTER_LEAVE_APPLICATION_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {

                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = todate
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] 
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.Empreport = retObject.ToArray();
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