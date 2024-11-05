using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;

using System.Reflection.Metadata;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.factories;
namespace HRMSServicesHub.com.vaps.Services
{
    public class EmployeeSalarySlipGenerationService : Interfaces.EmployeeSalarySlipGenerationInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeSalarySlipGenerationService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }
        public HR_Employee_SalaryDTO getBasicData(HR_Employee_SalaryDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }


        public HR_Employee_SalaryDTO GetAllDropdownAndDatatableDetails(HR_Employee_SalaryDTO dto)
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
                //employee

                employe = (from a in _HRMSContext.MasterEmployee
                           from b in _HRMSContext.HR_Employee_Salary
                           where (a.HRME_Id == b.HRME_Id && a.MI_Id.Equals(dto.MI_Id) && a.HRME_ActiveFlag == true)
                           select a).ToList();


                //  employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).ToList();
                dto.employeedropdown = employe.ToArray();

                //leave year
                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).OrderBy(t => t.HRMLY_LeaveYearOrder).ToList();
                dto.leaveyeardropdown = leaveyear.ToArray();

                PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                               from pa in _HRMSContext.HR_PROCESSDMO
                               from cc in _HRMSContext.Staff_User_Login
                               where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)
                               select pa).ToList();

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


                //dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();
                ////employee  
                ////employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) ).ToList();
                ////dto.employeedropdown = employe.ToArray();

                ////departmentdropdown
                //dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                ////designationdropdown 
                //dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HR_Employee_SalaryDTO GetEmployeeDetailsByLeaveYearAndMonth(HR_Employee_SalaryDTO dto)
        {
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            try
            {
                //employee

                //employee list
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

                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeeDetails = employe.ToArray();
                    dto.employeedropdown = employe.ToArray();

                    //  dto.employeedropdown = employeeDetails.ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdownlist = GroupTypelist.ToArray();

                    ////Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdownlist = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdownlist = Designationlist.ToArray();

                }
                else
                {

                    employe = (from a in _HRMSContext.MasterEmployee
                               from b in _HRMSContext.HR_Employee_Salary
                               where (b.HRME_Id == a.HRME_Id && b.MI_Id.Equals(dto.MI_Id))
                               && b.HRES_Year.Equals(dto.HRES_Year) && b.HRES_Month.Equals(dto.HRES_Month) && a.HRME_ActiveFlag == true
                               select a).Distinct().ToList();
                    if (employe.Count > 0)
                    {
                        // employe = employe.Where(a => a.HRME_LeftFlag == false && Convert.ToDateTime(a.HRME_DOJ) <= Convert.ToDateTime(selecteddate)).ToList();


                        if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();

                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                        }

                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }

                        if (Convert.ToInt32(dto.HRES_Year) > 0 && dto.HRES_Month != "")
                        {
                            //get month id by month name
                            var Month = _Context.month.Where(t => t.Is_Active == true && t.IVRM_Month_Name.Equals(dto.HRES_Month)).ToList();
                            var config = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).FirstOrDefault();
                            int IVRM_Month_Id = 0;
                            if (Month.Count > 0)
                            {
                                if (config.HRC_SalaryFromDay > 1 && Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id) < 12)
                                {

                                    IVRM_Month_Id = Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id) + 1;
                                }
                                else if (config.HRC_SalaryFromDay > 1 && Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id) == 12)
                                {
                                    IVRM_Month_Id = 01;
                                    dto.HRES_Year = (Convert.ToInt32(dto.HRES_Year) + 1).ToString();
                                }
                                else
                                {
                                    IVRM_Month_Id = Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id);
                                    var days = DateTime.DaysInMonth(Convert.ToInt32(dto.HRES_Year), IVRM_Month_Id);

                                    config.HRC_SalaryToDay = days;
                                }

                                //employee list
                                DateTime selectedFromdate = new DateTime(Convert.ToInt32(dto.HRES_Year), Convert.ToInt32(IVRM_Month_Id), config.HRC_SalaryFromDay, 0, 0, 0, 0);

                                // string selectedTodate = "" + config.HRC_SalaryToDay + "-" + IVRM_Month_Id + "-" + Convert.ToInt32(dto.HRES_Year) + "";
                                DateTime selectedTodate = new DateTime(Convert.ToInt32(dto.HRES_Year), Convert.ToInt32(IVRM_Month_Id), config.HRC_SalaryToDay, 0, 0, 0, 0);


                                var leftEmp = employe.Where(t => t.HRME_LeftFlag == true && Convert.ToDateTime(t.HRME_DOL) < Convert.ToDateTime(selectedFromdate)).Select(t => t.HRME_Id);
                                if (leftEmp.Count() > 0)
                                {
                                    employe = employe.Where(t => Convert.ToDateTime(t.HRME_DOJ) <= Convert.ToDateTime(selectedTodate) && !leftEmp.Contains(t.HRME_Id) == true).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                                }
                                else
                                {
                                    employe = employe.Where(t => Convert.ToDateTime(t.HRME_DOJ) <= Convert.ToDateTime(selectedTodate) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                                }

                            }

                        }

                    }
                }
                dto.employeedropdown = employe.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }



        public async Task<HR_Employee_SalaryDTO> GenerateEmployeeSalarySlip(HR_Employee_SalaryDTO dto)
        {
            try
            {
                List<HR_Employee_Salary> employeeSalaryDetails = (from a in _HRMSContext.HR_Employee_Salary
                                                                  from b in _HRMSContext.MasterEmployee
                                                                  where (a.HRME_Id == b.HRME_Id && a.HRES_Month.Equals(dto.HRES_Month)
                                                                  && a.HRES_Year.Equals(dto.HRES_Year) && a.MI_Id.Equals(dto.MI_Id)
                                                                  && dto.hrmdeS_IdList.Contains(a.HRMDES_Id) 
                                                                  && dto.hrmD_IdList.Contains(a.HRMD_Id) 
                                                                  && dto.groupTypeIdList.Contains(a.HRMGT_Id) 
                                                                  && b.HRME_PFApplicableFlag == true)
                                                                  select a).ToList();
                List<HR_Employee_SalaryDTO> main_list = new List<HR_Employee_SalaryDTO>();
                for (int i = 0; i < dto.empid.Count(); i++)
                {
                    HR_Employee_SalaryDTO obj = new HR_Employee_SalaryDTO();
                    obj.HRME_Id = dto.empid[i];
                    obj.MI_Id = dto.MI_Id;
                    obj.HRES_Year = dto.HRES_Year;
                    obj.HRES_Month = dto.HRES_Month;
                    obj.abc =employeeSalaryDetails[0].HRES_WorkingDays;

                    Institution institute = new Institution();
                    institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                    InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                    obj.institutionDetails = dmoObj;

                    MasterEmployee employe = _HRMSContext.MasterEmployee.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id.Equals(obj.HRME_Id));


                    var DepartmentName = (from a in _HRMSContext.HR_Employee_Salary
                                          from b in _HRMSContext.HR_Master_Department
                                          where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && a.HRES_Year == dto.HRES_Year && a.HRES_Month == dto.HRES_Month && a.HRME_Id.Equals(obj.HRME_Id))
                                          select (b.HRMD_DepartmentName)).FirstOrDefault();
                    var DesignationName = (from a in _HRMSContext.HR_Employee_Salary
                                           from b in _HRMSContext.HR_Master_Designation
                                           where (a.HRMDES_Id == b.HRMDES_Id && a.MI_Id == b.MI_Id && a.HRES_Year == dto.HRES_Year && a.HRES_Month == dto.HRES_Month && a.HRME_Id.Equals(obj.HRME_Id))
                                           select (b.HRMDES_DesignationName)).FirstOrDefault();

                    var GenderName = _HRMSContext.IVRM_Master_Gender.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.IVRMMG_Id.Equals(employe.IVRMMG_Id)).IVRMMG_GenderName;


                    //    var DepartmentName = _HRMSContext.HR_Master_Department.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_Id.Equals(employe.HRMD_Id)).HRMD_DepartmentName;
                    //var DesignationName = _HRMSContext.HR_Master_Designation.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_Id.Equals(employe.HRMDES_Id)).HRMDES_DesignationName;

                    //Employee Basic Details
                    MasterEmployeeDTO employeObj = Mapper.Map<MasterEmployeeDTO>(employe);
                    obj.currentemployeeDetails = employeObj;
                    obj.DesignationName = DesignationName;
                    obj.DepartmentName = DepartmentName;
                    obj.GenderName = GenderName;
                    // obj.HRME_Age = DateTime.Now.Year - employeObj.HRME_DOB.Value.Date.Year;

                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Employee_Age_Calculation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                        {
                            Value = dto.HRES_Year
                        });
                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar)
                        {
                            Value = dto.HRES_Month
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.VarChar)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                        {
                            Value = obj.HRME_Id
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
                                    //retObject.Add((ExpandoObject)dataRow);



                                    obj.HRME_Age = Convert.ToInt64(dataReader["Age"]);
                                    obj.HRES_WorkingDays = Convert.ToInt64(dataReader["HRES_WorkingDays"]);


                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //DateTime dt1 = DateTime.Parse(employeObj.HRME_DOB.ToString());
                    //DateTime dt2 = DateTime.Parse("01/" + obj.HRES_Month + "/" + dto.HRES_Year);
                    //obj.HRME_Age = (dt2 - dt1).Days / 365;
                    obj.HRME_EmployeeOrder = employeObj.HRME_EmployeeOrder;

                    //Configuration details
                    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));
                    HR_ConfigurationDTO HR_ConfigurationDTO = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                    obj.PayrollStandard = HR_ConfigurationDTO;

                    //Employee Earning /Deduction heads
                    obj = await getEmployeeSalarySlip(obj);

                    decimal Lopdays = 0;
                    decimal LopAmount = 0;
                    //LOP Calculation

                    var employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && t.HRME_Id == obj.HRME_Id).FirstOrDefault();
                    if (employeSalary != null)
                    {
                        HR_Employee_SalaryDTO employeSalObj = Mapper.Map<HR_Employee_SalaryDTO>(employeSalary);

                        obj.empsaldetail = employeSalObj;

                        var LOPcal = (from A in _HRMSContext.HR_Emp_Leave_Trans
                                      from B in _HRMSContext.HR_Master_Leave
                                      from C in _HRMSContext.HR_Emp_Leave_Trans_Details
                                      where (B.HRML_Id == A.HRELT_LeaveId &&
                                      A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == obj.HRME_Id &&
                                     A.HRELT_ActiveFlag == true && C.HRELT_Id == A.HRELT_Id && C.HRELTD_LWPFlag == true
                                    && ((A.HRELT_FromDate >= employeSalary.HRES_FromDate && A.HRELT_FromDate <= employeSalary.HRES_ToDate)
                                        || (A.HRELT_ToDate >= employeSalary.HRES_FromDate && A.HRELT_ToDate <= employeSalary.HRES_ToDate))
                                      )
                                      select A
                                   ).ToList();
                        if (LOPcal.Count() > 0)
                        {
                            Lopdays = LOPcal.Sum(t => t.HRELT_TotDays);
                            LopAmount = Convert.ToDecimal(Lopdays) * Convert.ToDecimal(employeSalary.HRES_DailyRates);
                        }

                        obj.empsaldetail.Lopdays = Lopdays;
                        obj.empsaldetail.LopAmount = LopAmount;

                        //Leave Details

                        var LeayearId = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_LeaveYear.Equals(dto.HRES_Year)).FirstOrDefault().HRMLY_Id;

                        if (LeayearId > 0)
                        {
                            //var LeaveDetails = (from A in _HRMSContext.HR_Emp_Leave_StatusDMO
                            //                    from B in _HRMSContext.HR_Master_Leave
                            //                    from C in _HRMSContext.HR_Emp_Leave_Trans
                            //                    where (B.HRML_Id == A.HRML_Id &&
                            //                    A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == obj.HRME_Id &&
                            //                    A.HRMLY_Id == LeayearId && C.MI_Id.Equals(dto.MI_Id) && C.HRME_Id == obj.HRME_Id && C.HRMLY_Id == LeayearId && C.HRELT_FromDate >= employeSalary.HRES_FromDate && C.HRELT_FromDate <= employeSalary.HRES_ToDate && B.HRML_Id == C.HRELT_LeaveId)
                            //                    select new HR_Emp_Leave_StatusDTO
                            //                    {
                            //                        HRELS_Id = A.HRELS_Id,
                            //                        HRML_LeaveName = B.HRML_LeaveName,
                            //                        HRELS_TotalLeaves = A.HRELS_TotalLeaves,
                            //                        HRELS_TransLeaves = C.HRELT_TotDays,
                            //                        HRELS_CBLeaves = A.HRELS_CBLeaves

                            //                    }).ToList();


                            using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "LeaveDetailsForsalaryslip";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.BigInt)
                                {
                                    Value = LeayearId
                                });
                                cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt)
                                {
                                    Value = dto.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                                {
                                    Value = obj.HRME_Id
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
                                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                                );
                                            }

                                            retObject.Add((ExpandoObject)dataRow);
                                        }
                                    }
                                    obj.employeeLeaveDetails = retObject.ToArray();


                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                        }

                    }
                    main_list.Add(obj);
                    ////---To Delete The Generated File Start ---

                    ///////////
                    //////var Empdetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == obj.HRME_Id && t.HRME_ActiveFlag == true).ToList();
                    //////string slaryslip = "Salary Slip Of Employee " + Empdetails.FirstOrDefault().HRME_EmployeeCode + ".pdf";
                    //////string GetDownloadFolderPath =
                    //////      Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();

                    //////IFileProvider provider = new PhysicalFileProvider(GetDownloadFolderPath);
                    //////IFileInfo fileInfo = provider.GetFileInfo(slaryslip);

                    //////bool fileexists = File.Exists(fileInfo.PhysicalPath);
                    //////if (fileexists == true)
                    //////{
                    //////    File.Delete(fileInfo.PhysicalPath);
                    //////}
                    //---To Delete The Generated File Start --- 
                    ///////////////////////////

                }
                dto.main_list = main_list.OrderBy(t => t.HRME_EmployeeOrder).ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }

            return dto;
        }


        public async Task<HR_Employee_SalaryDTO> getEmployeeSalarySlip(HR_Employee_SalaryDTO dto)
        {

            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmployeeSalarySlipGeneration";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_LeaveYear",
                  SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRES_Year)
                    });

                    cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Name",
                  SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(dto.HRES_Month)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.employeeSalaryslipDetails = retObject.OrderBy(t => t.HRMED_Order).ToArray();


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


            return dto;
        }


        public static string NumberToWords(long number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public async Task<HR_Employee_SalaryDTO> SendEmailSMS(HR_Employee_SalaryDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                for (int i = 0; i < dto.empid.Count(); i++)
                {
                    HR_Employee_SalaryDTO obj = new HR_Employee_SalaryDTO();
                    string EmployeeName = "";
                    obj.HRME_Id = dto.empid[i];

                    EmployeeName = (from a in _HRMSContext.MasterEmployee
                                    where a.MI_Id == dto.MI_Id && a.HRME_Id == obj.HRME_Id
                                    select new MasterEmployeeDTO
                                    {
                                        HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)
                                    }).FirstOrDefault().HRME_EmployeeFirstName;

                    var Empcode = _HRMSContext.MasterEmployee.Where(e => e.MI_Id == dto.MI_Id && e.HRME_Id == obj.HRME_Id).Select(m => m.HRME_EmployeeCode).FirstOrDefault();
                    var employeedetails = _HRMSContext.MasterEmployee.Single(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id == obj.HRME_Id);

                    for (int j = 0; j < dto.Template.Count(); j++)
                    {
                        if (dto.Template[j].hrmE_EmployeeCode == Empcode)
                        {
                            if (dto.MonthList != null) { dto.HRES_Month = dto.MonthList[j]; }
                            if (dto.EmailSMS == "Email")
                            {
                                Email Email = new Email(_Context);


                                var email_list = _HRMSContext.Emp_Email_Id.Where(t => t.HRME_Id == obj.HRME_Id && t.HRMEM_DeFaultFlag.Equals("default")).ToList();
                                var Empdetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == obj.HRME_Id && t.HRME_ActiveFlag == true).ToList();
                                string slaryslip = "Salary Slip Of Employee " + Empdetails.FirstOrDefault().HRME_EmployeeCode + ".pdf";

                                if (email_list.Count > 0)
                                {
                                    string Subject = "Salary slip";
                                    //string m = Email.sendmailWithoutTemplate(dto.MI_Id, email_list.FirstOrDefault().HRMEM_EmailId, dto.Template, Subject);
                                    string m = sendmailforslaryslip(dto.MI_Id, email_list.FirstOrDefault().HRMEM_EmailId, dto.Template[j].TemplateString, Subject, slaryslip, EmployeeName, dto.HRES_Month);

                                    dto.retrunMsg = m;
                                    Console.WriteLine(m);
                                }
                                else
                                {
                                    dto.retrunMsg = "notFound";
                                }
                            }
                            else if (dto.EmailSMS == "SMS")
                            {

                                var mobile_list = _HRMSContext.Emp_MobileNo.Where(t => t.HRME_Id == obj.HRME_Id && t.HRMEMNO_DeFaultFlag.Equals("default")).ToList();

                                if (mobile_list.Count > 0)
                                {
                                    SMS sms = new SMS(_Context);

                                    //string smsdet = await sms.sendSms(dto.MI_Id, Convert.ToInt64(mobile_list.FirstOrDefault().HRMEMNO_MobileNo), "EMPLOYEE_SALARY_SLIP", obj.HRME_Id);
                                    string smsdet = await sendSmsBdBoy(dto.MI_Id, Convert.ToInt64(mobile_list.FirstOrDefault().HRMEMNO_MobileNo), "EMPLOYEE_SALARY_SLIP", obj.HRME_Id, EmployeeName, dto.HRES_Month);
                                    dto.retrunMsg = smsdet;
                                    Console.WriteLine(smsdet);
                                }
                                else
                                {
                                    dto.retrunMsg = "notFound";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }
            return dto;
        }

        public string sendmailforslaryslip(long MI_Id, string Email, string Template, string Subject, string slaryslip, string EmployeeName, string monthname)
        {

            List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
            try
            {
                //string Mailmsg = "Dear Sir/Madam,Please find enclosed PDF attachment. This is auto generated email Don't replay to this email.Thanking You. ";
                string Mailmsg = getMessage(MI_Id, Template, EmployeeName, monthname);
                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)

                {

                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);

                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")

                    {
                        mailcc = alldetails[0].IVRM_mailcc.ToString();
                    }

                    SendGridMessage message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);
                    if (mailcc != null && mailcc != "")

                    {
                        message.AddCc(mailcc);

                    }

                    //  string GetDownloadFolderPath =
                    //  Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();

                    //string GetDownloadFolderPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
                    //new code//

                    StringBuilder sb = new StringBuilder(Template);
                    //sb.Append("<template>");
                    //sb.Append("<header class='clearfix'>");
                    //sb.Append("<h1>INVOICE</h1>");
                    //sb.Append("<div id='company' class='clearfix'>");


                    //sb.Append("</main>");
                    //sb.Append("<footer>");
                    //sb.Append("Invoice was created on a computer and is valid without the signature and seal.");
                    //sb.Append("</footer>");


                    StringReader sr = new StringReader(sb.ToString());

                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        pdfDoc.Open();

                        htmlparser.Parse(sr);
                        pdfDoc.Close();

                        byte[] bytess = memoryStream.ToArray();

                        memoryStream.Close();

                        var file = Convert.ToBase64String(bytess);
                        string emp;
                        emp = Convert.ToString(sr);
                        string c = "";
                        string v = emp.Replace("System.IO.StringReader", "SalarySlip.Pdf");
                        message.AddAttachment(v, file);// + ".pdf");
                        //message.AddContent(type:"text/pdfDoc", file);
                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())

                    {

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)


                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "HRMS"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return "Error";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
                //  return "Error";
            }
            return "success";
        }
        public string getMessage(long MI_Id, string TemplateName, string EmployeeName, string monthname)
        {
            string strMessage = "";
            string strEmpname = "";
            var smsemaildata = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "EMPLOYEE_SALARY_SLIP").FirstOrDefault();
            if (smsemaildata != null)
            {
                strMessage = smsemaildata.ISES_MailBody + " " + smsemaildata.ISES_MailFooter;
                strMessage = strMessage.Replace("[USR]", EmployeeName);
                strMessage = strMessage.Replace("[MONTH]", monthname);

                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(strMessage, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
            }
            else
            {
                strMessage = "Dear Sir/Madam,Please find enclosed PDF attachment. This is auto generated email Don't replay to this email.Thanking You.";
            }
            return strMessage;
        }

        public async Task<string> sendSmsBdBoy(long MI_Id, long mobileNo, string Template, long UserID, string Empname, string MonthName)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();


                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "EMPLOYEE_SALARY_SLIP" && e.ISES_SMSActiveFlag == true).ToList();



                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;
                if (sms != "")
                {
                    sms = sms.Replace("[USR]", Empname);
                    sms = sms.Replace("[MONTH]", MonthName);
                }

                string result = sms;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                        );
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _Context.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                    //try
                    //{
                    //    var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                    //    var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                    //    var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                    //    IVRM_sms_sentBoxDMO smssentbox = new IVRM_sms_sentBoxDMO();

                    //    smssentbox.MI_Id = MI_Id;
                    //    smssentbox.Mobile_no = PHNO;
                    //    smssentbox.Message = sms;
                    //    smssentbox.Datetime = DateTime.Now;

                    //    smssentbox.Message_id = messageid;
                    //    smssentbox.Module_Name = modulename[0];
                    //    smssentbox.CreatedDate = DateTime.Now;
                    //    smssentbox.UpdatedDate = DateTime.Now;

                    //    smssentbox.statusofmessage = "Deliverd";
                    //    smssentbox.To_FLag = "Student";

                    //    _db.IVRM_sms_sentBoxDMO.Add(smssentbox);
                    //    var returnmessage=_db.SaveChanges();

                    //}
                    //catch(Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public HR_Employee_SalaryDTO get_depts(HR_Employee_SalaryDTO data)
        {
            try
            {
                data.departmentdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HR_Employee_SalaryDTO get_desig(HR_Employee_SalaryDTO data)
        {
            try
            {
                data.designationdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
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
