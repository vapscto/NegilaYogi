using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
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
    public class EmployeeLoanReportService : Interfaces.SalaryloanInterfacereport
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeLoanReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public LoanReportDTO getBasicData(LoanReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }


        public LoanReportDTO GetAllDropdownAndDatatableDetails(LoanReportDTO dto)
        {
            List<HR_Employee_Salary> SalaryCalculation = new List<HR_Employee_Salary>();
            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();

            List<Month> Monthlist = new List<Month>();

            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            try
            {

                //leave year
                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();

                ////leave year
                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();



                PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                               from pa in _HRMSContext.HR_PROCESSDMO
                               from cc in _HRMSContext.Staff_User_Login
                               where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)
                               select pa
                         ).ToList();

                if (PROCESSList.Count() > 0)
                {

                    List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                    List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                    List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();


                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();

                }
                else
                {


                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();

                }

                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                dto.configurationDetails = dmoObj;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }


        public async Task<LoanReportDTO> getEmployeedetailsBySelection(LoanReportDTO dto)
        {
            try
            {

                //  Inatitution Details

                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;

                string HRMDES_Id = "0";                if (dto.hrmdeS_IdList != null && dto.hrmdeS_IdList.Length > 0)                {                    foreach (var d in dto.hrmdeS_IdList)                    {                        HRMDES_Id = HRMDES_Id + ',' + d;                    }                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Emploan_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@hrmeid", SqlDbType.VarChar)
                    {
                        Value = dto.hrmE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.VarChar)
                    {
                        Value = dto.HRELT_Month
                    });
                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.VarChar)
                    {
                        Value = dto.HRELT_Year
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar)
                    {
                        Value = HRMDES_Id
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
                        dto.employeeSalaryslipDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                //if (dto.hrmE_Id != 0)
                //{
                //    var empHeads = (from a in _HRMSContext.MasterEmployee
                //                    from b in _HRMSContext.HR_Master_Department
                //                    from c in _HRMSContext.HR_Emp_Loan
                //                    from d in _HRMSContext.HR_Emp_Loan_Transaction
                //                    from e in _HRMSContext.HRMasterLoan
                //                    from f in _HRMSContext.Month
                //                    where (a.HRME_Id == c.HRME_Id && a.MI_Id == dto.MI_Id && a.HRME_Id == c.HRME_Id && a.HRMD_Id == b.HRMD_Id && c.HREL_Id == d.HREL_Id && c.HRME_Id == d.HRME_Id && c.HRMLN_Id == e.HRMLN_Id && c.HREL_Id == d.HREL_Id && a.HRME_Id == dto.hrmE_Id && f.IVRM_Month_Name == d.HRELT_Month)
                //                    select new LoanReportDTO
                //                    {
                //                        hrmE_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                //                        hrme_employeecode = a.HRME_EmployeeCode,
                //                        HREL_LoanAmount = c.HREL_LoanAmount,
                //                        HREL_LoanInsallments = c.HREL_LoanInsallments,
                //                        HRELT_LoanAmount = d.HRELT_LoanAmount,
                //                        HRELT_Month = d.HRELT_Month,
                //                        HRELT_Year = d.HRELT_Year,
                //                        HRML_LoanType = e.HRML_LoanType,
                //                        HREL_TotalPending = c.HREL_TotalPending,
                //                        IVRM_Month_Id = f.IVRM_Month_Id
                //                    }).Distinct().OrderBy(t=> t.HRELT_Year).ThenBy(t=>t.IVRM_Month_Id).ToList();
                //    dto.employeeSalaryslipDetails = empHeads.ToArray();
                //}
                //else
                //{
                //    var empHeads = (from a in _HRMSContext.MasterEmployee
                //                    from b in _HRMSContext.HR_Master_Department
                //                    from c in _HRMSContext.HR_Emp_Loan
                //                    from d in _HRMSContext.HR_Emp_Loan_Transaction
                //                    from e in _HRMSContext.HRMasterLoan
                //                    where (dto.hrmdeS_IdList.Contains(a.HRMDES_Id)  && dto.hrmD_IdList.Contains(a.HRMD_Id) && a.HRME_Id == c.HRME_Id && a.MI_Id == dto.MI_Id && a.HRME_Id == c.HRME_Id && a.HRMD_Id == b.HRMD_Id && c.HREL_Id == d.HREL_Id && c.HRME_Id == d.HRME_Id && c.HRMLN_Id == e.HRMLN_Id && c.HREL_Id == d.HREL_Id && d.HRELT_Month == dto.HRELT_Month && d.HRELT_Year == dto.HRELT_Year)
                //                    select new LoanReportDTO
                //                    {
                //                        hrmE_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                //                        hrme_employeecode = a.HRME_EmployeeCode,
                //                        HREL_LoanAmount = c.HREL_LoanAmount,
                //                        HREL_LoanInsallments = c.HREL_LoanInsallments,
                //                        HRELT_LoanAmount = d.HRELT_LoanAmount,
                //                        HRELT_Month = d.HRELT_Month,
                //                        HRELT_Year = d.HRELT_Year,
                //                        HRML_LoanType = e.HRML_LoanType,
                //                        HREL_TotalPending = c.HREL_TotalPending,
                //                    }).Distinct().ToList();
                //    dto.employeeSalaryslipDetails = empHeads.ToArray();
                //}

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }



        public LoanReportDTO get_depts(LoanReportDTO data)
        {
            try
            {
                data.departmentdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id  && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public LoanReportDTO get_desig(LoanReportDTO data)
        {
            try
            {
                data.designationdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id  && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}