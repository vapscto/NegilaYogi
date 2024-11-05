using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class PFForm5Service:Interfaces.PFForm5Interface
    {

    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public PFForm5Service(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
    {
      _HRMSContext = HRMSContext;
      _Context = MsSqlServerContext;

    }
    public PFReportsDTO getBasicData(PFReportsDTO dto)
    {
      dto = GetAllDropdownAndDatatableDetails(dto);
      return dto;
    }

        public PFReportsDTO GetAllDropdownAndDatatableDetails(PFReportsDTO dto)
        {
            try
            {
                
                    //employee  
                  //  dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                    //leave year
                    dto.leaveyeardropdown = _HRMSContext.HR_MasterLeaveYear.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

                dto.monthdropdown = _Context.month.Where(t => t.Is_Active == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public PFReportsDTO FilterEmployeeData(PFReportsDTO dto)
    {
      //List<HR_Employee_Salary> employeeDetails = new List<HR_Employee_Salary>();
      //if (dto.FormatType.Equals("Format1"))
      //{
      //  if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
      //  {
      //    //employee
      //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();

      //  }
      //  else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
      //  {
      //    //employee
      //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id)).ToList();
      //  }
      //  else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
      //  {
      //    //employee
      //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
      //  }
      //  else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
      //  {
      //    //employee
      //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
      //  }
      //  else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() == 0)
      //  {
      //    //employee
      //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id)).ToList();
      //  }
      //  else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
      //  {
      //    //employee
      //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id)).ToList();
      //  }

      //  else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
      //  {
      //    //employee
      //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
      //  }

      //  if (dto.MonthBetweenDates.Equals("Month"))
      //  {
      //    employeeDetails = employeeDetails.Where(t => t.HRES_Month.Equals(dto.hreS_Month) && t.HRES_Year.Equals(dto.hreS_Year)).ToList();

      //  }
      //  else if (dto.MonthBetweenDates.Equals("BetweenDates"))
      //  {
      //    employeeDetails = employeeDetails.Where(t => t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate).ToList();
      //  }

      //  if (employeeDetails.Count() > 0)
      //  {
      //    var empIdList = employeeDetails.Select(t => t.HRME_Id);

      //    var employeedropdown = (from a in _HRMSContext.MasterEmployee
      //                            from b in _HRMSContext.HR_Employee_Salary
      //                            where a.HRME_Id.Equals(b.HRME_Id)
      //                            && b.MI_Id.Equals(dto.MI_Id)
      //                            && a.HRME_ActiveFlag == true && empIdList.Contains(a.HRME_Id)
      //                            orderby a.HRME_EmployeeOrder
      //                            select a).Distinct();
      //    dto.employeedropdown = employeedropdown.ToArray();
      //  }
      //}
      return dto;
    }

        public PFReportsDTO getEmployeedetailsBySelection(PFReportsDTO dto)
        {

            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            Institution InstitutionDetails = new Institution();
            List<HR_Employee_SalaryDTO> employeeSalaryDetailsDTO = new List<HR_Employee_SalaryDTO>();
            List<PFReportsDTO> alldata = new List<PFReportsDTO>();
            //List<PFReportsDTO> cumDTOList = new List<PFReportsDTO>();
            try
            {
                //get month id by month name
                long IVRM_Month_Id = _Context.month.Where(t => t.Is_Active == true && t.IVRM_Month_Name.Equals(dto.HRES_Month)).FirstOrDefault().IVRM_Month_Id;


                employeeDetails = _HRMSContext.MasterEmployee.Where(u => Convert.ToDateTime(u.HRME_PFDate).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_PFDate).Year == Convert.ToInt32(dto.HRES_Year) && u.HRME_ActiveFlag == true && u.MI_Id == dto.MI_Id).ToList();

                foreach (var employee in employeeDetails)
                {
                    PFReportsDTO ss = new PFReportsDTO();
                    string FatherHusbandName = "";

                    //Employee Details

                    employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == employee.HRME_Id).ToList();
                    var gender = _HRMSContext.IVRM_Master_Gender.Where(t => t.IVRMMG_Id.Equals(employee.IVRMMG_Id)).Select(t => t.IVRMMG_GenderName);

                    if (gender.FirstOrDefault().Equals("Female"))
                    {
                        var married = _HRMSContext.IVRM_Master_Marital_Status.Where(t => t.IVRMMMS_Id == employee.IVRMMMS_Id).Select(t => t.IVRMMMS_MaritalStatus);

                        if (married.FirstOrDefault().Equals("Married"))
                        {
                            FatherHusbandName = employee.HRME_SpouseName;
                        }
                        else
                        {
                            FatherHusbandName = employee.HRME_FatherName;
                        }
                    }
                    else
                    {
                        FatherHusbandName = employee.HRME_FatherName;
                    }

                    string TotalServicePeriod = "";

                    ss.HRME_PFAccNo = employee.HRME_PFAccNo;
                    ss.HRME_EmployeeFirstName = employee.HRME_EmployeeFirstName;
                    ss.HRME_EmployeeMiddleName = employee.HRME_EmployeeMiddleName;
                    ss.HRME_EmployeeLastName = employee.HRME_EmployeeLastName;

                    ss.FatherHusbandName = FatherHusbandName;

                    ss.HRME_DOB = employee.HRME_DOB;
                    ss.IVRMMG_GenderName = gender.FirstOrDefault();
                    ss.HRME_PFDate = employee.HRME_PFDate;
                    ss.TotalServicePeriod = TotalServicePeriod;

                   
                    alldata.Add(ss);

                }

                dto.pfreport = alldata.ToArray();

                // institutionDetails
                dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id == dto.MI_Id).ToArray();


                dto.PayrollStandard = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }


    }
}
